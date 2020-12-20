using System;
using System.IO;
using System.Net;
using System.Threading;
using HtmlAgilityPack;

namespace ItemStockChecker.Scrapers
{
    public class AmazonScraper : Scraper
    {
        public AmazonScraper(Uri url) : base(url)
        {
            StoreName = "Amazon";
            HtmlDocument doc = _web.Load(url);
            ProductName = doc.GetElementbyId("productTitle")?.InnerText.Trim();
        }

        public override void Scrape()
        {
            while (!FoundItemInStock)
            {
                ConsoleOutputHelper.Write($"{StoreName} {ProductName} Checking...", Thread.CurrentThread);

                try
                {
                    Html = GetHtml();

                    if (Html.Contains("In stock.", StringComparison.CurrentCultureIgnoreCase) && !Html.Contains("Currently unavailable", StringComparison.CurrentCultureIgnoreCase))
                    {
                        SendSuccessMessage();
                        break;
                    }

                    CanScrape = true;

                    ConsoleOutputHelper.Write($"{StoreName} {ProductName} [Not in stock. Trying again...]", ConsoleColor.Red, Thread.CurrentThread);
                }
                catch (Exception e)
                {
                    ConsoleOutputHelper.Write(StoreName, e);

                    CoolDown();
                }

                Thread.Sleep(_waitTime);
            }
        }

        public override string GetHtml()
        {
            var request = (HttpWebRequest)WebRequest.Create(_url);
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;

            using var response = request.GetResponse();
            var responseStream = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException());
            var html = responseStream.ReadToEnd();

            return html;
        }
    }

}
