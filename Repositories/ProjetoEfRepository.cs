using GerenciadorAcademico.Data;
using GerenciadorAcademico.Models;
using Microsoft.EntityFrameworkCore;
namespace GerenciadorAcademico.Repositories;
public class ProjetoEfRepository : IProjetoRepository
{
    private readonly AppDbContext _db;
    public ProjetoEfRepository(AppDbContext db) => _db = db;
    public Task<List<Projeto>> ListarAsync() => _db.Projetos.AsNoTracking().Include(p => p.Professor).ToListAsync();
    public Task<Projeto?> ObterPorIdAsync(int id) => _db.Projetos.AsNoTracking().Include(p => p.Professor).FirstOrDefaultAsync(p => p.Id == id);
    public async Task AdicionarAsync(Projeto projeto)
    {
        projeto.Id = 0;
        projeto.Professor = null;
        _db.Projetos.Add(projeto);
        await _db.SaveChangesAsync();
    }
    public async Task AtualizarAsync(Projeto projeto)
    {
        var atual = await _db.Projetos.FirstOrDefaultAsync(p => p.Id == projeto.Id)
            ?? throw new KeyNotFoundException("Projeto não encontrado.");
        atual.Titulo = projeto.Titulo;
        atual.Descricao = projeto.Descricao;
        atual.CargaHoraria = projeto.CargaHoraria;
        atual.ProfessorId = projeto.ProfessorId;
        await _db.SaveChangesAsync();
    }
    public async Task<bool> RemoverAsync(int id)
    {
        var atual = await _db.Projetos.FindAsync(id);
        if (atual is null) return false;
        _db.Projetos.Remove(atual);
        await _db.SaveChangesAsync();
        return true;
    }
}
