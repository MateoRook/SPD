using System;

namespace SPD_Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            string dataFileName = "in50.txt";
            SchedulingTask[] tasks;

            tasks = Util.SeedData(out int amountOfTasks, dataFileName);
            Console.WriteLine();
        }
    }
}
