using System;
using System.IO;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using It.FattureInCloud.Sdk.OauthHelper;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Quickstart.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            string code = HttpContext.Request.Query["code"];
            var oauth = new OAuth2AuthorizationCodeManager("CLIENT_ID", "CLIENT_SECRET", "http://localhost:5000/");

            if (code is null)
            {
                var scopes = new List<Scope> { Scope.ENTITY_SUPPLIERS_READ };
                var url = oauth.GetAuthorizationUrl(scopes, "EXAMPLE_STATE");
                Response.Redirect(url);
            }
            else
            {
                var token = oauth.FetchToken(code);
                using StreamWriter file = new("token.json");

                file.Write(JsonConvert.SerializeObject(token)); //saving the oAuth access token in the file token.json in the bin folder
                file.Close();

                ViewData["Content"] = "Token saved succesfully in token.json in your bin folder";
            }
        }
    }
}