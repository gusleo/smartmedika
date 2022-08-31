using Dna.Core.Base.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MediCore.Lamda.Data.Entitties
{
    public class HospitalAppointmentRating : IEntityBase
    {
        public int Id { get; set; }
        public int HospitalAppointmentId { get; set; }

        public int Rating { get; set; }

        [MaxLength(300)]
        public string Testimoni { get; set; }
    }
}
