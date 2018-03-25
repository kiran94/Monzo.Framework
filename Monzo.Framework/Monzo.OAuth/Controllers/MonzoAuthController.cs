using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;

namespace Monzo.OAuth.Controllers
{
    public class MonzoAuthController : Controller
    {

        const string MonzoAuthUrl = "https://auth.monzo.com/";
        const string ClientId = "oauthclient_00009TL0ugY0Esx3TTy2Yj";
        const string redirect_uri = "http://localhost:58347/MonzoAuth/OAuth";
        const string code = "secret";
        const string MonzoAccessUrl = "https://api.monzo.com/oauth2/token";
        const string MonzoClientSecret = "YxDtSvgwb33YlqtCtZtipzAIZP8Ity573KtIrfo6OGusvcyHp+6FvGE2fr1v71LGQIr30CnqP6MbBmhKmePC";
        const string redirect_uri2 = "http://localhost:58347/MonzoAuth/OAuth";

        const string url = MonzoAuthUrl + "?client_id=" + ClientId + "&redirect_uri=" + redirect_uri + "&response_type=code&state=" + code;
        static string auth_code = "";

        public ActionResult RequestAuth()
        {
            return Redirect(url);
        }

        public ActionResult OAuth(string code, string state)
        {
            auth_code = code;
            return Redirect("RequestAccess");
        }

        public string RequestAccess()
        {
            var httpClient = new HttpClient();
            var values = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "client_id", ClientId },
                { "client_secret", MonzoClientSecret },
                { "redirect_uri", redirect_uri2 },
                { "code", auth_code }
            };

            var content = new FormUrlEncodedContent(values);

            var response = httpClient.PostAsync(MonzoAccessUrl, content);

            var responseString = response.Result.Content.ReadAsStringAsync();

            responseString.Wait();

            return responseString.Result; 
        }

        public string Redirect()
        {
            return "You were redirected here by Monzo";
        }
    }
}