// NOTE: this is a complete request, but please customize it!!!
// In the next step we'll explain how to perform the request to the API.

// in this example we are using our Go SDK
// https://github.com/fattureincloud/fattureincloud-go-sdk
package main

import (
	"context"
	"encoding/json"
	"fmt"
	"os"

	fattureincloudapi "github.com/fattureincloud/fattureincloud-go-sdk/v2/api"
	fattureincloud "github.com/fattureincloud/fattureincloud-go-sdk/v2/model"
)

func main() {
	//set your access token
	auth := context.WithValue(context.Background(), fattureincloudapi.ContextAccessToken, "YOUR_ACCESS_TOKEN")
	configuration := fattureincloudapi.NewConfiguration()
	apiClient := fattureincloudapi.NewAPIClient(configuration)

	//set your company id
	companyId := int32(12345)

	entity := *fattureincloud.NewEntity().
		SetId(1).
		SetName("Mario Rossi").
		SetVatNumber("RSSMRA91M20B967Q").
		SetTaxCode("Via Italia, 66").
		SetAddressPostalCode("20900").
		SetAddressProvince("MI").
		SetCountry("Italia")

	invoice := *fattureincloud.NewIssuedDocument().
		SetEntity(entity).
		SetType(fattureincloud.IssuedDocumentTypes.INVOICE).
		SetDate("2022-01-20").
		SetNumber(1).
		SetNumeration("/fatt").
		SetSubject("internal subject").
		SetVisibleSubject("visible subject").
		SetCurrency(*fattureincloud.NewCurrency().SetId("EUR")).
		SetLanguage(*fattureincloud.NewLanguage().SetCode("it").SetName("italiano")).
		SetItemsList([]fattureincloud.IssuedDocumentItemsListItem{
			*fattureincloud.NewIssuedDocumentItemsListItem().
				SetProductId(4).
				SetCode("TV3").
				SetName("Tavolo in legno").
				SetNetPrice(100).
				SetCategory("cucina").
				SetDiscount(0).
				SetQty(1).
				SetVat(*fattureincloud.NewVatType().SetId(0)),
		}).
		SetPaymentsList([]fattureincloud.IssuedDocumentPaymentsListItem{
			*fattureincloud.NewIssuedDocumentPaymentsListItem().
				SetAmount(122).
				SetDueDate("2022-01-23").
				SetPaidDate("2022-01-22").
				SetStatus(fattureincloud.IssuedDocumentStatuses.NOT_PAID).
				SetPaymentAccount(*fattureincloud.NewPaymentAccount().SetId(110)),
		}).
		SetPaymentMethod(*fattureincloud.NewPaymentMethod().SetId(386683)).
		SetAttachmentToken("YmMyNWYxYzIwMTU3N2Y4ZGE3ZjZiMzg5OWY0ODNkZDQveXl5LmRvYw").
		SetTemplate(*fattureincloud.NewDocumentTemplate().SetId(150))

	// Here we put our invoice in the request object
	createIssuedDocumentRequest := *fattureincloud.NewCreateIssuedDocumentRequest().SetData(invoice)

	// Now we are all set for the final call
	// Create the invoice: https://github.com/fattureincloud/fattureincloud-go-sdk/blob/master/docs/IssuedDocumentsApi.md#createIssuedDocument
	resp, r, err := apiClient.IssuedDocumentsApi.CreateIssuedDocument(auth, companyId).CreateIssuedDocumentRequest(createIssuedDocumentRequest).Execute()
	if err != nil {
		fmt.Fprintf(os.Stderr, "Error when calling `IssuedDocumentsApi.CreateIssuedDocument``: %v\n", err)
		fmt.Fprintf(os.Stderr, "Full HTTP response: %v\n", r)
	}
	json.NewEncoder(os.Stdout).Encode(resp)
}
