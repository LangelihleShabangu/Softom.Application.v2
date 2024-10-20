﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
            Payment = new Payment();
            Vehicle = new Vehicle();
            Association = new Association();
            PaymentList = new List<Payment>();
            VehicleList = new List<Vehicle>();
        }
        public int MemberId { get; set; }
        public int PaymentTypeId { get; set; }        
        public Member? Member { get; set; }
        public ContactInformation? ContactInformation { get; set; }
        public Address? Address { get; set; }
        public List<Member>? Members { get; set; }
        public Vehicle? Vehicle { get; set; }
        public Payment? Payment { get; set; }
        public List<Payment> PaymentList { get; set; }
        public List<Vehicle> VehicleList { get; set; }
        public Association? Association { get; set; }
        public string? Associations { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem>? AssociationList { get; set; }

        public PaymentDetails? paymentDetailsVM { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? MemberList { get; set; }
        public PaymentType? PaymentType { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem>? PaymentTypeList { get; set; }
    }
}
