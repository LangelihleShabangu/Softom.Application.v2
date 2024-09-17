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
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext _db;

        public VillaNumberRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }
        
        public void Update(VillaNumber entity)
        {
            _db.VillaNumbers.Update(entity);
        }
    }
}
