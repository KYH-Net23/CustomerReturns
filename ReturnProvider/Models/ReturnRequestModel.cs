namespace ReturnProvider.Models
{
    public class ReturnRequestModel
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public string ReturnReason { get; set; }
        public string ResolutionType { get; set; }
    }

}
