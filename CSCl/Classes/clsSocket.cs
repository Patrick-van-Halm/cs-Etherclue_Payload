using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace Etherclue
{
    class clsSocket
    {
        const string EOL = "\r\n\r\n";
        private string host;
        private int port;
        private Socket socket;

        public clsSocket(string host, int port)
        {
            this.host = host;
            this.port = port;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Connect();
        }

        private IPAddress resolve()
        {
            IPAddress address = null;
            if (IPAddress.TryParse(host, out address))
            {
                return address;
            }
            else
            {
                return Dns.GetHostEntry(host).AddressList[0];
            }
        }

        private void Connect()
        {
            while (!socket.Connected)
            {
                try
                {
                    socket.Connect(resolve(), port);
                }
                catch (Exception)
                {
                    Thread.Sleep(5000);
                }
            }
        }
        
        public void Close()
        {
            if(IsConnected())
                socket.Disconnect(false);
        }

        public void Send(string msg)
        {
            byte[] message = Encoding.Default.GetBytes(msg + EOL);
            socket.Send(message, message.Length, SocketFlags.None);
            Thread.Sleep(200);
        }

        public string Receive()
        {
            Thread.Sleep(200);
            if(socket.Available > 0)
            {
                byte[] buffer = new byte[4096];
                //socket.Receive(buffer, 0, socket.Available, SocketFlags.Peek);
                while (buffer[0] == 0)
                {
                    socket.Receive(buffer);
                }
                string received = Encoding.Default.GetString(buffer, 0, buffer.Length).Replace("\0", "").Replace(EOL, "");
                return received;
            }
            return Receive();
        }

        public bool IsConnected()
        {
            return socket.Connected;
        }
    }
}
