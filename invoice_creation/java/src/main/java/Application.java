import it.fattureincloud.sdk.ApiClient;
import it.fattureincloud.sdk.ApiException;
import it.fattureincloud.sdk.Configuration;
import it.fattureincloud.sdk.auth.*;
import it.fattureincloud.sdk.model.*;
import it.fattureincloud.sdk.api.IssuedDocumentsApi;
import java.math.BigDecimal;
import java.time.LocalDate;

public class Application {
    public static void main(String[] args) {
        ApiClient defaultClient = Configuration.getDefaultApiClient();

        //set your access token
        // Configure OAuth2 access token for authorization: OAuth2AuthenticationCodeFlow
        OAuth OAuth2AuthenticationCodeFlow = (OAuth) defaultClient.getAuthentication("OAuth2AuthenticationCodeFlow");
        OAuth2AuthenticationCodeFlow.setAccessToken("YOUR_ACCESS_TOKEN");

        IssuedDocumentsApi apiInstance = new IssuedDocumentsApi(defaultClient);
        //set your company id
        Integer companyId = 12345;

        // NOTE: this is a complete request, but please customize it!!!
        // In the next step we'll explain how to perform the request to the API.

        // in this example we are using our Java SDK
        // https://search.maven.org/artifact/it.fattureincloud/fattureincloud-java-sdk

        Entity entity = new Entity()
                .id(1)
                .name("Mario Rossi")
                .vatNumber("47803200154")
                .taxCode("RSSMRA91M20B967Q")
                .addressStreet("Via Italia, 66")
                .addressPostalCode("20900")
                .addressCity("Milano")
                .addressProvince("MI");

        IssuedDocument invoice = new IssuedDocument()
                .type(IssuedDocumentType.INVOICE)
                .entity(entity)
                .date(LocalDate.of(2022, 1, 20))
                .number(1)
                .numeration("/fatt")
                .subject("internal subject")
                .visibleSubject("visible subject")
                .currency(new Currency().id("EUR"))
                .language(new Language()
                                .code("it")
                                .name("italiano")
                )
                .addItemsListItem(
                        new IssuedDocumentItemsListItem()
                                .productId(4)
                                .code("TV3")
                                .name("Tavolo in legno")
                                .netPrice(BigDecimal.valueOf(100))
                                .category("cucina")
                                .discount(BigDecimal.valueOf(0))
                                .qty(BigDecimal.valueOf(1))
                                .vat(new VatType().id(0))
                )
                .addPaymentsListItem(
                        new IssuedDocumentPaymentsListItem()
                                .amount(BigDecimal.valueOf(122))
                                .dueDate(LocalDate.of(2022, 01, 23))
                                .paidDate(LocalDate.of(2022, 01, 22))
                                .status(IssuedDocumentStatus.PAID)
                                .paymentAccount(new PaymentAccount().id(110))
                )
                .paymentMethod(
                        new PaymentMethod().id(386683)
                )
                .attachmentToken("YmMyNWYxYzIwMTU3N2Y4ZGE3ZjZiMzg5OWY0ODNkZDQveXl5LmRvYw")
                .template(new DocumentTemplate().id(150));
        
        // Here we put our invoice in the request object
        CreateIssuedDocumentRequest createIssuedDocumentRequest = new CreateIssuedDocumentRequest()
                .data(invoice);

        // Now we are all set for the final call
        // Create the invoice: https://github.com/fattureincloud/fattureincloud-java-sdk/blob/master/docs/IssuedDocumentsApi.md#createissueddocument
        try {
            CreateIssuedDocumentResponse result = apiInstance.createIssuedDocument(companyId, createIssuedDocumentRequest);
            System.out.println(result);
        } catch (ApiException e) {
            System.err.println("Exception when calling IssuedDocumentsApi#createIssuedDocument");
            System.err.println("Status code: " + e.getCode());
            System.err.println("Reason: " + e.getResponseBody());
            System.err.println("Response headers: " + e.getResponseHeaders());
            e.printStackTrace();
        }
    }
}