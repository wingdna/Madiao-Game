using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Assets.Scripts.Model
{
    public class EngineTool
    {
        /// <summary>
        /// 
        ///  /// 格式化扑克牌字符串
        /// 将带有花色、无顺序的扑克牌转换成有花色、统一顺序。主要用于牌型检测。
        /// 举例：
        /// 
        /// 3*H-4*S-7*H-6*D-5*S
        /// 格式化后：F
        /// 5*S-4*S-7*H-3*H-6*D

        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// 

        public static string FormatCardStrmd(string input, bool clubfirst = false)
        {
            // 按照事先定义的规则 进行降序排序
            string output = null; ;
            List<string> withColors = input.Split('-').ToList();

            withColors.Sort((a, b) =>
            {
                int posa = a.IndexOf("*"), posb = b.IndexOf("*");
                int index_acolor = CardSeyang.mdSuit.IndexOf(a.Substring(0, posa));
                int index_bcolor = CardSeyang.mdSuit.IndexOf(b.Substring(0, posb));
                int value_a = CardSeyang.CardValuesmd.IndexOf(a.Substring(posa + 1));
                int value_b = CardSeyang.CardValuesmd.IndexOf(b.Substring(posb + 1));


                if (!clubfirst)
                {
                    if (index_acolor > index_bcolor
                    || (index_acolor == index_bcolor && value_a > value_b))
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    if (index_acolor < index_bcolor
                  || (index_acolor == index_bcolor && value_a < value_b))
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
            });

            output = string.Join("-", withColors.ToArray());

            return output;
        }


        /// <summary>
        /// 计算扑克牌中某个指定卡牌出现的次数，input不带花色
        /// </summary>
        /// <param name="input"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static int CountInCardStr(String input, String target)
        {
            int count = 0;
            input.Split('-').ToList().ForEach((a) =>
            {
                if (a.Equals(target))
                {
                    count++;
                }
            });

            return count;
        }

        /// <summary>
        /// 重复指定卡牌N次，中间用-隔开
        /// </summary>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static String RepeatCard(int index, int count)
        {
            if (count <= 0 || index < 0 || index > 14)
            {
                return null;
            }

            String card_str = "";
            for (int i = 0; i < count; i++)
            {
                card_str += CardSeyang.CardValuesmd[index] + "-";
            }

            card_str = card_str.TrimEnd('-');

            return card_str;
        }

        public static string SuitToMd(string suit)
        {
            switch (suit)
            {
                case PokerConst.Club: return MDNameFanConst.Wen;
                case PokerConst.Diamond: return MDNameFanConst.Suo;
                case PokerConst.Heart: return MDNameFanConst.Guan;
                case PokerConst.Spade: return MDNameFanConst.Shi;
                default: return null;
            }


        }

        public static bool ExistCard(List<Card> lc, Card card)
        {
            if (lc.Exists(c => c.Suit == card.Suit &&
                             c.Value == card.Value))
                return true;
            else
                return false;
        }

        public static bool isTrueCard(List<Card> lc, Card card, Card faceCard = null, List<Card> handCardsAll = null)
        {
            if (faceCard != null && faceCard.Value < GetShangValve(faceCard.Suit, true) - 1)
                lc.Add(faceCard);//加入面张
            if (handCardsAll != null && handCardsAll.Count > 0)
                lc.AddRange(handCardsAll);

            int shangValve = GetShangValve(card.Suit, true);
            List<Card> sameSuitCards = new List<Card>();
            List<Card> tmpL = lc.Where(c => c.Suit == card.Suit).ToList();
            //生成该种花色的全部牌
            for (int j = 1; j <= shangValve; j++)
            {
                sameSuitCards.Add(new Card(card.Suit, j));//每种花色生成9-11张
            }

            //去除已出现过的牌
            //tmpL = tmpL.GroupBy(x => new { x.Suit }).Select(y => y.First()).ToList();
            sameSuitCards = sameSuitCards.Except(tmpL).ToList();

            if (!sameSuitCards.Exists(c => c.Value > card.Value))
                return true;
            else
                return false;

        }

        public static bool isAllTrueCard(List<Card> lc, List<Card> cardsHand, Card faceCard = null)
        {
            foreach (Card c in cardsHand)
            {
                if (!isTrueCard(lc, c, faceCard))
                    return false;
            }
            return true;
        }

        public static bool isCloseShang(List<Card> allDis, List<Card> lcards, string suit = PokerConst.Spade, bool blord = false)
        {
            var vtmp = lcards.GroupBy(x => new { x.Suit }).Select(y => y.First());
            // vtmp = vtmp.Distinct();
            int nsuits = vtmp.ToList().Count;

            /*
             * if (wiiDis.Suit != PokerConst.Spade && blord) //十子以外莊家隨便發
                 return true;
             if (wiiDis.Suit == suit1 ||
                 wiiDis.Suit == suit2)
                 return true;
            */

            List<Card> xCards = lcards.Where(c => c.Suit == suit).ToList();
            if (nsuits == 2 && xCards.Count > 0)
            {

                List<Card> otherCards = lcards.Except(xCards).ToList();
                //  只有两门  十子和另一门
                /* bool bshang = otherCards.Exists(c => c.Value == GetShangValve(c.Suit));
                 bool bjian = otherCards.Exists(c => c.Value == GetShangValve(c.Suit) - 1);
                 bool bjiandis = allDis.Exists(c => c.Value == GetShangValve(otherCards[0].Suit) - 1);
                 int nshi = xCards.Count(c => c.Value != PokerConst.JianValve &&
                                       c.Suit == PokerConst.Spade);

                 if (bshang && !bjian && !bjiandis && nshi > 0 && otherCards.Count > 1)
                     return true;*/
                if (otherCards.Exists(c => c.Value == GetShangValve(c.Suit)) &&      //有賞
                    !otherCards.Exists(c => c.Value == GetShangValve(c.Suit) - 1) && //無肩
                    otherCards.Count > 1 &&                                         //有青或趣
                    !allDis.Exists(c => c.Value == GetShangValve(otherCards[0].Suit) - 1 &&
                                        c.Suit == otherCards[0].Suit) && //賞下本肩未打出
                    xCards.Count(c => c.Value != PokerConst.JianValve &&
                                      c.Suit == PokerConst.Spade) > 0)  //有千僧以外十子                   
                    return true;
            }
            /* 
             if (lcards.Count(c => c.Suit == suit1) == 0 ||
                 lcards.Count(c => c.Suit == suit2) == 0)
             { 

                 if (nsuits == 3 && spadeCards.Count > 0 &&
                     (!lcards.Exists(c => c.Suit == suit1) ||
                      !lcards.Exists(c => c.Suit == suit2)))
                 {
                     List<Card> threeCards = lcards.Where(c => c.Suit != PokerConst.Spade &&
                                                               c.Suit != suit1 &&
                                                               c.Suit != suit2).ToList();
                     List<Card> zhengCards = lcards.Where(c => c.Suit == suit1 ||
                                                               c.Suit == suit2).ToList();
                     if (zhengCards.Exists(c => c.Value == GetShangValve(c.Suit)) &&      //有賞
                    !zhengCards.Exists(c => c.Value == GetShangValve(c.Suit) - 1) && //無肩
                    zhengCards.Count > 1 &&                                         //有青或趣
                    !allDis.Exists(c => c.Value == GetShangValve(c.Suit) - 1)) //賞下本肩未打出                                                                             
                     {
                         if (lcards.Exists(c => c.Suit == PokerConst.Spade &&
                                             c.Value == PokerConst.ShangValve - 1))
                             return true;
                         else
                             return false;
                     }
                     return false;

                 }
            return false;
             }*/
            return false;
        }


        public static bool isLastSuit1(Card card, List<Card> HandCards, List<Card> mycards, List<Card> FirstDis, List<Card> allDis, List<string> FourDoor)
        {
            if (HandCards.Count < 8 && card.Suit != PokerConst.Spade)
            {
                List<Card> firstSameSuuits = FirstDis.Intersect(mycards).ToList();//发牌集合与到手牌交集=>自己发过的牌
                if (firstSameSuuits.Count > 0)
                {
                    if (firstSameSuuits[0].Suit == card.Suit &&     //發張為原門
                        HandCards.Count(c => c.Suit == card.Suit) == 1 && //只剩一張原門
                        !HandCards.Exists(c => c.Value == PokerConst.JiValve && c.Suit == card.Suit) &&//手上無同門極
                        !allDis.Exists(c => c.Value == PokerConst.JiValve && c.Suit == card.Suit))//同門極未出過
                    {
                        if (FourDoor.Count >= 2 &&
                            (card.Suit == FourDoor[0] || card.Suit == FourDoor[1]))//首发张为熟门
                        {
                            if (HandCards.Count(c => c.Suit == FourDoor[0] ||
                                                     c.Suit == FourDoor[1]) > 1)  //還有其熟门
                                return true;

                            return false;
                        }

                        if (FourDoor.Count == 1)
                        {
                            if (HandCards.Count(c => c.Suit != FourDoor[0] &&
                                                     c.Suit != PokerConst.Spade) > 0)  //還有其他門
                                return true;

                            return false;
                        }

                    }

                }

            }
            return false;
        }

        public static bool IsSingleShang(List<Card> lc, bool bWan = false)
        {
            int countShangs = 0, countJians = 0;
            if (!bWan)
                countShangs = lc.Count(c => c.Value == GetShangValve(c.Suit, true) &&
                                            c.Suit != PokerConst.Spade);
            else
                countShangs = lc.Count(c => c.Value == GetShangValve(c.Suit, true));
            //千僧不算肩  有肩不算孤赏
            countJians = lc.Count(c => c.Value == GetShangValve(c.Suit, true) - 1 &&
                                       c.Suit != PokerConst.Spade);
            countShangs += countJians;

            return countShangs == 1 ? true : false;
        }

        public static int GetJianOrder(Card card, List<Card> allDis, bool bQian = false)
        {
            int njiansDis = 0;

            if (!bQian && card.Suit == PokerConst.Spade)
                return 0;
            if (card.Value != EngineTool.GetShangValve(card.Suit, true))
                return 0;

            if (bQian)
                njiansDis = allDis.Count(c => c.Value == GetShangValve(c.Suit, true) - 1);
            else
                njiansDis = allDis.Count(c => c.Value == GetShangValve(c.Suit, true) - 1 &&
                                             c.Suit != PokerConst.Spade);

            return njiansDis + 1;
        }


        //获取逼张出牌
        public static Card GetBiCard(Card card, List<Card> hc)
        {
            if (card.Value == GetShangValve(card.Suit))
                return null;

            List<Card> ltmp = new List<Card>(hc.Where(c => c.Suit == card.Suit));
            ltmp.Sort((a, b) => a.Value.CompareTo(b.Value));//升序排列
            for (int i = 0; i < ltmp.Count; i++)
            {
                if (card.GreatThan(ltmp[i]) || ltmp[i].Value == card.Value + 1)
                {
                    ltmp.RemoveAt(i);
                }
                else
                    break;
            }

            if (ltmp.Count > 0)
                return ltmp[0];
            return null;
        }


        public static Card GetCatchCard(Card card, List<Card> hc)
        {
            List<Card> Result = new List<Card>();
            for (int i = 0; i < hc.Count; i++)
            {
                if (hc[i].GreatThan(card))
                {
                    return hc[i];//添加到待出牌                         
                    //hc.RemoveAt(i);//移除出的牌
                    
                }
            }
            return null;
        }
        public static List<Card> GetCatchCards(Card card, List<Card> hc)
        {
            List<Card> Result = new List<Card>();
            for (int i = 0; i < hc.Count; i++)
            {
                if (hc[i].GreatThan(card))
                {
                    Result.Add(hc[i]);//添加到待出牌                         
                 //   hc.RemoveAt(i);//移除出的牌
                    break;//跳出
                }
            }
            return Result;
        }

        //计算除色样以外的基础积分
        public static List<int> CalcBaseJifen(int c1, int c2, int c3, int c4)
        {
            List<int> li = new List<int>();
            List<int> ljifen = new List<int>();
            li.Add(c1);
            li.Add(c2);
            li.Add(c3);
            li.Add(c4);

            int ilos = 0, itie = 0;
            for (int i = 0; i < li.Count; i++)
            {
                int fen = 0;
                if (li[i] < 2) { fen = -2; ilos++; }         //未正本
                else if (li[i] == 2) { fen = 0; itie++; }   //正本 不输不赢
                else { fen = 2; }                           //赢家

                ljifen.Add(fen);
            }
            //未正本-1吊 正本0吊 得桌超过2张1吊  香炉脚 6桌吊 7桌吊 八全另算
            int iwin = 4 - itie - ilos;
            for (int j = 0; j < ljifen.Count; j++)
            {

                if (ljifen[j] == 2)
                {
                    if (ilos == 1 && iwin == 2) { ljifen[j] = 1; }
                    if (ilos == 2 && iwin == 1) { ljifen[j] = 2; }
                    if (ilos == 3 && iwin == 1) { ljifen[j] = 2; }
                }
            }

            return ljifen;
        }


        public static bool IsListsMixed(List<string> t1, List<string> t2)
        {
            t1 = t1.Distinct().ToList();
            var lmix = t1.Except(t2).ToList();
            if (lmix.Count < t1.Count)
                return true;
            else
                return false;

        }

        public static bool isJinZhang(Card c)
        {
            if (c.Suit != PokerConst.Spade &&
                (c.Value == PokerConst.JiValve ||
                 c.Value >= GetShangValve(c.Suit) - 1) ||
                c.Suit == PokerConst.Spade &&
                (c.Value == PokerConst.JiValve ||
                 c.Value >= GetShangValve(c.Suit)))
                return true;
            else
                return false;
        }

        public static int GetShangValve(string suit, bool bWan = false)
        {
            int iShang = PokerConst.ShangValve;
            if (suit == PokerConst.Diamond ||
                suit == PokerConst.Heart ||
                (!bWan && suit == PokerConst.Spade))
                iShang -= 2;

            return iShang;
        }

        public static int GetQingCount(List<Card> lc, string suit)
        {
            int lowHong = GetShangValve(suit) - 1;
            if (suit == PokerConst.Spade)
                lowHong = GetShangValve(suit);
            return lc.Where(c => c.Suit == suit &&
                               c.Value > PokerConst.JiValve &&
                               c.Value < lowHong).Count();
        }

        public static int GetQingCount(List<List<Card>> llc, string suit)
        {
            int lowHong = GetShangValve(suit) - 1;
            if (suit == PokerConst.Spade)
                lowHong = GetShangValve(suit);
            foreach (var lc in llc)
            {
                if (lc.Count(c => c.Suit == suit) > 0)
                    return lc.Count(c => c.Suit == suit &&
                               c.Value > PokerConst.JiValve &&
                               c.Value < lowHong);
            }
            return 0;

        }

        public static int GetCountofSuit(List<List<Card>> llc, string suit)
        {
            int nCount = 0;
            foreach (var v in llc)
            {
                nCount = v.Count(c => c.Suit == suit);
                if (nCount > 0)
                    return nCount;
            }
            return nCount;
        }



        public static int GetSuitsCount(List<Card> lc)
        {
            int count = 0;
            List<string> lsuit = new List<string> {
            PokerConst.Club,//方块
            PokerConst.Diamond,//梅花
            PokerConst.Heart,//红桃
            PokerConst.Spade};//黑桃

            foreach (string suit in lsuit)
            {
                if (lc.Exists(c => c.Suit == suit))
                    count++;
            }
            return count;
        }


        //重新按正门 三门  十子排序
        #region 重新按正门 三门  十子排序
        public static List<List<Card>> Sort1230(List<Card> lc, bool bLord = false, string suit1 = PokerConst.Club, string suit2 = PokerConst.Diamond, bool flowerFallen = false)
        {
            List<List<Card>> listNew = new List<List<Card>>();
            lc.Sort((a, b) => { return a.Value.CompareTo(b.Value); });//升序排序

            List<Card> ls1 = lc.Where(c => c.Suit == suit1).ToList();
            List<Card> ls2 = lc.Where(c => c.Suit == suit2).ToList();
            List<Card> ls3 = lc.Where(c => c.Suit != suit1 && c.Suit != suit2 && c.Suit != PokerConst.Spade).ToList();
            List<Card> lspade = lc.Where(c => c.Suit == PokerConst.Spade).ToList();
            if (bLord)
            {
                if (ls3 != null && ls3.Count > 0) listNew.Add(ls3);
                if (ls1 != null && ls1.Count > 0) listNew.Add(ls1);
                if (ls2 != null && ls2.Count > 0) listNew.Add(ls2);
                if (lspade != null && lspade.Count > 0) listNew.Add(lspade);
            }
            else
            {
                if (ls1 != null && ls1.Count > 0) listNew.Add(ls1);
                if (ls2 != null && ls2.Count > 0) listNew.Add(ls2);
                if (flowerFallen)
                {
                    if (lspade != null && lspade.Count > 0) listNew.Add(lspade);
                    if (ls3 != null && ls3.Count > 0) listNew.Add(ls3);
                }
                else
                {
                    if (ls3 != null && ls3.Count > 0) listNew.Add(ls3);
                    if (lspade != null && lspade.Count > 0) listNew.Add(lspade);

                }
            }
            return listNew;
        }
        #endregion




        /// <summary>
        /// 负责将类似 {3-3-3, A, 6-6, BJ, 4-4}  整理成  BJ-A-6-6-4-4-3-3-3
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //public static String SuperSort(List<String> input)
        //{
        //    String a1, b1;
        //    int index_a, index_b;

        //    input.Sort((a, b) =>
        //    {
        //        a1 = a.Split('-')[0];
        //        b1 = b.Split('-')[0];

        //        index_a = EngineValues.CardValues.IndexOf(a1);
        //        index_b = EngineValues.CardValues.IndexOf(b1);

        //        if (index_a > index_b)
        //        {
        //            return -1;
        //        }
        //        else
        //        {
        //            return 1;
        //        }
        //    });

        //    return String.Join("-", input);
        //}

        /// <summary>
        /// 计算指定卡牌的索引（0~53 或者 0~14）
        /// </summary>
        /// <param name="card_str"></param>
        /// <param name="colored"></param>
        /// <returns></returns>
        //public static int IndexOfCard(String card_str, bool colored)
        //{
        //    if (colored)  // 有花色
        //    {
        //        return EngineValues.Cardsmd.IndexOf(card_str);
        //    }
        //    else  // 无花色
        //    {
        //        return EngineValues.CardValuesmd.IndexOf(card_str);
        //    }
        //}

        /// <summary>
        /// 根据指定牌型，将卡牌排序
        /// 卡牌：{6*S, 6*D, 6*H, 7*D, 7*H, 7*S, 7*C, 6*C}  牌型：6-6-6-7-7-7-7-6（飞机带两个）
        /// 排序后：{6*S, 6*H, 6*C, 7*S, 7*H, 7*C, 7*D, 6*D}
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="type"></param>
        //public static List<String> SortWithCardType(List<String> input, CardType type)
        //{
        //    List<String> target = type.CardKey.Split('-').ToList();

        //    for (int i = 0; i < target.Count; ++i)
        //    {
        //        for (int j = EngineValues.CardColors.Count - 1; j >= 0; --j)
        //        {
        //            if (input.Contains(target[i] + "*" + EngineValues.CardColors[j])
        //                && !target.Contains(target[i] + "*" + EngineValues.CardColors[j]))
        //            {
        //                target[i] = target[i] + "*" + EngineValues.CardColors[j];
        //                break;
        //            }

        //            if (input.Contains(target[i]))
        //            {
        //                break;
        //            }
        //        }
        //    }

        //    return target;
        //}
        #region 比较两张卡牌大小 a 是否小于 b
        public static bool IsCardLess(Card a, Card b)
        {
            if ((a.Suit != b.Suit) ||
                   (a.Suit == b.Suit &&
                    a.Value < b.Value))
                return true;
            else
                return false;
        }
        #endregion

        #region 比较两张卡牌大小 a 是否大于 b
        public static bool IsCardGreater(Card a, Card b)
        {
            if (a.Suit == b.Suit &&
                    a.Value > b.Value)
                return true;
            else
                return false;
        }
        #endregion
        #region  十字门开冲  百千万与二十九十互冲 
        public static CardType ShiChong(List<Card> lcards, Card card, bool bRepeat = false)
        {
            int jifen = 0;
            if (card.Suit == PokerConst.Spade)
            {
                //百老九十互冲
                if (card.Value == 8 && lcards.Exists(c => c.Value == 9))//百老冲九十 5（万千冲过百老再冲九十亦5）
                    jifen += 5;
                else if (card.Value == 9 && lcards.Exists(c => c.Value == 8))//九十冲百老 5
                    jifen += 5;
                //万千百互冲
                else if (card.Value > 8 && lcards.Exists(c => c.Value > 8))//红万千僧百子互冲 5
                    jifen += 5;
                //二十与万千百互冲
                else if (card.Value > 8 && lcards.Exists(c => c.Value == 1))//二十冲万千百 5
                    jifen += 5;
                else if (card.Value == 1 && lcards.Exists(c => c.Value > 8))//万千百冲二十 5  再冲三十亦 5
                    jifen += 5;
                //九十与万千互冲
                else if (card.Value == 8 && lcards.Exists(c => c.Value > 9))//九十冲万千 2，  九十上桌冲出百老 再冲万千 冲八十 俱5
                    jifen += 2;
                else if (card.Value > 9 && lcards.Exists(c => c.Value == 8))//万千冲九十 2，  万千冲过百老 再冲九十 亦5
                    jifen += 2;
                else if (card.Value == 1 && lcards.Exists(c => c.Value == 2))//二十上桌冲三十 2  .万千百上桌冲出二十再冲三十 俱5
                    jifen += 2;
                else if (card.Value == 9 && lcards.Exists(c => c.Value == 8) && lcards.Exists(c => c.Value == 6))//九十 七十同上冲出百老
                    jifen += 5;
                else if (card.Value == 7 && lcards.Exists(c => c.Value == 8) && lcards.Exists(c => c.Value == 6))//九十 七十同上冲出八十
                    jifen += 10;
                else if (lcards.Exists(c => c.Value == card.Value + 1 || c.Value == card.Value - 1))
                    jifen += 5;
            }
            if (jifen > 0)
                return new CardType { CardKey = card.SValue(), Weight = 1, Name = CardsTypeMdFan.JuanLianX, Jifen = jifen };
            return null;
        }
        #endregion

        #region 卷帘冲       极、副极 或 极、次副上桌开冲
        public static CardType JuanlianChong(List<Card> lcards, Card card, int icount = 0)
        {
            int jifen = 0;
            int ishang = 0, ijian = 0;
            string key = null;
            if (card.Suit == PokerConst.Diamond ||
                card.Suit == PokerConst.Heart)
            {
                ishang = 9;
                ijian = 8;
            }
            else if (card.Suit == PokerConst.Club)
            {
                ishang = 11;
                ijian = 10;
            }
            //极 副极上桌开冲
            if (lcards.Exists(c => c.Value == 1 && c.Suit == card.Suit) &&
               lcards.Exists(c => c.Value == 2 && c.Suit == card.Suit))
            {
                if (icount == 0 && card.Value == 3) //极 副极 冲出次副
                {
                    jifen += 4;
                    icount++;
                    //再冲出四索 4副
                    if (lcards.Exists(c => c.Value == 3 && c.Suit == card.Suit) &&
                        card.Value == 4 && icount > 0)
                    { jifen += 4; icount++; }
                }
                //
                if (card.Value == ishang && icount == 0)  //极 副极 冲出赏
                { jifen += 12; icount++; }
                if (card.Value == ijian && icount == 0) //极 副极 冲出肩
                {
                    jifen += 4;
                    icount++;
                    //再冲出副肩
                    if (lcards.Exists(c => c.Value == ijian || c.Value == ishang)
                        && card.Value == ijian - 1 && icount > 0)
                    { jifen += 3; icount++; }
                }

            }
            //1索3索上桌 冲出2索  5副   
            //极、次副上桌冲副极
            if (lcards.Exists(c => c.Value == 1 && c.Suit == card.Suit) &&
               lcards.Exists(c => c.Value == 3 && c.Suit == card.Suit) &&
               card.Value == 2)
            {
                jifen += 5;
                icount++;
            }


            if (jifen > 0)
                return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.JuanLianX, Jifen = jifen };
            return null;
        }
        #endregion

        #region  兄弟冲  8、6或9、6或9、1上桌开冲
        public static CardType XiongDiChong(List<Card> lcards, Card card, int icount = 0)
        {
            int jifen = 0;
            int ishang = 0, ijian = 0;
            string key = null;
            if (card.Suit == PokerConst.Diamond ||
                card.Suit == PokerConst.Heart)
            {
                ishang = 9;
                ijian = 8;
            }
            else if (card.Suit == PokerConst.Club)
            {
                ishang = 11;
                ijian = 10;
            }

            //9索8索 开冲 
            if (lcards.Exists(c => c.Value == ishang && c.Suit == card.Suit) &&
                lcards.Exists(c => c.Value == ijian && c.Suit == card.Suit))
            {
                if (card.Value == ijian - 1)
                    jifen += 4;
                else if (card.Value == 1)
                    jifen += 6;
            }

            //八六索上桌开冲
            if (lcards.Exists(c => c.Value == ijian && c.Suit == card.Suit) &&
            lcards.Exists(c => c.Value == ijian - 2 && c.Suit == card.Suit))
            {
                key = lcards.Find(c => c.Value == ishang).SValue() + "-" + card.SValue();
                if (icount == 0 && card.Value == ishang)//8 6索 冲 9索
                {
                    jifen += 3;
                    icount++;
                }
                //8索6索 冲出7索   
                if (lcards.Exists(c => c.Value == ijian - 1 && c.Suit == card.Suit))//兄弟冲 顺二逆三
                {
                    if (icount == 0)
                    {
                        jifen += 5;
                        icount++;
                    }
                    if (icount > 0 && lcards.Exists(c => c.Value == ijian - 1 && c.Suit == card.Suit))//又冲出5索 2顺
                    {
                        jifen += 4;
                        icount++;
                    }

                }
            }
            //9索6索 开冲 
            if (lcards.Exists(c => c.Value == ishang && c.Suit == card.Suit) &&
                lcards.Exists(c => c.Value == ijian - 2 && c.Suit == card.Suit))
            {
                //9索6索 冲 8索  2
                if (icount == 0 && card.Value == ijian)
                {
                    jifen += 2;
                    icount++;
                }
                //9索6索 再冲 7索   5
                if (icount > 0 && card.Value == ijian - 1 &&
                    lcards.Exists(c => c.Value == ijian))
                { jifen += 5; icount++; }
                //再冲5索 4
            }

            //9索 1索开冲  冲出8索
            if (lcards.Exists(c => c.Value == ishang && c.Suit == card.Suit) &&
                lcards.Exists(c => c.Value == 1 && c.Suit == card.Suit))
            {
                if (icount == 0 && card.Value == ijian)
                    jifen += 4;
            }
            //9索 8索 1索冲出7索  5
            if (lcards.Exists(c => c.Value == ishang && c.Suit == card.Suit) &&
                lcards.Exists(c => c.Value == ijian && c.Suit == card.Suit) &&
                lcards.Exists(c => c.Value == 1 && c.Suit == card.Suit))
                jifen += 5;
            //一贯上桌 冲九贯 接冲八贯 再冲七贯 俱三
            if (jifen > 0)
                return new CardType { CardKey = key, Weight = icount, Name = CardsTypeMdFan.XiongdiX, Jifen = jifen };
            return null;
        }
        #endregion

        #region 赏冲肩 副肩 趣 副趣 青张
        public static CardType ShangChong(List<Card> lcards, Card card)
        {
            int jifen = 0;
            int ishang = 0, ijian = 0;
            string key = null;


            if (card.Suit == PokerConst.Diamond ||
                card.Suit == PokerConst.Heart)
            {
                ishang = 9;
                ijian = 8;
            }
            else if (card.Suit == PokerConst.Club)
            {
                ishang = 11;
                ijian = 10;
            }

            if (lcards.Exists(c => c.Value == ishang && c.Suit == card.Suit))//赏冲趣
            {
                key = lcards.Find(c => c.Value == ishang).SValue() + "-" + card.SValue();
                if (card.Value == 1)
                {
                    jifen += 3;

                }
                else if (card.Value == 2 &&
                    lcards.Exists(c => c.Value == 1 && c.Suit == card.Suit)) //冲过趣 冲副趣
                {
                    jifen += 3;
                }
                if (card.Value == ijian - 1 &&
                 lcards.Exists(c => c.Value == ijian && c.Suit == card.Suit))//赏冲过肩 冲附肩2
                {
                    jifen += 2;
                }
                else if (card.Value == ijian && lcards.Exists(c => c.Value == ijian && c.Suit == card.Suit))//赏冲肩 2
                {
                    jifen += 2;
                }
                else  //赏冲青 1
                {
                    jifen += 1;
                }

            }
            if (jifen > 0)
                return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.XiongdiX, Jifen = jifen };
            return null;
        }
        #endregion
        //public static List<String> UpdateCardsnow(List<String> cardsnow, String cs)
        //{
        //    if (cardsnow.Count == 0 || cardsnow.Count > 3)
        //    {
        //        cardsnow.Clear();
        //        cardsnow.Add(cs);
        //    }
        //    else
        //    {
        //        bool inserted = false;
        //        for (int i = 0; i < cardsnow.Count; ++i)
        //        {
        //            if (cs.Substring(2, 1) == cardsnow[0].Substring(2, 1) &&
        //                EngineTool.IsCardGreater(cs, cardsnow[i]))
        //            {
        //                cardsnow.Insert(i, cs);
        //                inserted = true;

        //                break;
        //            }
        //        }

        //        if (!inserted)
        //        {

        //            //cards.Insert(cards.Count - 1, c);
        //            cardsnow.Add(cs);

        //        }
        //    }
        //    return cardsnow;
        //}
    }


}
