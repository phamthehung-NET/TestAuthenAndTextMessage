using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestAuthenAndTextMessage.Models;
using TestAuthenAndTextMessage.Ultilities;

namespace TestAuthenAndTextMessage.Data
{
    public class ApplicationDbContext : IdentityDbContext<CustomUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
			var passwordHasher = new PasswordHasher<CustomUser>();
			CustomUser admin = new()
			{
				Id = "1",
				UserName = "Admin",
				FirstName = "System",
				LastName = "Admin",
				Email = "admin@gmail.com",
				NormalizedUserName = "admin",
				PasswordHash = passwordHasher.HashPassword(null, "Abc@12345"),
				LockoutEnabled = true,
				EmailConfirmed = true,
			};
			builder.Entity<CustomUser>().HasData(admin);
			
			builder.Entity<IdentityRole>().HasData(
				new IdentityRole() { Id = "1", Name = Constants.AdminRole, ConcurrencyStamp = "1", NormalizedName = "admin" },
				new IdentityRole() { Id = "2", Name = Constants.UserRole, ConcurrencyStamp = "2", NormalizedName = "user" }
			);

			builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>() { RoleId = "1", UserId = "1" });
		}

		public DbSet<Attachment> Attachments { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Conversation> Conversations { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<UserGroupChat> UserGroupsChats { get; set; }
    }
}
