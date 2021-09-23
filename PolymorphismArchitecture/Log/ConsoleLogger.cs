using System;
using DzNet6;

namespace PolymorphismArchitecture.Log
{
    class ConsoleLogger : ILog
    {
        public void Print(string line)
        {
            Console.WriteLine(line);
        }
    }
}
