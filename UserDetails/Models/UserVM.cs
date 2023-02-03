using System.ComponentModel.DataAnnotations;

namespace UserDetails.Models
{
    public class UserVM
    {
        [Required(ErrorMessage = "*")]
        public Guid UserId { get; set; }
        [Required(ErrorMessage = "*")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "*")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "*")]
        public string EmailId { get; set; }

        [Required(ErrorMessage = "*")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "*")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "*")]
        public DateTime DOB { get; set; }
    }
}
