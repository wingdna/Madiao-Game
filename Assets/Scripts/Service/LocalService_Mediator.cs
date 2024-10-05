﻿using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;


public class LocalService_Mediator : EventMediator
{
    [Inject]
    public LocalService_View View { get; set; }
    public override void OnRegister()
    {
        View.Init();
        View.dispatcher.AddListener(ServiceConst.Service_MatchSuccess, Callback_MatchSuccess);
        View.dispatcher.AddListener(ServiceConst.Service_BaseCards, Callback_BaseCards);
        View.dispatcher.AddListener(ServiceConst.Service_CallLandlord, Callback_CallLandlord);
        View.dispatcher.AddListener(ServiceConst.Service_Claim, Callback_Claim);
        View.dispatcher.AddListener(ServiceConst.Service_DealCards, Callback_DealCards);
        View.dispatcher.AddListener(ServiceConst.Service_Discard, Callback_Discards);
        View.dispatcher.AddListener(ServiceConst.Service_ViewResult, Callback_ViewResult);
        View.dispatcher.AddListener(ServiceConst.Service_GameTurn, Callback_GameTurn);
        View.dispatcher.AddListener(ServiceConst.Service_NotCall, Callback_NotCall);
        View.dispatcher.AddListener(ServiceConst.Service_NotClaim, Callback_NotClaim);
        View.dispatcher.AddListener(ServiceConst.Service_Pass, Callback_Pass);
        View.dispatcher.AddListener(ServiceConst.Service_Player2, Callback_Player2);
        View.dispatcher.AddListener(ServiceConst.Service_Player3, Callback_Player3);
        View.dispatcher.AddListener(ServiceConst.Service_Player4, Callback_Player4);
        View.dispatcher.AddListener(ServiceConst.Service_StartPlayer, Callback_StartPlayer);
        View.dispatcher.AddListener(ServiceConst.Service_Gameover, CallbackGameOver);
        View.dispatcher.AddListener(ServiceConst.Service_Chong, Callback_Chong);
    }



    public override void OnRemove()
    {
        View.dispatcher.RemoveListener(ServiceConst.Service_MatchSuccess, Callback_MatchSuccess);
        View.dispatcher.RemoveListener(ServiceConst.Service_BaseCards, Callback_BaseCards);
        View.dispatcher.RemoveListener(ServiceConst.Service_CallLandlord, Callback_CallLandlord);
        View.dispatcher.RemoveListener(ServiceConst.Service_Claim, Callback_Claim);
        View.dispatcher.RemoveListener(ServiceConst.Service_DealCards, Callback_DealCards);
        View.dispatcher.RemoveListener(ServiceConst.Service_Discard, Callback_Discards);
        View.dispatcher.RemoveListener(ServiceConst.Service_ViewResult, Callback_ViewResult);
        View.dispatcher.RemoveListener(ServiceConst.Service_GameTurn, Callback_GameTurn);
        View.dispatcher.RemoveListener(ServiceConst.Service_NotCall, Callback_NotCall);
        View.dispatcher.RemoveListener(ServiceConst.Service_NotClaim, Callback_NotClaim);
        View.dispatcher.RemoveListener(ServiceConst.Service_Pass, Callback_Pass);
        View.dispatcher.RemoveListener(ServiceConst.Service_Player2, Callback_Player2);
        View.dispatcher.RemoveListener(ServiceConst.Service_Player3, Callback_Player3);
        View.dispatcher.RemoveListener(ServiceConst.Service_Player4, Callback_Player4);
        View.dispatcher.RemoveListener(ServiceConst.Service_StartPlayer, Callback_StartPlayer);
        View.dispatcher.RemoveListener(ServiceConst.Service_Gameover, CallbackGameOver);
        View.dispatcher.RemoveListener(ServiceConst.Service_Chong, Callback_Chong);
    }
    private void CallbackGameOver(IEvent evt)
    {
        dispatcher.Dispatch(NotificationConst.Noti_GameOver, evt.data);
    }
    private void Callback_MatchSuccess()
    {
        dispatcher.Dispatch(NotificationConst.Noti_MatchSuccess);
    }
    private void Callback_DealCards(IEvent evt)
    {
        dispatcher.Dispatch(NotificationConst.Noti_DealCards, evt.data);
    }
    private void Callback_BaseCards(IEvent evt)
    {
        dispatcher.Dispatch(NotificationConst.Noti_BaseCards, evt.data);
    }
    private void Callback_StartPlayer(IEvent evt)
    {
        dispatcher.Dispatch(NotificationConst.Noti_StartPlayer, evt.data);
    }
    private void Callback_Player2(IEvent evt)
    {
        dispatcher.Dispatch(NotificationConst.Noti_Player2, evt.data);
    }
    private void Callback_Player3(IEvent evt)
    {
        dispatcher.Dispatch(NotificationConst.Noti_Player3, evt.data);
    }
    private void Callback_Player4(IEvent evt)
    {
        dispatcher.Dispatch(NotificationConst.Noti_Player4, evt.data);
    }
    private void Callback_Pass(IEvent evt)
    {
        dispatcher.Dispatch(NotificationConst.Noti_Pass, evt.data);
    }
    private void Callback_Discards(IEvent evt)
    {
        dispatcher.Dispatch(NotificationConst.Noti_Discard, evt.data);
    }
    private void Callback_Chong(IEvent evt)
    {
        dispatcher.Dispatch(NotificationConst.Noti_Chong, evt.data);
    }
    private void Callback_ViewResult(IEvent evt)
    {
        dispatcher.Dispatch(NotificationConst.Noti_ViewResult, evt.data);
    }
    private void Callback_CallLandlord(IEvent evt)
    {
        dispatcher.Dispatch(NotificationConst.Noti_CallLandlord, evt.data);
    }
    private void Callback_NotCall(IEvent evt)
    {
        dispatcher.Dispatch(NotificationConst.Noti_NotCall, evt.data);
    }
    private void Callback_Claim(IEvent evt)
    {
        dispatcher.Dispatch(NotificationConst.Noti_Claim, evt.data);
    }
    private void Callback_NotClaim(IEvent evt)
    {
        dispatcher.Dispatch(NotificationConst.Noti_NotClaim, evt.data);
    }
    private void Callback_GameTurn(IEvent evt)
    {
        dispatcher.Dispatch(NotificationConst.Noti_GameTurn, evt.data);
    }
}