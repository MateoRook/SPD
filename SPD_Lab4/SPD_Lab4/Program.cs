using System;
using System.Collections.Generic;
using System.IO;

namespace SPD_Lab4
{
    class Program
    {
        static void Main(string[] args)
        {
            string dataFileName = "in50.txt";
            string dataFileName2 = "in100.txt";
            string dataFileName3 = "in200.txt";

            List<SchedulingTask> tasks;

            try
            {
                using (StreamWriter swr = new StreamWriter("result.txt"))
                {
                    swr.WriteLine("algorytm \t 50 \t 100 \t 200");
                    swr.WriteLine("-------------------------------------");

                    tasks = Util.SeedData(dataFileName);
                    int schrage1 = Util.CalculateSpanC(Util.Schrage(tasks));
                    int pmtn1 = Util.SchragePmtn(tasks);
                    tasks = Util.SeedData(dataFileName2);
                    int schrage2 = Util.CalculateSpanC(Util.Schrage(tasks));
                    int pmtn2 = Util.SchragePmtn(tasks);
                    tasks = Util.SeedData(dataFileName3);
                    int schrage3 = Util.CalculateSpanC(Util.Schrage(tasks));
                    int pmtn3 = Util.SchragePmtn(tasks);

                    swr.WriteLine($"Schrage \t {schrage1} \t {schrage2} \t {schrage3}");
                    swr.WriteLine($"Schrage Pmtn \t {pmtn1} \t {pmtn2} \t {pmtn3}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message); Console.ReadKey();
            }
        }
    }
}
