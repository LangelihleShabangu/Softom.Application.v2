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

namespace Softom.Application.UI.Controllers
{
    [Authorize]
    public class MemberController : Controller
    {
		private readonly IAddressService _AddressService;
		private readonly IMemberService _MemberService;
        private readonly IAssociationService _AssociationService;
        private readonly IContactInformationService _ContactInformationService;

        public MemberController(IMemberService MemberService, IAddressService AddressService, IContactInformationService ContactInformationService,
            IAssociationService AssociationService)
        {
			_AddressService = AddressService;
			_MemberService = MemberService;
            _AssociationService = AssociationService;
            _ContactInformationService = ContactInformationService;
        }

        public IActionResult Index()
        {
            var Members = _MemberService.GetAllMember();
            return View(Members);
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

        //[HttpPost]
        //public IActionResult Create(Member obj)
        //{
        //    if (string.IsNullOrEmpty(obj.MemberName))
        //    {
        //        ModelState.AddModelError("Name", "The name cannot be empty.");
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        _MemberService.CreateMember(obj);
        //        TempData["success"] = "The Member has been created successfully.";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View();
        //}

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
			var array = new Byte[64];
			Array.Clear(array, 0, array.Length);

			var filePath = Path.GetTempFileName();
			foreach (var formFile in Request.Form.Files)
			{
				if (formFile.Length > 0)
				{
					using (var inputStream = new FileStream(filePath, FileMode.Create))
					{						
						formFile.CopyToAsync(inputStream);						
						array = new byte[inputStream.Length];
						inputStream.Seek(0, SeekOrigin.Begin);
						inputStream.Read(array, 0, array.Length);
						MemberMV.Member.MemberImage = array;
					}
				}
			}
            MemberMV.ContactInformation.Notes = "None";
            _ContactInformationService.CreateContactInformation(MemberMV.ContactInformation);

            Member Member = new Member() {
                ContactInformationId = MemberMV.ContactInformation.ContactInformationId,
                AddressId = MemberMV.Address.AddressId,
                AssociationId = MemberMV.Association.AssociationId,                
                MemberImage = MemberMV.Member.MemberImage,
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
		
        public IActionResult Delete(int MemberId)
        {
            Member? obj = _MemberService.GetMemberById(MemberId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
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
                TempData["error"] = "Failed to delete the Member.";
            }
            return View();
        }
    }
}
