using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using LitJson;

namespace GameServer_FightTheLandlord
{





    public class NetClient
    {
        Socket _socket;
        Server _server;
        byte[] _buffer = new byte[1024 * 1024];
        public int UserId;
        UserDo _userDo = new UserDo();
        ResultDo _resultDo = new ResultDo();
        RoomDo _roomDo = new RoomDo();
        public Room Room;
        public int RoomId;

        public NetClient(Socket socket, Server server)
        {
            _socket = socket;
            _server = server;
            Start();
        }

        public void Start()
        {
            _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, StartReceiveCallback, _socket);
        }

        void StartReceiveCallback(IAsyncResult ar)
        {
            try
            {
                int length = _socket.EndReceive(ar);
                if (length == 0)
                {
                    Close();
                }
                else if (length > 0)
                {
                    string smsg = Encoding.UTF8.GetString(_buffer, 0, length);
                    //var strs = str.Split('|');
                    int requestCode = RequestCode.Default;// int.Parse(strs[0]);
                    int actionCode = ActionCode.Default;// int.Parse(strs[1]);
                    string sdata = "";
                    
                    RemoteCMD_Data recData = JsonMapper.ToObject<RemoteCMD_Data>(smsg);
                    switch (recData.cmd)
                    {
                        case RemoteCMD_Const.Login:
                            requestCode = RequestCode.User;
                            actionCode = ActionCode.Login;
                            sdata = recData.player.Name + ";" + recData.player.Pwd;
                            break;
                        case RemoteCMD_Const.Register:
                            requestCode = RequestCode.User;
                            actionCode = ActionCode.Register;
                            sdata = recData.player.Name + ";" + recData.player.Pwd;
                            break;
                        case RemoteCMD_Const.CreateRoom:
                            requestCode = RequestCode.Room;
                            actionCode = ActionCode.CreateRoom;
                            sdata = recData.MatchCounts.ToString();
                            break;
                        case RemoteCMD_Const.GetRoomList:
                            requestCode = RequestCode.Room;
                            actionCode = ActionCode.GetRoomList;
                            sdata = recData.MatchCounts.ToString();
                            break;
                        case RemoteCMD_Const.JoinRoom:
                            requestCode = RequestCode.Room;
                            actionCode = ActionCode.JoinRoom;
                            sdata = recData.MatchCounts.ToString();
                            break;
                        case RemoteCMD_Const.LeaveRoom:
                            requestCode = RequestCode.Room;
                            actionCode = ActionCode.LeaveRoom;
                            sdata = recData.MatchCounts.ToString();
                            break;
                        case RemoteCMD_Const.GetRoomInfo:
                            requestCode = RequestCode.Room;
                            actionCode = ActionCode.GetRoomInfo;
                            sdata = recData.MatchCounts.ToString();
                            break;
                    }
                   //string data = strs[2];
                    _server.OnRequest(requestCode, actionCode, sdata, this);
                }
                Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("无法接收消息：" + ex.Message);
                Close();
            }
        }

        public int Send(int requestCode, int actionCode, string data)
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(requestCode + "|" + actionCode + "|" + data);
                return _socket.Send(buffer);
            }
            catch (Exception ex)
            {
                Console.WriteLine("无法发送消息：" + ex.Message);
            }
            return -1;
        }

        public void Close()
        {
            try
            {
                if (RoomId != 0)
                {
                    _roomDo.LeaveRoom(RoomId, UserId);
                    RoomId = 0;
                }
                if (Room != null)
                {
                    Room.Leave(_userDo.GetUserInfo(UserId, "Name"));
                }
                if (UserId != 0)
                {
                    UserId = 0;
                }
                if (_socket != null)
                {
                    _socket.Close();
                }
                _server.RemoveClient(this);
            }
            catch (Exception ex)
            {
                Console.WriteLine("无法关闭连接：" + ex.Message);
            }
        }

        public bool Connected { get { return _socket != null && _socket.Connected; } }

        public string Address
        {
            get
            {
                if (Connected == true)
                {
                    return _socket.RemoteEndPoint.ToString();
                }
                return "";
            }
        }

        public string GetResultInfo(int userId)
        {
            _resultDo.Add(userId);
            StringBuilder sb = new StringBuilder();
            sb.Append(userId.ToString());
            sb.Append(";");
            sb.Append(_userDo.GetUserInfo(userId, "Name"));
            sb.Append(";");
            sb.Append(_resultDo.GetResultInfo(userId, "TotalCount"));
            sb.Append(";");
            sb.Append(_resultDo.GetResultInfo(userId, "WinCount"));
            return sb.ToString();
        }

        public string GetRoomInfo()
        {
            var room = _roomDo.GetRoom(RoomId);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < room.Count; i++)
            {
                sb.Append(_server.GetClient(room[i].UserId).GetResultInfo(room[i].UserId) + "#");
            }
            if (sb.Length == 0)
            {
                sb.Append("0");
            }
            else if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }
    }
}



