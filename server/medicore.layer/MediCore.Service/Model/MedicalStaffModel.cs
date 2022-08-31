using dna.core.service.Models;
using dna.core.service.Models.Abstract;
using MediCore.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Service.Model
{
    public class MedicalStaffModel : WriteHistoryBaseModel, IModelBase
    {
        public int Id { get; set; }

        [StringLength(10, ErrorMessage = "StringLength")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(100, ErrorMessage = "StringLength")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(100, ErrorMessage = "StringLength")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Required")]
        public MedicalStaffType StaffType { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(100, ErrorMessage = "StringLength")]
        public string MedicalStaffRegisteredNumber { get; set; }


        
        [StringLength(100, ErrorMessage = "StringLength")]
        public string Email { get; set; }

        [StringLength(20, ErrorMessage = "StringLength")]
        [Required(ErrorMessage = "Required")]
        public string PrimaryPhone { get; set; }

        [StringLength(20, ErrorMessage = "StringLength")]
        public string SecondaryPhone { get; set; }

        //AssociatedToUserId allow null 
        //since, MedicalStaff is registered by Clinic and it means staff maybe not part of the user
        public Nullable<int> AssociatedToUserId { get; set; }

        public string Address { get; set; }

        public int RegencyId { get; set; }

        public double Distance { get; set; }

        [Required]
        public MedicalStaffStatus Status { get; set; }

        // rating summary, it will be fill by AWS Lamda Function
        public double? Rating { get; set; }

        // this properties not mapped to entities
        public bool IsFavorite { get; set; } 

        public virtual UserModel AssociatedToUser { get; set; }
        public virtual RegencyModel Regency { get; set; }
        public virtual ICollection<MedicalStaffSpecialistMapModel> MedicalStaffSpecialists { get; set; }
        public virtual ICollection<HospitalMedicalStaffModel> MedicalStaffClinics { get; set; }
        public virtual ICollection<MedicalStaffImageModel> Images { get; set; }
    }
}
