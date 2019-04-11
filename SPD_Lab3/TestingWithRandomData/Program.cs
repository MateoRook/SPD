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
            const string resultFileName4 = "ProbMOD.txt";
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
                //    for (int j = 1; j < 10001; j++)
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


                //parametr u
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
                //double[] T0 = { 0.0001, 0.001, 0.01, 0.1, 1 };
                //int[] spanSum = { 0, 0, 0, 0, 0 };
                //using (StreamWriter swr = new StreamWriter(resultFileName3))
                //{
                //    swr.WriteLine("indeks \t 0.0001 \t 0.001 \t 0.01 \t 0.1 \t 1");
                //    SPD_Lab2.Util<SchedulingTask>.CreateRandomDataSet(dataFileName, amountOfTasks, amountOfMachines);
                //    tasks = SPD_Lab2.Util<SchedulingTask>.SeedData(out int t, out int m, dataFileName);
                //    for (int j = 0; j < 10; j++)
                //    {
                //        //string r = $"{j}";
                //        for (int k = 0; k < T0.Length; k++)
                //        {
                //            result = SPD_Lab3.Util<SchedulingTask>.SimulatedAnnealing(tasks.ToList(), SPD_Lab3.Util<SchedulingTask>.Swap, T0[j]);

                //            int p = SPD_Lab2.Util<SchedulingTask>.CalculateSpanC(tasks.Length, tasks[0].TimeOnMachine.Length, result);
                //            //r += $" \t {p}";
                //            spanSum[k] += p;
                //        }
                //        //r += $" \t {SPD_Lab2.Util<SchedulingTask>.CalculateSpanC2(SPD_Lab2.Util<SchedulingTask>.NEHAccelerated(tasks.ToList()).ToArray(), 0)}";
                //        //swr.WriteLine(r);
                //    }
                //    string XD = "\t";
                //    foreach (var x in spanSum)
                //    {
                //        XD += $"{x / 10} \t";
                //    }
                //    XD += $" \t {SPD_Lab2.Util<SchedulingTask>.CalculateSpanC2(SPD_Lab2.Util<SchedulingTask>.NEHAccelerated(tasks.ToList()).ToArray(), 0)}";
                //    swr.WriteLine(XD);
                //}

                //probMOD
                //bool probMOD = false;
                //using (StreamWriter swr = new StreamWriter(resultFileName4))
                //{
                //    swr.WriteLine(" nie \t tak ");
                //    amountOfMachines = 5;
                //    for (int s = 0; s < 5; s++)
                //    {
                //        int[] spanSum = { 0, 0 };
                //        amountOfTasks += 5;
                //        SPD_Lab2.Util<SchedulingTask>.CreateRandomDataSet(dataFileName, amountOfTasks, amountOfMachines);
                //        tasks = SPD_Lab2.Util<SchedulingTask>.SeedData(out int t, out int m, dataFileName);

                //        for (int j = 0; j < 10; j++)
                //        {
                //            for (int k = 0; k < spanSum.Length; k++)
                //            {
                //                result = SPD_Lab3.Util<SchedulingTask>.SimulatedAnnealing(tasks.ToList(), SPD_Lab3.Util<SchedulingTask>.Swap, probabilityMOD: probMOD);

                //                int p = SPD_Lab2.Util<SchedulingTask>.CalculateSpanC(tasks.Length, tasks[0].TimeOnMachine.Length, result);
                //                spanSum[k] += p;
                //                probMOD = !probMOD;
                //            }
                //        }
                //        string XD = "\t";
                //        foreach (var x in spanSum)
                //        {
                //            XD += $"{x / 10} \t";
                //        }
                //        swr.WriteLine(XD);
                //    }
                //}

                //mod z Cmax
                //bool cmaxMOD = false;
                //using (StreamWriter swr = new StreamWriter(resultFileName4))
                //{
                //    swr.WriteLine(" nie \t tak ");
                //    amountOfTasks = 100;
                //    for (int s = 0; s < 5; s++)
                //    {
                //        int[] spanSum = { 0, 0 };
                //        amountOfTasks += 50;
                //        SPD_Lab2.Util<SchedulingTask>.CreateRandomDataSet(dataFileName, amountOfTasks, amountOfMachines);
                //        tasks = SPD_Lab2.Util<SchedulingTask>.SeedData(out int t, out int m, dataFileName);

                //        for (int j = 0; j < 10; j++)
                //        {
                //            for (int k = 0; k < spanSum.Length; k++)
                //            {
                //                result = SPD_Lab3.Util<SchedulingTask>.SimulatedAnnealing(tasks.ToList(), SPD_Lab3.Util<SchedulingTask>.Swap, cmaxMOD: cmaxMOD);

                //                int p = SPD_Lab2.Util<SchedulingTask>.CalculateSpanC(tasks.Length, tasks[0].TimeOnMachine.Length, result);
                //                spanSum[k] += p;
                //                cmaxMOD = !cmaxMOD;
                //            }
                //        }
                //        string XD = "\t";
                //        foreach (var x in spanSum)
                //        {
                //            XD += $"{x / 10} \t";
                //        }
                //        swr.WriteLine(XD);
                //    }
                //}

                // ostatni
                //int p,p2;
                //SPD_Lab2.Util<SchedulingTask>.CreateRandomDataSet(dataFileName, amountOfTasks, amountOfMachines);
                //tasks = SPD_Lab2.Util<SchedulingTask>.SeedData(out int t, out int m, dataFileName);
                //result = SPD_Lab3.Util<SchedulingTask>.SimulatedAnnealing(tasks.ToList(), SPD_Lab3.Util<SchedulingTask>.Swap,isNeutral:true);
                //p = SPD_Lab2.Util<SchedulingTask>.CalculateSpanC(tasks.Length, tasks[0].TimeOnMachine.Length, result);
                //result = SPD_Lab3.Util<SchedulingTask>.SimulatedAnnealing(tasks.ToList(), SPD_Lab3.Util<SchedulingTask>.Swap);
                //p2 = SPD_Lab2.Util<SchedulingTask>.CalculateSpanC(tasks.Length, tasks[0].TimeOnMachine.Length, result); 
                //Console.WriteLine($"Neutral: {p} \t Neh: {p2}");

                // best
                int p, p2;
                SPD_Lab2.Util<SchedulingTask>.CreateRandomDataSet("ta.txt", amountOfTasks, amountOfMachines);
                tasks = SPD_Lab2.Util<SchedulingTask>.SeedData(out int t, out int m, dataFileName);
                result = SPD_Lab3.Util<SchedulingTask>.SimulatedAnnealing(tasks.ToList(), SPD_Lab3.Util<SchedulingTask>.Swap,u:0.9999,probabilityMOD:true);
                var result2 = SPD_Lab2.Util<SchedulingTask>.NEHAccelerated(tasks.ToList());
                p = SPD_Lab2.Util<SchedulingTask>.CalculateSpanC(tasks.Length, tasks[0].TimeOnMachine.Length, result);
                p2 = SPD_Lab2.Util<SchedulingTask>.CalculateSpanC(tasks.Length, tasks[0].TimeOnMachine.Length, result2);
                Console.WriteLine($"SA: {p} \t Neh: {p2}");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); Console.ReadKey();
            }
        }
    }
}