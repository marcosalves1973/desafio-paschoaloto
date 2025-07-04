using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Commands;
using Application.DTOs;

namespace Application.Interfaces;

public interface ITituloService
{
    Task IncluirAsync(IncluirTituloCommand comando);
    Task<List<TituloResumoDto>> ListarAsync();
}