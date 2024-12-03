using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WeCareAssignment.Models;

namespace WeCareAssignment.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<WeCareAssignment.Models.DailyActivity> DailyActivity { get; set; } = default!;
        public DbSet<WeCareAssignment.Models.Parent> Parent { get; set; } = default!;
        public DbSet<WeCareAssignment.Models.Teacher> Teacher { get; set; } = default!;
        public DbSet<WeCareAssignment.Models.Child> Child { get; set; } = default!;
    }
}
