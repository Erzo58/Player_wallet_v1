using Microsoft.AspNetCore.Http.HttpResults;

namespace Player_wallet_v1.Model.Dto
{
    public record TransactionRecordDto(
           Guid PlayerId,
           Guid TransactionId, 
           TransactionType Type, 
           decimal Amount,
           DateTime CreatedAt,
           bool isAccepted);
}
