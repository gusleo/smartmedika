using dna.core.service.Models.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Service.Model
{
    public class HospitalStaffOperatingHoursModel : WriteHistoryBaseModel, IModelBase
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public int Day { get; set; }

        [Required(ErrorMessage = "Required")]
        public int HospitalMedicalStaffId { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(10, ErrorMessage = "StringLength")]
        public string StartTime { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(10, ErrorMessage = "StringLength")]
        public string EndTime { get; set; }

        [Required(ErrorMessage = "Required")]
        public bool IsClossed { get; set; }

        public virtual HospitalMedicalStaffModel HospitalStaff { get; set; }

    }
}
