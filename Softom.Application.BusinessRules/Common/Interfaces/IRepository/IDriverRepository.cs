
using Softom.Application.Models;

namespace Softom.Application.BusinessRules.Common.Interfaces
{
    public interface IDriverRepository: IRepository<Driver>
    {
        Driver Update(Driver entity);
    }
}
