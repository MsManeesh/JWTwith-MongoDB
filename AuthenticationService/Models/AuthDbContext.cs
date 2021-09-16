using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Models
{
    public class AuthDbContext:DbContext
    {
        public AuthDbContext(DbContextOptions options) : base(options)
        {
              //make sure that database is auto generated using EF Core Code first
        }
        public DbSet<User> Users { get; set; }
        //Define a Dbset for User in the database
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var User = modelBuilder.Entity<User>();
            User.HasKey(x => x.UserId);
            User.Property(x => x.Password).IsRequired();
            User.Property(x => x.UserId).ValueGeneratedNever();
        }
    }
}
