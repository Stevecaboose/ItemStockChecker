using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
