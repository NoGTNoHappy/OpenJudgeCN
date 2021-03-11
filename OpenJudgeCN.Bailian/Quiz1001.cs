using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenJudgeCN.Bailian
{
    public class Quiz1001
    {
        public static void Test()
        {
            var input = Console.ReadLine();
            var strParams = input.Split(" ");
            var n = Convert.ToByte(strParams[1]);
            var res = strParams[0];
            for (var i = 1; i < n; ++i)
                res = Multiple(res, strParams[0]);

            Console.WriteLine(res);
        }

        private static string Multiple(string a, string b)
        {
            var aParts = a.Split(".");
            var bParts = b.Split(".");

            var aExp = aParts[1].Length;
            var bExp = bParts[1].Length;
            var totalExp = -aExp - bExp;

            var aNew = aParts[0] + aParts[1];
            var bNew = bParts[0] + bParts[1];

            var res = "0";
            for (var i = 0; i < aNew.Length; ++i)
            {
                var aChar = aNew[i];
                var tmp = MultipleIntegers(aChar, bNew);
                tmp += new string('0', aNew.Length - i - 1);
                res = AddIntegers(res, tmp);
            }

            var realExp = res.Length + totalExp;
            if (realExp > 0)
            {
                res = res.Insert(realExp, ".");
            }
            else
            {
                var prefix = "0." + new string('0', -realExp);
                res = res.Insert(0, prefix);
            }
            return res;
        }

        private static string MultipleIntegers(char c, string integer)
        {
            var cByte = Convert.ToByte(c.ToString());
            var res = "0";
            for (var i = 0; i < integer.Length; ++i)
            {
                var cCurrent= integer[i];
                var tmp = Convert.ToByte(cCurrent.ToString()) * cByte;
                var tmpStr = tmp + new string('0', integer.Length - i - 1);
                res = AddIntegers(res, tmpStr);
            }
            return res;
        }

        private static string AddIntegers(string a, string b)
        {
            var maxLength = a.Length > b.Length ? a.Length : b.Length;
            a = a.PadLeft(maxLength, '0');
            b = b.PadLeft(maxLength, '0');

            var addTenFlag = false;
            var res = new StringBuilder();
            for (var i = maxLength - 1; i >= 0; --i)
            {
                var tmp = Convert.ToByte(a[i].ToString()) + Convert.ToByte(b[i].ToString());
                if (addTenFlag)
                {
                    ++tmp;
                    addTenFlag = false;
                }

                if (tmp >= 10)
                {
                    tmp -= 10;
                    addTenFlag = true;
                }

                res.Insert(0, tmp);
            }

            if (addTenFlag)
                res.Insert(0, "1");

            return res.ToString();
        }
    }
}
