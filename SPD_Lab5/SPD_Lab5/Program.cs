using System;
using System.Collections.Generic;
using System.IO;
using Google.OrTools.LinearSolver;
using SPD_Lab4;

namespace SPD_Lab5
{
    class Program
    {
        static void Main(string[] args)
        {
            //// Create the linear solver with the CBC backend.
            //Solver solver = Solver.CreateSolver("SimpleMipProgram", "CBC_MIXED_INTEGER_PROGRAMMING");

            //// x and y are integer non-negative variables.
            //Variable x = solver.MakeIntVar(0.0, double.PositiveInfinity, "x");
            //Variable y = solver.MakeIntVar(0.0, double.PositiveInfinity, "y");

            //Console.WriteLine("Number of variables = " + solver.NumVariables());

            //// x + 7 * y <= 17.5.
            //solver.Add(x + 7 * y <= 17.5);

            //// x <= 3.5.
            //solver.Add(x <= 3.5);

            //Console.WriteLine("Number of constraints = " + solver.NumConstraints());

            //// Maximize x + 10 * y.
            //solver.Maximize(x + 10 * y);

            //Solver.ResultStatus resultStatus = solver.Solve();
            //// Check that the problem has an optimal solution.
            //if (resultStatus != Solver.ResultStatus.OPTIMAL)
            //{
            //    Console.WriteLine("The problem does not have an optimal solution!");
            //    return;
            //}

            //Console.WriteLine("Solution:");
            //Console.WriteLine("Objective value = " + solver.Objective().Value());
            //Console.WriteLine("x = " + x.SolutionValue());
            //Console.WriteLine("y = " + y.SolutionValue());

            //Console.WriteLine("\nAdvanced usage:");
            //Console.WriteLine("Problem solved in " + solver.WallTime() + " milliseconds");
            //Console.WriteLine("Problem solved in " + solver.Iterations() + " iterations");
            //Console.WriteLine("Problem solved in " + solver.Nodes() + " branch-and-bound nodes");

            //string dataFileName = "in0.txt";
            //List<SchedulingTask> tasks;

            //tasks = SPD_Lab4.Util.SeedData(dataFileName);

            //Util.RPQProblemCP(tasks);

            List<TaskWiti> taskWitis = Util.SeedData("witi.txt");
            Util.WiTiCp(taskWitis);
        }
    }
}
