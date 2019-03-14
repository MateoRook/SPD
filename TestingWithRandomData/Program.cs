using SPD_Lab1;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace TestingWithRandomData
{
    class Program
    {
        static void Main(string[] args)
        {
            string dataFileName = "DataSet.txt";
            const string resultFileName = "results.txt";
            SchedulingTask[] tasks;
            int minSpanPermute;
            int minSpanJohnson;
            Stopwatch sw = new Stopwatch();
            TimeSpan permuteTime, johnsonTime;

            try
            {
                using (StreamWriter swr = new StreamWriter(resultFileName))
                {
                    swr.WriteLine("ilość maszyn \t ilość zadań \t permute \t johnson");

                    for (int i = 0; i < 10; i++)
                    {
                        minSpanPermute = int.MaxValue;
                        minSpanJohnson = int.MaxValue;
                        Util<SchedulingTask>.CreateRandomDataSet(dataFileName);
                        tasks = Util<SchedulingTask>.SeedData(out int amountOfTasks, out int amountOfMachines, dataFileName);

                        sw.Restart();
                        foreach (var p in Util<SchedulingTask>.Permute(tasks))
                        {
                            int timeToComplete = Util<SchedulingTask>.CalculateSpanC(amountOfTasks, amountOfMachines, p);
                            minSpanPermute = Math.Min(minSpanPermute, timeToComplete);
                        }
                        permuteTime = sw.Elapsed;
                        tasks = Util<SchedulingTask>.SeedData(out amountOfTasks, out amountOfMachines, dataFileName);
                        sw.Restart();
                        minSpanJohnson = Util<SchedulingTask>.CalculateSpanC(
                            amountOfTasks, amountOfMachines, Util<SchedulingTask>.Johnsons(tasks.ToList()));
                        johnsonTime = sw.Elapsed;

                        swr.WriteLine($"{amountOfMachines} \t\t {amountOfTasks} \t\t" +
                            $"{permuteTime.ToString("mm\\:ss\\.ffff")} \t {johnsonTime.ToString("mm\\:ss\\.ffff")}");// \t {minSpanPermute} {minSpanJohnson});
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message); Console.ReadKey();
            }

        }
    }
}
