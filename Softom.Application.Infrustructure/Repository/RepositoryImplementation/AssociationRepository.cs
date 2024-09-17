using Softom.Application.BusinessRules.Common.Interfaces;
using Softom.Application.Infrustructure.Data;
using Softom.Application.Models;

namespace Softom.Application.Infrustructure.Repository
{
    public class AssociationRepository : Repository<Association>, IAssociationRepository
    {
        private readonly ApplicationDbContext db;
        public AssociationRepository(ApplicationDbContext _db) : base(_db) { db = _db; }
        public Association Update(Association entity)
        {
            entity.Modifieddate = DateTime.Now;
            db.Update(entity);
            return entity;
        }
    }
}
