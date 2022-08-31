using dna.core.service.Models.Abstract;
using MediCore.Data.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;

namespace MediCore.Service.Model
{
    public class MedicalStaffSpecialistModel : IModelBase
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(100, ErrorMessage = "StringLength")]
        public string Name { get; set; }

        [StringLength(255, ErrorMessage = "StringLength")]
        public string Alias { get; set; }

        public Nullable<int> PolyClinicId { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "Required")]
        public MedicalStaffType StaffType { get; set; }


        
        public virtual PolyClinicModel PolyClinic { get; set; }
    }
}