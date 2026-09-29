namespace SkillLoop.Domain.Entities
{
    public class CreditPackage
    {
        public Guid Id { get; set; }
        public int Credits { get; set; }
        public decimal PriceEgp { get; set; }
        public bool IsActive { get; set; }

        public ICollection<PaymentOrder> PaymentOrders { get; set; } = new List<PaymentOrder>();
    }
}
