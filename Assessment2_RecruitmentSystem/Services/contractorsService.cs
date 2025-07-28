using Assessment2_RecruitmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assessment2_RecruitmentSystem.Services
{
    public class contractorsService
    {
        List<Contractor> _contractors = new List<Contractor>();

        /// <summary>
        /// Returns a list of all contractors currently recorded in the system.
        /// </summary>
        /// <returns>A list of <see cref="Contractor"/> objects representing all contractors.</returns>
        public List<Contractor> GetContractors()
        {
            return _contractors;
        }

        /// <summary>
        /// Adds a new contractor record to be stored in the system.
        /// </summary>
        /// <param name="contractor">The <see cref="Contractor"/> object to be added to the system.</param>
        public void AddContractor(Contractor contractor)
        {
            _contractors.Add(contractor);
        }

        /// <summary>
        /// Removes an existing contractor record from the system.
        /// </summary>
        /// <param name="contractor">The <see cref="Contractor"/> object to be removed from the system.</param>
        public void RemoveContractor(Contractor contractor)
        {
            _contractors.Remove(contractor);
        }

        /// <summary>
        /// Assigns a job to the specified contractor and updates both accordingly.
        /// </summary>
        /// <param name="job">The <see cref="Job"/> to assign.</param>
        /// <param name="contractor">The <see cref="Contractor"/> to which the job is assigned.</param>
        public void AssignJob(Job job, Contractor contractor)
        {
            contractor.AssignedJob = job;
            job.ContractorAssigned = contractor;
        }

        /// <summary>
        /// Returns a list of all contractors currently recorded in the system that do not have an active job.
        /// </summary>
        /// <returns>A list of <see cref="Contractor"/> objects representing all contractors not currently assigned a job.</returns>
        public List<Contractor> GetAvailableContractors()
        // return a list of available contractors not currently on a job
        {
            return _contractors.Where(contractor => contractor.AssignedJob == null).ToList();
        }

    }
}
