using dna.core.data.Infrastructure;
using dna.core.service.Models.Abstract;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MediCore.Service.Model
{
    public class CountryModel : IModelBase
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(100, ErrorMessage = "StringLength")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(5, ErrorMessage = "StringLength")]
        public string Code { get; set; }

        [Required]
        public Status Status { get; set; }

        public virtual ICollection<RegionModel> Regions { get; set; }
    }
}
