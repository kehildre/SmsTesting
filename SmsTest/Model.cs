using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SmsTest
{
    public class UserContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=user.db");
    }

    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        public string UserPassword { get; set; }

    }

    
}