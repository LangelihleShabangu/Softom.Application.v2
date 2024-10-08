﻿using Microsoft.EntityFrameworkCore;
using Softom.Application.BusinessRules.Common.Interfaces;
using Softom.Application.Infrustructure.Data;
using Softom.Application.Models;

namespace Softom.Application.Infrustructure.Repository
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        private readonly ApplicationDbContext db;
        public PaymentRepository(ApplicationDbContext _db) : base(_db) { db = _db; }
        public Payment Update(Payment entity)
        {
            entity.Modifieddate = DateTime.Now;
            db.Update(entity);
            return entity;
        }

        public async Task<IEnumerable<Payment>> GetAsync()
        {
            try
            {
                return await db.Payment.FromSqlRaw("EXEC sp_GetPayments").ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
