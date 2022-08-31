using Dna.Core.Base.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Lamda.Data.Entitties
{
    public class MedicalStaff : WriteHistoryBase, IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(10)]
        public string Title { get; set; }

        [Required, MaxLength(100)]
        public string FirstName { get; set; }

        [Required, MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        public MedicalStaffType StaffType { get; set; }

        [Required, MaxLength(100)]
        public string MedicalStaffRegisteredNumber { get; set; }

        [MaxLength(100)]
        public string Email { get; set; }

        [Required, MaxLength(20)]
        public string PrimaryPhone { get; set; }

        [MaxLength(20)]
        public string SecondaryPhone { get; set; }

        public string Address { get; set; }

        [Required]
        [ForeignKey("Region")]
        public int RegencyId { get; set; }

        // rating summary, it will be fill by AWS Lamda Function
        public double? Rating { get; set; }



        //AssociatedToUserId allow null 
        //since, MedicalStaff is registered by Clinic and it means staff maybe not part of the user
        [ForeignKey("ApplicationUser")]
        public int? AssociatedToUserId { get; set; }

        
    }
}
