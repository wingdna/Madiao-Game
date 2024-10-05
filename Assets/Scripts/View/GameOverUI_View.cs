using UnityEngine;
using System.Collections;
using strange.extensions.mediation.impl;
using UnityEngine.UI;

public class GameOverUI_View : EventView
{

    private Text Result_Txt;//游戏结果
    private Button Btn_Return;//返回大厅
    private Button Btn_Continue;//继续游戏
    private Text Coutinue_Txt;//继续游戏消息
    private GameObject Coutinue_Panel;//继续游戏等待界面
    public IMdData gameData { get; set; }
    public void Init()
    {
        Result_Txt = transform.Find("Result_Txt").GetComponent<Text>();   
        Btn_Return = transform.Find("Btn_Return").GetComponent<Button>();
        Coutinue_Txt = transform.Find("Btn_Continue").GetComponent<Text>();
        Btn_Continue = transform.Find("Btn_Continue").GetComponent<Button>();
        Btn_Return.onClick.AddListener(Click_BtnReturn);//点击事件
        Btn_Continue.onClick.AddListener(Click_BtnContinue);//点击事件

        Text txt_Return = Btn_Return.transform.Find("Text").GetComponent<Text>();
        txt_Return.text = StringFanConst.ReturnLobby;
        Text txt_Continue = Btn_Continue.transform.Find("Text").GetComponent<Text>();
        txt_Continue.text = StringFanConst.CountinueGame;

        Coutinue_Panel = transform.Find("Continue_Panel").gameObject;
        Coutinue_Txt = Coutinue_Panel.transform.Find("Continue_Txt").GetComponent<Text>();
        HideContinuePanel();
    }
    public void UpdateResultMsg(string msg)
    {     
        Result_Txt.text = msg;
    }

    private void Click_BtnReturn()
    {
       // dispatcher.Dispatch(ViewConst.Click_Match);
        dispatcher.Dispatch(ViewConst.Click_ReturnLobby);//返回大厅 //      

    }

    private void Click_BtnContinue()
    {
        Coutinue_Txt.text = StringFanConst.Replaying;
        
        dispatcher.Dispatch(ViewConst.Click_ContinueGame);//继续下一局游戏 //

       // this.gameObject.SetActive(false);
    }

    public void ShowContinuePanel()
    {
        Coutinue_Panel.SetActive(true);
    }

    public void HideContinuePanel()
    {
        Coutinue_Panel.SetActive(false);
    }
}
