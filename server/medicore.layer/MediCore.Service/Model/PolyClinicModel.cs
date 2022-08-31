using dna.core.service.Models.Abstract;
using System.ComponentModel.DataAnnotations;

namespace MediCore.Service.Model
{
    public class PolyClinicModel : IModelBase
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(100, ErrorMessage = "StringLength")]
        public string Name { get; set; }
    }
}