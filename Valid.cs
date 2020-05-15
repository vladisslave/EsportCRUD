using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EsportMVC
{
    public class Valid
    {
        public static bool IsAllLetters(string s)
        {
            foreach (char c in s)
            {
                if (!Char.IsLetter(c) && c != ' ')
                    return false;
            }
            return true;
        }
        public static bool Datecheck(object value)
        {
            var starttime = new DateTime(1950, 1, 1);
            var endtime = DateTime.Now;
            var dt = (DateTime)value;
            if (dt >= starttime && dt <= endtime)
            {
                return true;
            }
            return false;
        }
        public class CurrentDateAttribute : ValidationAttribute
        {
            
            public CurrentDateAttribute()
            {
            }

            public override bool IsValid(object value)
            {
                var starttime = new DateTime(1950, 1, 1);
                var endtime = new DateTime(2005, 1, 1);
                var dt = (DateTime)value;
                if (dt >= starttime && dt < endtime)
                {
                    return true;
                }
                return false;
            }
        }
        public class Isletter : ValidationAttribute
        {

            public Isletter()
            {
            }

            public override bool IsValid(object value)
            {
                var str = (string)value;

                return IsAllLetters(str);
            }
        }

    }
}
