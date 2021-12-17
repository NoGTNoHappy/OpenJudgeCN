using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenJudgeCN.Bailian
{
    public class Quiz1013
    {
        public const string Introduce = @"
        描述
        Sally Jones has a dozen Voyageur silver dollars. However, only eleven of the coins are true silver dollars;
        one coin is counterfeit even though its color and size make it indistinguishable from the real silver dollars.
        The counterfeit coin has a different weight from the other coins but Sally does not know if it is heavier or lighter than the real coins.
        Happily, Sally has a friend who loans her a very accurate balance scale.
        The friend will permit Sally three weighings to find the counterfeit coin.
        For instance, if Sally weighs two coins against each other and the scales balance then she knows these two coins are true.
        Now if Sally weighs one of the true coins against a third coin and the scales do not balance
        then Sally knows the third coin is counterfeit and she can tell whether it is light or heavy depending on
        whether the balance on which it is placed goes up or down, respectively.
        By choosing her weighings carefully, Sally is able to ensure that she will find the counterfeit coin with exactly three weighings.

        输入
        The first line of input is an integer n (n > 0) specifying the number of cases to follow.
        Each case consists of three lines of input, one for each weighing. Sally has identified each of the coins with the letters A--L.
        Information on a weighing will be given by two strings of letters and then one of the words ``up'', ``down'', or ``even''.
        The first string of letters will represent the coins on the left balance; the second string, the coins on the right balance.
        (Sally will always place the same number of coins on the right balance as on the left balance.)
        The word in the third position will tell whether the right side of the balance goes up, down, or remains even.

        输出
        For each case, the output will identify the counterfeit coin by its letter and tell whether it is heavy or light.
        The solution will always be uniquely determined.

        样例输入
        1 
        ABCD EFGH even 
        ABCI EFJK up 
        ABIJ EFGH even 

        样例输出
        K is the counterfeit coin and it is light.";

        public static void Test()
        {
            var count = int.Parse(Console.ReadLine());
            var res = new StringBuilder();
            for (var i = 0; i < count; ++i)
            {
                var first = Console.ReadLine();
                var second = Console.ReadLine();
                var third = Console.ReadLine();
                res.AppendLine(FindCounterfeitCoin(first, second, third));
            }

            Console.WriteLine();
            Console.WriteLine(res.ToString());
        }

        private static string FindCounterfeitCoin(string first, string second, string third)
        {
            var trueCoins = new List<char>();
            var falseCoin = char.MinValue;
            var firstTest = first.Split(" ");
            var secondTest = second.Split(" ");
            var thirdTest = third.Split(" ");
            FindTrueCoins(firstTest, trueCoins);
            FindTrueCoins(secondTest, trueCoins);
            FindTrueCoins(thirdTest, trueCoins);

            var resultFlag = FindHeavyFalseCoins(firstTest, trueCoins, ref falseCoin) ??
                         FindHeavyFalseCoins(secondTest, trueCoins, ref falseCoin) ??
                         FindHeavyFalseCoins(thirdTest, trueCoins, ref falseCoin);

            if (!resultFlag.HasValue)
                throw new Exception("Impossible");

            var result = resultFlag.Value ? "heavy" : "light";
            return $"{falseCoin} is the counterfeit coin and it is {result}.";
        }

        private static void FindTrueCoins(string[] test, List<char> list)
        {
            if (test[2] != "even") return;
            list.AddRange(test[0].ToCharArray());
            list.AddRange(test[1].ToCharArray());
        }

        private static bool? FindHeavyFalseCoins(string[] test, List<char> list, ref char falseCoin)
        {
            if (test[2] == "even") return null;
            foreach (var c in test[0])
                if (!list.Contains(c))
                    falseCoin = c;

            bool fromLeft;
            if (falseCoin == char.MinValue)
            {
                fromLeft = false;
                foreach (var c in test[1])
                    if (!list.Contains(c))
                        falseCoin = c;
            }
            else
                fromLeft = true;

            if (falseCoin == char.MinValue) return null;
            return test[2] == "up" ? fromLeft : !fromLeft;
        }
    }
}
