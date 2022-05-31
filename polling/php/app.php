<?php
require("vendor/autoload.php");
// The following dependencies are required
// composer require stechstudio/backoff
// composer require fattureincloud/fattureincloud-php-sdk

use FattureInCloud\Api\ProductsApi;
use FattureInCloud\Configuration;
use GuzzleHttp\Client;
use STS\Backoff\Backoff;

// This code should be executed periodically using a cron library or job scheduler.
// For example: https://github.com/Cron/Cron

// Here we init the Fatture in Cloud SDK
// The Access Token is retrieved using the "getToken" method
$config = Configuration::getDefaultConfiguration()->setAccessToken(getToken());
// In this example we're using the Products API
$productsApiInstance = new ProductsApi(
    new Client(),
    $config
);

// In this example we suppose to export the data to a JSON Lines file.
// First, we cancel the content of the destination file
file_put_contents("./products.jsonl", "");

// This is the ID of the company we're currently managing
$companyId = 2;
// We require the first page using the ListProducts method
$result = listProductsWithBackoff($productsApiInstance, $companyId, 1);
// We extract the index of the last page from the first response
$lastPage = $result["last_page"];
// We append all the products to the destination file
// "data" contains an array of products 
appendProductsToFile($result["data"]);

// For all the missing pages (we already have the first one)
for ($i = 2; $i <= $lastPage; $i++) {
    // We require the page at the selected index to the API
    $result = listProductsWithBackoff($productsApiInstance, $companyId, $i);
    // We append this page products to the file
    appendProductsToFile($result["data"]);
}

// In this function we append the products in the JSON Lines file.
// You can replace this function to perform the operations you need.
// For example, you can build SQL queries or call a third-party API using the retrieved products.
function appendProductsToFile($products)
{
    // For each product in the array
    foreach ($products as $product) {
        // We encode it to a JSON string and append it to the file as a single line
        file_put_contents("products.jsonl", json_encode($product) . "\n", FILE_APPEND);
    }
}

// Here we wrap the SDK method with an exponential backoff
// This is to manage the quota exceeded issue
function listProductsWithBackoff($productsApiInstance, $companyId, $currentPage): Object
{
    $attempt = 0;
    $backoff = new Backoff(100, 'exponential', 300000, true);
    return $backoff->run(function () use ($productsApiInstance, $companyId, $currentPage, &$attempt) {
        $waitTime = 2 ** $attempt * 1000;
        echo sprintf("Page: %s Attempt: %s WaitTime(millis): %s\n", $currentPage, $attempt++, $waitTime);

        // The actual SDK method is executed here
        $result = $productsApiInstance->listProducts($companyId, null, "detailed", null, $currentPage, 5);

        return $result;
    });
}

// This is just a mock: this function should contain the code to retrieve the Access Token
function getToken(): String {
  return "YOUR_ACCESS_TOKEN";
}