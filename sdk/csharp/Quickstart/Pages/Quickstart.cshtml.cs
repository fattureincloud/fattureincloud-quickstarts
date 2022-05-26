using System;
using System.IO;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using It.FattureInCloud.Sdk.Api;
using It.FattureInCloud.Sdk.Model;
using It.FattureInCloud.Sdk.Client;
using Newtonsoft.Json;

namespace Quickstart.Pages
{
    public class QuickstartModel : PageModel
    {
        private readonly ILogger<QuickstartModel> _logger;

        public QuickstartModel(ILogger<QuickstartModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            using StreamReader file = new("token.json");

            //retrieve the oAuth access token in the file token.json in the bin folder
            var line = file.ReadLine();
            file.Close();
            dynamic json = JsonConvert.DeserializeObject<dynamic>(line);
            string accessToken = json.access_token;

            Configuration config = new Configuration();
            config.AccessToken = accessToken.Replace(@"\", "");

            // Modify the selected supplier
            ModifySupplierRequest modifySupplierRequest = new ModifySupplierRequest();
            modifySupplierRequest.Data = new Supplier();
            modifySupplierRequest.Data.Name = "nuovo nome";
            modifySupplierRequest.Data.Phone = "03561234312";

            var result = modifyFirstSupplier(config, modifySupplierRequest);

            ViewData["Content"] = result;
        }

        public static string modifyFirstSupplier(Configuration config, ModifySupplierRequest modifySupplierRequest)
        {
            try
            {
                var userApiInstance = new UserApi(config);
                var suppliersApiInstance = new SuppliersApi(config);

                // Retrieve User Companies
                var userCompaniesResponse = userApiInstance.ListUserCompanies();
                var firstCompanyId = userCompaniesResponse.Data.Companies[0].Id;

                // Retrieve the list of the Suppliers for the selected company
                var fields = "";  // string | List of comma-separated fields. (optional) 
                var fieldset = "detailed";  // string | Name of the fieldset. (optional) 
                var sort = "-id";  // string | List of comma-separated fields for result sorting (minus for desc sorting). (optional) 
                var page = 2;  // int? | The page to retrieve. (optional)  (default to 1)
                var perPage = 8;  // int? | The size of the page. (optional)  (default to 5)
                var companySuppliers = suppliersApiInstance.ListSuppliers((int)firstCompanyId, null, fieldset, sort, page, perPage);

                return companySuppliers.ToJson();
            }
            catch (Exception e)
            {
                Console.Write(e);
                return e.ToString();
            }
        }
    }
}