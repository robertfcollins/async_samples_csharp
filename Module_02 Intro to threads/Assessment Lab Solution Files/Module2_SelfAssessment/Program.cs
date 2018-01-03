using System;

namespace Module2_SelfAssessment
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start");
            var manager = new ParallelMethodManager();
            manager.Execute();
            Console.ReadKey();
        }
    }
}
