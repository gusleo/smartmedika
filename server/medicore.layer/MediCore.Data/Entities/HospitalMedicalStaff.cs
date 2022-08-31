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
    public class HospitalMedicalStaff : WriteHistoryBase, IEntityBase
    {
        public int Id { get; set; }
        public int HospitalId { get; set; }
        public int MedicalStaffId { get; set; }        

        [ForeignKey("HospitalId")]
        public virtual Hospital Hospital { get; set; }

        [ForeignKey("MedicalStaffId")]
        public virtual MedicalStaff MedicalStaff { get; set; }

        [Required]
        public HospitalStaffStatus Status { get; set; }

        //estimate time in minutes
        public Nullable<int> EstimateTimeForPatient { get; set; }
        public virtual ICollection<HospitalStaffOperatingHours> OperatingHours { get; set; }
    }
}
