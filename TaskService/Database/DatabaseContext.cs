using Microsoft.EntityFrameworkCore;

namespace TaskService.Database
{
    public class DatabaseContext: DbContext
    {
        //public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        public virtual DbSet<TasksDb> Tasks { get; set; }

        public virtual DbSet<PauseSessions> PauseSessions { get; set; }
    }
}
