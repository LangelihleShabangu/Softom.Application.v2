using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Softom.Application.BusinessRules.Services.Implementation;
using Softom.Application.BusinessRules.Services.Interface;
using Softom.Application.Models;
using Softom.Application.Models.Entities;
using Softom.Application.Models.MV;
using System.ComponentModel.DataAnnotations.Schema;

namespace Softom.Application.UI.Controllers
{
    [Authorize]
    public class AssociationController : Controller
    {
        private readonly IAddressService _AddressService;
        private readonly IAssociationService _AssociationService;
        public AssociationController(IAssociationService AssociationService, IAddressService AddressService)
        {
            _AddressService = AddressService;
            _AssociationService = AssociationService;
        }

        public IActionResult Index()
        {
            var Associations = _AssociationService.GetAllAssociation();
            return View(Associations);
        }

        public IActionResult Create()
        {
            return View("Upsert", new Application.Models.MV.AssociationDetails());
        }

        public IActionResult Details(int AssociationId)
        {
            return View("Detail", new Application.Models.MV.AssociationDetails());
        }


        [HttpGet]
        public IActionResult Upsert(int AssociationId)
        {
            AssociationDetails associationDetails = new AssociationDetails();
            Association? obj = _AssociationService.GetAssociationById(AssociationId);

            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            else
            {
                associationDetails.Association = obj;
                associationDetails.Address = obj.Address;
            }
            return View(associationDetails);
        }

        [HttpPost]
        public IActionResult Update(Association obj)
        {
            if (ModelState.IsValid && obj.AssociationId > 0)
            {

                _AssociationService.UpdateAssociation(obj);
                TempData["success"] = "The Association has been updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Softom.Application.Models.MV.AssociationDetails associationMV, List<IFormFile> files)
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
                        associationMV.Association.Logo = array;
                    }
                }
            }

            Association association = new Association()
            {
                AddressId = associationMV.Address.AddressId,
                AssociationName = associationMV.Association.AssociationName,
                CellNumber = associationMV.Association.CellNumber,
                EmailAddress = associationMV.Association.EmailAddress,
                Logo = associationMV.Association.Logo,
                Createddate = System.DateTime.Now,
                PhoneNumber = associationMV.Association.PhoneNumber,
                Modifieddate = System.DateTime.Now,
                Isdeleted = false,
                Website = associationMV.Association.Website,
                Notes = associationMV.Association.Notes,
                AssociationId = associationMV.Association.AssociationId
            };

            associationMV.Association.Logo = array;

            if (associationMV.Association.AssociationId == 0)
            {
                _AddressService.CreateAddress(associationMV.Address);
                association.AddressId = associationMV.Address.AddressId;
                _AssociationService.CreateAssociation(association);
                TempData["success"] = "Association created successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _AddressService.UpdateAddress(associationMV.Address);
                _AssociationService.UpdateAssociation(association);
                TempData["success"] = "Association updated successfully";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(Softom.Application.Models.MV.AssociationDetails associationMV, List<IFormFile> files)
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
                        associationMV.Association.Logo = array;
                    }
                }
            }

            Association association = new Association()
            {
                AddressId = associationMV.Address.AddressId,
                AssociationName = associationMV.Association.AssociationName,
                CellNumber = associationMV.Association.CellNumber,
                EmailAddress = associationMV.Association.EmailAddress,
                Logo = associationMV.Association.Logo,
                Createddate = System.DateTime.Now,
                PhoneNumber = associationMV.Association.PhoneNumber,
                Modifieddate = System.DateTime.Now,
                Isdeleted = false,
                Website = associationMV.Association.Website,
                Notes = associationMV.Association.Notes,
                AssociationId = associationMV.Association.AssociationId
            };

            _AssociationService.UpdateAssociation(association);
            associationMV.Association.Logo = array;
            try
            {
                _AddressService.UpdateAddress(associationMV.Address);
            }
            catch
            {  
                TempData["success"] = "Association updated successfully";
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int AssociationId)
        {
            Association? obj = _AssociationService.GetAssociationById(AssociationId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Association obj)
        {
            bool deleted = _AssociationService.DeleteAssociation(obj.AssociationId);
            if (deleted)
            {
                TempData["success"] = "The Association has been deleted successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = "Failed to delete the Association.";
            }
            return View();
        }
    }
}
