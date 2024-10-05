using UnityEngine;
using Assets.Scripts.Model;
using strange.extensions.command.impl;

public class UpdatePlayerIdentityCommand : EventCommand
{
    [Inject]
    public IMdData gameData { get; set; }
    public override void Execute()
    {
        Sprite farmer = Resources.Load<Sprite>("Image/Pokers/Identity_Farmer");
        Sprite landlord = Resources.Load<Sprite>("Image/Pokers/Identity_Landlord");
        Debug.Log(gameData.Landlord);


        if (!string.IsNullOrEmpty(gameData.Landlord))
        {
            if (gameData.Landlord.Equals(gameData.PlayerSelf.Name))//自己是地主
            {
                dispatcher.Dispatch(ViewConst.UpdatePlayerSelfIdentiy, landlord);
                dispatcher.Dispatch(ViewConst.UpdatePlayer2Identiy, farmer);
                dispatcher.Dispatch(ViewConst.UpdatePlayer3Identiy, farmer);
                dispatcher.Dispatch(ViewConst.UpdatePlayer4Identiy, farmer);
                //gameData.RestCardNum += 3;//地主牌数+3
            }
            if (gameData.Landlord.Equals(gameData.Player2.Name))//玩家2是地主
            {
                dispatcher.Dispatch(ViewConst.UpdatePlayerSelfIdentiy, farmer);
                dispatcher.Dispatch(ViewConst.UpdatePlayer2Identiy, landlord);
                dispatcher.Dispatch(ViewConst.UpdatePlayer3Identiy, farmer);
                dispatcher.Dispatch(ViewConst.UpdatePlayer4Identiy, farmer);
                // dispatcher.Dispatch(ViewConst.UpdatePlayer2Cards, 3);//玩家加3张牌
            }
            if (gameData.Landlord.Equals(gameData.Player3.Name))//玩家3是地主
            {
                dispatcher.Dispatch(ViewConst.UpdatePlayerSelfIdentiy, farmer);
                dispatcher.Dispatch(ViewConst.UpdatePlayer2Identiy, farmer);
                dispatcher.Dispatch(ViewConst.UpdatePlayer3Identiy, landlord);
                dispatcher.Dispatch(ViewConst.UpdatePlayer4Identiy, farmer);
                //dispatcher.Dispatch(ViewConst.UpdatePlayer3Cards, 3);//玩家加3张牌
            }
            if (gameData.Landlord.Equals(gameData.Player4.Name))//玩家4是地主
            {
                dispatcher.Dispatch(ViewConst.UpdatePlayerSelfIdentiy, farmer);
                dispatcher.Dispatch(ViewConst.UpdatePlayer2Identiy, farmer);
                dispatcher.Dispatch(ViewConst.UpdatePlayer3Identiy, farmer);
                dispatcher.Dispatch(ViewConst.UpdatePlayer4Identiy, landlord);
                //dispatcher.Dispatch(ViewConst.UpdatePlayer4Cards, 3);//玩家加3张牌
            }
        }

        if (string.IsNullOrEmpty(gameData.Landlord))
        {
            Debug.Log("Init Identity....");
            dispatcher.Dispatch(ViewConst.UpdatePlayerSelfIdentiy, farmer);
            dispatcher.Dispatch(ViewConst.UpdatePlayer2Identiy, farmer);
            dispatcher.Dispatch(ViewConst.UpdatePlayer3Identiy, farmer);
            dispatcher.Dispatch(ViewConst.UpdatePlayer4Identiy, farmer);

        }
    }
}