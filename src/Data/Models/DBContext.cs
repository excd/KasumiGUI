using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Reflection;

namespace KasumiGUI.Data.Models
{
    public class DBContext : DbContext
    {
        private string? DbPath { get; }
        public DbSet<DechiResponse> Dechi { get; set; }
        public DbSet<EightBallResponse> EightBall { get; set; }
        public DbSet<PokeResponse> Poke { get; set; }
        public DbSet<BullyResponse> Bully { get; set; }
        public DbSet<ChoiceResponse> Choice { get; set; }

        public DBContext()
        {
            string dbDirectory = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), Assembly.GetExecutingAssembly().GetName().Name);
            if (!Directory.Exists(dbDirectory))
                _ = Directory.CreateDirectory(dbDirectory);
            DbPath = Path.Join(dbDirectory, ConfigurationManager.AppSettings["SQLiteDBFile"]);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder.Entity<DechiResponse>().ToTable("Dechi");
            _ = modelBuilder.Entity<EightBallResponse>().ToTable("EightBall");
            _ = modelBuilder.Entity<PokeResponse>().ToTable("Poke");
            _ = modelBuilder.Entity<BullyResponse>().ToTable("Bully");
            _ = modelBuilder.Entity<ChoiceResponse>().ToTable("Choice");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={DbPath}");
    }

    public class BaseResponse
    {
        public int Id { get; set; }
        public string? Message { get; set; }
    }

    public sealed class DechiResponse : BaseResponse { }
    public sealed class EightBallResponse : BaseResponse { }
    public sealed class PokeResponse : BaseResponse { }
    public sealed class BullyResponse : BaseResponse { }
    public sealed class ChoiceResponse : BaseResponse { }
}