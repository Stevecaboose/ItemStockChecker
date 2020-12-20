using System;
using System.Threading;
using HtmlAgilityPack;

namespace ItemStockChecker.Scrapers
{
    public sealed class BHScraper : Scraper
    {
        public BHScraper(Uri url) : base(url)
        {
            StoreName = "B&H";
            HtmlDocument doc = _web.Load(url);
            ProductName = doc.QuerySelector("h1[data-selenium='productTitle']")?.InnerText.Trim();
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

                    if (Html.Contains("<button data-selenium=\"addToCartButton\""))
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
