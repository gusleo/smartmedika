using dna.core.data.Repositories;
using MediCore.Data.Entities;
using MediCore.Data.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace MediCore.Data.Repositories
{
    public class NotificationRepository : EntityReadWriteBaseRepository<Notification>, INotificationRepository
    {
        public IMediCoreContext MediCoreContext
        {
            get { return _context as IMediCoreContext; }
        }
        public NotificationRepository(IMediCoreContext context) : base(context)
        {
        }
    }
}
