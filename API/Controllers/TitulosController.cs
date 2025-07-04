using Application.Interfaces;
using Application.Commands;
using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/titulos")]
public class TitulosController : ControllerBase
{
    private readonly ITituloService _tituloService;

    public TitulosController(ITituloService tituloService)
    {
        _tituloService = tituloService;
    }

    [HttpPost]
    public async Task<IActionResult> Incluir([FromBody] IncluirTituloCommand comando)
    {
        await _tituloService.IncluirAsync(comando);
        return Ok(new { mensagem = "Título incluído com sucesso!" });
    }

    [HttpGet]
    public async Task<ActionResult<List<TituloResumoDto>>> Consultar()
    {
        var titulos = await _tituloService.ListarAsync();
        return Ok(titulos);
    }
}