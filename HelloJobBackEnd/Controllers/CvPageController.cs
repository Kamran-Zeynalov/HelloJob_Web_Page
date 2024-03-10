using HelloJobBackEnd.DAL;
using HelloJobBackEnd.Entities;
using HelloJobBackEnd.Services;
using HelloJobBackEnd.Services.Interface;
using HelloJobBackEnd.Utilities.Enum;
using HelloJobBackEnd.Utilities.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HelloJobBackEnd.Controllers
{
    public class CvPageController : Controller
    {
        private readonly HelloJobDbContext _context;
        private readonly ICvPageService _cvPageService;

        public CvPageController(HelloJobDbContext context, ICvPageService cvPageService)
        {
            _context = context;
            _cvPageService = cvPageService;
        }
        public IActionResult Index(int page = 1)
        {
            IQueryable<Cv> allcvs = _cvPageService.GetAllCvs();
            ViewBag.Education = _context.Educations.ToList();
            ViewBag.Experince = _context.Experiences.ToList();
            ViewBag.Mode = _context.OperatingModes.ToList();
            ViewBag.Business = _context.BusinessTitle.Include(b => b.BusinessAreas).ThenInclude(b => b.Cvs.Where(x => x.TimeIsOver == false)).ToList();
            ViewBag.Setting = _context.Settings.ToDictionary(s => s.Key, s => s.Value);
            ViewBag.TotalPage = Math.Ceiling((double)_context.Cvs.Count() / 8);
            ViewBag.CurrentPage = page;

            List<Cv> cvs = allcvs.Skip((page - 1) * 8).Take(8).Where(x => x.Status == OrderStatus.Accepted && x.TimeIsOver == false).OrderByDescending(x => x.Id).ToList();

            return View(cvs);
        }




        [HttpPost]
        public async Task<IActionResult> Sorts(string? sort, int page = 1)
        {
            List<Cv> filtered = await _cvPageService.GetSortedCvs(sort);

            List<Cv> cvs = filtered.Skip((page - 1) * 8).Take(8).Where(x => x.Status == OrderStatus.Accepted && x.TimeIsOver == false).ToList();
            ViewBag.Education = _context.Educations.ToList();
            ViewBag.Experince = _context.Experiences.ToList();
            ViewBag.Mode = _context.OperatingModes.ToList();
            ViewBag.Setting = _context.Settings.ToDictionary(s => s.Key, s => s.Value);
            ViewBag.TotalPage = Math.Ceiling((double)_context.Cvs.Count() / 8);
            ViewBag.CurrentPage = page;
            ViewBag.Business = _context.BusinessTitle.Include(b => b.BusinessAreas).ThenInclude(b => b.Cvs.Where(x => x.TimeIsOver == false)).ToList();

            return PartialView("_UserblocksPartial", cvs);
        }

        [HttpPost]
        public async Task<IActionResult> FilterData(int[] businessIds, int[] modeIds, int[] educationIds, int[] experienceIds, bool? hasDriverLicense, int page = 1)
        {

            ViewBag.Setting = _context.Settings.ToDictionary(s => s.Key, s => s.Value);
            ViewBag.TotalPage = Math.Ceiling((double)_context.Cvs.Count() / 8);
            ViewBag.CurrentPage = page;
            List<Cv> filteredCvs = await _cvPageService.GetFilteredData(businessIds, modeIds, educationIds, experienceIds, hasDriverLicense);
            List<Cv> PageCv = filteredCvs.Skip((page - 1) * 8).Take(8).Where(s => s.Status == OrderStatus.Accepted).OrderByDescending(x => x.Id).ToList();
            return PartialView("_UserblocksPartial", PageCv);
        }

        public IActionResult Detail(int id)
        {
            if (id == 0) return RedirectToAction("Error", "Error");
            IQueryable<Cv> allcvs = _cvPageService.GetAllCvs();


            Cv? cv = _cvPageService.Details(id);
            if (cv is null) RedirectToAction("Error", "Error");
            cv.Count++;
            _context.SaveChanges();
            ViewBag.Related = ExtensionMethods.RelatedByBusinessArea(allcvs, cv, id);
            ViewBag.Setting = _context.Settings.ToDictionary(s => s.Key, s => s.Value);

            return View(cv);
        }

        public async Task<IActionResult> Search(string search)
        {
            IQueryable<Cv> query = _cvPageService.GetAllCvs()
                      .Where(x => x.Position.Contains(search));

            List<Cv> cv = query.OrderByDescending(x => x.Id)
                .Take(3)
                .Where(c => c.Status == OrderStatus.Accepted)
                .ToList();

            return PartialView("_SerachcvPartial", cv);
        }



        public async Task<IActionResult> SearchResult(string search, int page = 1)
        {
            ViewBag.TotalPage = Math.Ceiling((double)_context.Cvs.Count() / 8);
            ViewBag.CurrentPage = page;
            IQueryable<Cv> allcv = _cvPageService.GetAllCvs();
            ViewBag.Setting = _context.Settings.ToDictionary(s => s.Key, s => s.Value);
            if (search is not null)
            {
                allcv = allcv.Where(c => c.Position.Contains(search));
            }
            else
            {
                allcv = allcv;
            }
            List<Cv> searching = allcv.Skip((page - 1) * 8).Take(8).Where(s => s.Status == OrderStatus.Accepted).ToList();

            return PartialView("_UserblocksPartial", searching);
        }


    }
}
