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
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AddressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void CreateAddress(Address Address)
        {
            ArgumentNullException.ThrowIfNull(Address);

            _unitOfWork.Address.Add(Address);
            _unitOfWork.Save();
        }

        public bool DeleteAddress(int id)
        {
            try
            {
                var Address = _unitOfWork.Address.Get(u => u.AddressId == id);
                if (Address != null)
                {
                    _unitOfWork.Address.Remove(Address);
                    _unitOfWork.Save();
                    return true;
                }
                else
                {
                    throw new InvalidOperationException($"Address with ID {id} not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public IEnumerable<Address> GetAllAddress()
        {
            return _unitOfWork.Address.GetAll();
        }

        public Address GetAddressById(int id)
        {
            return _unitOfWork.Address.Get(u => u.AddressId == id);
        }

        public void UpdateAddress(Address Address)
        {
            ArgumentNullException.ThrowIfNull(Address);

            _unitOfWork.Address.Update(Address);
            _unitOfWork.Save();
        }
	}
}
