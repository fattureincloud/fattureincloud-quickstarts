import datetime
import fattureincloud_python_sdk
from fattureincloud_python_sdk.api import issued_documents_api
from fattureincloud_python_sdk.models.vat_type import VatType
from fattureincloud_python_sdk.models.currency import Currency
from fattureincloud_python_sdk.models.language import Language
from fattureincloud_python_sdk.models.entity import Entity
from fattureincloud_python_sdk.models.payment_method import PaymentMethod
from fattureincloud_python_sdk.models.payment_account import PaymentAccount
from fattureincloud_python_sdk.models.document_template import DocumentTemplate
from fattureincloud_python_sdk.models.issued_document import IssuedDocument
from fattureincloud_python_sdk.models.issued_document_type import IssuedDocumentType
from fattureincloud_python_sdk.models.issued_document_status import IssuedDocumentStatus
from fattureincloud_python_sdk.models.create_issued_document_request import CreateIssuedDocumentRequest
from fattureincloud_python_sdk.models.create_issued_document_response import CreateIssuedDocumentResponse
from fattureincloud_python_sdk.models.issued_document_items_list_item import IssuedDocumentItemsListItem
from fattureincloud_python_sdk.models.issued_document_payments_list_item import IssuedDocumentPaymentsListItem

from pprint import pprint

# set your access token
configuration = fattureincloud_python_sdk.Configuration()
configuration.access_token = "YOUR_ACCESS_TOKEN"

# set your company id
company_id = 12345

# NOTE: this is a complete request, but please customize it!!!
# In the next step we'll explain how to perform the request to the API.

# in this example we are using our Python SDK 
# https://pypi.org/project/fattureincloud-python-sdk/

entity = Entity(
    id=1,
    name="Mario Rossi",
    vat_number="47803200154",
    tax_code="RSSMRA91M20B967Q",
    address_street="Via Italia, 66",
    address_postal_code="20900",
    address_city="Milano",
    address_province="MI",
    country="Italia"
)

invoice = IssuedDocument(
    type = IssuedDocumentType("invoice"),
    entity = entity,
    date = datetime.date(2021, 1, 20),
    number = 1,
    numeration = "/fatt",
    subject = "internal subject",
    visible_subject = "visible subject",
    currency = Currency(
        id="EUR"
    ),
    language = Language(
        code="it",
        name="italiano"
    ),
    items_list = [
        IssuedDocumentItemsListItem(
            product_id=4,
            code="TV3",
            name="Tavolo in legno",
            net_price=100.0,
            category="cucina",
            discount=0.0,
            qty=1.0,
            vat=VatType(
                id=0
            )
        )
    ],
    payments_list = [
        IssuedDocumentPaymentsListItem(
            amount=122.0,
            due_date=datetime.date(2022, 1, 23),
            paid_date=datetime.date(2022, 1, 22),
            status=IssuedDocumentStatus("paid"),
            payment_account=PaymentAccount(
                id=110
            )
        )
    ],
    payment_method = PaymentMethod(
        id=386683
    ),
    attachment_token = "YmMyNWYxYzIwMTU3N2Y4ZGE3ZjZiMzg5OWY0ODNkZDQveXl5LmRvYw",
    template = DocumentTemplate(
        id=150
    )
)

# Here we put our invoice in the request object
create_issued_document_request = CreateIssuedDocumentRequest(
    data = invoice
)

# Now we are all set for the final call
# Create the invoice: https://github.com/fattureincloud/fattureincloud-python-sdk/blob/master/docs/IssuedDocumentsApi.md#create_issued_document

with fattureincloud_python_sdk.ApiClient(configuration) as api_client:

    api_instance = issued_documents_api.IssuedDocumentsApi(api_client)
    try:
        api_response = api_instance.create_issued_document(company_id, create_issued_document_request=create_issued_document_request)
        pprint(api_response)
    except fattureincloud_python_sdk.ApiException as e:
        print("Exception when calling IssuedDocumentsApi->create_issued_document: %s\n" % e)