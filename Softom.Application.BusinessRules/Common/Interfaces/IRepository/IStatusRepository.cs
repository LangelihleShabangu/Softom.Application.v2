using Softom.Application.Models;
namespace Softom.Application.BusinessRules.Common.Interfaces
{
    public interface IStatusRepository: IRepository<Status>
    {
        Status Update(Status entity);
    }
}
