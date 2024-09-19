﻿using Softom.Application.BusinessRules.Common.Interfaces;
using Softom.Application.Infrustructure.Data;
using Softom.Application.Models;

namespace Softom.Application.Infrustructure.Repository
{
    public class PaymentStatusRepository : Repository<PaymentStatus>, IPaymentStatusRepository
    {
        private readonly ApplicationDbContext db;
        public PaymentStatusRepository(ApplicationDbContext _db) : base(_db) { db = _db; }

        public PaymentStatus Update(PaymentStatus entity)
        {
            entity.Modifieddate = DateTime.Now;
            db.Update(entity);
            return entity;
        }
    }
}
