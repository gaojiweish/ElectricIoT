using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GK.TCPIPServer;
using GK.SerialPortServer;

namespace WinFormServerDemo
{
    public partial class frmMain : Form
    {
        private SystemConfig config = new SystemConfig();
        private EnvironmentDetection ed = new EnvironmentDetection();
        private IntegratedProtector ip = new IntegratedProtector();
        private IntegratedProtector om = new IntegratedProtector();
        private EventLog el = new EventLog();
        private List<Control> ControlList = new List<Control>();

        private static TSocketServerBase<TTestSession> m_socketServer = new TSocketServerBase<TTestSession>();
        public frmMain()
        {
            InitializeComponent();
        }
        #region //UI
        private void label_MouseEnter(object sender, EventArgs e)
        {
            ((Label)sender).Font = new Font("Calibri", 14, FontStyle.Bold);
        }

        private void label_MouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).Font = new Font("Calibri", 14, FontStyle.Regular);
        }

        private void label_Click(object sender, EventArgs e)
        {
            if(((Label)sender).Text.Contains("Environment"))
                panelCore.Controls.SetChildIndex(ed, 0);
            else if (((Label)sender).Text.Contains("Integrated"))
                panelCore.Controls.SetChildIndex(ip, 0);
            else if (((Label)sender).Text.Contains("Online"))
                panelCore.Controls.SetChildIndex(om, 0);
            else if(((Label)sender).Text.Contains("Event"))
                panelCore.Controls.SetChildIndex(el, 0);
        }

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources._2;
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources._1;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            panelCore.Controls.SetChildIndex(config, 0);
        }

        private void FillControl(Control ctl)
        {
            ctl.Dock = DockStyle.Fill;
            panelCore.Controls.Add(ctl);
        }
        private void frmMain_Load(object sender, EventArgs e)
        {
            ControlList.Add(ed);
            ControlList.Add(config);
            ControlList.Add(ip);
            ControlList.Add(om);
            ControlList.Add(el);
            foreach (Control c in ControlList)
                FillControl(c);
            panelCore.BringToFront();

        }
        #endregion
        private void StartServer()
        {
            //启动Socket服务
            m_socketServer.MaxDatagramSize = 1024 * 32;
            //附加所有事件
            AttachServerEvent();
            m_socketServer.Start();

            SerialPortServer m_EDSerial = new SerialPortServer(Properties.Settings.Default.EDInterface,
                                                                                            int.Parse(Properties.Settings.Default.EDProperty));
            m_EDSerial.Port.DataReceived += EnvironmentDetectionDataReceived;
            m_EDSerial.Start();
        }
        private static void AttachServerEvent()
        {
            //m_socketServer.ServerStarted += SocketServer_Started;
            //m_socketServer.ServerClosed += SocketServer_Stoped;
            //m_socketServer.ServerListenPaused += SocketServer_Paused;
            //m_socketServer.ServerListenResumed += SocketServer_Resumed;
            //m_socketServer.ServerException += SocketServer_Exception;

            //m_socketServer.SessionRejected += SocketServer_SessionRejected;
            //m_socketServer.SessionConnected += SocketServer_SessionConnected;
            //m_socketServer.SessionDisconnected += SocketServer_SessionDisconnected;
            //m_socketServer.SessionReceiveException += SocketServer_SessionReceiveException;
            //m_socketServer.SessionSendException += SocketServer_SessionSendException;

            //m_socketServer.DatagramDelimiterError += SocketServer_DatagramDelimiterError;
            //m_socketServer.DatagramOversizeError += SocketServer_DatagramOversizeError;
            //m_socketServer.DatagramAccepted += SocketServer_DatagramReceived;
            //m_socketServer.DatagramError += SocketServer_DatagramrError;
            //m_socketServer.DatagramHandled += SocketServer_DatagramHandled;

            //m_socketServer.ShowDebugMessage += SocketServer_ShowDebugMessage;
        }
        private void EnvironmentDetectionDataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            
        }
    }
}
