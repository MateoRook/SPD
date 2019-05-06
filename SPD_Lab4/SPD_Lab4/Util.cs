using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SPD_Lab4
{
    public static class Util
    {
        public static SchedulingTask[] SeedData(out int amountOfTasks, string path)
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
            return tasks;
        }

        public static SchedulingTask[] Schrage(SchedulingTask[] tasks)
        {
            int t = 0;
            int j = 0;

        }
    }
}
