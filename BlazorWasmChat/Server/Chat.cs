using BlazorWasmChat.Server.Authorization;
using BlazorWasmChat.Shared;

public class Chat
{
    // Данные приложения
    public static List<ChatRoom> Rooms { get; } = new List<ChatRoom>();
    public static List<ChatMessage> Messages { get; } = new List<ChatMessage>();
    public static List<UserLogin> Users { get; } = new List<UserLogin>();

    // Список поставщиков авторизации
    public static List<IChatAuthProvider> AuthProviders { get; } = new List<IChatAuthProvider>();

    static Chat()
    {
        // При запуске внести тестовые данные
        Rooms.Add(new ChatRoom { Id = 1, Name = "Общий чат" });
        Rooms.Add(new ChatRoom { Id = 2, Name = "Новости" });
        Rooms.Add(new ChatRoom { Id = 3, Name = "Политика" });
        Rooms.Add(new ChatRoom { Id = 4, Name = "Торренты" });

        Users.Add(new UserLogin { UserName = "admin", Password = "1234" });
    }

    // Локальная авторизация пользователя
    public static bool UserExists(UserLogin user)
    {
        return Chat.Users.Exists(x => x.UserName == user.UserName);
    }

    public static bool AddUser(UserLogin user)
    {
        Chat.Users.Add(new UserLogin { UserName = user.UserName, Password = user.Password });

        return true;
    }

    public static bool PasswordCorrect(UserLogin user)
    {
        return Chat.Users.Exists(x => x.UserName == user.UserName && x.Password == user.Password);
    }
}
