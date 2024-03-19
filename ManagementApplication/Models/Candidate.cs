using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagementApplication.Models
{
    public class Candidate
    {
        public Candidate()
        {
                this.Degrees = new List<Degree>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid CandidateId { get; set; }

        [Required(ErrorMessage = "Please enter your last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter your first Name")]
        public string FirstName { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [MaxLength(10)]
        public string Mobile { get; set; }

        public virtual ICollection<Degree> Degrees { get; set; }


        [DefaultValue(ApplicationStatus.Initial)]
        public ApplicationStatus ApplicationStatus { get; set; }

        public string Comments { get; set; }


        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreationTime { get; set; }
    }
}
