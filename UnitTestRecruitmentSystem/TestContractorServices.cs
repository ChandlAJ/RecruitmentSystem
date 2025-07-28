using Assessment2_RecruitmentSystem.Models;
using Assessment2_RecruitmentSystem.Services;

namespace UnitTestRecruitmentSystem
{
    [TestClass]
    public class ContractorsServiceTests
    {
        [TestMethod]
        public void GetContractors_ShouldReturnAllContractors()
        {
            var service = new contractorsService();
            var today = DateOnly.FromDateTime(DateTime.Today);
            var contractor1 = new Contractor("Bob", "Dylan", today, 50f, null);
            var contractor2 = new Contractor("Freddie", "Mercury", today, 50f, null);

            service.AddContractor(contractor1);
            service.AddContractor(contractor2);
            var result = service.GetContractors();

            Assert.IsTrue(result.Contains(contractor1));
            Assert.IsTrue(result.Contains(contractor2));
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void AddContractor_ShouldAddContractorToList()
        {

            var service = new contractorsService();
            var today = DateOnly.FromDateTime(DateTime.Today);
            var contractor = new Contractor("Bob", "Dylan", today, 50f, null);

            service.AddContractor(contractor);
            var result = service.GetContractors();

            Assert.IsTrue(result.Contains(contractor));
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void RemoveContractor_RemoveContractorFromList()
        {
            // Arrange
            var service = new contractorsService();
            var today = DateOnly.FromDateTime(DateTime.Today);
            var contractor = new Contractor("Bob", "Dylan", today, 50f, null);
            service.AddContractor(contractor);

            // Act
            service.RemoveContractor(contractor);
            var result = service.GetContractors();

            Assert.IsFalse(result.Contains(contractor));
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void AssignJob_ShouldAssignJobToContractorAndContractorToJob()
        {
            var service = new contractorsService();
            var today = DateOnly.FromDateTime(DateTime.Today);
            var contractor = new Contractor("Bob", "Dylan", today, 50f, null);
            var job = new Job("Rock Concert", today, 1200f, false, null);

            service.AssignJob(job, contractor);

            Assert.AreEqual(job, contractor.AssignedJob);
            Assert.AreEqual(contractor, job.ContractorAssigned);
        }

        [TestMethod]
        public void GetAvailableContractors_ShouldReturnOnlyContractorsWithNoAssignedJob()
        {
            var service = new contractorsService();
            var today = DateOnly.FromDateTime(DateTime.Today);
            var job = new Job("Rock Concert", today, 1200f, false, null);
            var available = new Contractor("Bob", "Dylan", today, 50f, null);
            var unavailable = new Contractor("Freddie", "Mercury", today, 50f, job);

            service.AddContractor(available);
            service.AddContractor(unavailable);

            var result = service.GetAvailableContractors();
            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.Contains(available));
            Assert.IsFalse(result.Contains(unavailable));
        }
    }
}
