using dna.core.service.Models.Abstract;
using MediCore.Data.Entities;
using MediCore.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MediCore.Service.Model
{
    public class HospitalModel : WriteHistoryBaseModel, IModelBase
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(100, ErrorMessage = "StringLength")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required")]
        public string Address { get; set; }

        public string ZipCode { get; set; }

        public Nullable<double> Latitude { get; set; }
        public Nullable<double> Longitude { get; set; }
        [Required(ErrorMessage = "Required")]
        public int RegencyId { get; set; }

        [Required(ErrorMessage = "Required")]
        public bool IsHaveAmbulance { get; set; }      
        

        [Required(ErrorMessage = "Required")]
        public HospitalStatus Status { get; set; }

        public string Description { get; set; }

        [StringLength(100, ErrorMessage = "StringLength")]
        public string Website { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(20, ErrorMessage = "StringLength")]
        public string PrimaryPhone { get; set; }

        [StringLength(20, ErrorMessage = "StringLength")]
        public string SecondaryPhone { get; set; }

        [StringLength(100, ErrorMessage = "StringLength")]
        public string PrimaryEmail { get; set; }

        [StringLength(100, ErrorMessage = "StringLength")]
        public string SecondaryEmail { get; set; }

        public double Distance { get; set; }

        // rating summary, it will be fill by AWS Lamda Function
        public double? Rating { get; set; }

        public virtual RegencyModel Regency { get; set; }

        public virtual ICollection<HospitalOperatingHoursModel> OperatingHours { get; set; }
        public virtual ICollection<PolyClinicToHospitalMapModel> PolyClinicMaps { get; set; }
        public virtual ICollection<HospitalImageModel> Images { get; set; }
    }
}
