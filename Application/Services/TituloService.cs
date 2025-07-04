using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Application.Commands;
using Application.DTOs;
using Application.Interfaces;
using Domain.DTOs;
using Domain.Interfaces;
using Domain.Services;

namespace Application.Services;

public class TituloService : ITituloService
{
    private readonly ITituloRepository _repository;
    private readonly TituloFinanceiroService _financeiro;

    public TituloService(
        ITituloRepository repository,
        TituloFinanceiroService financeiro)
    {
        _repository = repository;
        _financeiro = financeiro;
    }

    public async Task IncluirAsync(IncluirTituloCommand comando)
    {
        var parcelas = comando.Parcelas.Select(p => new ParcelaInput
        {
            NumeroParcela = p.NumeroParcela,
            Valor = p.Valor,
            Vencimento = p.Vencimento
        }).ToList();

        await _repository.AddAsync(
            comando.Numero,
            comando.NomeDevedor,
            comando.CPF,
            comando.JurosMensal,
            comando.MultaPercentual,
            parcelas
        );
    }

    public async Task<List<TituloResumoDto>> ListarAsync()
    {
        var titulos = await _repository.GetAllAsync();
        return titulos
            .Select(t => new TituloResumoDto
            {
                Numero = t.Numero,
                NomeDevedor = t.NomeDevedor,
                ValorOriginal = t.CalcularValorOriginal(),
                ValorAtualizado = t.CalcularValorAtualizado(DateTime.Today)
            })
            .ToList();
    }
}