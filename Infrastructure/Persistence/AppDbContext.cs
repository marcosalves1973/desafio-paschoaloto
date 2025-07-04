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

            entity.HasKey(t => t.Numero);

            entity.Property(t => t.Numero).IsRequired();
            entity.Property(t => t.NomeDevedor).IsRequired();
            entity.Property(t => t.CPF).IsRequired();
            entity.Property(t => t.JurosMensal).HasColumnType("decimal(10,2)");
            entity.Property(t => t.MultaPercentual).HasColumnType("decimal(10,2)");

            entity.HasMany(t => t.Parcelas)
                  .WithOne(p => p.Titulo)
                  .HasForeignKey(p => p.TituloNumero)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 🔗 Tabela Parcela
        modelBuilder.Entity<Parcela>(entity =>
        {
            entity.ToTable("Parcelas");

            // ✅ Chave primária composta corrigida
            entity.HasKey(p => new { p.TituloNumero, p.NumeroParcela });

            entity.Property(p => p.TituloNumero).IsRequired();
            entity.Property(p => p.Valor).HasColumnType("decimal(10,2)");
            entity.Property(p => p.Vencimento).IsRequired();
            entity.Property(p => p.NumeroParcela).IsRequired();
        });
    }
}

public class Parcela
{
    // Add the foreign key property for Titulo
    public int TituloNumero { get; set; }
}