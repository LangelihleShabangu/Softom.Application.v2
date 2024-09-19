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
    public class PaymentStatusService : IPaymentStatusService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentStatusService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
       
        public void CreatePaymentStatus(PaymentStatus PaymentStatus)
        {
            _unitOfWork.PaymentStatus.Add(PaymentStatus);
            _unitOfWork.Save();
        }

        public bool DeletePaymentStatus(int id)
        {
            try
            {
                PaymentStatus? objFromDb = _unitOfWork.PaymentStatus.Get(u => u.PaymentStatusId == id);
                if (objFromDb is not null)
                {
                    _unitOfWork.PaymentStatus.Remove(objFromDb);
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

        public IEnumerable<PaymentStatus> GetAllPaymentStatus()
        {
            return _unitOfWork.PaymentStatus.GetAll();
        }

        public PaymentStatus GetPaymentStatusById(int id)
        {
            return _unitOfWork.PaymentStatus.Get(u => u.PaymentStatusId == id);            
        }

        public void UpdatePaymentStatus(PaymentStatus PaymentStatus)
        {
            _unitOfWork.PaymentStatus.Update(PaymentStatus);
            _unitOfWork.Save();
        }
    }
}
