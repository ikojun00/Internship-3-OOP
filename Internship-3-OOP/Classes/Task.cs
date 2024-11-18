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
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Ime projekta ne može biti prazno.");
                name = value;
            }
        }

        public string Description
        {
            get => description;
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Opis projekta ne može biti prazan.");
                description = value;
            }
        }

        public DateTime Deadline
        {
            get => deadline;
            private set
            {
                if (value < DateTime.Now)
                    throw new ArgumentException("Rok ne može biti u prošlosti.");
                deadline = value;
            }
        }

        public TaskStatus Status
        {
            get => status;
            set
            {
                if (status == TaskStatus.Completed)
                    throw new InvalidOperationException("Ne možeš modificirati završen zadatak.");
                status = value;
            }
        }

        public int Duration
        {
            get => duration;
            private set
            {
                if (value <= 0)
                    throw new ArgumentException("Trajanje zadatka mora biti veće od 0 minuta.");
                duration = value;
            }
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
