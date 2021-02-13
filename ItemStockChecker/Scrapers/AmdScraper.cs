using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace ItemStockChecker.Scrapers
{
    public class AmdScraper : Scraper
    {
        public AmdScraper(Uri url) : base(url)
        {
            StoreName = "AMD";
            HtmlDocument doc = _web.Load(url);
            ProductName = doc.QuerySelector("h1[class='page-title']")?.InnerText.Trim();
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

                    if (!Html.Contains("<p class=\"product-out-of-stock\">Out of stock</p>"))
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

        #endregion
    }
}
