using Softom.Application.BusinessRules.Common.Interfaces;
using Softom.Application.Infrustructure.Data;
using Softom.Application.Models;

namespace Softom.Application.Infrustructure.Repository
{
    public class StatusRepository : Repository<Status>, IStatusRepository
    {
        private readonly ApplicationDbContext db;
        public StatusRepository(ApplicationDbContext _db) : base(_db) { db = _db; }

        public Status Update(Status entity)
        {
            entity.Modifieddate = DateTime.Now;
            db.Update(entity);
            db.SaveChangesAsync();
            return entity;
        }
    }
}
