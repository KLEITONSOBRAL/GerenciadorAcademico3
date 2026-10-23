using GerenciadorAcademico.Models;
using GerenciadorAcademico.Repositories;
namespace GerenciadorAcademico.Services;
public class ProjetoService : IProjetoService
{
    private readonly IProjetoRepository _projetos;
    private readonly IProfessorRepository _professores;
    public ProjetoService(IProjetoRepository projetos, IProfessorRepository professores)
    { _projetos = projetos; _professores = professores; }

    public async Task<List<Projeto>> ListarAsync(string? pesquisa = null, string? ordem = null)
    {
        var projetos = await _projetos.ListarAsync();
        var professores = (await _professores.ListarAsync()).ToDictionary(p => p.Id);
        foreach (var projeto in projetos)
            projeto.Professor = professores.GetValueOrDefault(projeto.ProfessorId);
        if (!string.IsNullOrWhiteSpace(pesquisa))
            projetos = projetos.Where(p => p.Titulo.Contains(pesquisa, StringComparison.OrdinalIgnoreCase)).ToList();
        return ordem == "cargahoraria"
            ? projetos.OrderBy(p => p.CargaHoraria).ThenBy(p => p.Titulo).ToList()
            : projetos.OrderBy(p => p.Titulo).ToList();
    }
    public async Task<Projeto?> ObterPorIdAsync(int id)
    {
        var projeto = await _projetos.ObterPorIdAsync(id);
        if (projeto != null) projeto.Professor = await _professores.ObterPorIdAsync(projeto.ProfessorId);
        return projeto;
    }
    public Task<List<Professor>> ListarProfessoresAsync() => _professores.ListarAsync();
    private async Task ValidarProfessorAsync(int id)
    {
        if (await _professores.ObterPorIdAsync(id) is null)
            throw new ArgumentException("Selecione um professor válido.");
    }
    public async Task CadastrarAsync(Projeto projeto)
    {
        await ValidarProfessorAsync(projeto.ProfessorId);
        await _projetos.AdicionarAsync(projeto);
    }
    public async Task EditarAsync(Projeto projeto)
    {
        await ValidarProfessorAsync(projeto.ProfessorId);
        await _projetos.AtualizarAsync(projeto);
    }
    public async Task<bool> ExcluirAsync(int id) => await _projetos.RemoverAsync(id);
}
