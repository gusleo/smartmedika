using dna.core.service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Service.Model
{
    public class UserDetailMediCoreModel : UserDetailModel
    {
        public UserDetailMediCoreModel() : base(){

        }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        public int? RegencyId { get; set; }
        public int? PatientId { get; set; }
        
        public virtual RegencyModel Regency { get; set; }
        public virtual PatientModel Patient { get; set; }
    }
}
