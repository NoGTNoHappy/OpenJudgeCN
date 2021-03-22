using System;

namespace OpenJudgeCN.Bailian
{
    public class Quiz1000
    {
        /*
        描述
        Calculate a + b
        
        输入
        Two integer a,,b (0 ≤ a,b ≤ 10)

        输出
        Output a + b

        样例输入
        1 2

        样例输出
        3
        */
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
