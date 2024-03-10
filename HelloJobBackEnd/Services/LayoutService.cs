using HelloJobBackEnd.DAL;
using HelloJobBackEnd.Entities;
using HelloJobBackEnd.Utilities.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HelloJobBackEnd.Services
{
    public class LayoutService
    {
        private readonly HelloJobDbContext _context;
        private readonly IHttpContextAccessor _accessor;
        private readonly UserManager<User> _userManager;

        public LayoutService(HelloJobDbContext context, IHttpContextAccessor accessor, UserManager<User> userManager)
        {
            _context = context;
            _accessor = accessor;
            _userManager = userManager;
        }

        public async Task<User> GetUserFullName()
        {
            var user = await _userManager.GetUserAsync(_accessor.HttpContext.User);
            return user;
        }

        public Dictionary<string, string> GetSettings()
        {
            Dictionary<string, string> settings = _context.Settings.ToDictionary(s => s.Key, s => s.Value);

            return settings;
        }

        public (List<Cv> cvList, List<Company> companyList, List<Vacans> vacansList) GetAllData()
        {
            var cvList = _context.Cvs.Where(x => x.Status == OrderStatus.Pending).ToList();
            var companyList = _context.Companies.Where(x => x.Status == OrderStatus.Pending).ToList();
            var vacansList = _context.Vacans.Include(x => x.Company)
                .Include(b => b.BusinessArea).ThenInclude(x => x.BusinessTitle)
                .Where(x => x.Status == OrderStatus.Pending).ToList();

            return (cvList, companyList, vacansList);
        }


        public List<RequestItem> GetAcceptedOrRejectedRequests(string userId)
        {
            List<RequestItem> acceptedOrRejectedRequests = new List<RequestItem>();

            List<Request> requests = _context.Requests
                .Include(r => r.RequestItems)
                .Where(r => r.UserId == userId)
                .ToList();

            foreach (Request request in requests)
            {
                foreach (RequestItem requestItem in request.RequestItems)
                {
                    if (requestItem.Status == OrderStatus.Accepted || requestItem.Status == OrderStatus.Rejected)
                    {
                        acceptedOrRejectedRequests.Add(requestItem);
                    }
                }
            }

            return acceptedOrRejectedRequests;
        }


        public List<OperatingMode> GetMode()
        {
            List<OperatingMode> modes = _context.OperatingModes.ToList();

            return modes;
        }

    }
}
