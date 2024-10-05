using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Model;
using strange.extensions.command.impl;


public class SendUserinfoCommand : EventCommand
{
    [Inject]
    public IClientService clientService { get; set; }
    [Inject]
    public IMdData gameData { get; set; }
    public override void Execute()
    {
        RemoteCMD_Data recData = evt.data as RemoteCMD_Data;    

        clientService.SendDataToServer(new RemoteMsg(recData));//调用服务，发送命令和数据       

    }
}


