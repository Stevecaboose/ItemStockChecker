using System;
using System.IO;
using System.Net;
using System.Threading;

namespace ItemStockChecker.Scrapers
{
    public class AmazonScraper : Scraper
    {
        public AmazonScraper(Uri url) : base(url)
        {
        }

        public override void Scrape()
        {
            while (!FoundItemInStock)
            {
                ConsoleOutputHelper.Write("Checking...", Thread.CurrentThread);
                if (_webScraper == null)
                {
                    throw new NullReferenceException("WebScraper cannot be null");
                }

                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(_url);
                    request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

                    using (var response = request.GetResponse())
                    {
                        var responseStream = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException());
                        Html = responseStream.ReadToEnd();
                    }


                    if (Html.Contains("In stock.", StringComparison.CurrentCultureIgnoreCase) && !Html.Contains("Currently unavailable", StringComparison.CurrentCultureIgnoreCase))
                    {
                        SendSuccessMessage();
                        break;
                    }

                    CanScrape = true;

                    ConsoleOutputHelper.Write("[Not in stock. Trying again...]", ConsoleColor.Red, Thread.CurrentThread);
                }
                catch (Exception e)
                {
                    ConsoleOutputHelper.Write("Amazon", e);

                    CoolDown();
                }

                Thread.Sleep(_waitTime);
            }
        }
    }

}
