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
    /// <summary>
    /// Class of mapping between user and hospital
    /// </summary>
    /// <remarks>
    /// User saved is user have role of Operator and Admin
    /// </remarks>
    public class HospitalOperatorModel : WriteHistoryBaseModel, IModelBase
    {
        public int Id { get; set; }

        [Required]
        public int HospitalId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public HospitalStaffStatus Status { get; set; }
        
        public virtual UserModel User { get; set; }
       
        public virtual HospitalModel Hospital { get; set; }
    }
}
