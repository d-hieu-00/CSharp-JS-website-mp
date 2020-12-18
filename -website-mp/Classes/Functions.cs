using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public static byte[] StringToByteArray(string input)
        {
            char[] temp = input.ToCharArray();
            List<byte> result = new List<byte>();

            foreach (char i in temp)
            {
                result.Add(Convert.ToByte(i));
            }
            return result.ToArray();
        }
    }
}
