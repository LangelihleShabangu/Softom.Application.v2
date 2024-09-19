using Softom.Application.Models;
using Softom.Application.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softom.Application.BusinessRules.Services.Interface
{
    public interface IPaymentTypeService
    {
        IEnumerable<PaymentType> GetAllPaymentType();
        void CreatePaymentType(PaymentType PaymentType);
        void UpdatePaymentType(PaymentType PaymentType);
        PaymentType GetPaymentTypeById(int id);
        bool DeletePaymentType(int id);
    }
}