
using IdentityDemoApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityDemoApp.Data
{
    public class ApplicationDBContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }

        public DbSet<Category> Categories{get;set;}

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
            

        //    //deleting foriegn key constraints
        //    foreach(var foriegnkey in builder.Model.GetEntityTypes()
        //        .SelectMany(e=>e.GetForeignKeys()))
        //    {
        //        foriegnkey.DeleteBehavior = DeleteBehavior.Restrict;
        //    }



        //}

    }
}
