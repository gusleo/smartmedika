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
    public class PatientModel : WriteHistoryBaseModel, IModelBase
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(100, ErrorMessage = "StringLength")]
        public string PatientName { get; set; }

        [Required(ErrorMessage = "Required")]        
        public RelationshipStatus RelationshipStatus { get; set; }

        [Required(ErrorMessage = "Required")]
        
        public PatientStatus PatientStatus { get; set; }

        [Required(ErrorMessage = "Required")]
        public Gender Gender { get; set; }

        public Nullable<DateTime> DateOfBirth { get; set; }


        //if patient is user
        public Nullable<int> AssociatedUserId { get; set; }

      
        public virtual UserModel AssociatedUser { get; set; }

    }
}
