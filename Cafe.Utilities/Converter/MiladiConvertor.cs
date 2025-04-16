using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Utilities.Converter
{
    public static class MiladiConvertor
    {
        public static DateTime ToMiladi(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, new PersianCalendar());
        }
    }
}
