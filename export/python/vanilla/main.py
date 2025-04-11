import requests
import json

# You must set the company_id and token variables before running this script
company_id = 0
token = "TOKEN"
filepath = "products.xlsx"

payload = {}
headers = {
  'Accept': 'application/json',
  'Authorization': f"Bearer {token}"
}

# # Uncomment the following lines to get the company_id from the api
# url = f"https://api-v2.fattureincloud.it/user/companies"
# response = requests.request("GET", url, headers=headers, data=payload)
# m = response.json()
# company_id = m['data']['companies'][0]['id']
# print(f"Using company id: {company_id}")

products = []

curr_page = 1
last_page = 1
while True:
  url = f"https://api-v2.fattureincloud.it/c/{company_id}/products?fieldset=detailed&page={curr_page}&per_page=5"
  response = requests.request("GET", url, headers=headers, data=payload)
  m = response.json()
  items = m['data']
  products = products + items
  if curr_page == 1:
    # Get the total number of pages
    last_page = m['last_page']
  print(f"Page {curr_page} of {last_page}")
  curr_page = curr_page + 1
  if curr_page > last_page:
    break

print(json.dumps(products, indent=4))

# here we write the products to an excel file
from openpyxl import Workbook
wb = Workbook()

ws = wb.active


# Rows can also be appended
ws.append(["id", "name", "code"])

for product in products:
    ws.append([product['id'], product['name'], product['code']])

# Save the file
wb.save(filepath)