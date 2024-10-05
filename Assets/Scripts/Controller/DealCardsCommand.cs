using UnityEngine;
using Assets.Scripts.Model;
using strange.extensions.command.impl;

public class DealCardsCommand : EventCommand
{
    [Inject]
    public IMdData gameData { get; set; }
    public override void Execute()
    {
        dispatcher.Dispatch(NotificationConst.Noti_ShowMainGameUI);//显示主界面
        Debug.Log("DealCards:Get HandCards");
        RemoteCMD_Data recData = evt.data as RemoteCMD_Data;
        //升代
        if (gameData.BaseCards != null && gameData.BaseCards.Count > 0)
        {
            Card c = gameData.BaseCards[gameData.BaseCards.Count - 1];
            Card c0 = GameEngine.FindReplaceFace(c);
            recData.cards = GameEngine.ReplaceCard(recData.cards, c0, c);
        }

        gameData.HandCards.Clear();
        gameData.HandCards.AddRange((Card[])recData.cards.Clone());//保存手牌        
        dispatcher.Dispatch(ViewConst.ShowHandCards,(Card[])recData.cards.Clone());//显示手牌
        dispatcher.Dispatch(ViewConst.UpdatePlayer2Cards, 8);//显示玩家2的牌
        dispatcher.Dispatch(ViewConst.UpdatePlayer3Cards, 8);//显示玩家3的牌
        dispatcher.Dispatch(ViewConst.UpdatePlayer4Cards, 8);//显示玩家4的牌

     /*   if (gameData.HandCards.Exists(c => c.Value == 9 && c.Suit.Equals(PokerConst.Spade)))//是否手持百老
            gameData.Baijia = gameData.Players.FindIndex(
            (p) =>
            {
                return p.Name == gameData.PlayerSelf.Name;
            });//找到手持百老家所在的顺序
     */
    }
}