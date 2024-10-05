using System;
using System.Collections.Generic;
using System.Text;
using LitJson;
using System.Linq;
namespace GameServer_FightTheLandlord
{
 
    public class Room
    {
        public List<string> Members { get;}
  
       
        public int RoomId { get;}
        public string LandlordIP { get; set; }//庄家的IP
        public int CurrentTurn;//当前行动玩家索引
        public int iZhuang;//庄家所在位置
        public int ClaimCount;//争夺地主次数
        public int iContinue; //点击继续游戏人数
        public PlayStatus playStatus ;

        public enum PlayStatus : int
        {
            WaitJoin = 0,//等待加入
            WaitBattle, //等待游戏开始
            NewPlay ,//新建游戏
            RePlay,//继续游戏
            Playing//游戏中
        }

        public bool IsFull {
            get
            {
                return Members.Count >=Capacity;
            }
        }

        public bool IsWaitJoin { get { return playStatus == PlayStatus.WaitJoin; } }
        public int Capacity { get;}
        public Room(int capacity,int id)
        {
            Members = new List<string>();
            //_clientList = new List<NetClient>();
            Capacity = capacity;
            RoomId = id;
            iZhuang = 0;
            iContinue = 0;
            ClaimCount = 1;
            playStatus = PlayStatus.WaitJoin;
            //ClaimCount = 4;//最多4次
        }
        public bool DelMember(string name)
        {
            if(Members.Count > 1)
            {
                //if (Members.Contains(name)) return false;
                if ( !Members.Contains(name)) return false;
                Members.Remove(name);

                if (Members.Count < Capacity)
                    playStatus = PlayStatus.WaitJoin;
                else if (Members.Count == Capacity)
                    playStatus = PlayStatus.WaitBattle;
                return true;
            }
            return false;
        }
        public void ClearMembers()
        {
            Members.Clear();
            ClaimCount = 0;
            LandlordIP = null;
        }
        public bool AddMember(string name)
        {
            if (  Members.Count < Capacity
                &&playStatus == PlayStatus.WaitJoin )
            {
                if (Members.Contains(name)) return false;
                Members.Add(name);
                return true;
            }
            if (Members.Count == Capacity)
                playStatus = PlayStatus.WaitBattle;
            return false;
        }

        public void Leave(string name)
        {
            if (IsRoomOwner(name))
            {
                ClearMembers();               
            }
            else
            {
                Members.Remove(name);
                playStatus = PlayStatus.WaitJoin;
            }
        }

        public bool IsRoomOwner(string name)
        {
            if (Members == null || Members.Count <= 0)
            {
                return false;
            }
            return Members[0] == name;
        }

        public void SetZhuang(int i)
        {
            iZhuang = i;
        }

        public int GetZhuang()
        {
            return iZhuang ;
        }
    }

  
    public class RoomManager
    {
        private Queue<int> freeRoomId;
        private Dictionary<int, Room> rooms;

       
        public RoomManager(int count)
        {
            freeRoomId = new Queue<int>();
            rooms = new Dictionary<int, Room>();
            Init(count);
        }
        public void Init(int count)
        {
            for(int i = 0; i < count; i++)
            {
                rooms.Add(i + 1, new Room(4, i + 1));
                freeRoomId.Enqueue(i + 1);
            }
        }
        public Room GetRoom()
        {
            if (freeRoomId.Count < 1)
            {
                rooms.Add(rooms.Count + 1, new Room(4, rooms.Count + 1));
                return rooms[rooms.Count];
            }
            return rooms[freeRoomId.Dequeue()];
        }
        public void ReleaseRoom(Room room)
        {
            room.ClearMembers();
            freeRoomId.Enqueue(room.RoomId);
        }
        public Room QueryRoom(int id)
        {
            if (rooms.ContainsKey(id))
            {
                return rooms[id];
            }
            return null;
        }
        public Room this[int id]
        {
            get
            {
                if (rooms.ContainsKey(id))
                {
                    return rooms[id];
                }
                return null;
            }
        }

      
    }


    public class MyGameServer : Server
    {
        private RoomManager roomManager;
        private Dictionary<string, int> player_Room;//玩家的房间号
        private Dictionary<string, string> ipname_PlayerName;//ip--昵称
        private Room curFreeRoom;
        private Random rand;
        private MyParser parser;
       
        public List<Card> CurrentDiscards4;

