using HelloJobBackEnd.DAL;
using HelloJobBackEnd.Entities;
using HelloJobBackEnd.Utilities.Enum;
using HelloJobBackEnd.Utilities.Extension;
using HelloJobBackEnd.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using HelloJobBackEnd.Services.Interface;
using HelloJobBackEnd.Services;
using System.Net.Mail;
using System.Net;

namespace HelloJobBackEnd.Controllers
{
    [Authorize(Roles = "business,employeer")]
    public class MyAccountController : Controller
    {
        private readonly UserManager<User> _usermanager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly HelloJobDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IVacansService _vacansService;
        private readonly ICvPageService _cvPageService;
        private readonly IEmailService _emailService;
        private readonly ILikedService _likedService;
        private readonly ICompanyService _companyService;

        public MyAccountController(UserManager<User> usermanager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager, HelloJobDbContext context, IWebHostEnvironment env, IVacansService vacansService, ICvPageService cvPageService, IEmailService emailService, ILikedService likedService, ICompanyService companyService)
        {
            _usermanager = usermanager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _env = env;
            _vacansService = vacansService;
            _cvPageService = cvPageService;
            _emailService = emailService;
            _likedService = likedService;
            _companyService = companyService;
        }


        public async Task<IActionResult> Index()
        {
            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBags(user);

            return View();
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------


        public async Task<IActionResult> CreateVacans()
        {
            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBags(user);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateVacans(VacansVM newVacans)
        {
            TempData["Create"] = false;
            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBags(user);
            if (!ModelState.IsValid)
            {
                return View();
            }

            Vacans vacans = new()
            {
                CityId = newVacans.CityId,
                OperatingModeId = newVacans.OperatingModeId,
                ExperienceId = newVacans.ExperienceId,
                Salary = newVacans.Salary,
                BusinessAreaId = newVacans.BusinessAreaId,
                EducationId = newVacans.EducationId,
                DrivingLicense = newVacans.DrivingLicense,
                CreatedAt = DateTime.Now,
                EndedAt = DateTime.Now.AddMonths(1),
                Position = newVacans.Position,
                CompanyId = newVacans.CompanyId,
                Status = OrderStatus.Pending,
                Count = 0
            };

            if (newVacans.InfoWorks is null)
            {
                ModelState.AddModelError("", "Please write work's info");
                return View();
            }
            else
            {
                _vacansService.AddInfoWorks(vacans, newVacans.InfoWorks);

            }

            if (newVacans.infoEmployeers is null)
            {
                ModelState.AddModelError("", "Please write work's info");
                return View();
            }
            else
            {
                _vacansService.AddInfoEmployeers(vacans, newVacans.infoEmployeers);
            }
            _context.Vacans.Add(vacans);
            _context.SaveChanges();
            TempData["Create"] = true;
            return RedirectToAction(nameof(MyOrder));
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------





        public async Task<IActionResult> EditVacans(int id)
        {
            if (id == 0) return BadRequest();
            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBags(user);

            VacansVM? vacanVM = _vacansService.GetEditedModelVC(id);
            if (vacanVM is null) return RedirectToAction("Error", "Error");
            return View(vacanVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditVacans(int id, VacansVM editedvacans)
        {
            if (id == 0) return RedirectToAction("Error", "Error");
            TempData["Edited"] = false;
            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBags(user);
            VacansVM? vacanVM = _vacansService.GetEditedModelVC(id);
            if (vacanVM is null) return RedirectToAction("Error", "Error");
            Vacans? vacans = _vacansService.GetVacansWithRelatedEntitiesById(id);
            vacans.InfoWorks.RemoveAll(p => !editedvacans.DeleteWork.Contains(p.Id));
            if (editedvacans.InfoWorks is not null)
            {
                _vacansService.AddInfoWorks(vacans, editedvacans.InfoWorks);

            }
            vacans.infoEmployeers.RemoveAll(p => !editedvacans.DeleteEmployeers.Contains(p.Id));
            if (editedvacans.infoEmployeers is not null)
            {
                _vacansService.AddInfoEmployeers(vacans, editedvacans.infoEmployeers);

            }
            vacans.CityId = editedvacans.CityId;
            vacans.OperatingModeId = editedvacans.OperatingModeId;
            vacans.ExperienceId = editedvacans.ExperienceId;
            vacans.Salary = editedvacans.Salary;
            vacans.BusinessAreaId = editedvacans.BusinessAreaId;
            vacans.EducationId = editedvacans.EducationId;
            vacans.DrivingLicense = editedvacans.DrivingLicense;
            vacans.CreatedAt = DateTime.Now;
            vacans.EndedAt = DateTime.Now.AddMonths(1);
            vacans.Position = editedvacans.Position;
            vacans.Status = OrderStatus.Pending;
            _context.SaveChanges();
            TempData["Edited"] = true;
            return RedirectToAction(nameof(MyOrder));
        }



        //-------------------------------------------------------------------------------------------------------------------------------------------------------




        //-----------------------------------------------------------------------------------------------------------------------------------------------------





        public async Task<IActionResult> DeleteVacans(int id)
        {


            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBags(user);

            VacansVM? vacanVM = _vacansService.GetEditedModelVC(id);

            return View(vacanVM);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteVacans(int id, VacansVM deleteVacans)
        {
            TempData["Deleted"] = false;
            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBags(user);
            VacansVM? vacanVM = _vacansService.GetEditedModelVC(id);
            Vacans? vacans = _vacansService.GetVacansWithRelatedEntitiesById(id);
            List<WishListItem> wishlistItems = _context.WishListItems.Where(w => w.VacansId == vacans.Id).ToList();
            List<RequestItem> requestItem = _context.RequestItems.Where(w => w.VacansId == vacans.Id).ToList();
            _context.WishListItems.RemoveRange(wishlistItems);
            _context.RequestItems.RemoveRange(requestItem);
            _context.Vacans.Remove(vacans);
            _context.SaveChanges();
            TempData["Deleted"] = true;
            return RedirectToAction(nameof(MySticker));
        }



        //-------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------------


        public async Task<IActionResult> MySticker(int page = 1)
        {
            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBags(user);
            if (User.IsInRole(UserRole.employeer.ToString()))
            {
                IQueryable<Cv> allcvs = _cvPageService.GetAllCvs();
                ViewBag.TotalPage = Math.Ceiling((double)_context.Cvs.Where(x => x.Status == OrderStatus.Accepted && x.UserId == user.Id).Count() / 6);
                ViewBag.CurrentPage = page;
                ViewBag.Allcv = allcvs.AsNoTracking().OrderByDescending(x => x.Id).Skip((page - 1) * 6).Where(x => x.Status == OrderStatus.Accepted && x.UserId == user.Id).Take(6).ToList();
                return View();
            }
            else
            {
                IQueryable<Vacans> allvacans = _vacansService.GetAcceptedVacansWithRelatedData();
                ViewBag.TotalPage = Math.Ceiling((double)_context.Vacans.Where(x => x.Status == OrderStatus.Accepted && x.Company.UserId == user.Id).Count() / 6);
                ViewBag.CurrentPage = page;
                ViewBag.Allvacans = allvacans.AsNoTracking().OrderByDescending(x => x.Id).Where(x => x.Status == OrderStatus.Accepted && x.Company.UserId == user.Id).Skip((page - 1) * 6).Take(6).ToList();
                return View();

            }

        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------
        public async Task<IActionResult> CreateCv()
        {
            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBags(user);


            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCv(CvVM newCv)
        {
            TempData["Create"] = false;
            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBags(user);

            if (!ModelState.IsValid)
            {
                return View();
            }

            Cv cv = new()
            {
                User = user,
                Name = newCv.Name,
                Surname = newCv.Surname,
                CityId = newCv.CityId,
                BornDate = newCv.BornDate,
                OperatingModeId = newCv.OperatingModeId,
                ExperienceId = newCv.ExperienceId,
                Salary = newCv.Salary,
                BusinessAreaId = newCv.BusinessAreaId,
                EducationId = newCv.EducationId,
                DrivingLicense = newCv.DrivingLicense,
                CreatedAt = DateTime.Now,
                EndedAt = DateTime.Now.AddMonths(1),
                Email = newCv.Email,
                Number = newCv.Number,
                Position = newCv.Position,
                Status = OrderStatus.Pending,
                Count = 0
            };

            if (newCv.Image is not null)
            {
                if (!newCv.Image.IsValidFile("image/"))
                {
                    ModelState.AddModelError(string.Empty, "Şəkil file seçin");
                    return View();
                }

                if (!newCv.Image.IsValidLength(2))
                {
                    ModelState.AddModelError(string.Empty, "Cv şəkili max 2Mb ola bilər..");
                    return View();
                }

                var imageFolderPath = Path.Combine(_env.WebRootPath, "assets", "images");
                cv.Image = await newCv.Image.CreateImage(imageFolderPath, "User");
            }
            else
            {
                cv.Image = "user-128.png";
            }

            if (newCv.CvPDF is not null)
            {
                var pdfFolderPath = Path.Combine(_env.WebRootPath, "assets", "images", "User");
                cv.CvPDF = await newCv.CvPDF.CreateImage(pdfFolderPath, "CVs");
            }

            if (newCv.CvPDF is null)
            {
                ModelState.AddModelError(string.Empty, "Cv fileni seçin");
                return View();
            }
            _context.Cvs.Add(cv);
            _context.SaveChanges();
            TempData["Create"] = true;
            return RedirectToAction(nameof(MyOrder));
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------

        //-----------------------------------------------------------------------------------------------------------------------------------------------------





        public async Task<IActionResult> EditCV(int id)
        {

            if (id == 0) return RedirectToAction("Error", "Error");
            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }
            CvVM? cvVm = _cvPageService.EditedModelCv(id);
            ViewBags(user);
            if (cvVm is null) return RedirectToAction("Error", "Error"); ;
            return View(cvVm);
        }


        [HttpPost]
        public async Task<IActionResult> EditCV(int id, CvVM editedCv)
        {
            if (id == 0) return BadRequest();
            TempData["Edited"] = false;
            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBags(user);

            CvVM? cvVm = _cvPageService.EditedModelCv(id);

            Cv? cv = _cvPageService.Details(id);

            if (cv is null) return BadRequest();

            if (editedCv.Image is not null)
            {
                if (!editedCv.Image.IsValidFile("image/"))
                {
                    ModelState.AddModelError(string.Empty, "Şəkil file seçin");
                    return View();
                }
                if (!editedCv.Image.IsValidLength(2))
                {
                    ModelState.AddModelError(string.Empty, "Maximum ölçü 1 mb ola bilər");
                    return View();
                }
                var imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "images");
                string filepath = Path.Combine(imagefolderPath, "User", cv.Image);
                ExtensionMethods.DeleteImage(filepath);
                cv.Image = await editedCv.Image.CreateImage(imagefolderPath, "User");
            }

            if (editedCv.CvPDF is not null)
            {
                var cvfolderPath = Path.Combine(_env.WebRootPath, "assets", "images", "User");
                string filepath = Path.Combine(cvfolderPath, "CVs", cv.CvPDF);
                ExtensionMethods.DeleteImage(filepath);
                cv.CvPDF = await editedCv.CvPDF.CreateImage(cvfolderPath, "CVs");
            }

            cv.Name = editedCv.Name;
            cv.Email = editedCv.Email;
            cv.BusinessAreaId = editedCv.BusinessAreaId;
            cv.CityId = editedCv.CityId;
            cv.EducationId = editedCv.EducationId;
            cv.ExperienceId = editedCv.ExperienceId;
            cv.OperatingModeId = editedCv.OperatingModeId;
            cv.Salary = editedCv.Salary;
            cv.Position = editedCv.Position;
            cv.Number = editedCv.Number;
            cv.Status = OrderStatus.Pending;
            _context.SaveChanges();
            TempData["Edited"] = true;
            return RedirectToAction(nameof(MyOrder));
        }



        //-------------------------------------------------------------------------------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------------------------------------------------------------------------





        public async Task<IActionResult> DeleteCV(int id)
        {


            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBags(user);
            CvVM? cv = _cvPageService.EditedModelCv(id);

            return View(cv);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCV(int id, CvVM deleteCv)
        {
            TempData["Deleted"] = false;
            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBags(user);

            CvVM? cvvm = _cvPageService.EditedModelCv(id);

            Cv? cv = _cvPageService.Details(id);
            var imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "images");
            string filepath = Path.Combine(imagefolderPath, "User", cv.Image);
            ExtensionMethods.DeleteImage(filepath);
            string pdfpath = Path.Combine(imagefolderPath, "User", "CVs", cv.CvPDF);
            ExtensionMethods.DeleteImage(pdfpath);
            List<WishListItem> wishlistItems = _context.WishListItems.Where(w => w.CvId == cv.Id).ToList();
            List<RequestItem> requestItem = _context.RequestItems.Where(w => w.CvId == cv.Id).ToList();
            _context.WishListItems.RemoveRange(wishlistItems);
            _context.RequestItems.RemoveRange(requestItem);
            _context.Cvs.Remove(cv);
            _context.SaveChanges();
            TempData["Deleted"] = true;
            return RedirectToAction(nameof(MySticker));
        }



        //-------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------------


        public async Task<IActionResult> MyOrder()
        {
            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBags(user);

            if (User.IsInRole(UserRole.employeer.ToString()))
            {
                IQueryable<Cv> cvs = _cvPageService.GetAllCvs();
                ViewBag.Allcv = cvs.Where(x => x.Status == OrderStatus.Pending && x.UserId == user.Id).ToList();
                return View();
            }
            else
            {
                IQueryable<Vacans> vacans = _vacansService.GetAcceptedVacansWithRelatedData();

                ViewBag.Allvacans = vacans.Where(x => x.Status == OrderStatus.Pending && x.Company.UserId == user.Id).ToList();
                return View();

            }

        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------


        public async Task<IActionResult> MyRequest()
        {

            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBags(user);

            if (User.IsInRole(UserRole.employeer.ToString()))
            {
                ViewBag.AllRequest = _context.Requests
                  .Include(r => r.RequestItems)
                     .ThenInclude(ri => ri.Cv)
                 .Include(r => r.RequestItems)
                     .ThenInclude(ri => ri.Vacans)
                 .ThenInclude(v => v.Company).Where(x => x.User == user)
                  .ToList();
            }
            else
            {
                ViewBag.AllRequest = _context.Requests
                .Include(r => r.RequestItems)
                 .ThenInclude(ri => ri.Cv)
                    .Include(r => r.RequestItems)
                  .ThenInclude(ri => ri.Vacans)
                   .ThenInclude(v => v.Company)
                .Where(x => x.User == user || x.RequestItems.Any(ri => ri.Vacans.Company.User == user))
                    .ToList();


            }
            return View();
        }

        public async Task<IActionResult> AcceptRequestItem(int requestId, int requestItemId)
        {
            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }

            Request? request = _context.Requests
                .Include(r => r.RequestItems)
                .ThenInclude(ri => ri.Cv)
                .Include(r => r.RequestItems)
                .ThenInclude(ri => ri.Cv.User)
                .Include(r => r.RequestItems)
                .ThenInclude(ri => ri.Vacans)
                .ThenInclude(v => v.Company)
                .FirstOrDefault(r => r.Id == requestId && (r.User == user || r.RequestItems.Any(ri => ri.Vacans.Company.User == user)));

            if (request is null)
            {
                return RedirectToAction("Index", "Myaccount");
            }

            RequestItem requestItem = request.RequestItems.FirstOrDefault(ri => ri.Id == requestItemId);
            if (requestItem is null)
            {
                return RedirectToAction("Index", "Myaccount");
            }

            if (requestItem.Status != OrderStatus.Accepted)
            {
                requestItem.Status = OrderStatus.Accepted;
                _context.SaveChanges();

                string recipientEmail = requestItem.Cv.User.Email;
                string subject = "Bildiriş";
                string body = string.Empty;

                using (StreamReader reader = new StreamReader("wwwroot/assets/template/acceptedmail.html"))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{{userFullName}}", string.Concat(requestItem.Cv.Name, " ", requestItem.Cv.Surname));
                body = body.Replace("{{companyName}}", requestItem.Vacans.Company.Name);
                body = body.Replace("{{position}}", requestItem.Vacans.Position);

                _emailService.SendEmail(recipientEmail, subject, body);
            }

            return RedirectToAction(nameof(MyRequest));
        }

        //---------------------------------------------------------------------------------------------------------------------------



        //--------------------------------------------------------------------------------------------------------------------------
        public async Task<IActionResult> RejectedRequestItem(int requestId, int requestItemId)
        {
            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }

            Request? request = _context.Requests
                .Include(r => r.RequestItems)
                .ThenInclude(ri => ri.Cv)
                .Include(r => r.RequestItems)
                .ThenInclude(ri => ri.Cv.User)
                .Include(r => r.RequestItems)
                .ThenInclude(ri => ri.Vacans)
                .ThenInclude(v => v.Company)
                .FirstOrDefault(r => r.Id == requestId && (r.User == user || r.RequestItems.Any(ri => ri.Vacans.Company.User == user)));

            if (request is null)
            {
                return RedirectToAction("Index", "Myaccount");
            }

            RequestItem requestItem = request.RequestItems.FirstOrDefault(ri => ri.Id == requestItemId);
            if (requestItem is null)
            {
                return RedirectToAction("Index", "Myaccount");
            }

            if (requestItem.Status != OrderStatus.Rejected)
            {
                requestItem.Status = OrderStatus.Rejected;
                _context.SaveChanges();

                string recipientEmail = requestItem.Cv.User.Email;
                string subject = "Bildiriş";
                string body = string.Empty;

                using (StreamReader reader = new StreamReader("wwwroot/assets/template/RejectedMail.html"))
                {
                    body = reader.ReadToEnd();
                }

                body = body.Replace("{{userFullName}}", string.Concat(requestItem.Cv.Name, " ", requestItem.Cv.Surname));
                body = body.Replace("{{companyName}}", requestItem.Vacans.Company.Name);
                body = body.Replace("{{position}}", requestItem.Vacans.Position);

                _emailService.SendEmail(recipientEmail, subject, body);
            }

            return RedirectToAction(nameof(MyRequest));
        }



        public async Task<IActionResult> DeleteRequest(int requestId, int requestItemId)
        {
            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }

            Request? request = _context.Requests
                .Include(r => r.RequestItems)
                .ThenInclude(ri => ri.Cv)
                .Include(r => r.RequestItems)
                .ThenInclude(ri => ri.Cv.User)
                .Include(r => r.RequestItems)
                .ThenInclude(ri => ri.Vacans)
                .ThenInclude(v => v.Company)
                .FirstOrDefault(r => r.Id == requestId && (r.User == user || r.RequestItems.Any(ri => ri.Vacans.Company.User == user)));

            if (request is null)
            {
                return RedirectToAction("Index", "Myaccount");
            }

            RequestItem requestItem = request.RequestItems.FirstOrDefault(ri => ri.Id == requestItemId);
            if (requestItem is null)
            {
                return RedirectToAction("Index", "Myaccount");
            }

            request.RequestItems.Remove(requestItem);
            _context.SaveChanges();

            return RedirectToAction(nameof(MyRequest));
        }








        public async Task<IActionResult> WishListPage()
        {
            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBags(user);
            List<WishList> wishlists = await _likedService.GetWishLists(user);
            ViewBag.Setting = _context.Settings.ToDictionary(s => s.Key, s => s.Value);

            return View(wishlists);
        }


        //-------------------------------------------------------------------------------------------------------------------------------------------------------


        public async Task<IActionResult> Security()
        {
            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBags(user);
            ProfileVM profileVM = new()
            {
                Email = user.Email,
                UserName = user.UserName,
                FullName = user.FullName
            };
            return View(profileVM);
        }

        [HttpPost]
        public async Task<IActionResult> Security(ProfileVM profileVM)
        {
            if (!ModelState.IsValid)
            {
                foreach (string message in ModelState.Values.SelectMany(e => e.Errors).Select(e => e.ErrorMessage))
                {
                    ModelState.AddModelError("", message);
                }
            }
            TempData["Security"] = false;
            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            ViewBags(user);
            if (!string.IsNullOrWhiteSpace(profileVM.ConfirmNewPassword) && !string.IsNullOrWhiteSpace(profileVM.NewPassword))
            {
                var passwordChangeResult = await _usermanager.ChangePasswordAsync(user, profileVM.CurrentPassword, profileVM.NewPassword);

                if (!passwordChangeResult.Succeeded)
                {
                    foreach (var item in passwordChangeResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }

                    return View();
                }

            }

            user.UserName = profileVM.UserName;
            var result = await _usermanager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }

                return View();
            }
            await _signInManager.SignOutAsync();
            TempData["Security"] = true;
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> DeleteUserData(string userId)
        {
            User user = await _usermanager.FindByIdAsync(userId);
            if (user != null)
            {
                IQueryable<Cv> cvsToDelete = _context.Cvs.Where(cv => cv.UserId == userId);
                _context.Cvs.RemoveRange(cvsToDelete);

                IQueryable<Company> companiesToDelete = _context.Companies.Where(company => company.UserId == userId);
                _context.Companies.RemoveRange(companiesToDelete);

                IQueryable<Request> requestToDelete = _context.Requests.Where(rq => rq.UserId == userId);
                _context.Requests.RemoveRange(requestToDelete);

                IQueryable<WishList> wishListsDelete = _context.WishLists.Where(ws => ws.UserId == userId);
                _context.WishLists.RemoveRange(wishListsDelete);

                await _usermanager.DeleteAsync(user);

                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction(nameof(Security));

        }
        //-----------------------------------------------------------------------------------------------------------------------------------------------------

        public async Task<IActionResult> CreatingCompany()
        {

            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBags(user);


            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreatingCompany(CompanyVM newCompany)
        {
            TempData["Company"] = false;
            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBags(user);

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!newCompany.Image.IsValidFile("image/"))
            {
                ModelState.AddModelError(string.Empty, "Şəkil file seçin");
                return View();
            }
            if (!newCompany.Image.IsValidLength(1))
            {
                ModelState.AddModelError(string.Empty, "Maximum ölçü 1 mb ola bilər");
                return View();
            }
            Company company = new()
            {
                User = user,
                Name = newCompany.Name,
                Email = newCompany.Email,
                Status = OrderStatus.Pending
            };

            var imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "images");
            company.Image = await newCompany.Image.CreateImage(imagefolderPath, "Company");

            _context.Companies.Add(company);
            _context.SaveChanges();
            TempData["Company"] = true;
            return RedirectToAction(nameof(MyCompany));
        }



        //-----------------------------------------------------------------------------------------------------------------------------------------------------





        public async Task<IActionResult> EditCompany(int id)
        {

            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBags(user);
            CompanyVM? companyVM = _companyService.GetCompanyById(id);

            return View(companyVM);
        }
        [HttpPost]
        public async Task<IActionResult> EditCompany(int id, CompanyVM editedCompany)
        {
            TempData["Edited"] = false;
            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBags(user);

            CompanyVM? companyVM = _companyService.GetCompanyById(id);

            Company company = _companyService.GetCompanyWithVacansById(id);

            if (editedCompany.Image is not null)
            {
                if (!editedCompany.Image.IsValidFile("image/"))
                {
                    ModelState.AddModelError(string.Empty, "Şəkil file seçin");
                    return View();
                }
                if (!editedCompany.Image.IsValidLength(2))
                {
                    ModelState.AddModelError(string.Empty, "Maximum ölçü 1 mb ola bilər");
                    return View();
                }
                await AdjustPlantPhoto(editedCompany.Image, company);
            }

            company.Name = editedCompany.Name;
            company.Email = editedCompany.Email;
            _context.SaveChanges();
            TempData["Edited"] = true;
            return RedirectToAction(nameof(MyCompany));
        }



        //-------------------------------------------------------------------------------------------------------------------------------------------------------


        //-----------------------------------------------------------------------------------------------------------------------------------------------------





        public async Task<IActionResult> DeleteCompany(int id)
        {

            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBags(user);
            CompanyVM? companyVM = _companyService.GetCompanyById(id);


            return View(companyVM);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCompany(int id, CompanyVM deleteCompany)
        {
            TempData["Deleted"] = false;
            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBags(user);

            CompanyVM? companyVM = _companyService.GetCompanyById(id);


            Company company = _companyService.GetCompanyWithVacansById(id);

            var imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "images");
            string filepath = Path.Combine(imagefolderPath, "Company", company.Image);
            ExtensionMethods.DeleteImage(filepath);

            _context.Companies.Remove(company);
            _context.SaveChanges();
            TempData["Deleted"] = true;
            return RedirectToAction(nameof(MyCompany));
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------
        public async Task<IActionResult> MyCompany()
        {
            User user = await _usermanager.FindByNameAsync(User.Identity.Name);
            if (user is null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBags(user);
            List<Company> companies = _context.Companies.Include(v => v.Vacans).Where(u => u.UserId == user.Id).ToList();
            return View(companies);
        }
        public void ViewBags(User user)
        {
            ProfileVM profileVM = new()
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FullName = user.FullName
            };
            ViewBag.User = profileVM;
            ViewBag.Cities = _context.Cities.OrderBy(x => x.Name).ToList();
            ViewBag.Education = _context.Educations.ToList();
            ViewBag.Experince = _context.Experiences.ToList();
            ViewBag.Mode = _context.OperatingModes.ToList();
            ViewBag.Business = _context.BusinessTitle.Include(b => b.BusinessAreas).ToList();
            ViewBag.Company = _context.Companies.Where(x => x.UserId == user.Id).ToList();

        }


        private async Task AdjustPlantPhoto(IFormFile image, Company company)
        {
            var imagefolderPath = Path.Combine(_env.WebRootPath, "assets", "images");
            string filepath = Path.Combine(imagefolderPath, "Company", company.Image);
            ExtensionMethods.DeleteImage(filepath);
            company.Image = await image.CreateImage(imagefolderPath, "Company");
        }



    }
}
