using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MwProject.Helpers
{
    public static class StringHelper
    {
        public static string StripText(this string val)
        {
            string input = val.Trim().ToLower();
            string tmp = input.Replace("ą", "a");
            tmp = tmp.Replace("ć", "c");
            tmp = tmp.Replace("ę", "e");
            tmp = tmp.Replace("ł", "l");
            tmp = tmp.Replace("ń", "n");
            tmp = tmp.Replace("ó", "o");
            tmp = tmp.Replace("ś", "s");
            tmp = tmp.Replace("ź", "z");
            tmp = tmp.Replace("ż", "z");
            string output = tmp.Replace(" ", "-");
            return output;
        }

        public static string AddBr(this string val)
        {
            if (val==null)
            {
                return string.Empty;
            }
            string tmp = val.Replace(System.Environment.NewLine, "<br />");
            return tmp;
        }
    }
}
