using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player4_Mediator : EventMediator
{
    [Inject]
    public Player4_View View { get; set; }
    public override void OnRegister()
    {
        View.Init();
        //View.UpdateCards(17);
        dispatcher.AddListener(ViewConst.UpdatePlayer4Cards, CallBack_UpdatePlayer4Cards);
        dispatcher.AddListener(ViewConst.ShowPlayer4Msg, CallBack_ShowPlayer4Msg);
        dispatcher.AddListener(ViewConst.SwitchTimer_Player4, CallBack_SwitchTimer_Player4);
        dispatcher.AddListener(ViewConst.UpdatePlayer4Name, CallBack_UpdatePlayer4Name);
        dispatcher.AddListener(ViewConst.UpdatePlayer4Identiy, CallBack_UpdatePlayerIdentity);
        dispatcher.AddListener(ViewConst.UpdateWinCards4, CallBack_UpdateWinCards);
        dispatcher.AddListener(ViewConst.ShowChongCards4, CallBack_ShowChongCards);
    }
    private void CallBack_UpdatePlayerIdentity(IEvent evt)
    {
        View.UpdatePlayerIdentity((Sprite)evt.data);
    }
    private void CallBack_SwitchTimer_Player4(IEvent evt)
    {
        View.SwitchTimer((bool)evt.data);
    }
    private void CallBack_UpdatePlayer4Name(IEvent evt)
    {
        View.UpdatePlayerName((string)evt.data);
    }
    private void CallBack_UpdatePlayer4Cards(IEvent evt)
    {
        StartCoroutine(View.UpdateCards((int)evt.data, ViewConst.CardDelay));
    }
    private void CallBack_ShowPlayer4Msg(IEvent evt)
    {
        View.UpdateMsg((string)evt.data, ViewConst.MsgClearDelay);
    }

    private void CallBack_UpdateWinCards(IEvent evt)
    {
        View.UpdateWinCards(evt.data as Card[]);
    }

    private void CallBack_ShowChongCards(IEvent evt)
    {
        View.ShowChongCards(evt.data as Card[]);
    }
    public override void OnRemove()
    {
        dispatcher.RemoveListener(ViewConst.UpdatePlayer4Cards, CallBack_UpdatePlayer4Cards);
        dispatcher.RemoveListener(ViewConst.ShowPlayer4Msg, CallBack_ShowPlayer4Msg);
        dispatcher.RemoveListener(ViewConst.SwitchTimer_Player4, CallBack_SwitchTimer_Player4);
        dispatcher.RemoveListener(ViewConst.UpdatePlayer4Name, CallBack_UpdatePlayer4Name);
        dispatcher.RemoveListener(ViewConst.UpdatePlayer4Identiy, CallBack_UpdatePlayerIdentity);
        dispatcher.RemoveListener(ViewConst.UpdateWinCards4, CallBack_UpdateWinCards);
        dispatcher.RemoveListener(ViewConst.ShowChongCards4, CallBack_ShowChongCards);
    }
}
