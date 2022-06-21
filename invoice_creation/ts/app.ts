// NOTE: this is a complete request, but please customize it before trying to send it!

// in this example we are using our TS SDK
// https://www.npmjs.com/package/@fattureincloud/fattureincloud-ts-sdk

import { Configuration, IssuedDocumentsApi, Entity, IssuedDocument, IssuedDocumentType, CreateIssuedDocumentRequest } from '@fattureincloud/fattureincloud-ts-sdk';

//set your access token
const apiConfig = new Configuration({
	accessToken: "YOUR_ACCESS_TOKEN"
});

let apiInstance = new IssuedDocumentsApi(apiConfig);

//set your company id
let companyId = 12345;

let entity: Entity = {}
entity.id = 1
entity.name = "Mario Rossi"
entity.vat_number = "47803200154"
entity.tax_code = "RSSMRA91M20B967Q"
entity.address_street = "Via Italia, 66"
entity.address_postal_code = "20900"
entity.address_city = "Milano"
entity.address_province = "MI"
entity.country = "Italia"

let invoice: IssuedDocument = {
	type: IssuedDocumentType.Invoice,
	entity: entity,
	date: "2022-01-20",
	number: 1,
	numeration: "/fatt",
	subject: "internal subject",
	visible_subject: "visible subject",
	currency: {
		id: "EUR"
	},
	language: {
		code: "it",
		name: "italiano"
	},
	items_list: [
		{
			product_id: 4,
			code: "TV3",
			name: "Tavolo in legno",
			net_price: 100,
			category: "cucina",
			discount: 0,
			qty: 1,
			vat: {
				id: 0
			}
		}
	],
	payments_list: [
		{
			amount: 122,
			due_date: "2022-01-23",
			paid_date: "2022-01-22",
			status: "paid",
			payment_account: {
				id: 10
			}
		}
	],
	payment_method: {
		id: 386683
	},
	attachment_token: "YmMyNWYxYzIwMTU3N2Y4ZGE3ZjZiMzg5OWY0ODNkZDQveXl5LmRvYw",
	template: {
		id: 150
	}
}

// Here we put our invoice in the request object
let createIssuedDocumentRequest: CreateIssuedDocumentRequest = {
	data: invoice
}

// Now we are all set for the final call
// Create the invoice: https://github.com/fattureincloud/fattureincloud-ts-sdk/blob/master/docs/IssuedDocumentsApi.md#createIssuedDocument
apiInstance.createIssuedDocument(companyId, createIssuedDocumentRequest).then((data) => {
	console.log(data);
}, (error) => {
	console.error(error);
});