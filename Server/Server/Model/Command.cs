using System.Diagnostics.CodeAnalysis;

namespace Server.Model
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class Command
    {
        public static int
            LOGIN = 0, // Login into the server
            LOGOUT = 1, // Login of the server
            MESSAGE = 2, // Chat message
            SERVER_MESSAGE = 3; // Server messageaaaaaaaaaaaa
    }
}
