using System;
using System.Linq;
using System.Text;

namespace OpenJudgeCN.Bailian
{
    public class Quiz1011
    {
        public const string Introduce = @"
        描述
        George took sticks of the same length and cut them randomly until all parts became at most 50 units long.
        Now he wants to return sticks to the original state, but he forgot how many sticks he had originally and how long they were originally.
        Please help him and design a program which computes the smallest possible original length of those sticks.
        All lengths expressed in units are integers greater than zero.

        输入
        The input contains blocks of 2 lines. The first line contains the number of sticks parts after cutting,
        there are at most 64 sticks. The second line contains the lengths of those parts separated by the space.
        The last line of the file contains zero.

        输出
        The output should contains the smallest possible length of original sticks, one per line.

        样例输入
        9
        5 2 1 5 2 1 5 2 1
        4
        1 2 3 4
        0

        样例输出
        6
        5";

        public static void Test()
        {
            var res = new StringBuilder();
            while (true)
            {
                var count = int.Parse(Console.ReadLine());
                if (count == 0) break;
                var inputArr = Console.ReadLine().Split(" ").Select(o => new Stick(int.Parse(o))).ToArray();
                res.AppendLine(GetSmallestPossibleLength(inputArr).ToString());
            }

            Console.WriteLine();
            Console.WriteLine(res.ToString());
        }

        private static int GetSmallestPossibleLength(Stick[] arr)
        {
            if (arr.Length == 1) return arr[0].Length;
            arr = arr.OrderByDescending(o => o.Length).ToArray();
            var totalLength = arr.Select(o => o.Length).Sum();
            var possibleLength = arr.Select(o => o.Length).Max();
            while (totalLength > possibleLength)
            {
                if (totalLength % possibleLength == 0)
                    if (ValidateLength(arr, possibleLength))
                        return possibleLength;

                ++possibleLength;
            }

            throw new Exception("Impossible");
        }

        private static bool ValidateLength(Stick[] arr, int length)
        {
            while (arr.Count(o => !o.Used) > 0)
            {
                var sum = 0;
                var index = 0;
                Stick current = null;
                for (; index < arr.Length; ++index)
                {
                    if (arr[index].Used) continue;
                    current = arr[index];
                    break;
                }

                if (current == null)
                    throw new Exception("Impossible");

                current.Used = true;
                sum += current.Length;
                var index2 = index + 1;
                for (; index2 < arr.Length; ++index2)
                {
                    if (arr[index2].Used) continue;
                    if (arr[index2].Length + sum > length)
                    {
                        FindNextDifferentStickIndex(arr, ref index2);
                        if (index2 == arr.Length - 1) break;
                        --index2;
                        continue;
                    }

                    arr[index2].Used = true;
                    sum += arr[index2].Length;
                }

                if (sum == length) continue;
                // back to base state
                foreach (var stick in arr.Where(o => o.Used))
                    stick.Used = false;

                return false;
            }

            return true;
        }

        private static void FindNextDifferentStickIndex(Stick[] arr, ref int index)
        {
            var current = arr[index];
            while (index < arr.Length - 1 && arr[index].Length == current.Length)
                ++index;
        }

        private class Stick
        {
            public Stick(int length)
            {
                Length = length;
                Used = false;
            }

            public int Length { get; }
            public bool Used { get; set; }
        }
    }
}