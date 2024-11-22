using System;

namespace ReturnProvider.Models.Entities
{
    public class ReturnEntity
    {
        public Guid ReturnId { get; set; }
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public string ReturnReason { get; set; }
        public string ResolutionType { get; set; }
        public string Status { get; set; } 
        public DateTime CreatedAt { get; set; }
    }
}
