using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace PortServer
{
    class Program
    {
        static SerialPort port = null;
        [DllImport("winInet.dll ")]
        //声明外部的函数：
        private static extern bool InternetGetConnectedState(ref  int dwFlag, int dwReserved);
        private static SersonValue ser=new SersonValue();
        private static string PortName="COM4";
        private static string PostUrl = @"Http://139.196.197.62/AmbientData.php";
        static void Main(string[] args)
        {
            if (SerialPort.GetPortNames().Contains(PortName))
            {
                port = new SerialPort(PortName, 9600, Parity.None, 8, StopBits.One);
                    port.DataReceived += Port_DataReceived;
                    port.Open();
                    Console.Write("Port: " + PortName + " , 115200,8,None,1    " + DateTime.Now.ToString() + "\r\n");
                    ser.SersonValueChange += SersonChange;
            }
            else
            {
                System.Threading.Thread.Sleep(5000);
                Application.Restart();
                Environment.Exit(0);
            }
            Console.Read();
        }
        private static void SersonChange(object sender, SersonChangeEventArgs e)
        {
            int dwFlag = 0;
            if (InternetGetConnectedState(ref dwFlag, 0))
            {
                //PostWebRequest(PostUrl, e.Value, Encoding.ASCII);
                Console.Write(e.Value + "   " + DateTime.Now.ToString() + "\r\n");
            }
        }
        private static void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                int length = port.ReadBufferSize;
                byte[] data = new byte[length];
                port.Read(data, 0, length);
                int index = 0;
                while (index < length)
                {
                    if (data[index++] == 0xFE)
                    {
                        switch (data[index++])
                        {
                            case 0x01:
                                ser.Smoke = data[index++] == 0x00 ? 1 : 0;
                                break;
                            case 0x03:
                                ser.Humidity = data[index++];
                                ser.Temperature = (float)(BitConverter.ToUInt16(data.Skip(index++).Take(2).ToArray(), 0) * 0.1);
                                break;
                            case 0x04:
                                ser.WaterDipped1 = data[index++];
                                ser.WaterDipped2 = data[index++];
                                //ser.WaterLevel = (float)(((BitConverter.ToUInt16(data.Skip(index++).Take(2).ToArray(), 0) * 0.1) - 0.7) * 300 / 2.3);
                                ser.WaterLevel = 0;
                                break;
                        }
                    }
                    index++;
                }
            }
            catch (Exception ex)
            {
                Console.Write("Error: " + ex.ToString() + "   " + DateTime.Now.ToString() + "\r\n");
            }
        }

        private static string PostWebRequest(string postUrl, string paramData, Encoding dataEncode)
        {
            string ret = string.Empty;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/x-www-form-urlencoded";

                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                Console.Write("PostWebRequest:" + ex.Message + "\r\n");
            }
            return ret;
        }
    }
    public class SersonChangeEventArgs : EventArgs
    {
        private string value;
        public SersonChangeEventArgs(string str)
        {
            this.value = str;
        }
        public string Value
        {
            get {
                return this.value;
            }
        }
    }
    public class SersonValue
    {
        public event EventHandler<SersonChangeEventArgs> SersonValueChange;
        private int CountFlag = 40;
        private int m_Humidity;
        private int m_HumidityCount;
        private float m_Temperature;
        private int m_TemperatureCount;
        private float m_WaterLevel;
        private int m_WaterLevelCount;
        private int m_Smoke;
        private int m_SmokeCount;
        private int m_WaterDipped1;
        private int m_WaterDipped1Count;
        private int m_WaterDipped2;
        private int m_WaterDipped2Count;

        private string _SiteID = "1"; //无锡
        private string _DataTypeID = "1";//环境数据
        public int Humidity
        {
            set {
                if (m_Humidity != value)
                {
                    m_Humidity = value;
                    m_HumidityCount=0;
                    SersonChangeEventArgs e = new SersonChangeEventArgs(_SiteID + "," + _DataTypeID + ",0510000000000001,2,1," + m_Humidity);
                    OnSersorValueChange(e);
                }
                else
                {
                    m_HumidityCount++;
                    if (m_HumidityCount == CountFlag)
                    {
                        SersonChangeEventArgs e = new SersonChangeEventArgs(_SiteID + "," + _DataTypeID + ",0510000000000001,2,1," + m_Humidity);
                        OnSersorValueChange(e);
                        m_HumidityCount = 0;
                    }
                }
            }
        }
        public int Smoke
        {
            set {
                if (m_Smoke != value)
                {
                    m_Smoke = value;
                    m_SmokeCount = 0;
                    SersonChangeEventArgs e = new SersonChangeEventArgs(_SiteID + "," + _DataTypeID + ",0510000000000002,4,1," + m_Smoke);
                    OnSersorValueChange(e);
                }
                else
                {
                    m_SmokeCount++;
                    if (m_SmokeCount == CountFlag)
                    {
                        m_SmokeCount = 0;
                        SersonChangeEventArgs e = new SersonChangeEventArgs(_SiteID + "," + _DataTypeID + ",0510000000000002,4,1," + m_Smoke);
                        OnSersorValueChange(e);
                    }
                }
            }
        }
        public int WaterDipped1 
        {
            set {
                if (m_WaterDipped1 != value)
                {
                    m_WaterDipped1 = value;
                    m_WaterDipped1Count = 0;
                    SersonChangeEventArgs e = new SersonChangeEventArgs(_SiteID + "," + _DataTypeID + ",0510000000000003,3,1," + m_WaterDipped1);
                    OnSersorValueChange(e);
                }
                else
                {
                    m_WaterDipped1Count++;
                    if (m_WaterDipped1Count == CountFlag)
                    {
                        m_WaterDipped1Count = 0;
                        SersonChangeEventArgs e = new SersonChangeEventArgs(_SiteID + "," + _DataTypeID + ",0510000000000003,3,1," + m_WaterDipped1);
                        OnSersorValueChange(e);
                    }
                }
            }
        }
        public int WaterDipped2
        {
            set
            {
                if (m_WaterDipped2 != value)
                {
                    m_WaterDipped2 = value;
                    m_WaterDipped2Count = 0;
                    SersonChangeEventArgs e = new SersonChangeEventArgs(_SiteID + "," + _DataTypeID + ",0510000000000003,3,2," + m_WaterDipped2);
                    OnSersorValueChange(e);
                }
                else
                {
                    m_WaterDipped2Count++;
                    if (m_WaterDipped2Count == CountFlag)
                    {
                        m_WaterDipped2Count = 0;
                        SersonChangeEventArgs e = new SersonChangeEventArgs(_SiteID + "," + _DataTypeID + ",0510000000000003,3,2," + m_WaterDipped2);
                        OnSersorValueChange(e);
                    }
                }
            }
        }
        public float Temperature 
        {
            set {
                if (m_Temperature != value)
                {
                    m_Temperature = value;
                    m_TemperatureCount = 0;
                    SersonChangeEventArgs e = new SersonChangeEventArgs(_SiteID + "," + _DataTypeID + ",0510000000000001,1,1," + m_Temperature);
                    OnSersorValueChange(e);
                }
                else
                {
                    m_TemperatureCount++;
                    if (m_TemperatureCount == CountFlag)
                    {
                        m_TemperatureCount = 0;
                        SersonChangeEventArgs e = new SersonChangeEventArgs(_SiteID + "," + _DataTypeID + ",0510000000000001,1,1," + m_Temperature);
                        OnSersorValueChange(e);
                    }
                }
            } 
        }
        public float WaterLevel 
        {
            set {
                if (m_WaterLevel != value)
                {
                    m_WaterLevel = value;
                    m_WaterLevelCount = 0;
                    SersonChangeEventArgs e = new SersonChangeEventArgs(_SiteID + "," + _DataTypeID + ",0510000000000004,5,1," + m_WaterLevel);
                    OnSersorValueChange(e);
                }
                else
                {
                    m_WaterLevelCount++;
                    if (m_WaterLevelCount == CountFlag)
                    {
                        m_WaterLevelCount = 0;
                        SersonChangeEventArgs e = new SersonChangeEventArgs(_SiteID + "," + _DataTypeID + ",0510000000000004,5,1," + m_WaterLevel);
                        OnSersorValueChange(e);
                    }
                }
            } 
        }
        protected virtual void OnSersorValueChange(SersonChangeEventArgs e)
        {
            EventHandler<SersonChangeEventArgs> handler = System.Threading.Interlocked.CompareExchange(ref SersonValueChange, null, null);
            if (handler != null)
                handler(this, e);
        }
    }
}
