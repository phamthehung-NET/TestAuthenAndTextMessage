using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestAuthenAndTextMessage.Models;

namespace TestAuthenAndTextMessage.Data
{
    public class ApplicationDbContext : IdentityDbContext<CustomUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Attachment> Attachments { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Conversation> Conversations { get; set; }

        public DbSet<Group> Groups { get; set; }

        public DbSet<Message> Messages { get; set; }

        public DbSet<UserGroupChat> UserGroupsChats { get; set; }
    }
}
