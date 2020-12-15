using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using Credfeto.UrlShortener.Shorteners;
using MailKit.Net.Smtp;
using MimeKit;

namespace ItemStockChecker
{
    public class Texter
    {
        #region Private Variables

        protected string _email;

        #endregion

        #region Public Properties

        public Carrier Carrier { get; set; }

        #endregion

        #region Constructor

        public Texter()
        {
        }

        #endregion

        public void Send(string subject, string body)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Steven Scholz", "stevenkurtscholz@gmail.com"));

            message.To.Add(new MailboxAddress("User", _email));

            message.Subject = subject;


            message.Body = new TextPart("plain")
            {
                Text = body
            };

            using var client = new SmtpClient();
            client.Connect("smtp.gmail.com", 465, true);
            client.Authenticate(ConfigurationManager.AppSettings["YourEmail"], ConfigurationManager.AppSettings["AppPassword"]);
            client.Send(message);
            client.Disconnect(true);
        }
    }
}
