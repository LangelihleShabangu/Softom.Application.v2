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
    public class StatusService : IStatusService
    {
        private readonly IUnitOfWork _unitOfWork;

        public StatusService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
       
        public void CreateStatus(Status Status)
        {
            _unitOfWork.Status.Add(Status);
            _unitOfWork.Save();
        }

        public bool DeleteStatus(int id)
        {
            try
            {
                Status? objFromDb = _unitOfWork.Status.Get(u => u.StatusId == id);
                if (objFromDb is not null)
                {
                    _unitOfWork.Status.Remove(objFromDb);
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

        public IEnumerable<Status> GetAllStatus()
        {
            return _unitOfWork.Status.GetAll();
        }

        public Status GetStatusById(int id)
        {
            return _unitOfWork.Status.Get(u => u.StatusId == id);            
        }

        public void UpdateStatus(Status Status)
        {
            _unitOfWork.Status.Update(Status);
            _unitOfWork.Save();
        }
    }
}
