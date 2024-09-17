using Softom.Application.BusinessRules.Common.Interfaces;
using Softom.Application.Infrustructure.Data;
using Softom.Application.Models;

namespace Softom.Application.Infrustructure.Repository
{
    public class PaymentTypeRepository : Repository<PaymentType>, IPaymentTypeRepository
    {
        private readonly ApplicationDbContext db;
        public PaymentTypeRepository(ApplicationDbContext _db) : base(_db) { db = _db; }

        public PaymentType Update(PaymentType entity)
        {
            entity.Modifieddate = DateTime.Now;
            db.Update(entity);
            return entity;
        }
    }
}
