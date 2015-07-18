using System.Net.Sockets;

namespace Server.Model
{
    public class ClientSocket
    {
        public ClientSocket(Socket client)
        {
            Socket = client;
        }



        public Socket Socket;

        public bool Connected
        {
            get { return Socket.Connected; }
        }
    }
}