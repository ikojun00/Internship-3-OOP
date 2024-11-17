using Internship_3_OOP.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internship_3_OOP.Classes
{
    public class Project
    {
        private string name;
        private string description;
        private DateTime startDate;
        private DateTime endDate;
        private ProjectStatus status;

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

        public DateTime StartDate
        {
            get => startDate;
            set => startDate = value;
        }

        public DateTime EndDate
        {
            get => endDate;
            set => endDate = value;
        }

        public ProjectStatus Status
        {
            get => status;
            set => status = value;
        }

        public Project(string name, string description, DateTime startDate, DateTime endDate)
        {
            Name = name;
            Description = description;
            this.startDate = startDate;
            EndDate = endDate;
            Status = ProjectStatus.Active;
        }
    }
}
