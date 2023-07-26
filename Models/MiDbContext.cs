using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

public class MiDbContext : DbContext
{
    public MiDbContext(DbContextOptions<MiDbContext> options) : base(options)
    {

    }
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=todo_db.db");
    }

    // Agrega tus DbSet y modelos aquí
    public DbSet<ToDo> ToDo { get; set; }
}
