using GerenciadorAcademico.Data;
using GerenciadorAcademico.Repositories;
using GerenciadorAcademico.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var provider = builder.Configuration["Persistence:Provider"] ?? "Sqlite";
builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (provider.Equals("SqlServer", StringComparison.OrdinalIgnoreCase))
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
    else if (provider.Equals("Sqlite", StringComparison.OrdinalIgnoreCase))
        options.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
    else
        throw new InvalidOperationException("Provedor inválido: escolha Sqlite ou SqlServer.");
});
builder.Services.AddScoped<IProjetoRepository, ProjetoEfRepository>();
builder.Services.AddScoped<IProfessorRepository, ProfessorEfRepository>();
builder.Services.AddScoped<IProjetoService, ProjetoService>();
var app = builder.Build();
await using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await context.Database.EnsureCreatedAsync();
    await DadosIniciais.PreencherAsync(context, app.Environment.ContentRootPath);
}
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Projetos/Erro");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute("default", "{controller=Projetos}/{action=Index}/{id?}");
app.Run();