        UserDo _userDo = new UserDo();
        RoomDo _roomDo = new RoomDo();
        ResultDo _resultDo = new ResultDo();

        public MyGameServer(string ip, string port) : base(ip, port)
        {
            rand = new Random();
            roomManager = new RoomManager(10);
            player_Room = new Dictionary<string, int>();
            curFreeRoom = roomManager.GetRoom();
            parser = new MyParser();
            ipname_PlayerName = new Dictionary<string, string>();
            CurrentDiscards4 = new List<Card>();
            
        }
        public static int renames = 2;
        List<string> ListName = new List<string>();
        private RemoteCMD_Data Rename(RemoteCMD_Data recData)
        {
            //重复名字需改名
            if (ListName.Exists(s => s == recData.player.Name))
            {
                recData.player.Name = recData.player.Name + "_" + renames++.ToString();
                PlayerInfo newplayer = new PlayerInfo();
                newplayer.Name = recData.player.Name;
                recData.player = newplayer;
            }
            ListName.Add(recData.player.Name);
            if (ListName.Count >= 4)
            {
                ListName.Clear();
                renames = 2;
            }
            return recData;
        }
        public override void HandleRecievedData(string ipname, string data)
        {
            Console.WriteLine(data);
            RemoteCMD_Data recData = JsonMapper.ToObject<RemoteCMD_Data>(data);

            //RemoteCMD_Data recData = Rename(recDatax);       

            switch (recData.cmd)
            {
                case RemoteCMD_Const.Match:
                    {
                        HandleMatch(ipname, recData.player);
                    } break;
                case RemoteCMD_Const.CallLandlord:
                    {
                        HandleCallLandlord(ipname, recData);
                    }
                    break;
                case RemoteCMD_Const.NotCall:
                    {
                        HandleNotCall(ipname, recData);
                    }
                    break;
                case RemoteCMD_Const.Claim:
                    {
                        HandleClaim(ipname, recData);
                    }
                    break;
                case RemoteCMD_Const.NotClaim:
                    {
                        HandleNotClaim(ipname, recData);
                    }
                    break;
                case RemoteCMD_Const.GenerateCards:
                    {
                        HandleGenerateCards(recData.cards, ipname,recData.player.Name );
                    }
                    break;
                case RemoteCMD_Const.Discards:
                    {
                        HandleDiscards(ipname, recData);
                    }
                    break;
                case RemoteCMD_Const.Pass:
                    {
                        HandlePass(ipname, recData);
                    }
                    break;
                case RemoteCMD_Const.GamerOver:
                    {
                        HandleGameOver(ipname, recData);
                    }
                    break;
                case RemoteCMD_Const.CancelMatch:
                    {
                        HandleCancelMatch(ipname, recData);
                    }
                    break;
                case RemoteCMD_Const.Replay:
                    {
                        HandleReplay(ipname, recData);
                    }
                    break;
                case RemoteCMD_Const.ReturnLobby:
                    {
                        HandleReturnLobby(ipname, recData);
                    }
                    break;
                case RemoteCMD_Const.ReturnRoom:
                    {
                        HandleReturnRoom(ipname, recData);
                    }
                    break;
                case RemoteCMD_Const.CalcScore:
                    {
                        HandleCalcScore(ipname, recData);
                    }
                    break;
                case RemoteCMD_Const.Login:
                    {
                        HandleLogin(ipname, recData);
                    }
                    break;
                case RemoteCMD_Const.Register:
                    {
                        HandleRegister(ipname, recData);
                    }
                    break;
                case RemoteCMD_Const.CreateRoom:
                    {
                        HandleCreateRoom(recData);
                    }
                    break;
                case RemoteCMD_Const.Quit:
                    {
                        HandleDisconnected(ipname);
                    }
                    break;
            }
        }

        private void HandleCancelMatch(string ipname, RemoteCMD_Data recData)
        {
            
            lock (curFreeRoom)
            {
                //发送匹配等待命令,更新匹配人数
                RemoteCMD_Data rec = new RemoteCMD_Data();
                rec.cmd = RemoteCMD_Const.MatchWait;
                rec.MatchCounts = curFreeRoom.Members.Count - 1;
                TransmitRecCMD(ipname, rec);
                curFreeRoom.DelMember(ipname);
                roomManager[player_Room[ipname]].DelMember(ipname);//移除房间中的玩家
                if (player_Room.ContainsKey(ipname))
                    player_Room.Remove(ipname);
                Console.WriteLine("玩家：" + ipname_PlayerName[ipname] + "退出匹配");
                
            }
        }


