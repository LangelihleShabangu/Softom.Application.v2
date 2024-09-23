using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softom.Application.BusinessRules
{
    public static class Configuration
    {
        public static class CultureInfoSettings
        {
            public static CultureInfo GetZACulture()
            {
                CultureInfo zaCulture = new CultureInfo("en-za");
                zaCulture.NumberFormat.CurrencyDecimalSeparator = zaCulture.NumberFormat.NumberDecimalSeparator = ".";
                zaCulture.NumberFormat.CurrencyPositivePattern = zaCulture.NumberFormat.CurrencyNegativePattern = 0; // CurrencyPatterns:  this means the value will be formatted as 'Rn' and not 'R n' (where n is the number), 
                zaCulture.DateTimeFormat.DateSeparator = "/";                                                        // CurrencyPatterns:  refer to this link for more info: https://msdn.microsoft.com/en-us/library/system.globalization.numberformatinfo.currencypositivepattern(v=vs.110).aspx
                zaCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
                return zaCulture;
            }
        }
    }
}
