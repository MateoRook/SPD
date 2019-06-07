using System;
using System.Collections.Generic;
using System.Text;
using SPD_Lab4;
using Google.OrTools.LinearSolver;
using Google.OrTools.Sat;
using System.Linq;
using System.IO;

namespace SPD_Lab5
{
    public static class Util
    {
        public static List<TaskWiti> SeedData(string path)
        {
            TaskWiti[] tasks;
            int amountOfTasks = 0;

            using (StreamReader sr = new StreamReader(path))
            {
                string line = sr.ReadLine();
                string[] signs = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                amountOfTasks = int.Parse(signs[0]);

                tasks = new TaskWiti[amountOfTasks];
                int p, w, d;
                for (int i = 0; i < amountOfTasks; i++)
                {
                    line = sr.ReadLine();
                    signs = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    p = int.Parse(signs[0]);
                    w = int.Parse(signs[1]);
                    d = int.Parse(signs[2]);

                    tasks[i] = new TaskWiti(p, w, d);
                }
            }
            return tasks.ToList();
        }

        public static void WiTiCp(List<TaskWiti> tasks)
        {
            var model = new CpModel();
            //int variablesMaxValue = 0;
            int timeSum = tasks.Sum(x => x.P);
            //foreach (var task in tasks)
            //    variablesMaxValue += ((timeSum - task.D) > 0 ? (timeSum - task.D) : 0) * task.W;

            IntVar[] starts = new IntVar[tasks.Count];
            IntVar[] ends = new IntVar[tasks.Count];
            IntVar[] punishment = new IntVar[tasks.Count];
            IntervalVar[] intervals = new IntervalVar[tasks.Count];
            //var witi = model.NewIntVar(0, variablesMaxValue, "witi");

            for (int i = 0; i < tasks.Count; i++)
            {
                starts[i] = model.NewIntVar(0, timeSum, $"start_task{i}");
            }
            for (int i = 0; i < tasks.Count; i++)
            {
                ends[i] = model.NewIntVar(0, timeSum, $"end_task{i}");
                intervals[i] = model.NewIntervalVar(starts[i], tasks[i].P, ends[i], $"interval{i}");
            }
            for (int i = 0; i < tasks.Count; i++)
            {
                if ((timeSum - tasks[i].D) > 0)
                    punishment[i] = model.NewIntVar(0, (timeSum - tasks[i].D) * tasks[i].W, $"punish_task{i}");
                else
                    punishment[i] = model.NewIntVar(0, 0, $"punish_task{i}");
            }

            model.AddNoOverlap(intervals);
            /*
foreach (var task in tasks)
{
    model.Add(ends[tasks.IndexOf(task)] == starts[tasks.IndexOf(task)] + task.P);
}
foreach (var task in tasks)
{
    model.Add(witi >= (ends[tasks.IndexOf(task)] - task.D) * task.W);
}*/
            foreach (var task in tasks)
            {
                model.Add(punishment[tasks.IndexOf(task)] >= (ends[tasks.IndexOf(task)] - task.D) * task.W);
            }
            //foreach (var task in tasks)
            //{
            //    model.Add(punishment[tasks.IndexOf(task)] <= ((timeSum - task.D) > 0 ? (timeSum - task.D) : 0) * task.W);
            //}

           
            var witi = punishment.Sum();    
            model.Minimize(witi);

            CpSolver solver = new CpSolver();
            solver.StringParameters = "max_time_in_seconds:30.0";
            CpSolverStatus solverStatus = solver.Solve(model);
            if (solverStatus != CpSolverStatus.Optimal)
            {
                Console.WriteLine("Solver didn’t find optimal solution!");
            }
            Console.WriteLine("Objective value = " + solver.Value(witi));
        }

