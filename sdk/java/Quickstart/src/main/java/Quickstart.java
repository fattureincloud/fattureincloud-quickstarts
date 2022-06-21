import it.fattureincloud.sdk.ApiClient;
import it.fattureincloud.sdk.ApiException;
import it.fattureincloud.sdk.Configuration;
import it.fattureincloud.sdk.api.SuppliersApi;
import it.fattureincloud.sdk.api.UserApi;
import it.fattureincloud.sdk.auth.OAuth;
import it.fattureincloud.sdk.model.ListSuppliersResponse;
import it.fattureincloud.sdk.model.ListUserCompaniesResponse;

public class Quickstart {
    public static String getFirstCompanySuppliers(String token) {
        ApiClient defaultClient = Configuration.getDefaultApiClient();

        // Configure OAuth2 access token for authorization: OAuth2AuthenticationCodeFlow
        OAuth OAuth2AuthenticationCodeFlow = (OAuth) defaultClient.getAuthentication("OAuth2AuthenticationCodeFlow");
        OAuth2AuthenticationCodeFlow.setAccessToken(token);

        UserApi userApiInstance = new UserApi(defaultClient);
        SuppliersApi suppliersApiInstance = new SuppliersApi(defaultClient);

        try {
            // Retrieve the first company id
            ListUserCompaniesResponse userCompanies = userApiInstance.listUserCompanies();
            int firstCompanyId = userCompanies.getData().getCompanies().get(0).getId();

            // Retrieve the list of first 10 Suppliers for the selected company
            Integer companyId = 12345; // Integer | The ID of the company.
            Integer page = 1; // Integer | The page to retrieve.
            Integer perPage = 10; // Integer | The size of the page.

            ListSuppliersResponse result = suppliersApiInstance.listSuppliers(companyId, null, null, null, page, perPage, null);
            return result.getData().toString();

        } catch (ApiException e) {
            System.err.println("Status code: " + e.getCode());
            System.err.println("Reason: " + e.getResponseBody());
            System.err.println("Response headers: " + e.getResponseHeaders());
            e.printStackTrace();
            return e.getResponseBody();
        }
    }
}