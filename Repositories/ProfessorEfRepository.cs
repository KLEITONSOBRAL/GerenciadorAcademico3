using GerenciadorAcademico.Data;
using GerenciadorAcademico.Models;
using Microsoft.EntityFrameworkCore;
namespace GerenciadorAcademico.Repositories;
public class ProfessorEfRepository : IProfessorRepository
{
    private readonly AppDbContext _db;
    public ProfessorEfRepository(AppDbContext db) => _db = db;
    public Task<List<Professor>> ListarAsync() => _db.Professores.AsNoTracking().OrderBy(p => p.Nome).ToListAsync();
    public Task<Professor?> ObterPorIdAsync(int id) => _db.Professores.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
}
