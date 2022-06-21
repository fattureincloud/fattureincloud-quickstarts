require 'time'
require 'fattureincloud_ruby_sdk'

FattureInCloud_Ruby_Sdk.configure do |config|
  # set your access token
  config.access_token = 'YOUR ACCESS TOKEN'
end

api_instance = FattureInCloud_Ruby_Sdk::IssuedDocumentsApi.new
# set your company id
company_id = 12345

# NOTE: this is a complete request, but please customize it!!!
# In the next step we'll explain how to perform the request to the API.

# in this example we are using our Ruby SDK 
# https://rubygems.org/gems/fattureincloud_ruby_sdk

entity = FattureInCloud_Ruby_Sdk::Entity.new(
  id: 1,
  name: "Mario Rossi",
  vat_number: "47803200154",
  tax_code: "RSSMRA91M20B967Q",
  address_street: "Via Italia, 66",
  address_postal_code: "20900",
  address_city: "Milano",
  address_province: "MI",
  country: "Italia",
)

invoice = FattureInCloud_Ruby_Sdk::IssuedDocument.new(
  type: FattureInCloud_Ruby_Sdk::IssuedDocumentType::INVOICE,
  entity: entity,
  date: Date.new(2022, 01, 20),
  number: 1,
  numeration: "/fatt",
  subject: "internal subject",
  visible_subject: "visible subject",
  currency: FattureInCloud_Ruby_Sdk::Currency.new(
      id: "EUR"
  ),
  language: FattureInCloud_Ruby_Sdk::Language.new(
      code: "it",
      name: "italiano"
  ),
  items_list: Array(
    FattureInCloud_Ruby_Sdk::IssuedDocumentItemsListItem.new(
          product_id: 4,
          code: "TV3",
          name: "Tavolo in legno",
          net_price: 100,
          category: "cucina",
          discount: 0,
          qty: 1,
          vat: FattureInCloud_Ruby_Sdk::VatType.new(
              id: 0
          )
      )
  ),
  payments_list: Array(
    FattureInCloud_Ruby_Sdk::IssuedDocumentPaymentsListItem.new(
          amount: 122,
          due_date: Date.new(2022, 01, 23),
          paid_date: Date.new(2022, 01, 22),
          status: FattureInCloud_Ruby_Sdk::IssuedDocumentStatus::PAID,
          payment_account: FattureInCloud_Ruby_Sdk::PaymentAccount.new(
              id: 110
          )
      )
  ),
  payment_method: FattureInCloud_Ruby_Sdk::PaymentMethod.new(
      id: 386683
  ),
  attachment_token: "YmMyNWYxYzIwMTU3N2Y4ZGE3ZjZiMzg5OWY0ODNkZDQveXl5LmRvYw",
  template: FattureInCloud_Ruby_Sdk::DocumentTemplate.new(
      id: 150
  )
)

# Here we put our invoice in the request object
opts = {
  create_issued_document_request: FattureInCloud_Ruby_Sdk::CreateIssuedDocumentRequest.new(data: invoice)
}
# Now we are all set for the final call
# Create the invoice: https://github.com/fattureincloud/fattureincloud-ruby-sdk/blob/master/docs/IssuedDocumentsApi.md#create_issued_document
begin
  result = api_instance.create_issued_document(company_id, opts)
  p result
rescue FattureInCloud_Ruby_Sdk::ApiError => e
  puts "Error when calling IssuedDocumentsApi->create_issued_document: #{e}"
end