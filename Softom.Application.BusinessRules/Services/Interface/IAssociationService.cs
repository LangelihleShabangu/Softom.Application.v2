using Softom.Application.Models;
using Softom.Application.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softom.Application.BusinessRules.Services.Interface
{
    public interface IAssociationService
    {
        IEnumerable<Association> GetAllAssociation();
        void CreateAssociation(Association Association);
        void UpdateAssociation(Association Association);
        Association GetAssociationById(int id);
        bool DeleteAssociation(int id);
    }
}