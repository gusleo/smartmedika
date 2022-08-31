using dna.core.auth.Entity;
using dna.core.data.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MediCore.Data.Entities
{
    public class Notification : WriteHistoryBase, IEntityBase
    {
        
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }

        //Related identifier
        public int ObjectId { get; set; }

        [Required]
        public string TargetScreen { get; set; }

        public string JsonData { get; set; }
        public bool IsRead { get; set; }

        //send to
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
