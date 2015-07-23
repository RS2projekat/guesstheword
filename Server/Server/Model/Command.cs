using System.Diagnostics.CodeAnalysis;

namespace Server.Model
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class Command
    {
        public const int
            REGISTER = 0, // Register new user
            LOGIN = 1, // Login into the server
            LOGOUT_REQUEST = 2, // Login of the server
            MESSAGE = 3, // Chat message
            SERVER_MESSAGE = 4, // Server message
            USER_DISCONNECT = 5, // Other user dissconected
            USER_NICKNAME_IN_USE = 6, // there is already user with that nickname
            UNUSED2 = 7,
            REFRESH_USER_LIST = 8, // indicates that  client needs to refresh user list
            SEND_COORDINATES = 9,
            CANVAS_UNDO = 10,
            CANVAS_CLEAR = 11,
            UNUSED6 = 12,
            UNUSED7 = 13,
            UNUSED8 = 14,
            UNUSED9 = 15,
            UNUSED10 = 16,
            UNUSED11 = 17,
            UNUSED12 = 18,
            UNUSED13 = 19,
            UNUSED14 = 20,
            UNUSED15 = 21,
            UNUSED16 = 22,
            UNUSED17 = 23,
            UNUSED18 = 24,
            UNUSED19 = 25;
    }
}
