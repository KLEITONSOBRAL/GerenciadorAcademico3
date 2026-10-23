using System.Text.Json;
using GerenciadorAcademico.Models;
using Microsoft.EntityFrameworkCore;

namespace GerenciadorAcademico.Data;

public static class DadosIniciais
{
    // Os arquivos JSON servem apenas para popular um banco novo, uma única vez.
    public static async Task PreencherAsync(AppDbContext db, string contentRoot)
    {
        if (await db.Professores.AnyAsync() || await db.Projetos.AnyAsync()) return;
        var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var pasta = Path.Combine(contentRoot, "Data", "Exemplos");
        var professores = JsonSerializer.Deserialize<List<Professor>>(
            await File.ReadAllTextAsync(Path.Combine(pasta, "professores.json")), jsonOptions) ?? [];
        var projetos = JsonSerializer.Deserialize<List<Projeto>>(
            await File.ReadAllTextAsync(Path.Combine(pasta, "projetos.json")), jsonOptions) ?? [];
        var ids = professores.Select(p => p.Id).ToHashSet();
        if (projetos.Any(p => !ids.Contains(p.ProfessorId)))
            throw new InvalidDataException("Existem projetos com ProfessorId inválido nos dados iniciais.");
        await using var transacao = await db.Database.BeginTransactionAsync();
        db.Professores.AddRange(professores);
        await db.SaveChangesAsync();
        db.Projetos.AddRange(projetos);
        await db.SaveChangesAsync();
        await transacao.CommitAsync();
    }
}
