import fattureincloud_python_sdk
from fattureincloud_python_sdk.api import user_api
from fattureincloud_python_sdk.api import suppliers_api
import json
import collections
collections.Callable = collections.abc.Callable #needed if you are using python > 3.10

class Quickstart:
    def get_first_company_suppliers(self):
        
        token_file = open("./token.json")
        json_file = json.load(token_file)
        token_file.close()
        configuration = fattureincloud_python_sdk.Configuration(
            access_token = json_file["access_token"]
        )
        with fattureincloud_python_sdk.ApiClient(configuration) as api_client:
            
            # Retrieve the first company id
            user_api_instance = user_api.UserApi(api_client)
            user_companies_response = user_api_instance.list_user_companies()
            first_company_id = user_companies_response.data.companies[0].id
            
            # Retrieve the list of the Suppliers
            suppliers_api_instance = suppliers_api.SuppliersApi(api_client)
            company_suppliers = suppliers_api_instance.list_suppliers(first_company_id)
            self.send_response(200)
            self.send_header('Content-type','text/html')
            self.end_headers()
            self.wfile.write(bytes(str(company_suppliers.data), "utf8"))
        return