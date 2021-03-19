using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenJudgeCN.Bailian
{
    public class Quiz1002
    {
        private static readonly Dictionary<char, char> ConvertDict = new Dictionary<char, char>
        {
            {'A', '2'}, {'B', '2'}, {'C', '2'},
            {'D', '3'}, {'E', '3'}, {'F', '3'},
            {'G', '4'}, {'H', '4'}, {'I', '4'},
            {'J', '5'}, {'K', '5'}, {'L', '5'},
            {'M', '6'}, {'N', '6'}, {'O', '6'},
            {'P', '7'}, {'R', '7'}, {'S', '7'},
            {'T', '8'}, {'U', '8'}, {'V', '8'},
            {'W', '9'}, {'X', '9'}, {'Y', '9'}
        };

        public static void Test()
        {
            var count = int.Parse(Console.ReadLine());
            var inputList = new List<string>(count);
            for (var i = 0; i < count; ++i)
                inputList.Add(Console.ReadLine());

            for (var i = 0; i < inputList.Count; ++i)
                inputList[i] = ConvertToPhoneNumber(inputList[i]);

            var set = new HashSet<string>();
            var dict = new Dictionary<string, int>();
            foreach (var input in inputList)
            {
                if (set.Add(input))
                    dict.Add(input, 1);
                else
                    ++dict[input];
            }

            Console.WriteLine();

            if (dict.Count == inputList.Count)
                Console.WriteLine("No duplicates.");
            else
            {
                foreach (var pair in dict.Where(o => o.Value > 1).OrderBy(o => o.Key))
                {
                    Console.WriteLine($"{pair.Key.Insert(3, "-")} {pair.Value}");
                }
            }
        }

        private static string ConvertToPhoneNumber(string str)
        {
            str = str.Replace("-", "");
            var res = new StringBuilder(7);
            for (var i = 0; i < 7; ++i)
            {
                res.Append(ConvertDict.TryGetValue(str[i], out var converted) ? converted : str[i]);
            }

            return res.ToString();
        }
    }
}
