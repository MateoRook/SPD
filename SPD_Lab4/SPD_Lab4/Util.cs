using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace SPD_Lab4
{
    public static class Util
    {
        public static List<SchedulingTask> SeedData(out int amountOfTasks, string path)
        {
            SchedulingTask[] tasks;

            using (StreamReader sr = new StreamReader(path))
            {
                string line = sr.ReadLine();
                string[] signs = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                amountOfTasks = int.Parse(signs[0]);

                tasks = new SchedulingTask[amountOfTasks];
                int r, p, q;
                for (int i = 0; i < amountOfTasks; i++)
                {
                    line = sr.ReadLine();
                    signs = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    
                    r = int.Parse(signs[0]);
                    p = int.Parse(signs[1]);
                    q = int.Parse(signs[2]);

                    tasks[i] = new SchedulingTask(r, p, q);
                }
            }
            return tasks.ToList();
        }

        public static List<SchedulingTask> Schrage(List<SchedulingTask> tasks)
        {
            int t = tasks.Min(x => x.R);
            int j = 0;
            List<SchedulingTask> G = new List<SchedulingTask>();
            List<SchedulingTask> PI = new List<SchedulingTask>();
            SchedulingTask task;
            tasks = tasks.OrderBy(x => x.R).ToList();
            while (tasks.Count != 0 || G.Count != 0)
            {
                while (tasks.Count != 0 && tasks.Min(x => x.R) <= t)
                {
                    j = tasks.FindIndex(x => x.R == tasks.Min(y => y.R));
                    G.Add(tasks.ElementAt(j));
                    tasks.RemoveAt(j);
                }
                if (G.Count == 0)
                    t = tasks.Min(x => x.R);
                else
                {
                    j = G.FindIndex(x => x.Q == G.Max(y => y.Q));
                    task = G.ElementAt(j);
                    PI.Add(task);
                    G.RemoveAt(j);
                    t += task.P;
                }
            }
            return PI;
        }

        public static int CalculateSpanC(List<SchedulingTask> tasks)
        {
            int M = 0;
            int C = 0;
            foreach (var item in tasks)
            {
                M = Math.Max(M, item.R) + item.P;
                C = Math.Max(C, M + item.Q);
            }
            return C;
        }
    }
}
