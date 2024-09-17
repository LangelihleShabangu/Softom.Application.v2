using Softom.Application.BusinessRules.Common.Interfaces;
using Softom.Application.Infrustructure.Data;
using Softom.Application.Models;

namespace Softom.Application.Infrustructure.Repository
{
    public class ContactInformationRepository : Repository<ContactInformation>, IContactInformationRepository
    {
        private readonly ApplicationDbContext db;
        public ContactInformationRepository(ApplicationDbContext _db) : base(_db) { db = _db; }
        public ContactInformation Update(ContactInformation entity)
        {
            entity.Modifieddate = DateTime.Now;
            db.Update(entity);
            return entity;
        }
    }
}
