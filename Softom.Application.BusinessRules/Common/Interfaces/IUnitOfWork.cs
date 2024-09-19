using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softom.Application.BusinessRules.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IVillaRepository Villa { get; }
        IVillaNumberRepository VillaNumber { get; }
        IBookingRepository Booking { get; }
        IApplicationUserRepository User { get; }
        IAmenityRepository Amenity { get; }
		IAddressRepository Address { get; }
		IAssociationRepository Association { get; }
        IMemberRepository Member { get; }
        IContactInformationRepository ContactInformation { get; }
        IPaymentStatusRepository PaymentStatus { get; }
        IPaymentTypeRepository PaymentType { get; }
        IPaymentRepository Payment { get; }
        IStatusRepository Status { get; }
        void Save();
    }
}
