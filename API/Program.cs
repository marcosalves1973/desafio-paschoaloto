using Application.Commands;
using Application.Interfaces;
using Application.Services;
using Domain.Services;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;
using Infrastructure.Repositories;


var builder = WebApplication.CreateBuilder(args);


// 1. Registra o DbContext apontando para seu banco de dados
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. Registra a camada Application e o serviço de domínio
builder.Services.AddScoped<ITituloService, TituloService>();
builder.Services.AddScoped<TituloFinanceiroService>();
builder.Services.AddScoped<ITituloRepository, TituloRepository>();

// 4. Adiciona e configura o Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// 5. Define seus endpoints minimal APIs
app.MapPost("/titulos", async (ITituloService svc, IncluirTituloCommand cmd) =>
{
    await svc.IncluirAsync(cmd);
    return Results.Created($"/titulos/{cmd.Numero}", null);
});

app.MapGet("/titulos", async (ITituloService svc) =>
{
    return Results.Ok(await svc.ListarAsync());
});

app.MapGet("/titulos/{numero}/valor-atualizado", async (
    string numero,
    ITituloRepository repo) =>
{
    var valor = await repo.ObterValorAtualizadoAsync(numero);
    return valor is null
        ? Results.NotFound("Título não encontrado.")
        : Results.Ok(new { numero, valorAtualizado = valor });
});

app.MapGet("/titulos/{numero}/parcelas", async (
    string numero,
    ITituloRepository repo) =>
{
    var parcelas = await repo.ObterParcelasPorTituloAsync(numero);
    return parcelas.Any()
        ? Results.Ok(parcelas)
        : Results.NotFound("Título não encontrado ou sem parcelas.");
});

app.Run();

