using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using Asakabank.IdentityApi.Models;

namespace Asakabank.IdentityApi.Helpers {
    public static class ApiClient {
        public static async Task<UserAuthResponse> UserAuth(string uri, UserCred userCred) {
            var handler = new HttpClientHandler {
                ServerCertificateCustomValidationCallback = ServerCertificateCustomValidation
            };
            var client = new HttpClient(handler) {Timeout = TimeSpan.FromSeconds(30)};
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.PostAsJsonAsync(new Uri(uri), userCred);
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
                return await JsonSerializer.DeserializeAsync<UserAuthResponse>(
                    await response.Content.ReadAsStreamAsync());
            return null;

            //var data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(userCred));
            //var webRequest = (HttpWebRequest)WebRequest.Create(uri);
            //webRequest.Method = "POST";
            //webRequest.Accept = "application/json";
            //webRequest.ContentType = "application/json; charset=UTF-8";
            //webRequest.ContentLength = data.Length;

            //ServicePointManager.Expect100Continue = true;
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
            //                                       | SecurityProtocolType.Tls11
            //                                       | SecurityProtocolType.Tls12;

            //ServicePointManager.ServerCertificateValidationCallback =
            //    (ss, certificate, chain, sslPolicyErrors) => true;

            //var newStream = webRequest.GetRequestStream();
            //newStream.Write(data, 0, data.Length);
            //newStream.Close();
            //var response = webRequest.GetResponse();
            //var responseStream = response.GetResponseStream();

            //if (responseStream == null) return null;
            ////var sr = new StreamReader(responseStream);
            ////var result = sr.ReadToEnd();
            //return await JsonSerializer.DeserializeAsync<UserAuthResponse>(responseStream);
        }

        private static bool ServerCertificateCustomValidation(HttpRequestMessage requestMessage,
            X509Certificate2 certificate, X509Chain chain, SslPolicyErrors sslErrors) {
            //// It is possible inpect the certificate provided by server
            //Console.WriteLine($"Requested URI: {requestMessage.RequestUri}");
            //Console.WriteLine($"Effective date: {certificate.GetEffectiveDateString()}");
            //Console.WriteLine($"Exp date: {certificate.GetExpirationDateString()}");
            //Console.WriteLine($"Issuer: {certificate.Issuer}");
            //Console.WriteLine($"Subject: {certificate.Subject}");

            //// Based on the custom logic it is possible to decide whether the client considers certificate valid or not
            //Console.WriteLine($"Errors: {sslErrors}");
            return true;//sslErrors == SslPolicyErrors.None;
        }
    }
}