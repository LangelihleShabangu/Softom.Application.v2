using Microsoft.EntityFrameworkCore;
using Softom.Application.BusinessRules.Common.Interfaces;
using Softom.Application.Infrustructure.Data;
using Softom.Application.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Softom.Application.Infrustructure.Repository
{
    public class BookingRepository : Repository<Booking>, IBookingRepository
    {
        private readonly ApplicationDbContext _db;

        public BookingRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }
        
        public void Update(Booking entity)
        {
            _db.Bookings.Update(entity);
        }       
    }
}
