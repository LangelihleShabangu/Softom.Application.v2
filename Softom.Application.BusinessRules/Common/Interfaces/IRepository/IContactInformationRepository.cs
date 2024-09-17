
using Softom.Application.Models;

namespace Softom.Application.BusinessRules.Common.Interfaces
{
    public interface IContactInformationRepository: IRepository<ContactInformation>
    {
        ContactInformation Update(ContactInformation entity);
    }
}
