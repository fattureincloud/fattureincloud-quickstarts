// NOTE: this is a complete request, but please customize it before trying to send it!

// in this example we are using our JS SDK
// https://www.npmjs.com/package/@fattureincloud/fattureincloud-js-sdk

var fattureInCloudSdk = require('@fattureincloud/fattureincloud-js-sdk')
let defaultClient = fattureInCloudSdk.ApiClient.instance;

//set your access token
let OAuth2AuthenticationCodeFlow = defaultClient.authentications['OAuth2AuthenticationCodeFlow'];
OAuth2AuthenticationCodeFlow.accessToken = 'YOUR_ACCESS_TOKEN';

let apiInstance = new fattureInCloudSdk.IssuedDocumentsApi();
//set your company id
let companyId = 12345;

let entity = new fattureInCloudSdk.Entity()
entity.id = 1 
entity.name = "Mario Rossi"
entity.vat_number = "47803200154"
entity.tax_code = "RSSMRA91M20B967Q"
entity.address_street = "Via Italia, 66"
entity.address_postal_code = "20900"
entity.address_city = "Milano"
entity.address_province = "MI"
entity.country = "Italia"

let invoice = new fattureInCloudSdk.IssuedDocument()
invoice.type = new fattureInCloudSdk.IssuedDocumentType().invoice
invoice.entity = entity
invoice.date = "2022-07-20"
invoice.number = 1
invoice.numeration = "/fatt"
invoice.subject = "internal subject"
invoice.visible_subject = "visible subject"
invoice.currency = {
    id: "EUR"
}
invoice.language = {
    code: "it",
    name: "Italiano"
}
invoice.items_list = [
    {
        product_id: 4,
        code: "tv3",
        name: "tavolo in legno",
        net_price: 100,
        category: "cucina",
        discount: 0,
        qty: 1,
        vat: {
            id: 0
        }
    }
]
invoice.payments_list = [
    {
        amount: 122,
        due_date: "2022-07-23",
        paid_date: "2022-07-22",
        status: "paid",
        payment_account: {
            id: 10
        }
    }
]
invoice.payment_method = {
    id: 386683
}
invoice.attachment_token = "YmMyNWYxYzIwMTU3N2Y4ZGE3ZjZiMzg5OWY0ODNkZDQveXl5LmRvYw"
invoice.template = {
    id: 150
}

// Here we put our invoice in the request object
let createIssuedDocumentRequest = new fattureInCloudSdk.CreateIssuedDocumentRequest();
createIssuedDocumentRequest.data = invoice 

let opts = {
    'createIssuedDocumentRequest': createIssuedDocumentRequest
  };

// Now we are all set for the final call
// Create the invoice: https://github.com/fattureincloud/fattureincloud-js-sdk/blob/master/docs/IssuedDocumentsApi.md#createIssuedDocument
apiInstance.createIssuedDocument(companyId, opts).then((result) => {
  console.log('API called successfully. Returned result: ' + JSON.stringify(result));
}, (error) => {
  console.error(error);
});