using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assessment2_RecruitmentSystem.Models;

namespace Assessment2_RecruitmentSystem.Services
{
    public class jobsService
    {

        List<Job> _jobs = new List<Job>();

        /// <summary>
        /// Returns a list of all jobs currently recorded in the system.
        /// </summary>
        /// <returns>A list of <see cref="Job"/> objects representing all jobs.</returns>
        public List<Job> GetJobs()
        {
            return _jobs;
        }

        /// <summary>
        /// Adds a new unassigned job record to be stored in the system.
        /// </summary>
        /// <param name="job">The <see cref="Job"/> object to be added to the system.</param>
        public void AddJob(Job job)
        {
            _jobs.Add(job);
        }

        /// <summary>
        /// Updates the selected job's status to completed and returns the assigned contractor to the available pool.
        /// </summary>
        /// <param name="job">The <see cref="Job"/> that has been completed.</param>
        /// <param name="contractor">The <see cref="Contractor"/> assigned to the completed job.</param>
        public void CompleteJob(Job job, Contractor contractor)
        {
            job.Completed = true;
            contractor.AssignedJob = null;
        }

        /// <summary>
        /// Returns a list of all jobs currently recorded in the system that do not have a contractor assigned.
        /// </summary>
        /// <returns>A list of <see cref="Job"/> objects representing all unassigned jobs.</returns>
        public List<Job> GetUnassignedJobs()
        {
            return _jobs.Where(job => job.ContractorAssigned == null).ToList();
        }

        /// <summary>
        /// Returns a list of all job records that are currently assigned to a contractor and not yet completed.
        /// </summary>
        /// <returns>A list of <see cref="Job"/> objects representing active, assigned jobs.</returns>
        public List<Job> GetAssignedJobs()
        {
            return _jobs.Where(job => job.ContractorAssigned != null && job.Completed == false).ToList();
        }

        /// <summary>
        /// Assigns a job to the specified contractor and updates both accordingly.
        /// </summary>
        /// <param name="job">The <see cref="Job"/> to assign.</param>
        /// <param name="contractor">The <see cref="Contractor"/> to which the job is assigned.</param>
        public void AssignJob(Job job, Contractor contractor)
        {
            job.ContractorAssigned = contractor;
            contractor.AssignedJob = job;
        }
        /// <summary>
        /// Removes an existing job record from the system.
        /// </summary>
        /// <param name="job">The <see cref="Job"/> object to be removed from the system.</param>
        public void RemoveJob(Job job)
        {
            _jobs.Remove(job);
        }

        /// <summary>
        /// Returns a list of all jobs with a cost value that falls within a specified range, regardless of job status.
        /// </summary>
        /// <param name="minCost">The minimum cost value to be included in the search filter.</param>
        /// <param name="maxCost">The maximum cost value to be included in the search filter.</param>
        /// <returns>A list of jobs that meet the search criteria based on cost value.</returns>
        public List<Job> GetJobByCost(float minCost, float maxCost)
        // Used for monthly reporting when using only cost filters, to meet business requirements
        {
            return _jobs.Where(job => job.Cost >= minCost && job.Cost <= maxCost).ToList();
        }

        /// <summary>
        /// Returns a list of all jobs with a date value that falls within a specified range, regardless of job status.
        /// </summary>
        /// <param name="minDate">The earliest date value to be included in the search filter.</param>
        /// <param name="maxDate">The latest date value to be included in the search filter.</param>
        /// <returns>A list of jobs that meet the search criteria based on their date value.</returns>
        public List<Job> GetJobsByDate(DateOnly? minDate, DateOnly? maxDate)
        // Used for monthly reporting if filtering on dates only, additional function
        {
            return _jobs.Where(job => job.Date.HasValue && job.Date.Value >= minDate && job.Date.Value <= maxDate).ToList();
        }

        /// <summary>
        /// Returns a list of all jobs that meet fall within the specified cost and date range when all search filters have been used, regardless of job status.
        /// </summary>
        /// <param name="minCost">The minimum cost value to be included in the search filter.</param>
        /// <param name="maxCost">The maximum cost value to be included in the search filter.</param>
        /// <param name="minDate">The earliest date value to be included in the search filter.</param>
        /// <param name="maxDate">The latest date value to be included in the search filter.</param>
        /// <returns>A list of all jobs that meet the search criteria based on both their cost and date values.</returns>
        public List<Job> GetJobsByDateAndCost(float minCost, float maxCost, DateOnly? minDate, DateOnly? maxDate)
        // Used for monthly reporting when using both cost and date filters
        {
            return _jobs.Where(job => job.Cost >= minCost && job.Cost <= maxCost && job.Date.HasValue && job.Date.Value >= minDate && job.Date.Value <= maxDate).ToList();
        }

    }
}
