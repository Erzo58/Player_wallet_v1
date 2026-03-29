namespace Player_wallet_v1.Model
{
    public record TransactionRequestDto(
           Guid TransactionId,
           TransactionType Type, 
           decimal Amount);
}
