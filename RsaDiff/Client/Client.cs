using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using JLM.NetSocket;
using System.IO;
using System.Threading;

namespace TestNetClient
{
    public partial class Client : Form
    {
        private NetClient client = new NetClient();
        private delegate void Safe(string n);
        private Safe SafeCall;
        private Diffy_Hellmen diffy_ = new Diffy_Hellmen();
        public Client()
        {
            InitializeComponent();

            this.client.Connected += new EventHandler<NetSocketConnectedEventArgs>(client_Connected);
            this.client.DataArrived += new EventHandler<NetSockDataArrivalEventArgs>(Client_DataClaim);
            this.client.Disconnected += new EventHandler<NetSocketDisconnectedEventArgs>(client_Disconnected);
            this.client.ErrorReceived += new EventHandler<NetSockErrorReceivedEventArgs>(client_ErrorReceived);
            this.client.StateChanged += new EventHandler<NetSockStateChangedEventArgs>(client_StateChanged);

            this.SafeCall = new Safe(Log_Local);
        }

        private void Log(string n)
        {
            if (this.InvokeRequired)
                this.Invoke(this.SafeCall, n);
            else
                this.Log_Local(n);
        }

        private void Log_Local(string n)
        {
            this.lbLog.Items.Add(n);
        }

        private void client_StateChanged(object sender, NetSockStateChangedEventArgs e)
        {
            if (e.NewState == SocketState.Connecting)
            {
                diffy_.ConnectClient(lbLog, tbServerX.Text);
            }
        }

        private void client_ErrorReceived(object sender, NetSockErrorReceivedEventArgs e)
        {
            if (e.Exception.GetType() == typeof(System.Net.Sockets.SocketException))
            {
                System.Net.Sockets.SocketException s = (System.Net.Sockets.SocketException)e.Exception;
                this.Log("Error: " + e.Function + " - " + s.SocketErrorCode.ToString() + "\r\n" + s.ToString());
            }
            else
                this.Log("Error: " + e.Function + "\r\n" + e.Exception.ToString());
        }

        private void client_Disconnected(object sender, NetSocketDisconnectedEventArgs e)
        {
            this.Log("Disconnected: " + e.Reason);
        }
        public string msg = "";
        private void Client_DataClaim(object sender, NetSockDataArrivalEventArgs e)
        {
            msg = Encoding.ASCII.GetString(e.Data);
            this.Log("Recieved: " + msg + " (" + e.Data.Length.ToString() + " bytes)");
        }

        private void client_Connected(object sender, NetSocketConnectedEventArgs e)
        {
            this.Log("Connected: " + e.SourceIP);
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            System.Net.IPEndPoint end = new System.Net.IPEndPoint(System.Net.IPAddress.Parse(this.tbIP.Text), 3333);
            this.client.Connect(end);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.lbLog.SelectedItem != null)
                MessageBox.Show((string)this.lbLog.SelectedItem);
        }

        private void buttonSendText_Click(object sender, EventArgs e)
        {
            if (this.client.State != SocketState.Connected)
            {
                this.Log("Send Cancelled");
                return;
            }
            byte[] name = Encoding.ASCII.GetBytes(tbSourceText.Text + "," + tbData1.Text + "," + tbData2.Text + "," + tbPrivateKeyB.Text);
            {
                this.client.Send(name);
            }
        }

        private void frmClosing(object sender, FormClosingEventArgs e)
        {
            this.client.Close("User forced");
        }

        private void Client_Load(object sender, EventArgs e)
        {
            diffy_.ListenClient(tbPrivateKeyB.Text, lbLog);
        }
    }
}