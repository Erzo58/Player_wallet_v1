using Microsoft.AspNetCore.Mvc;
using Player_wallet_v1.Model;
using Player_wallet_v1.Model.Dto;
using Player_wallet_v1.Services;

namespace Player_wallet_v1.Controllers
{
    [ApiController]
    [Route("api/players")]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        //
        // register  playet wallet
        [HttpPost]
        public async Task<IActionResult> RegisterPlayer([FromBody] RegisterPlayerRequest request)
        {
            var success = await _walletService.RegisterPlayerWalletAsync(request.PlayerId);
            if (!success)
                return Conflict(new { Message = "Player's wallet is already registered." }); 

            return CreatedAtAction(nameof(GetBalance), new { playerId = request.PlayerId }, null);
        }

        // get player balance
        [HttpGet("{playerId:guid}/balance")]
        public async Task<IActionResult> GetBalance(Guid playerId)
        {
            var balance = await _walletService.GetBalanceAsync(playerId);
            return Ok(new { PlayerId = playerId, Balance = balance });
        }

        // credit transaction
        [HttpPost("{playerId:guid}/transactions")]
        public async Task<IActionResult> CreditTransaction(Guid playerId, [FromBody] TransactionRequestDto request)
        {
            var result = await _walletService.ProcessTransactionAsync(playerId, request);

            if (!result.IsAccepted)
                return UnprocessableEntity(result);

            return Ok(result); 
        }

        // get transactions
        [HttpGet("{playerId:guid}/transactions")]
        public async Task<IActionResult> GetTransactions(Guid playerId)
        {
            var transactions = await _walletService.GetPlayerTransactionsAsync(playerId);
            if (transactions == null)
                return NotFound(new { Message = "Player not found." });

            return Ok(transactions); 
        }
    }
}
