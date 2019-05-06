using System;
using System.Collections.Generic;

namespace SPD_Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            string dataFileName = "in50.txt";
            List<SchedulingTask> tasks;

            tasks = Util.SeedData(out int amountOfTasks, dataFileName);
            int spanC = Util.CalculateSpanC(Util.Schrage(tasks));
            Console.WriteLine(spanC);
        }
    }
}
