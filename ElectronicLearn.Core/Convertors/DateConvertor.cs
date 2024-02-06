using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace ElectronicLearn.Core.Convertors
{
    public static class DateConvertor
    {
        public static string ToShamsi(this DateTime date)
        {
            var persian = new PersianCalendar();

            return $"{persian.GetYear(date)}/{persian.GetMonth(date).ToString("00")}/{persian.GetDayOfMonth(date).ToString("00")}";
        }

        public static string ToLongShamsi(this DateTime date)
        {
            var persian = new PersianCalendar();
            var shamsiDate = $"{persian.GetYear(date)}/{persian.GetMonth(date).ToString("00")}/{persian.GetDayOfMonth(date).ToString("00")}";
            var time = date.ToString("hh:mm:ss tt", new CultureInfo("Fa-ir"));

            return $"{shamsiDate} - {time}";
        }
    }
}
