﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD_Lab1
{
    public static class Util<T>
    {
        public static IEnumerable<T[]> Permute(T[] zad, int low = 0)
        {
            T temp;
            if (low + 1 >= zad.Length)
                yield return zad;
            else
            {
                foreach (var p in Permute(zad, low + 1))
                    yield return p;

                for (int i = low + 1; i < zad.Length; i++)
                {
                    temp = zad[low];
                    zad[low] = zad[i];
                    zad[i] = temp;

                    foreach (var p in Permute(zad, low + 1))
                        yield return p;

                    temp = zad[low];
                    zad[low] = zad[i];
                    zad[i] = temp;
                }
            }
        }

        public static int CalculateSpanC (int amountOfTasks, int amountOfMachines, IEnumerable<SchedulingTask> task)
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

        public static IEnumerable<SchedulingTask> Johnsons(List<SchedulingTask> tasks)
        {
            int amountOfMachines = tasks[0].TimeOnMachine.Length;
            if (amountOfMachines > 3)
                throw new ArgumentException("The number of working machines must lower or equal 3");
            else if (amountOfMachines == 3)
                return JohnsonsThree(tasks);
            else
                return JohnsonsTwo(tasks);
        }

        public static IEnumerable<SchedulingTask> JohnsonsThree(List<SchedulingTask> tasks)
        {
            List<SchedulingTask> tasks2 = new List<SchedulingTask>();
            for (int i = 0; i < tasks.Count; i++)
            {
                SchedulingTask task = new SchedulingTask(2, tasks[i].TimeOnMachine[0] + tasks[i].TimeOnMachine[1],
                    tasks[i].TimeOnMachine[1] + tasks[i].TimeOnMachine[2]);
                task.JohnsonIndex = i;
                tasks2.Add(task);
            }

            foreach (var t in JohnsonsTwo(tasks2))
            {
                yield return tasks[t.JohnsonIndex];
            }
        }
        private static IEnumerable<SchedulingTask> JohnsonsTwo(List<SchedulingTask> tasks)
        {
            List<SchedulingTask> machineOne = new List<SchedulingTask>();
            List<SchedulingTask> machineTwo = new List<SchedulingTask>();
            SchedulingTask.TaskMinimum taskMinimum = new SchedulingTask.TaskMinimum();

            while (tasks.Count > 0)
            {
                int i = 0;
                int min = int.MaxValue;
                foreach (var task in tasks)
                {
                    if (task.FindMin().MinValue < min)
                    {
                        taskMinimum = task.FindMin();
                        taskMinimum.TaskIndex = i;
                        min = taskMinimum.MinValue;
                    }
                    i++;
                }
                if (taskMinimum.MachineIndex == 0)
                {
                    machineOne.Add(tasks[taskMinimum.TaskIndex]);
                    tasks.RemoveAt(taskMinimum.TaskIndex);
                }
                else
                {
                    machineTwo.Insert(0, tasks[taskMinimum.TaskIndex]);
                    tasks.RemoveAt(taskMinimum.TaskIndex);
                }
            }
            foreach (var x in machineOne)
            {
                yield return x;
            }
            foreach (var x in machineTwo)
            {
                yield return x;
            }
        }

        public static void CreateRandomDataSet(string pathToFile)
        {
            int amountOfTasks, amountOfMachines, timeOnMachine;

            Random rand = new Random();

            using (StreamWriter sr = new StreamWriter(pathToFile))
            {
                amountOfTasks = rand.Next(4, 12);
                amountOfMachines = rand.Next(2, 4);
                sr.WriteLine($"{amountOfTasks} {amountOfMachines}");

                for (int i = 0; i < amountOfTasks; i++)
                {
                    for (int j = 0; j < amountOfMachines; j++)
                    {
                        timeOnMachine = rand.Next(100);
                        sr.Write($"{timeOnMachine} ");
                    }
                    sr.Write(Environment.NewLine);
                }
            }
        }
    }
}
