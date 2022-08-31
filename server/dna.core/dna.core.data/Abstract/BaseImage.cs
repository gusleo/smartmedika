using System.ComponentModel.DataAnnotations;

namespace dna.core.data.Abstract
{
    public abstract class BaseImage : WriteHistoryBase, IEntityBase
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string ImagePath { get; set; }
        [Required, MaxLength(100)]
        public string ImageName { get; set; }
        
    }
}
