using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etherclue
{
    class Command
    {
        Encryption encryption = new Encryption();
        string commandEncrypted = "";
        string commandDecrypted = "";

        public Command(string cmd, bool send)
        {
            if (!send)
            {
                
                commandEncrypted = cmd;
                if(cmd != "")
                    commandDecrypted = encryption.decrypt(cmd.Split(';')[0], Program.cyphertext, cmd.Split(';')[1]);
            }
            else
            {
                string iv = encryption.GenerateRandomIV(16);
                commandDecrypted = cmd;
                if (cmd != "")
                    commandEncrypted = encryption.encrypt(cmd, Program.cyphertext, iv) + ";" + iv;
            }
        }

        public string GottenRequest()
        {
            return commandDecrypted;
        }

        public string SendRequest()
        {
            return commandEncrypted;
        }
    }
}