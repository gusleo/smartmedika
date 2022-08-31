using dna.core.service.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Service.Model
{
    public class MedicalStaffSpecialistMapModel : IModelBase
    {
        public int Id { get; set; }
        public int MedicalStaffId { get; set; }
        public int MedicalStaffSpecialistId { get; set; }

       
        public virtual MedicalStaffModel MedicalStaff { get; set; }
       
        public virtual MedicalStaffSpecialistModel MedicalStaffSpecialist { get; set; }
    }
}
