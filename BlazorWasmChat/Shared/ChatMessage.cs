namespace BlazorWasmChat.Shared
{
    public class ChatMessage
    {
        public DateTime Issued { get; set; } = DateTime.Now;

        public string? FromUser { get; set; }

        public string? RoomId { get; set; }

        public string? Text { get; set; }

        public override string ToString()
        {
            return $"{FromUser}: {Text}";
        }
    }
}
