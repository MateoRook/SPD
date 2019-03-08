using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD_Lab1
{
    class Program
    {
        static void Main(string[] args)
        {
            SchedulingTask[] tasks;
            int minSpan = int.MaxValue;

            try
            {
                tasks = Util<SchedulingTask>.SeedData(out int amountOfTasks, out int amountOfMachines);
                
                foreach (var p in Util<SchedulingTask>.Permute(tasks))
                {
                    int timeToComplete = Util<SchedulingTask>.CalculateSpanC(amountOfTasks, amountOfMachines, p);
                    Console.WriteLine($"Czas wykonania {timeToComplete}");
                    minSpan = Math.Min(minSpan, timeToComplete);
                }
                Console.WriteLine($"Minimalny czas pracy to: {minSpan}");
            }
            catch (Exception e)
            {
                Console.Write(e.Message); Console.ReadKey();
            }
        }   
    }
}