        private void HandleReplay(string ipname, RemoteCMD_Data recData)
        {
            int izhuang = roomManager[player_Room[ipname]].Members.FindIndex((p) =>
                  {
                      return ipname_PlayerName[p] == ipname_PlayerName[ipname];
                  });//找到庄家所在的顺序
            roomManager[player_Room[ipname]].SetZhuang(izhuang);
            _userDo.SetUserInfo(recData.player.Name, "Gold", recData.player.Score.ToString() );
            Console.WriteLine("玩家" + ipname_PlayerName[ipname] + "得分：" + recData.player.Score.ToString() + ",本局登基");           

        }

        private void HandleCalcScore(string ipname, RemoteCMD_Data recData)
        {
        
            _userDo.SetUserInfo(recData.player.Name, "Gold", recData.player.Score.ToString());
            Console.WriteLine("玩家" + ipname_PlayerName[ipname] +"得分：" + recData.player.Score.ToString() );

        }

        private void HandleLogin(string ipname, RemoteCMD_Data recData)
        {
            if (recData.player.Name == "")
            {
                return ;
            }
            
            if (ipname_PlayerName != null &&
                 ipname_PlayerName.ContainsValue(recData.player.Name) )
            {
                Console.WriteLine("另一个客户端登录了User Id[") ;//+ client.UserId + "]");
                //client.Send(RequestCode.User, ActionCode.Login, "-3");
                //client.UserId = 0;
                //return "";
            }
            int userId = _userDo.Login(recData.player.Name);
            if (userId > 0)
            {
                var user = _userDo.GetList().Where(u => u.Id == userId).FirstOrDefault();
                if (user.Pwd == recData.player.Pwd)
                {
                    user.EndLoginTime = DateTime.Now.ToString();
                    _userDo.Update(user.Id, "EndLoginTime", user.EndLoginTime);
                   // client.UserId = user.Id;
                    Console.WriteLine("UserDo Login New User");
                    RemoteCMD_Data rec = new RemoteCMD_Data();
                    rec.cmd = RemoteCMD_Const.LoginSuccess;
                    rec.player = recData.player;
                    rec.player.Score = user.Gold;
                    SendRecCMD(ipname,rec);
                    //  client.Send(RequestCode.User, ActionCode.Login, client.GetResultInfo(client.UserId));
                }
                else
                {
                    Console.WriteLine("UserDo Login的返回值为-1");
                    //   client.Send(RequestCode.User, ActionCode.Login, "-1");
                    recData.cmd = RemoteCMD_Const.LoginPwdErr ;                   
                    SendRecCMD(ipname, recData);
                }
            }
            else if (userId == 0)
            {
                Console.WriteLine("UserDo Login的返回值为0");
                // client.Send(RequestCode.User, ActionCode.Login, "0");
                recData.cmd = RemoteCMD_Const.LoginUserErr;
                SendRecCMD(ipname, recData);
            }
            else
            {
                Console.WriteLine("UserDo Login的返回值为-2");
            }
            return ;
        }

        private void HandleRegister(string ipname, RemoteCMD_Data recData)
        {
            if (recData.player.Name == "")
            {
                return ;
            }
            //var array = data.Split(';');
            int result = _userDo.Register(recData.player.Name, recData.player.Pwd,recData.player.Score);
            if (result == 1)
            {
                Console.WriteLine("User Do Register New User");
                RemoteCMD_Data rec = new RemoteCMD_Data();
                rec.cmd = RemoteCMD_Const.RegSuccess;
                rec.player = recData.player;
                SendRecCMD(ipname,rec) ;
            }
            else if (result == -2)
            {
                Console.WriteLine("User Do Register的返回值为-2");
                recData.cmd = RemoteCMD_Const.RegAlready;
                SendRecCMD(ipname, recData);
                //client.Send(RequestCode.User, ActionCode.Register, "-2");
            }
            else if (result == -1)
            {
                Console.WriteLine("User Do Register的返回值为-1");
                recData.cmd = RemoteCMD_Const.RegError;
                SendRecCMD(ipname, recData);
            }
            
        }

