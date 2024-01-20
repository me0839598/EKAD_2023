using Ekad.Voucher.domain.context;
using Ekad.Voucher.domain.Models.VM;
using Ekad.Voucher.domain.Resources;
using Ekad.Voucher.logic.Service.Interfaces;
using Ekad.Voucher.logic.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Ekad.Voucher.web.Controllers
{
    public class sectorController : BaseController
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
        private readonly ICompanyService _SectorService;
        public sectorController(IConfiguration config, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager,ICompanyService companyService, ISettingService SettingService) : base(config, env, userManager, roleManager, signInManager)
        {
            this._env = env;
            this._SectorService = companyService;
           
        }

        // GET: Sector
        public ActionResult Index()
        {
            var sector = _SectorService.GetsectorList(new domain.Models.GeneralFilter { }).ToList();
            return View(sector);
        }
        // search
        public PartialViewResult Searchsector(string? sectorName)
        {
            var model = _SectorService.Getsector(new domain.Models.sectorfilter()
            {
                sectorname = sectorName
            }).ToList();

            return PartialView("_PartialSectorList",model);
        }

        // GET: Sector/Create
        public ActionResult Create()
        {
           
            return View(new TblSector());
        }
        // POST: Sector/Create
        [HttpPost]
        public JsonResult create (SectorVM Vm)
        {
            try
            {
                if (Vm.sectorId == 0)
                {

                    TblSector sector = new TblSector
                    {
                        SectorNameAr = Vm.sectorNameAr,
                        SectorNameEn = Vm.sectorNameEn,
                        SectorNotesAr = Vm.sectorNotesAr,
                        SectorNotesEn = Vm.sectorNotesEn,
                        CreateDate = DateTime.Now,
                        IsDeleted = false,
                        CreatedBy = LoggedUser.Id
                    };
                    bool ADDED = _SectorService.AddSect(sector).IsCompletedSuccessfully;
                    if (ADDED == true) {

                        return Json(new { success = true, message = Resource.SavedSuccessfully });
                    }
                   

                }
                else
                {
                    var Obj = _SectorService.GetSectorID(Vm.sectorId);
                    if (Obj == null)
                    {
                        return Json(new { success = false, message = "no sector with this ID" });
                    }

                    Obj.SectorNameAr = Vm.sectorNameAr;
                    Obj.SectorNameEn = Vm.sectorNameEn;
                    Obj.SectorNotesAr = Vm.sectorNotesAr;
                    Obj.SectorNotesEn = Vm.sectorNotesEn;
                    
                    
                    string success = _SectorService.Updatesec(Obj);
                    if (success == "success")
                    {
                        return Json(new { success = true, message = Resource.SavedSuccessfully });

                    }
                    
                }
                return Json(new { success = false, message = "Error occurred while processing the request." });

            }
            catch
            {
                return Json(new { success = false, message = "Error occurred while processing the request." });
            }
        }


        // GET: Sector/Edit
        public ActionResult Edit(int id)
        {
            
            var model = _SectorService.GetSectorID(id);
            return View("Create", model);
        }

        // POST: Sector/Edit
        //not used
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, SectorVM Vm)
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



        // Get: Sector/Delete
        // POST: Sector/Delete
        public JsonResult DeleteSector(int id)
        {
            string res = _SectorService.DeleteSector(id);
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
