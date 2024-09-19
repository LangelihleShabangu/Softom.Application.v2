using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Softom.Application.BusinessRules.Services.Implementation;
using Softom.Application.BusinessRules.Services.Interface;
using Softom.Application.Models;
using Softom.Application.Models.Entities;
using Softom.Application.Models.MV;
using Softom.Application.UI.ViewModels;
using System.Globalization;

namespace Softom.Application.UI.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private readonly IPaymentServices _PaymentService;
        private readonly IMemberService _MemberService;
        private readonly IPaymentTypeService _PaymentTypeService;
        public PaymentController(IPaymentServices PaymentService, IMemberService memberService, IPaymentTypeService PaymentTypeService)
        {
            _PaymentService = PaymentService;
            _MemberService = memberService;
            _PaymentTypeService = PaymentTypeService;
        }

        public IActionResult Index()
        {
            PaymentDetails paymentDetailsVM = new()
            {
                MemberList = _MemberService.GetAllMember().Select(u => new SelectListItem
                {
                    Text = u.ContactInformation.Firstname + " " + u.ContactInformation.Surname,
                    Value = u.MemberId.ToString()
                }),
                PaymentTypeList = _PaymentTypeService.GetAllPaymentType().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.PaymentTypeId.ToString()
                }),
                PaymentList = _PaymentService.GetAllPayment().ToList()
            };

            foreach(var item in paymentDetailsVM.PaymentList)
            {
                item.Member.ContactInformation = _MemberService.GetMemberById(item.MemberId).ContactInformation;
            }

            DateTime startAtMonday = DateTime.Now.AddDays(DayOfWeek.Monday - DateTime.Now.DayOfWeek);
            DateTime startAtSunday = startAtMonday.AddDays(6);

            paymentDetailsVM.PaymentList = paymentDetailsVM.PaymentList.Where(f=>f.Createddate >= startAtMonday && f.Createddate <= startAtSunday).ToList();

            return View(paymentDetailsVM);
        }

        public IActionResult Create()
        {
            return View();
        }
       
        [HttpPost]
        public IActionResult CreatePayment(int Member, int PaymentType, decimal Amount)
        {
            var payment = new Payment();
            payment.PaymentStatusId = 1; /* Active */
            payment.MemberId = Member;
            payment.PaymentTypeId = PaymentType;
            payment.Amount = Amount;
            payment.Notes = "Payment";
            payment.Createddate = DateTime.Now; 
            payment.PaymentDate = DateTime.Now; 
            payment.Modifieddate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _PaymentService.CreatePayment(payment);
                TempData["success"] = "The Payment has been created successfully.";
                return RedirectToAction(nameof(Index));
            }

            return Json(new { success = true });
        }

        public IActionResult Update(int PaymentId)
        {
            Payment? obj = _PaymentService.GetPaymentById(PaymentId);
            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Update(Payment obj)
        {
            if (ModelState.IsValid && obj.PaymentId > 0)
            {

               _PaymentService.UpdatePayment(obj);
                TempData["success"] = "The Payment has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Delete(int PaymentId)
        {
            Payment? obj = _PaymentService.GetPaymentById(PaymentId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }


        [HttpPost]
        public IActionResult Delete(Payment obj)
        {
         bool deleted = _PaymentService.DeletePayment(obj.PaymentId);
            if (deleted)
            {
                TempData["success"] = "The Payment has been deleted successfully.";
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
