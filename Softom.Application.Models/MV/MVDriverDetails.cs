using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softom.Application.Models.MV
{
    public class MVDriverDetails
	{
        public MVDriverDetails()
        {
			MVDriverDetail = new MVDriverDetails();             
        }
        public int DriverId { get; set; }
		public int Firstname { get; set; }
		public int Surname { get; set; }
		public int PhoneNumber { get; set; }
		public int Address { get; set; }
		public MVDriverDetails MVDriverDetail { get; set; }
	}
}
