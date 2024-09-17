using Softom.Application.Models;
namespace Softom.Application.BusinessRules.Common.Interfaces
{
    public interface IVehicleRepository: IRepository<Vehicle>
    {
        Vehicle Update(Vehicle entity);
    }
}
