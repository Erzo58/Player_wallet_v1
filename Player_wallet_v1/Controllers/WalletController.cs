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
        /// <summary>
        /// register wallet for new player, to add a new player change the guid ID
        /// </summary>
        [HttpPost("walletAdd")]
        public async Task<IActionResult> RegisterPlayer([FromBody] RegisterPlayerRequest request)
        {
            var success = await _walletService.RegisterPlayerWalletAsync(request.PlayerId);
            if (!success)
                return Conflict(new { Message = "Player's wallet is already registered." }); 

            return CreatedAtAction(nameof(GetBalance),new { playerId = request.PlayerId }, new { playerId = request.PlayerId, walletBalance = 0 });
        }

        // get player balance
        /// <summary>
        /// get player's balance
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        [HttpGet("{playerId:guid}/balance")]
        public async Task<IActionResult> GetBalance(Guid playerId)
        {
            var balance = await _walletService.GetBalanceAsync(playerId);
            return Ok(new { PlayerId = playerId, Balance = balance });
        }

        // credit transaction
        /// <summary>
        /// credit transaction to player's wallet, to create a new transaction change the guid ID
        /// </summary>
        /// <param name="playerId"></param>
        /// <param name="request"></param>
        /// <returns></returns>

        [HttpPost("{playerId:guid}/credittransactions")]
        public async Task<IActionResult> CreditTransaction(Guid playerId, [FromBody] TransactionRequestDto request)
        {
            var result = await _walletService.ProcessTransactionAsync(playerId, request);

            if (!result.IsAccepted)
                return UnprocessableEntity(result);

            return Ok(result); 
        }

        // get transactions
        /// <summary>
        /// get list of saved transactions
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
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
