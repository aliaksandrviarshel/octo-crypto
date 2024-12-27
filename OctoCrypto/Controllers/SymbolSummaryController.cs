using Microsoft.AspNetCore.Mvc;
using OctoCrypto.Core.SymbolSummary;

namespace OctoCrypto.Controllers;

[ApiController]
[Route("[controller]")]
public class SymbolSummaryController : ControllerBase
{
    private readonly SymbolSummaryService _symbolSummaryService;

    public SymbolSummaryController(SymbolSummaryService symbolSummaryService)
    {
        _symbolSummaryService = symbolSummaryService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int? pageIndex, int? pageSize)
    {
        return Ok(await _symbolSummaryService.GetSymbolSummaries(pageIndex, pageSize));
    }
}