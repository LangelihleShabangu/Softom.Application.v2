
using Softom.Application.Models;

namespace Softom.Application.BusinessRules.Common.Interfaces
{
    public interface IMember_VehicleRepository: IRepository<Member_Vehicle>
    {
        Member_Vehicle Update(Member_Vehicle entity);
    }
}
