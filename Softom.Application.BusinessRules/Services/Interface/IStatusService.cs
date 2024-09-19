using Softom.Application.Models;
using Softom.Application.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softom.Application.BusinessRules.Services.Interface
{
    public interface IStatusService
    {
        IEnumerable<Status> GetAllStatus();
        void CreateStatus(Status Status);
        void UpdateStatus(Status Status);
        Status GetStatusById(int id);
        bool DeleteStatus(int id);
    }
}