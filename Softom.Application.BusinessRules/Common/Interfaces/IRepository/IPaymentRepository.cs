using Softom.Application.Models;

namespace Softom.Application.BusinessRules.Common.Interfaces
{
    public interface IPaymentRepository: IRepository<Payment>
    {
        Payment Update(Payment entity);
    }
}
