namespace ReturnProvider.Models
{
    public class ReturnModel
    {
        public Guid ReturnId { get; set; }
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public string ReturnReason { get; set; }
        public string ResolutionType { get; set; } // Refund or Exchange
        public string Status { get; set; } // e.g., Requested, In Transit, Refunded
        public DateTime CreatedAt { get; set; }
    }
}
