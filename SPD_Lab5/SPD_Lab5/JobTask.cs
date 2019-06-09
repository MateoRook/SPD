using System;
using System.Collections.Generic;
using System.Text;

namespace SPD_Lab5
{
    public class Job
    {
        public Job(int jobId, List<TaskJob> taskJobs)
        {
            JobId = jobId;
            Tasks = taskJobs;
            Name = $"J{JobId}"; 
        }
        public List<TaskJob> Tasks { get; set; }
        public string Name { get; }
        public int JobId { get; set; }
    }

    public class TaskJob
    {
        public TaskJob (int taskId, int duration, int machine)
        {
            TaskId = taskId;
            Duration = duration;
            Machine = machine;
            Name = $"T{taskId}M{machine}D{duration}";
        }
        public int TaskId { get; set; }
        public int Machine { get; set; }
        public int Duration { get; set; }
        public string Name { get; }
    }
}
