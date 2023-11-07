namespace API.Helpers
{
    public enum MessageContainer { Unread, Inbox, Outbox }

    public class MessageParams : PaginationParams
    {
        public string UserName { get; set; }
        public MessageContainer MessageContainer { get; set; } = MessageContainer.Unread;
    }
}
