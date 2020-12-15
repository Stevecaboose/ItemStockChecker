using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using HTML_DOM_Parser;

namespace ItemStockChecker.Scrapers
{
    public abstract class Scraper
    {
        #region Private Variables

        protected Uri _url;
        protected WebScraper _webScraper;
        protected int _waitTime;
        protected List<Texter> _texterList;
        private int _cooldown;
        private int _cooldownTimer;


        #endregion

        #region Public Properties

        public string Html { get; set; }

        public bool FoundItemInStock { get; set; } = false;

        public bool CanScrape { get; set; } = false;

        #endregion

        #region Constructor

        protected Scraper(Uri url)
        {
            _texterList = new List<Texter>();
            _url = url;
            _webScraper = new WebScraper();
            _waitTime = int.Parse(ConfigurationManager.AppSettings["PageRefreshTime"] ?? string.Empty);
            _cooldown = int.Parse(ConfigurationManager.AppSettings["CoolDownTime"] ?? "30");
            _cooldownTimer = _cooldown;

            if (bool.Parse(ConfigurationManager.AppSettings["TMobileEnabled"] ?? throw new InvalidOperationException()))
            {
                _texterList.Add(new TMobileTexter());
            }

            if (bool.Parse(ConfigurationManager.AppSettings["VerizonEnabled"] ?? throw new InvalidOperationException()))
            {
                _texterList.Add(new VerizonTexter());
            }

        }

        #endregion

        #region Protected Methods

        protected void SendSuccessMessage()
        {
            // We found an item in stock so lets send a text
            ConsoleOutputHelper.Write("[Found item in stock]: " + _url.AbsoluteUri, ConsoleColor.Green, Thread.CurrentThread);
            ConsoleOutputHelper.Write("Sending text...", Thread.CurrentThread);
            FoundItemInStock = true;
            SendText();
            ConsoleOutputHelper.Write("Sent", Thread.CurrentThread);
        }

        protected void CoolDown()
        {
            // If we threw another exception after the cool down is at 0 that means we need to wait longer
            if (!CanScrape)
            {
                _cooldownTimer = _cooldown * 2;
                _cooldown = _cooldownTimer;
            }
            else
            {
                _cooldown = int.Parse(ConfigurationManager.AppSettings["CoolDownTime"] ?? "30");
                _cooldownTimer = _cooldown;
            }

            CanScrape = false;

            ConsoleOutputHelper.Write($"Cooling down for {_cooldownTimer} minutes...", Thread.CurrentThread);

            for (int i = _cooldownTimer; i > 0; i--)
            {
                ConsoleOutputHelper.Write($"Cooldown for {i} minutes...", Thread.CurrentThread);
                Thread.Sleep(new TimeSpan(0, 1, 0));
            }
            
        }

        #endregion

        #region Public Methods

        public void SendText()
        {
            try
            {
                foreach (var texter in _texterList)
                {
                    // T-Mobile blocks best buy links sent via text
                    if (_url.Host.Contains("bestbuy", StringComparison.CurrentCultureIgnoreCase) && texter.Carrier == Carrier.TMOBILE)
                    {
                        string lhs = _url.AbsoluteUri.Substring(0, (int) _url.AbsoluteUri.Length / 2);
                        string rhs = _url.AbsoluteUri.Substring((int) (_url.AbsoluteUri.Length / 2),
                            (int) (_url.AbsoluteUri.Length / 2));

                        texter.Send("Found Item in stock", rhs + lhs);
                    }
                    else
                    {
                        texter.Send("Found item in stock!", _url.AbsoluteUri);
                    }

                }

            }
            catch (Exception e)
            {
                ConsoleOutputHelper.Write(e);
            }
        }

        public abstract void Scrape();

        #endregion
    }
}
