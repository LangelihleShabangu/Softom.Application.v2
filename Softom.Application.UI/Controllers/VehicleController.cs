using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class VehicleController : Controller
    {
        private readonly IStatusService _StatusService;
        private readonly IVehicleService _VehicleService;

        public VehicleController(IVehicleService VehicleService, IStatusService StatusService)
        {
            _VehicleService = VehicleService;
            _StatusService = StatusService;
        }

        public IActionResult Index()
        {
            var Vehicles = _VehicleService.GetAllVehicle();
            return View(Vehicles);
        }

        //public IActionResult Create()
        //{
        //    return View("Upsert", new Application.Models.MV.VehicleDetails());
        //}

        //public IActionResult Details(int VehicleId)
        //{
        //    return View("Detail", new Application.Models.MV.VehicleDetails());
        //}

        [HttpGet]
        public IActionResult Upsert(int VehicleId)
        {            
            Vehicle? obj = _VehicleService.GetVehicleById(VehicleId);
            VehicleDetails VehicleDetails = new()
            {
                StatusList = _StatusService.GetAllStatus().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.StatusId.ToString()
                }),
                Vehicle = obj,
            };
            return View(VehicleDetails);
        }

        //[HttpPost]
        //public IActionResult Update(Vehicle obj)
        //{
        //    if (ModelState.IsValid && obj.VehicleId > 0)
        //    {

        //        _VehicleService.UpdateVehicle(obj);
        //        TempData["success"] = "The Vehicle has been updated successfully.";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View();
        //}

        //[HttpPost]
        //public async Task<IActionResult> Create(Softom.Application.Models.MV.VehicleDetails VehicleMV, List<IFormFile> files)
        //{
        //    var array = new Byte[64];
        //    Array.Clear(array, 0, array.Length);

        //    var filePath = Path.GetTempFileName();
        //    foreach (var formFile in Request.Form.Files)
        //    {
        //        if (formFile.Length > 0)
        //        {
        //            using (var inputStream = new FileStream(filePath, FileMode.Create))
        //            {
        //                formFile.CopyToAsync(inputStream);
        //                array = new byte[inputStream.Length];
        //                inputStream.Seek(0, SeekOrigin.Begin);
        //                inputStream.Read(array, 0, array.Length);
        //                VehicleMV.Vehicle.Logo = array;
        //            }
        //        }
        //    }

        //    Vehicle Vehicle = new Vehicle()
        //    {
        //        StatusId = VehicleMV.Status.StatusId,
        //        VehicleName = VehicleMV.Vehicle.VehicleName,
        //        CellNumber = VehicleMV.Vehicle.CellNumber,
        //        EmailStatus = VehicleMV.Vehicle.EmailStatus,
        //        Logo = VehicleMV.Vehicle.Logo,
        //        Createddate = System.DateTime.Now,
        //        PhoneNumber = VehicleMV.Vehicle.PhoneNumber,
        //        Modifieddate = System.DateTime.Now,
        //        Isdeleted = false,
        //        Website = VehicleMV.Vehicle.Website,
        //        Notes = VehicleMV.Vehicle.Notes,
        //        VehicleId = VehicleMV.Vehicle.VehicleId
        //    };

        //    VehicleMV.Vehicle.Logo = array;

        //    if (VehicleMV.Vehicle.VehicleId == 0)
        //    {
        //        _StatusService.CreateStatus(VehicleMV.Status);
        //        Vehicle.StatusId = VehicleMV.Status.StatusId;
        //        _VehicleService.CreateVehicle(Vehicle);
        //        TempData["success"] = "Vehicle created successfully";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    else
        //    {
        //        _StatusService.UpdateStatus(VehicleMV.Status);
        //        _VehicleService.UpdateVehicle(Vehicle);
        //        TempData["success"] = "Vehicle updated successfully";
        //        return RedirectToAction(nameof(Index));
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> CreateVehicle(Vehicle vehicle)
        {
            vehicle.Notes = "None";
            vehicle.StatusId = 1; //Active
            vehicle.CreatedBy = new Guid();
            vehicle.ModifiedBy = new Guid();
            vehicle.Createddate = DateTime.Now;
            vehicle.Modifieddate = DateTime.Now;

            try
            {
                _VehicleService.CreateVehicle(vehicle);
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
        public async Task<IActionResult> Upsert(Vehicle vehicle)
        {
            vehicle.Notes = "None";
            vehicle.StatusId = 1; //Active
            vehicle.CreatedBy = new Guid();
            vehicle.ModifiedBy = new Guid();
            vehicle.Createddate = DateTime.Now;
            vehicle.Modifieddate = DateTime.Now;

            try
            {
                _VehicleService.UpdateVehicle(vehicle);
                TempData["success"] = "Vehicle created successfully";
                return RedirectToAction(nameof(Index), "Member");
            }
            catch
            {
                TempData["success"] = "Vehicle updated successfully";
            }
            return RedirectToAction(nameof(Index), "Member");
        }

        public IActionResult Delete(int VehicleId)
        {
            Vehicle? obj = _VehicleService.GetVehicleById(VehicleId);
            if (obj is null)
            {
                return RedirectToAction("Error", "Home");
            }
            else
            {
                TempData["success"] = "The Vehicle has been deleted successfully.";
                return RedirectToAction("GetVehicleDetailsById", "Member", new { MemberId = obj.MemberId });
            }
        }

        [HttpPost]
        public IActionResult Delete(Vehicle obj)
        {
            bool deleted = _VehicleService.DeleteVehicle(obj.VehicleId);
            if (deleted)
            {
                TempData["success"] = "The Vehicle has been deleted successfully.";
                return RedirectToAction("GetVehicleDetailsById", "Member", new { MemberId = obj.MemberId });
            }
            else
            {
                TempData["error"] = "Failed to delete the Vehicle.";
            }
            return View();
        }
    }
}
