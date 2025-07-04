using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public DbSet<Titulo> Titulos { get; set; }
    public DbSet<Parcela> Parcelas { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 🔗 Tabela Titulo
        modelBuilder.Entity<Titulo>(entity =>
        {
            entity.ToTable("Titulos");

            entity.HasKey(t => t.Id);

            entity.Property(t => t.Numero).IsRequired();
            entity.Property(t => t.NomeDevedor).IsRequired();
            entity.Property(t => t.CPF).IsRequired();
            entity.Property(t => t.JurosMensal).HasColumnType("decimal(10,2)");
            entity.Property(t => t.MultaPercentual).HasColumnType("decimal(10,2)");

            entity.HasMany(t => t.Parcelas)
                  .WithOne(p => p.Titulo)
                  .HasForeignKey(p => p.TituloId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 🔗 Tabela Parcela
        modelBuilder.Entity<Parcela>(entity =>
        {
            entity.ToTable("Parcelas");

            entity.HasKey(p => p.Id); // ← chave primária simples

            entity.Property(p => p.TituloId).IsRequired();
            entity.Property(p => p.Valor).HasColumnType("decimal(10,2)");
            entity.Property(p => p.Vencimento).HasColumnType("timestamp without time zone");
            entity.Property(p => p.NumeroParcela).IsRequired();
        });
    }
}