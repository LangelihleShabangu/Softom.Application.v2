using Softom.Application.Models;

namespace Softom.Application.BusinessRules.Common.Interfaces
{
    public interface IPaymentTypeRepository: IRepository<PaymentType>
    {
        PaymentType Update(PaymentType entity);
    }
}
