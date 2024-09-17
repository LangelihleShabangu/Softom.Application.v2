using Softom.Application.BusinessRules.Common.Interfaces;
using Softom.Application.Infrustructure.Data;
using Softom.Application.Models;

namespace Softom.Application.Infrustructure.Repository
{
    public class Member_VehicleRepository : Repository<Member_Vehicle>, IMember_VehicleRepository
    {
        private readonly ApplicationDbContext db;
        public Member_VehicleRepository(ApplicationDbContext _db) : base(_db) { db = _db; }

        public Member_Vehicle Update(Member_Vehicle entity)
        {
            entity.Modifieddate = DateTime.Now;
            db.Update(entity);
            return entity;
        }
    }
}
