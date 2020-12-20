using System;
using System.Configuration;

namespace ItemStockChecker
{
    public class TMobileTexter : Texter
    {
        public TMobileTexter() : base()
        {
            Carrier = Carrier.TMOBILE;

            _email = String.Format(
                ConfigurationManager.AppSettings["TMobileFormat"] ?? throw new InvalidOperationException(),
                ConfigurationManager.AppSettings["TMobilePhoneNumber"]);
        }
    }
}
