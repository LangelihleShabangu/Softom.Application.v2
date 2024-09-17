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
    public class ContactInformationService : IContactInformationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContactInformationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
       
        public void CreateContactInformation(ContactInformation ContactInformation)
        {
            _unitOfWork.ContactInformation.Add(ContactInformation);
            _unitOfWork.Save();
        }

        public bool DeleteContactInformation(int id)
        {
            try
            {
                ContactInformation? objFromDb = _unitOfWork.ContactInformation.Get(u => u.ContactInformationId == id);
                if (objFromDb is not null)
                {
                    _unitOfWork.ContactInformation.Remove(objFromDb);
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

        public IEnumerable<ContactInformation> GetAllContactInformation()
        {
            return _unitOfWork.ContactInformation.GetAll(includeProperties: "Address");
        }

        public ContactInformation GetContactInformationById(int id)
        {
            return _unitOfWork.ContactInformation.Get(u => u.ContactInformationId == id, includeProperties: "Address");            
        }

        public void UpdateContactInformation(ContactInformation ContactInformation)
        {
            _unitOfWork.ContactInformation.Update(ContactInformation);
            _unitOfWork.Save();
        }
    }
}
