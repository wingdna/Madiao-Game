using UnityEngine;
using Assets.Scripts;
using System.Collections.Generic;
using strange.extensions.command.impl;

public class ViewResultCommand : EventCommand
{
    [Inject]
    public IClientService clientService { get; set; }
    [Inject]
    public IMdData gameData { get; set; }
    public override void Execute()
    {
        clientService.SendDataToServer(new RemoteMsg(new RemoteCMD_Data()
        {
            cmd = RemoteCMD_Const.GamerOver,
            player = new PlayerInfo()
            {
                Name = gameData.LastPlayer == gameData.PlayerSelf.Name ? gameData.Player2.Name :
                                                  gameData.LastPlayer == gameData.Player2.Name ? gameData.Player3.Name :
                                                  gameData.LastPlayer == gameData.Player3.Name ? gameData.Player4.Name :
                                                  gameData.PlayerSelf.Name
            }
        }));//游戏结束




    }
}