using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Softom.Application.BusinessRules.Common.Interfaces;
using Softom.Application.BusinessRules.Services.Implementation;
using Softom.Application.BusinessRules.Services.Interface;
using Softom.Application.Models;
using Softom.Application.Models.Entities;
using Softom.Application.Models.MV;
using Softom.Application.UI.ViewModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;

namespace Softom.Application.UI.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
        private readonly IAddressService _AddressService;
        private readonly IMemberService _MemberService;
        private readonly IAssociationService _AssociationService;
        private readonly IContactInformationService _ContactInformationService;

        private readonly IPaymentServices _PaymentService;
        private readonly IVehicleService _VehicleService;
        private readonly IPaymentTypeService _PaymentTypeService;
        private readonly UserManager<ApplicationUser> _userManager;
        public MemberController(
            IMemberService MemberService, 
            IAddressService AddressService, 
            IContactInformationService ContactInformationService,
            IAssociationService AssociationService, 
            IPaymentTypeService PaymentTypeService, 
            UserManager<ApplicationUser> userManager, 
            IPaymentServices PaymentService,
            IVehicleService VehicleService)
        {
            _AddressService = AddressService;
            _MemberService = MemberService;
            _AssociationService = AssociationService;
            _ContactInformationService = ContactInformationService;
            _PaymentTypeService = PaymentTypeService;
            _userManager = userManager;
            _PaymentService = PaymentService;
            _VehicleService = VehicleService;
        }

        public IActionResult Index()
        {
            MemberVM memberVM = new MemberVM();
            memberVM.MemberList = _MemberService.GetAllMember();

            PaymentDetails paymentDetailsVM = new()
            {
                PaymentTypeList = _PaymentTypeService.GetAllPaymentType().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.PaymentTypeId.ToString()
                })
            };
            memberVM.paymentDetailsVM = paymentDetailsVM;
            return View(memberVM);
        }

        public IActionResult MemberDetails()
        {
			return View();
		}

	    [HttpGet]
        public IActionResult MemberPaymentList(int MemberId)
        {
            var memberVM = new PaymentDetails();
            memberVM.PaymentList =  _PaymentService.GetAllPayment().Where(f=>f.MemberId == MemberId).ToList();
            memberVM.Member = _MemberService.GetMemberById(MemberId);
            PaymentDetails paymentDetailsVM = new()
            {
                PaymentTypeList = _PaymentTypeService.GetAllPaymentType().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.PaymentTypeId.ToString()
                })
            };
            memberVM.paymentDetailsVM = paymentDetailsVM;
            return View("MemberPaymentList", memberVM);
        }
        
        [HttpPost]
        public IActionResult CreatePayments(Softom.Application.UI.ViewModels.MemberVM memberVM)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ApplicationUser user = _userManager.FindByIdAsync(userId).GetAwaiter().GetResult();

            var payment = new Payment();
            payment.PaymentStatusId = 1; /* Active */
            payment.MemberId = memberVM.MemberId;
            payment.PaymentTypeId = memberVM.PaymentTypeId;
            payment.Amount = memberVM.Payment.Amount;
            payment.Notes = userId.ToString();
            payment.Createddate = DateTime.Now;
            payment.PaymentDate = DateTime.Now;
            payment.Modifieddate = DateTime.Now;

            //if (ModelState.IsValid)
            //{
                _PaymentService.CreatePayment(payment);
                TempData["success"] = "The Payment has been created successfully.";
                var invoiceVM = new InvoiceVM();
                invoiceVM.Payment = _PaymentService.GetPaymentById(payment.PaymentId);
                invoiceVM.Member = _MemberService.GetMemberById(invoiceVM.Payment.MemberId);

                invoiceVM.Association = _AssociationService.GetAssociationById(invoiceVM.Member.AssociationId.Value);
                var byteInfoStatement = new Softom.Application.BusinessRules.Generate_PDF.CreateInvoicePDF().GeneratePDFFile(invoiceVM).ToArray();
                File(byteInfoStatement, "APPLICATION/pdf", "Payment_" + System.DateTime.Now.ToString("dd MMMM yyyy") + "_" + invoiceVM.Member.ContactInformation.Firstname + "_" + invoiceVM.Member.ContactInformation.Surname + ".pdf");
                return RedirectToAction( "MemberPaymentDetails","Payment", new { PaymentId = payment.PaymentId });
            //}
            //return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(Member obj)
        {
            bool deleted = _MemberService.DeleteMember(obj.MemberId);
            if (deleted)
            {
                TempData["success"] = "The Member has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Failed to delete the Payment.";
            }
            return View();
        }

        public IActionResult Create()
        {
            var memberdetails = new Application.Models.MV.MemberDetails();
            memberdetails.Member = new Member();
            memberdetails.ContactInformation = new ContactInformation();
            memberdetails.Address = new Address();
            memberdetails.Association = new Association();
            memberdetails.AssociationList = _AssociationService.GetAllAssociation().Select(x => new SelectListItem
            {
                Text = x.AssociationName,
                Value = x.AssociationId.ToString()
            });
            return View("Upsert", memberdetails);
        }

        public IActionResult Details(int MemberId)
        {
            return View("Detail", new Application.Models.MV.MemberDetails());
        }

        [HttpGet]
        public IActionResult GetMemberDetailsById(int MemberId)
        {
            MemberDetails MemberDetails = new MemberDetails();
            Member obj = _MemberService.GetMemberById(MemberId);
            Association association = _AssociationService.GetAssociationById(obj.AssociationId.Value);
            MemberDetails.AssociationList = _AssociationService.GetAllAssociation().Select(x => new SelectListItem
            {
                Text = x.AssociationName,
                Value = x.AssociationId.ToString()
            });
            MemberDetails.Member = obj;
            MemberDetails.Association = association;                
            MemberDetails.ContactInformation = obj.ContactInformation;  
            MemberDetails.Address = obj.Address;           
            return View("MemberDetails", MemberDetails);
        }

        [HttpGet]
        public IActionResult Upsert(int MemberId)
        {
            MemberDetails MemberDetails = new MemberDetails();
            Member obj = _MemberService.GetMemberById(MemberId);
            MemberDetails.AssociationList = _AssociationService.GetAllAssociation().Select(x => new SelectListItem
            {
                Text = x.AssociationName,
                Value = x.AssociationId.ToString()
            });

            MemberDetails.Member = obj;
            MemberDetails.ContactInformation = obj.ContactInformation;
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            else
            {
                MemberDetails.Member = obj;
                MemberDetails.Address = obj.Address;
            }
            return View(MemberDetails);
        }

        [HttpPost]
        public IActionResult Update(Member obj)
        {
            if (ModelState.IsValid && obj.MemberId > 0)
            {
                _MemberService.UpdateMember(obj);
                TempData["success"] = "The Member has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Softom.Application.Models.MV.MemberDetails MemberMV, List<IFormFile> files)
        {
            MemberMV.Member.AssociationId = _AssociationService.GetAllAssociation().FirstOrDefault().AssociationId;

            //if (MemberMV.Member == null || MemberMV.Member.AssociationId == null || MemberMV.Member.AssociationId == 0)
            //{
            //    ModelState.AddModelError("Associations", "Please select the Association.");
            //    var memberdetails = new Application.Models.MV.MemberDetails();
            //    memberdetails.Member = new Member();
            //    memberdetails.ContactInformation = new ContactInformation();
            //    memberdetails.Address = new Address();
            //    memberdetails.Association = new Association();
            //    memberdetails.AssociationList = _AssociationService.GetAllAssociation().Select(x => new SelectListItem
            //    {
            //        Text = x.AssociationName,
            //        Value = x.AssociationId.ToString()
            //    });
            //    return View("Upsert", memberdetails);
            //}

            MemberMV.ContactInformation.Notes = "None";
            _ContactInformationService.CreateContactInformation(MemberMV.ContactInformation);
            Member Member = new Member()
            {
                ContactInformationId = MemberMV.ContactInformation.ContactInformationId,
                AddressId = MemberMV.Address.AddressId,
                AssociationId = MemberMV.Member.AssociationId,
                Createddate = System.DateTime.Now,
                Modifieddate = System.DateTime.Now,
                Isdeleted = false,
                Notes = "None",
                MemberId = MemberMV.Member.MemberId
            };

            if (MemberMV.Member.MemberId == 0)
            {
                _AddressService.CreateAddress(MemberMV.Address);
                Member.AddressId = MemberMV.Address.AddressId;
                _MemberService.CreateMember(Member);
                TempData["success"] = "Member created successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _AddressService.UpdateAddress(MemberMV.Address);
                _MemberService.UpdateMember(Member);
                TempData["success"] = "Member updated successfully";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle(Softom.Application.UI.ViewModels.MemberVM memberVM)
        {
            memberVM.Vehicle.Notes = "None";
            memberVM.Vehicle.StatusId = 1; //Active
            memberVM.Vehicle.CreatedBy = new Guid();
            memberVM.Vehicle.ModifiedBy = new Guid();
            memberVM.Vehicle.Createddate = DateTime.Now;
            memberVM.Vehicle.Modifieddate = DateTime.Now;
            memberVM.Vehicle.MemberId = memberVM.MemberId;

            try
            {
                _VehicleService.CreateVehicle(memberVM.Vehicle);
                TempData["success"] = "Vehicle created successfully";
                return RedirectToAction(nameof(Index), "Member");
            }
            catch
            {
                TempData["success"] = "Vehicle updated successfully";
            }
            return RedirectToAction(nameof(Index), "Member");
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(Softom.Application.Models.MV.MemberDetails MemberMV, List<IFormFile> files)
        {
            _ContactInformationService.UpdateContactInformation(MemberMV.ContactInformation);
            try
            {
                _AddressService.UpdateAddress(MemberMV.Address);
            }
            catch
            { 
                TempData["success"] = "Member updated successfully";
            }

            TempData["success"] = "Member updated successfully";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int MemberId)
        {
            bool deleted = _MemberService.DeleteMember(MemberId);
            if (deleted)
            {
                TempData["success"] = "The Member has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Failed to delete the Payment.";
            }
            return View();            
        }        
    }
}
