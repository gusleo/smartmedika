using Dna.Core.Base.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MediCore.Lamda.Data.Entitties
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

        public double? Rating { get; set; }

        

    }
}
