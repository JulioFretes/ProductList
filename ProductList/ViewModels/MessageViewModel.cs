using Newtonsoft.Json;

namespace ProductList.ViewModels
{
    public enum MessageType
    {
        Information,
        Error
    }

    public class MessageViewModel
    {
        public MessageType Type { get; set; }
        public string Text { get; set; }
        public MessageViewModel(string message, MessageType type = MessageType.Information)
        {
            Type = type;
            Text = message;
        }

        public static string Serialize(string message, MessageType type = MessageType.Information)
        {
            var mensagemModel = new MessageViewModel(message, type);
            return JsonConvert.SerializeObject(mensagemModel);
        }

        public static MessageViewModel Deserialize(string mensagemString)
        {
            return JsonConvert.DeserializeObject<MessageViewModel>(mensagemString);
        }
    }
}