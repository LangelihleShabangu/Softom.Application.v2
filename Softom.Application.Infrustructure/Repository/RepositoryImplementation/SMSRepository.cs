using Softom.Application.BusinessRules.Common.Interfaces;
using Softom.Application.Infrustructure.Data;
using Softom.Application.Models;

namespace Softom.Application.Infrustructure.Repository
{
    public class SMSRepository : Repository<SMS>, ISMSRepository
    {
        private readonly ApplicationDbContext db;
        public SMSRepository(ApplicationDbContext _db) : base(_db) { db = _db; }
        public SMS Update(SMS entity)
        {
            entity.Modifieddate = DateTime.Now;
            db.Update(entity);
            return entity;
        }
    }
}
