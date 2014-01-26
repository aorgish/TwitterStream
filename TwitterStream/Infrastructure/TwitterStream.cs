using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;

namespace TwitterStream.Infrastructure
{
    public class TwitterStreamReader : IDisposable
    {
        static string accessToken = ConfigurationManager.AppSettings["token.accessToken"];
        static string accessTokenSecret = ConfigurationManager.AppSettings["token.accessTokenSecret"];
        static string consumerKey = ConfigurationManager.AppSettings["token.consumerKey"];
        static string consumerSecret = ConfigurationManager.AppSettings["token.consumerSecret"];

        private StreamReader reader;
        private Stream stream;
        private WebResponse response;

        public IEnumerable<string> Twits() {
            response = GetResponse();
            stream = response.GetResponseStream();
            reader = new StreamReader(stream, Encoding.UTF8);
            
            while(true) {
               yield return reader.ReadLine();
            }
        }

        private WebResponse GetResponse() {
            var request = WebRequest.CreateHttp("https://stream.twitter.com/1.1/statuses/sample.json");
            request.KeepAlive = false;
            request.Headers[HttpRequestHeader.AcceptEncoding] = "gzip"; // https://blog.twitter.com/2012/announcing-gzip-compression-streaming-apis
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.SignOAuth10(accessToken, accessTokenSecret, consumerKey, consumerSecret);
            Console.WriteLine("Connecting to twitter stream...");
            return request.GetResponse();
        }

        public void Close() {
            if (reader != null) reader.Close();
            if (stream != null) stream.Close();
            if (response != null) response.Close();

            reader = null;
            stream = null;
            response = null;
        } 

        void IDisposable.Dispose() {
            Close();
        }
    }
}
