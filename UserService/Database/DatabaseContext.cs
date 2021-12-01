﻿using Microsoft.EntityFrameworkCore;

namespace UserService.Database
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
        public DbSet<User> UsersDb { get; set; }
    }    
}
