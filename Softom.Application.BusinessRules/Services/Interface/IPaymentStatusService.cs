using Softom.Application.Models;
using Softom.Application.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softom.Application.BusinessRules.Services.Interface
{
    public interface IPaymentStatusService
    {
        IEnumerable<PaymentStatus> GetAllPaymentStatus();
        void CreatePaymentStatus(PaymentStatus PaymentStatus);
        void UpdatePaymentStatus(PaymentStatus PaymentStatus);
        PaymentStatus GetPaymentStatusById(int id);
        bool DeletePaymentStatus(int id);
    }
}