using strange.extensions.mediation.impl;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using Assets.Scripts.Model;


public class CurrentCard_View : EventView {
    private GameObject Card;
    private Transform CardPos,CardPos2;
 
    public void Init()
    {
        Card = Resources.Load<GameObject>("Prefabs/Card");
        CardPos = transform.Find("CardPos");
        CardPos2 = transform.Find("CardPos2");
      
    }
    public void UpdateCards(Card[] cards)
    {
        RemoveAllCards();
        Sprite[] sprites = LoadSprites(cards);
        //装载所有卡片对象
        for (int i = 0; i < sprites.Length; i++)
        {
            GameObject c = Instantiate(Card, CardPos);
            
            c.GetComponent<Image>().sprite = sprites[i];
        }
    }

    public void UpdateBaseCardsBefore(Card[] cards)
    {
        RemoveAllCards();

        Sprite[] sprites = LoadSprites(cards);
        GameObject back = Resources.Load<GameObject>("Prefabs/Card_Back");

        //装载所有马吊底牌卡片对象
        for (int i = 0; i < sprites.Length; i++)
        {
            if (i < sprites.Length - 1)
                Instantiate(back, CardPos2);
            else
            {
                GameObject c = Instantiate(Card, CardPos2);
                c.GetComponent<Image>().sprite = sprites[sprites.Length - 1];
            }
        }

    }

    public void UpdateBaseCardsChong(Card[] cards)
    {
        RemoveAllCards();

        Sprite[] sprites = LoadSprites(cards);
        GameObject back = Resources.Load<GameObject>("Prefabs/Card_Back");

        //装载所有马吊底牌卡片对象
        for (int i = 0; i < sprites.Length; i++)
        {
            if (i == sprites.Length - 1)
                Instantiate(back, CardPos2);
            else
            {
                GameObject c = Instantiate(Card, CardPos2);
                c.GetComponent<Image>().sprite = sprites[i];
            }
        }

    }

    public void UpdateDisCards(Card[] cards)
    {
        RemoveAllCards();
        List<Card> lst = new List<Card>();
                


        Sprite[] sprites = LoadSprites(cards);
        //装载所有卡片对象
        for (int i = 0; i < sprites.Length; i++)
        {
            for (int j = 0; j < i; j++)
                lst.Add(cards[j]);
            lst.Sort((a, b) => { return a.Suit.Equals(b.Suit) ? b.Value.CompareTo(a.Value) : -1; });  //升序排序

            if (i == 0 || (lst.Count>0&& cards[i].GreatThan( lst[0]) ) )
            {
                GameObject c = Instantiate(Card, CardPos);
                c.GetComponent<Image>().sprite = sprites[i];           
                if (i == sprites.Length -1)
                    SoundManager.Instance.PlaySound("Sound/" + cards[i].MdName());
            }
            else
            {
                GameObject back = Resources.Load<GameObject>("Prefabs/Card_Back");
                //Color color = transform.GetComponent<Image>().color;
                //color.a = 0.5f;
                //transform.GetComponent<Image>().color = color;

                //Transform wallTransform = gameObject.transform.GetChild(i);
                //Color mcolor = wallTransform.gameObject.GetComponent<Renderer>().material.color;
                //mcolor.a = 0.4f;
                //YPTools.SetMaterialRenderingMode(wallTransform.gameObject.GetComponent<Renderer>().material, YPTools.RenderingMode.Transparent);
                //wallTransform.gameObject.GetComponent<Renderer>().material.color = mcolor;

                Instantiate(back, CardPos);
                if (i == sprites.Length -1)
                    SoundManager.Instance.PlaySound("Sound/灭牌");
            }
            lst.Clear();
        }
    }
    private void RemoveAllCards()
    {
        for(int i = 0; i < CardPos.childCount; i++)
        {
            Destroy(CardPos.GetChild(i).gameObject);
        }

        for (int i = 0; i < CardPos2.childCount; i++)
        {
            Destroy(CardPos2.GetChild(i).gameObject);
        }
    }
    public void ShowBackCard(Card[] cards)
    {
        Debug.Log("BaseCardBack");
        RemoveAllCards();
               
        GameObject back= Resources.Load<GameObject>("Prefabs/Card_Back");
        for(int i = 1; i < 8; i++)
        {
            Instantiate(back, CardPos);
        }
    }

    public void ResetGame()
    {

        for (int i = 0; i < CardPos.childCount; i++)
        {
            Destroy(CardPos.GetChild(i).gameObject);
        }
        for (int i = 0; i < CardPos2.childCount; i++)
        {
            Destroy(CardPos2.GetChild(i).gameObject);
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
            array[i] = Resources.Load<Sprite>("Image/Pokers/" + name);
        }
        return array;
    }
}
