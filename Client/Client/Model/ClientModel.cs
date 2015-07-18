using System.Net.Sockets;
using Common;

namespace Client.Model
{
    public class ClientModel : ModelBase
    {
        public string UserName { get; set; }
        //todo encryption for pw
        public string Password { get; set; }

        public void Login()
        {
            
        }

        private Packet PrepareLoginPacket()
        {
            Packet core = Packet.Instance;
            core.AddCommand(Command.LOGIN);
            core.AddData("username", UserName);
            core.AddData("password", Password);
            return core;
        }

    }
}
