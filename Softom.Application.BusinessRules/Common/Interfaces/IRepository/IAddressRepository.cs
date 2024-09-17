using Softom.Application.Models;

namespace Softom.Application.BusinessRules.Common.Interfaces
{
    public interface IAddressRepository: IRepository<Address>
    {
        Address Update(Address entity);
    }
}
