using Softom.Application.BusinessRules.Common.Interfaces;
using Softom.Application.Infrustructure.Data;
using Softom.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softom.Application.Infrustructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IVillaRepository Villa { get; private set; }
		public IAddressRepository Address { get; private set; }
		public IAmenityRepository Amenity { get; private set; }
        public IBookingRepository Booking { get; private set; }        
		public IApplicationUserRepository User { get; private set; }
		public IAssociationRepository Association { get; private set; }
        public IMemberRepository Member { get; private set; }
        public IVillaNumberRepository VillaNumber { get; private set; }
        public IContactInformationRepository ContactInformation { get; private set; }
        public IStatusRepository Status { get; private set; }
        public IPaymentRepository Payment { get; private set; }
        public IPaymentTypeRepository PaymentType { get; private set; }
        public IPaymentStatusRepository PaymentStatus { get; private set; }
        public IVehicleRepository Vehicle { get; private set; }
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Villa = new VillaRepository(_db);
            User = new ApplicationUserRepository(_db);
            Booking = new BookingRepository(_db);   
            Amenity = new AmenityRepository(_db);
			Address = new AddressRepository(_db);
			Association = new AssociationRepository(_db);
            VillaNumber = new VillaNumberRepository(_db);
            Member = new MemberRepository(_db);
            Member = new MemberRepository(_db);
            ContactInformation = new ContactInformationRepository(_db);
            Payment = new PaymentRepository(_db);
            Status = new StatusRepository(_db);
            PaymentType = new PaymentTypeRepository(_db);
            PaymentStatus = new PaymentStatusRepository(_db);
            Vehicle = new VehicleRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
