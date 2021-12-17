using System;
using System.Collections.Generic;

namespace OpenJudgeCN.Bailian
{
    public class Quiz1003
    {
        public const string Introduce = @"
        描述
        How far can you make a stack of cards overhang a table? If you have one card, you can create a maximum overhang of half a card length.
        (We're assuming that the cards must be perpendicular to the table.) With two cards you can make the top card overhang the bottom
        one by half a card length, and the bottom one overhang the table by a third of a card length, for a total maximum overhang of
        1/2 + 1/3 = 5/6 card lengths. In general you can make n cards overhang by 1/2 + 1/3 + 1/4 + ... + 1/(n + 1) card lengths,
        where the top card overhangs the second by 1/2, the second overhangs tha third by 1/3, the third overhangs the fourth by 1/4,
        etc., and the bottom card overhangs the table by 1/(n + 1). This is illustrated in the figure below.

        输入
        The input consists of one or more test cases, followed by a line containing the number 0.00 that signals the end of the input.
        Each test case is a single line containing a positive floating-point number c whose value is at least 0.01 and at most 5.20;
        c will contain exactly three digits.

        输出
        For each test case, output the minimum number of cards necessary to achieve an overhang of at least c card lengths.
        Use the exact output format shown in the examples.

        样例输入
        1.00
        3.71
        0.04
        5.19
        0.00

        样例输出
        3 card(s)
        61 card(s)
        1 card(s)
        273 card(s)";

        public static void Test()
        {
            var inputList = new List<float>();
            while (true)
            {
                var input = float.Parse(Console.ReadLine());
                if (Math.Abs(input) > 0)
                    inputList.Add(input);
                else
                    break;
            }

            Console.WriteLine();

            foreach (var input in inputList) 
                Console.WriteLine($"{FindMinimumCardCount(input)} card(s)");
        }

        private static ushort FindMinimumCardCount(float c)
        {
            if (c < 0.5) return 1;
            ushort count = 1;
            var res1 = 0f;
            var res2 = 0.5f;
            while (true)
            {
                res1 += 1f / (count + 1);
                res2 += 1f / (count + 2);
                ++count;
                if (res2 > c)
                {
                    if (res1 <= c)
                        return count;

                    throw new Exception("???");
                }
            }
        }
    }
}