        public static void RPQProblemMIP(List<SchedulingTask> tasks)
        {
            Solver solver = Solver.CreateSolver("SimpleMipProgram", "CBC_MIXED_INTEGER_PROGRAMMING");
            int variablesMaxValue = 0;
            foreach (SchedulingTask task in tasks)
                variablesMaxValue += task.R + task.P + task.Q;

            var alfas = solver.MakeIntVarMatrix(tasks.Count, tasks.Count, 0, 1);
            var starts = solver.MakeIntVarArray(tasks.Count, 0, variablesMaxValue);
            var cmax = solver.MakeIntVar(0, variablesMaxValue, "cmax");

            foreach (SchedulingTask task in tasks)
            {
                solver.Add(starts[tasks.IndexOf(task)] >= task.R);
            }
            foreach (SchedulingTask task in tasks)
            {
                solver.Add(cmax >= starts[tasks.IndexOf(task)] + task.P + task.Q);
            }

            for (int i = 0; i < tasks.Count; i++)
            {
                for (int j = i + 1; j < tasks.Count; j++)
                {
                    var job1 = tasks[i];
                    var job2 = tasks[j];
                    var job1ID = tasks.IndexOf(job1);
                    var job2ID = tasks.IndexOf(job2);
                    solver.Add(starts[job1ID] + job1.P <= starts[job2ID] +
                    alfas[job1ID, job2ID] * variablesMaxValue);
                    solver.Add(starts[job2ID] + job2.P <= starts[job1ID] +
                    alfas[job2ID, job1ID] * variablesMaxValue);
                    solver.Add(alfas[job1ID, job2ID] + alfas[job2ID, job1ID] == 1);
                }
            }
            solver.Minimize(cmax);
            Solver.ResultStatus resultStatus = solver.Solve();
            if (resultStatus != Solver.ResultStatus.OPTIMAL)
            {
                Console.WriteLine("Solver didn’t find optimal solution!");
            }
            Console.WriteLine("Objective value = " + solver.Objective().Value());

        }

        public static void RPQProblemCP(List<SchedulingTask> tasks)
        {
            var model = new CpModel();
            int variablesMaxValue = 0;
            foreach (SchedulingTask task in tasks)
                variablesMaxValue += task.R + task.P + task.Q;

            IntVar[,] alfas = new IntVar[tasks.Count,tasks.Count];
            IntVar[] starts = new IntVar[tasks.Count];
            var cmax = model.NewIntVar(0, variablesMaxValue, "cmax");

            for (int i = 0; i < tasks.Count; i++)
            {
                for (int j = 0; j < tasks.Count; j++)
                {
                    alfas[i, j] = model.NewIntVar(0, 1, $"task{i}_{j}");
                }
            }

            for (int i = 0; i < tasks.Count; i++)
            {
                starts[i] = model.NewIntVar(0, variablesMaxValue, $"start_task{i}");
            }


            foreach (SchedulingTask task in tasks)
            {
                model.Add(starts[tasks.IndexOf(task)] >= task.R);
            }
            foreach (SchedulingTask task in tasks)
            {
                model.Add(cmax >= starts[tasks.IndexOf(task)] + task.P + task.Q);
            }

            for (int i = 0; i < tasks.Count; i++)
            {
                for (int j = i + 1; j < tasks.Count; j++)
                {
                    var job1 = tasks[i];
                    var job2 = tasks[j];
                    var job1ID = tasks.IndexOf(job1);
                    var job2ID = tasks.IndexOf(job2);
                    model.Add(starts[job1ID] + job1.P <= starts[job2ID] +
                    alfas[job1ID, job2ID] * variablesMaxValue);
                    model.Add(starts[job2ID] + job2.P <= starts[job1ID] +
                    alfas[job2ID, job1ID] * variablesMaxValue);
                    model.Add(alfas[job1ID, job2ID] + alfas[job2ID, job1ID] == 1);
                }
            }

            model.Minimize(cmax);

            CpSolver solver = new CpSolver();
            solver.StringParameters = "max_time_in_seconds:30.0";
            CpSolverStatus solverStatus = solver.Solve(model);
            if (solverStatus != CpSolverStatus.Optimal)
            {
                Console.WriteLine("Solver didn’t find optimal solution!");
            }
            Console.WriteLine("Objective value = " + solver.Value(cmax));
        }
    }
}
