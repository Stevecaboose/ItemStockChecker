﻿using System;
using System.Threading;
using HtmlAgilityPack;

namespace ItemStockChecker.Scrapers
{
    public class BestBuyScraper : Scraper
    {
        public BestBuyScraper(Uri url) : base(url)
        {
            StoreName = "Best Buy";
            HtmlDocument doc = _web.Load(url);
            ProductName = doc.QuerySelector("div[class='sku-title'] > h1")?.InnerText.Trim();
        }

        public override void Scrape()
        {
            while (!FoundItemInStock)
            {
                ConsoleOutputHelper.Write($"{StoreName} {ProductName} Checking...", Thread.CurrentThread);

                try
                {
                    var doc = GetHtmlDocument();
                    Html = doc.Text;

                    var addToCartButton = doc.QuerySelector("button[class*='add-to-cart-button']")?.InnerText.Trim()
                        .ToLower();

                    if (Html.Contains("Add to Cart", StringComparison.CurrentCultureIgnoreCase) && !String.IsNullOrEmpty(addToCartButton))
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
    }
}
