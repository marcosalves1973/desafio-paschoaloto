namespace Domain.DTOs;
using System.ComponentModel.DataAnnotations;


public class ParcelaInput
{
    public int NumeroParcela { get; set; }
    public decimal Valor { get; set; }

    [Required] 
    public DateTime Vencimento { get; set; }
}
