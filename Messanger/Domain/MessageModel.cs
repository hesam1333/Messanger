﻿namespace Messenger.Domain
{
    public class MessageModel
    {
        public string SenderId { get; set; }
        public DateTime CreateDateTime { get; set; }
        public string MessageBody { get; set; }
    }
}
