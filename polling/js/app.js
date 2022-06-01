// The following dependency is required
// yarn add @fattureincloud/fattureincloud-js-sdk

const fs = require('fs')
const fattureInCloudSdk = require('@fattureincloud/fattureincloud-js-sdk')

// Here we init the Fatture in Cloud SDK
// The Access Token is retrieved using the "getToken" method
var defaultClient = fattureInCloudSdk.ApiClient.instance
var OAuth2AuthenticationCodeFlow = defaultClient.authentications['OAuth2AuthenticationCodeFlow']
OAuth2AuthenticationCodeFlow.accessToken = getToken()

// In this example we're using the Products API
var productsApiInstance = new fattureInCloudSdk.ProductsApi()

var fileName = 'products.jsonl'

// This code should be executed periodically using a cron library or job scheduler.
// For example: https://www.npmjs.com/package/node-cron
main()
    .then()
    .catch(err => console.error(err))

async function main() {
    // In this example we suppose to export the data to a JSON Lines file.
    // First, we delete the content of the destination file if it exists.
    let exists = fs.existsSync(fileName)
    if(exists){
        fs.truncate(fileName, err => {
            if (err) {
                console.error(err)
                return
            }
        })
    } else {
        fs.writeFileSync(fileName);
    }
  
    // Here we define the parameters for the first request.
    let opts = {
        'fields': null,
        'fieldset': 'detailed',
        'sort': null,
        'page': 1, // We're trying to obtain the first page
        'perPage': 5 // Every page will contain at most 5 products
    }
    let companyId = 2 // This is the ID of the company we're working on
    
    try {
        // We perform the first request
        let result = await listProductsWithBackoff(companyId, opts)
        // We recover the last page index
        let lastPage = result['last_page']
        // We write the products of this page to the file
        // "data" contains an array of products 
        await appendProductsToFile(result['data'])
      
        // For all the remaining pages (we already have the first one)
        for (var i = 2; i <= lastPage; i++) {
            // We update the page index
            opts['page'] = i
            // We require the page at the selected index
            result = await listProductsWithBackoff(companyId, opts)
            // And we write the products to the file
            await appendProductsToFile(result['data'])
        }
        console.log('products succesfully retrieved and saved in ./products.jsonl')
    } catch (e) {
        console.log(e)
    }
}

// In this function we append the products in the JSON Lines file.
// You can replace this function to perform the operations you need.
// For example, you can build SQL queries or call a third-party API using the retrieved products.
async function appendProductsToFile(products) {
    // For each product in the array
    for (i in products) {
        let product = products[i]
        // We obtain the related JSON and append it to the file as single line
        fs.appendFileSync(fileName, JSON.stringify(product) + '\n', err => {
            if (err) {
                console.error(err)
                return
            }
        })
    }
}

// Here we wrap the SDK method with an exponential backoff
// This is to manage the quota exceeded issue
async function listProductsWithBackoff(companyId, opts) {
    var count = 0
    const delay = retryCount => new Promise(resolve => setTimeout(resolve, 2 ** retryCount * 1000))
    const getProd = async (retryCount = 0, lastError = null) => {
        if (retryCount > 20) throw new Error(lastError)
        try {
            console.log('Page:', opts['page'], 'Attempt:', count++, 'WaitTime(millis):', 2 ** retryCount * 1000)
            // The actual SDK method is executed here
            return await productsApiInstance.listProducts(companyId, opts)
        } catch (e) {
            await delay(retryCount)
            return getProd(retryCount + 1, e)
        }
    }
    try {
        var res = await getProd()
        return (res)
    } catch (e) {
        console.log(e)
    }
}

// This is just a mock: this function should contain the code to retrieve the Access Token
function getToken() {
  return 'YOUR_ACCESS_TOKEN'
}