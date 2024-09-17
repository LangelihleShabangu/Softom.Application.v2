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
    public class AssociationService : IAssociationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AssociationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
       
        public void CreateAssociation(Association Association)
        {
            _unitOfWork.Association.Add(Association);
            _unitOfWork.Save();
        }

        public bool DeleteAssociation(int id)
        {
            try
            {
                Association? objFromDb = _unitOfWork.Association.Get(u => u.AssociationId == id);
                if (objFromDb is not null)
                {
                    _unitOfWork.Association.Remove(objFromDb);
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

        public IEnumerable<Association> GetAllAssociation()
        {
            return _unitOfWork.Association.GetAll(includeProperties: "Address");
        }

        public Association GetAssociationById(int id)
        {
            return _unitOfWork.Association.Get(u => u.AssociationId == id, includeProperties: "Address");            
        }

        public void UpdateAssociation(Association Association)
        {
            _unitOfWork.Association.Update(Association);
            _unitOfWork.Save();
        }
    }
}