        private void HandleCreateRoom( RemoteCMD_Data recData)
        {
            int userId = _userDo.Login(recData.player.Name);
            int roomId = recData.MatchCounts;
            Console.WriteLine("开始创建房间 能否创建房间：" + (roomId == 0));
            if (roomId != 0)
            {
                _roomDo.LeaveRoom(roomId, userId);
                roomId = 0;
            }
            Console.WriteLine("正在创建房间中");
            _roomDo.CreateRoom(roomId, userId, 3);
            roomId = userId;
            Console.WriteLine("创建房间成功");
            
        }
        private void HandleJoinRoom(RemoteCMD_Data recData)
        {
            int userId = _userDo.Login(recData.player.Name);
            int roomId = recData.MatchCounts;
            //如果加入者之前曾创建房间 则离开房间并清除
            if (roomId != 0)
            {
                _roomDo.LeaveRoom(roomId, userId);
                roomId = 0;
            }
            //int roomId = int.Parse(data);
            _roomDo.JoinRoom(roomId, userId);
            //client.RoomId = roomId;
            //Broadcast(client, server, ActionCode.JoinRoom, client.GetRoomInfo());
            //client.Send(RequestCode.Room, ActionCode.JoinRoom, client.GetRoomInfo());
            //return "";
        }


        public void HandleMatch(string ipname, PlayerInfo p)
        {
            //if (curFreeRoom.IsFull)
            //    ReleaseRoom(ipname);//释放房间    
            lock (curFreeRoom)
            {
                Console.WriteLine("玩家：" + p.Name + "加入房间,开始匹配");
                curFreeRoom.AddMember(ipname);//加入当前房间
                                              //绑定ip和昵称
                if (!ipname_PlayerName.ContainsKey(ipname))
                {                    
                    ipname_PlayerName.Add(ipname, p.Name);
                }
                ipname_PlayerName[ipname] = p.Name;
                //p.RoomId curFreeRoom.RoomId;
                //绑定玩家和房间号
                if (!player_Room.ContainsKey(ipname))
                {
                    player_Room.Add(ipname, curFreeRoom.RoomId);
                }
                player_Room[ipname] = curFreeRoom.RoomId;
                //当前房间满了
                if (curFreeRoom.IsFull)
                {
                    Console.WriteLine("房间满员，开始游戏");
                    //确定玩家顺序
                    int startPlayer = 0;//rand.Next(0, 4);//随机起始
                    curFreeRoom.CurrentTurn = startPlayer;
                    int player2 = (startPlayer+1) % 4;
                    int player3 = (startPlayer+2) % 4;
                    int player4 = (startPlayer + 3) % 4;
                    //发送匹配成功命令
                    RemoteCMD_Data rec = new RemoteCMD_Data();
                    rec.cmd = RemoteCMD_Const.MatchSuccess;
                    TransmitRecCMD(ipname, rec);

                    

                    //将顺序转发给房间内的玩家
                    rec.cmd = RemoteCMD_Const.StartPlayer;
                    rec.player.Name = ipname_PlayerName[curFreeRoom.Members[startPlayer]];
                    rec.player.Score = int.Parse( _userDo.GetUserInfo(rec.player.Name, "Gold") );
                    TransmitRecCMD(ipname, rec);
                    //
                    rec.cmd = RemoteCMD_Const.Player2;
                    rec.player.Name = ipname_PlayerName[curFreeRoom.Members[player2]];
                    rec.player.Score = int.Parse(_userDo.GetUserInfo(rec.player.Name, "Gold"));
                    TransmitRecCMD(ipname, rec);
                    //
                    rec.cmd = RemoteCMD_Const.Player3;
                    rec.player.Name = ipname_PlayerName[curFreeRoom.Members[player3]];
                    rec.player.Score = int.Parse(_userDo.GetUserInfo(rec.player.Name, "Gold"));
                    TransmitRecCMD(ipname, rec);
                    //
                    rec.cmd = RemoteCMD_Const.Player4;
                    rec.player.Name = ipname_PlayerName[curFreeRoom.Members[player4]];
                    rec.player.Score = int.Parse(_userDo.GetUserInfo(rec.player.Name, "Gold"));
                    TransmitRecCMD(ipname, rec);
                    ////新开一个房间
                    curFreeRoom = roomManager.GetRoom();

                    roomManager[player_Room[ipname]].playStatus = Room.PlayStatus.NewPlay;
                }
                else
                {
                    //发送匹配等待命令，更新匹配人数
                    RemoteCMD_Data rec = new RemoteCMD_Data();
                    rec.cmd = RemoteCMD_Const.MatchWait;
                    rec.MatchCounts = curFreeRoom.Members.Count;
                    TransmitRecCMD(ipname, rec);
                }
            }
        }

