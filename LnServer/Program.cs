using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace LnServer
{
    class Program
    {
        //InternetGetConnectedState返回的状态标识位的含义：
        private const int INTERNET_CONNECTION_MODEM = 1;
        private const int INTERNET_CONNECTION_LAN = 2;
        private const int INTERNET_CONNECTION_PROXY = 4;
        private const int INTERNET_CONNECTION_MODEM_BUSY = 8;
        [DllImport("winInet.dll ")]
        //声明外部的函数：
        private static extern bool InternetGetConnectedState(ref  int dwFlag,int dwReserved);
        static TSocketServerBase<TTestSession> m_socketServer = new TSocketServerBase<TTestSession>();
        static bool SessionConnectFlag = false;
        static System.Timers.Timer timer = new System.Timers.Timer(30000);

        static void Main(string[] args)
        {
            int dwFlag = 0;
            if (!InternetGetConnectedState(ref dwFlag, 0))
            {
                Console.Write("Not internet！    " + DateTime.Now.ToString() + "\r\n");
                System.Threading.Thread.Sleep(10000);
                Application.Restart();
                Environment.Exit(0);
            }
            else
            {
                //联网状态
                if ((dwFlag & INTERNET_CONNECTION_MODEM) != 0)
                    Console.Write("Internet is modem; \r\n");
                if ((dwFlag & INTERNET_CONNECTION_LAN) != 0)
                    Console.Write("Internet is network card;  \r\n");
                if ((dwFlag & INTERNET_CONNECTION_PROXY) != 0)
                    Console.Write("Internet is proxy;  \r\n");
                if ((dwFlag & INTERNET_CONNECTION_MODEM_BUSY) != 0)
                    Console.Write("Modemin use by another non-Internet connection;  \r\n");
                //启动Socket服务
                m_socketServer.MaxDatagramSize = 1024 * 1000;
                //附加所有事件
                AttachServerEvent();
                m_socketServer.Start();
                Console.Read();
            }
        }

        private static void Timer_Elapsed(object sneder, System.Timers.ElapsedEventArgs e)
        {
            if (!SessionConnectFlag)
            {
                Console.Write("Not session connect，reset application!");
                System.Threading.Thread.Sleep(60000);
                Application.Restart();
                Environment.Exit(0);
            }
            else
                timer.Stop();
        }
        private static void AttachServerEvent()
        {
            m_socketServer.ServerStarted += SocketServer_Started;
            m_socketServer.ServerClosed += SocketServer_Stoped;
            m_socketServer.ServerListenPaused += SocketServer_Paused;
            m_socketServer.ServerListenResumed += SocketServer_Resumed;
            m_socketServer.ServerException += SocketServer_Exception;

            m_socketServer.SessionRejected += SocketServer_SessionRejected;
            m_socketServer.SessionConnected += SocketServer_SessionConnected;
            m_socketServer.SessionDisconnected += SocketServer_SessionDisconnected;
            m_socketServer.SessionReceiveException += SocketServer_SessionReceiveException;
            m_socketServer.SessionSendException += SocketServer_SessionSendException;

            m_socketServer.DatagramDelimiterError += SocketServer_DatagramDelimiterError;
            m_socketServer.DatagramOversizeError += SocketServer_DatagramOversizeError;
            m_socketServer.DatagramAccepted += SocketServer_DatagramReceived;
            m_socketServer.DatagramError += SocketServer_DatagramrError;
            m_socketServer.DatagramHandled += SocketServer_DatagramHandled;

            m_socketServer.ShowDebugMessage += SocketServer_ShowDebugMessage;
        }

        private static void SocketServer_Started(object sender, EventArgs e)
        {
            Console.Write("Server started at: " + DateTime.Now.ToString() + "\r\n");
        }

        private static void SocketServer_Stoped(object sender, EventArgs e)
        {
            Console.Write("Server stoped at: " + DateTime.Now.ToString() + "\r\n");
        }

        private static void SocketServer_Paused(object sender, EventArgs e)
        {
            Console.Write("Server paused at: " + DateTime.Now.ToString() + "\r\n");
        }

        private static void SocketServer_Resumed(object sender, EventArgs e)
        {
            Console.Write("Server resumed at: " + DateTime.Now.ToString() + "\r\n");
        }

        private static void SocketServer_Exception(object sender, TExceptionEventArgs e)
        {
            Console.Write("Server exception: " + e.ExceptionMessage + "\r\n");
        }

        private static void SocketServer_SessionRejected(object sender, EventArgs e)
        {
            Console.Write("Session connect rejected" + "\r\n");
        }

        private static void SocketServer_SessionTimeout(object sender, TSessionEventArgs e)
        {
            Console.Write("Session timeout: ip " + e.SessionBaseInfo.IP + "\r\n");
        }

        private static void SocketServer_SessionConnected(object sender, TSessionEventArgs e)
        {
            SessionConnectFlag = true;
            Console.Write("Session connected: ip " + e.SessionBaseInfo.IP + "\r\n");
        }

        private static void SocketServer_SessionDisconnected(object sender, TSessionEventArgs e)
        {
            Console.Write("Session disconnected: ip " + e.SessionBaseInfo.IP + "\r\n");
        }

        private static void SocketServer_SessionReceiveException(object sender, TSessionEventArgs e)
        {
            Console.Write("Session receive exception: ip " + e.SessionBaseInfo.IP + "\r\n");
        }

        private static void SocketServer_SessionSendException(object sender, TSessionEventArgs e)
        {
            Console.Write("Session send exception: ip " + e.SessionBaseInfo.IP + "\r\n");
        }

        private static void SocketServer_SocketReceiveException(object sender, TSessionExceptionEventArgs e)
        {
            Console.Write("Client socket receive exception: ip: " + e.SessionBaseInfo.IP + "  exception message: " + e.ExceptionMessage + "\r\n");
        }

        private static void SocketServer_SocketSendException(object sender, TSessionExceptionEventArgs e)
        {
            Console.Write("Client socket send exception: ip: " + e.SessionBaseInfo.IP + " exception message: " + e.ExceptionMessage + "\r\n");
        }

        private static void SocketServer_DatagramDelimiterError(object sender, TSessionEventArgs e)
        {
            Console.Write("datagram delimiter error. ip: " + e.SessionBaseInfo.IP + "\r\n");
        }

        private static void SocketServer_DatagramOversizeError(object sender, TSessionEventArgs e)
        {
            Console.Write("datagram oversize error. ip: " + e.SessionBaseInfo.IP + "\r\n");
        }

        private static void SocketServer_DatagramReceived(object sender, TSessionEventArgs e)
        {
            Console.Write("datagram received. ip: " + e.SessionBaseInfo.IP + "\r\n");
        }

        private static void SocketServer_DatagramrError(object sender, TSessionEventArgs e)
        {
            Console.Write("datagram error. ip: " + e.SessionBaseInfo.IP + "\r\n");
        }

        private static void SocketServer_DatagramHandled(object sender, TSessionEventArgs e)
        {
            Console.Write("datagram handled. ip: " + e.SessionBaseInfo.IP + "\r\n");
        }

        private static void SocketServer_ShowDebugMessage(object sender, TExceptionEventArgs e)
        {
            Console.Write("debug message: " + e.ExceptionMessage + "\r\n");
        }
    }

    /// <summary>
    /// 测试用会话Session类
    /// </summary>
    public class TTestSession : TSessionBase
    {
        List<DataCollection> data = new List<DataCollection>();
        private static string PostUrl = @"Http://139.196.197.62/AmbientData.php";

        /// <summary>
        /// 重写错误处理方法, 返回消息给客户端
        /// </summary>
        protected override void OnDatagramDelimiterError()
        {
            base.OnDatagramDelimiterError();

            base.SendDatagram("datagram delimiter error");
        }

        /// <summary>
        /// 重写错误处理方法, 返回消息给客户端
        /// </summary>
        protected override void OnDatagramOversizeError()
        {
            base.OnDatagramOversizeError();

            base.SendDatagram("datagram over size");
        }
        /// <summary>
        /// 重写 AnalyzeDatagram 方法, 调用数据存储方法
        /// </summary>
        protected override void AnalyzeDatagram(byte[] datagramBytes)
        {
            try
            {
                int subIndex = 0;
                while (subIndex < datagramBytes.Length)
                {
                    string result = "";
                    int length = 0;
                    int count = 0;
                    switch (datagramBytes[subIndex++])
                    {
                        case 0x05://标识ASDU5
                            //subIndex++;
                            ASCIIEncoding ascii = new ASCIIEncoding();
                            Console.Write("传送原因：" + datagramBytes[++subIndex].ToString() + "\r\n");
                            Console.Write("应用服务数据单元公共地址：" + datagramBytes[++subIndex].ToString() + "\r\n");
                            Console.Write("功能类型：" + datagramBytes[++subIndex].ToString() + "\r\n");
                            Console.Write("信息序号：" + datagramBytes[++subIndex].ToString() + "\r\n");
                            subIndex++;
                            Console.Write("制造厂名称：" + ascii.GetString(datagramBytes.Skip(++subIndex).Take(8).Reverse().ToArray()) + "\r\n");
                            subIndex += 8;
                            Console.Write("装置类型：" + datagramBytes[++subIndex].ToString() + "\r\n");
                            Console.Write("装置出厂8位编号：" + datagramBytes[++subIndex].ToString() + "\r\n");
                            Console.Write("软件版本号：" + BitConverter.ToUInt16(datagramBytes.Skip(++subIndex).Take(2).Reverse().ToArray(), 0).ToString() + "\r\n");
                            //Console.Write("ASDU5格式：传送原因,应用服务数据单元公共地址,功能类型,信息序号,制造厂名称,装置类型,装置出厂8位编号,软件版本号\r\n");
                            break;
                        case 0x0A://通用分类数据ASDU10                     
                            //Console.Write("ASDU10格式：传送原因,应用服务数据单元公共地址,功能类型,信息序号,通用分类数据集数目,通用分类标示组号,条目号,描述类别,数据类型,数据......通用分类标示组号,条目号,描述类别,数据类型,数据\r\n");
                            int transmitCause = datagramBytes[++subIndex];
                            //Console.Write("传送原因：" + transmitCause.ToString() + "\r\n");
                            int deviceAddr = datagramBytes[++subIndex];
                            //Console.Write("应用服务数据单元公共地址：" + deviceAddr.ToString() + "\r\n");
                            int functionType = datagramBytes[++subIndex];
                            //Console.Write("功能类型：" + functionType.ToString() + "\r\n");
                            int infoID = datagramBytes[++subIndex];
                            //Console.Write("信息序号：" + infoID.ToString() + "\r\n");
                            subIndex++;
                            count = datagramBytes[++subIndex];//7
                            //Console.Write("通用分类数据集数目：" + count.ToString() + "\r\n");
                            for (int i = 0; i < count; i++)
                            {
                                int groupID = datagramBytes[++subIndex];
                                //Console.Write("通用分类标示组号：" + groupID.ToString() + "\r\n");
                                int itemID = datagramBytes[++subIndex];
                                //Console.Write("条目号：" + itemID.ToString() + "\r\n");
                                int describeType = datagramBytes[++subIndex];
                                //Console.Write("描述类别：" + describeType.ToString() + "\r\n");
                                int m_DataType = datagramBytes[++subIndex];//11
                                int m_DataSize = datagramBytes[++subIndex];//12
                                string str = Convert.ToString(datagramBytes[++subIndex], 2).PadLeft(8, '0');//13
                                int m_Number = int.Parse(str.Substring(1, 7));
                                int m_Cont = int.Parse(str.Substring(0, 1));
                                //Console.Write("数据类型：" + m_DataType.ToString() + "\r\n");
                                string data = "";
                                if (m_DataSize == 1)
                                {
                                    data = GetValue(m_DataType, m_DataSize, m_Number, datagramBytes[++subIndex], null);
                                    //Console.Write("数据：" + data + "\r\n");
                                }
                                else
                                {
                                    data = GetValue(m_DataType, m_DataSize, m_Number, 0, datagramBytes.Skip(++subIndex).Take(m_DataSize).ToArray());
                                    //Console.Write("数据：" + data + "\r\n");
                                }
                                if (groupID == 0x04)
                                {
                                    ValueChangeEventArgs e = new ValueChangeEventArgs(deviceAddr, groupID, itemID, describeType, m_DataType, data);
                                    ValueChange(this, e);
                                }
                                else
                                {
                                    if (data.Contains("溢出") || data.Contains("无效"))
                                        Console.Write("Error data：" + data + "\r\n");
                                    else
                                        CheckDataPackage(deviceAddr, groupID, itemID, describeType, m_DataType, data);
                                }

                                subIndex += m_DataSize - 1;
                                length += 6 + m_DataSize;
                            }
                            length += 8;
                            break;
                        case 0x17://被记录的扰动表ASDU23
                            Console.Write("ASDU23格式：传送原因,应用服务数据单元公共地址,故障序号,故障状态,7个八位组二进制时间(年，月，日，星期，时，分，毫秒)\r\n");
                            int m_Count = datagramBytes[subIndex + 1];
                            int m_Milliseconds = datagramBytes[subIndex + 9] | datagramBytes[subIndex + 10] << 8;
                            int m_Invalid = int.Parse(Convert.ToString(datagramBytes[subIndex + 11], 2).PadLeft(8, '0').Substring(0, 1));//IV<0>=有效;IV<1>=无效
                            int m_RES1 = int.Parse(Convert.ToString(datagramBytes[subIndex + 11], 2).PadLeft(8, '0').Substring(1, 1));//备用1
                            int m_Minutes = int.Parse(Convert.ToString(datagramBytes[subIndex + 11], 2).PadLeft(8, '0').Substring(2, 6));
                            int m_SummerTime = int.Parse(Convert.ToString(datagramBytes[subIndex + 12], 2).PadLeft(8, '0').Substring(0, 1));//SU<0>=标准时间;SU<1>=夏季时间
                            int m_RES2 = int.Parse(Convert.ToString(datagramBytes[subIndex + 12], 2).PadLeft(8, '0').Substring(1, 2));
                            int m_Hours = int.Parse(Convert.ToString(datagramBytes[subIndex + 12], 2).PadLeft(8, '0').Substring(3, 5));
                            int m_DayOfWeek = int.Parse(Convert.ToString(datagramBytes[subIndex + 13], 2).PadLeft(8, '0').Substring(0, 3));
                            int m_DayOfMonth = int.Parse(Convert.ToString(datagramBytes[subIndex + 13], 2).PadLeft(8, '0').Substring(0, 3));
                            int m_RES3 = int.Parse(Convert.ToString(datagramBytes[subIndex + 14], 2).PadLeft(8, '0').Substring(0, 4));
                            int m_Months = int.Parse(Convert.ToString(datagramBytes[subIndex + 14], 2).PadLeft(8, '0').Substring(4, 4));
                            int m_RES4 = int.Parse(Convert.ToString(datagramBytes[subIndex + 15], 2).PadLeft(8, '0').Substring(0, 1));
                            int m_Years = int.Parse(Convert.ToString(datagramBytes[subIndex + 15], 2).PadLeft(8, '0').Substring(1, 7));
                            result += "" + datagramBytes[subIndex + 2].ToString() + "," +
                                                           "" + datagramBytes[subIndex + 3].ToString() + "," +
                                                           "" + BitConverter.ToUInt16(datagramBytes.Skip(subIndex + 6).Take(2).Reverse().ToArray(), 0).ToString() + "," +
                                                           "" + datagramBytes[subIndex + 8].ToString() + "," +
                                                           "" + m_Years.ToString() + "," +
                                                           "" + m_Months.ToString() + "," +
                                                           "" + m_DayOfMonth.ToString() + "," +
                                                           "" + m_DayOfWeek.ToString() + "," +
                                                           "" + m_Hours.ToString() + "," +
                                                           "" + m_Minutes.ToString() + "," +
                                                           "" + m_Milliseconds.ToString() + "";
                            length = 6 + datagramBytes[subIndex + 1] * 10;
                            subIndex += length;
                            break;
                        //case 0x1A://扰动数据传输准备就绪ASDU26
                        //    Console.Write("ASDU26格式：传送原因,应用服务数据单元公共地址,扰动值类型,故障序号,电网故障序号,通道数目,一个通道信息元素的数目,信息元素间的间隔,四个八位位组二进制时间\r\n");
                        //    int n_Milliseconds = datagramBytes[subIndex + 17] | datagramBytes[subIndex + 18] << 8;
                        //    int n_Invalid = int.Parse(Convert.ToString(datagramBytes[subIndex + 19], 2).Substring(0, 1));//IV<0>=有效;IV<1>=无效
                        //    int n_RES1 = int.Parse(Convert.ToString(datagramBytes[subIndex + 19], 2).Substring(1, 1));//备用1
                        //    int n_Minutes = int.Parse(Convert.ToString(datagramBytes[subIndex + 19], 2).Substring(2, 6));
                        //    int n_SummerTime = int.Parse(Convert.ToString(datagramBytes[subIndex + 20], 2).Substring(0, 1));//SU<0>=标准时间;SU<1>=夏季时间
                        //    int n_RES2 = int.Parse(Convert.ToString(datagramBytes[subIndex + 20], 2).Substring(1, 2));
                        //    int n_Hours = int.Parse(Convert.ToString(datagramBytes[subIndex + 20], 2).Substring(3, 5));
                        //    result += "" + datagramBytes[subIndex + 2].ToString() + "," +
                        //                                   "" + datagramBytes[subIndex + 3].ToString() + "," +
                        //                                   "" + datagramBytes[subIndex + 7].ToString() + "," +
                        //                                   "" + BitConverter.ToUInt16(datagramBytes.Skip(subIndex + 8).Take(2).Reverse().ToArray(), 0).ToString() + "," +
                        //                                   "" + BitConverter.ToUInt16(datagramBytes.Skip(subIndex + 10).Take(2).Reverse().ToArray(), 0).ToString() + "," +
                        //                                   "" + datagramBytes[subIndex + 12].ToString() + "," +
                        //                                   "" + BitConverter.ToUInt16(datagramBytes.Skip(subIndex + 13).Take(2).Reverse().ToArray(), 0).ToString() + "," +
                        //                                   "" + BitConverter.ToUInt16(datagramBytes.Skip(subIndex + 15).Take(2).Reverse().ToArray(), 0).ToString() + "," +
                        //                                   "" + n_Hours.ToString() + "," +
                        //                                   "" + n_Minutes.ToString() + "," +
                        //                                   "" + n_Milliseconds.ToString() + "";
                        //    length = 21;
                        //    subIndex += length;
                        //    break;
                        //case 0x1B://被记录的通道传输准备就绪ASDU27
                        //    Console.Write("ASDU27格式：传送原因,应用服务数据单元公共地址,扰动值类型,故障序号,实际通道序号,一次额定值,二次额定值,参比因子\r\n");
                        //    result += "" + datagramBytes[subIndex + 2].ToString() + "," +
                        //                                  "" + datagramBytes[subIndex + 3].ToString() + "," +
                        //                                  "" + datagramBytes[subIndex + 7].ToString() + "," +
                        //                                  "" + BitConverter.ToUInt16(datagramBytes.Skip(subIndex + 8).Take(2).Reverse().ToArray(), 0).ToString() + "," +
                        //                                  "" + datagramBytes[subIndex + 10].ToString() + "," +
                        //                                  "" + BitConverter.ToSingle(datagramBytes.Skip(subIndex + 11).Take(4).Reverse().ToArray(), 0).ToString() + "," +
                        //                                  "" + BitConverter.ToSingle(datagramBytes.Skip(subIndex + 15).Take(4).Reverse().ToArray(), 0).ToString() + "," +
                        //                                  "" + BitConverter.ToSingle(datagramBytes.Skip(subIndex + 19).Take(4).Reverse().ToArray(), 0).ToString() + "";
                        //    length = 23;
                        //    subIndex += length;
                        //    break;
                        case 0x1C://带标志的状态变位传输准备就绪ASDU28
                            Console.Write("ASDU28格式：传送原因,应用服务数据单元公共地址,故障序号\r\n");
                            result += "" + datagramBytes[subIndex + 2].ToString() + "," +
                                                          "" + datagramBytes[subIndex + 3].ToString() + "," +
                                                          "" + BitConverter.ToUInt16(datagramBytes.Skip(subIndex + 8).Take(2).Reverse().ToArray(), 0).ToString() + "";
                            length = 10;
                            subIndex += length;
                            break;
                        case 0x1D://传送带标志的状态变化ASDU29
                            Console.Write("ASDU29格式：传送原因,应用服务数据单元公共地址,故障序号,带标志的状态变位的数目,标志的位置,功能类型,信息序号,DPI........标志的位置,功能类型,信息序号,DPI\r\n");
                            count = datagramBytes[subIndex + 8];
                            result += "" + datagramBytes[subIndex + 2].ToString() + "," +
                                                          "" + datagramBytes[subIndex + 3].ToString() + "," +
                                                          "" + BitConverter.ToUInt16(datagramBytes.Skip(subIndex + 6).Take(2).Reverse().ToArray(), 0).ToString() + "," +
                                                          "" + datagramBytes[subIndex + 8].ToString() + "";
                            for (int i = 0; i < count; i++)
                            {
                                result += "," + BitConverter.ToUInt16(datagramBytes.Skip(subIndex + 9).Take(2).Reverse().ToArray(), 0).ToString() + "," +
                                                "" + datagramBytes[subIndex + 11].ToString() + "," +
                                                "" + datagramBytes[subIndex + 12].ToString() + "," +
                                                "" + Convert.ToString(datagramBytes[subIndex + 20], 2).Substring(6, 1) + "";
                                subIndex += 5;
                            }
                            length = 9 + count * 5;
                            subIndex += 9;
                            break;
                        //case 0x1E://传送扰动值ASDU30
                        //    Console.Write("ASDU30格式：传送原因,应用服务数据单元公共地址,扰动值类型,故障序号,实际通道号,每个应用服务数据单元有关扰动值的数目,应用服务数据单元的第一个信息元素的序号,单个扰动值1........单个扰动值N\r\n");
                        //    count = BitConverter.ToUInt16(datagramBytes.Skip(subIndex + 11).Take(2).Reverse().ToArray(), 0);
                        //    result += "" + datagramBytes[subIndex + 2].ToString() + "," +
                        //                                  "" + datagramBytes[subIndex + 3].ToString() + "," +
                        //                                  "" + datagramBytes[subIndex + 7].ToString() + "," +
                        //                                  "" + BitConverter.ToUInt16(datagramBytes.Skip(subIndex + 8).Take(2).Reverse().ToArray(), 0).ToString() + "," +
                        //                                  "" + datagramBytes[subIndex + 10].ToString() + "," +
                        //                                  "" + count.ToString() + "," +
                        //                                  "" + BitConverter.ToUInt16(datagramBytes.Skip(subIndex + 13).Take(2).Reverse().ToArray(), 0).ToString() + "";
                        //    for (int i = 0; i < count; i++)
                        //    {
                        //        result += "," + BitConverter.ToUInt16(datagramBytes.Skip(subIndex + 15).Take(2).Reverse().ToArray(), 0).ToString() + "";
                        //        subIndex += 2;
                        //    }
                        //    length = 15 + count * 2;
                        //    subIndex += 15;
                        //    break;
                        //case 0x1F://传送结束ASDU31
                        //    Console.Write("ASDU31格式：传送原因,应用服务数据单元公共地址,命令类型,扰动值类型,故障序号,实际通道号\r\n");
                        //    result += "" + datagramBytes[subIndex + 2].ToString() + "," +
                        //                                  "" + datagramBytes[subIndex + 3].ToString() + "," +
                        //                                  "" + datagramBytes[subIndex + 6].ToString() + "," +
                        //                                  "" + datagramBytes[subIndex + 7].ToString() + "," +
                        //                                  "" + BitConverter.ToUInt16(datagramBytes.Skip(subIndex + 8).Take(2).Reverse().ToArray(), 0).ToString() + "," +
                        //                                  "" + datagramBytes[subIndex + 10].ToString() + "";
                        //    length = 11;
                        //    subIndex += length;
                        //    break;
                        default:
                            subIndex++;
                            continue;
                    }
                    subIndex++;
                }
                //int clientAddress = 0;

                //if (datagramBytes.Length > 10)
                //    clientAddress = datagramBytes[4];

                //base.OnDatagramAccepted();  // 模拟接收到一个完整的数据包

                //if (clientAddress!=0 && datagramBytes.Length > 5)
                //{

                //    if (datagramTextLength == datagramBytes.Length)
                //    {
                //        base.SendDatagram("<OK: " + clientAddress.ToString() + ", datagram length = " + datagramTextLength.ToString() + ">");

                //this.Store(datagramBytes);
                //        base.OnDatagramHandled();  // 模拟已经处理（存储）了数据包
                //    }
                //    else
                //    {
                //        base.SendDatagram("<ERROR: " + clientName + ", error length, datagram length = " + datagramTextLength.ToString() + ">");
                //        base.OnDatagramError();  // 错误包
                //    }
                //}
                //else if (string.IsNullOrEmpty(clientName))
                //{
                //    base.SendDatagram("client: no name, datagram length = " + datagramTextLength.ToString());
                //    base.OnDatagramError();
                //}
                //else if (datagramTextLength == 0)
                //{
                //    base.SendDatagram("client: " + clientName + ", datagram length = " + datagramTextLength.ToString());
                //    base.OnDatagramError();  // 错误包
                //}
            }
            catch(Exception e)
            {
                Console.Write(e.ToString() + "   " + base.IP);
            }
        }

        string GetValue(int dataType,int dataSize,int dataNumber,byte dataByte,byte[] data)
        {
            string value = "";
            switch (dataType)
            {
                case 0:
                    break;
                case 1:
                    ASCIIEncoding ascii = new ASCIIEncoding();
                    value = ascii.GetString(data);
                    break;
                case 2:
                    break;
                case 3:
                    if (dataSize == 1)
                        value = dataByte.ToString();
                    else
                        value = Math.Abs(BitConverter.ToUInt32(data, 0)).ToString();
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
                case 9:
                    value = Convert.ToInt32(Convert.ToString(dataByte, 2).PadLeft(8, '0').Substring(6, 2), 2).ToString();
                    break;
                case 10:
                    break;
                case 11:
                    break;
                case 12:
                    string str = Convert.ToString(data[1], 2).PadLeft(8, '0') + Convert.ToString(data[0], 2).PadLeft(8, '0');
                    value = (str.Substring(0, 1) == "1" ? "溢出" : "") + (str.Substring(1, 1) == "1" ? "无效" : "" + Convert.ToInt32(str.Substring(3, 13), 2).ToString());
                    break;
                case 13:
                    break;
                case 14:
                    break;
                case 15:
                    break;
                case 16:
                    break;
                case 17:
                    break;
                case 18:
                    int DPI = Convert.ToInt32(Convert.ToString(data[0], 2).PadLeft(8, '0').Substring(6, 2),2);
                    int n_Milliseconds = data[1] | data[2] << 8;
                    int n_Minutes = Convert.ToInt32(Convert.ToString(data[3], 2).PadLeft(8,'0').Substring(2, 6),2);
                    int n_SummerTime = Convert.ToInt32(Convert.ToString(data[4], 2).PadLeft(8,'0').Substring(0, 1),2);//SU<0>=标准时间;SU<1>=夏季时间
                    int n_Hours = Convert.ToInt32(Convert.ToString(data[4], 2).PadLeft(8, '0').Substring(3, 5), 2);
                    value = "DPI=" + DPI.ToString() + " " + (n_SummerTime == 1 ? "夏季时间" : "标准时间 ") + n_Hours.ToString() + ":" + n_Minutes.ToString() + " " + n_Milliseconds.ToString();
                    break;
                case 19:
                    break;
                case 20:
                    break;
                case 21:
                    break;
                case 22:
                    break;
                case 23:
                    break;
                case 24:
                    break;
                case 25:
                    break;
                case 26:
                    break;
                case 201:
                    break;
            }
            return value;
        }
        private string PostWebRequest(string postUrl, string paramData, Encoding dataEncode)
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

        private void CheckDataPackage(int _DeviceAddr,int _GroupID,int _ItemID,int _DescribeType,int _DataType,string _Data)
        {
            DataCollection group = data.Find(delegate(DataCollection dc)
                {
                    return dc.GroupID == _GroupID;
                });
            if(group != null)
            {
                DataCollectionBase item = group.GroupData.Find(delegate(DataCollectionBase dcb)
                    {
                        return dcb.EntryID == _ItemID;
                    });
                if(item !=null)
                {
                    if(item.Data != _Data)
                    {
                        item.Data = _Data;
                        ValueChangeEventArgs e = new ValueChangeEventArgs(_DeviceAddr, _GroupID, _ItemID, _DescribeType, _DataType, _Data);
                        ValueChange(this,e);
                    }
                }
                else
                {
                    DataCollectionBase itemObj = new DataCollectionBase(_ItemID, _DescribeType, _DataType, _Data);
                    group.GroupData.Add(itemObj);
                    ValueChangeEventArgs e = new ValueChangeEventArgs(_DeviceAddr, _GroupID, _ItemID, _DescribeType, _DataType, _Data);
                    ValueChange(this, e);
                }
            }
            else
            {
                DataCollection groupObj = new DataCollection(_GroupID, _ItemID, _DescribeType, _DataType, _Data);
                groupObj.ValueChange += ValueChange;
                data.Add(groupObj);
                ValueChangeEventArgs e = new ValueChangeEventArgs(_DeviceAddr, _GroupID, _ItemID, _DescribeType, _DataType, _Data);
                ValueChange(this, e);
            }
        }
        private void ValueChange(object sender, ValueChangeEventArgs e)
        {
            PostWebRequest(PostUrl, "1,2," + e.Value, Encoding.ASCII);
            Console.Write("1,2," + e.Value + "     " + DateTime.Now.ToString() + "   " + base.IP + "\r\n");
        }
    }

    class DataCollection
    {
        private int groupID;
        private List<DataCollectionBase> groupData=new List<DataCollectionBase>();
        public event EventHandler<ValueChangeEventArgs> ValueChange;
        public int GroupID
        {
            get
            {
                return groupID;
            }
        }
        public List<DataCollectionBase> GroupData
        {
            get {
                return groupData;
            }
            set {
                groupData = value;
            }
        }
        public DataCollection(int _groupID, int _entryID, int _description, int _dataType, string _data)
        {
            groupID = _groupID;
            DataCollectionBase dcb=new DataCollectionBase(_entryID, _description,  _dataType, _data);
            groupData.Add(dcb);
        }
        protected virtual void OnValueChangeEvent(ValueChangeEventArgs e)
        {
            EventHandler<ValueChangeEventArgs> handler = System.Threading.Interlocked.CompareExchange(ref ValueChange, null, null);
            if (handler != null)
                handler(this, e);
        }
    }
    class DataCollectionBase
    {
        private int entryID;
        private int description;
        private int dataType;
        private string data;
        public int EntryID
        {
            get {
                return entryID;
            }
        }
        public int Description
        {
            get {
                return description;
            }
        }
        public int DataType
        {
            get {
                return dataType;
            }
        }
        public string Data
        {
            get {
                return data;
            }
            set {
                data = value;
            }
        }
        public DataCollectionBase(int _entryID, int _description, int _dataType, string _data)
        {
            entryID = _entryID;
            description = _description;
            dataType = _dataType;
            data = _data;
        }
    }
    public class ValueChangeEventArgs : EventArgs
    {
        private string value;
        public ValueChangeEventArgs(int _DeviceAddr, int _GroupID, int _ItemID, int _DescribeType, int _DataType, string _Data)
        {
            this.value = _DeviceAddr.ToString() + "," + _GroupID.ToString() + "," + _ItemID.ToString() + "," + _DescribeType.ToString() + "," + _DataType.ToString() + "," + _Data;
        }
        public string Value
        {
            get
            {
                return this.value;
            }
        }
    }
}
