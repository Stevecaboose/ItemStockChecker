using System;
using System.IO;
using System.Net;
using System.Threading;
using static System.Configuration.ConfigurationManager;

namespace ItemStockChecker.Scrapers
{
    public class MicroCenterScraper : Scraper
    {
        public MicroCenterScraper(Uri url) : base(url)
        {
        }

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
                    var request = (HttpWebRequest)WebRequest.Create(_url);

                    var microCenterStoreId = AppSettings["MicroCenterStore"] ?? "045";
                    request.TryAddCookie(new Cookie("storeSelected", microCenterStoreId, "/", _url.Host));

                    using (var response = request.GetResponse())
                    {
                        var responseStream = new StreamReader(response.GetResponseStream() ?? throw new InvalidOperationException());
                        Html = responseStream.ReadToEnd();
                    }

                    if (!Html.Contains("sold out", StringComparison.CurrentCultureIgnoreCase))
                    {
                        SendSuccessMessage();
                        break;
                    }

                    CanScrape = true;

                    ConsoleOutputHelper.Write("[Not in stock. Trying again...]", ConsoleColor.Red, Thread.CurrentThread);
                }
                catch (Exception e)
                {
                    ConsoleOutputHelper.Write("Micro Center", e);

                    CoolDown();
                }

                Thread.Sleep(_waitTime);
            }
        }

        #endregion

    }
}
