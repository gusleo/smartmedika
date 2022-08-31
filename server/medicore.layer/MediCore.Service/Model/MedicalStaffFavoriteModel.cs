using dna.core.service.Models;
using dna.core.service.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediCore.Service.Model
{
    public class MedicalStaffFavoriteModel : WriteHistoryBaseModel, IModelBase
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MedicalStaffId { get; set; }

        public virtual UserModel User { get; set; }
        public virtual MedicalStaffModel MedicalStaff { get; set; }
    }
}
