using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;

namespace Domain.Services;

public class TituloFinanceiroService
{
    public decimal CalcularTotalJuros(IEnumerable<Parcela> parcelas, DateTime hoje, decimal jurosMensal)
    {
        return parcelas.Sum(p => p.CalcularJuros(hoje, jurosMensal));
    }

    public decimal CalcularValorOriginal(IEnumerable<Parcela> parcelas)
    {
        return parcelas.Sum(p => p.Valor);
    }
}