using System.ComponentModel.DataAnnotations;

namespace JobApplicationTracker.Models
{
    public class JobApplication
    {
        public int id { get; set; }

        [Display (Name = "Company")]
        public string CompanyName { get; set; } = string.Empty;
        [Display (Name = "Job Title")]
        public  string JobTitle { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Status { get; set; } = "Applied";
        [Display (Name = "Date Applied")]
        public DateTime DateApplied { get; set; } = DateTime.Today;
        [Display (Name = "Salary Range")]
        public string? SalaryRange { get; set; }
        [Display (Name = "Job Posting URL")]   
        public string? JobPostingUrl { get; set; }
        public string? Notes { get; set; }
        }
    }

