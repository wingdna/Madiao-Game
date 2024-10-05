using System;

namespace GameServer_FightTheLandlord
{
	public class BaseController
	{
		public virtual string OnRequest(int requestCode, int actionCode, string data, NetClient client, Server server)
		{
			return "";
		}
	}
}

