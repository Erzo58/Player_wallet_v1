namespace Player_wallet_v1.Model
{
    public record TransactionResponseDto(
           Guid TransactionId, 
           bool IsAccepted, 
           decimal CurrentBalance, 
           string Message);
}
