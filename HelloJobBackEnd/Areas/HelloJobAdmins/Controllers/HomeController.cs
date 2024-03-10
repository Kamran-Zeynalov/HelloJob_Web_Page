using HelloJobBackEnd.Areas.HelloJobAdmins.ViewModel;
using HelloJobBackEnd.DAL;
using HelloJobBackEnd.Entities;
using HelloJobBackEnd.Services;
using HelloJobBackEnd.Services.Interface;
using HelloJobBackEnd.Utilities.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HelloJobBackEnd.Areas.HelloJobAdmins.Controllers
{

    [Authorize(Roles = "superadmin, admin, moderator")]
    [Area("HelloJobAdmins")]
    public class HomeController : Controller
    {
        private readonly HelloJobDbContext _context;
        private readonly IVacansService _vacansService;
        private readonly ICvPageService _cvPageService;
        private readonly IBusinessTitleService _businessTitleService;
        private readonly ICompanyService _companyService;
        private readonly UserService _userService;

        public HomeController(HelloJobDbContext context, IVacansService vacansService, ICvPageService cvPageService, IBusinessTitleService businessTitleService, ICompanyService companyService, UserService userService)
        {
            _context = context;
            _vacansService = vacansService;
            _cvPageService = cvPageService;
            _businessTitleService = businessTitleService;
            _companyService = companyService;
            _userService = userService;
        }


        public IActionResult Index()
        {
            List<Vacans> vacansList = _vacansService.GetAcceptedVacansWithRelatedData().Where(x => x.TimeIsOver == false).ToList();
            List<Cv> cvList = _cvPageService.GetAllCvs().Where(x => x.TimeIsOver == false).ToList();
            List<Company> companyList = _companyService.GetTopAcceptedCompaniesWithVacans().ToList();
            List<BusinessTitle> businessTitles = _businessTitleService.GetAllBusinessTitlesWithAreas();
            List<OperatingMode> operatingModes = _context.OperatingModes.Include(x => x.Cvs).Include(x => x.Vacans).ToList();
            List<CompanyChartData> companyVacansCounts = companyList.Select(c => new CompanyChartData
            {
                CompanyName = c.Name,
                VacansCount = vacansList.Count(v => v.CompanyId == c.Id)
            }).ToList();

            List<BusinessTitleChartData> CountByBusinessArea = businessTitles.Select(ba => new BusinessTitleChartData
            {
                BusinessName = ba.Name,
                CvCount = cvList.Count(cv => cv.BusinessArea.BusinessTitleId == ba.Id),
                VacansCount = vacansList.Count(v => v.BusinessArea.BusinessTitleId == ba.Id)
            }).ToList();

            List<ModesVM> modesCount = operatingModes.Select(ba => new ModesVM
            {
                ModeName = ba.Name,
                CvCount = cvList.Count(cv => cv.OperatingModeId == ba.Id),
                VacansCount = vacansList.Count(v => v.OperatingModeId == ba.Id)
            }).ToList();
            var labels_company = companyVacansCounts.Select(cv => cv.CompanyName).ToList();
            var labels = businessTitles.Select(ba => ba.Name).ToList();
            var labelsMode = operatingModes.Select(ba => ba.Name).ToList();
            var datasetData = companyVacansCounts.Select(cv => cv.VacansCount).ToList();
            var dataset1Data = CountByBusinessArea.Select(item => item.CvCount).ToList();
            var dataset2Data = CountByBusinessArea.Select(item => item.VacansCount).ToList();
            var dataset3Data = modesCount.Select(item => item.CvCount).ToList();
            var dataset4Data = modesCount.Select(item => item.VacansCount).ToList();
            ViewBag.LabelsCompany = labels_company;
            ViewBag.DatasetData = datasetData;
            ViewBag.Labels = labels;
            ViewBag.LabelsMode = labelsMode;

            ViewBag.Dataset1Data = dataset1Data;
            ViewBag.Dataset2Data = dataset2Data;
            ViewBag.Vacans = vacansList;
            ViewBag.Company = companyList;
            ViewBag.Cvs = cvList;
            ViewBag.Users = _userService.GetAllUsers();
            return View();
        }




    }

}

