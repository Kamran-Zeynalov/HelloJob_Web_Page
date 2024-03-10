using HelloJobBackEnd.DAL;
using HelloJobBackEnd.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelloJobBackEnd.Services
{
    public class UserService
    {
        private readonly HelloJobDbContext _context;

        public UserService(HelloJobDbContext context)
        {
            _context = context;
        }

        public List<User> GetAllUsers()
        {

            List<User> users = _context.Users.Include(x => x.Cvs).Include(x => x.Companies).ThenInclude(x => x.Vacans).ToList();
            return users;
        }

        public User GetUserById(string id)
        {
            User? users = _context.Users.Include(x => x.Cvs).Include(x => x.Companies).ThenInclude(x => x.Vacans).FirstOrDefault(x => x.Id == id);
            return users;
        }

    }

}
