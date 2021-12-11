using System;
using System.Text;

namespace OpenJudgeCN.Bailian
{
    public class Quiz1012
    {
        public const string Introduce =
      @"描述
        The Joseph's problem is notoriously known. For those who are not familiar with the original problem:
        from among n people, numbered 1, 2, . . ., n, standing in circle every mth is going to be executed
        and only the life of the last remaining person will be saved. Joseph was smart enough to choose
        the position of the last remaining person, thus saving his life to give us the message about the incident.
        For example when n = 6 and m = 5 then the people will be executed in the order 5, 4, 6, 2, 3 and 1 will be saved.
        Suppose that there are k good guys and k bad guys. In the circle the first k are good guys and the last k bad guys.
        You have to determine such minimal m that all the bad guys will be executed before the first good guy.
        
        输入
        The input file consists of separate lines containing k. The last line in the input file contains 0.
        You can suppose that 0 < k < 14.

        输出
        The output file will consist of separate lines containing m corresponding to k in the input file.

        样例输入
        3
        4
        0

        样例输出
        5
        30";

        // Total count
        private static int _n;
        // m th
        private static int _m;

        public static void Test()
        {
            var res = new StringBuilder();
            while (true)
            {
                var k = int.Parse(Console.ReadLine());
                if (k == 0) break;
                res.AppendLine(MinimumM(k).ToString());
            }

            Console.WriteLine();
            Console.WriteLine(res.ToString());
        }

        private static int MinimumM(int k)
        {
            _n = 2 * k;
            _m = 1;
            while (true)
            {
                var i = 1;
                for (; i <= k; ++i)
                {
                    var killedNo = KilledNo(i);
                    // Good guy was killed late enough
                    if (killedNo >= k) continue;
                    // Good guy was killed too early, try next
                    ++_m;
                    break;
                }

                // If exit from loop without break, then m is the result
                if (i > k) return _m;
            }
        }

        private static int KilledNo(int i)
        {
            // f(0) = 0
            // f(i) = (f(i-1) + m - 1) % (n + 1 - i)
            if (i == 0) return 0;
            return (KilledNo(i - 1) + _m - 1) % (_n + 1 - i);
        }
    }
}