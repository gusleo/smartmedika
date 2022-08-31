using dna.core.auth.Entity;
using dna.core.data.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace dna.core.data.Entities
{
    public class ArticleFavorite : WriteHistoryBase, IEntityBase
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ArticleId { get; set; }

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }

        [ForeignKey("ArticleId")]
        public virtual Article Article { get; set; }
    }
}
