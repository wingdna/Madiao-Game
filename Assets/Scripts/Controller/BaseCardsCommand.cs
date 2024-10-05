
using strange.extensions.command.impl;
using UnityEngine;

public class BaseCardsCommand : EventCommand
{
    [Inject]
    public IClientService clientService { get; set; }
    [Inject]
    public IMdData gameData { get; set; }
    public override void Execute()
    {
        RemoteCMD_Data recData = evt.data as RemoteCMD_Data;
        Debug.Log("BaseCards:"+LitJson.JsonMapper.ToJson(recData));
        Card[] baseCard = new Card[recData.cards.Length];
        recData.cards.CopyTo(baseCard,0);
        gameData.BaseCards.AddRange(baseCard);//保存底牌数据

        gameData.CurrentTurn = baseCard[baseCard.Length - 1].Value % 4;
        if (string.IsNullOrEmpty(gameData.Landlord))
            gameData.Landlord = gameData.Players[gameData.CurrentTurn].Name;

        /*    gameData.CurrentTurn = gameData.Players.FindIndex((p) => { return p.Name == gameData.Landlord; });//找到地主所在的顺序;
        else if (baseCard.Length > 0)
        {
            gameData.CurrentTurn = baseCard[baseCard.Length - 1].Value % 4;
            gameData.Landlord = gameData.Players[gameData.CurrentTurn].Name;
        }*/

        //if (gameData.CurrentMode == MdMode.MutiPlayer)//多人模式下向服务器转发庄家顺序
        //{
        //    recData.player.Name = gameData.Landlord;
        //    clientService.SendDataToServer(new RemoteMsg(recData));//调用服务，发送命令和数据
        //}


        dispatcher.Dispatch(ViewConst.ShowBaseCards_Back);//显示底牌背面
        dispatcher.Dispatch(ViewConst.UpdateStatus_Base, gameData.PlayerSelf.Score);//显示玩家分数
        //dispatcher.Dispatch(ViewConst.UpdateStatus_SeYangTip, gameData.TipSeyang());//提示玩家色样组合
    }
}