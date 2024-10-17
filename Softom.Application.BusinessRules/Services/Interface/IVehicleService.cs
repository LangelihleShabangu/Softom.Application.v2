using Softom.Application.Models;
using Softom.Application.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softom.Application.BusinessRules.Services.Interface
{
    public interface IVehicleService
    {
        IEnumerable<Vehicle> GetAllVehicle();
        void CreateVehicle(Vehicle Vehicle);
        void UpdateVehicle(Vehicle Vehicle);
        Vehicle GetVehicleById(int id);
        bool DeleteVehicle(int id);
    }
}