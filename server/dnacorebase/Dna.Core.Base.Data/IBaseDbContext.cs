using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dna.Core.Base.Data
{
    public interface IBaseDbContext
    {
        DatabaseFacade Database { get; }
        int SaveChanges();
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
        void Updated<TEntity>(TEntity entity) where TEntity : class;

        void Dispose();
    }
}
