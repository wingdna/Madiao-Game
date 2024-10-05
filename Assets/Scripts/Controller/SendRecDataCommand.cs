using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Model;
using strange.extensions.command.impl;

public class SendRecDataCommand : EventCommand
{
    [Inject]
    public IClientService clientService { get; set; }
    [Inject]
    public IMdData gameData { get; set; }
    public override void Execute()
    {
        RemoteCMD_Data recData = evt.data as RemoteCMD_Data;
        //if (string.IsNullOrEmpty(recData.player.Name) )
        recData.player = gameData.PlayerSelf;
        //bool isEnd = false;


        if (recData.cmd == RemoteCMD_Const.Discards)
        {
            recData.cards = gameData.SelectedCards.ToArray();
            Debug.Log("要出的牌:" + LitJson.JsonMapper.ToJson(recData.cards));
            Debug.Log("上次出牌人：" + gameData.LastPlayer);
            if (recData.player.Name == gameData.PlayerSelf.Name    )
            {
                /* SoundManager.Instance.PlaySound("SoundEffect/DisError");
                 dispatcher.Dispatch(ViewConst.DiscardFail);
                 return;//出牌失败
                */
                //捉牌检查 可急捉 急捉后不能提万千
                if (gameData.CheckCatchRush(recData.cards[0]))//闲家急捉闲家
                {
                    dispatcher.Dispatch(ViewConst.UpdateStatus_TitleTip, StringFanConst.TipDiserror);
                    dispatcher.Dispatch(ViewConst.UpdateStatus_SeYangTip, StringFanConst.CatchErr);//提示玩家滅牌錯誤
                    SoundManager.Instance.PlaySound("SoundEffect/MieError");
                    dispatcher.Dispatch(ViewConst.DiscardFail);
                    return;
                }
                //灭牌检查
                int recode = gameData.CheckMieCard(recData.cards[0]);
                if (recode > 0)
                {
                    string strError = "";

                    switch (recode)
                    {
                        case 1://未灭十
                            strError = StringFanConst.MieShiOrder;
                            break;
                        case 2://未灭生
                            strError = StringFanConst.MieShengOrder;
                            break;
                        case 3://十下滅孤十
                            strError = StringFanConst.MieShiOnlyOne;
                            break;
                        case 4:
                            strError = StringFanConst.MieErrHave20;
                            break;
                        case 5:
                            strError = StringFanConst.MieErrQian;
                            break;
                        case 6:
                            strError = StringFanConst.MieErrLastShi;
                            break;
                        case 7:
                            strError = StringFanConst.MieShiByShi;
                            break;
                    }
                    
                   
                    dispatcher.Dispatch(ViewConst.UpdateStatus_TitleTip, StringFanConst.TipDiserror);                    
                    dispatcher.Dispatch(ViewConst.UpdateStatus_SeYangTip, strError);//提示玩家滅牌錯誤
                    SoundManager.Instance.PlaySound("SoundEffect/MieError");
                    dispatcher.Dispatch(ViewConst.DiscardFail);
                    return;
                }

                //首發 发牌检查
                if (gameData.CurrentDiscards4.Count < 1)
                {
                    string strError = "";
                    gameData.firstStatus = gameData.CheckFirstCard(recData.cards[0],
                                                                    gameData.HandCards, 
                                                                    gameData.PlayerSelf.winCards,
                                                                    gameData.PlayerSelf.isBai);
                    switch (gameData.firstStatus)
                    {
                        case FirstStatus.Flower3Not:
                            strError = StringFanConst.Flower3Not;
                            break;
                        case FirstStatus.WuShangGaoBai:
                            strError = StringFanConst.WuShangGaoBai;
                            break;
                        case FirstStatus.LordFirst:
                            strError = StringFanConst.LordFirst;
                            break;
                        case FirstStatus.TwoDoorOpen:
                            strError = StringFanConst.TwoDoorOpen;
                            break;
                        case FirstStatus.TiJinWanQian:
                            strError = StringFanConst.TiJinWanQian;
                            break;
                        case FirstStatus.DaoTiQianWan:
                            strError = StringFanConst.DaoTiQianWan;
                            break;
                        case FirstStatus.TiWan5Zhuo:
                            strError = StringFanConst.TiWan5Zhuo;
                            break;
                        case FirstStatus.ChuangQian:
                            strError = StringFanConst.ChuangQian;
                            break;
                        case FirstStatus.JiZhuoTiWan:
                            strError = StringFanConst.JiZhuoTiWan;
                            break;
                        case FirstStatus.KaiSan:
                            strError = StringFanConst.KaiSan;
                            break;
                        case FirstStatus.LastOneSuit:
                            strError = StringFanConst.LastOneSuit;
                            break;
                    }

                    if (!string.IsNullOrEmpty(strError))
                    {
                        dispatcher.Dispatch(ViewConst.UpdateStatus_TitleTip, StringFanConst.TipDiserror);
                        dispatcher.Dispatch(ViewConst.UpdateStatus_SeYangTip, strError);//提示玩家冲出色样组合
                        SoundManager.Instance.PlaySound("SoundEffect/MieError");
                        dispatcher.Dispatch(ViewConst.DiscardFail);
                        return;
                    }
                    gameData.Full4Door(recData.cards[0].Suit,recData.player.Name);
                }
            }


            if ((gameData.GetCardsType(recData.cards) != CardsTypeMdFan.Mussy //不能是杂牌
                || gameData.PlayerSelf.Name.Equals(gameData.LastPlayer))//上一次是自己
                && gameData.CurrentDiscards4.Count <= 4)
            {              

                dispatcher.Dispatch(ViewConst.DiscardSuccess);//出牌成功
                gameData.RestCardNum -= recData.cards.Length;//更新剩余牌数      
               
            }
            else
            {

                dispatcher.Dispatch(ViewConst.DiscardFail);
                return;//出牌失败

            }          

        }


        gameData.SelectedCards = new System.Collections.Generic.List<Card>();//清空
        
        if (gameData.CurrentMode == MdMode.SinglePlayer)
        {
            switch (recData.cmd)
            {
                case RemoteCMD_Const.ReturnLobby:
                    dispatcher.Dispatch(NotificationConst.Noti_ShowGameLobbyUI);
                    break;
                case RemoteCMD_Const.ReturnRoom:
                    dispatcher.Dispatch(NotificationConst.Noti_ContinueGame);
                    break;              
            }

        }
    
        clientService.SendDataToServer(new RemoteMsg(recData));//调用服务，发送命令和数据       

    }


    private bool IsVaildDiscard(List<Card> list, Card card)
    {
        if (card.Suit == PokerConst.Spade &&    //出牌为十子
            !gameData.FirstDiscards.Exists(x => x.Suit == PokerConst.Spade) &&  //之前无人出过十子
            (   list.Exists(c => c.Suit != PokerConst.Spade //三门未尽 非尽手十子
            && !CardSeyang.Shang.ContainsKey(c.SValue()) )   )//存在贯索文且不为赏
            )
        {
            if ((gameData.Baijia != 0 && gameData.CurrentMode == MdMode.SinglePlayer) ||    //非百老之家
                 (gameData.Baijia < 0 && gameData.CurrentMode == MdMode.MutiPlayer))
                return false;
            else
                return true;
        }
        else
            return true;
    }
}