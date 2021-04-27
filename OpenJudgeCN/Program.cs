using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using OpenJudgeCN.Bailian;

namespace OpenJudgeCN
{
    class Program
    {
        const string Guidance = "Please input test's ID. -l or --list to get available tests. Input \"Q\" to exist.";
        const string TestNotFound = "{0} is not found in avaliable tests. Input -l or --list to get available tests.\r\n";
        const string RunTestConfirm = "Input \"R\" to run, or \"H\" to get help.";
        const string ContinueAsk = "Continue? Y/N";
        const string Bye = "Bye!";

        static Assembly[] Assemblies;
        static Type[] HasTestTypes;

        static Program()
        {
            _ = StaticReference.Ref;
            Assemblies = AppDomain.CurrentDomain.GetAssemblies();
            HasTestTypes = Assemblies.SelectMany(o => o.GetTypes()).Where(o => o.GetMethod("Test") != null).ToArray();
        }

        static void Main(string[] args)
        {
            var cancel = false;
            while (!cancel)
            {
                try
                {
                    Console.WriteLine(Guidance);
                    while (true)
                    {
                        var cmd = Console.ReadLine();
                
                        switch (cmd)
                        {
                            case "Q":
                            case "q":
                                cancel = true;
                                goto end;
                            case "-l":
                            case "--list":
                            case "-I":
                            case "-1":
                                var sb = new StringBuilder();
                                foreach (var t in GetAvaliableTests())
                                    sb.Append(t + ", ");

                                if (sb.Length >= 2)
                                    sb.Length -= 2;
                                Console.WriteLine(sb.ToString());
                                Console.WriteLine();
                                break;
                            default:
                                RunTest(cmd);
                                break;
                        }

                        Console.WriteLine();
                        Console.WriteLine(Guidance);
                    }
                }
                catch (Exception e)
                {
                    if (e.StackTrace.Contains("Test"))
                        Console.WriteLine("Possibile Invalid Input");
                    else
                        Console.WriteLine(e.ToString());
                }
                Console.WriteLine(ContinueAsk);
                var isContinue = Console.ReadLine();
                cancel = !(isContinue == "Y" || isContinue == "y");
            end:;
            }

            Console.WriteLine(Bye);
            Console.ReadKey();
        }

        static IEnumerable<string> GetAvaliableTests()
        {
            return HasTestTypes.Select(o => o.Name);
        }

        static void RunTest(string name)
        {
            var target = HasTestTypes.FirstOrDefault(o => o.Name == name);
            if(target == null)
            {
                Console.WriteLine(string.Format(TestNotFound, name));
                Console.WriteLine(Guidance);
                return;
            }

            Console.WriteLine(RunTestConfirm);
            var cancel = false;
            while (!cancel)
            {
                switch (Console.ReadLine())
                {
                    case "R":
                    case "r":
                        target.GetMethod("Test").Invoke(null, null);
                        cancel = true;
                        break;
                    case "H":
                    case "h":
                        Console.WriteLine(target.GetField("Introduce").GetValue(null));
                        Console.WriteLine();
                        Console.WriteLine(RunTestConfirm);
                        break;
                    default:
                        Console.WriteLine(RunTestConfirm);
                        break;
                }
            }
        }
    }
}
