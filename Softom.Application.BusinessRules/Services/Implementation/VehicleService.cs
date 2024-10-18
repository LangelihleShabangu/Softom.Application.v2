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
    public class VehicleService : IVehicleService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VehicleService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
       
        public void CreateVehicle(Vehicle Vehicle)
        {
            _unitOfWork.Vehicle.Add(Vehicle);
            _unitOfWork.Save();
        }

        public bool DeleteVehicle(int id)
        {
            try
            {
                Vehicle? objFromDb = _unitOfWork.Vehicle.Get(u => u.VehicleId == id);
                if (objFromDb is not null)
                {
                    _unitOfWork.Vehicle.Remove(objFromDb);
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

        public IEnumerable<Vehicle> GetAllVehicle()
        {
            return _unitOfWork.Vehicle.GetAll(includeProperties: "Status");
        }

        public Vehicle GetVehicleById(int id)
        {
            return _unitOfWork.Vehicle.Get(u => u.VehicleId == id);            
        }

        public void UpdateVehicle(Vehicle Vehicle)
        {
            _unitOfWork.Vehicle.Update(Vehicle);
            _unitOfWork.Save();
        }
    }
}
