using System;
using System.Collections.Generic;
using System.Text;

namespace Dna.Core.Base.Data
{
    public interface IWriteBaseRepository<T> where T : class, IEntityBase, new()
    {
        void Add(T entity);
        void AddRange(IEnumerable<T> entitites);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entitites);
        void Edit(T entity);
        void EditRange(IEnumerable<T> entitites);
        int SaveChanges();
    }
}
