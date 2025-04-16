using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Utilities.Converter
{
    public static class ShamsiConverter
    {
        public static string ToShamsi(this DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();
            return $"{pc.GetYear(date)}/{pc.GetMonth(date).ToString("00")}/{pc.GetDayOfMonth(date).ToString("00")}  --  {pc.GetHour(date).ToString("00")}:{pc.GetMinute(date).ToString("00")}:{pc.GetSecond(date).ToString("00")}";
        }
    }
}
