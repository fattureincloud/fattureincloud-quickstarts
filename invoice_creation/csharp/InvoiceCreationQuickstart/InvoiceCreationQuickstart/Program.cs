using System;
using System.Collections.Generic;
using It.FattureInCloud.Sdk.Api;
using It.FattureInCloud.Sdk.Client;
using It.FattureInCloud.Sdk.Model;

namespace test {
    class Program {
        static void Main(string[] args) {
            Configuration config = new Configuration();

            //set your access token
            config.AccessToken = "YOUR_ACCESS_TOKEN";

            var apiInstance = new IssuedDocumentsApi(config);
            //set your company id
            var companyId = 12345;

            // NOTE: this is a complete request, but please customize it!!!
            // In the next step we'll explain how to perform the request to the API.

            // in this example we are using our C# SDK
            // https://www.nuget.org/packages/It.FattureInCloud.Sdk/

            Entity entity = new Entity(
                id: 1,
                name: "Mario Rossi",
                vatNumber: "47803200154",
                taxCode: "RSSMRA91M20B967Q",
                addressStreet: "Via Italia, 66",
                addressPostalCode: "20900",
                addressCity: "Milano",
                addressProvince: "MI",
                country: "Italia"
            );

            IssuedDocument invoice = new IssuedDocument(
                type: IssuedDocumentType.Invoice,
                entity: entity,
                date: new DateTime(2022, 01, 20),
                number: 1,
                numeration: "/fatt",
                subject: "internal subject",
                visibleSubject: "visible subject",
                currency: new Currency(
                    id: "EUR"
                ),
                language: new Language(
                    code: "it",
                    name: "italiano"
                ),
                itemsList: new List < IssuedDocumentItemsListItem > {
                    new IssuedDocumentItemsListItem(
                        productId: 4,
                        code: "TV3",
                        name: "Tavolo in legno",
                        netPrice: 100,
                        category: "cucina",
                        discount: 0,
                        qty: 1,
                        vat: new VatType(
                            id: 0
                        )
                    )
                },
                paymentsList: new List < IssuedDocumentPaymentsListItem > {
                    new IssuedDocumentPaymentsListItem(
                        amount: 122,
                        dueDate: new DateTime(2022, 01, 23),
                        paidDate: new DateTime(2022, 01, 22),
                        status: IssuedDocumentStatus.Paid,
                        paymentAccount: new PaymentAccount(
                            id: 110
                        )
                    )
                },
                paymentMethod: new PaymentMethod(
                    id: 386683
                ),
                attachmentToken: "YmMyNWYxYzIwMTU3N2Y4ZGE3ZjZiMzg5OWY0ODNkZDQveXl5LmRvYw",
                template: new DocumentTemplate(
                    id: 150
                )
            );

            // Here we put our invoice in the request object
            CreateIssuedDocumentRequest createIssuedDocumentRequest = new CreateIssuedDocumentRequest(
                data: invoice
            );

            // Now we are all set for the final call
            // Create the invoice: https://github.com/fattureincloud/fattureincloud-csharp-sdk/blob/master/docs/IssuedDocumentsApi.md#createissueddocument
            try {
                CreateIssuedDocumentResponse result = apiInstance.CreateIssuedDocument(companyId, createIssuedDocumentRequest);
                Console.WriteLine(result);
            } catch (ApiException e) {
                Console.WriteLine("Exception when calling IssuedDocumentsApi.CreateIssuedDocument: " + e.Message);
                Console.WriteLine("Status Code: " + e.ErrorCode);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}