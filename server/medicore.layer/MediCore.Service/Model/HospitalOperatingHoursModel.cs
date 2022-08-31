using dna.core.service.Models.Abstract;
using System.ComponentModel.DataAnnotations;

namespace MediCore.Service.Model
{
    public class HospitalOperatingHoursModel : WriteHistoryBaseModel, IModelBase
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required")]
        public int Day { get; set; }

        [Required(ErrorMessage = "Required")]
        public int HospitalId { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(10, ErrorMessage = "StringLength")]
        public string StartTime { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(10, ErrorMessage = "StringLength")]
        public string EndTime { get; set; }

        [Required(ErrorMessage = "Required")]
        public bool IsClossed { get; set; }

        public virtual HospitalModel Hospital { get; set; }
    }
}
