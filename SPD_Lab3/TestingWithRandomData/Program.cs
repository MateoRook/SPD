using System;
using SPD_Lab3;
using SPD_Lab2;
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
            const string resultFileName = "SwapVsInsert.txt";
            const string resultFileName2 = "ParamU.txt";
            const string resultFileName3 = "ParamT0.txt";
            int minSpanSwap = 0;
            int minSpanInsert = 0;
            int amountOfMachines = 20, amountOfTasks = 500;
            SchedulingTask[] tasks, tasks2 = new SchedulingTask[500];
            List<SchedulingTask> result;

            try
            {
                // SwapVsInsert
                //using (StreamWriter swr = new StreamWriter(resultFileName))
                //{
                //    swr.WriteLine("indeks \t Swap \t Insert");
                //    int s = 0;
                //    int i = 0;
                //    for (int j = 1; j < 11; j++)
                //    {
                //        SPD_Lab2.Util<SchedulingTask>.CreateRandomDataSet(dataFileName, amountOfTasks, amountOfMachines);
                //        tasks = SPD_Lab2.Util<SchedulingTask>.SeedData(out int t, out int m, dataFileName);
                //        tasks.CopyTo(tasks2, 0);

                //        result = SPD_Lab3.Util<SchedulingTask>.SimulatedAnnealing(tasks.ToList(), SPD_Lab3.Util<SchedulingTask>.Swap);

                //        minSpanSwap = SPD_Lab2.Util<SchedulingTask>.CalculateSpanC(tasks.Length, tasks[0].TimeOnMachine.Length, result);

                //        result = SPD_Lab3.Util<SchedulingTask>.SimulatedAnnealing(tasks.ToList(), SPD_Lab3.Util<SchedulingTask>.Insert);

                //        minSpanInsert = SPD_Lab2.Util<SchedulingTask>.CalculateSpanC(tasks.Length, tasks[0].TimeOnMachine.Length, result);

                //        swr.WriteLine($"{j} \t {minSpanSwap} \t {minSpanInsert}");
                //        if (minSpanInsert < minSpanSwap) i++;
                //        else s++;
                //    }
                //    swr.WriteLine($"Swap: {s} \t Insert: {i}");
                //}


                // parametr u
                //double[] u = { 0.8, 0.9, 0.95, 0.99, 0.9999 };
                //int[] best = { 0, 0, 0, 0, 0 };
                //using (StreamWriter swr = new StreamWriter(resultFileName2))
                //{
                //    swr.WriteLine("indeks \t 0.8 \t 0.9 \t 0.95 \t 0.99 \t 0.9999 \t NEHacc");

                //    for (int j = 1; j < 11; j++)
                //    {
                //        string r = $"{j}";
                //        SPD_Lab2.Util<SchedulingTask>.CreateRandomDataSet(dataFileName, amountOfTasks, amountOfMachines);
                //        tasks = SPD_Lab2.Util<SchedulingTask>.SeedData(out int t, out int m, dataFileName);
                //        int bestValue = int.MaxValue;
                //        int whichBest = -1;
                //        for (int k = 0; k < u.Length; k++)
                //        {
                //            result = SPD_Lab3.Util<SchedulingTask>.SimulatedAnnealing(tasks.ToList(), SPD_Lab3.Util<SchedulingTask>.Swap, u[k]);

                //            int p = SPD_Lab2.Util<SchedulingTask>.CalculateSpanC(tasks.Length, tasks[0].TimeOnMachine.Length, result);
                //            r += $" \t {p}";
                //            if (p < bestValue) { bestValue = p; whichBest = k; }
                //        }
                //        best[whichBest]++;
                //        r += $" \t {SPD_Lab2.Util<SchedulingTask>.CalculateSpanC2(SPD_Lab2.Util<SchedulingTask>.NEHAccelerated(tasks.ToList()).ToArray(), 0)}";
                //        swr.WriteLine(r);
                //    }

                //    string x = "\t";
                //    foreach (var y in best)
                //    {
                //        x += $"{y} \t";
                //    }
                //    swr.WriteLine(x);
                //}


                // parametr T0
                double[] T0 = { 10, 1000, 100000, 10000000, 1000000000 };
                int[] spanSum = { 0, 0, 0, 0, 0 };
                using (StreamWriter swr = new StreamWriter(resultFileName3))
                {
                    swr.WriteLine("indeks \t 10 \t 100 \t 1000 \t 10000 \t 100000 NEHacc");

                    for (int j = 1; j < 11; j++)
                    {
                        string r = $"{j}";
                        SPD_Lab2.Util<SchedulingTask>.CreateRandomDataSet(dataFileName, amountOfTasks, amountOfMachines);
                        tasks = SPD_Lab2.Util<SchedulingTask>.SeedData(out int t, out int m, dataFileName);

                        for (int k = 0; k < T0.Length; k++)
                        {
                            result = SPD_Lab3.Util<SchedulingTask>.SimulatedAnnealing(tasks.ToList(), SPD_Lab3.Util<SchedulingTask>.Swap, T0: T0[k]);

                            int p = SPD_Lab2.Util<SchedulingTask>.CalculateSpanC(tasks.Length, tasks[0].TimeOnMachine.Length, result);
                            r += $" \t {p}";
                            spanSum[k] += p;
                        }
                        r += $" \t {SPD_Lab2.Util<SchedulingTask>.CalculateSpanC2(SPD_Lab2.Util<SchedulingTask>.NEHAccelerated(tasks.ToList()).ToArray(), 0)}";
                        swr.WriteLine(r);
                    }
                    string XD = "\t";
                    foreach (var x in spanSum)
                    {
                        XD += $"{x/10} \t";
                    }
                    swr.WriteLine(XD);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); Console.ReadKey();
            }
        }
    }
}