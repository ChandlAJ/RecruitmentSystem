using Assessment2_RecruitmentSystem.Models;
using Assessment2_RecruitmentSystem.Services;
using System.Diagnostics.Eventing.Reader;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Assessment2_RecruitmentSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        contractorsService _contractorService = new contractorsService();
        jobsService _jobsService = new jobsService();
        public MainWindow()
        {
            InitializeComponent();
            _contractorService.AddContractor(new Contractor("Robert", "Plant", new DateOnly(2025, 05, 30), 60.50f, null));
            _contractorService.AddContractor(new Contractor("Jimmy", "Page", new DateOnly(2025, 06, 01), 65.00f, null));
            _contractorService.AddContractor(new Contractor("John", "Bonham", new DateOnly(2025, 01, 15), 55.30f, null));
            _jobsService.AddJob(new Job("Selection Report", new DateOnly(2025, 06, 12), 255.40f, false, null));
            _jobsService.AddJob(new Job("Rock Concert", new DateOnly(2025, 07, 10), 1200.00f, false, null));
            _jobsService.AddJob(new Job("Encore Show", new DateOnly(2025, 07, 11), 950.50f, false, null));
        }
        public void LoadAvailableContractors_Click(object sender, RoutedEventArgs e)
        {
            ListBoxContractors.ItemsSource = _contractorService.GetAvailableContractors();
        }

        public void ContractorDataEntryClear() // Used to clear data from contractor textboxes
        {
            TextBoxFirstName.Clear();
            TextBoxLastName.Clear();
            TextBoxWage.Clear();
            DatePickerStartDate.SelectedDate = null;
        }
        public void JobDataEntryClear() // Used to clear data from jobs textboxes
        {
            TextBoxTitle.Clear();
            TextBoxCost.Clear();
            DatePickerJobDate.SelectedDate = null;
        }

        public void RefreshListBoxes() // to be used throughout code to include automatic refresh of listboxes upon actions
        {
            ListBoxContractors.ItemsSource = _contractorService.GetAvailableContractors();
            ListBoxUnassignedJobs.ItemsSource = _jobsService.GetUnassignedJobs();
            ListBoxActiveJobs.ItemsSource = _jobsService.GetAssignedJobs();
        }


        public void AddContractor_Click(object sender, RoutedEventArgs e) // To add new contractor to the system
        {
            string firstName = TextBoxFirstName.Text.Trim();
            string lastName = TextBoxLastName.Text.Trim();
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                MessageBox.Show("Contractor must have a valid first and last name.");
                return;
            }
            DateTime? selectedDate = DatePickerStartDate.SelectedDate;
            DateOnly? startDate = selectedDate.HasValue ? DateOnly.FromDateTime(selectedDate.Value) : null;
            if (!startDate.HasValue) 
            {
                MessageBox.Show("Contract must have a start date");
                return;
            }
            if (!float.TryParse(TextBoxWage.Text, out float wage)) // wage input validation
            {
                MessageBox.Show("Please enter a valid wage amount.");
                return;
            }
            Contractor contractor = new Contractor(firstName, lastName, startDate, wage, null);
            _contractorService.AddContractor(contractor);
            MessageBox.Show("New contractor added to the system.");
            ContractorDataEntryClear();
            RefreshListBoxes();
        }
        public void RemoveContractor_Click(Object sender, RoutedEventArgs e)
        {
            Contractor contractorToRemove = ListBoxContractors.SelectedItem as Contractor;
            if (contractorToRemove != null)
            {
                if (contractorToRemove.AssignedJob != null) // check if contractor is assigned a job, unable to remove if so
                {
                    MessageBox.Show("Unable to remove a contractor who is assigned a job.");
                    return;
                }
                else
                {
                    _contractorService.RemoveContractor(contractorToRemove);
                    MessageBox.Show("Contractor removed successfully.");
                }
            }
            else
            {
                MessageBox.Show("Contractor not found.");
            }
            ContractorDataEntryClear();
            RefreshListBoxes();
        }
        public void LoadUnassignedJobs_Click(object sender, RoutedEventArgs e)
        {
            ListBoxUnassignedJobs.ItemsSource = _jobsService.GetUnassignedJobs();
        }
        public void AddJob_Click(object sender, RoutedEventArgs e) // To add a new job to the system
        {
            string jobTitle = TextBoxTitle.Text.Trim();
            if (string.IsNullOrEmpty(jobTitle))
            {
                MessageBox.Show("Job must have a title.");
                return;
            }
            DateTime? selectedDate = DatePickerJobDate.SelectedDate;
            DateOnly? jobDate = selectedDate.HasValue ? DateOnly.FromDateTime(selectedDate.Value) : null;
            if (!jobDate.HasValue)
            {
                MessageBox.Show("You must include a job date.");
                return;
            }
            if (!float.TryParse(TextBoxCost.Text, out float cost))
            {
                MessageBox.Show("Please enter a valid cost amount.");
                return;
            }
            Job job = new Job(jobTitle, jobDate, cost, false, null);
            _jobsService.AddJob(job);
            MessageBox.Show("New job added to the system.");
            JobDataEntryClear();
            RefreshListBoxes();
        }

        public void RemoveJob_Click(Object sender, RoutedEventArgs e) // added remove function for invalid/cancelled jobs
        {
            Job jobToRemove = ListBoxUnassignedJobs.SelectedItem as Job;
            if (jobToRemove != null)
            {
                _jobsService.RemoveJob(jobToRemove);
                MessageBox.Show("Job successfully removed.");
            }
            JobDataEntryClear();
            RefreshListBoxes();
        }

        public void AssignJob_Click(object sender, RoutedEventArgs e)
        {
            Job selectedJob = ListBoxUnassignedJobs.SelectedItem as Job;
            Contractor selectedContractor = ListBoxContractors.SelectedItem as Contractor;
            if (selectedJob != null && selectedContractor != null) // validation check to confirm both contractor and job selected
            {
                _jobsService.AssignJob(selectedJob, selectedContractor);
                MessageBox.Show($"Job has been assigned to {selectedContractor.FirstName} {selectedContractor.LastName}");
            }
            else
            {
                MessageBox.Show("You must select a contractor and a job.");
            }
            RefreshListBoxes();
        }

        public void CompleteJob_Click(Object sender, RoutedEventArgs e) // To complete job and return contractor to available
        {
            Job selectedJob = ListBoxActiveJobs.SelectedItem as Job;
            Contractor selectedContractor = selectedJob?.ContractorAssigned as Contractor;
            if (selectedJob != null)
            {
                _jobsService.CompleteJob(selectedJob, selectedContractor);
                MessageBox.Show($"Job successfully completed, {selectedContractor.FirstName} {selectedContractor.LastName} available for more work.");
            }
            else
            {
                MessageBox.Show("Please select and active job.");
            }
            RefreshListBoxes();
        }
        public void LoadAssignedJobs_Click(object sender, RoutedEventArgs e)
        {
            ListBoxActiveJobs.ItemsSource = _jobsService.GetAssignedJobs();
            RefreshListBoxes();
        }

        public void LoadJobsReport_Click(object sender, RoutedEventArgs e)
        {
            DateTime? startDate = DatePickerDateStart.SelectedDate;
            DateOnly? minDate = startDate.HasValue ? DateOnly.FromDateTime(startDate.Value) : null;
            DateTime? endDate = DatePickerDateEnd.SelectedDate;
            DateOnly? maxDate = endDate.HasValue ? DateOnly.FromDateTime(endDate.Value) : null;
            bool minCostValid = float.TryParse(TextBoxMinCost.Text, out float minCost);
            bool maxCostValid = float.TryParse(TextBoxMaxCost.Text, out float maxCost);
            // If both cost fields are empty, filter only by dates
            if (string.IsNullOrWhiteSpace(TextBoxMinCost.Text) && string.IsNullOrWhiteSpace(TextBoxMaxCost.Text))
            {
                if (minDate != null && maxDate != null && minDate < maxDate)
                {
                    ListBoxJobsReport.ItemsSource = _jobsService.GetJobsByDate(minDate, maxDate);
                    return;
                }
                else if (minDate == null && maxDate == null)
                {
                    ListBoxJobsReport.ItemsSource = _jobsService.GetJobs();
                    return;
                }
                else
                {
                    MessageBox.Show("Please select a valid date range.");
                    return;
                }
            }
            // If cost values are provided, validate them and apply filters
            if (!minCostValid || !maxCostValid || maxCost < minCost)
            {
                MessageBox.Show("Please enter a valid cost amount.");
                return;
            }
            if (minCost != null && maxCost != null && minDate == null && maxDate == null)
            {
                ListBoxJobsReport.ItemsSource = _jobsService.GetJobByCost(minCost, maxCost);
                return;
            }
            else if (minCost != null && maxCost != null && minDate != null && maxDate != null && minDate < maxDate)
            {
                ListBoxJobsReport.ItemsSource = _jobsService.GetJobsByDateAndCost(minCost, maxCost, minDate, maxDate);
                return;
            }

            else
            {
                MessageBox.Show("Please select a valid date range.");
                return;
            }
        }
        public void ClearJobsReport_Click(Object sender, RoutedEventArgs e)
            // Clears all aspects of the reporting page
        {
            ListBoxJobsReport.ItemsSource= null;
            TextBoxMinCost.Text = string.Empty;
            TextBoxMaxCost.Text = string.Empty;
            DatePickerDateEnd.SelectedDate = null;
            DatePickerDateStart.SelectedDate = null;
        }
    }
}



