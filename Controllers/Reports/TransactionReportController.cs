using MicroFinance.Dtos;
using MicroFinance.Models.Wrapper.Reports;
using MicroFinance.Services.Reports;
using MicroFinance.Token;
using Microsoft.AspNetCore.Mvc;

namespace MicroFinance.Controllers.Reports;

public class TransactionReportController : BaseApiController
{
    private readonly ITransactionReportService _transactionReportService;
    private readonly ITokenService _tokenService;

    public TransactionReportController(ITransactionReportService transactionReportService, ITokenService tokenService)
    {
        _transactionReportService = transactionReportService;
        _tokenService = tokenService;
    }

    private TokenDto GetDecodedToken()
    {
        string token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var decodedToken = _tokenService.DecodeJWT(token);
        return decodedToken;
    }

    [HttpGet("depositAccount")]
    public async Task<ActionResult<DepositAccountTransactionReportWrapper>> GetDepositAccountTransactionReport([FromQuery] string fromDate, [FromQuery] string toDate, [FromQuery] int depositAccountId)
    {
        var decodedToken = GetDecodedToken();
        return Ok(await _transactionReportService.GetDepositAccountTransactionReportService(fromDate, toDate, depositAccountId, decodedToken));
    }
}