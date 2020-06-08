using Microsoft.EntityFrameworkCore;
using Nats.Client.Domain.Model;

namespace Nats.Client.Infrastructure.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<MessageForSave> MessagesForSave { get; set; }

        public DbSet<MessageForSend> MessagesForSend { get; set; }

    }
}
