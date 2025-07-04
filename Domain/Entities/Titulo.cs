using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities;

public class Titulo
{
    public Guid Id { get; private set; } = Guid.NewGuid(); // 🔑 Chave primária
    public string Numero { get; private set; } = default!;
    public string NomeDevedor { get; private set; } = default!;
    public string CPF { get; private set; } = default!;
    public decimal JurosMensal { get; private set; }
    public decimal MultaPercentual { get; private set; }

    public List<Parcela> Parcelas { get; private set; } = new();

    // Construtor sem parâmetros exigido pelo EF Core
    protected Titulo() { }

    // Construtor principal
    public Titulo(
        string numero,
        string nomeDevedor,
        string cpf,
        decimal jurosMensal,
        decimal multaPercentual,
        List<Parcela> parcelas)
    {
        Id = Guid.NewGuid();
        Numero = numero;
        NomeDevedor = nomeDevedor;
        CPF = cpf;
        JurosMensal = jurosMensal;
        MultaPercentual = multaPercentual;
        Parcelas = parcelas ?? new List<Parcela>();

        foreach (var parcela in Parcelas)
        {
            parcela.TituloId = Id; // 🔗 Define a FK corretamente
        }
    }

    public decimal CalcularValorOriginal() =>
        Parcelas.Sum(p => p.Valor);

    public decimal CalcularValorAtualizado(DateTime hoje)
    {
        var valorOriginal = CalcularValorOriginal();
        decimal multa = valorOriginal * (MultaPercentual / 100);
        decimal juros = Parcelas.Sum(p => p.CalcularJuros(hoje, JurosMensal));
        return valorOriginal + multa + juros;
    }
}