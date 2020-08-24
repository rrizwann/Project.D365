using System;
using System.Net;
using System.Text;

namespace Risika.D365.Core.Common.Helpers
{
    public class RestClient
    {
        private readonly string _baseUrl;
        private readonly string _baseLanguage;
        private readonly string _accessToken;

        public RestClient(string baseUrl, string accessToken, string baseLanguage = null)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                | SecurityProtocolType.Tls11
                | SecurityProtocolType.Tls12;

            this._baseUrl = baseUrl;
            this._baseLanguage = baseLanguage;
            this._accessToken = accessToken;
        }

        public T Send<T>(Uri uri, string method, string data = null, string token = null)
        {
            var request = HttpWebRequest.CreateHttp(new Uri(new Uri(this._baseUrl), uri));
            request.Method = method;
            request.ContentType = "application/json";
            if (!string.IsNullOrEmpty(this._baseLanguage))
            {
                request.Headers["Accept-Language"] = $"{this._baseLanguage}";
            } 

            if (string.IsNullOrEmpty(token))
            {
                request.Headers["Authorization"] = $"{this._accessToken}";
            }
            else
            {
                request.Headers["Authorization"] = $"{token}";
            }

            if (method == "POST" || method == "PUT")
            {
                var content = Encoding.UTF8.GetBytes(data);
                request.ContentLength = content.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(content, 0, content.Length);
                }
            }

            HttpWebResponse response = null;

            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException error)
             {
                try
                {
                    return SerializerClient.ReadObject<T>(error.Response.GetResponseStream());
                }
                catch
                {
                    throw error;
                }

                throw error;
            }

            if (response?.StatusCode == HttpStatusCode.OK)
            {
                return SerializerClient.ReadObject<T>(response.GetResponseStream());
            }

            return default(T);
        }
    }
}
