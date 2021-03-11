using System;

namespace OpenJudgeCN.Bailian
{
    public class Quiz1000
    {
        public static void Test()
        {
            var input = Console.ReadLine();
            var strParams = input.Split(" ");
            var a = Convert.ToByte(strParams[0]);
            var b = Convert.ToByte(strParams[1]);
            var res = a + b;
            Console.WriteLine(res);
        }
    }
}
