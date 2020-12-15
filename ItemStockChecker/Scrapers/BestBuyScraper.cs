using System;
using System.Configuration;
using System.Threading;
using RestSharp;

namespace ItemStockChecker.Scrapers
{
    public class BestBuyScraper : Scraper
    {

        public string BestBuyCookie { get; set; }

        public BestBuyScraper(Uri url) : base(url)
        {
            if (ConfigurationManager.AppSettings != null)
                BestBuyCookie = ConfigurationManager.AppSettings["BestBuyCookie"];
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
                    var client = new RestClient(_url) {Timeout = -1};
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("Cookie", BestBuyCookie);
                    var response = client.Execute(request);

                    Html = response.Content;


                    if (Html.Contains("Add to Cart", StringComparison.CurrentCultureIgnoreCase))
                    {
                        SendSuccessMessage();
                        break;
                    }

                    CanScrape = true;

                    ConsoleOutputHelper.Write("[Not in stock. Trying again...]", ConsoleColor.Red, Thread.CurrentThread);
                }
                catch (Exception e)
                {
                    ConsoleOutputHelper.Write("Best Buy", e);

                    CoolDown();
                }

                Thread.Sleep(_waitTime);
            }
        }
    }
}
