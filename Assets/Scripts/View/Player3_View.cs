using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player3_View : EventView {

    private Image Role_IMG;
    private Image Frame_IMG;
    private Image Identity_IMG;
    private Text Name_Txt;
    private Text CardNum_Txt;
    private Transform CardPos;
    private Transform CardPos2;
    private Transform CardPos3;
    private Text Msg_Txt;
    private Text Timer_Txt;
    private GameObject CardBack;
    public void Init()
    {
        Role_IMG = transform.Find("Role_IMG").GetComponent<Image>();
        Frame_IMG = transform.Find("Frame_IMG").GetComponent<Image>();
        Identity_IMG = transform.Find("Identity_IMG").GetComponent<Image>();
        Name_Txt = transform.Find("Name_Txt").GetComponent<Text>();
        CardNum_Txt = transform.Find("CardNum_Txt").GetComponent<Text>();
        CardPos = transform.Find("CardPos");
        CardPos2 = transform.Find("CardPos2");
        CardPos3 = transform.Find("CardPos3");
        Msg_Txt = transform.Find("Msg_Txt").GetComponent<Text>();
        Timer_Txt = transform.Find("Timer_Txt").GetComponent<Text>();
        CardBack = Resources.Load<GameObject>("Prefabs/Card_Back");
        Msg_Txt.text = "";
        Timer_Txt.text = "";
    }
    public void UpdatePlayerName(string name)
    {
        Name_Txt.text = name;
    }
    public void UpdatePlayerIdentity(Sprite id)
    {
        Identity_IMG.sprite = id;
    }
    public void UpdateMsg(string msg, float time)
    {
        StopCoroutine(ClearMsg(time));
        Msg_Txt.text = msg;
        StartCoroutine(ClearMsg(time));
    }
    private IEnumerator ClearMsg(float time)
    {
        yield return new WaitForSeconds(time);
        Msg_Txt.text = "";
    }
    public void SwitchTimer(bool on_off)
    {
        if (on_off)
        {
            StopCoroutine("ShowTime");
            StartCoroutine("ShowTime");
        }
        else
        {
            StopCoroutine("ShowTime");
            Timer_Txt.text = "";
        }
    }
    public void ResetGame()
    {
        Msg_Txt.text = "";
        for (int i = 0; i < CardPos.childCount; i++)
        {
            Destroy(CardPos.GetChild(i).gameObject);
        }
        for (int i = 0; i < CardPos2.childCount; i++)
        {
            Destroy(CardPos2.GetChild(i).gameObject);
        }
        for (int i = 0; i < CardPos3.childCount; i++)
        {
            Destroy(CardPos3.GetChild(i).gameObject);
        }
    }
    private IEnumerator ShowTime()
    {
        int time = 10;
        while (time > 0)
        {
            Timer_Txt.text = time.ToString();
            yield return new WaitForSeconds(1f);
            time--;
        }
        Timer_Txt.text = "";
    }

    public IEnumerator UpdateCards(int cardCount,float time)
    {
        Debug.Log(cardCount);
        if (cardCount > 0)//为正表示添加新牌
        {
            for(int i = 0; i < cardCount; i++)
            {
                Instantiate(CardBack, CardPos);
                yield return new WaitForSeconds(time);
            }
        }
        else
        {
            //删除牌（出牌）
            if (Mathf.Abs(cardCount) > 1)   //不能出超过一张
                yield return new WaitForSeconds(0);
            for (int i = 0; i < Mathf.Abs(cardCount); i++)
            {
                if (CardPos.childCount <= 0) yield break;
                DestroyImmediate(CardPos.GetChild(0).gameObject);
                yield return new WaitForSeconds(time);
            }
        }
        CardNum_Txt.text = CardPos.childCount.ToString();
    }

    public void UpdateWinCards(Card[] cards)
    {
        for (int i = 0; i < CardPos2.childCount; i++)
        {
            Destroy(CardPos2.GetChild(i).gameObject);
        }
        if (cards != null && cards.Length > 0)
        {
           
            //放到得桌列表
            Sprite[] sprites = LoadSprites(cards);
            GameObject Card = Resources.Load<GameObject>("Prefabs/Card");
            for (int i = 0; i < sprites.Length; i++)
            {                
                GameObject c = Instantiate(Card, CardPos2);
                c.GetComponent<Image>().sprite = sprites[i];
            }
        }
    }

    public void ShowChongCards(Card[] cards)
    {
        for (int i = 0; i < CardPos3.childCount; i++)
        {
            Destroy(CardPos3.GetChild(i).gameObject);
        }
        if (cards != null && cards.Length > 0)
        {

            //放到得桌列表
            Sprite[] sprites = LoadSprites(cards);
            GameObject Card = Resources.Load<GameObject>("Prefabs/Card");
            for (int i = 0; i < sprites.Length; i++)
            {
                GameObject c = Instantiate(Card, CardPos3);
                c.GetComponent<Image>().sprite = sprites[i];
            }
        }
    }
    private Sprite[] LoadSprites(Card[] cards)
    {
        Sprite[] array = new Sprite[cards.Length];
        for (int i = 0; i < cards.Length; i++)
        {
            //获取图片名字
            string name = global::Card.GetSpriteName(cards[i]);
            //加载图片
            Debug.Log(name);
            array[i] = Resources.Load<Sprite>("Image/Pokers/" + name);
        }
        return array;
    }

}
