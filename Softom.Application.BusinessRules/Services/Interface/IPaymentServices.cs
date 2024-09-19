using Softom.Application.Models;
using Softom.Application.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softom.Application.BusinessRules.Services.Interface
{
    public interface IPaymentServices
    {
        IEnumerable<Payment> GetAllPayment();
        void CreatePayment(Payment Payment);
        void UpdatePayment(Payment Payment);
        Payment GetPaymentById(int id);
        bool DeletePayment(int id);
    }
}