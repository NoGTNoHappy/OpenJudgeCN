using System;
using OpenJudgeCN.Bailian;

namespace OpenJudgeCN
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    Quiz1001.Test();
                }
                catch (Exception e)
                {
                    break;
                }
            }
        }
    }
}
