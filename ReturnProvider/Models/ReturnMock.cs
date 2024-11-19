namespace ReturnProvider.Models
{
    public class ReturnMock
    {
        public int ReturnId { get; set; }
        public int OrderId { get; set; }
        public string Reason { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
