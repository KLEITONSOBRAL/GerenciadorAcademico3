using GerenciadorAcademico.Models;
using GerenciadorAcademico.Services;
using GerenciadorAcademico.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace GerenciadorAcademico.Controllers;
public class ProjetosController : Controller
{
    private readonly IProjetoService _servico;
    public ProjetosController(IProjetoService servico) => _servico = servico;

    public async Task<IActionResult> Index(string? pesquisa, string? ordem)
    {
        return View(new ProjetoListaViewModel
        {
            Projetos = await _servico.ListarAsync(pesquisa, ordem), Pesquisa = pesquisa, Ordem = ordem
        });
    }
    [HttpGet]
    public async Task<IActionResult> Detalhes(int id)
    {
        var projeto = await _servico.ObterPorIdAsync(id);
        if (projeto is null) return NotFound();
        return View(projeto);
    }

    public Task<IActionResult> Pesquisar(string? pesquisa) => Index(pesquisa, null);
    public Task<IActionResult> Ordenar(string? ordem) => Index(null, ordem);

    private async Task PreencherProfessoresAsync(ProjetoFormViewModel modelo)
    {
        modelo.Professores = (await _servico.ListarProfessoresAsync())
            .OrderBy(p => p.Nome).Select(p => new SelectListItem(p.Nome, p.Id.ToString())).ToList();
    }
    [HttpGet] public async Task<IActionResult> Cadastrar()
    {
        var modelo = new ProjetoFormViewModel();
        await PreencherProfessoresAsync(modelo);
        return View(modelo);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Cadastrar(ProjetoFormViewModel modelo)
    {
        if (ModelState.IsValid)
        {
            try
            {
                await _servico.CadastrarAsync(ParaProjeto(modelo));
                TempData["Mensagem"] = "Projeto cadastrado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException erro) { ModelState.AddModelError(nameof(modelo.ProfessorId), erro.Message); }
        }
        await PreencherProfessoresAsync(modelo);
        return View(modelo);
    }
    [HttpGet] public async Task<IActionResult> Editar(int id)
    {
        var projeto = await _servico.ObterPorIdAsync(id);
        if (projeto is null) return NotFound();
        var modelo = new ProjetoFormViewModel { Id = projeto.Id, Titulo = projeto.Titulo,
            Descricao = projeto.Descricao, CargaHoraria = projeto.CargaHoraria, ProfessorId = projeto.ProfessorId };
        await PreencherProfessoresAsync(modelo);
        return View(modelo);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(int id, ProjetoFormViewModel modelo)
    {
        if (id != modelo.Id) return BadRequest();
        if (ModelState.IsValid)
        {
            try
            {
                await _servico.EditarAsync(ParaProjeto(modelo));
                TempData["Mensagem"] = "Projeto atualizado com sucesso.";
                return RedirectToAction(nameof(Index));
            }
            catch (ArgumentException erro) { ModelState.AddModelError(nameof(modelo.ProfessorId), erro.Message); }
            catch (KeyNotFoundException) { return NotFound(); }
        }
        await PreencherProfessoresAsync(modelo);
        return View(modelo);
    }
    [HttpGet] public async Task<IActionResult> Excluir(int id)
    {
        var projeto = await _servico.ObterPorIdAsync(id);
        if (projeto is null) return NotFound();
        return View(projeto);
    }
    [HttpPost, ActionName("Excluir"), ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmarExclusao(int id)
    {
        if (!await _servico.ExcluirAsync(id)) return NotFound();
        TempData["Mensagem"] = "Projeto excluído com sucesso.";
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Erro() => View();
    private static Projeto ParaProjeto(ProjetoFormViewModel m) => new()
    { Id = m.Id, Titulo = m.Titulo.Trim(), Descricao = m.Descricao.Trim(),
      CargaHoraria = m.CargaHoraria, ProfessorId = m.ProfessorId };
}
