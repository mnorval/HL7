using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace HL7
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendHL7("127.0.0.1",1234,"TESTING HL7");
        }

        //******HL7**************
        //******HL7**************
        //******HL7**************
        //******HL7**************
        private static bool SendHL7(string server, int port, string hl7message)
        {
            try
            {
                // Add the leading and trailing characters so it is LLP complaint.
                string llphl7message = Convert.ToChar(11).ToString() + hl7message + Convert.ToChar(28).ToString() + Convert.ToChar(13).ToString();

                // Get the size of the message that we have to send.
                Byte[] bytesSent = Encoding.ASCII.GetBytes(llphl7message);
                Byte[] bytesReceived = new Byte[256];

                // Create a socket connection with the specified server and port.
                Socket s = ConnectSocket(server, port);

                // If the socket could not get a connection, then return false.
                if (s == null)
                    return false;

                // Send message to the server.
                s.Send(bytesSent, bytesSent.Length, 0);

                // Receive the response back
                int bytes = 0;
                s.ReceiveTimeout = 3000;
                bytes = s.Receive(bytesReceived, bytesReceived.Length, 0);
                string page = Encoding.ASCII.GetString(bytesReceived, 0, bytes);
                s.Close();

                // Check to see if it was successful
                if (page.Contains("MSA|AA"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static Socket ConnectSocket(string server, int port)
        {
            Socket s = null;
            IPHostEntry hostEntry = null;

            // Get host related information.
            hostEntry = Dns.GetHostEntry(server);

            foreach (IPAddress address in hostEntry.AddressList)
            {
                IPEndPoint ipe = new IPEndPoint(address, port);
                Socket tempSocket = new Socket(ipe.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                tempSocket.Connect(ipe);

                if (tempSocket.Connected)
                {
                    s = tempSocket;
                    break;
                }
                else
                {
                    continue;
                }
            }
            return s;
        }

        //******HL7**************
        //******HL7**************
        //******HL7**************
        //******HL7**************

    }

}