        private void HandleReturnLobby(string ipname, RemoteCMD_Data recData)
        {
            //roomManager[player_Room[ipname]].DelMember(ipname);//移除房间中的玩家
            
            recData.cmd = RemoteCMD_Const.ReturnLobby;
            TransmitRecCMD(ipname, recData);
            Console.WriteLine("玩家：" + ipname_PlayerName[ipname] + "退出，该房已退");

            
            List<string> lipname = new List<string>(roomManager[player_Room[ipname]].Members.ToList() ); 
            for (int i = lipname.Count - 1; i >= 0; i--)
            {
               
                if (player_Room.ContainsKey(lipname[i]))
                {
                    if (roomManager[player_Room[lipname[i]]].IsRoomOwner(lipname[i]))
                        roomManager.ReleaseRoom(roomManager[player_Room[lipname[i]]]);
                    //else
                        player_Room.Remove(lipname[i]);
                }
                
            }
           // if (roomManager[player_Room[ipname]].Members.Count > 0)
           //     roomManager.ReleaseRoom(roomManager[player_Room[ipname]]);
        }
        public void HandleReturnRoom(string ipname, RemoteCMD_Data recData)
        {
            if (!roomManager[player_Room[ipname]].IsFull)
            {
                recData.cmd = RemoteCMD_Const.ReturnLobby;
                SendRecCMD(ipname, recData);
            }

            lock (roomManager[player_Room[ipname]])
            {

                if (roomManager[player_Room[ipname]].Members.Contains(ipname))
                    roomManager[player_Room[ipname]].iContinue++;
                if (roomManager[player_Room[ipname]].iContinue ==
                    roomManager[player_Room[ipname]].Capacity)
                {
                    roomManager[player_Room[ipname]].iContinue = 0;
                    recData.cmd = RemoteCMD_Const.ReturnRoom;
                    SendRecCMD(ipname, recData);
                }
                          
            }
        }
        public void HandleGenerateCards(Card[] cards, string ipname,string recName)
        {
           

            Console.WriteLine("开始发牌...");
            Card[] cards1 = new Card[8];
            Array.Copy(cards, 0, cards1, 0, 8);
            Card[] cards2 = new Card[8];
            Array.Copy(cards, 8, cards2, 0, 8);
            Card[] cards3 = new Card[8];
            Array.Copy(cards, 16, cards3, 0, 8);
            Card[] cards4 = new Card[8];
            Array.Copy(cards, 24, cards4, 0, 8);
            Card[] baseCards = new Card[8];
            Array.Copy(cards, 32, baseCards, 0, 8);

            //升代 露面底牌为赏肩百趣者 则需挨近之牌代替 副极为极 肩升赏 次肩升肩 九十升百千万
            Card c = baseCards[baseCards.Length - 1];
            Card c0 = FindReplaceFace(c);
            cards1 = ReplaceCard(cards1, c0, c);
            cards2 = ReplaceCard(cards2, c0, c);
            cards3 = ReplaceCard(cards3, c0, c);
            cards4 = ReplaceCard(cards4, c0, c);

            //手牌1
            RemoteCMD_Data recData = new RemoteCMD_Data();
           
            recData.cmd = RemoteCMD_Const.DealCards;
            recData.cards = cards1;
            SendRecCMD(roomManager[player_Room[ipname]].Members[0], recData);
            //手牌2
            recData.cmd = RemoteCMD_Const.DealCards;
            recData.cards = cards2;
            SendRecCMD(roomManager[player_Room[ipname]].Members[1], recData);
            //手牌3
            recData.cmd = RemoteCMD_Const.DealCards;
            recData.cards = cards3;
            SendRecCMD(roomManager[player_Room[ipname]].Members[2], recData);
            //手牌4
            recData.cmd = RemoteCMD_Const.DealCards;
            recData.cards = cards4;
            SendRecCMD(roomManager[player_Room[ipname]].Members[3], recData);
            //底牌
            recData.cmd = RemoteCMD_Const.BaseCards;
            recData.cards = baseCards;
            TransmitRecCMD(ipname, recData);

            if (roomManager[player_Room[ipname]].playStatus == Room.PlayStatus.NewPlay)
            {
                int izhuang = baseCards[baseCards.Length - 1].Value % 4;
                roomManager[player_Room[ipname]].SetZhuang( izhuang);
            }
            //开始游戏
            SendTurn(ipname);
        }

