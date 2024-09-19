using Microsoft.AspNetCore.Hosting;
using Softom.Application.BusinessRules.Common.Interfaces;
using Softom.Application.BusinessRules.Services.Interface;
using Softom.Application.Models;
using Softom.Application.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softom.Application.BusinessRules.Services.Implementation
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentServices(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void CreatePayment(Payment Payment)
        {
            _unitOfWork.Payment.Add(Payment);
            _unitOfWork.Save();
        }

        public bool DeletePayment(int id)
        {
            try
            {
                Payment? objFromDb = _unitOfWork.Payment.Get(u => u.PaymentId == id);
                if (objFromDb is not null)
                {
                    _unitOfWork.Payment.Remove(objFromDb);
                    _unitOfWork.Save();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public IEnumerable<Payment> GetAllPayment()
        {
            return _unitOfWork.Payment.GetAll(includeProperties:"Member,PaymentStatus,PaymentType");
        }

        public Payment GetPaymentById(int id)
        {
            return _unitOfWork.Payment.Get(u => u.PaymentId == id, includeProperties: "Member,PaymentStatus,PaymentType");
        }

        public void UpdatePayment(Payment Payment)
        {
            _unitOfWork.Payment.Update(Payment);
            _unitOfWork.Save();
        }
    }
}
