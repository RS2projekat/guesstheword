using System.Diagnostics.CodeAnalysis;

namespace Client.Model
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class Command
    {
        public static int
            REGISTER                                                            = 0, // Register new user
            LOGIN                                                               = 1, // Login into the server
            LOGOUT                                                              = 2, // Login of the server
            MESSAGE                                                             = 3, // Chat message
            SERVER_MESSAGE                                                      = 4, // Server message
            USER_DISCONNECT                                                     = 5, // Other user dissconected
            UNUSED1                                                             = 7,
            UNUSED2                                                             = 8,
            UNUSED3                                                             = 9,
            UNUSED4                                                             = 10,
            UNUSED5                                                             = 11,
            UNUSED6                                                             = 12,
            UNUSED7                                                             = 13,
            UNUSED8                                                             = 14,
            UNUSED9                                                             = 15,
            UNUSED10                                                            = 16,
            UNUSED11                                                            = 17,
            UNUSED12                                                            = 18,
            UNUSED13                                                            = 19,
            UNUSED14                                                            = 20,
            UNUSED15                                                            = 21,
            UNUSED16                                                            = 22,
            UNUSED17                                                            = 23,
            UNUSED18                                                            = 24,
            UNUSED19                                                            = 25;
    }
}
