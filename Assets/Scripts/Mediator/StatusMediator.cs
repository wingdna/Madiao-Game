using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusMediator : EventMediator {
    [Inject]
	public Status_View View { get; set; }
    public override void OnRegister()
    {
        View.Init();
        dispatcher.AddListener(ViewConst.UpdateStatus_Base, CallBack_UpdateStatus_Base);
        dispatcher.AddListener(ViewConst.UpdateStatus_Mutiple, CallBack_UpdateStatus_Mutiple);
        dispatcher.AddListener(ViewConst.UpdateStatus_SeYangTip, CallBack_UpdateStatus_SeYangTip);
        dispatcher.AddListener(ViewConst.UpdateStatus_TitleTip, CallBack_UpdateStatus_TitleTip);
        dispatcher.AddListener(ViewConst.UpdateStatus_OpenDoors, CallBack_UpdateStatus_OpenDoors);
    }
    private void CallBack_UpdateStatus_Base(IEvent evt)
    {
        View.UpdateBase((int)evt.data);
    }
    private void CallBack_UpdateStatus_Mutiple(IEvent evt)
    {
        View.UpdateMutiple((int)evt.data);
    }
    private void CallBack_UpdateStatus_SeYangTip(IEvent evt)
    {
        View.UpdateSeYangTip((string)evt.data);
    }

    private void CallBack_UpdateStatus_TitleTip(IEvent evt)
    {
        View.UpdateTipTitle((string)evt.data);
    }

    private void CallBack_UpdateStatus_OpenDoors(IEvent evt)
    {
        View.UpdateOpenDoorsValve((string)evt.data);
    }

    public override void OnRemove()
    {
        dispatcher.RemoveListener(ViewConst.UpdateStatus_Base, CallBack_UpdateStatus_Base);
        dispatcher.RemoveListener(ViewConst.UpdateStatus_Mutiple, CallBack_UpdateStatus_Mutiple);
        dispatcher.RemoveListener(ViewConst.UpdateStatus_SeYangTip, CallBack_UpdateStatus_SeYangTip);
        dispatcher.RemoveListener(ViewConst.UpdateStatus_TitleTip, CallBack_UpdateStatus_TitleTip);
        dispatcher.RemoveListener(ViewConst.UpdateStatus_OpenDoors, CallBack_UpdateStatus_OpenDoors);
    }
}
