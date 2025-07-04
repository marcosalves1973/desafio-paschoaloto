using Domain.Entities;
using Domain.Services;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Application.Tests.Domain.Services;

public class TituloFinanceiroServiceTests
{
    private readonly TituloFinanceiroService _service = new();

    [Fact]
    public void CalcularValorAtualizado_DeveRetornarValorCorretoComMultaEJuros()
    {
        // Arrange
        var titulo = new Titulo
        {
            JurosMensal = 0.01m,
            MultaPercentual = 0.02m,
            Parcelas = new List<Parcela>
            {
                new Parcela { Numero = 1, Valor = 100m, Vencimento = DateTime.Today.AddDays(-73) },
                new Parcela { Numero = 2, Valor = 100m, Vencimento = DateTime.Today.AddDays(-42) },
                new Parcela { Numero = 3, Valor = 100m, Vencimento = DateTime.Today.AddDays(-11) }
            }
        };

        var hoje = DateTime.Today;

        // Act
        var resultado = _service.CalcularValorAtualizado(titulo, hoje);

        // Assert
        resultado.Should().BeApproximately(310.20m, 0.01m);
    }
    [Fact]
    public void CalcularDiasEmAtraso_DeveSomarApenasDiasVencidos()
    {
        // Arrange
        var titulo = new Titulo
        {
            Parcelas = new List<Parcela>
            {
                new Parcela { Numero = 1, Valor = 100m, Vencimento = DateTime.Today.AddDays(-10) },
                new Parcela { Numero = 2, Valor = 100m, Vencimento = DateTime.Today.AddDays(-5) },
                new Parcela { Numero = 3, Valor = 100m, Vencimento = DateTime.Today.AddDays(3) } // ainda não venceu
            }
        };

        var hoje = DateTime.Today;

        // Act
        var totalDias = _service.CalcularDiasEmAtraso(titulo, hoje);

        // Assert
        totalDias.Should().Be(15); // 10 + 5 dias
    }
    [Fact]
    public void CalcularDiasEmAtraso_DeveRetornarZeroQuandoNaoHaAtrasos()
    {
        // Arrange
        var titulo = new Titulo
        {
            Parcelas = new List<Parcela>
            {
                new Parcela { Numero = 1, Valor = 100m, Vencimento = DateTime.Today.AddDays(5) },
                new Parcela { Numero = 2, Valor = 100m, Vencimento = DateTime.Today.AddDays(10) }
            }
        };

        var hoje = DateTime.Today;

        // Act
        var totalDias = _service.CalcularDiasEmAtraso(titulo, hoje);

        // Assert
        totalDias.Should().Be(0);
    }
}