using GerenciadorAcademico.Models;
namespace GerenciadorAcademico.ViewModels;
public class ProjetoListaViewModel
{
    public List<Projeto> Projetos { get; set; } = new();
    public string? Pesquisa { get; set; }
    public string? Ordem { get; set; }
}
