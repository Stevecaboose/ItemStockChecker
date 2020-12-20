using System;
using System.Configuration;

namespace ItemStockChecker
{
    public class VerizonTexter : Texter
    {
        public VerizonTexter() : base()
        {
            Carrier = Carrier.VERIZON;

            _email = String.Format(
                ConfigurationManager.AppSettings["VerizonFormat"] ?? throw new InvalidOperationException(),
                ConfigurationManager.AppSettings["VerizonPhoneNumber"]);
        }
    }
}
