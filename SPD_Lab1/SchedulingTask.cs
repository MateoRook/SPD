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
        public int JohnsonIndex { get; set; }

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

        public TaskMinimum FindMin ()
        {
            int min = int.MaxValue;
            int index;
            foreach  (int item in TimeOnMachine)
            {
                min = Math.Min(min, item);
            }
            index = Array.IndexOf(TimeOnMachine, min);
            return new TaskMinimum {
                MinValue = min,
                MachineIndex = index
            };
        }

        public struct TaskMinimum
        {
            public int MinValue { get; set; }
            public int MachineIndex { get; set; }
            public int TaskIndex { get; set; }
        }
    }
}
