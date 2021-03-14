using Microsoft.EntityFrameworkCore;

namespace api.todo
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
        }

        public DbSet<TodoItemDb> TodoItems { get; set; }
    }
}