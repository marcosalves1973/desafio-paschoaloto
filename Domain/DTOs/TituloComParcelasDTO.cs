public class ParcelaDto
{
    public int NumeroParcela { get; set; }
    public decimal Valor { get; set; }
    public DateTime Vencimento { get; set; }
}

public class TituloComParcelasDto
{
    public string Numero { get; set; }
    public string NomeDevedor { get; set; }
    public string CPF { get; set; }
    public decimal JurosMensal { get; set; }
    public decimal MultaPercentual { get; set; }
    public List<ParcelaDto> Parcelas { get; set; }
}