import com.google.gson.Gson;
import io.github.resilience4j.core.IntervalFunction;
import io.github.resilience4j.retry.Retry;
import io.github.resilience4j.retry.RetryConfig;
import io.github.resilience4j.retry.RetryRegistry;
import io.vavr.CheckedFunction0;
import it.fattureincloud.sdk.ApiClient;
import it.fattureincloud.sdk.ApiException;
import it.fattureincloud.sdk.Configuration;
import it.fattureincloud.sdk.api.ProductsApi;
import it.fattureincloud.sdk.auth.OAuth;
import it.fattureincloud.sdk.model.ListProductsResponse;
import it.fattureincloud.sdk.model.Product;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Paths;
import java.nio.file.StandardOpenOption;
import java.util.List;

public class Application {
    public static void main(String[] args) throws Throwable {
        // This code should be executed periodically using a cron library or job scheduler.
        // For example: http://www.quartz-scheduler.org/
        syncProducts();
    }

    static void syncProducts() throws Throwable {

        // Here we init the Fatture in Cloud SDK
        ApiClient defaultClient = Configuration.getDefaultApiClient();

        // The Access Token is retrieved using the "getToken" method
        OAuth OAuth2AuthenticationCodeFlow =
                (OAuth) defaultClient.getAuthentication("OAuth2AuthenticationCodeFlow");
        OAuth2AuthenticationCodeFlow.setAccessToken(getToken());

        // In this example we're using the Products API
        ProductsApi apiInstance = new ProductsApi(defaultClient);

        // The ID of the controlled company.
        int companyId = 2;

        // Here we setup the exponential backoff config
        RetryConfig config = RetryConfig.custom()
                .maxAttempts(10)
                .retryExceptions(ApiException.class)
                .intervalFunction(IntervalFunction.ofExponentialBackoff(1000, 2))
                .build();

        RetryRegistry registry = RetryRegistry.of(config);
        Retry retry = registry.retry("listProducts", config);

        Retry.EventPublisher publisher = retry.getEventPublisher();
        publisher.onRetry(event -> System.out.println(String.format("Attempt: %d, WaitTime(millis): %d", event.getNumberOfRetryAttempts(), (int)Math.pow(2, event.getNumberOfRetryAttempts()) * 1000)));

        // In this example we suppose to export the data to a JSON Lines file.
        // First, we cancel the content of the destination file
        Files.write(Paths.get("products.jsonl"), ("").getBytes(),
                StandardOpenOption.CREATE, StandardOpenOption.TRUNCATE_EXISTING);

        // List Products
        int perPage = 5;

        // We perform the first request
        ListProductsResponse result = listProductsWithBackoff(companyId, 1, perPage, retry, apiInstance);

        // We use the first response to extract the last page index
        int lastPage = result.getLastPage();

        // We append the products obtained with the first request top the output file
        // Data contains an array of products
        appendProductsToFile(result.getData());

        // For the missing pages (we already requested the first one)
        for (int i = 2; i <= lastPage; i++)
        {
            // We require the page to the API
            result = listProductsWithBackoff(companyId, i, perPage, retry, apiInstance);
            // And append all the retrieved products
            appendProductsToFile(result.getData());

        }
    }

    // Here we wrap the SDK method with an exponential backoff
    // This is to manage the quota exceeded issue
    static ListProductsResponse listProductsWithBackoff(int companyId, int currentPage, int perPage,
                                                        Retry retry, ProductsApi apiInstance) throws Throwable {
        System.out.println(String.format("Page: %d", currentPage));
        CheckedFunction0<ListProductsResponse> retryingListSuppliers =
                Retry.decorateCheckedSupplier(retry,
                        () -> apiInstance.listProducts(companyId, null, "detailed", null, currentPage, 5, null));
        return retryingListSuppliers.apply();
    }

    static void appendProductsToFile(List<Product> products) throws IOException {
        for (Product product : products) {
            String p = new Gson().toJson(product);
            Files.write(Paths.get("products.jsonl"), (p + System.lineSeparator()).getBytes(),
                    StandardOpenOption.CREATE, StandardOpenOption.APPEND);
        }
    }

    // This is just a mock: this function should contain the code to retrieve the Access Token
    static String getToken() {
        return "YOUR_ACCESS_TOKEN";
    }
}