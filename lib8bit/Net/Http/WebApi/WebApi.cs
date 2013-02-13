using EightBitIdeas.Lib8bit.Net.Http.WebApi.ApiResponse;
using LitJson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace EightBitIdeas.Lib8bit.Net.Http.WebApi
{
    public class WebApi
    {
        public static readonly string WebApiRootUrl = "http://8-bitideas.com/api/";

        public IWebApiResponse Login(string username, string password)
        {
			Dictionary<string, string> options = new Dictionary<string, string>();
			options.Add("username", username);
            options.Add("password", password);
			
            return GetResponseObject<LoginResponse>("user/auth", "POST", null, options);
        }

        public IWebApiResponse CreateUser(string username, string password)
        {
			Dictionary<string, string> options = new Dictionary<string, string>();
			options.Add("username", username);
            options.Add("password", password);
			
            return GetResponseObject<LoginResponse>("user/create", "POST", null, options);
        }

        public IWebApiResponse GetResponseObject<T>(string relativeUrl, string method, Dictionary<string, string> headers, Dictionary<string, string> formResults)  where T : IWebApiResponse, new()
        {
            if (headers == null)
                headers = new Dictionary<string, string>();

            if (headers.ContainsKey("Content-Type"))
                headers.Remove("Content-Type");

            headers.Add("Content-Type", "application/x-www-form-urlencoded");

            string body = "";
            foreach (KeyValuePair<string, string> kvp in formResults)
            {
                body += string.Format("{0}={1}&", HttpUtility.UrlEncode(kvp.Key), HttpUtility.UrlEncode(kvp.Value));
            }

            body = body.TrimEnd('&');
            return GetResponseObject<T>(relativeUrl, method, headers, body);
        }

        public IWebApiResponse GetResponseObject<T>(string relativeUrl, string method, Dictionary<string, string> headers, string body)  where T : IWebApiResponse, new()
        {
            string trimmedRelative = relativeUrl.Trim('/') + '/';

            Uri loginUri = new Uri(WebApiRootUrl + trimmedRelative);
            HttpClient client = new HttpClient(loginUri);

            if (!client.IsError())
            {
                T login = new T();
                IWebApiResponse webApiResponse = login as IWebApiResponse;

                webApiResponse.FromJsonData(client.GetJsonData());

                return login;
                
            }

            return client.GetError();
        }
    }
}
