using dna.core.data.Infrastructure;
using dna.core.service.Models.Abstract;
using System.ComponentModel.DataAnnotations;

namespace MediCore.Service.Model
{
    public class UTCTimeBaseModel : IModelBase
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Required")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(100, ErrorMessage = "StringLength")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(10, ErrorMessage = "StringLength")]
        public string Code { get; set; }

        //UTC is +8, -8, etc
        [Required(ErrorMessage = "Required")]
        public int UTC { get; set; }

        [Required]
        public Status Status { get; set; }

        public virtual CountryModel Country { get; set; }
    }
}
