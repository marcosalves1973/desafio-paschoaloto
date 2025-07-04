using System;

namespace Application.Commands;

public class ParcelaCommand
{
    public int NumeroParcela { get; set; }
    public DateTime Vencimento { get; set; }
    public decimal Valor { get; set; }
}
