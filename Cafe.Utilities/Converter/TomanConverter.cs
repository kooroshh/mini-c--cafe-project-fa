
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Utilities.Converter
{
    public static class TomanConverter
    {
        public static string ToToman(this int price)
        {
            return price.ToString("#,## تومان");
        }
    }
}
