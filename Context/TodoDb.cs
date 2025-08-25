using Microsoft.EntityFrameworkCore;
using TodoApi.Model;

namespace TodoApi.Context
{
    public class TodoDb:DbContext
    {
        public TodoDb(DbContextOptions<TodoDb> options):base(options)
        {
            
        }

        public DbSet<TodoItem> TodoItem { get; set; }
    }
}
