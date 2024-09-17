
using Softom.Application.Models;

namespace Softom.Application.BusinessRules.Common.Interfaces
{
    public interface IMemberRepository: IRepository<Member>
    {
        Member Update(Member entity);
    }
}
