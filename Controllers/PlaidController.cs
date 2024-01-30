using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using Main_Plaid.Models; // Corrected namespace

namespace Main_Plaid.Controllers
{
    public class PlaidController : Controller
    {
        //"publicKey": "public-sandbox-b0e2c4ee-a763-4df5-bfe9-46a46bce993d";
        //private const string request_id= "Aim3b";
        private const string PublicTokenURL = "https://sandbox.plaid.com/sandbox/public_token/create";
        private const string AccessTokenURL = "https://sandbox.plaid.com/item/public_token/exchange";
        private readonly IConfiguration _configuration;

        public PlaidController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ActionResult Index()
        {
            try
            {
                var clientId = _configuration["PlaidApi:client_id"];
                var clientSecret = _configuration["PlaidApi:ClientSecret"];
                var publicKey = _configuration["PlaidApi:PublicKey"];

                // Step 1: Generate public token
                var createRequest = new publictokenRequest
                {
                    institution_id = "ins_3",
                    initial_products = new string[] { "Auth" }
                };

                Console.WriteLine(createRequest);
                var createResponse = ExecuteRequest<ResponcePublictoken>(PublicTokenURL, createRequest); // Corrected class name

                // Step 2: Exchange public_token for access_token
                var exchangeRequest = new AccessTokenRequest
                {
                    PublicToken = createResponse.public_token
                };

                var exchangeResponse = ExecuteRequest<ResponceAccessToken>(AccessTokenURL, exchangeRequest); // Corrected class name

                // Step 3: Use the access token
                if (exchangeResponse != null)
                {
                    ViewBag.Token = exchangeResponse.access_token;
                    return View(exchangeResponse); // Pass the entire model to the view
                }
            }
            catch (WebException webex)
            {
                WebResponse errResp = webex.Response;
                using (Stream respStream = errResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream);
                    var result = reader.ReadToEnd();
                }
            }

            return View();
        }

        private TResponse ExecuteRequest<TResponse>(string url, object requestObject)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            Console.WriteLine(url);
            httpWebRequest.Method = "POST";
            httpWebRequest.ContentType = "application/json";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string requestData = JsonConvert.SerializeObject(requestObject);
                streamWriter.Write(requestData);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var responseData = streamReader.ReadToEnd();
                return JsonConvert.DeserializeObject<TResponse>(responseData);
            }
        }
    }
}
