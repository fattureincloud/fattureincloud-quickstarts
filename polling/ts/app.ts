// The following dependency is required
// yarn add @fattureincloud/fattureincloud-ts-sdk
import fs from 'fs'
import { Configuration, ProductsApi, Product } from '@fattureincloud/fattureincloud-ts-sdk'

// Here we init the Fatture in Cloud SDK
// The Access Token is retrieved using the "getToken" method
const apiConfig = new Configuration({
    accessToken: getToken()
});

// In this example we're using the Products API
var productsApiInstance = new ProductsApi(apiConfig);

// File where the products will be saved
var fileName = './products.jsonl'

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
        fs.writeFileSync(fileName, "");
    }
  
    // Here we define the parameters for the first request.
    let page = 1

    let companyId = 2 // This is the ID of the company we're working on
    
    try {
        // We perform the first request
        let result = await listProductsWithBackoff(companyId, page)
        // We recover the last page index
        let lastPage = result["last_page"]
        // We write the products of this page to the file
        // "data" contains an array of products 
        await appendProductsToFile(result.data)
        
        // For all the remaining pages (we already have the first one)
        for (var i = 2; i <= lastPage; i++) {
            // We update the page index
            page = i
            // We require the page at the selected index
            result = await listProductsWithBackoff(companyId, page)
            // And we write the products to the file
            await appendProductsToFile(result.data)
        }
        console.log("products succesfully retrieved and saved in ./products.jsonl")
    } catch (e) {
        console.log(e)
    }
}

// In this function we append the products in the JSON Lines file.
// You can replace this function to perform the operations you need.
// For example, you can build SQL queries or call a third-party API using the retrieved products.
async function appendProductsToFile(products: Array<Product>) {
    // For each product in the array
    for (var i in products) {
        let product = products[i]
        // We obtain the related JSON and append it to the file as single line
        fs.appendFileSync('./products.jsonl', JSON.stringify(product) + "\n")
    }
}

// Here we wrap the SDK method with an exponential backoff
// This is to manage the quota exceeded issue
async function listProductsWithBackoff(companyId: number, page: number) {
    var count = 0
    var perPage = 5
    const delay = (retryCount: number) => new Promise(resolve => setTimeout(resolve, 2 ** retryCount * 1000))
    const getProd: any = async (retryCount = 0, lastError?: string) => {
        if (retryCount > 20) throw new Error(lastError)
        try {
            console.log('Page:', page, 'Attempt:', count++, 'WaitTime(millis):', 2 ** retryCount * 1000)
            // The actual SDK method is executed here
            return await (await productsApiInstance.listProducts(companyId, undefined, "detailed", undefined, page, perPage)).data
        } catch (e: any) {
            await delay(retryCount)
            return getProd(++retryCount, e.message)
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
  return "YOUR_ACCESS_TOKEN"
}