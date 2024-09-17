using Softom.Application.BusinessRules.Common.Interfaces;
using Softom.Application.Infrustructure.Data;
using Softom.Application.Models;

namespace Softom.Application.Infrustructure.Repository
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        private readonly ApplicationDbContext db;
        public AddressRepository(ApplicationDbContext _db) : base(_db) { db = _db; }
        public Address Update(Address entity)
        {
            entity.Modifieddate = DateTime.Now;
            db.Update(entity);
            db.SaveChangesAsync();
            return entity;
        }       
    }
}
