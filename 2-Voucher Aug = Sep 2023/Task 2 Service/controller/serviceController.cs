using Ekad.Voucher.domain.context;
using Ekad.Voucher.domain.Models;
using Ekad.Voucher.domain.Models.VM;
using Ekad.Voucher.domain.Resources;
using Ekad.Voucher.logic.Service.Interfaces;
using Ekad.Voucher.logic.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Text;
using Microsoft.EntityFrameworkCore;

namespace Ekad.Voucher.web.Controllers
{
    public class serviceController : BaseController
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        private readonly ITblServiceService _ServiceService;
        private readonly ISettingService _SettingService;
        public serviceController(IConfiguration config, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, ISettingService SettingService, ITblServiceService ServiceService) : base(config, env, userManager, roleManager, signInManager)
        {
            this._env = env;
            this._ServiceService = ServiceService;
            this._SettingService = SettingService;

        }

        // GET: Service
        public ActionResult Index()
        {
            ///var service = _ServiceService.GetAll().Result;
            ViewBag.settingtype = _SettingService.GetServiceType().ToList();
            return View();
        }


        // search
        public async Task<PartialViewResult> Search(string? ServiceName, int? ServiceType)
        {


            var model = await _ServiceService.GetService(new Servicefilter()
            {
                Servicename = ServiceName,
                ServiceType = ServiceType

            });


            return PartialView("_PartialServiceList", model);
        }


        //GET: Service / Create
        public ActionResult Create()
        {
            string ProjectLink = _config.GetValue<string>("AppSettings:ApiServer");
            ViewBag.ProjectLink = ProjectLink;
            ViewBag.settingtype = _SettingService.GetServiceType().ToList();
            return View(new TblService());
        }

        //  POST: service/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(TblService service)
        {
            try
            {
                if (service.ServiceId == 0)
                {
                    bool added = _ServiceService.Add(service).IsCompletedSuccessfully;
                    if (added == true)
                    {
                        return Json(new { success = true, message = Resource.SavedSuccessfully });
                    }
                }
                else
                {
                    var res = _ServiceService.Update(service);
                    if (res == "success")
                    {
                        return Json(new { success = true, message = Resource.SavedSuccessfully });
                    }

                }
                return Json(new { success = false, message = Resource.SaveFailed });
            }
            catch
            {
                return Json(new { success = false, message = "Error occurred while processing the request." });
            }
        }

        // GET: ProviderController/Edit/5
        public ActionResult Edit(int id)
        {
            string ProjectLink = _config.GetValue<string>("AppSettings:ApiServer");
            ViewBag.ProjectLink = ProjectLink;
            ViewBag.settingtype = _SettingService.GetServiceType().ToList();
            var model = _ServiceService.Get(id).Result;
            return View("Create", model);
        }

        // POST: ProviderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }




        // Get:  Service/Delete
        // POST: Service/Delete
        public JsonResult DeleteService(int id)
        {
            string res = _ServiceService.DeleteService(id);
            if (res == "success")
            {
                return Json(new { success = true, message = "Successfully Deleted" });
            }
            else
            {
                return Json(new { success = false, message = "Can't Delete this employee" });
            }
        }




    }




}


//    [HttpPost]
//    public async Task<IActionResult> Create([FromBody] TblService service)
//    {
//        try
//        {
//            await _ServiceService.Add(service);
//            return Ok();
//        }
//        catch (Exception ex)
//        {
//            // Handle any exceptions and return an appropriate error response
//            return StatusCode(500, $"An error occurred: {ex.Message}");
//        }
//    }
//}



//        POST: Sector/Create
//       [HttpPost]
//        public JsonResult create(SectorVM Vm)
//        {
//            try
//            {
//                if (Vm.sectorId == 0)
//                {

//                    TblSector sector = new TblSector
//                    {
//                        SectorNameAr = Vm.sectorNameAr,
//                        SectorNameEn = Vm.sectorNameEn,
//                        SectorNotesAr = Vm.sectorNotesAr,
//                        SectorNotesEn = Vm.sectorNotesEn,
//                        CreateDate = DateTime.Now,
//                        IsDeleted = false,
//                        CreatedBy = LoggedUser.Id
//                    };
//                    _SectorService.AddSect(sector);
//                    return Json(new { success = true, message = Resource.SavedSuccessfully });

//                }
//                else
//                {
//                    var Obj = _SectorService.GetSectorID(Vm.sectorId);
//                    if (Obj == null)
//                    {
//                        return Json(new { success = false, message = "no sector with this ID" });
//                    }

//                    Obj.SectorNameAr = Vm.sectorNameAr;
//                    Obj.SectorNameEn = Vm.sectorNameEn;
//                    Obj.SectorNotesAr = Vm.sectorNotesAr;
//                    Obj.SectorNotesEn = Vm.sectorNotesEn;


//                    string success = _SectorService.Updatesec(Obj);
//                    if (success == "success")
//                    {
//                        return Json(new { success = true, message = Resource.SavedSuccessfully });

//                    }
//                    return Json(new { success = false, message = "Error occurred while processing the request." });
//                }

//            }
//            catch
//            {
//                return Json(new { success = false, message = "Error occurred while processing the request." });
//            }
//        }
//        GET: Sector/Edit
//        public ActionResult Edit(int id)
//        {

//            var model = _SectorService.GetSectorID(id);
//            return View("Create", model);
//        }

//        POST: Sector/Edit
//       not used
//       [HttpPost]
//       [ValidateAntiForgeryToken]
//        public ActionResult Edit(int id, SectorVM Vm)
//        {
//            try
//            {
//                return RedirectToAction(nameof(Index));
//            }
//            catch
//            {
//                return View();
//            }
//        }

//        Get: Sector/Delete
//        POST: Sector/Delete
//        public JsonResult DeleteSector(int id)
//        {
//            string res = _SectorService.DeleteSector(id);
//            if (res == "success")
//            {
//                return Json(new { success = true, message = "Successfully Deleted" });
//            }
//            else
//            {
//                return Json(new { success = false, message = "Can't Delete this employee" });
//            }
//        }