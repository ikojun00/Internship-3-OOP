using Internship_3_OOP.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Internship_3_OOP.Classes
{
    public class Task
    {
        private string name;
        private string description;
        private DateTime deadline;
        private TaskStatus status;
        private int duration;
        private Project project;

        public string Name
        {
            get => name;
            set => name = value;
        }

        public string Description
        {
            get => description;
            set => description = value;
        }

        public DateTime Deadline
        {
            get => deadline;
            set => deadline = value;
        }

        public TaskStatus Status
        {
            get => status;
            set => status = value;
        }

        public int Duration
        {
            get => duration;
            set => duration = value;
        }

        public Project Project
        {
            get => project;
            set => project = value;
        }

        public Task(string name, string description, DateTime deadline, int duration, Project project)
        {
            Name = name;
            Description = description;
            Deadline = deadline;
            Duration = duration;
            Project = project;
            Status = TaskStatus.Active;
        }
    }
}
