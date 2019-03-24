using System;
using SPD_Lab2;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace TestingWithRandomData
{
    class Program
    {
        static void Main(string[] args)
        {
            string dataFileName = "DataSet.txt";
            const string resultFileName = "results1.txt";
            const string resultFileName2 = "results2.txt";
            int minSpanNEH = 0;
            int minSpanNEHMod = 0;
            int minSpanNEHAcc = 0;
            Stopwatch sw = new Stopwatch();
            TimeSpan NEHTime = new TimeSpan(), NEHMod = new TimeSpan(), NEHAccTime = new TimeSpan();
            int amountOfMachines = 0, amountOfTasks = 0;
            SchedulingTask[] tasks;

            try
            {
                using (StreamWriter swr = new StreamWriter(resultFileName))
                {
                    swr.WriteLine("ilość maszyn;ilość zadań;NEH;NEHC;NEHAcc;NEHAccC;NEHMod;NEHModC");

                    amountOfTasks = 200;
                    for (int i = 8; i < 21; i++)
                    {
                        amountOfMachines = i;

                        for (int j = 0; j < 20; j++)
                        {
                            Util<SchedulingTask>.CreateRandomDataSet(dataFileName, amountOfTasks, amountOfMachines);
                            tasks = Util<SchedulingTask>.SeedData(out int t, out int m, dataFileName);

                            sw.Restart();
                            minSpanNEH = Util<SchedulingTask>.CalculateSpanC(amountOfTasks, amountOfMachines, Util<SchedulingTask>.NEH(tasks.ToList()));
                            NEHTime += sw.Elapsed;

                            sw.Restart();
                            minSpanNEHAcc = Util<SchedulingTask>.CalculateSpanC2(Util<SchedulingTask>.NEHAccelerated(tasks.ToList()).ToArray(), 0);
                            NEHAccTime += sw.Elapsed;

                            sw.Restart();
                            minSpanNEHMod = Util<SchedulingTask>.CalculateSpanC(amountOfTasks, amountOfMachines, Util<SchedulingTask>.NEHMod(tasks.ToList()));
                            NEHMod += sw.Elapsed;
                        }
                        NEHTime /= 20;
                        NEHAccTime /= 20;
                        NEHMod /= 20;
                        swr.WriteLine($"{amountOfMachines};{amountOfTasks};{minSpanNEH};{NEHTime.ToString("mm\\:ss\\.ffff")};" +
                            $"{minSpanNEHAcc};{NEHAccTime.ToString("mm\\:ss\\.ffff")};{minSpanNEHMod};{NEHMod.ToString("mm\\:ss\\.ffff")}");
                        NEHTime = new TimeSpan(0);
                        NEHAccTime = new TimeSpan(0);
                        NEHMod = new TimeSpan(0);
                    }
                }

                using(StreamWriter swr = new StreamWriter(resultFileName2))
                {
                    swr.WriteLine("ilość maszyn;ilość zadań;NEH;NEHC;NEHAcc;NEHAccC;NEHMod;NEHModC");

                    amountOfMachines = 20;
                    for (int i = 25; i <= 200; i += 25)
                    {
                        amountOfTasks = i;

                        for (int j = 0; j < 20; j++)
                        {
                            Util<SchedulingTask>.CreateRandomDataSet(dataFileName, amountOfTasks, amountOfMachines);
                            tasks = Util<SchedulingTask>.SeedData(out int t, out int m, dataFileName);

                            sw.Restart();
                            minSpanNEH = Util<SchedulingTask>.CalculateSpanC(amountOfTasks, amountOfMachines, Util<SchedulingTask>.NEH(tasks.ToList()));
                            NEHTime += sw.Elapsed;

                            sw.Restart();
                            minSpanNEHAcc = Util<SchedulingTask>.CalculateSpanC2(Util<SchedulingTask>.NEHAccelerated(tasks.ToList()).ToArray(), 0);
                            NEHAccTime += sw.Elapsed;

                            sw.Restart();
                            minSpanNEHMod = Util<SchedulingTask>.CalculateSpanC(amountOfTasks, amountOfMachines, Util<SchedulingTask>.NEHMod(tasks.ToList()));
                            NEHMod += sw.Elapsed;
                        }
                        NEHTime /= 20;
                        NEHAccTime /= 20;
                        NEHMod /= 20;
                        swr.WriteLine($"{amountOfMachines};{amountOfTasks};{minSpanNEH};{NEHTime.ToString("mm\\:ss\\.ffff")};" +
                            $"{minSpanNEHAcc};{NEHAccTime.ToString("mm\\:ss\\.ffff")};{minSpanNEHMod};{NEHMod.ToString("mm\\:ss\\.ffff")}");
                        NEHTime = new TimeSpan(0);
                        NEHAccTime = new TimeSpan(0);
                        NEHMod = new TimeSpan(0);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); Console.ReadKey();
            }
        }
    }
}
