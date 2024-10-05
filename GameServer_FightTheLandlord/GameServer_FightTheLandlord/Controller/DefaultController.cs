using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;


namespace GameServer_FightTheLandlord
{
	public class DefaultController : BaseController
	{
		public override string OnRequest (int requestCode, int actionCode, string data, NetClient client, Server server)
		{
			if (actionCode == ActionCode.Default) {
				//时间  IP地址  消息数据
				Console.WriteLine ("[" + DateTime.Now + "]" + "[" + client.Address + "]" + data);
			}
			return "";
		}
	}
}

