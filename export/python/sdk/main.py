import fattureincloud_python_sdk
from fattureincloud_python_sdk.models.list_products_response import ListProductsResponse
from fattureincloud_python_sdk.rest import ApiException
import json

# You must set the company_id and token variables before running this script
company_id = 0
token = "TOKEN"
filepath = "products.xlsx"

configuration = fattureincloud_python_sdk.Configuration(
  host = "https://api-v2.fattureincloud.it",
  access_token = token
)

with fattureincloud_python_sdk.ApiClient(configuration) as api_client:
  # # Uncomment the following lines to get the company_id from the api
  # user_api = fattureincloud_python_sdk.UserApi(api_client)
  # try:
  #   # List User Companies
  #   api_response = user_api.list_user_companies()
  #   company_id = api_response.data.companies[0].id
  #   print(f"Using company id: {company_id}")
  # except Exception as e:
  #   print("Exception when calling UserApi->list_user_companies: %s\n" % e)

  products_api = fattureincloud_python_sdk.ProductsApi(api_client)
  
  per_page = 100
  fieldset = "detailed"

  try:
    products = []

    curr_page = 1
    last_page = 1
    while True:
      # List Products
      api_response = products_api.list_products(company_id, fieldset=fieldset, page=curr_page, per_page=per_page)
      items = []
      for p in api_response.data:
        items.append(p.to_dict())

      products = products + items
      if curr_page == 1:
        # Get the total number of pages
        last_page = api_response.last_page
      print(f"Page {curr_page} of {last_page}")
      curr_page = curr_page + 1
      if curr_page > last_page:
        break
  except Exception as e:
      print("Exception when calling ProductsApi->list_products: %s\n" % e)

print(json.dumps(products, indent=4))

# Here we write the products to an excel file
from openpyxl import Workbook
wb = Workbook()

ws = wb.active

# Here we select only a few columns, but you can customize the file as you want
ws.append(["id", "name", "code"])

for product in products:
    ws.append([product['id'], product['name'], product['code']])

# Save the file
wb.save(filepath)