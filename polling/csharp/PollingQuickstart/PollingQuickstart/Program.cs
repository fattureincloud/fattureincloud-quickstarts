// The following dependencies are required
// dotnet add package It.FattureInCloud.Sdk
// dotnet add package Polly
// dotnet add package Polly.Contrib.WaitAndRetry

using Newtonsoft.Json;
using Polly;
using It.FattureInCloud.Sdk.Api;
using It.FattureInCloud.Sdk.Model;
using It.FattureInCloud.Sdk.Client;
using Polly.Contrib.WaitAndRetry;

namespace poll
{
    class Program
    {
        public static ProductsApi apiInstance;

        static void Main(string[] args)
        {
            // This code should be executed periodically using a cron library or job scheduler.
            // For example: https://www.quartz-scheduler.net/
            SyncProducts();
        }

        private static void SyncProducts()
        {
            // Here we init the Fatture in Cloud SDK
            // The Access Token is retrieved using the "GetToken" method
            Configuration config = new Configuration();
            config.AccessToken = GetToken();

            // In this example we're using the Products API
            apiInstance = new ProductsApi(config);

            // The ID of the controlled company.
            var companyId = 2;

            // Here we setup the exponential backoff config
            var maxRetryAttempts = 5;
            var pauseBetweenFailures =
                Backoff.ExponentialBackoff(TimeSpan.FromSeconds(2), retryCount: maxRetryAttempts);

            var retryPolicy = Policy
                .Handle<ApiException>()
                .WaitAndRetry(pauseBetweenFailures);

            try
            {
                // In this example we suppose to export the data to a JSON Lines file.
                // First, we cancel the content of the destination file
                File.WriteAllText("products.jsonl", String.Empty);

                // List Products
                var perPage = 50;

                // We perform the first request
                ListProductsResponse result =
                    ListProductsWithBackoff(companyId, 1, perPage, retryPolicy, apiInstance);
                // We use the first response to extract the last page index
                var lastPage = result.LastPage;
                // We append the products obtained with the first request top the output file
                // Data contains an array of products
                AppendProductsToFile(result.Data);

                // For the missing pages (we already requested the first one)
                for (var i = 2; i <= lastPage; i++)
                {
                    // We require the page to the API
                    result = ListProductsWithBackoff(companyId, i, perPage, retryPolicy, apiInstance);
                    // And append all the retrieved products
                    AppendProductsToFile(result.Data);
                }
            }
            catch (ApiException ex)
            {
                Console.WriteLine("Exception when calling ProductsApi.ListProducts: " + ex.Message);
                Console.WriteLine("Status Code: " + ex.ErrorCode);
                Console.WriteLine(ex.StackTrace);
            }
        }

        // In this function we append the products in the JSON Lines file.
        // You can replace this function to perform the operations you need.
        // For example, you can build SQL queries or call a third-party API using the retrieved products.
        private static void AppendProductsToFile(List<Product> products)
        {
            StreamWriter sw = File.AppendText("products.jsonl");
            // For each product in the list
            foreach (Product p in products)
            {
                // We write the product to the file
                sw.WriteLine(JsonConvert.SerializeObject(p, Formatting.None));
            }
            sw.Close();
        }

        // Here we wrap the SDK method with an exponential backoff
        // This is to manage the quota exceeded issue
        private static ListProductsResponse ListProductsWithBackoff(int companyId, int currentPage, int perPage, Policy retryPolicy, ProductsApi apiInstance)
        {
            int attempt = 0;
            return retryPolicy.Execute(() =>
            {
                attempt++;
                Console.WriteLine(String.Format("Page: {0}, Attempt: {1}, WaitTime(millis): {2}", currentPage, attempt, Math.Pow(2, attempt) * 1000));
                // The actual SDK method is executed here
                return apiInstance.ListProducts(companyId, null, "detailed", null, currentPage, 5);
            });
        }

        // This is just a mock: this function should contain the code to retrieve the Access Token
        private static string GetToken()
        {
            return "YOUR_ACCESS_TOKEN";
        }
    }
}