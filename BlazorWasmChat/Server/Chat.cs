using BlazorWasmChat.Shared;

public class Chat
{
    public static List<ChatRoom> Rooms { get; } = new List<ChatRoom>();

    public static List<ChatMessage> Messages { get; } = new List<ChatMessage>();

    static Chat()
    {
        Rooms.Add(new ChatRoom { Id = 1, Name = "Общий чат" });
        Rooms.Add(new ChatRoom { Id = 2, Name = "Новости" });
        Rooms.Add(new ChatRoom { Id = 3, Name = "Политика" });
        Rooms.Add(new ChatRoom { Id = 4, Name = "Торренты" });
    }
}
