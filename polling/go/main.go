// The following dependencies is required
// go get github.com/fattureincloud/fattureincloud-go-sdk/
// go get github.com/cenkalti/backoff/v4

package main

import (
	"context"
	"encoding/json"
	"fmt"
	"os"

	backoff "github.com/cenkalti/backoff/v4"
	fattureincloudapi "github.com/fattureincloud/fattureincloud-go-sdk/api"
	fattureincloud "github.com/fattureincloud/fattureincloud-go-sdk/model"
)

var (
	f, _ = os.OpenFile("products.jsonl", os.O_APPEND|os.O_WRONLY, 0644)
	// The Access Token is retrieved using the "getToken" method
	auth          = context.WithValue(context.Background(), fattureincloudapi.ContextAccessToken, getToken())
	configuration = fattureincloudapi.NewConfiguration()
	apiClient     = fattureincloudapi.NewAPIClient(configuration)
	companyId     = int32(16) // This is the ID of the company we're working on
	// Here we define the parameters for the first request.
	nextPage = 1
	attempts = 0
)

func main() {
	// This code should be executed periodically using a cron library or job scheduler.
	syncProducts()
}

func syncProducts() {
	// In this example we suppose to export the data to a JSON Lines file.
	// First, we cancel the content of the destination file
	f.Truncate(0)
	// Here we define the operation that retrieves the products
	operation := func() error {
		attempts++
		fmt.Printf("Attempt: %d\n", attempts)
		// In this example we're using the Products API
		// Here we execute the actual SDK method
		resp, _, err := apiClient.ProductsApi.ListProducts(auth, companyId).Page(int32(nextPage)).PerPage(5).Execute()
		if resp != nil {
			// We check if there are other pages to retrieve
			if resp.NextPageUrl.Get() == nil {
				nextPage = 0
			} else {
				nextPage++
			}
			// We write the products of this page to the file
			// "data" contains an array of products
			appendProductsToFile(resp.Data)
		}
		return err
	}
	// For all the pages
	for nextPage != 0 {
		attempts = 0
		// We call the operation function using Exponential Backoff
		err := backoff.Retry(operation, backoff.NewExponentialBackOff())
		if err != nil {
			fmt.Fprintf(os.Stderr, "Error %v\n", err)
			return
		}
	}
	f.Close()
	fmt.Println("products succesfully retrieved and saved in ./products.jsonl")
}

// In this function we append the products in the JSON Lines file.
// You can replace this function to perform the operations you need.
// For example, you can build SQL queries or call a third-party API using the retrieved products.
func appendProductsToFile(products []fattureincloud.Product) {
	// For each product in the array
	for _, element := range products {
		// We obtain the related JSON and append it to the file as single line
		jsonStr, _ := json.Marshal(element)
		f.WriteString(string(jsonStr) + "\n")
	}
}

// This is just a mock: this function should contain the code to retrieve the Access Token
func getToken() string {
	return "YOUR_TOKEN"
}
