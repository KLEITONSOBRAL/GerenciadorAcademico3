using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace GerenciadorAcademico.ViewModels;
public class ProjetoFormViewModel
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Informe o título.")]
    [StringLength(120)] public string Titulo { get; set; } = string.Empty;
    [Required(ErrorMessage = "Informe a descrição.")]
    [StringLength(1000)] public string Descricao { get; set; } = string.Empty;
    [Range(1, 10000, ErrorMessage = "Informe uma carga horária válida.")]
    [Display(Name = "Carga horária")] public int CargaHoraria { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Selecione um professor.")]
    [Display(Name = "Professor")] public int ProfessorId { get; set; }
    public List<SelectListItem> Professores { get; set; } = new();
}
