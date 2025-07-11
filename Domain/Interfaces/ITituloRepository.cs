using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.DTOs;

namespace Domain.Interfaces;

public interface ITituloRepository
{
    Task AddAsync(
        string numero,
        string nomeDevedor,
        string cpf,
        decimal jurosMensal,
        decimal multaPercentual,
        IEnumerable<ParcelaInput> parcelas
    );

    Task<List<Titulo>> GetAllAsync();

    Task<decimal?> ObterValorAtualizadoAsync(string numero);
    

    Task<List<TituloComParcelasDto>> ListarComParcelasAsync(string? cpf, string? numero, int page, int pageSize);

    // ✅ Corrigido para retornar DTO
    //Task<List<ParcelaDto>> ObterParcelasPorTituloAsync(string numero);
}