using Softom.Application.BusinessRules.Common.Interfaces;
using Softom.Application.Infrustructure.Data;
using Softom.Application.Models;

namespace Softom.Application.Infrustructure.Repository
{
    public class DriverRepository : Repository<Driver>, IDriverRepository
    {
        private readonly ApplicationDbContext db;
        public DriverRepository(ApplicationDbContext _db) : base(_db) { db = _db; }
        public Driver Update(Driver entity)
        {
            entity.Modifieddate = DateTime.Now;
            db.Update(entity);
            return entity;
        }
    }
}
