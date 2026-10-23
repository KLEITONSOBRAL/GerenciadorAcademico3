using GerenciadorAcademico.Models;
namespace GerenciadorAcademico.Services;
public interface IProjetoService
{
    Task<List<Projeto>> ListarAsync(string? pesquisa = null, string? ordem = null);
    Task<Projeto?> ObterPorIdAsync(int id);
    Task<List<Professor>> ListarProfessoresAsync();
    Task CadastrarAsync(Projeto projeto);
    Task EditarAsync(Projeto projeto);
    Task<bool> ExcluirAsync(int id);
}
