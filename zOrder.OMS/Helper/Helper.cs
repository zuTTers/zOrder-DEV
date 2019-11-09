using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using RestSharp.Extensions;
using System.Web;
using System.Net.Http.Headers;
using System.Web.Routing;


namespace zOrder.OMS.Helper
{
    public class ReturnValue
    {
        public string error;        //Hata mesajı içeriği
        public string message;      //Server taraflı işlem sonu dönen mesaj içeriği
        public bool requiredLogin;  //Session kontrolü
        public object retObject;    //İşlem sonu dönen data objesi
        public bool success;        //İşlem başarı kontrolü

        public ReturnValue() { }
    }

    public class query
    {
        public string filter { get; set; }
        public string limit { get; set; }
        public string order { get; set; }
        public int page { get; set; }
        public int count { get; set; }
    }


    public static class Shared
    {
        //Oturum kontrolu yapar.
        public static bool CheckSession()
        {
            if (HttpContext.Current.Session["UserId"] == null)
            {
                return false;
            }
            return true;
        }

        //public HttpResponseMessage RedirectApiUrl(string controller, string action)
        //{
        //    var newUrl = this.Url.Link("DefaultApi", new
        //    {
        //        Controller = controller,
        //        Action = action
        //    });
        //    return Request.CreateResponse(HttpStatusCode.OK,
        //                                              new { Success = true, RedirectUrl = newUrl });
        //}

        //public HttpResponseMessage GetGoogle()
        //{
        //    var response = Request.CreateResponse(HttpStatusCode.Found);
        //    response.Headers.Location = new Uri("http://www.google.com");
        //    return response;
        //}
    }
}