using UnityEngine;
using strange.extensions.command.impl;
using System.Collections.Generic;
using Assets.Scripts.Model;
public class TipsCommand : EventCommand
{
    [Inject]
    public IMdData gameData { get; set; }
    public override void Execute()
    {
        DisRole iRols = DisRole.FirstCard;
        int firstTurn = gameData.TheFirstTurn,
            CurTurn = gameData.CurrentTurn;
        //出牌阶段

        if (gameData.LastPlayer == gameData.Players[CurTurn].Name || //上次是自己
             string.IsNullOrEmpty(gameData.LastPlayer) ||
            gameData.CurrentDiscards.Count <= 0)//自己第一个出牌
        {
            firstTurn = CurTurn;
            
        }
        iRols = gameData.getRoleCard(firstTurn, CurTurn);

        gameData.HandCards.AddRange((Card[])gameData.SelectedCards.ToArray().Clone());
        List<Card> selectCards = gameData.GetEnabledDisCards(
            new List<Card>((Card[])gameData.HandCards.ToArray().Clone()),
            gameData.PlayerSelf.Name , iRols );
        if (selectCards.Count == 1)
        {
            gameData.SelectedCards.Clear();
            gameData.SelectedCards.AddRange((Card[])selectCards.ToArray().Clone());
            //更新选择的牌的数据
            dispatcher.Dispatch(NotificationConst.Noti_UpdateSelectedCards, (Card[])selectCards.ToArray().Clone());
            //更显显示UI
            dispatcher.Dispatch(ViewConst.ShowTipsCards, (Card[])selectCards.ToArray().Clone());
         }
        //List<CardType> lt = GameEngine.GetSeyangTip(gameData.HandCards);
        //string tip = "色样:";
        //if (lt.Count > 0)
        //{
        //    foreach(CardType ct in lt)
        //    {if(ct.Name!=CardsTypeMdFan.BaQuan && ct.Name!=CardsTypeMdFan.QiDiao && ct.Name!=CardsTypeMdFan.LiuDiao)
        //        tip+= GameEngine.ConvertMdName(ct.CardKey) + " " + ct.Name + " " + ct.Jifen.ToString() + "注  ";
        //    }
        //}
        //dispatcher.Dispatch(ViewConst.ShowDiscardMsg, tip);//显示牌型
    }
}
