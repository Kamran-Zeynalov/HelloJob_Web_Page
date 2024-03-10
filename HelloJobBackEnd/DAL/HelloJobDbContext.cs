using HelloJobBackEnd.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace HelloJobBackEnd.DAL
{
    public class HelloJobDbContext : IdentityDbContext<User>
    {
        public HelloJobDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Cv> Cvs { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<OperatingMode> OperatingModes { get; set; }
        public DbSet<BusinessArea> BusinessArea { get; set; }
        public DbSet<BusinessTitle> BusinessTitle { get; set; }
        public DbSet<Vacans> Vacans { get; set; }
        public DbSet<InfoEmployeer> InfoEmployeers { get; set; }
        public DbSet<InfoWork> InfoWorks { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<RequestItem> RequestItems { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<WishListItem> WishListItems { get; set; }
        public DbSet<WishList> WishLists { get; set; }
        public DbSet<Rules> Rules { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Subscribe> Subscribe { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().
                    HasIndex(s => s.Name).
                    IsUnique();
            modelBuilder.Entity<Request>()
              .HasOne(r => r.User)
                 .WithMany()
                 .HasForeignKey(r => r.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<WishList>()
      .HasOne(r => r.User)
         .WithMany()
         .HasForeignKey(r => r.UserId)
          .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);
        }
    }
}
