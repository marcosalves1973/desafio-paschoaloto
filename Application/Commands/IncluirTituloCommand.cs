using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Application.Commands;

public class IncluirTituloCommand
{
    public string Numero { get; init; } = null!;
    public string NomeDevedor { get; init; } = null!;

    [JsonPropertyName("cpf")]
    public string Cpf { get; init; } = null!;

    public decimal JurosMensal { get; init; }
    public decimal MultaPercentual { get; init; }
    public List<ParcelaCommand> Parcelas { get; init; } = new();
}