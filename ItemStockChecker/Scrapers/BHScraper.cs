using System;
using System.Threading;

namespace ItemStockChecker.Scrapers
{
    public class BHScraper : Scraper
    {
        public BHScraper(Uri url) : base(url) {}

        #region Public Methods


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
                    Html = _webScraper.Get(_url.AbsoluteUri);

                    if (Html.Contains("<button data-selenium=\"addToCartButton\""))
                    {
                        SendSuccessMessage();
                        break;
                    }

                    CanScrape = true;

                    ConsoleOutputHelper.Write("[Not in stock. Trying again...]", ConsoleColor.Red, Thread.CurrentThread);
                }
                catch (Exception e)
                {
                    ConsoleOutputHelper.Write("B&H", e);

                    CoolDown();
                }

                Thread.Sleep(_waitTime);
            }
        }

        #endregion

    }
}
