using UnityEngine;
using Assets.Scripts;
using System.Collections.Generic;
using strange.extensions.command.impl;

public class PassCommand :EventCommand 
{
    [Inject]
    public IMdData gameData { get; set; }
    public override void Execute()
    {
        Debug.Log("Pass Command is Executed");
        RemoteCMD_Data recData = evt.data as RemoteCMD_Data;
        //if (recData.player.Name.Equals(gameData.Player2.Name))
        //{
        //    dispatcher.Dispatch(ViewConst.ShowPlayer2Msg, StringFanConst.Pass);
        //}
        //if (recData.player.Name.Equals(gameData.Player3.Name))
        //{
        //    dispatcher.Dispatch(ViewConst.ShowPlayer3Msg, StringFanConst.Pass);
        //}
        //if (recData.player.Name.Equals(gameData.Player4.Name))
        //{
        //    dispatcher.Dispatch(ViewConst.ShowPlayer4Msg, StringFanConst.Pass);
        //}

        gameData.HandCards.AddRange((Card[])gameData.SelectedCards.ToArray().Clone());
        List<Card> selectCards = gameData.GetEnabledDisCards(
            new List<Card>((Card[])gameData.HandCards.ToArray().Clone())
            , gameData.PlayerSelf.Name,DisRole.CatchCard);
        if (selectCards.Count == 1)
        {
            gameData.SelectedCards.Clear();
            gameData.SelectedCards.AddRange((Card[])selectCards.ToArray().Clone());
            //更新选择的牌的数据
            dispatcher.Dispatch(NotificationConst.Noti_UpdateSelectedCards, (Card[])selectCards.ToArray().Clone());
            //更显显示UI
            dispatcher.Dispatch(ViewConst.ShowTipsCards, (Card[])selectCards.ToArray().Clone());

            recData = new RemoteCMD_Data();
            recData.cmd = RemoteCMD_Const.Discards;
            recData.player = gameData.PlayerSelf;
            recData.cards = (Card[])selectCards.ToArray().Clone();
            dispatcher.Dispatch(NotificationConst.Noti_Discard, recData);
        }


       

    }
}