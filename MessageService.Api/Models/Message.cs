namespace MessageService.Api.Models
{
    public class Message
    {
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public int SequenceNumber { get; set; }
    }
}
