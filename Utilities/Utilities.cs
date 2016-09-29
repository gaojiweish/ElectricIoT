using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.NetworkInformation;

namespace GK.Utilities
{
    public class Utilities
    {
        public static string GetPPPoEIP(int timeout)
        {
            try
            {
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                bool havePPPOE = false;
                foreach (NetworkInterface adapter in nics)
                {
                    if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ppp)
                    {
                        havePPPOE = true;
                        IPInterfaceProperties ip = adapter.GetIPProperties();     //IP配置信息
                        if (ip.UnicastAddresses.Count > 0)
                        {
                            return ip.UnicastAddresses[0].Address.ToString();
                        }
                    }
                }
                //当没有宽带连接的时候直接返回空
                if (!havePPPOE) return "";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return "";
        }
        /// <summary>
        /// 获取客户端外网IP，省份，城市，运营商
        /// </summary>
        /// <returns></returns>
        private static string getOutMessage()
        {
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.Default;
            string response = client.UploadString("http://iframe.ip138.com/ipcity.asp", "");
            Match mc = Regex.Match(response, @"location.href=""(.*)"""); response = client.UploadString(mc.Groups[1].Value, "");
            return response;
        }
        /// <summary>
        /// 获取外网IP
        /// </summary>
        /// <returns>外网IP地址</returns>
        public static string getOutIp()
        {
            string response = getOutMessage();
            int i = response.IndexOf("[") + 1;
            string ip = response.Substring(i, 14);
            string ips = ip.Replace("]", "").Replace(" ", "");
            return ips;
        }
        /// <summary>
        /// 获取省份
        /// </summary>
        /// <returns>省份</returns>
        public static string getOutProvince()
        {
            string response = getOutMessage();
            int i = response.IndexOf("自") + 2;
            string province = response.Substring(i, response.IndexOf("省") - i + 1);
            return province;
        }
        /// <summary>
        /// 获取城市
        /// </summary>
        /// <returns>城市</returns>
        public static string getOutCity()
        {
            string response = getOutMessage();
            int i = response.IndexOf("省") + 1;
            string city = response.Substring(i, response.IndexOf("市") - i + 1);
            return city;
        }
        /// <summary>
        /// 获取运营商
        /// </summary>
        /// <returns>运营商</returns>
        public static string getOutProvider()
        {
            string response = getOutMessage();
            int i = response.IndexOf("市") + 2;
            string provider = response.Substring(i, 2);
            return provider;
        }
    }
}
