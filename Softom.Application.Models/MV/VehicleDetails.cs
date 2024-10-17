using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softom.Application.Models.MV
{
    public class VehicleDetails
    {
        public VehicleDetails()
        {
            Vehicle = new Vehicle();
        } 
        public Vehicle? Vehicle { get; set; }

        [BindProperty(Name = "StatusId")]
        public int StatusId { get; set; }
        public Status? Status { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? StatusList { get; set; }
    }
}
