using System;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace TwitterStream.Infrastructure
{
    public static class OAuthExtensions
    {

        public static void SignOAuth10(this HttpWebRequest request, 
                                       string accessToken, string accessTokenSecret, 
                                       string consumerKey, string consumerKeySecret) 
        {
            var url = request.RequestUri;
            var authValue = GetAuthenticationOAuth10(url, accessToken, accessTokenSecret, consumerKey, consumerKeySecret);
            request.Headers.Add(HttpRequestHeader.Authorization, authValue);
        }


        private static string GetAuthenticationOAuth10(Uri url, 
                                                      string accessToken, string accessTokenSecret, 
                                                      string consumerKey, string consumerKeySecret)
        {
            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var timestamp = Convert.ToInt64(ts.TotalSeconds).ToString();
            var nonce = Guid.NewGuid().ToString();
            
            var parameters = new[] {
                "oauth_consumer_key="+consumerKey,
                "oauth_nonce="+nonce,
                "oauth_signature_method=HMAC-SHA1",
                "oauth_timestamp="+timestamp,
                "oauth_token="+accessToken,
                "oauth_version=1.0"
            };
            
            var urlParameters = url.Query.TrimStart('?').Split(new[] {'&'}, StringSplitOptions.RemoveEmptyEntries);

            var requestParts = new[] {
                "GET",
                url.GetLeftPart(UriPartial.Path),
                string.Join("&", parameters.Concat(urlParameters).OrderBy(x=>x))
            };

            var stringToSign = string.Join("&", requestParts.Select(x => Uri.EscapeDataString(x)));

            var secretKey = string.Format("{0}&{1}",
                                            Uri.EscapeDataString(consumerKeySecret),
                                            Uri.EscapeDataString(accessTokenSecret));

            var hasher = new HMACSHA1(Encoding.ASCII.GetBytes(secretKey));
            var signature = Convert.ToBase64String(hasher.ComputeHash(Encoding.ASCII.GetBytes(stringToSign)));
            signature = Uri.EscapeDataString(signature);

            return "OAuth " + string.Join(",", parameters) + ",oauth_signature=" + signature;
        }


    }
}
