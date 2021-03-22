using System;
using System.Collections.Generic;
using System.Text;

namespace OpenJudgeCN.Bailian
{
    public class Quiz1001
    {
        /*
        描述
        Problems involving the computation of exact values of very large magnitude and precision are common.
        For example, the computation of the national debt is a taxing experience for many computer systems.
        
        This problem requires that you write a program to compute the exact value of R^n
        where R is a real number ( 0.0 < R < 99.999 ) and n is an integer such that 0 < n <= 25.
        
        输入
        The input will consist of a set of pairs of values for R and n.
        The R value will occupy columns 1 through 6, and the n value will be in columns 8 and 9.

        输出
        The output will consist of one line for each line of input giving the exact value of R^n.
        Leading zeros should be suppressed in the output. Insignificant trailing zeros must not be printed.
        Don't print the decimal point if the result is an integer.

        样例输入
        95.123 12
        0.4321 20
        5.1234 15
        6.7592  9
        98.999 10
        1.0100 12

        样例输出
        548815620517731830194541.899025343415715973535967221869852721
        .00000005148554641076956121994511276767154838481760200726351203835429763013462401
        43992025569.928573701266488041146654993318703707511666295476720493953024
        29448126.764121021618164430206909037173276672
        90429072743629540498.107596019456651774561044010001
        1.126825030131969720661201
        */

        public static void Test()
        {
            var inputList = new List<string>();
            while (true)
            {
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                    break;
                inputList.Add(input);
            }

            foreach (var input in inputList)
            {
                var strParams = input.Split(" ");
                var n = Convert.ToByte(strParams[1]);
                var res = strParams[0];
                for (var i = 1; i < n; ++i)
                    res = Multiple(res, strParams[0]);


                Console.WriteLine(FinishResult(res));
            }
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
                var cCurrent = integer[i];
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

        private static string FinishResult(string res)
        {
            if (res.IndexOf('.') != -1)
            {
                var tmp = res.Split('.');
                var tmpBefore = tmp[0];
                tmpBefore = tmpBefore.TrimStart('0');
                if (tmpBefore.Length == 0)
                    tmpBefore = tmpBefore.Insert(0, "0");

                res = tmpBefore + "." + tmp[1];
            }

            res = res.TrimEnd('0');
            return res;
        }
    }
}