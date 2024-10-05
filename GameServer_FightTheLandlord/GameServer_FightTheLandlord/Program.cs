using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
//using System.Threading.Tasks;

namespace GameServer_FightTheLandlord
{
    
    class Program
    {
        public const string IPAddress = "106.13.27.133";
        public const string IPPort = "50000";
        static void Main(string[] args)
        {
            //IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
            //string ipaddr = "";
            //foreach(var ip in ips)
            //{
            //    Console.WriteLine(ip.ToString());
            //    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            //    {
            //        ipaddr = ip.ToString();
            //        break;
            //    }
            //}
            Console.WriteLine("请输入服务器域名或IP地址：");
            string ip = Console.ReadLine();          
            
            if (!IsIPAddress(ip) )//如果不是IP地址而是域名则解析为IP
            {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(ip);
                IPAddress myip = ipHostInfo.AddressList[0];
                ip = myip.ToString();
            }


            Console.WriteLine("请输入服务器端口号：");
            string port = Console.ReadLine();
            MyGameServer server = new MyGameServer(ip, port);
            server.StartListen();
            while (true)
            {
                string cmd = Console.ReadLine();
                if (cmd.Equals("exit"))
                {
                    break;
                }
                if (cmd.Equals("test"))
                {
                    RemoteCMD_Data recData = new RemoteCMD_Data();
                    recData.player.Name = "test";
                    MyParser parser = new MyParser();
                    string msg= LitJson.JsonMapper.ToJson(recData);
                    byte[] buf = parser.EncoderData(msg);
                    server.BroadcastMsg(null, buf);
                }
            }
        }

        public static bool IsIPAddress(string sip)
        {
            if (string.IsNullOrEmpty(sip) || sip.Length < 7 || sip.Length > 15) return false;
            const string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";

            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(sip);
        }
    }
}
