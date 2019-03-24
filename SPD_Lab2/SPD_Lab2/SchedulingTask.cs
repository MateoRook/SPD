using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD_Lab2
{
    public class SchedulingTask
    {
        public int[] TimeOnMachine { get; }
        public int[] TotalTimeOnMachine { get; set; }
        public int[] TimeToEnd { get; set; }
        public int SumOfTimes { get; }

        public SchedulingTask(int amountOfMachines, params int[] timeOnMachine)
        {
            TimeOnMachine = new int[amountOfMachines];
            TotalTimeOnMachine = new int[amountOfMachines];
            TimeToEnd = new int[amountOfMachines];

            timeOnMachine.CopyTo(TimeOnMachine, 0);
            SumOfTimes = TimeOnMachine.Sum();
        }
        public void CopyTimeOnMachine()
        {
            int total = 0;
            for (int i = 0; i < TimeOnMachine.Length; i++)
            {
                total += TimeOnMachine[i];
                TotalTimeOnMachine[i] = total;
            }
        }
        public void WriteDown()
        {
            foreach (var x in TimeOnMachine)
            {
                Console.Write($"{x} ");
            }
            Console.WriteLine();
        }
    }
}
