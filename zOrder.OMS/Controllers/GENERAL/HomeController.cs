using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Extensions;
using zOrder.OMS.Helper;
using zOrder.OMS.Models;

namespace zOrder.OMS.Controllers
{
    public class HomeController : ApiController
    {
        public string twitter_consumer_key = System.Configuration.ConfigurationManager.AppSettings["TwitterConsumerKey"];
        public string twitter_consumer_secret = System.Configuration.ConfigurationManager.AppSettings["TwitterConsumerSecretKey"];
        public string twitter_token_key = System.Configuration.ConfigurationManager.AppSettings["TwitterTokenKey"];
        public string twitter_token_secret = System.Configuration.ConfigurationManager.AppSettings["TwitterTokenSecretKey"];

        [HttpGet, HttpPost]
        [ActionName("UpdateStatus")]
        public JsonResult<ReturnValue> UpdateStatus()
        {
            ReturnValue ret = new ReturnValue();
            try
            {
                var client = new RestClient("https://api.twitter.com");
                var request = new RestRequest("/1.1/statuses/update.json", Method.POST);

                //var jsonContent = Request.Content.ReadAsFormDataAsync().Result;
                var allParam = HttpContext.Current.Request.Form.AllKeys;
                var getParam = HttpContext.Current.Request.Params["data"];
                var formData = new JavaScriptSerializer().Serialize(HttpContext.Current.Request.Params["data"]);

                request.AddQueryParameter("status", getParam);
                client.Authenticator = OAuth1Authenticator.ForProtectedResource(twitter_consumer_key, twitter_consumer_secret, twitter_token_key, twitter_token_secret);
                var response = client.Execute(request);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    ret.retObject = response;
                    ret.success = true;
                    ret.message = "Tweet atildi";
                }
                else
                {
                    ret.success = false;
                    ret.message = "Hata!";
                }

            }
            catch (Exception ex)
            {
                ret.success = false;
                ret.error = ex.Message;
            }

            return Json(ret);
        }

        //Todo : Get user tweets
        [HttpGet, HttpPost]
        [ActionName("GetTweetList")]
        public JsonResult<ReturnValue> GetTweetList()
        {
            ReturnValue ret = new ReturnValue();

            try
            {
                var client = new HttpClient();

                ret.retObject = "";
                ret.success = true;
                ret.message = "Twitter";
            }
            catch (Exception ex)
            {
                ret.success = false;
                ret.error = ex.Message;
            }

            return Json(ret);
        }

        private T GetPostParam<T>(string key)
        {
            var p = Request.Content.ReadAsAsync<JObject>();
            return (T)Convert.ChangeType(p.Result[key], typeof(T)); // example conversion, could be null...
        }

        public class MFormData
        {
            public string Name { get; set; }
            public int Age { get; set; }
            public string Xyz { get; set; }
            public string ImageUrl { get; set; }
        }

    }
}
