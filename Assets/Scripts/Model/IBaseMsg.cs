using UnityEngine;

using System;

public  interface IBaseMsg
{
    RemoteCMD_Data Data { get; set; }
    string ToJson();
}
public class RemoteMsg : IBaseMsg
{
    public RemoteCMD_Data Data{ get;set; }
    public RemoteMsg(RemoteCMD_Data data)
    {
        Data = data;
    }
    public string ToJson()
    {
        string json = LitJson.JsonMapper.ToJson(Data);
        return json;
    }
}


[Serializable]
public class RemoteCMD_Data
{
    public RemoteCMD_Const cmd;    
    public PlayerInfo player;
    public Card[] cards;
    public int MatchCounts;
    public RemoteCMD_Data()
    {
        player = new PlayerInfo();
    }
}
[Serializable]
public enum RemoteCMD_Const:int
{
    Match=0,//匹配
    MatchSuccess,//匹配成功
    MatchWait,//等待匹配完成             
    GenerateCards,//生成牌
    DealCards,//发牌
    BaseCards,//底牌
    Player2,//指定玩家2
    Player3,//指定玩家3
    Player4,//指定玩家4
    StartPlayer,//游戏开始的玩家
    CallLandlord,//叫地主
    NotCall,//不叫
    Claim,//抢地主
    NotClaim,//不抢
    Pass,//跳过
    Discards,//出牌
    Chong,//冲
    GamerOver,//游戏结束
    GameTurn,//行动回合
    CancelMatch,//取消匹配
    CalcScore,//计算积分
    Replay,//赢家
    ReturnLobby,//返回大厅
    ReturnRoom,//返回房间
    Register,//注册
    RegSucess,//注册成功
    RegError,//格式错误
    RegAlready,//该用户名已注册
    Login,    //登录
    LoginSuccess,//登录成功
    LoginPwdErr, //密码错误
    LoginUserErr, //无此用户
    CreateRoom,//创建房间
    GetRoomList,//获取房间列表
    JoinRoom,//加入房间
    LeaveRoom,//离开房间
    GetRoomInfo,//获取房间信息
    Quit//退出游戏
}