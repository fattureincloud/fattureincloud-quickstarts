# The following dependencies are required
# pip install backoff
# pip install fattureincloud-python-sdk
import fattureincloud_python_sdk
from fattureincloud_python_sdk.api import products_api
from fattureincloud_python_sdk.exceptions import ApiException
import backoff
# import collections #needed if you are using python > 3.10
# collections.Callable = collections.abc.Callable #needed if you are using python > 3.10

# Here we implement the custom backoff message
def backoff_hdlr(details):
    print(("Attempt: {tries}, WaitTime(millis):").format(**details), 2 ** details['tries'] * 1000)

# Here we setup the exponential backoff config
@backoff.on_exception(backoff.expo, ApiException, max_tries=10, on_backoff=backoff_hdlr, on_success=backoff_hdlr)
def list_products_with_backoff(products_api_instance, company_id, current_page, per_page):
    print("Page:", current_page, end=", ")
    return products_api_instance.list_products(company_id, page=current_page, per_page=per_page)

# In this function we append the products in the JSON Lines file.
# You can replace this function to perform the operations you need.
# For example, you can build SQL queries or call a third-party API using the retrieved products.
def append_products_to_page(products):
    # For each product in the list
    for p in products:
        f = open("products.jsonl", "a")
        # We write the product to the file
        f.write(str(p).replace("\n", ""))
        f.write("\n")
        f.close()
    
def get_token():
    return "YOUR_ACCESS_TOKEN"

def sync_products():
    # Here we init the Fatture in Cloud SDK
    # The Access Token is retrieved using the "GetToken" method
    configuration = fattureincloud_python_sdk.Configuration(
        access_token = get_token()
    )
    configuration.retries = 0 # Needed to implement custom backoff
    # The ID of the controlled company.
    company_id = 2
    current_page = 1
    per_page = 50

    with fattureincloud_python_sdk.ApiClient(configuration) as api_client:
        # In this example we're using the Products API
        products_api_instance = products_api.ProductsApi(api_client)

        # We perform the first request
        result = list_products_with_backoff(products_api_instance, company_id, current_page, per_page)
        last_page = result.last_page

        # We append the products obtained with the first request to the output file
        # Data contains an array of products
        append_products_to_page(result.data)

        # In this example we suppose to export the data to a JSON Lines file.
        # First, we cancel the content of the destination file
        file = open("products.jsonl","r+")
        file.truncate(0)
        file.close()
        
        # For the missing pages (we already requested the first one)
        for x in range(2, last_page):
            # We require the page to the API
            result = list_products_with_backoff(products_api_instance, company_id, x, per_page)
            # And append all the retrieved products
            append_products_to_page(result.data)

# This code should be executed periodically using a cron library or job scheduler.
sync_products()