using System;
using System.Threading;
using HtmlAgilityPack;

namespace ItemStockChecker.Scrapers
{
    public class NewEggScraper : Scraper
    {
        public NewEggScraper(Uri url) : base(url)
        {
            StoreName = "B&H";
            HtmlDocument doc = _web.Load(url);
            ProductName = doc.QuerySelector("h1[class='product-title']")?.InnerText.Trim();
        }

        public override void Scrape()
        {
            while (!FoundItemInStock)
            {
                ConsoleOutputHelper.Write($"{StoreName} {ProductName} Checking...", Thread.CurrentThread);

                try
                {
                    Html = GetHtml();

                    if (Html.Contains("add to cart", StringComparison.CurrentCultureIgnoreCase))
                    {
                        SendSuccessMessage();
                        break;
                    }

                    ConsoleOutputHelper.Write($"{StoreName} {ProductName} [Not in stock. Trying again...]", ConsoleColor.Red, Thread.CurrentThread);

                    CanScrape = true;
                }
                catch (Exception e)
                {
                    ConsoleOutputHelper.Write(StoreName, e);
                    CoolDown();
                }

                Thread.Sleep(_waitTime);
            }
        }
    }
}
