using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _website_mp.Classes
{
    public static class Functions
    {
        public static string FormatPrice(string price)
        {
            string rs = "";
            int c = 0;
            for(var i=price.Length-1; i>=0; i--)
            {
                if(c%3==0 && c != 0)
                {
                    rs += ",";
                }
                rs += price[i];
                c++;
            }
            char[] rev = rs.ToCharArray();
            Array.Reverse(rev);
            return new string(rev);
        }
    }
}
