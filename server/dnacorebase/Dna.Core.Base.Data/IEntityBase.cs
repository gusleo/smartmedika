using System.ComponentModel.DataAnnotations;

namespace Dna.Core.Base.Data
{
    public interface IEntityBase
    {
        [Key]
        int Id { get; set; }
    }
}
