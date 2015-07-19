using System.Diagnostics.CodeAnalysis;

namespace Client.Model
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class Command
    {
        public const int
            REGISTER                                                            = 0, // Register new user
            LOGIN                                                               = 1, // Login into the server
            LOGOUT                                                              = 2, // Login of the server
            MESSAGE                                                             = 3, // Chat message
            SERVER_MESSAGE                                                      = 4, // Server message
            USER_DISCONNECT                                                     = 5, // Other user dissconected
            USER_NICKNAME_IN_USE                                                = 6, // there is already user with that nickname
            USER_SUCCESSFULL_LOGIN                                              = 7, // when user logs in successfull
            NEW_USER                                                            = 8, // indicates that new user logged in, so client could refresh user list
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
