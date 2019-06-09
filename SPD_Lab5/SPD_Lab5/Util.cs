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
        public static List<Job> InitJobList2()
        {
            List<Job> jobs = new List<Job>();

            List<TaskJob> taskList = new List<TaskJob>();
            taskList.Add(new TaskJob(0, 4, 0));
            taskList.Add(new TaskJob(1, 2, 2));
            taskList.Add(new TaskJob(2, 5, 1));
            jobs.Add(new Job(0, taskList));

            List<TaskJob> taskList2 = new List<TaskJob>();
            taskList2.Add(new TaskJob(0, 2, 1));
            taskList2.Add(new TaskJob(1, 2, 2));
            jobs.Add(new Job(1, taskList2));

            List<TaskJob> taskList3 = new List<TaskJob>();
            taskList3.Add(new TaskJob(0, 4, 0));
            taskList3.Add(new TaskJob(1, 2, 1));
            jobs.Add(new Job(2, taskList3));

            List<TaskJob> taskList4 = new List<TaskJob>();
            taskList4.Add(new TaskJob(0, 2, 2));
            taskList4.Add(new TaskJob(1, 2, 0));
            taskList4.Add(new TaskJob(2, 5, 1));
            taskList4.Add(new TaskJob(3, 2, 2));
            jobs.Add(new Job(3, taskList4));

            return jobs;
        }

        public static List<Job> InitJobList()
        {
            List<Job> jobs = new List<Job>();

            List<TaskJob> taskList = new List<TaskJob>();
            taskList.Add(new TaskJob(0, 65, 0));
            taskList.Add(new TaskJob(1, 5, 1));
            taskList.Add(new TaskJob(2, 15, 2));
            jobs.Add(new Job(0, taskList));

            List<TaskJob> taskList2 = new List<TaskJob>();
            taskList2.Add(new TaskJob(0, 15, 0));
            taskList2.Add(new TaskJob(1, 25, 1));
            taskList2.Add(new TaskJob(2, 10, 2));
            jobs.Add(new Job(1, taskList2));

            List<TaskJob> taskList3 = new List<TaskJob>();
            taskList3.Add(new TaskJob(0, 25, 0));
            taskList3.Add(new TaskJob(1, 30, 1));
            taskList3.Add(new TaskJob(2, 40, 2));
            jobs.Add(new Job(2, taskList3));

            List<TaskJob> taskList4 = new List<TaskJob>();
            taskList4.Add(new TaskJob(0, 20, 0));
            taskList4.Add(new TaskJob(1, 35, 1));
            taskList4.Add(new TaskJob(2, 10, 2));
            jobs.Add(new Job(3, taskList4));

            List<TaskJob> taskList5 = new List<TaskJob>();
            taskList5.Add(new TaskJob(0, 15, 0));
            taskList5.Add(new TaskJob(1, 25, 1));
            taskList5.Add(new TaskJob(2, 10, 2));
            jobs.Add(new Job(4, taskList5));

            List<TaskJob> taskList6 = new List<TaskJob>();
            taskList6.Add(new TaskJob(0, 25, 0));
            taskList6.Add(new TaskJob(1, 30, 1));
            taskList6.Add(new TaskJob(2, 40, 2));
            jobs.Add(new Job(5, taskList6));

            List<TaskJob> taskList7 = new List<TaskJob>();
            taskList7.Add(new TaskJob(0, 20, 0));
            taskList7.Add(new TaskJob(1, 35, 1));
            taskList7.Add(new TaskJob(2, 10, 2));
            jobs.Add(new Job(6, taskList7));

            List<TaskJob> taskList8 = new List<TaskJob>();
            taskList8.Add(new TaskJob(0, 10, 0));
            taskList8.Add(new TaskJob(1, 15, 1));
            taskList8.Add(new TaskJob(2, 50, 2));
            jobs.Add(new Job(7, taskList8));

            List<TaskJob> taskList9 = new List<TaskJob>();
            taskList9.Add(new TaskJob(0, 50, 0));
            taskList9.Add(new TaskJob(1, 10, 1));
            taskList9.Add(new TaskJob(2, 29, 2));
            jobs.Add(new Job(8, taskList9));

            return jobs;
        }


        public static void JobShopCp(List<Job> jobs, int machines)
        {
            CpModel model = new CpModel();
            int maxValue = 0;
            foreach (var job in jobs)
            {
                maxValue += job.Tasks.Sum(j => j.Duration);
            }

            List<List<IntervalVar>> intervals = new List<List<IntervalVar>>(jobs.Count);
            List<List<IntVar>> starts = new List<List<IntVar>>(jobs.Count);
            List<List<IntVar>> ends = new List<List<IntVar>>(jobs.Count);
            List<List<IntervalVar>> machinesIntervals = new List<List<IntervalVar>>();
            List<List<IntVar>> machinesStarts = new List<List<IntVar>>();

            for (int i = 0; i < machines; i++)
            {
                machinesIntervals.Add(new List<IntervalVar>());
                machinesStarts.Add(new List<IntVar>());
            }

            foreach (var job in jobs)
            {
                starts.Add(new List<IntVar>());
                ends.Add(new List<IntVar>());
                intervals.Add(new List<IntervalVar>());
                foreach (var task in job.Tasks)
                {
                    IntVar start = model.NewIntVar(0, maxValue, job.Name + task.Name);
                    IntVar end = model.NewIntVar(0, maxValue, job.Name + task.Name);
                    IntervalVar oneTask =
                        model.NewIntervalVar(start, task.Duration, end, job.Name + task.Name);
                    intervals[job.JobId].Add(oneTask);
                    starts[job.JobId].Add(start);
                    ends[job.JobId].Add(end);
                    machinesIntervals[task.Machine].Add(oneTask);
                    machinesStarts[task.Machine].Add(start);
                }
            }

            for (int j = 0; j < jobs.Count; ++j)
            {
                for (int t = 0; t < jobs[j].Tasks.Count - 1; ++t)
                {
                    model.Add(ends[j][t] <= starts[j][t + 1]);
                }
            }

            for (int machineId = 0; machineId < machines; ++machineId)
            {
                model.AddNoOverlap(machinesIntervals[machineId].ToArray());
            }

            IntVar[] allEnds = new IntVar[jobs.Count];
            for (int i = 0; i < jobs.Count; i++)
            {
                allEnds[i] = ends[i].Last();
            }

            IntVar makespan = model.NewIntVar(0, maxValue, "makespan");
            model.AddMaxEquality(makespan, allEnds);
            model.Minimize(makespan);

            CpSolver solver = new CpSolver();
            solver.StringParameters = "max_time_in_seconds:30.0";
            CpSolverStatus solverStatus = solver.Solve(model);
            if (solverStatus != CpSolverStatus.Optimal)
            {
                Console.WriteLine("Solver didn’t find optimal solution!");
            }
            Console.WriteLine("Objective value = " + solver.Value(makespan));
        }

        public static List<Job> SeedDataJobShop(string path)
        {
            List<Job> jobs; ;
            int amountOfMachines = 0;
            int amountOfJobs = 0;
            int amountOfTasks = 0;

            using (StreamReader sr = new StreamReader(path))
            {
                string line = sr.ReadLine();
                string[] signs = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                amountOfJobs = int.Parse(signs[0]);
                amountOfMachines = int.Parse(signs[1]);
                amountOfTasks = int.Parse(signs[2]);

                jobs = new List<Job>(amountOfJobs);
                for (int i = 0; i < amountOfJobs; i++)
                {
                    line = sr.ReadLine();
                    signs = line.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    List<TaskJob> tasks = new List<TaskJob>(int.Parse(signs[0]));

                    for (int j = 0; j < tasks.Count; j++)
                    {
                        tasks.Add(new TaskJob(j, int.Parse(signs[1 + j]), int.Parse(signs[2 + j])));
                    }
                    jobs.Add(new Job(0, tasks));
                }
            }
            return jobs;
        }

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
