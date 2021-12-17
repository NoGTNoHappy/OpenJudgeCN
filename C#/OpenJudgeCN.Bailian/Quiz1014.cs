using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenJudgeCN.Bailian
{
    public class Quiz1014
    {
        public const string Introduce = @"
        描述
        Marsha and Bill own a collection of marbles. They want to split the collection among themselves
        so that both receive an equal share of the marbles. This would be easy if all the marbles had the same value,
        because then they could just split the collection in half. But unfortunately, some of the marbles are larger,
        or more beautiful than others. So, Marsha and Bill start by assigning a value, a natural number between one and six,
        to each marble. Now they want to divide the marbles so that each of them gets the same total value.
        Unfortunately, they realize that it might be impossible to divide the marbles in this way
        (even if the total value of all marbles is even). For example, if there are one marble of value 1,
        one of value 3 and two of value 4, then they cannot be split into sets of equal value. So, 
        they ask you to write a program that checks whether there is a fair partition of the marbles.

        输入
        Each line in the input file describes one collection of marbles to be divided.
        The lines contain six non-negative integers n1, ..., n6, where ni is the number of marbles of value i.
        So, the example from above would be described by the input-line ""1 0 1 2 0 0"".
        The maximum total number of marbles will be 20000.
        The last line of the input file will be ""0 0 0 0 0 0""; do not process this line.

        输出
        For each collection, output ""Collection #k:"", where k is the number of the test case,
        and then either ""Can be divided."" or ""Can't be divided."".
        Output a blank line after each test case.

        样例输入
        1 0 1 2 0 0 
        1 0 0 0 1 1 
        0 0 0 0 0 0 

        样例输出
        Collection #1:
        Can't be divided.

        Collection #2:
        Can be divided.";

        public static void Test()
        {
            var inputList = new List<string>();
            while (true)
            {
                var input = Console.ReadLine();
                if(input == "0 0 0 0 0 0")
                    break;

                inputList.Add(input);
            }

            Console.WriteLine();

            for (var i = 0; i < inputList.Count; ++i)
            {
                Console.WriteLine($"Collection #{i + 1}:");
                var canDo = DivideJudgment(inputList[i]) ? string.Empty : "'t";
                Console.WriteLine($"Can{canDo} be divided.");
            }
        }

        private static bool DivideJudgment(string input)
        {
            var ballCounts = input.Split(' ').Select(int.Parse).ToArray();
            var totalPoints = 1 * ballCounts[0] + 2 * ballCounts[1] + 3 * ballCounts[2] + 4 * ballCounts[3] +
                              5 * ballCounts[4] + 6 * ballCounts[5];
            if (totalPoints % 2 != 0) return false;
            var halfPoints = totalPoints / 2;
            for (var i = ballCounts.Length - 1; i >= 0; --i)
            {
                halfPoints -= (i + 1) * ballCounts[i];
                if (halfPoints == 0) return true;
                if (halfPoints < 0) return false;
            }

            throw new Exception("Impossible.");
        }
    }
}