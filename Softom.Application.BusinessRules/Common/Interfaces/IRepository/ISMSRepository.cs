

using Softom.Application.Models;

namespace Softom.Application.BusinessRules.Common.Interfaces
{
    public interface ISMSRepository: IRepository<SMS>
    {
        SMS Update(SMS entity);
    }
}
