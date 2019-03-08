using System;
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

        public static int CalculateSpanC (int amountOfTasks, int amountOfMachines, SchedulingTask[] task)
        {
            int[] workTimeMachine = new int[amountOfMachines];
            for (int i = 0; i < amountOfTasks; i++)
            {
                workTimeMachine[0] += task[i].TimeOnMachine[0];
                for (int j = 1; j < amountOfMachines; j++)
                {
                    workTimeMachine[j] = Math.Max(workTimeMachine[j - 1], workTimeMachine[j]) + task[i].TimeOnMachine[j];
                }
            }
            return workTimeMachine[amountOfMachines - 1];
        }

        public static SchedulingTask[] SeedData(out int amountOfTasks, out int amountOfMachines)
        {
            SchedulingTask[] tasks;
            Console.Write("Ścieżka do pliku: ");
            string path = Console.ReadLine();

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
    }
}
