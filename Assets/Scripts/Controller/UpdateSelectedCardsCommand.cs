using UnityEngine;
using strange.extensions.command.impl;
using System.Collections.Generic;
using Assets.Scripts.Model;

public class UpdateSelectedCardsCommand : EventCommand
{
    [Inject]
    public IMdData gameData { get; set; }
    public override void Execute()
    {
        Debug.Log("Update Selected Cards:"+LitJson.JsonMapper.ToJson(evt.data as Card[]));
        Card[] cards = evt.data as Card[];//这次选择的牌
        gameData.SelectedCards.Clear();
        gameData.SelectedCards.AddRange((Card[])cards.Clone());

        dispatcher.Dispatch(ViewConst.UpdateStatus_TitleTip, StringFanConst.TipSeyang);//色样标题       
        //string stip = gameData.TipSeyang();
        //if (!string.IsNullOrEmpty(stip) )
        dispatcher.Dispatch(ViewConst.UpdateStatus_SeYangTip, gameData.TipSeyang());//提示玩家色样组合
    }
}