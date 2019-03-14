using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SPD_Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            SchedulingTask[] tasks;
            string path;
            int minSpan = int.MaxValue;
            Stopwatch sw = new Stopwatch();
            try
            {
                Console.Write("Ścieżka do pliku: ");
                path = Console.ReadLine();

                tasks = Util<SchedulingTask>.SeedData(out int amountOfTasks, out int amountOfMachines, path);
                
                foreach (var p in Util<SchedulingTask>.Permute(tasks))
                {
                    int timeToComplete = Util<SchedulingTask>.CalculateSpanC(amountOfTasks, amountOfMachines, p);
                    //Console.WriteLine($"Czas wykonania {timeToComplete}");
                    minSpan = Math.Min(minSpan, timeToComplete);
                }
                Console.WriteLine($"Uzyskany minimalny czas pracy za pomocą przegloądu zupełnego: {minSpan}");
                minSpan = Util<SchedulingTask>.CalculateSpanC(amountOfTasks, amountOfMachines,
                     Util<SchedulingTask>.Johnsons(tasks.ToList()));
                Console.WriteLine($"Uzyskany minimalny czas pracy za pomocą Algotymu Johnsona: {minSpan}");
            }
            catch (Exception e)
            {
                Console.Write(e.Message); Console.ReadKey();
            }
        }   
    }
}
