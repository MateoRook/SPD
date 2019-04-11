using System;
using System.Collections.Generic;
using SPD_Lab2;
using System.Linq;

namespace SPD_Lab3
{
    class Program
    {
        static void Main(string[] args)
        {
            SchedulingTask[] tasks;
            List<SchedulingTask> result;
            string path;

            Console.Write("Ścieżka do pliku: ");
            path = Console.ReadLine();

            tasks = SPD_Lab2.Util<SchedulingTask>.SeedData(out int amountOfTasks, out int amountOfMachines, path);
            result = Util<SchedulingTask>.SimulatedAnnealing(tasks.ToList(),Util<SchedulingTask>.Swap);
            int spanC = SPD_Lab2.Util<SchedulingTask>.CalculateSpanC(tasks.Length, tasks[0].TimeOnMachine.Length, result);
            int spanC2 = SPD_Lab2.Util<SchedulingTask>.CalculateSpanC(tasks.Length, tasks[0].TimeOnMachine.Length,
                SPD_Lab2.Util<SchedulingTask>.NEH(tasks.ToList()));

            Console.WriteLine($"Wyliczony czas pracy: {spanC}, {spanC2}");
        }
    }
}
