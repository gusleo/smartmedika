using dna.core.auth.Entity;
using dna.core.data.Abstract;
using MediCore.Data.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MediCore.Data.Entities
{

    public class Patient : WriteHistoryBase, IEntityBase
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string PatientName { get; set; }

        [Required]
        public RelationshipStatus RelationshipStatus { get; set; }

        //soft deleted
        [Required]
        public PatientStatus PatientStatus { get; set; }

        public Nullable<DateTime> DateOfBirth { get; set; }
       
        [Required]
        public Gender Gender { get; set; }

        //if patient is user
        public Nullable<int> AssociatedUserId { get; set; }

        [ForeignKey("AssociatedUserId")]
        public virtual ApplicationUser AssociatedUser { get; set; }
       
    }
}
