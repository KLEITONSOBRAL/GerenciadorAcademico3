using GerenciadorAcademico.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorAcademico.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Projeto> Projetos => Set<Projeto>();
    public DbSet<Professor> Professores => Set<Professor>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Professor>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Nome).HasMaxLength(120).IsRequired();
            entity.Property(p => p.Email).HasMaxLength(180).IsRequired();
            entity.HasIndex(p => p.Email).IsUnique();
        });
        modelBuilder.Entity<Projeto>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Titulo).HasMaxLength(180).IsRequired();
            entity.Property(p => p.Descricao).HasMaxLength(2000).IsRequired();
            entity.HasOne(p => p.Professor).WithMany().HasForeignKey(p => p.ProfessorId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
