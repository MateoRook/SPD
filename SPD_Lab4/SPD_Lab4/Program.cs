using System;
using System.Collections.Generic;

namespace SPD_Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            string dataFileName = "in.txt";
            List<SchedulingTask> tasks;

            tasks = Util.SeedData(out int amountOfTasks, dataFileName);
            int spanC = Util.CalculateSpanC(Util.Carelier(tasks));
            Console.WriteLine(spanC);
        }
    }
}
