using dna.core.auth.Entity;
using dna.core.data.Abstract;
using MediCore.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Data.Entities
{
    public class MedicalStaff : WriteHistoryBase, IEntityBase
    {
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
        public int RegencyId { get; set; }

        // rating summary, it will be fill by AWS Lamda Function
        double? _rating;
        public double? Rating
        {
            get
            {
                return _rating;
            }
            // since it updated by lamda function
            // do not allow to edit from EF
            internal set => _rating = value;
        }

        [NotMapped]
        public double Distance { get; set; }


        //AssociatedToUserId allow null 
        //since, MedicalStaff is registered by Clinic and it means staff maybe not part of the user
        public int? AssociatedToUserId { get; set; }

        [ForeignKey("AssociatedToUserId")]
        public virtual ApplicationUser AssociatedToUser { get; set; }

        [Required]
        public MedicalStaffStatus Status { get; set; }

        [ForeignKey("RegencyId")]
        public virtual Regency Regency { get; set; }

        public virtual ICollection<MedicalStaffSpecialistMap> MedicalStaffSpecialists { get; set; }
        public virtual ICollection<HospitalMedicalStaff> MedicalStaffClinics { get; set; }
        public virtual ICollection<MedicalStaffImage> Images { get; set; }

        
    }
}
