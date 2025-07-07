using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Domain.Entities;
using Domain.Interfaces;
using Domain.DTOs;
using Infrastructure.Persistence;

namespace Infrastructure.Repositories;

public class TituloRepository : ITituloRepository
{
    private readonly AppDbContext _context;

    public TituloRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(
        string numero,
        string nomeDevedor,
        string cpf,
        decimal jurosMensal,
        decimal multaPercentual,
        IEnumerable<ParcelaInput> parcelasInput)
    {
        // 🔍 Verifica se já existe um título com o mesmo número
        var existente = await _context.Titulos
            .Include(t => t.Parcelas)
            .FirstOrDefaultAsync(t => t.Numero == numero);

        if (existente != null)
        {
            _context.Titulos.Remove(existente);
            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();
        }

        // 🔨 Cria o novo título com ID único
        var novoTitulo = new Titulo(
            numero,
            nomeDevedor,
            cpf,
            jurosMensal,
            multaPercentual,
            new List<Domain.Entities.Parcela>() // ← será preenchido abaixo
        );

        // 🔗 Cria as parcelas associadas ao novo título
        var parcelas = parcelasInput.Select(p => new Domain.Entities.Parcela(
            novoTitulo.Id,
            p.Valor,
            DateTime.SpecifyKind(p.Vencimento, DateTimeKind.Unspecified),
            p.NumeroParcela
        )).ToList();

        // 🔗 Atribui as parcelas ao título
        novoTitulo = new Titulo(
            numero,
            nomeDevedor,
            cpf,
            jurosMensal,
            multaPercentual,
            parcelas
        );

        await _context.Titulos.AddAsync(novoTitulo);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Titulo>> GetAllAsync()
    {
        return await _context.Titulos
            .AsNoTracking()
            .Include(t => t.Parcelas)
            .ToListAsync();
    }

    public async Task<decimal?> ObterValorAtualizadoAsync(string numero)
    {
        var titulo = await _context.Titulos
            .Include(t => t.Parcelas)
            .FirstOrDefaultAsync(t => t.Numero == numero);

        if (titulo == null) return null;

        return titulo.CalcularValorAtualizado(DateTime.Today);
    }

    public async Task<List<Domain.Entities.Parcela>> ObterParcelasPorTituloAsync(string numero)
    {
        var titulo = await _context.Titulos
            .Include(t => t.Parcelas)
            .FirstOrDefaultAsync(t => t.Numero == numero);

        return titulo?.Parcelas ?? new List<Domain.Entities.Parcela>();
    }

 public async Task<List<TituloComParcelasDto>> ListarComParcelasAsync(string? cpf, string? numeroTitulo, int page, int pageSize)
    {
        var query = _context.Titulos
            .Include(t => t.Parcelas)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(cpf))
            query = query.Where(t => t.CPF == cpf);

        if (!string.IsNullOrWhiteSpace(numeroTitulo))
            query = query.Where(t => t.Numero == numeroTitulo);

        return await query
            .OrderBy(t => t.Numero)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TituloComParcelasDto
            {
                Numero = t.Numero,
                NomeDevedor = t.NomeDevedor,
                CPF = t.CPF,
                JurosMensal = t.JurosMensal,
                MultaPercentual = t.MultaPercentual,
                Parcelas = t.Parcelas.Select(p => new ParcelaDto
                {
                    NumeroParcela = p.NumeroParcela,
                    Valor = p.Valor,
                    Vencimento = p.Vencimento
                }).ToList()
            })
            .ToListAsync();
    }
}