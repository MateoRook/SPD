﻿using System;
using System.Collections.Generic;
using System.Text;
using SPD_Lab2;
using System.Linq;

namespace SPD_Lab3
{
    public static class Util<T>
    {
        public static List<SchedulingTask> SimulatedAnnealing(List<SchedulingTask> tasks)
        {
            List<SchedulingTask> piZero = SPD_Lab2.Util<SchedulingTask>.NEH(tasks).ToList();
            Random random = new Random();
            double T0 = 73;
            double TEND = 12.213;
            double probability;
            while (TEND < T0)
            {
                SchedulingTask[] temp = new SchedulingTask[tasks.Count];
                piZero.CopyTo(temp);
                List<SchedulingTask> piNext = temp.ToList();
                int one = random.Next(0, tasks.Count - 1);
                int two = random.Next(0, tasks.Count - 1);
                Swap(piNext, one, two);
                probability = CalculateProbability(piZero, piNext, T0);
                double decide = random.NextDouble();
                if (probability >= decide)
                {
                    piZero = piNext;
                }
                T0 = Cooling(T0);
            }
            return piZero;
        }

        public static double Cooling(double temperature)
        {
            double u = 0.8;
            return temperature * u;
        }

        public static void Swap(List<SchedulingTask> tasks, int one, int two)
        {
            SchedulingTask temp = tasks[one];
            tasks[one] = tasks[two];
            tasks[two] = temp;
        }

        public static void Insert(List<SchedulingTask> tasks, int one, int two)
        {
            SchedulingTask temp = tasks.ElementAt(one);
            tasks.RemoveAt(one);
            tasks.Insert(two, temp);
        }
        public static double CalculateProbability(List<SchedulingTask> t1, List<SchedulingTask> t2, double T)
        {
            int amountOfTasks = t1.Count;
            int amountOfMachines = t1[0].TimeOnMachine.Length;
            int SpanC1 = SPD_Lab2.Util<SchedulingTask>.CalculateSpanC(amountOfTasks, amountOfMachines, t1);
            int SpanC2 = SPD_Lab2.Util<SchedulingTask>.CalculateSpanC(amountOfTasks, amountOfMachines, t2);
            double prob = SpanC2 < SpanC1 ? 1 : Math.Exp((SpanC1 - SpanC2) / T);
            return prob;
        }
    }
}