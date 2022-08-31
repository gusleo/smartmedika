using dna.core.service.Models.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace dna.core.service.Models
{
    public class ArticleFavoriteModel : WriteHistoryBaseModel, IModelBase
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ArticleId { get; set; }
        public virtual UserModel User { get; set; }
        public virtual ArticleModel Article { get; set; }
    }
}