        public void HandleCallLandlord(string ipname, RemoteCMD_Data recData)
        {
            Console.WriteLine("玩家"+recData.player.Name+"叫地主");
            TransmitRecCMD(ipname, recData);
            DecideLandlord(ipname, true);
            SendTurn(ipname);
        }

        public void HandleNotCall(string ipname, RemoteCMD_Data recData)
        {
            Console.WriteLine("玩家" + recData.player.Name + "不叫地主");
            TransmitRecCMD(ipname, recData);
            //DecideLandlord(ipname, false);
            SendTurn(ipname);
        }

        public void HandleClaim(string ipname, RemoteCMD_Data recData)
        {
            Console.WriteLine("玩家" + recData.player.Name + "抢地主");
            TransmitRecCMD(ipname, recData);
            //DecideLandlord(ipname, true);
            SendTurn(ipname);
        }

        public void HandleNotClaim(string ipname, RemoteCMD_Data recData)
        {
            Console.WriteLine("玩家" + recData.player.Name + "不抢地主");
            TransmitRecCMD(ipname, recData);
            //DecideLandlord(ipname, false);
            SendTurn(ipname);
        }

        private void DecideLandlord(string ipname,bool update)
        {
            if(update)
            {
                roomManager[player_Room[ipname]].LandlordIP = ipname;//更新地主
            }
            //roomManager[player_Room[ipname]].ClaimCount--;
            //if (roomManager[player_Room[ipname]].ClaimCount <= 0)
            //{
                roomManager[player_Room[ipname]].CurrentTurn =
                    roomManager[player_Room[ipname]].Members.IndexOf(
                        roomManager[player_Room[ipname]].LandlordIP);//起始玩家改为地主
            //}
        }

        public void HandleDiscards(string ipname, RemoteCMD_Data recData)
        {
            Console.WriteLine("玩家" + recData.player.Name + "出牌");
            if (recData.cards != null && CurrentDiscards4.Count < 4)
                CurrentDiscards4.Add(recData.cards[0]);
            if (CurrentDiscards4.Count >= 4)
            {
                int i = CurrentCards4Top();
                int Turn = roomManager[player_Room[ipname]].CurrentTurn;
                Turn -= 4 - i;
                Turn = Turn <= 0 ? (Turn + 4) % 4
                                     : Turn % 4;
                roomManager[player_Room[ipname]].CurrentTurn = Turn;
                CurrentDiscards4.Clear();
            }
            TransmitRecCMD(ipname, recData);           
            SendTurn(ipname);
        }

        public void HandlePass(string ipname, RemoteCMD_Data recData)
        {
            Console.WriteLine("玩家" + recData.player.Name + "跳过");
            TransmitRecCMD(ipname, recData);
            SendTurn(ipname);
        }

        public void HandleGameOver(string ipname, RemoteCMD_Data recData)
        {
            Console.WriteLine("玩家" + recData.player.Name + "获得胜利，游xi结束");
            roomManager[player_Room[ipname]].playStatus = Room.PlayStatus.RePlay;
            TransmitRecCMD(ipname, recData);            
                    
        }

        private void ReleaseRoom(string ipname)
        {
            for (int i = 0; i < roomManager[player_Room[ipname]].Members.Count; i++)
            {
                ipname_PlayerName.Remove(roomManager[player_Room[ipname]].Members[i]);

            }
            roomManager.ReleaseRoom(roomManager[player_Room[ipname]]);
            if (player_Room.ContainsKey(ipname) )
                player_Room.Remove(ipname);           
        }

        private void TransmitRecCMD(string ipname,RemoteCMD_Data recData)
        {
            string json = JsonMapper.ToJson(recData);
            byte[] buf = parser.EncoderData(json);
            TransmitMsg(roomManager[player_Room[ipname]].Members.ToArray(), buf, null);
        }

        private void SendRecCMD(string ipname, RemoteCMD_Data recData)
        {
            string json = JsonMapper.ToJson(recData);
            byte[] buf = parser.EncoderData(json);
            SendDataBegin(ipname, buf);
        }

