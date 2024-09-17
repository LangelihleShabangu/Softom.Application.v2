using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softom.Application.Models.MV
{
    public class DriverDetails
    {
        public DriverDetails()
        {
            Driver = new Driver();
            Address = new Address();
            VehicleList = new List<Vehicle>();
            ContactInformation = new ContactInformation();  
        } 
        public Driver? Driver { get; set; }
        public Address? Address { get; set; }
        public List<Vehicle>? VehicleList { get; set; }        
        public ContactInformation? ContactInformation { get; set; }           
    }
}
