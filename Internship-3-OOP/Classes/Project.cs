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
        private Guid id;

        public Guid Id { get; private set; }
        public string Name
        {
            get => name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Ime projekta ne može biti prazno.");
                name = value;
            }
        }

        public string Description
        {
            get => description;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Opis projekta ne može biti prazan.");
                description = value;
            }
        }

        public DateTime StartDate
        {
            get => startDate;
            set => startDate = value;
        }

        public DateTime EndDate
        {
            get => endDate;
            set
            {
                if (value < startDate)
                    throw new ArgumentException("Datum završetka ne može biti prije datuma početka.");
                endDate = value;
            }
        }

        public ProjectStatus Status
        {
            get => status;
            set
            {
                if (status == ProjectStatus.Completed)
                    throw new InvalidOperationException("Ne možeš modificirati završen projekt.");
                status = value;
            }
        }

        public Project(string name, string description, DateTime startDate, DateTime endDate)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Status = ProjectStatus.Active;
        }
    }
}
