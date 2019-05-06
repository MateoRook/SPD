using System;
using System.Collections.Generic;
using System.Text;

namespace SPD_Lab4
{
    public class SchedulingTask
    {
        public int R { get; set; }
        public int P { get; set; }
        public int Q { get; set; }

        public SchedulingTask(int r, int p, int q)
        {
            R = r;
            P = p;
            Q = q;
        }
    }
}
