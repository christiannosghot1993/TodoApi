using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

public class MiDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=todo_db.db");
    }

    // Agrega tus DbSet y modelos aquí
    public DbSet<ToDo> ToDo { get; set; }
}
