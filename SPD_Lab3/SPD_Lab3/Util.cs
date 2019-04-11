using System;
using System.Collections.Generic;
using System.Text;
using SPD_Lab2;
using System.Linq;

namespace SPD_Lab3
{
    public static class Util<T>
    {
        public static List<SchedulingTask> SimulatedAnnealing(List<SchedulingTask> tasks,
            Action<List<SchedulingTask>, int, int> action, 
            double u = 0.8,
            double T0 = 1000,
            bool probabilityMOD = false,
            bool cmaxMOD = false)
        {
            List<SchedulingTask> piZero = tasks; //SPD_Lab2.Util<SchedulingTask>.NEHAccelerated(tasks).ToList();
            Random random = new Random();
            double TEND = 1;
            double probability;
            //double T0 = 1000;
            while (TEND < T0)
            {
                SchedulingTask[] temp = new SchedulingTask[tasks.Count];
                List<SchedulingTask> piNext = new List<SchedulingTask>();
                //Swap(piNext, one, two);
                do
                {
                    piZero.CopyTo(temp);
                    piNext = temp.ToList();
                    int one = random.Next(0, tasks.Count - 1);
                    int two = random.Next(0, tasks.Count - 1);
                    action(piNext, one, two);
                    probability = CalculateProbability(piZero, piNext, T0, probabilityMOD, cmaxMOD);
                } while (probability < 0);
                double decide = random.NextDouble();
                if (probability >= decide)
                {
                    piZero = piNext;
                }
                T0 = Cooling(T0, u);
            }
            return piZero;
        }

        public static double Cooling(double temperature, double u = 0.8)
        {
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
        public static double CalculateProbability(List<SchedulingTask> t1, List<SchedulingTask> t2, double T, bool probabilityMOD, bool cmaxMOD)
        {
            int amountOfTasks = t1.Count;
            int amountOfMachines = t1[0].TimeOnMachine.Length;
            int SpanC1 = SPD_Lab2.Util<SchedulingTask>.CalculateSpanC(amountOfTasks, amountOfMachines, t1);
            int SpanC2 = SPD_Lab2.Util<SchedulingTask>.CalculateSpanC(amountOfTasks, amountOfMachines, t2);
            if (cmaxMOD & (SpanC1 == SpanC2)) return -1;
            double prob;
            if (probabilityMOD)
                prob = Math.Exp((SpanC1 - SpanC2) / T);
            else
                prob = SpanC2 < SpanC1 ? 1 : Math.Exp((SpanC1 - SpanC2) / T);
            return prob;
        }
    }
}
