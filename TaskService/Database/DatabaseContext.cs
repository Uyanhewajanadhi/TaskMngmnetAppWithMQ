using Microsoft.EntityFrameworkCore;

namespace TaskService.Database
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        public DbSet<TasksDb> Tasks { get; set; }

        public DbSet<PauseSessions> PauseSessions { get; set; }
    }
}
