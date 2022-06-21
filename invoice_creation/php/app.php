// NOTE: this is a complete request, but please customize it before trying to send it!

// In this example we are using our PHP SDK
// https://packagist.org/packages/fattureincloud/fattureincloud-php-sdk

<?php
require_once(__DIR__ . '/vendor/autoload.php');

use FattureInCloud\Model\Currency;
use FattureInCloud\Model\DocumentTemplate;
use FattureInCloud\Model\Entity;
use FattureInCloud\Model\IssuedDocument;
use FattureInCloud\Model\IssuedDocumentItemsListItem;
use FattureInCloud\Model\IssuedDocumentPaymentsListItem;
use FattureInCloud\Model\IssuedDocumentStatus;
use FattureInCloud\Model\IssuedDocumentType;
use FattureInCloud\Model\CreateIssuedDocumentRequest;
use FattureInCloud\Model\Language;
use FattureInCloud\Model\PaymentAccount;
use FattureInCloud\Model\PaymentMethod;
use FattureInCloud\Model\VatType;


//set your access token
$config = FattureInCloud\Configuration::getDefaultConfiguration()->setAccessToken('YOUR_ACCESS_TOKEN');

$apiInstance = new FattureInCloud\Api\IssuedDocumentsApi(
    new GuzzleHttp\Client(),
    $config
);

//set your company id
$company_id = 12345;
    
$entity = new Entity;
$entity
    ->setId(1)
    ->setName("Mario Rossi")
    ->setVatNumber("47803200154")
    ->setTaxCode("RSSMRA91M20B967Q")
    ->setAddressStreet("Via Italia, 66")
    ->setAddressPostalCode("20900")
    ->setAddressCity("Milano")
    ->setAddressProvince("MI")
    ->setCountry("Italia");

$invoice = new IssuedDocument;
$invoice->setType(IssuedDocumentType::INVOICE);
$invoice->setEntity($entity);
$invoice->setDate(new DateTime("2022-07-20"));
$invoice->setNumber(1);
$invoice->setNumeration("/fatt");
$invoice->setSubject("internal subject");
$invoice->setVisibleSubject("visible subject");
$invoice->setCurrency(
    new Currency(
        array(
           "id" => "EUR" 
        )
    )
);
$invoice->setLanguage(
    new Language(
        array(
            "code" => "it",
            "name" => "italiano"
        )
    )
);
$invoice->setItemsList(
    array(
        new IssuedDocumentItemsListItem(
            array(
                "product_id" => 4,
                "code" => "TV3",
                "name" => "Tavolo in legno",
                "net_price" => 100,
                "category" => "cucina",
                "discount" => 0,
                "qty" => 1,
                "vat" => new VatType(
                    array(
                        "id" => 0
                    )
                )
            )
        )
    )
);
$invoice->setPaymentMethod(
    new PaymentMethod(
        array(
            "id" => 386683
        )
    )
);
$invoice->setPaymentsList(
    array(
        new IssuedDocumentPaymentsListItem(
            array(
                "amount" => 122,
                "due_date" => new DateTime("2022-07-23"),
                "paid_date" => new DateTime("2022-07-22"),
                "status" => IssuedDocumentStatus::PAID,
                "payment_account" => new PaymentAccount(
                    array(
                        "id" => 110
                    )
                )
            )
        )
    )
);
$invoice->setAttachmentToken("YmMyNWYxYzIwMTU3N2Y4ZGE3ZjZiMzg5OWY0ODNkZDQveXl5LmRvYw");
$invoice->setTemplate(
    new DocumentTemplate(
        array(
            "id" => 150
        )
    )
);

// Here we put our invoice in the request object
$create_issued_document_request = new CreateIssuedDocumentRequest();
$create_issued_document_request->setData($invoice);

// Now we are all set for the final call
// Create the invoice: https://github.com/fattureincloud/fattureincloud-php-sdk/blob/master/docs/Api/IssuedDocumentsApi.md#createissueddocument
try {
    $result = $apiInstance->createIssuedDocument($company_id, $create_issued_document_request);
    print_r($result);
} catch (Exception $e) {
    echo 'Exception when calling IssuedDocumentsApi->createIssuedDocument: ', $e->getMessage(), PHP_EOL;
}