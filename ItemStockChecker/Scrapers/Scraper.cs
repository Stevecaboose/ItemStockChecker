using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using HtmlAgilityPack;

namespace ItemStockChecker.Scrapers
{
    public abstract class Scraper
    {
        #region Private Variables

        protected Uri _url;
        protected int _waitTime;
        protected List<Texter> _texterList;
        private int _cooldownTime;
        private int _currentCoolDownTime;
        protected HtmlWeb _web;


        #endregion

        #region Public Properties

        public string Html { get; set; }

        public bool FoundItemInStock { get; set; } = false;

        public bool CanScrape { get; set; } = false;

        public string ProductName { get; set; }

        public string StoreName { get; set; }

        #endregion

        #region Constructor

        protected Scraper(Uri url)
        {
            _texterList = new List<Texter>();
            _url = url;
            _waitTime = int.Parse(ConfigurationManager.AppSettings["PageRefreshTime"] ?? string.Empty);
            _cooldownTime = int.Parse(ConfigurationManager.AppSettings["CoolDownTime"] ?? "30");
            _currentCoolDownTime = _cooldownTime;
            _web = new HtmlWeb();


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
                // If this is a new timer, it will be at 0 and we need to then set it up
                if (_currentCoolDownTime == 0)
                {
                    _currentCoolDownTime = _cooldownTime * 2;
                    _cooldownTime = _currentCoolDownTime;
                }
            }

            CanScrape = false;

            ConsoleOutputHelper.Write($"{StoreName} {ProductName} is cooling down for {_cooldownTime} minutes...", Thread.CurrentThread);

            for (int i = _cooldownTime; i >= 0; i--)
            {
                ConsoleOutputHelper.Write($"{StoreName} {ProductName} is cooling down for {i} minutes...", Thread.CurrentThread);
                _currentCoolDownTime = i;
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

        public virtual string GetHtml()
        {
            HtmlDocument doc = _web.Load(_url);
            return doc.Text;
        }

        public virtual HtmlDocument GetHtmlDocument()
        {
            return _web.Load(_url);
        }

        #endregion
    }
}
