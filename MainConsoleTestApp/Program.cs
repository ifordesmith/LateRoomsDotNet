using System;
using TestClassLibrary;
namespace MainConsoleTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Running Tests....");
            TestClassLibrary.Tester test = new Tester();
            Console.WriteLine("Press any Key to quit.");
            Console.ReadKey();
        }
    }
}
