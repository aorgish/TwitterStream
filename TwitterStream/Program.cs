using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using TwitterStream.Infrastructure;
using System.IO.Compression;

namespace TwitterStream
{
    class Program
    {
        static void Main(string[] args)
        {

            //string accessToken = ConfigurationManager.AppSettings["token.accessToken"];
            //string accessTokenSecret = ConfigurationManager.AppSettings["token.accessTokenSecret"];
            //string consumerKey = ConfigurationManager.AppSettings["token.consumerKey"];
            //string consumerSecret = ConfigurationManager.AppSettings["token.consumerSecret"];
            //var r = WebRequest.CreateHttp("https://api.twitter.com/1.1/statuses/user_timeline.json?include_entities=true&include_rts=false&trim_user=true&screen_name=twitterapi&count=3000");
            //r.SignOAuth10(accessToken, accessTokenSecret, consumerKey, consumerSecret);
            //var resp = r.GetResponse();
            //using (var stream = resp.GetResponseStream())
            //using (var reader = new StreamReader(stream))
            //{
            //    var data = reader.ReadToEnd();
            //    Console.WriteLine(data);
            //    Console.ReadLine();
            //}
            //return;


            //using(var file = File.OpenRead(@"C:\Work\tweeterStream\20140112_065851.gz"))
            //using(var gz = new GZipStream(file,CompressionMode.Decompress))
            //using (var fout = File.OpenWrite(@"C:\Work\tweeterStream\20140112_065851.txt"))
            //{
            //    gz.CopyTo(fout);
            //}
            //return;

            var isCancelled = false;
            Console.CancelKeyPress += (sender, ev) => isCancelled = true;

            while (!isCancelled)
            try { 
                using(var twitterStream = new TwitterStreamReader())
                using(var writer = new FileLineWriter())
                foreach (var twit in twitterStream.Twits())
                {
                    writer.WriteLine(twit);
                    if (isCancelled) break;
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
         }

    }
}
