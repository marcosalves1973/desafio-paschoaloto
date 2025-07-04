using Application.Services;
using Application.DTOs;
using Domain.Entities;
using Domain.Repositories;
using Moq;
using Xunit;

public class TituloServiceTests
{
    [Fact]
    public async Task IncluirAsync_DeveSalvarTituloComParcelas()
    {
        // Arrange
        var repoMock = new Mock<ITituloRepository>();
        var calculoMock = new Mock<Domain.Services.TituloFinanceiroService>();
        var service = new TituloService(repoMock.Object, calculoMock.Object);

        var comando = new IncluirTituloCommand
        {
            Numero = "101010",
            NomeDevedor = "Fulano",
            CPF = "12345678900",
            JurosMensal = 0.01m,
            MultaPercentual = 0.02m,
            Parcelas = new List<ParcelaDto>
            {
                new() { Numero = 1, Valor = 100m, Vencimento = DateTime.Today.AddDays(-10) },
                new() { Numero = 2, Valor = 200m, Vencimento = DateTime.Today.AddDays(-5) }
            }
        };

        // Act
        await service.IncluirAsync(comando);

        // Assert
        repoMock.Verify(r => r.AdicionarAsync(It.Is<Titulo>(t =>
            t.Numero == "101010" &&
            t.Parcelas.Count == 2 &&
            t.Parcelas[0].Valor == 100m
        )), Times.Once);
    }
    [Fact]
    public async Task ListarAsync_DeveRetornarResumoComValoresAtualizados()
    {
        // Arrange
        var hoje = DateTime.Today;
        var titulos = new List<Titulo>
        {
            new Titulo
            {
                Numero = "101010",
                NomeDevedor = "Fulano",
                JurosMensal = 0.01m,
                MultaPercentual = 0.02m,
                Parcelas = new List<Parcela>
                {
                    new() { Numero = 1, Valor = 100m, Vencimento = hoje.AddDays(-73) },
                    new() { Numero = 2, Valor = 100m, Vencimento = hoje.AddDays(-42) },
                    new() { Numero = 3, Valor = 100m, Vencimento = hoje.AddDays(-11) }
                }
            }
        };

        var repoMock = new Mock<ITituloRepository>();
        repoMock.Setup(r => r.ObterTodosAsync()).ReturnsAsync(titulos);

        var calculo = new TituloFinanceiroService();
        var service = new TituloService(repoMock.Object, calculo);

        // Act
        var resultado = await service.ListarAsync();

        // Assert
        var dto = resultado.Single();
        Assert.Equal("101010", dto.Numero);
        Assert.Equal(3, dto.QuantidadeParcelas);
        Assert.Equal(300m, dto.ValorOriginal);
        Assert.Equal(73 + 42 + 11, dto.DiasEmAtraso);
        Assert.Equal(310.20m, dto.ValorAtualizado, 2); // margem de arredondamento
    }

}