using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment2_RecruitmentSystem.Models
{
    public class Job
    {
        public string Title { get; set; }
        public DateOnly? Date { get; set; }
        public float Cost { get; set; }
        public bool Completed { get; set; }
        public Contractor ContractorAssigned { get; set; }

        public Job(string title, DateOnly? date, float cost, bool completion, Contractor contractor)
        {
            Title = title;
            Date = date;
            Cost = cost;
            Completed = completion;
            ContractorAssigned = contractor;
              
        }
        public override string ToString()
        {
            if (ContractorAssigned ==  null)
            {
                return $"{Title} ({Date}) - ${Cost}";
            }
            else
            {
                return $"{Title} ({Date}) - ${Cost} - Completed: {Completed} - Contractor: {ContractorAssigned.FirstName} {ContractorAssigned.LastName}";
            }
        }

    }
}
