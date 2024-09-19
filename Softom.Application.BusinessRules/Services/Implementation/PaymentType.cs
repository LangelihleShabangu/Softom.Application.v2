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
    public class PaymentTypeService : IPaymentTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PaymentTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
       
        public void CreatePaymentType(PaymentType PaymentType)
        {
            _unitOfWork.PaymentType.Add(PaymentType);
            _unitOfWork.Save();
        }

        public bool DeletePaymentType(int id)
        {
            try
            {
                PaymentType? objFromDb = _unitOfWork.PaymentType.Get(u => u.PaymentTypeId == id);
                if (objFromDb is not null)
                {
                    _unitOfWork.PaymentType.Remove(objFromDb);
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

        public IEnumerable<PaymentType> GetAllPaymentType()
        {
            return _unitOfWork.PaymentType.GetAll();
        }

        public PaymentType GetPaymentTypeById(int id)
        {
            return _unitOfWork.PaymentType.Get(u => u.PaymentTypeId == id);            
        }

        public void UpdatePaymentType(PaymentType PaymentType)
        {
            _unitOfWork.PaymentType.Update(PaymentType);
            _unitOfWork.Save();
        }
    }
}
