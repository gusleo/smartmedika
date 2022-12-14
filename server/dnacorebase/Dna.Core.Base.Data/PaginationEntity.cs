using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dna.Core.Base.Data
{
    public class PaginationEntity<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }

        public int Count
        {
            get
            {
                return (null != this.Items) ? this.Items.Count() : 0;
            }
        }


        public int TotalCount { get; set; }
        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling((decimal)TotalCount / PageSize);
            }
        }

        public IEnumerable<T> Items { get; set; }
    }
}
