using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
