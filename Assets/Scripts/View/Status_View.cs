using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status_View : EventView {
    
    private Text Mutiple_Txt;
    private Text Base_Txt;
    private Text Tip_Txt;
    private Text SeYangTip_Txt;
    private Text OpenDoors_Txt;



    public void Init()
    {
        transform.Find("Tex_M").GetComponent<Text>().text = StringFanConst.MultiBase;
        transform.Find("Text_B").GetComponent<Text>().text = StringFanConst.BaseGold;
        transform.Find("DoorTitle_Txt").GetComponent<Text>().text = StringFanConst.OpenDoors;
        Mutiple_Txt = transform.Find("Mutiple_Txt").GetComponent<Text>();
        Base_Txt = transform.Find("Base_Txt").GetComponent<Text>();
        SeYangTip_Txt = transform.Find("SeYangTip_Txt").GetComponent<Text>();
        Tip_Txt = transform.Find("Tip_Txt").GetComponent<Text>();
        OpenDoors_Txt = transform.Find("DoorValve_Txt").GetComponent<Text>();
    }
    public void ResetGame()
    {
        Base_Txt.text = "1000";
        Mutiple_Txt.text = "1";
        SeYangTip_Txt.text = "";
        Tip_Txt.text = StringFanConst.TipSeyang;
        OpenDoors_Txt.text = "";
    }
    public void UpdateBase(int msg)
    {
        Base_Txt.text = msg.ToString();
    }
    public void UpdateMutiple(int msg)
    {
        Mutiple_Txt.text = msg.ToString();
    }

    public void UpdateSeYangTip(string msg)
    {
       if (SeYangTip_Txt.text != msg)
            SeYangTip_Txt.text = msg;
    }

    public void UpdateTipTitle(string msg)
    {
    
          Tip_Txt.text = msg;
    }

    public void UpdateOpenDoorsValve(string msg)
    {
        if (OpenDoors_Txt.text != msg)
            OpenDoors_Txt.text = msg;
    }
}