        private void SendTurn(string ipname)
        {
            RemoteCMD_Data recData = new RemoteCMD_Data();
            //重名者改名
            //recData = Rename(recData);

            recData.cmd = RemoteCMD_Const.GameTurn;

            if (roomManager[player_Room[ipname]].playStatus != Room.PlayStatus.Playing)
            { //DecideLandlord(ipname, false);               
                roomManager[player_Room[ipname]].CurrentTurn = roomManager[player_Room[ipname]].GetZhuang();
                roomManager[player_Room[ipname]].playStatus = Room.PlayStatus.Playing;
            }

            int index = roomManager[player_Room[ipname]].CurrentTurn;//当前回合索引
            recData.player.Name = ipname_PlayerName[
                roomManager[player_Room[ipname]].Members[index]
                ];//当前玩家昵称
                  //string targetIP = roomManager[player_Room[ipname]].Members[index];//IP

            TransmitRecCMD(ipname, recData);//转发回合          

            roomManager[player_Room[ipname]].CurrentTurn++;//当前回合数增加
            roomManager[player_Room[ipname]].CurrentTurn %= 4;

        }

        public override void HandleDisconnected(string ipname)
        {
            ipname_PlayerName.Remove(ipname);
            //断开连接释放房间
            if (!player_Room.ContainsKey(ipname)) return;//已经释放
            ReleaseRoom(ipname);
            Console.WriteLine("玩家" + ipname + "断开连接，退出游戏");

        }
        public override void HandleAccepted(string ipName)
        {
            
        }

        public int CurrentCards4Top()
        {
            Card maxC = CurrentDiscards4[0];
            int pos = 0;
            for (int i = 1; i < CurrentDiscards4.Count; i++)
            {
                if (CurrentDiscards4[i].GreatThan(maxC))
                {
                    maxC = CurrentDiscards4[i];
                    pos = i;
                }
            }
            return pos;

        }


        #region 查找百老发给谁家
        public  int Whoisbai(List<Card> c0, List<Card> c1, List<Card> c2, List<Card> c3)
        {
            if (c0.Exists(c => c.Suit.Equals(PokerConst.Spade) && c.Value == 9))
                return 0;
            if (c1.Exists(c => c.Suit.Equals(PokerConst.Spade) && c.Value == 9))
                return 1;
            if (c2.Exists(c => c.Suit.Equals(PokerConst.Spade) && c.Value == 9))
                return 2;
            if (c3.Exists(c => c.Suit.Equals(PokerConst.Spade) && c.Value == 9))
                return 3;
            return -1;
        }
        #endregion

        public  Card[] ReplaceCard(Card[] cards, Card c1, Card c2)
        {
            List<Card> lcards = new List<Card>(cards);
            for (int i = 0; i < cards.Length; i++)
            {
                if (cards[i].Suit == c1.Suit && cards[i].Value == c1.Value)
                    cards[i] = c2;
            }
            return cards;
        }
        #region  升代  露面底牌为赏肩百趣者 则需挨近之牌代替 副极为极 肩升赏 次肩升肩 九十升百千万
        public  Card FindReplaceFace(Card c)
        {
            Card c0 = new Card();
            switch (c.Value)
            {
                case 1:
                    c0.Value = 2;
                    c0.Suit = c.Suit;
                    break;
                default: c0 = c; break;
            }
            if (c.Suit == PokerConst.Spade)
            {
                switch (c.Value)
                {
                    case 9:
                    case 10:
                    case 11:
                        c0.Value = 8;
                        c0.Suit = c.Suit;
                        break;
                    default: c0 = c; break;
                }
            }
            if (c.Suit == PokerConst.Diamond || c.Suit == PokerConst.Heart)
            {
                switch (c.Value)
                {
                    case 9:
                        c0.Value = 8;
                        c0.Suit = c.Suit;
                        break;
                    case 8:
                        c0.Value = 7;
                        c0.Suit = c.Suit;
                        break;
                    default: c0 = c; break;
                }

            }
            if (c.Suit == PokerConst.Club)
            {
                switch (c.Value)
                {
                    case 11:
                        c0.Value = 10;
                        c0.Suit = c.Suit;
                        break;
                    case 10:
                        c0.Value = 9;
                        c0.Suit = c.Suit;
                        break;
                    default: c0 = c; break;
                }

            }
            return c0;
        }
        #endregion
    }
}
