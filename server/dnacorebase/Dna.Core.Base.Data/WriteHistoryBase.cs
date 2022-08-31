using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Dna.Core.Base.Data
{
    public abstract class WriteHistoryBase
    {
        [Required]
        [ForeignKey("ApplicationUser")]
        public int CreatedById { get; set; }

        [Required]
        [ForeignKey("ApplicationUser")]
        public int UpdatedById { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
