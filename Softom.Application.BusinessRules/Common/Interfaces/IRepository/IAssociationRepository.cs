using Softom.Application.Models;

namespace Softom.Application.BusinessRules.Common.Interfaces
{
    public interface IAssociationRepository: IRepository<Association>
    {
        Association Update(Association entity);
    }
}
