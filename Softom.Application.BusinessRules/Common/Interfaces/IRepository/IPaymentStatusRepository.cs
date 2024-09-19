using Softom.Application.Models;

namespace Softom.Application.BusinessRules.Common.Interfaces
{
    public interface IPaymentStatusRepository: IRepository<PaymentStatus>
    {
        PaymentStatus Update(PaymentStatus entity);
    }
}
