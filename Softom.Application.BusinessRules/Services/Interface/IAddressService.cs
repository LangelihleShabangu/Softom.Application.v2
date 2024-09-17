using Softom.Application.Models;
using Softom.Application.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softom.Application.BusinessRules.Services.Interface
{
    public interface IAddressService
    {
        IEnumerable<Address> GetAllAddress();
        void CreateAddress(Address Address);
        void UpdateAddress(Address Address);
        Address GetAddressById(int id);
        bool DeleteAddress(int id);
    }
}