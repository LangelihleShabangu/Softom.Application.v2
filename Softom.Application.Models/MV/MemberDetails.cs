using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softom.Application.Models.MV
{
    public class MemberDetails
    {
        public MemberDetails()
        {
            Member = new Member();
            Address = new Address();            
            Association = new Association();    
        }
        public int MemberId { get; set; }
        public Member? Member { get; set; }
        public ContactInformation? ContactInformation { get; set; }
        public Address? Address { get; set; }
        public List<Member>? Members { get; set; }
        public Association? Association { get; set; }
        public string? Associations { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? AssociationList { get; set; }
    }
}
