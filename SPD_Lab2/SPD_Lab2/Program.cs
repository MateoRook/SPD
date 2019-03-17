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
            string path;
            int minSpan = int.MaxValue;
            try
            {
                Console.Write("Ścieżka do pliku: ");
                path = Console.ReadLine();

                tasks = Util<SchedulingTask>.SeedData(out int amountOfTasks, out int amountOfMachines, path);
                //foreach(var p in Util<SchedulingTask>.NEH(tasks.ToList()))
                //{
                //    p.WriteDown();
                //}
                minSpan = Util<SchedulingTask>.CalculateSpanC(amountOfTasks, amountOfMachines, 
                    Util<SchedulingTask>.NEH(tasks.ToList()));
                Console.WriteLine($"Minmalny czas trwania to: {minSpan}");
            }
            catch (Exception e)
            {
                Console.Write(e.Message); Console.ReadKey();
            }
        }
    }
}
