using Ekad.Voucher.domain.context;
using Ekad.Voucher.domain.Resources;
using Ekad.Voucher.logic.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ekad.Voucher.web.Controllers
{
    public class ProviderController : BaseController
    {
        private readonly ICompanyService _CompanyService;
        private readonly ISettingService _SettingService;
        private readonly IProviderService _ProviderService;

        public ProviderController(IConfiguration config, Microsoft.AspNetCore.Hosting.IHostingEnvironment env, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, ICompanyService companyService, ISettingService SettingService, IProviderService ProviderService) : base(config, env, userManager, roleManager, signInManager)
        {
            this._CompanyService = companyService;
            this._SettingService = SettingService;
            this._ProviderService = ProviderService;
        }

        // GET: ProviderController
        public ActionResult Index()
        {
            ViewBag.CommessionTypes=_SettingService.GetCommessionType().ToList();
            //var providers=_ProviderService.GetAll().Result;
            return View();
        }

        // GET: ProviderController
        public ActionResult ProviderBranches(int? id)
        {
            var ProviderBranches = _ProviderService.GetListProviderBranch(new domain.Models.ProviderbrachesFilter()
            {
             ProviderFk = id,
            });
            return View(ProviderBranches);
        }
        // GET: Providerbrache map
        public ActionResult Map (int id)
       
        {
           
            var model = _ProviderService.GetProviderBranchById(id);
            return View("ProviderBranchesMap" , model);
        }

        // POST: Providerbrache map
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Map(int id, IFormCollection collection)
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



        public ActionResult SearchProvider(string? Provider, int? CommessionType)
        {
            var model = _ProviderService.SearchProvider(new domain.Models.Providerfilter()
            {
                textLike = Provider,
                CommessionType = CommessionType
            });

            return PartialView("_PartialProviderList", model);
        }
        // GET: ProviderController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProviderController/Create
        public ActionResult Create()
        {
            string ProjectLink = _config.GetValue<string>("AppSettings:ApiServer");
            ViewBag.ProjectLink = ProjectLink;
            ViewBag.CommessionTypes = _SettingService.GetCommessionType().ToList();
            return View(new TblProvider());
        }

        // POST: ProviderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Create(TblProvider provider)
        {
            try
            {
                if (provider.ProviderId == 0)
                {
                    provider.CreateDate=DateTime.Now;
                    provider.CreatedBy = LoggedUser.Id;
                    provider.IsDeleted = false;
                    bool added=_ProviderService.Add(provider).IsCompletedSuccessfully;
                    {
                        return Json(new { success = true, message = Resource.SavedSuccessfully });
                    }
                    
                }
                else
                {
                    var res=_ProviderService.Update(provider);
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
            ViewBag.CommessionTypes = _SettingService.GetCommessionType().ToList();
            var model = _ProviderService.Get(id).Result;
            return View("Create",model);
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

        // GET: ProviderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProviderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
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
    }
}
