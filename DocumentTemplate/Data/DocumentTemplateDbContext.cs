using DocumentTemplate.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentTemplate.Data
{
    public class DocumentTemplateDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Logging> Loggings { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<Email> Emails { get; set; }
        public DocumentTemplateDbContext(DbContextOptions<DocumentTemplateDbContext> options)
            : base(options)
        {

        }
    }
}
