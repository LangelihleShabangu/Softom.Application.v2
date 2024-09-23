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
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
       
        public void CreateMember(Member Member)
        {
            _unitOfWork.Member.Add(Member);
            _unitOfWork.Save();
        }

        public bool DeleteMember(int id)
        {
            try
            {
                Member? objFromDb = _unitOfWork.Member.Get(u => u.MemberId == id);
                if (objFromDb is not null)
                {
                    _unitOfWork.Member.Remove(objFromDb);
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

        public IEnumerable<Member> GetAllMember()
        {
            return _unitOfWork.Member.GetAll(includeProperties: "Address,ContactInformation,Association");
        }

        public Member GetMemberById(int id)
        {
            return _unitOfWork.Member.Get(u => u.MemberId == id, includeProperties: "Address,ContactInformation,Association");            
        }

        public void UpdateMember(Member Member)
        {
            _unitOfWork.Member.Update(Member);
            _unitOfWork.Save();
        }
    }
}
