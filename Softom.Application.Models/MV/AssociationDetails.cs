using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softom.Application.Models.MV
{
    public class AssociationDetails
    {
        public AssociationDetails()
        {
            Address = new Address();
            Association = new Association();    
        }
        public int AssociationId { get; set; }
        public Member? Member { get; set; }
        public Address? Address { get; set; }
        public List<Member>? Members { get; set; }
        public Association? Association { get; set; }   
    }
}
