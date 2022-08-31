using dna.core.data.Infrastructure;
using dna.core.service.Models.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MediCore.Service.Model
{
    public class RegionModel : IModelBase
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Required")]
        [StringLength(100, ErrorMessage = "StringLength")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required")]
        public int CountryId { get; set; }
        [Required(ErrorMessage = "Required")]
        public int UTCId { get; set; }

        [Required]
        public Status Status { get; set; }

        public virtual CountryModel Country { get; set; }

       
        public virtual UTCTimeBaseModel UTCTime { get; set; }

        public virtual ICollection<RegencyModel> Regencys { get; set; }
    }
}
