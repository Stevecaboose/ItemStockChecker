using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ItemStockChecker.Scrapers;

namespace ItemStockChecker
{
    public static class Loader
    {
        public static Thread CreateThread(Uri url)
        {
            // determine the domain

            Scraper scraper = null;
            Thread thread = null;

            switch (url.Host)
            {

                case "www.bhphotovideo.com":
                {
                    scraper = new BHScraper(url);
                    break;
                }

                case "www.newegg.com":
                {
                    scraper = new NewEggScraper(url);
                    break;
                }

                case "www.amazon.com":
                {
                    scraper = new AmazonScraper(url);
                    break;
                }

                case "www.bestbuy.com":
                {
                    scraper = new BestBuyScraper(url);
                    break;
                }

                case "www.microcenter.com":
                {
                    scraper = new MicroCenterScraper(url);
                    break;
                }

                default: throw new ArgumentException("Could not find scraper for url: " + url.Host);
            }

            try
            {
                thread = new Thread(scraper.Scrape);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return thread;
        }
    }
}
