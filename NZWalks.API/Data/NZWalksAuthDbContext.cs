using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalks.API.Data
{
    public class NZWalksAuthDbContext : IdentityDbContext
    {
        public NZWalksAuthDbContext(DbContextOptions<NZWalksAuthDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var readerRoleId = "c7758bc3-dc83-4dff-9e79-649c79ca04c9";
            var writerRoleId = "2331bc6d-6ec0-4222-aa17-22c4a80fb70c";
            var roles = new List<IdentityRole>()
            {
                new IdentityRole
                {
                    Id =readerRoleId,
                    ConcurrencyStamp = readerRoleId,
                    Name = "Reader",
                    NormalizedName= "Reader".ToUpper(),
                },
                new IdentityRole
                {
                    Id =writerRoleId,
                    ConcurrencyStamp = writerRoleId,
                    Name = "Writer",
                    NormalizedName= "Writer".ToUpper(),
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
            
        }
    }
}
