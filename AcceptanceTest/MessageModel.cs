namespace AcceptanceTest
{
    enum MessageType
    {
        Message = 100,
        Command = 300,
        Error = 200,
    }
    public class MessageModel
    {
        public string SenderId { get; set; }
        public string ResiverId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string MessageBody { get; set; }
    }
}
