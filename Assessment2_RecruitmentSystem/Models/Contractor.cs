using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment2_RecruitmentSystem.Models
{
    public class Contractor
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly? StartDate { get; set; }
        public float HourlyWage { get; set; }
        public Job? AssignedJob { get; set; }

        public Contractor(string firstName, string lastName, DateOnly? startDate, float hourlyWage, Job? assignedJob)
        {
            FirstName = firstName;
            LastName = lastName;
            StartDate = startDate;
            HourlyWage = hourlyWage;
            AssignedJob = assignedJob;
        }

        public override string ToString()
        {
            if (AssignedJob == null)
            {
                return $"{FirstName} {LastName} ({StartDate}) - Wage: ${HourlyWage}/h";

            }
            else
            {
                return $"{FirstName} {LastName} ({StartDate}) - Wage: ${HourlyWage}/h - Assigned Job: {AssignedJob}";

            }
        }

    }
}
