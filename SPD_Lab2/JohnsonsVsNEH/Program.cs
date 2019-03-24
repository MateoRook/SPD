using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace JohnsonsVsNEH
{
    class Program
    {
        static void Main(string[] args)
        {
            string dataFileName = "DataSetJonNeh.txt";
            const string resultFileName = "resultsPN1.txt";
            const string resultFileName2 = "resultsPN2.txt";
            int minSpanNEH = 0;
            int minSpanPermute = int.MaxValue;
            Stopwatch sw = new Stopwatch();
            TimeSpan NEHT = new TimeSpan(), JohnsonT = new TimeSpan();
            int amountOfMachines = 0, amountOfTasks = 0;
            SPD_Lab2.SchedulingTask[] tasksNEH;
            SPD_Lab1.SchedulingTask[] tasksJohn;
            try
            {
                using (StreamWriter swr = new StreamWriter(resultFileName))
                {
                    swr.WriteLine("ilość maszyn;ilość zadań;NEH;NEHC;Permute;PermuteC");
                    //swr.WriteLine("ilość maszyn;ilość zadań;NEH;NEHC;John;JohnC");
                    for (int k = 2; k < 10; k++)
                    {
                        amountOfTasks = k;
                        amountOfMachines = 2;

                        for (int j = 0; j < 20; j++)
                        {
                            SPD_Lab2.Util<SPD_Lab2.SchedulingTask>.CreateRandomDataSet(dataFileName, amountOfTasks, amountOfMachines);
                            tasksNEH = SPD_Lab2.Util<SPD_Lab2.SchedulingTask>.SeedData(out int t, out int m, dataFileName);
                            tasksJohn = SPD_Lab1.Util<SPD_Lab1.SchedulingTask>.SeedData(out int a, out int b, dataFileName);

                            sw.Restart();
                            minSpanNEH = SPD_Lab2.Util<SPD_Lab2.SchedulingTask>.CalculateSpanC(amountOfTasks, amountOfMachines,
                                SPD_Lab2.Util<SPD_Lab2.SchedulingTask>.NEH(tasksNEH.ToList()));
                            NEHT += sw.Elapsed;

                            sw.Restart();
                            foreach (var p in SPD_Lab1.Util<SPD_Lab1.SchedulingTask>.Permute(tasksJohn))
                            {
                                int timeToComplete = SPD_Lab1.Util<SPD_Lab1.SchedulingTask>.CalculateSpanC(amountOfTasks, amountOfMachines, p);
                                //Console.WriteLine($"Czas wykonania {timeToComplete}");
                                if (j == 19)
                                    minSpanPermute = Math.Min(minSpanPermute, timeToComplete);
                            }
                            //minSpanJohnson = SPD_Lab1.Util<SPD_Lab1.SchedulingTask>.CalculateSpanC(amountOfTasks, amountOfMachines,
                            //    SPD_Lab1.Util<SPD_Lab1.SchedulingTask>.Johnsons(tasksJohn.ToList()));
                            JohnsonT += sw.Elapsed;
                        }         
                        NEHT /= 20;
                        JohnsonT /= 20;
                        swr.WriteLine($"{amountOfMachines};{amountOfTasks};{minSpanNEH};{NEHT.ToString("mm\\:ss\\.ffff")};" +
                        $"{minSpanPermute};{JohnsonT.ToString("mm\\:ss\\.ffff")}");
                        NEHT = new TimeSpan(0);
                        JohnsonT = new TimeSpan(0);
                        minSpanPermute = int.MaxValue;
                    }
                }

                using (StreamWriter swr = new StreamWriter(resultFileName2))
                {
                    swr.WriteLine("ilość maszyn;ilość zadań;NEH;NEHC;Permute;PermuteC");
                    //swr.WriteLine("ilość maszyn;ilość zadań;NEH;NEHC;John;JohnC");
                    for (int k = 2; k < 10; k++)
                    {
                        amountOfTasks = k;
                        amountOfMachines = 3;

                        for (int j = 0; j < 20; j++)
                        {
                            SPD_Lab2.Util<SPD_Lab2.SchedulingTask>.CreateRandomDataSet(dataFileName, amountOfTasks, amountOfMachines);
                            tasksNEH = SPD_Lab2.Util<SPD_Lab2.SchedulingTask>.SeedData(out int t, out int m, dataFileName);
                            tasksJohn = SPD_Lab1.Util<SPD_Lab1.SchedulingTask>.SeedData(out int a, out int b, dataFileName);

                            sw.Restart();
                            minSpanNEH = SPD_Lab2.Util<SPD_Lab2.SchedulingTask>.CalculateSpanC(amountOfTasks, amountOfMachines,
                                SPD_Lab2.Util<SPD_Lab2.SchedulingTask>.NEH(tasksNEH.ToList()));
                            NEHT += sw.Elapsed;

                            sw.Restart();
                            foreach (var p in SPD_Lab1.Util<SPD_Lab1.SchedulingTask>.Permute(tasksJohn))
                            {
                                int timeToComplete = SPD_Lab1.Util<SPD_Lab1.SchedulingTask>.CalculateSpanC(amountOfTasks, amountOfMachines, p);
                                //Console.WriteLine($"Czas wykonania {timeToComplete}");
                                if (j == 19)
                                    minSpanPermute = Math.Min(minSpanPermute, timeToComplete);
                            }
                            //minSpanJohnson = SPD_Lab1.Util<SPD_Lab1.SchedulingTask>.CalculateSpanC(amountOfTasks, amountOfMachines,
                            //    SPD_Lab1.Util<SPD_Lab1.SchedulingTask>.Johnsons(tasksJohn.ToList()));
                            JohnsonT += sw.Elapsed;
                        }
                        NEHT /= 20;
                        JohnsonT /= 20;
                        swr.WriteLine($"{amountOfMachines};{amountOfTasks};{minSpanNEH};{NEHT.ToString("mm\\:ss\\.ffff")};" +
                        $"{minSpanPermute};{JohnsonT.ToString("mm\\:ss\\.ffff")}");
                        NEHT = new TimeSpan(0);
                        JohnsonT = new TimeSpan(0);
                        minSpanPermute = int.MaxValue;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
