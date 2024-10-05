﻿using UnityEngine;
using strange.extensions.command.impl;

public class StartPVPCommand : EventCommand
{
    [Inject]
    public IMdData gameData { get; set; }
    public override void Execute()
    {
        gameData.CurrentMode = MdMode.MutiPlayer;//模式改为多人
        RemoteCMD_Data recData = new RemoteCMD_Data();
        recData.cmd = RemoteCMD_Const.Match;
        recData.player = gameData.PlayerSelf;
        dispatcher.Dispatch(NotificationConst.Noti_SendRecData, recData);//开始多人模式
    }
}