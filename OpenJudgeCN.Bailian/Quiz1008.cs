using System;
using System.Collections.Generic;

namespace OpenJudgeCN.Bailian
{
    public class Quiz1008
    {
        /*
        描述
        During his last sabbatical, professor M. A. Ya made a surprising discovery about the old Maya calendar.
        From an old knotted message, professor discovered that the Maya civilization used a 365 day long year,
        called Haab, which had 19 months. Each of the first 18 months was 20 days long, and the names of the months
        were pop, no, zip, zotz, tzec, xul, yoxkin, mol, chen, yax, zac, ceh, mac, kankin, muan, pax, koyab, cumhu.
        Instead of having names, the days of the months were denoted by numbers starting from 0 to 19. The last month of Haab
        was called uayet and had 5 days denoted by numbers 0, 1, 2, 3, 4. The Maya believed that this month was unlucky,
        the court of justice was not in session, the trade stopped, people did not even sweep the floor. For religious purposes,
        the Maya used another calendar in which the year was called Tzolkin (holly year). The year was divided into thirteen periods,
        each 20 days long. Each day was denoted by a pair consisting of a number and the name of the day. They used 20 names: imix, ik,
        akbal, kan, chicchan, cimi, manik, lamat, muluk, ok, chuen, eb, ben, ix, mem, cib, caban, eznab, canac, ahau and 13 numbers;
        both in cycles. Notice that each day has an unambiguous description. For example, at the beginning of the year the days
        were described as follows: 1 imix, 2 ik, 3 akbal, 4 kan, 5 chicchan, 6 cimi, 7 manik, 8 lamat, 9 muluk, 10 ok, 11 chuen,
        12 eb, 13 ben, 1 ix, 2 mem, 3 cib, 4 caban, 5 eznab, 6 canac, 7 ahau, and again in the next period 8 imix, 9 ik, 10 akbal ...
        Years (both Haab and Tzolkin) were denoted by numbers 0, 1, ... , where the number 0 was the beginning of the world.
        Thus, the first day was: Haab: 0. pop 0 Tzolkin:1 imix 0 Help professor M. A. Ya and write a program for him to convert
        the dates from the Haab calendar to the Tzolkin calendar.

        输入
        The date in Haab is given in the following format:
        NumberOfTheDay. Month Year
        The first line of the input file contains the number of the input dates in the file. The next n lines contain n dates
        in the Haab calendar format, each in separate line. The year is smaller then 5000.
        输出
        The date in Tzolkin should be in the following format:
        Number NameOfTheDay Year
        The first line of the output file contains the number of the output dates. In the next n lines, there are dates in the Tzolkin
        calendar format, in the order corresponding to the input dates.

        样例输入
        3
        10. zac 0
        0. pop 0
        10. zac 1995

        样例输出
        3
        3 chuen 0
        1 imix 0
        9 cimi 2801
        */

        public static void Test()
        {
            var count = int.Parse(Console.ReadLine());
            var inputList = new List<string>();
            for (var i = 0; i < count; ++i)
                inputList.Add(Console.ReadLine());

            Console.WriteLine();

            Console.WriteLine(count);
            foreach (var input in inputList)
                Console.WriteLine(ConvertMayaCalendar(input));
        }

        private static string ConvertMayaCalendar(string str)
        {
            var haab = Haab.Parse(str);
            var tzolkin = haab.ToTzolkin();
            return tzolkin.ToString();
        }

        private struct Haab
        {
            private const int NormalMonthCount = 18;
            private const int NormalMonthDays = 20;
            private const int IrregularMonthCount = 1;
            private const int IrregularMonthDays = 5;

            private const int OneYearDays =
                NormalMonthCount * NormalMonthDays + IrregularMonthCount * IrregularMonthDays;

            private static readonly List<string> MonthNames = new List<string>
            {
                "pop", "no", "zip", "zotz", "tzec", "xul", "yoxkin", "mol", "chen", "yax", "zac", "ceh", "mac",
                "kankin", "muan", "pax", "koyab", "cumhu", "uayet"
            };

            private readonly ulong _totalDays;

            public Haab(ulong days)
            {
                _totalDays = days;
            }

            public static Haab Parse(string str)
            {
                try
                {
                    var splits = str.Split(" ");
                    var day = ulong.Parse(splits[0].Replace(".", "")) + 1;
                    var month = Convert.ToUInt64(MonthNames.IndexOf(splits[1]) + 1);
                    var year = ulong.Parse(splits[2]);

                    var totalDays = year * OneYearDays + (month - 1) * NormalMonthDays + day;
                    return new Haab(totalDays);
                }
                catch (Exception e)
                {
                    throw new Exception("Can't parse to Haab.");
                }
            }

            public Tzolkin ToTzolkin()
            {
                return new Tzolkin(_totalDays);
            }
        }

        private struct Tzolkin
        {
            private const int NormalMonthCount = 13;
            private const int NormalMonthDays = 20;
            private const int OneYearDays = NormalMonthCount * NormalMonthDays;

            private static readonly List<string> DayNames = new List<string>
            {
                "imix", "ik", "akbal", "kan", "chichan", "cimi", "manik", "lamat", "muluk", "ok", "chuen", "eb", "ben",
                "ix", "mem", "cib", "caban", "eznab", "canca", "ahau"
            };

            private readonly ulong _totalDays;

            public Tzolkin(ulong days)
            {
                _totalDays = days;
            }

            public override string ToString()
            {
                if (_totalDays < 1)
                    return "Invalid Tzolkin.";

                var year = _totalDays / OneYearDays;
                var monthAndDays = Convert.ToInt32(_totalDays % OneYearDays);
                var month = monthAndDays % NormalMonthCount;
                var day = monthAndDays % NormalMonthDays;

                return $"{month} {DayNames[day - 1]} {year}";
            }
        }
    }
}