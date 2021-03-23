using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenJudgeCN.Bailian
{
    public class Quiz1007
    {
        /*
        描述
        现在有一些长度相等的DNA串（只由ACGT四个字母组成），请将它们按照逆序对的数量多少排序。
        逆序对指的是字符串A中的两个字符A[i]、A[j]，具有i < j 且 A[i] > A[j] 的性质。如字符串”ATCG“中，T和C是一个逆序对，
        T和G是另一个逆序对，这个字符串的逆序对数为2。

        输入
        第1行：两个整数n和m，n(0<n<=50)表示字符串长度，m(0<m<=100)表示字符串数量
        第2至m+1行：每行是一个长度为n的字符串

        输出
        按逆序对数从少到多输出字符串，逆序对数一样多的字符串按照输入的顺序输出。

        样例输入
        10 6
        AACATGAAGG
        TTTTGGCCAA
        TTTGGCCAAA
        GATCAGATTT
        CCCGGGGGGA
        ATCGATGCAT

        样例输出
        CCCGGGGGGA
        AACATGAAGG
        GATCAGATTT
        ATCGATGCAT
        TTTTGGCCAA
        TTTGGCCAAA
        */

        public static void Test()
        {
            var lengthAndCount = Console.ReadLine().Split(" ");
            var length = int.Parse(lengthAndCount[0]);
            var count = int.Parse(lengthAndCount[1]);
            var dict = new Dictionary<string, int>();
            for (var i = 0; i < count; ++i)
                dict.Add(Console.ReadLine(), i);

            foreach (var pair in dict.ToArray())
                dict[pair.Key] = pair.Value + CalNegSeq(pair.Key.ToCharArray()) * 100;

            Console.WriteLine();

            foreach (var pair in dict.OrderBy(o => o.Value))
                Console.WriteLine(pair.Key);
        }

        private static int CalNegSeq(char[] arr)
        {
            var count = 0;
            var mid = arr.Length / 2;
            var left = arr[..mid];
            var right = arr[mid..];
            if (left.Length == 1 && right.Length == 1)
            {
                if (left[0] > right[0])
                    ++count;
            }
            else if (left.Length == 1 && right.Length == 2)
            {
                if (left[0] > right[0])
                    ++count;
                if (left[0] > right[1])
                    ++count;
                if (right[0] > right[1])
                    ++count;
            }
            else
            {
                count += CalNegSeq(left);
                count += CalNegSeq(right);
                left = left.OrderBy(o => o).ToArray();
                right = right.OrderBy(o => o).ToArray();
                count += CalNegSeqMerge(left, right);
            }

            return count;
        }

        private static int CalNegSeqMerge(char[] left, char[] right)
        {
            var count = 0;
            foreach (var rc in right)
                for (var l = 0; l < left.Length; ++l)
                {
                    var lc = left[l];
                    if (rc < lc)
                    {
                        count += left.Length - l;
                        break;
                    }
                }

            return count;
        }
    }
}