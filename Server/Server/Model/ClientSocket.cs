using System;
using System.Net.Sockets;
using System.Xml.Linq;

namespace Server.Model
{
    public class ClientSocket
    {
        
        public ClientSocket(Socket client)
        {
            Socket = client;
        }

        public String NickName { get; set; }

        public Socket Socket;

        public bool Connected
        {
            get { return Socket.Connected; }
        }
    }
}