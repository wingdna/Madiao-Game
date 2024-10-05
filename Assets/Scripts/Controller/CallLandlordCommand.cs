﻿using strange.extensions.command.impl;

public class CallLandlordCommand : EventCommand
{
    [Inject]
    public IMdData gameData { get; set; }
    public override void Execute()
    {
        //gameData.CurrentStatus = GameStatus.FightLandlord;
        //gameData.ClaimCount--;
        //int i = gameData.BaseCards[gameData.BaseCards.Count - 1].Value % 4;
        //switch (i)
        //{
        //    case 0:
        //        gameData.Landlord = gameData.PlayerSelf.Name;
        //        break;
        //    case 1:
        //        gameData.Landlord = gameData.Player2.Name;
        //        break;
        //    case 2:
        //        gameData.Landlord = gameData.Player3.Name;
        //        break;
        //    case 3:
        //        gameData.Landlord = gameData.Player4.Name;
        //        break;
        //}
        RemoteCMD_Data recData = evt.data as RemoteCMD_Data;
        if (recData.player.Name.Equals(gameData.Player2.Name))
        {
            dispatcher.Dispatch(ViewConst.ShowPlayer2Msg, StringFanConst.CallLandlord);
        }
        if (recData.player.Name.Equals(gameData.Player3.Name))
        {
            dispatcher.Dispatch(ViewConst.ShowPlayer3Msg, StringFanConst.CallLandlord);
        }
        if (recData.player.Name.Equals(gameData.Player4.Name))
        {
            dispatcher.Dispatch(ViewConst.ShowPlayer4Msg, StringFanConst.CallLandlord);
        }
        //if (recData.player.Name.Equals(gameData.PlayerSelf.Name))
        //{
        //    dispatcher.Dispatch(ViewConst.ChangeOptionMenuMode, OptionMenu_Status.HideAll);
        //}
        gameData.Landlord = recData.player.Name;//地主
        gameData.CurrentStatus = GameStatus.CallLandlord;
        gameData.ClaimCount--;//只能争夺3次
    }
}