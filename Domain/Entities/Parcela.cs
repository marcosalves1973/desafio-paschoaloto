using System;

namespace Domain.Entities;

public class Parcela
{
    public Guid Id { get; private set; } = Guid.NewGuid(); // 🔑 Chave primária
    public Guid TituloId { get; set; }                     // 🔗 Chave estrangeira

    public decimal Valor { get; private set; }
    public DateTime Vencimento { get; private set; }
    public int NumeroParcela { get; private set; }

    public Titulo Titulo { get; set; } = null!;            // Navegação

    // Construtor sem parâmetros exigido pelo EF Core
    protected Parcela() { }

    // Construtor principal
    public Parcela(Guid tituloId, decimal valor, DateTime vencimento, int numeroParcela)
    {
        Id = Guid.NewGuid();
        TituloId = tituloId;
        Valor = valor;
        Vencimento = vencimento;
        NumeroParcela = numeroParcela;
    }

    public decimal CalcularJuros(DateTime hoje, decimal jurosMensal)
    {
        var diasAtraso = (hoje - Vencimento).Days;
        if (diasAtraso <= 0) return 0;
        return (jurosMensal / 30m / 100) * diasAtraso * Valor;
    }
}