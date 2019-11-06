using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using zOrder.OMS.Models;
using zOrder.OMS.Helper;
using System.Web.Http.Results;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Extensions;
using System.Web;

namespace zOrder.OMS.Controllers
{
    public class LoginController : ApiController
    {
        public string twitter_consumer_key = System.Configuration.ConfigurationManager.AppSettings["TwitterKey"];
        public string twitter_consumer_secret = System.Configuration.ConfigurationManager.AppSettings["TwitterSecretKey"];

        // GET: api/Login
        [HttpGet, HttpPost]
        [ActionName("userAuthentication")]
        public JsonResult<ReturnValue> userAuthentication(string username,string password)
        {
            ReturnValue ret = new ReturnValue();
            var token = Guid.NewGuid();

            using (var db = new zOrderEntities())
            {
                try
                {
                    var user = db.Users.Where(x => x.Mail == username && x.Password == password && x.IsDeleted == false)
                        .Select(x => new { Mail = x.Mail, Password = x.Password, IsDeleted = x.IsDeleted }).First();
                                    
                    if (user != null)
                    {
                        ret.retObject = new { token = token };
                        ret.success = true;
                        ret.message = "Giriş başarılı";
                    }
                    else
                    {
                        ret.success = false;
                        ret.message = "Giriş başarısız";
                    }

                }
                catch (Exception ex)
                {
                    ret.success = false;
                    ret.error = ex.ToString();
                }
            }
            return Json(ret);
        }

        public string GetRequestToken(string key, string secret, string callBackUrl)
        {
            var client = new RestClient("https://api.twitter.com");
            client.Authenticator = OAuth1Authenticator.ForRequestToken(key, secret, callBackUrl);

            var request = new RestRequest("/oauth/request_token", Method.POST);
            var response = client.Execute(request);

            var qs = HttpUtility.ParseQueryString(response.Content);

            string oauthToken = qs["oauth_token"];
            string oauthTokenSecret = qs["oauth_token_secret"];

            request = new RestRequest("oauth/authorize?oauth_token=" + oauthToken);

            string url = client.BuildUri(request).ToString();
            return url;
        }

        public string GetAccessToken(string key, string secret, string otoken, string otokensecret, string overifier)
        {
            var client = new RestClient("https://api.twitter.com");
            var request = new RestRequest("/oauth/access_token", Method.POST);

            client.Authenticator = OAuth1Authenticator.ForAccessToken(key, secret, otoken, otokensecret, overifier);

            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var qs = HttpUtility.ParseQueryString(response.Content);
                //we have token

                //Todo: Save user detail in database
                string oauthToken = qs["oauth_token"];
                string oauthTokenSecret = qs["oauth_token_secret"];
                string userId = qs["user_id"];
                string screenName = qs["screen_name"];
                string xAuthExpires = qs["x_auth_expires"];

                return screenName;
            }
            else
            {
                return "Error";
            }

        }


        [HttpGet, HttpPost]
        [ActionName("TwitterAuth")]
        public JsonResult<ReturnValue> TwitterAuth()
        {
            ReturnValue ret = new ReturnValue();

            try
            {
                // localhost | https://127.0.0.1/Login/TwitterAccess  
                // app | https://zutters.github.io/Login/TwitterAccess 
                // ngrok | http://b08b7735.ngrok.io/Login/TwitterAccess

                var url = GetRequestToken(twitter_consumer_key, twitter_consumer_secret, "http://845d0264.ngrok.io/api/Login/TwitterAccess"); 

                ret.retObject = url;
                ret.success = true;
                ret.message = "TwitterAuth";
            }
            catch (Exception ex)
            {
                ret.success = false;
                ret.error = ex.Message;
            }

            return Json(ret);
        }

        [HttpGet, HttpPost]
        [ActionName("TwitterAccess")]
        public JsonResult<ReturnValue> TwitterAccess(string oauth_token,string oauth_verifier)
        {
            ReturnValue ret = new ReturnValue();

            try
            {
                var data = GetAccessToken(twitter_consumer_key, twitter_consumer_secret, oauth_token, "", oauth_verifier); 

                ret.retObject = data;
                ret.success = true;
                ret.message = "TwitterAccess";
            }
            catch (Exception ex)
            {
                ret.success = false;
                ret.error = ex.Message;
            }

            return Json(ret);
        }

        //Todo : Get user tweets
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


    }
}
