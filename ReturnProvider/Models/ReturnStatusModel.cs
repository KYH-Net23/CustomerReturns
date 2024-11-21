namespace ReturnProvider.Models
{
    public class ReturnStatusModel
    {
        public string Status { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<string> StatusHistory { get; set; }
    }

}
