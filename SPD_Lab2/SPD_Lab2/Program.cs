using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD_Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            SchedulingTask[] tasks;
            List<SchedulingTask> result;
            string path;
            int minSpan = int.MaxValue;

            Console.Write("Ścieżka do pliku: ");
            path = Console.ReadLine();

            tasks = Util<SchedulingTask>.SeedData(out int amountOfTasks, out int amountOfMachines, path);

            minSpan = Util<SchedulingTask>.CalculateSpanC(amountOfTasks, amountOfMachines,
                Util<SchedulingTask>.NEH(tasks.ToList()));

            Console.WriteLine($"Minmalny czas trwania to: {minSpan}");

            result = Util<SchedulingTask>.NEHAccelerated(tasks.ToList());
            minSpan = result[result.Count - 1].TotalTimeOnMachine[amountOfMachines - 1];
            Console.WriteLine($"Minmalny czas trwania to: {minSpan}");

            minSpan = Util<SchedulingTask>.CalculateSpanC(amountOfTasks, amountOfMachines,
                Util<SchedulingTask>.NEHMod(tasks.ToList()));

            Console.WriteLine($"Minmalny czas trwania to: {minSpan}");
        }
    }
}
