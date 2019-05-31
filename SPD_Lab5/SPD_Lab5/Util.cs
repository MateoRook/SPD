using System;
using System.Collections.Generic;
using System.Text;
using SPD_Lab4;
using Google.OrTools.LinearSolver;
using Google.OrTools.Sat;

namespace SPD_Lab5
{
    public static class Util
    {
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
