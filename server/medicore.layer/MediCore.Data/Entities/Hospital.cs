using dna.core.data.Abstract;
using MediCore.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Data.Entities
{
    public class Hospital : WriteHistoryBase, IEntityBase
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        public string ZipCode { get; set; }

        public Nullable<double> Latitude { get; set; }
        public Nullable<double> Longitude { get; set; }
        [Required]
        public int RegencyId { get; set; }

        [Required]
        public bool IsHaveAmbulance { get; set; }

        [ForeignKey("RegencyId")]
        public Regency Regency { get; set; }

        [Required]
        public HospitalStatus Status { get; set; }

        public string Description { get; set; }

        [MaxLength(100)]
        public string Website { get; set; }

        [Required]
        [MaxLength(20)]
        public string PrimaryPhone { get; set; }

        [MaxLength(20)]
        public string SecondaryPhone { get; set; }

        [MaxLength(100)]
        public string PrimaryEmail { get; set; }

        [MaxLength(100)]
        public string SecondaryEmail { get; set; }

        [NotMapped]
        public double Distance { get; set; }

        // rating summary, it will be fill by AWS Lamda Function
        double? _rating;
        public double? Rating { 
            get{
                return _rating;
            } 
            internal set => _rating = value; 
        }

        public virtual ICollection<HospitalOperatingHours> OperatingHours { get; set; }
        public virtual ICollection<PolyClinicToHospitalMap> PolyClinicMaps { get; set; }
        public virtual ICollection<HospitalImage> Images { get; set; }

       

    }
}
