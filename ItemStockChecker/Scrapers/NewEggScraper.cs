using System;
using System.Threading;

namespace ItemStockChecker.Scrapers
{
    public class NewEggScraper : Scraper
    {
        public NewEggScraper(Uri url) : base(url)
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
                    Html = _webScraper.Get(_url.AbsoluteUri);

                    if (Html.Contains("add to cart", StringComparison.CurrentCultureIgnoreCase))
                    {
                        SendSuccessMessage();
                        break;
                    }

                    ConsoleOutputHelper.Write("[Not in stock. Trying again...]", ConsoleColor.Red, Thread.CurrentThread);

                    CanScrape = true;
                }
                catch (Exception e)
                {
                    ConsoleOutputHelper.Write("New Egg", e);

                    CoolDown();
                }

                Thread.Sleep(_waitTime);
            }
        }
    }
}
