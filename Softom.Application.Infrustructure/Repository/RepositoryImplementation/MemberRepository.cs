using Softom.Application.BusinessRules.Common.Interfaces;
using Softom.Application.Infrustructure.Data;
using Softom.Application.Models;

namespace Softom.Application.Infrustructure.Repository
{
    public class MemberRepository : Repository<Member>, IMemberRepository
    {
        private readonly ApplicationDbContext db;
        public MemberRepository(ApplicationDbContext _db) : base(_db) { db = _db; }
        public Member Update(Member entity)
        {
            entity.Modifieddate = DateTime.Now;
            db.Update(entity);
            return entity;
        }
    }
}
