namespace WPFCommon
{
    public interface IMessageService
    {
        MessageInfo Get(string msgID, string langCD, params string[] addStrings);

        public class MessageInfo
        {
            public string MessageString { get; set; }
            public string MessageType { get; set; }
        }
    }
}