using dna.core.service.Models.Abstract;
using MediCore.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Service.Model
{
    public class HospitalMedicalStaffModel : WriteHistoryBaseModel, IModelBase
    {
        public int Id { get; set; }
        public int HospitalId { get; set; }
        public int MedicalStaffId { get; set; }
       
        public virtual HospitalModel Hospital { get; set; }

       
        public virtual MedicalStaffModel MedicalStaff { get; set; }

      
        public HospitalStaffStatus Status { get; set; }

        //estimate time in minutes
        public Nullable<int> EstimateTimeForPatient { get; set; }

        public virtual ICollection<HospitalStaffOperatingHoursModel> OperatingHours { get; set; }
    }
}
