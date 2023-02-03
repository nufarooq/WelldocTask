using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserDetails.Models
{
    public class UserRegistration
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(50)]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(50)]
        public string Password { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(20)]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "*")]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required(ErrorMessage = "*")]
        public DateTime DOB { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
