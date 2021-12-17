using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenJudgeCN.Bailian
{
    public class Quiz1010
    {
        public const string Introduce = @"
        描述
        Have you done any Philately lately?
        You have been hired by the Ruritanian Postal Service (RPS) to design their new postage software.
        The software allocates stamps to customers based on customer needs and the denominations that are currently in stock.
        Ruritania is filled with people who correspond with stamp collectors. As a service to these people,
        the RPS asks that all stamp allocations have the maximum number of different types of stamps in it.
        In fact, the RPS has been known to issue several stamps of the same denomination in order to please customers
        (these count as different types, even though they are the same denomination).
        The maximum number of different types of stamps issued at any time is twenty-five.
        To save money, the RPS would like to issue as few duplicate stamps as possible
        (given the constraint that they want to issue as many different types). Further, the RPS won't sell more than four stamps at a time.

        输入
        The input for your program will be pairs of positive integer sequences, consisting of two lines, alternating until end-of-file.
        The first sequence are the available values of stamps, while the second sequence is a series of customer requests. For example:
        1 2 3 0     ; three different stamp types
        7 4 0       ; two customers
        1 1 0       ; a new set of stamps (two of the same type)
        6 2 3 0     ; three customers
        Note: the comments in this example are *not* part of the data file; data files contain only integers.

        输出
        For each customer, you should print the ""best"" combination that is exactly equal to the customer's needs, with a maximum of four stamps.
        If no such combination exists, print ""none"".
        The ""best"" combination is defined as the maximum number of different stamp types.
        In case of a tie, the combination with the fewest total stamps is best. If still tied, the set with the highest single-value stamp is best.
        If there is still a tie, print ""tie"".
        For the sample input file, the output should be:
        7 (3): 1 1 2 3
        4 (2): 1 3
        6 ---- none
        2 (2): 1 1
        3 (2): tie
        That is, you should print the customer request, the number of types sold and the actual stamps. In case of no legal allocation,
        the line should look like it does in the example, with four hyphens after a space.
        In the case of a tie, still print the number of types but do not print the allocation
        (again, as in the example).Don't print extra blank at the end of each line.

        样例输入
        1 2 3 0     ; three different stamp types
        7 4 0       ; two customers
        1 1 0       ; a new set of stamps (two of the same type)
        6 2 3 0     ; three customers

        样例输出
        7 (3): 1 1 2 3 
        4 (2): 1 3 
        6 ---- none
        2 (2): 1 1
        3 (2): tie";

        public static void Test()
        {
            var result = new StringBuilder();
            while (true)
            {
                var stamps = Console.ReadLine();
                if (string.IsNullOrEmpty(stamps)) break;
                var requires = Console.ReadLine();
                result.Append(GetBestCombinations(stamps, requires));
            }

            Console.WriteLine();

            Console.WriteLine(result.ToString());
        }

        private static string GetBestCombinations(string stampsStr, string requiresStr)
        {
            var stamps = stampsStr.Split(" ")[..^1].Select((o, i) => new MyStamp(i, Convert.ToInt32(o))).ToArray();
            var requires = requiresStr.Split(" ")[..^1].Select(o => Convert.ToInt32(o)).ToArray();
            var resStr = new StringBuilder();
            foreach (var r in requires)
            {
                var possible = new HashSet<List<MyStamp>>(MyListEqualityComparer.Instance);
                var p1 = new List<MyStamp>();
                var p2 = new List<MyStamp>();
                var p3 = new List<MyStamp>();
                var p4 = new List<MyStamp>();
                GetPossibleCombination(stamps, r, 1, p1, possible);
                GetPossibleCombination(stamps, r, 2, p2, possible);
                GetPossibleCombination(stamps, r, 3, p3, possible);
                GetPossibleCombination(stamps, r, 4, p4, possible);

                if (possible.Count == 0)
                {
                    resStr.AppendLine($"{r} ---- none");
                    continue;
                }

                if (possible.Count == 1)
                {
                    var res = possible.First();
                    resStr.AppendLine(Output(r, res));
                    continue;
                }

                var arr = possible.Select(o => Tuple.Create(o, o.Distinct(MyStampEqualityComparer.Instance).Count()))
                    .ToArray();
                var best1Tmp = arr.OrderByDescending(o => o.Item2).ToArray();
                var best1 = best1Tmp.Where(o => o.Item2 == best1Tmp[0].Item2).ToArray();
                if (best1.Length == 1)
                {
                    resStr.AppendLine(Output(r, best1[0].Item1));
                    continue;
                }

                var best2Tmp = best1.Select(o => Tuple.Create(o.Item1, o.Item1.Count)).OrderBy(o => o.Item2).ToArray();
                var best2 = best2Tmp.Where(o => o.Item2 == best2Tmp[0].Item2).ToArray();
                if (best2.Length == 1)
                {
                    resStr.AppendLine(Output(r, best2[0].Item1));
                    continue;
                }

                var best3Tmp = best2.Select(o => Tuple.Create(o.Item1, o.Item1.Max())).OrderByDescending(o => o.Item2)
                    .ToArray();
                var best3 = best3Tmp.Where(o => o.Item2 == best3Tmp[0].Item2).ToArray();
                if (best3.Length == 1)
                {
                    resStr.AppendLine(Output(r, best3[0].Item1));
                    continue;
                }

                resStr.AppendLine($"{r} ({best3[0].Item1.Distinct(MyStampEqualityComparer.Instance).Count()}): tie");
            }

            return resStr.ToString();
        }

        private static void GetPossibleCombination(MyStamp[] stamps, int require, int count, List<MyStamp> result,
            HashSet<List<MyStamp>> possible)
        {
            if (count == 0) return;
            if (count == 1)
            {
                var res = stamps.Where(o => o.Val == require).ToArray();
                if (res.Length > 0)
                    foreach (var r in res)
                    {
                        var resTmp = new List<MyStamp>(result) {r};
                        possible.Add(resTmp);
                    }

                return;
            }

            foreach (var stamp in stamps)
            {
                var left = require - stamp.Val;
                if (left < 0) continue;
                var resTmp = new List<MyStamp>(result) {stamp};
                GetPossibleCombination(stamps, left, count - 1, resTmp, possible);
            }
        }

        private static string Output(int require, List<MyStamp> resList)
        {
            var res = new StringBuilder();
            res.Append($"{require} ({resList.Distinct(MyStampEqualityComparer.Instance).Count()}):");
            foreach (var re in resList.OrderBy(o => o))
                res.Append(" " + re);

            return res.ToString();
        }

        private struct MyStamp : IComparable<MyStamp>
        {
            public int No { get; }
            public int Val { get; }

            public MyStamp(int no, int val)
            {
                No = no;
                Val = val;
            }

            public static bool operator ==(MyStamp a, MyStamp b)
            {
                return a.No == b.No && a.Val == b.Val;
            }

            public static bool operator !=(MyStamp a, MyStamp b)
            {
                return !(a == b);
            }

            public int CompareTo(MyStamp other)
            {
                return Val.CompareTo(other.Val);
            }

            public override int GetHashCode()
            {
                return No.ToString().GetHashCode() ^ Val.ToString().GetHashCode();
            }

            public override bool Equals(object obj)
            {
                return obj is MyStamp ms && this == ms;
            }

            public override string ToString()
            {
                return Val.ToString();
            }
        }

        private class MyStampEqualityComparer : IEqualityComparer<MyStamp>
        {
            private MyStampEqualityComparer()
            {
            }

            public static MyStampEqualityComparer Instance => new MyStampEqualityComparer();

            public bool Equals(MyStamp x, MyStamp y)
            {
                if (x == y) return true;
                return x.No == y.No;
            }

            public int GetHashCode(MyStamp obj)
            {
                return obj.No;
            }
        }

        private class MyListEqualityComparer : IEqualityComparer<List<MyStamp>>
        {
            private MyListEqualityComparer()
            {
            }

            public static MyListEqualityComparer Instance => new MyListEqualityComparer();

            public bool Equals(List<MyStamp> x, List<MyStamp> y)
            {
                if (x == y) return true;
                if (x == null || y == null) return false;
                if (x.Count != y.Count) return false;
                var orderedX = x.OrderBy(o => o.No).ToArray();
                var orderedY = y.OrderBy(o => o.No).ToArray();
                for (var i = 0; i < orderedX.Length; ++i)
                    if (orderedX[i].No != orderedY[i].No)
                        return false;

                return true;
            }

            public int GetHashCode(List<MyStamp> obj)
            {
                return obj.Count + obj.Sum(o => o.GetHashCode());
            }
        }
    }
}