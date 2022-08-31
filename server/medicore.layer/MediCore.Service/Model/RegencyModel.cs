using dna.core.data.Infrastructure;
using dna.core.service.Models.Abstract;
using System.ComponentModel.DataAnnotations;

namespace MediCore.Service.Model
{
    public class RegencyModel : IModelBase
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(100, ErrorMessage = "StringLength")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required")]
        public int RegionId { get; set; }

        [Required]
        public Status Status { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public virtual RegionModel Region { get; set; }
    }
}
