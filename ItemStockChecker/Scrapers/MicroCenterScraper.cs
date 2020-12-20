using System;
using System.IO;
using System.Net;
using System.Threading;
using HtmlAgilityPack;
using static System.Configuration.ConfigurationManager;

namespace ItemStockChecker.Scrapers
{
    public class MicroCenterScraper : Scraper
    {
        public MicroCenterScraper(Uri url) : base(url)
        {
            StoreName = "Micro Center";
            HtmlDocument doc = _web.Load(url);
            ProductName = doc.QuerySelector("span[class^='ProductLink']")?.InnerText.Trim();
        }

        #region Public Methods

        public override void Scrape()
        {
            while (!FoundItemInStock)
            {
                ConsoleOutputHelper.Write($"{StoreName} {ProductName} Checking...", Thread.CurrentThread);

                try
                {
                    Html = GetHtml();

                    if (!Html.Contains("sold out", StringComparison.CurrentCultureIgnoreCase))
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

            var microCenterStoreId = AppSettings["MicroCenterStore"] ?? "045";
            request.TryAddCookie(new Cookie("storeSelected", microCenterStoreId, "/", _url.Host));

            using var response = request.GetResponse();
            var responseStream = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException());
            return responseStream.ReadToEnd();
        }

        #endregion

    }
}
