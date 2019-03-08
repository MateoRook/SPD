using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD_Lab1
{
    public class SchedulingTask
    {
        public int[] TimeOnMachine { get; }

        public SchedulingTask(int amountOfMachines, params int[] timeOnMachine)
        {
            TimeOnMachine = new int[amountOfMachines];

            timeOnMachine.CopyTo(TimeOnMachine, 0);
        }
        public void WriteDown()
        {
            foreach(var x in TimeOnMachine)
            {
                Console.Write($"{x} ");
            }
            Console.WriteLine();
        }
    }
}
