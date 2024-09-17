using Softom.Application.Models;
using Softom.Application.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softom.Application.BusinessRules.Services.Interface
{
    public interface IContactInformationService
    {
        IEnumerable<ContactInformation> GetAllContactInformation();
        void CreateContactInformation(ContactInformation ContactInformation);
        void UpdateContactInformation(ContactInformation ContactInformation);
        ContactInformation GetContactInformationById(int id);
        bool DeleteContactInformation(int id);
    }
}