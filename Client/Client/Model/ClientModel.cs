using System;
using System.ComponentModel;

namespace Client.Model
{
    public class ClientModel : ModelBase, IDataErrorInfo
    {
        public string NickName { get; set; }
        //todo encryption for pw
        public string Password { get; set; }

        public void Login()
        {
            Packet core = PrepareLoginPacket();
            Connection.Send(core);
        }

        private Packet PrepareLoginPacket()
        {
            Packet core = Packet.Instance;
            core.AddCommand(Command.LOGIN);
            core.AddData("nickname", NickName);
            //core.AddData("password", Password);
            return core;
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "NickName")
                {
                    if (string.IsNullOrWhiteSpace(NickName))
                        return "NickName is required";
                }

                return null;
            }
        }

        public string Error { get { return null; } }
    }
}
