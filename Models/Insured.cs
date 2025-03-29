using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Api_InsuranceABC.Models
{
    [Table("tbl_InsuranceABC_Insureds")]
    public class Insured
    {
        [Key]
        public long IdNumber { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string FirstLastName { get; set; }

        [Required]
        public string MiddleLastName { get; set; }

        [Required, Phone]
        public string ContactPhone { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public decimal EstimatedValue { get; set; }

        public string Observations { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public DateTime UpDateDate { get; set; }
    }

}
