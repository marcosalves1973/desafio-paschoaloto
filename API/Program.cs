using Application.Commands;
using Application.Interfaces;
using Application.Services;
using Domain.Interfaces;
using Domain.Services;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔌 Configura o DbContext com PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 🧠 Injeta os serviços da aplicação e domínio
builder.Services.AddScoped<ITituloService, TituloService>();
builder.Services.AddScoped<TituloFinanceiroService>();
builder.Services.AddScoped<ITituloRepository, TituloRepository>();

// 📘 Configura Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🌐 Configura CORS para permitir chamadas do Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
        policy.WithOrigins("http://localhost:4200") // ajuste se necessário
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

var app = builder.Build();

// 🧪 Middleware do Swagger
app.UseSwagger();
app.UseSwaggerUI();

// 🌐 Aplica a política de CORS
app.UseCors("AllowAngular");

// 📌 Endpoints agrupados para organização
// 📌 Endpoints agrupados para organização
var titulos = app.MapGroup("/titulos");

titulos.MapPost("", async (ITituloService svc, IncluirTituloCommand cmd) =>
{
    await svc.IncluirAsync(cmd);
    return Results.Created($"/titulos/{cmd.Numero}", null);
});

titulos.MapGet("", async (ITituloService svc) =>
{
    return Results.Ok(await svc.ListarAsync());
});

titulos.MapGet("/com-parcelas", async (
    ITituloService svc,
    string? cpf,
    string? numeroTitulo,
    int page = 1,
    int pageSize = 10
) =>
{
    var resultado = await svc.ListarComParcelasAsync(cpf, numeroTitulo, page, pageSize);
    return Results.Ok(resultado);
});


titulos.MapGet("{numero}/valor-atualizado", async (string numero, ITituloRepository repo) =>
{
    var valor = await repo.ObterValorAtualizadoAsync(numero);
    return valor is null
        ? Results.NotFound("Título não encontrado.")
        : Results.Ok(new { numero, valorAtualizado = valor });
});

app.Run();