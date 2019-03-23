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

        public static int CalculateSpanC2(SchedulingTask[] tasks, int insertedAt)
        {
            int amountOfMachines = tasks[0].TimeOnMachine.Length;
            int result;
            if (insertedAt == tasks.Length - 1)
            {
                tasks[insertedAt].TotalTimeOnMachine[0] = tasks[insertedAt - 1].TotalTimeOnMachine[0] + tasks[insertedAt].TimeOnMachine[0];
                for (int i = 1; i < amountOfMachines; i++)
                {
                    tasks[insertedAt].TotalTimeOnMachine[i] =
                        Math.Max(tasks[insertedAt].TotalTimeOnMachine[i - 1], tasks[insertedAt - 1].TotalTimeOnMachine[i])
                        + tasks[insertedAt].TimeOnMachine[i];
                }
                result = tasks[insertedAt].TotalTimeOnMachine[amountOfMachines - 1];
            }
            else if (insertedAt > 0)
            {
                tasks[insertedAt].TotalTimeOnMachine[0] = tasks[insertedAt - 1].TotalTimeOnMachine[0] + tasks[insertedAt].TimeOnMachine[0];
                result = tasks[insertedAt].TotalTimeOnMachine[0] + tasks[insertedAt + 1].TimeToEnd[0];

                for (int i = 1; i < amountOfMachines; i++)
                {
                    tasks[insertedAt].TotalTimeOnMachine[i] =
                        Math.Max(tasks[insertedAt].TotalTimeOnMachine[i - 1], tasks[insertedAt - 1].TotalTimeOnMachine[i])
                        + tasks[insertedAt].TimeOnMachine[i];
                    result = Math.Max(result, tasks[insertedAt].TotalTimeOnMachine[i] + tasks[insertedAt + 1].TimeToEnd[i]);
                }
            }
            else
            {
                tasks[insertedAt].TotalTimeOnMachine[0] = tasks[insertedAt].TimeOnMachine[0];
                result = tasks[insertedAt].TotalTimeOnMachine[0] + tasks[insertedAt + 1].TimeToEnd[0];
                for (int i = 1; i < amountOfMachines; i++)
                {
                    tasks[insertedAt].TotalTimeOnMachine[i] = tasks[insertedAt].TotalTimeOnMachine[i - 1] + tasks[insertedAt].TimeOnMachine[i];
                    result = Math.Max(result, tasks[insertedAt].TotalTimeOnMachine[i] + tasks[insertedAt + 1].TimeToEnd[i]);
                }
            }

            return result;
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
            tasks = tasks.OrderByDescending(t => t.TimeOnMachine.Sum()).ToList();
            int amountOfMachines = tasks.First().TimeOnMachine.Length;
            
            List<SchedulingTask> order = new List<SchedulingTask>();
            for (int i = 0; i < tasks.Count; i++)
            {
                CalculateBestOrder(tasks, order, amountOfMachines, i);
            }
            return order;
        }

        public static List<SchedulingTask> NEHAccelerated(List<SchedulingTask> tasks)
        {
            tasks = tasks.OrderByDescending(t => t.TimeOnMachine.Sum()).ToList();
            int amountOfMachines = tasks.First().TimeOnMachine.Length;

            List<SchedulingTask> order = new List<SchedulingTask>();

            order.Insert(0, tasks.ElementAt(0));
            UpdateTotalTimes(order.ToArray(), 0, order[0].SumOfTimes);

            for (int i = 1; i < tasks.Count; i++)
            {
                CalculateBestOrder2(tasks, order, amountOfMachines, i);
            }
            return order;
        }
        private static void CalculateBestOrder2(List<SchedulingTask> tasks, List<SchedulingTask> order, int amountOfMachines, int i)
        {
            int index = 0;
            int currentValue = int.MaxValue;
            int howLong = order.Count + 1;
            int localMin = int.MaxValue;
            for (int j = 0; j < howLong; j++)
            {
                order.Insert(j, tasks.ElementAt(i));
                currentValue = CalculateSpanC2(order.ToArray(), j);
                if (currentValue < localMin)
                {
                    localMin = currentValue;
                    index = j;
                }
                order.RemoveAt(j);
            }
            order.Insert(index, tasks.ElementAt(i));
            UpdateTotalTimes(order.ToArray(), index, localMin);
        }

        private static void CalculateBestOrder(List<SchedulingTask> tasks, List<SchedulingTask> order, int amountOfMachines, int i)
        {
            int index = 0;
            int currentValue = int.MaxValue;
            int howLong = order.Count + 1;
            int localMin = int.MaxValue;
            for (int j = 0; j < howLong; j++)
            {
                order.Insert(j, tasks.ElementAt(i));
                currentValue = CalculateSpanC(order.Count, amountOfMachines, order);
                if (currentValue < localMin)
                {
                    localMin = currentValue;
                    index = j;
                }
                order.RemoveAt(j);
            }
            order.Insert(index, tasks.ElementAt(i));
        }
        public static void UpdateTotalTimes(SchedulingTask[] tasks, int insertedAt, int calculatedSpanC)
        {
            if (tasks.Length == 1)
            {
                tasks[0].CopyTimeOnMachine();
                tasks[0].TimeToEnd[0] = tasks[0].SumOfTimes;
                for (int i = 1; i < tasks[0].TimeOnMachine.Length; i++)
                {
                    tasks[0].TimeToEnd[i] = tasks[0].TimeToEnd[i - 1] - tasks[0].TimeOnMachine[i - 1];
                }
                return;
            }

            int amountOfMachines = tasks[0].TimeOnMachine.Length;

            if (insertedAt > 0)
            {
                for (int i = insertedAt; i < tasks.Length; i++)
                {
                    tasks[i].TotalTimeOnMachine[0] = tasks[i - 1].TotalTimeOnMachine[0] + tasks[i].TimeOnMachine[0];

                    for (int j = 1; j < amountOfMachines; j++)
                    {
                        tasks[i].TotalTimeOnMachine[j] = Math.Max(tasks[i - 1].TotalTimeOnMachine[j], tasks[i].TotalTimeOnMachine[j - 1])
                            + tasks[i].TimeOnMachine[j];
                    }
                }
            }
            else
            {
                tasks[0].CopyTimeOnMachine();

                for (int i = insertedAt + 1; i < tasks.Length; i++)
                {
                    tasks[i].TotalTimeOnMachine[0] = tasks[i - 1].TotalTimeOnMachine[0] + tasks[i].TimeOnMachine[0];

                    for (int j = 1; j < amountOfMachines; j++)
                    {
                        tasks[i].TotalTimeOnMachine[j] = Math.Max(tasks[i - 1].TotalTimeOnMachine[j], tasks[i].TotalTimeOnMachine[j - 1])
                        + tasks[i].TimeOnMachine[j];
                    }
                }
            }

            //tasks[tasks.Length - 1].TimeToEnd[amountOfMachines - 1] = tasks[tasks.Length - 1].TimeOnMachine[amountOfMachines - 1];
            //for (int i = amountOfMachines - 2; i >= 0; i--)
            //{
            //    tasks[tasks.Length - 1].TimeToEnd[i] = tasks[tasks.Length - 1].TimeToEnd[i + 1] + tasks[tasks.Length - 1].TimeOnMachine[i];
            //}
            //for (int i = tasks.Length - 2; i >= 0; i--)
            //{
            //    for (int j = amountOfMachines - 1; j >= 0; j--)
            //    {
            //        tasks[i].TimeToEnd[j] = tasks[i + 1].TimeToEnd[j] + tasks[i].TimeOnMachine[j];
            //    }
            //}

            if (insertedAt < tasks.Length - 1)
            {
                for (int i = insertedAt; i >= 0; i--)
                {
                    tasks[i].TimeToEnd[0] = tasks[i + 1].TimeToEnd[0] + tasks[i].TimeOnMachine[0];
                    for (int j = amountOfMachines - 1; j > 0; j--)
                    {
                        tasks[i].TimeToEnd[j] = tasks[tasks.Length - 1].TotalTimeOnMachine[amountOfMachines - 1]
                            - Math.Max(tasks[i].TotalTimeOnMachine[j], tasks[i + 1].TotalTimeOnMachine[j - 1]) + tasks[i].TimeOnMachine[j];
                    }
                }
            }
            else
            {
                tasks[tasks.Length - 1].TimeToEnd[amountOfMachines - 1] = tasks[tasks.Length - 1].TimeOnMachine[amountOfMachines - 1];
                for (int i = amountOfMachines - 2; i >= 0; i--)
                {
                    tasks[tasks.Length - 1].TimeToEnd[i] = tasks[tasks.Length - 1].TimeToEnd[i + 1] + tasks[tasks.Length - 1].TimeOnMachine[i];
                }

                for (int i = insertedAt - 1; i >= 0; i--)
                {
                    tasks[i].TimeToEnd[0] = tasks[i + 1].TimeToEnd[0] + tasks[i].TimeOnMachine[0];
                    for (int j = amountOfMachines - 1; j > 0; j--)
                    {
                        tasks[i].TimeToEnd[j] = tasks[tasks.Length - 1].TotalTimeOnMachine[amountOfMachines - 1]
                            - Math.Max(tasks[i].TotalTimeOnMachine[j], tasks[i + 1].TotalTimeOnMachine[j - 1]) + tasks[i].TimeOnMachine[j];
                    }
                }
            }
        }
    }
}
