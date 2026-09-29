namespace SkillLoop.Domain.Entities
{
    public class Wallet
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int Balance { get; set; }
        public byte[] RowVersion { get; set; } = null!;

        public User User { get; set; } = null!;
        public ICollection<WalletTransaction> Transactions { get; set; } = new List<WalletTransaction>();
    }
}
