using Softom.Application.Models;
using Softom.Application.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softom.Application.BusinessRules.Services.Interface
{
    public interface IMemberService
    {
        IEnumerable<Member> GetAllMember();
        void CreateMember(Member Member);
        void UpdateMember(Member Member);
        Member GetMemberById(int id);
        bool DeleteMember(int id);
    }
}