using Softom.Application.BusinessRules.Common.Interfaces;
using Softom.Application.Infrustructure.Data;
using Softom.Application.Models;

namespace Softom.Application.Infrustructure.Repository
{
    public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
    {
        private readonly ApplicationDbContext db;
        public VehicleRepository(ApplicationDbContext _db) : base(_db) { db = _db; }

        public Vehicle Update(Vehicle entity)
        {
            entity.Modifieddate = DateTime.Now;
            db.Update(entity);
            db.SaveChangesAsync();
            return entity;
        }
    }
}
