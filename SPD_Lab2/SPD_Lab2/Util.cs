using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD_Lab2
{
    static class Util<T>
    {
        public static int CalculateSpanC(int amountOfTasks, int amountOfMachines, IEnumerable<SchedulingTask> task)
        {
            int[] workTimeMachine = new int[amountOfMachines];
            foreach (var t in task)
            {
                workTimeMachine[0] += t.TimeOnMachine[0];
                for (int j = 1; j < amountOfMachines; j++)
                {
                    workTimeMachine[j] = Math.Max(workTimeMachine[j - 1], workTimeMachine[j]) + t.TimeOnMachine[j];
                }
            }
            return workTimeMachine[amountOfMachines - 1];
        }

        public static SchedulingTask[] SeedData(out int amountOfTasks, out int amountOfMachines, string path)
        {
            SchedulingTask[] tasks;

            using (StreamReader sr = new StreamReader(path))
            {
                string line = sr.ReadLine();
                string[] signs = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                amountOfTasks = int.Parse(signs[0]);
                amountOfMachines = int.Parse(signs[1]);

                tasks = new SchedulingTask[amountOfTasks];

                for (int i = 0; i < amountOfTasks; i++)
                {
                    line = sr.ReadLine();
                    signs = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    int[] numbers = new int[signs.Length];

                    for (int j = 0; j < signs.Length; j++)
                    {
                        numbers[j] = int.Parse(signs[j]);
                    }
                    tasks[i] = new SchedulingTask(amountOfMachines, numbers);
                }
            }
            return tasks;
        }

        public static IEnumerable<SchedulingTask> NEH (List<SchedulingTask> tasks)
        {
            var descendingByTimeSum = tasks.OrderByDescending(t => t.TimeOnMachine.Sum());
            int amountOfMachines = tasks.First().TimeOnMachine.Length;
            int index = 0;
            int currentValue = int.MaxValue;
            List<SchedulingTask> order = new List<SchedulingTask>();
            for (int i = 0; i < tasks.Count; i++)
            {
                int howLong = order.Count + 1;
                int localMin = int.MaxValue;
                for (int j = 0; j < howLong; j++)
                {
                    order.Insert(j, descendingByTimeSum.ElementAt(i));
                    currentValue = CalculateSpanC(order.Count, amountOfMachines, order);
                    if (currentValue < localMin)
                    {
                        localMin = currentValue;
                        index = j;
                    }
                    order.RemoveAt(j);
                }
                order.Insert(index, descendingByTimeSum.ElementAt(i));
            }
            return order;
        }
    }
}
