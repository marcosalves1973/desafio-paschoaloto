namespace Application.DTOs;

public class TituloResumoDto
{
    public string Numero { get; set; } = null!;
    public string NomeDevedor { get; set; } = null!;
    public decimal ValorOriginal { get; set; }
    public decimal ValorAtualizado { get; set; }
}