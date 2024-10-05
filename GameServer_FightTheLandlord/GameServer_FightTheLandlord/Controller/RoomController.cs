using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;

namespace GameServer_FightTheLandlord
{
	public class RoomController : BaseController 
	{
		ResultDo _resultDo = new ResultDo();
		RoomDo _roomDo = new RoomDo();

		public override string OnRequest (int requestCode, int actionCode, string data, NetClient client, Server server)
		{
			if (actionCode == ActionCode.CreateRoom) {
				return CreateRoom (data, client, server);
			}
			else if (actionCode == ActionCode.GetRoomList) {
				return GetRoomList (data, client, server);
			}
			else if (actionCode == ActionCode.JoinRoom) {
				return JoinRoom (data, client, server);
			}
			else if (actionCode == ActionCode.LeaveRoom) {
				return LeaveRoom (data, client, server);
			}
			/*
			else if (actionCode == ActionCode.GetRoomInfo) {
				return GetRoomInfo (data, client, server);
			}
			*/
			return "";
		}



		string CreateRoom(string data, NetClient client, Server server)
		{
			Console.WriteLine ("开始创建房间 能否创建房间：" + (client.RoomId == 0));
			if (client.RoomId != 0) {
				_roomDo.LeaveRoom (client.RoomId, client.UserId);
				client.RoomId = 0;
			}
			Console.WriteLine ("正在创建房间中");
			_roomDo.CreateRoom (client.UserId, client.UserId, 2);
			client.RoomId = client.UserId;
			Console.WriteLine ("创建房间成功");
			return "1";
			/*
			if (client.Room != null) {
				return "";
			}
			server.CreateRoom (client, 2);
			return client.Room.GetId ().ToString ();
			*/
		}
		//获取房间列表
		string GetRoomList(string data, NetClient client, Server server)
		{
			var roomList = _roomDo.GetHouseOwnerList ();
			if (roomList == null || roomList.Count <= 0) {
				return "0";
			}
			StringBuilder sb = new StringBuilder ();
			for (int i = 0; i < roomList.Count; i++) {
				var m_client = server.GetClient (roomList [i].RoomId);
				if (m_client != null) {
					sb.Append (m_client.GetResultInfo (roomList [i].RoomId) + "#");
				}
			}
			if (sb.Length == 0) {
				sb.Append ("0");
			}
			if (sb.Length > 0) {
				sb.Remove (sb.Length - 1, 1);
			}
			return sb.ToString ();
			/*
			var roomList = server.GetRoomList ();
			StringBuilder sb = new StringBuilder ();
			for (int i = 0; i < roomList.Count; i++) {
				if (roomList[i].IsWaitJoin == true) {
					sb.Append (roomList [i].GetHouseOwnerInfo () + "#");
				}
			}
			if (sb.Length == 0) {
				sb.Append ("0");
			}
			else if (sb.Length > 0) {
				sb.Remove (sb.Length - 1, 1);
			}
			return sb.ToString ();
			*/
		}

		//TODO
		#region 解决房主第一次离开房间后的创建房间和加入房间不起作用问题？

		#endregion
		//加入房间
		string JoinRoom(string data, NetClient client, Server server)
		{
			//如果加入者之前曾创建房间 则离开房间并清除
			if (client.RoomId != 0) {
				_roomDo.LeaveRoom (client.RoomId, client.UserId);
				client.RoomId = 0;
			}
			int roomId = int.Parse (data);
			_roomDo.JoinRoom (roomId, client.UserId);
			client.RoomId = roomId;
			Broadcast (client, server, ActionCode.JoinRoom, client.GetRoomInfo ());
			client.Send (RequestCode.Room, ActionCode.JoinRoom, client.GetRoomInfo ());
			return "";
		}
		//离开房间
		string LeaveRoom(string data, NetClient client, Server server)
		{
			if (client.RoomId <= 0) {
				return "";
			}
			if (client.RoomId == client.UserId) {//离开用户自己创建的房间
				Broadcast (client, server, ActionCode.LeaveRoom, "0");
				_roomDo.LeaveRoom (client.RoomId, client.UserId);
				client.RoomId = 0;
			}
			else {
				_roomDo.LeaveRoom (client.RoomId, client.UserId);
				Console.WriteLine ("离开房间后的将更新房间信息为" + client.GetRoomInfo ());
				Broadcast (client, server, ActionCode.LeaveRoom, client.GetRoomInfo ());
				client.RoomId = 0;
			}
			client.RoomId = 0;
			return "2";
		}

		/*
		string GetRoomInfo(string data, NetClient client, NetServer server)
		{
			if (client.RoomId == 0) {
				return "";
			}
			var room = _roomDo.GetRoom (client.RoomId);
			Broadcast (client, server, ActionCode.GetRoomInfo, client.GetRoomInfo ());

			return client.GetRoomInfo ();
		}
		*/
		//广播事件
		int Broadcast(NetClient client, Server server, int actionCode, string data)
		{
			var room = _roomDo.GetRoom(client.RoomId);
			for (int i = 0; i < room.Count; i++) {
				if (room[i].UserId != client.UserId) {
					return server.GetClient (room [i].UserId).Send (RequestCode.Room, actionCode, data);
				}
			}
			return -1;
		}
	}
}

