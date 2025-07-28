using Assessment2_RecruitmentSystem.Models;
using Assessment2_RecruitmentSystem.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestRecruitmentSystem
{
    [TestClass]
    public class TestJobsService
    {
        [TestMethod]
        public void AddJob_ShouldAddJobToList()
        {
            var service = new jobsService();
            var today = DateOnly.FromDateTime(DateTime.Today);
            var job = new Job("Sound Check", today, 950f, false, null);

            service.AddJob(job);
            var jobs = service.GetJobs();

            Assert.IsTrue(jobs.Contains(job));
            Assert.AreEqual(1, jobs.Count);
        }

        [TestMethod]
        public void RemoveJob_ShouldRemoveJobFromList()
        {
            var service = new jobsService();
            var today = DateOnly.FromDateTime(DateTime.Today);
            var job = new Job("Sound Check", today, 950f, false, null);
            service.AddJob(job);

            service.RemoveJob(job);
            var jobs = service.GetJobs();

            Assert.IsFalse(jobs.Contains(job));
            Assert.AreEqual(0, jobs.Count);
        }

        [TestMethod]
        public void GetUnassignedJobs_ShouldReturnOnlyUnassignedJobs()
        {
            var service = new jobsService();
            var today = DateOnly.FromDateTime(DateTime.Today);

            var unassigned = new Job("Stage Setup", today, 1000f, false, null);
            var contractor = new Contractor("Bob", "Dylan", today, 50f, null);
            var assigned = new Job("Sound Check", today, 1200f, false, contractor);
            contractor.AssignedJob = assigned;

            service.AddJob(unassigned);
            service.AddJob(assigned);

            var result = service.GetUnassignedJobs();

            Assert.IsTrue(result.Contains(unassigned));
            Assert.IsFalse(result.Contains(assigned));
            Assert.AreEqual(1, result.Count);
        }
        [TestMethod]
        public void GetAssignedJobs_ShouldReturnOnlyAssignedJobs()
        {
            var service = new jobsService();
            var today = DateOnly.FromDateTime(DateTime.Today);

            var unassigned = new Job("Stage Setup", today, 1000f, false, null);
            var contractor = new Contractor("Bob", "Dylan", today, 50f, null);
            var assigned = new Job("Sound Check", today, 1200f, false, contractor);
            contractor.AssignedJob = assigned;

            service.AddJob(unassigned);
            service.AddJob(assigned);

            var result = service.GetAssignedJobs();

            Assert.IsFalse(result.Contains(unassigned));
            Assert.IsTrue(result.Contains(assigned));
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void CompleteJob_ShouldUpdateJobStatusAndUnassignContractor()
        {
            var service = new jobsService();
            var today = DateOnly.FromDateTime(DateTime.Today);
            var job = new Job("Sound Check", today, 950f, false, null);
            var contractor = new Contractor("Bob", "Dylan", today, 50f, null);
            contractor.AssignedJob = job;

            service.CompleteJob(job, contractor);

            Assert.IsTrue(job.Completed);
            Assert.IsNull(contractor.AssignedJob);
        }

        [TestMethod]
        public void AssignJob_ShouldAssignContractorToJobAndJobToContractor()
        {
            var service = new jobsService();
            var today = DateOnly.FromDateTime(DateTime.Today);
            var job = new Job("Sound Check", today, 950f, false, null);
            var contractor = new Contractor("Bob", "Dylan", today, 50f, null);
            contractor.AssignedJob = job;

            service.AssignJob(job, contractor);

            Assert.AreEqual(contractor, job.ContractorAssigned);
            Assert.AreEqual(job, contractor.AssignedJob);
        }
        [TestMethod]
        public void GetJobByCost_ShouldOnlyReturnsJobsWithinCostRange()
        {
            var service = new jobsService();
            var today = DateOnly.FromDateTime(DateTime.Today);
            var job1 = new Job("Sound Check", today, 800f, false, null);
            var job2 = new Job("Private Function", today, 1500f, false, null);
            var job3 = new Job("World Tour", today, 2200f, false, null);

            service.AddJob(job1);
            service.AddJob(job2);
            service.AddJob(job3);

            var result = service.GetJobByCost(1000f, 2000f);

            Assert.IsTrue(result.Contains(job2));
            Assert.IsFalse(result.Contains(job1));
            Assert.IsFalse(result.Contains(job3));
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void GetJobsByDate_ShouldOnlyReturnJobsWithinDateRange()
        {
            var service = new jobsService();
            var date1 = new DateOnly(2024, 12, 1);
            var date2 = new DateOnly(2025, 1, 15);
            var date3 = new DateOnly(2025, 2, 10);
            var job1 = new Job("Sound Check", date1, 800f, false, null);
            var job2 = new Job("Private Function", date2, 1500f, false, null);
            var job3 = new Job("World Tour", date3, 2200f, false, null);
            service.AddJob(job1);
            service.AddJob(job2);
            service.AddJob(job3);

            var result = service.GetJobsByDate(date1, date2);

            Assert.IsTrue(result.Contains(job1));
            Assert.IsTrue(result.Contains(job2));
            Assert.IsFalse(result.Contains(job3));
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void GetJobsByDateAndCost_ShouldOnlyReturnJobsWithinDateAndCostRange()
        {
            var service = new jobsService();
            var date1 = new DateOnly(2024, 12, 1);
            var date2 = new DateOnly(2025, 1, 15);
            var date3 = new DateOnly(2025, 2, 10);
            var job1 = new Job("Sound Check", date1, 800f, false, null);
            var job2 = new Job("Private Function", date2, 1500f, false, null);
            var job3 = new Job("World Tour", date3, 2200f, false, null);
            service.AddJob(job1);
            service.AddJob(job2);
            service.AddJob(job3);

            var result = service.GetJobsByDateAndCost(600f, 1600f, date2, date3);

            Assert.IsFalse(result.Contains(job1));
            Assert.IsTrue(result.Contains(job2));
            Assert.IsFalse(result.Contains(job3));
            Assert.AreEqual(1, result.Count);
        }
    }
}
