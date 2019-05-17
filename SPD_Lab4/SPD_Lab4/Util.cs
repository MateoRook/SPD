using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace SPD_Lab4
{
    public static class Util
    {
        public static List<SchedulingTask> Carelier(List<SchedulingTask> tasks)
        {
            int UB, LB, U;
            int a, b, c;
            int r, p, q;
            int ru, pu, qu;
            int q_b;
            List<SchedulingTask> PI = new List<SchedulingTask>();
            List<SchedulingTask> K = new List<SchedulingTask>();
            List<SchedulingTask> tmp = new List<SchedulingTask>();
            IEnumerable<SchedulingTask> cc = new List<SchedulingTask>();
            SchedulingTask task, taskc;
            UB = CalculateSpanC(tasks);
            PI = Schrage(tasks);
            U = CalculateSpanC(PI);
            if (U < UB)
            {
                UB = U;
                tasks = PI;
            }
            b = PI
                .Where(t => (CalculateEndTime(PI.GetRange(0, PI.IndexOf(t) + 1)) + t.Q == U))
                .Max(ta => PI.IndexOf(ta));
            q_b = PI.ElementAt(b).Q;
            a =  PI
                .GetRange(0, b + 1)
                .Where(t => ((t.R + q_b + PI.GetRange(PI.IndexOf(t), b - PI.IndexOf(t) + 1).Sum(x => x.P)) == U))
                .Min(ta => PI.IndexOf(ta));
            cc = PI
                .GetRange(a, b - a + 1)
                .Where(t => t.Q < q_b);
            if (cc.Count() == 0)
                return tasks;
            else
                c = cc.Max(ta => PI.IndexOf(ta));
            K = PI.GetRange(c + 1, b - c);
            r = K.Min(t => t.R);
            q = K.Min(t => t.Q);
            p = K.Sum(t => t.P);
            taskc = PI.ElementAt(c);
            task = new SchedulingTask(taskc.R, taskc.P, taskc.Q);
            taskc.R = Math.Max(PI.ElementAt(c).R, r + p);

            LB = SchragePmtn(PI.Clone());
            K.Insert(0, taskc);
            ru = K.Min(t => t.R);
            qu = K.Min(t => t.Q);
            pu = K.Sum(t => t.P);
            LB = Math.Max(Math.Max(r + p + q, ru + qu + pu), LB);

            if (LB < UB)
                PI = Carelier(PI.Clone()); // bez Clone program dziala w nieskonczoność
            taskc.R = task.R;
            taskc.Q = Math.Max(taskc.Q, q + p);
            LB = SchragePmtn(PI.Clone());
            LB = Math.Max(Math.Max(r + p + q, ru + qu + pu), LB);
            if (LB < UB)
                PI = Carelier(PI.Clone());
            taskc.Q = task.Q;
            return PI;
        }

        public static List<SchedulingTask> SeedData(string path)
        {
            SchedulingTask[] tasks;
            int amountOfTasks = 0;

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

        public static int SchragePmtn(List<SchedulingTask> tasks)
        {
            int Cmax = 0;
            SchedulingTask t_j, task = new SchedulingTask(0,0,0);
            List<SchedulingTask> G = new List<SchedulingTask>();
            int t = 0;
            int q = int.MaxValue;
            int j;
            while (tasks.Count != 0 || G.Count != 0)
            {
                while (tasks.Count != 0 && tasks.Min(y => y.R) <= t)
                {
                    j = tasks.FindIndex(x => x.R == tasks.Min(y => y.R));
                    G.Add(tasks.ElementAt(j));
                    t_j = tasks.ElementAt(j);
                    tasks.RemoveAt(j);

                    if (t_j.Q > q)
                    {
                        task.P = t - t_j.R;
                        t = t_j.R;
                        if (task.P > 0)
                            G.Add(task);
                    }
                }
                if (G.Count == 0)
                    t = tasks.Min(x => x.R);
                else
                {
                    j = G.FindIndex(x => x.Q == G.Max(y => y.Q));
                    task = G.ElementAt(j);
                    q = task.Q;
                    G.RemoveAt(j);
                    t += task.P;
                    Cmax = Math.Max(Cmax, t + task.Q);
                }
            }
            return Cmax;
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

        public static int CalculateEndTime(List<SchedulingTask> tasks)
        {
            int M = 0;
            foreach (var item in tasks)
            {
                M = Math.Max(M, item.R) + item.P;
            }
            return M;
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

        public static List<T> Clone<T>(this List<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }
    }
}
