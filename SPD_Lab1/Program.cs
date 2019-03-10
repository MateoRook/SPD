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
            int minSpan = int.MaxValue;
            Stopwatch sw = new Stopwatch();
            try
            {
                tasks = Util<SchedulingTask>.SeedData(out int amountOfTasks, out int amountOfMachines);
                
                foreach (var p in Util<SchedulingTask>.Permute(tasks))
                {
                    int timeToComplete = Util<SchedulingTask>.CalculateSpanC(amountOfTasks, amountOfMachines, p);
                    //Console.WriteLine($"Czas wykonania {timeToComplete}");
                    minSpan = Math.Min(minSpan, timeToComplete);
                    if (minSpan == 32)
                    {
                        foreach( var c in p)
                        {
                            c.WriteDown();
                        }
                        break;
                    }
                }
                Console.WriteLine($"Minimalny czas pracy to: {minSpan}");
                foreach (var c in Util<SchedulingTask>.JohnsonsThree(tasks.ToList()))
                {
                    c.WriteDown();
                }
                minSpan = Util<SchedulingTask>.CalculateSpanC(amountOfTasks, 3,
                     Util<SchedulingTask>.JohnsonsThree(tasks.ToList()));
                Console.WriteLine($"Minimalny czas pracy to: {minSpan}");
            }
            catch (Exception e)
            {
                Console.Write(e.Message); Console.ReadKey();
            }
        }   
    }
}
