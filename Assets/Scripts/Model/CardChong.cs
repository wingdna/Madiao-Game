using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Model
{
    public class CardChong
    {
        /*public List<Card> lHole;
        public List<Card> lTab;
        public List<Card> lNew;

        public CardChong()
        {
            

        }
        public CardChong(List<Card> lTable)
        {
            lTab = new List<Card>(lTable);
            lHole = new List<Card>();
            lNew = new List<Card>(lTable);


        }*/

        private static List<Card> GetSuitCards(List<Card> lc, string suit)
        {
            List<Card> lcards = new List<Card>();
            foreach (Card c in lc)
            {
                if (c.Suit == suit)
                    lcards.Add(c);
            }
            lcards.Sort((a, b) => { return a.Suit.Equals(b.Suit) ? a.Value.CompareTo(b.Value) : a.Suit.CompareTo(b.Suit); });//升序排序
            return lcards;
        }


        public static int ChongQingAll(List<Card> lc, Card c)
        {
            int score1 = 0, score2 = 0;
            int iShang = EngineTool.GetShangValve(c.Suit);

            for (int i = 2; i < c.Value; i++)
            {
                if (!lc.Exists(card => card.Value == i))
                {
                    score1 = 1;
                    break;
                }
            }
            if (score1 == 0 && (c.Value == PokerConst.JiValve ||
                lc.Exists(cx => cx.Value == PokerConst.JiValve)))
                score1 = 2;





            for (int i = iShang - 2; i > c.Value; i--)
            {
                if (!lc.Exists(card => card.Value == i))
                {
                    score2 = 1;
                    break;
                }
            }
            if (score1 == 0 && c.Value == iShang && lc.Exists(cx => cx.Value == iShang - 1))
                score1 = 2;
            if (score2 == 0 && c.Value == PokerConst.JiValve
                           && lc.Exists(cx => cx.Value == iShang - 1)
                           && lc.Exists(cx => cx.Value == iShang))
                score2 = 2;

            return score1 >= score2 ? score1 : score2;

        }


        public static int ChongQing(List<Card> lc, Card c)
        {
            int score1 = 1, score2 = 1;
            int iShang = EngineTool.GetShangValve(c.Suit);

            for (int i = 2; i < c.Value; i++)
            {
                if (!lc.Exists(card => card.Value == i))
                {
                    score1 = 0;
                    break;
                }
            }
            if (score1 > 0 && (c.Value == PokerConst.JiValve ||
                lc.Exists(cx => cx.Value == PokerConst.JiValve)))
                score1 = 2;





            for (int i = iShang - 2; i > c.Value; i--)
            {
                if (!lc.Exists(card => card.Value == i))
                {
                    score2 = 0;
                    break;
                }
            }
            if (score2 > 0 && c.Value == iShang && lc.Exists(cx => cx.Value == iShang - 1))
                score2 = 2;
            if (score2 > 0 && c.Value == PokerConst.JiValve
                           && lc.Exists(cx => cx.Value == iShang - 1)
                           && lc.Exists(cx => cx.Value == iShang))
                score2 = 2;

            return score1 >= score2 ? score1 : score2;

        }




        public static Tuple<int, List<Card>, List<Card>> KanChong(List<Card> lc, List<Card> B, List<Card> lTotal,int twins =0)
        {
            int score = 0, thisScore = 0;
            List<Card> C = new List<Card>(lTotal);

            int ishang = PokerConst.ShangValve;


            if (lc.Count < 1)//无桌
                return Tuple.Create(score, B, C);
            if (C == null || C.Count < 1)
                C = lc;

            while (B.Count > 0)
            {
                Card b = B[0];
                ishang = EngineTool.GetShangValve(b.Suit);
                List<Card> A = lc.Where(c => c.Suit == b.Suit).ToList();
                List<Card> Cb = C.Where(c => c.Suit == b.Suit).ToList();
                Cb.Sort((x, y) => y.Value.CompareTo(x.Value));

                int nShang = A.Count(x => x.Value >= ishang);
                int nSpade = nShang + A.Count(x => x.Value == PokerConst.JiValve);

                if (A.Count < 1)
                    return Tuple.Create(score, B, C);
                //没有赏肩极
                if (!A.Exists(a => a.Value == PokerConst.JiValve) &&
                    !A.Exists(a => a.Value >= ishang - 1))
                {
                    return Tuple.Create(score, B, C);
                }
                //肩上桌不遇赏
                if (A.Exists(a => a.Value == ishang - 1) &&
                    !Cb.Exists(a => a.Value >= ishang) &&
                    !Cb.Exists(a => a.Value == PokerConst.JiValve) &&
                    b.Value < ishang)
                {
                    return Tuple.Create(score, B, C);
                }

                bool bContinue = true;
                List<int> E1 = new List<int>(),
                          E2 = new List<int>();//连续牌组开冲的集合
                for (int i = ishang; i >= PokerConst.JiValve; i--)
                {
                    if (ishang == i)
                    {
                        for (int j = PokerConst.JiValve; j <= ishang; j++)//从A开始。从小到大
                        {
                            if (Cb.Exists(a => a.Value == j) || b.Value == j)
                                E1.Add(j);
                            else if (b.Value > j && !Cb.Exists(a => a.Value == j))
                            {
                                bContinue = false;//从趣到b递增不能连续
                                break;
                            }
                            else
                                break;
                        }
                    }

                    if (Cb.Exists(c => c.Value == i) || b.Value == i)
                    {
                        E1.Add(i);
                    }
                    else if (!bContinue &&
                                b.Value < i &&  //从赏到b递减不能连续
                                !Cb.Exists(a => a.Value == i))  //上桌牌组中不包含大于被冲的某一张牌的牌 开冲卡牌与上桌牌组断连
                    {
                        E1.Clear();
                        break;
                    }
                    else
                    {
                        break;
                    }
                }


                if (E1.Count >= 3 &&
                    (E1.Max() >= ishang || E1.Min() == PokerConst.JiValve) &&       //冲出总和牌组中包含赏或极
                        ((b.Value >= ishang || b.Value == PokerConst.JiValve)        //当前底牌为赏或极
                    || (A.Exists(a => a.Value >= ishang) || A.Exists(a => a.Value >= PokerConst.JiValve))))//上桌牌组中包含赏或极
                {
                    var Av = from n in A select n.Value;
                    List<int> F = E1.Intersect(Av).ToList();
                  
                    int m = F.Count;

                    bool bSpadeChong = false;
                    if (b.Suit == PokerConst.Spade &&
                        EngineTool.isJinZhang(b) &&
                        F.Exists(i=>i >= ishang ||i==PokerConst.JiValve) )//十字门应考虑万千百二十互冲
                        bSpadeChong = true;
                    // m -= F.Count(x=>x > ishang);


                 
                    if (b.Value >= F.Max())
                    {
                        thisScore += 3 * m;
                        thisScore += bSpadeChong ? nSpade * 2 : 0;
                    }
                    else if (b.Value <= F.Min())
                    {
                        thisScore += 2 * m;
                        if (bSpadeChong)
                            thisScore += nSpade  * 3;
                        else if (F.Contains(ishang) && b.Value == PokerConst.JiValve) //包含赏冲极
                            thisScore += 1;
                    }
                    else
                    {
                        thisScore += F.Count(x => x > b.Value) * 2;
                        int subMax = F.Where(x => x< b.Value).Max();

                        if (F.Contains(PokerConst.JiValve) && F.Contains(ishang) ||
                            !F.Exists(x => x < b.Value && x >= subMax)) //包含赏冲极
                        {
                            thisScore += F.Count(x => x < b.Value) * 2;
                            thisScore += bSpadeChong ? F.Count(spadex => spadex < b.Value &&
                                                      (spadex >= ishang || spadex == PokerConst.JiValve)) * 3 : 0;
                        }
                        else
                        {
                            thisScore += F.Count(x => x < b.Value) * 3;
                            thisScore += bSpadeChong ? F.Count(spadex => spadex < b.Value &&
                                                          (spadex >= ishang || spadex == PokerConst.JiValve)) * 2 : 0;
                        }
                    }


                    if (thisScore > 0)
                    {
                        if (b.Suit == PokerConst.Spade &&
                             (b.Value >= ishang || b.Value == PokerConst.JiValve))
                        {
                            //千僧红万多胞胎
                            if (twins > 0)//千僧红万多胞胎
                                thisScore += 5 * twins;

                          //  thisScore += (nSpade-1) * 5;                           
                        }
                        else if (b.Suit == PokerConst.Spade &&
                             (b.Value < ishang && b.Value > PokerConst.JiValve))
                        {
                            //千僧红万多胞胎
                            if (twins > 0)//千僧红万多胞胎
                                thisScore += 2 * twins;
                        //    thisScore += (nSpade - 1) * 2;
                        }

                            C.Add(b);
                        B.Remove(b);
                        score += thisScore;
                        thisScore = 0;
                        continue;

                    }

                }



                if (b.Suit == PokerConst.Spade &&
                       nSpade > 0 &&
                       (b.Value >= ishang || b.Value == PokerConst.JiValve))
                {
                    if (twins > 0)//千僧红万多胞胎
                        thisScore += 5 * twins;                    

                    thisScore += nSpade * 5;
                }
                else
                {
                    if ((A.Exists(a => a.Value == ishang) && b.Suit != PokerConst.Spade)
                        || (A.Exists(a => a.Value >= ishang) && b.Suit == PokerConst.Spade))
                    {
                        int xchong = 0;
                        if (b.Value == ishang - 1)//赏冲肩
                        {
                            xchong =  2;                           
                        }
                        else if (b.Value == PokerConst.JiValve)//赏冲极
                        {
                            thisScore += nShang * 3;
                        }
                        else if (b.Value == ishang - 2 || b.Value == PokerConst.JiValve + 1)//赏冲副肩 副趣
                        {
                            
                            if (Cb.Exists(c => c.Value == ishang - 1) && b.Value == ishang - 2)
                                xchong = 2;
                            else if (Cb.Exists(c => c.Value == PokerConst.JiValve) && b.Value == PokerConst.JiValve + 1)
                                xchong = 2;
                            else
                                xchong = 1;
                           

                        }
                        else //赏冲青
                        {
                            xchong = ChongQingAll(Cb, b);
                        }

                        thisScore += nShang * xchong;
                        //千僧红万多胞胎
                        if (twins > 0 && b.Suit == PokerConst.Spade)//千僧红万多胞胎
                            thisScore += xchong  * twins;
                    }


                    //肩冲赏
                    if (A.Exists(a => a.Value == ishang - 1))
                    {
                        if (b.Value == ishang)
                        {
                            thisScore += 3;
                        }
                        else if (  (Cb.Exists(c => c.Value == ishang) || A.Exists(c => c.Value == ishang) )
                                && b.Value > ishang)
                            thisScore += 3;
                        else if (Cb.Exists(c => c.Value == ishang) || A.Exists(c => c.Value == ishang))

                        {
                            if (b.Value == ishang - 2 || b.Value == PokerConst.JiValve)//肩冲赏再冲青)
                                thisScore += 2;
                            else if (b.Value == PokerConst.JiValve + 1 && Cb.Exists(c => c.Value == PokerConst.JiValve))
                                thisScore += 2;
                            else if (b.Value == PokerConst.JiValve + 1)
                                thisScore += 1;
                            else
                                thisScore += ChongQingAll(Cb, b);
                        }
                    }


                    //极沖
                    if (A.Exists(a => a.Value == PokerConst.JiValve))
                    {
                        //极冲肩  极沖副极
                        if (b.Value == ishang - 1 || b.Value == PokerConst.JiValve + 1)
                        {
                            thisScore += 2;
                        }
                        else if (b.Value == ishang - 2)//极沖副肩
                        {
                            if (Cb.Exists(c => c.Value == ishang) && Cb.Exists(c => c.Value == ishang - 1))
                                thisScore += 2;
                            else
                                thisScore += 1;
                        }
                        else if (b.Value == ishang)//极沖赏
                        {
                            thisScore += 3;
                        }
                        else
                        {
                            thisScore += ChongQingAll(Cb, b);
                        }
                    }

                }


                if (thisScore >= 1)
                {
                    C.Add(b);
                    B.Remove(b);
                    score += thisScore;
                    thisScore = 0;
                }
                else
                    return Tuple.Create(score, B, C);

            }

            return Tuple.Create(score, B, C);
        }


        private static List<List<Card>> Sort4Suits(List<Card> lc)
        {
            List<List<Card>> llc = new List<List<Card>>();
            llc.Add(lc.Where(c => c.Suit == PokerConst.Club).ToList());
            llc.Add(lc.Where(c => c.Suit == PokerConst.Diamond).ToList());
            llc.Add(lc.Where(c => c.Suit == PokerConst.Heart).ToList());
            llc.Add(lc.Where(c => c.Suit == PokerConst.Spade).ToList());
            return llc;
        }

        private static int GetHongsCount(List<Card> lc, bool b90 = false)
        {
            List<List<Card>> llc = Sort4Suits(lc);
            int nhongs = 0, ijian = 0;
            foreach (List<Card> lcards in llc)
            {
                if (lcards != null && lcards.Count > 0)
                {
                    ijian = EngineTool.GetShangValve(lcards[0].Suit) - 1;
                    nhongs += lcards.Where(card => card.Value >= ijian).ToList().Count;
                }

            }
            if (!b90)
                nhongs -= lc.Where(card => card.Suit == PokerConst.Spade
                                        && card.Value == PokerConst.ShangValve - 3).ToList().Count;
            return nhongs;
        }
        private static bool IsContinuous(List<Card> lc)
        {
            var Av = from n in lc select n.Value;
            List<int> ln = Av.ToList();
            ln.Sort((x, y) => -x.CompareTo(y));//降序
            bool result = false;

            for (int i = 0; i < ln.Count() - 1; i++)
            {
                if (ln[i] - ln[i + 1] == 1)
                    result = true;
                else
                {
                    result = false;
                    break;
                }
            }
            return result;
        }







        public static List<Tuple<int, string>> GetChongSeYang(List<Card> ltotal, List<Card> lc = null)
        {
            Tuple<int, string> tu = Tuple.Create(0, "");
            List<Tuple<int, string>> ltu = new List<Tuple<int, string>>();

            tu = QuanHongZuiYangFei(ltotal);//十红醉杨妃
            if (tu.Item1 > 0)
                ltu.Add(tu);
            else if (ShiHongJuHui(ltotal).Item1 > 0)//十红聚会            
                ltu.Add(ShiHongJuHui(ltotal));

            else if (JiuHongZuiYangFei(ltotal).Item1 > 0)//九红醉杨妃
                ltu.Add(JiuHongZuiYangFei(ltotal));

            else if (ManTangHong(ltotal).Item1 > 0)//满堂红
                ltu.Add(ManTangHong(ltotal));

            else if (BaHongZuiYangFei(ltotal).Item1 > 0)//八红醉杨妃
                ltu.Add(BaHongZuiYangFei(ltotal));

            tu = QianShiQuanMen(ltotal);//钱十全门
            if (tu.Item1 > 0)
                ltu.Add(tu);
            else if (ShiLianHuan(ltotal).Item1 > 0)//十连环
                ltu.Add(ShiLianHuan(ltotal));

            else if (ShiTong(ltotal).Item1 > 0)//十同
                ltu.Add(ShiTong(ltotal));

            else if (JiuLianHuan(ltotal).Item1 > 0)//九连环
                ltu.Add(JiuLianHuan(ltotal));

            else if (JiuTong(ltotal).Item1 > 0)//九同
                ltu.Add(JiuTong(ltotal));

            tu = TaiJiTu(ltotal, lc);//太极图
            if (tu.Item1 > 0)
                ltu.Add(tu);
            else if (YuanYangQiTong(ltotal).Item1 > 0)//鸳鸯七同
                ltu.Add(JiuTong(ltotal));


            tu = LianHuanQiTong(ltotal, lc);//连环七同 同门赏极连冲七张
            if (tu.Item1 > 0)
                ltu.Add(tu);

            else if (YiChuoQi(ltotal, lc).Item1 > 0)//一冲七  一张牌连冲七张
                ltu.Add(YiChuoQi(ltotal, lc));

            else if (YiChuoLiu(ltotal, lc).Item1 > 0)//一冲六  一张牌连冲六张
                ltu.Add(YiChuoLiu(ltotal, lc));


            tu = TwoJueZang(ltotal, lc);//双掘藏 九十二十上桌冲出顺风旗
            if (tu.Item1 > 0)
                ltu.Add(tu);
            else if (JinJueZang(ltotal, lc).Item1 > 0)//金掘藏  九十万上桌冲出顺风旗(隔青不算)
                ltu.Add(JinJueZang(ltotal, lc));
            else if (YinJueZang(ltotal, lc).Item1 > 0)//銀掘藏  二十万上桌冲出顺风旗(隔青不算)
                ltu.Add(JinJueZang(ltotal, lc));

            tu = YinHua(ltotal, lc);//印花  九十冲出百老，并形成四门
            if (tu.Item1 > 0)
                ltu.Add(tu);

            Tuple<int, string> tux = TianNvSanHuaChong(ltotal, lc);
            if (tux.Item1 > 0)
                ltu.Add(tux);
            
            tux = DaCanChanChong(ltotal, lc);
            if (tux.Item1 > 0)
                ltu.Add(tux);

            tux = XiaoCanChan(ltotal, lc);
            if (tux.Item1 > 0)
                ltu.Add(tux);

            tux = GongSunChong(ltotal, lc);
            if (tux.Item1 > 0)
                ltu.Add(tux);

            tux = FuZiChong(ltotal, lc);
            if (tux.Item1 > 0)
                ltu.Add(tux);

            tux = ShangJianDuiZuo(ltotal, lc);
            if (tux.Item1 > 0)
                ltu.Add(tux);

            tux = ShangBaiDuiZuo(ltotal, lc);
            if (tux.Item1 > 0)
                ltu.Add(tux);

            tux = JianBaiDuiZuo(ltotal, lc);
            if (tux.Item1 > 0)
                ltu.Add(tux);


            return ltu;

        }


        #region 1.十红聚会  九红+九十万  60贺(60)        

        public static Tuple<int, string> ShiHongJuHui(List<Card> lc)
        {
            if (GetHongsCount(lc, true) >= 10)

                return Tuple.Create(60, CardsTypeMdFan.ShiHongJuHui);
            else
                return Tuple.Create(0, "");
        }
        #endregion

        #region 2.满堂红  九红  50贺        

        public static Tuple<int, string> ManTangHong(List<Card> lc)
        {
            if (GetHongsCount(lc) >= 9)

                return Tuple.Create(50, CardsTypeMdFan.JiuHong);
            else
                return Tuple.Create(0, "");
        }
        #endregion

        #region 3.十红醉杨妃  九红+九十万+二十万  80贺      

        public static Tuple<int, string> QuanHongZuiYangFei(List<Card> lc)
        {
            if (GetHongsCount(lc, true) >= 10 &&
                lc.Exists(ca => ca.Value == PokerConst.JiValve && ca.Suit == PokerConst.Spade))

                return Tuple.Create(80, CardsTypeMdFan.QuanHongZuiYangFei);
            else
                return Tuple.Create(0, "");
        }
        #endregion

        #region 4.九红醉杨妃  九张红牌+二十万  60贺        

        public static Tuple<int, string> JiuHongZuiYangFei(List<Card> lc)
        {
            if (GetHongsCount(lc) >= 9 &&
                lc.Exists(ca => ca.Value == PokerConst.JiValve && ca.Suit == PokerConst.Spade))
                return Tuple.Create(60, CardsTypeMdFan.QuanHongZuiYangFei);
            else
                return Tuple.Create(0, "");
        }
        #endregion


        #region 5.八红醉杨妃  8张红牌+二十万  40贺        

        public static Tuple<int, string> BaHongZuiYangFei(List<Card> lc)
        {
            if (GetHongsCount(lc) >= 8 &&
                lc.Exists(ca => ca.Value == PokerConst.JiValve && ca.Suit == PokerConst.Spade))
                return Tuple.Create(40, CardsTypeMdFan.BaHongZuiYangFei);
            else
                return Tuple.Create(0, "");
        }
        #endregion


        #region 6.九同  9张同花  20贺        

        public static Tuple<int, string> JiuTong(List<Card> lc)
        {
            List<List<Card>> llc = Sort4Suits(lc);
            foreach (List<Card> lcards in llc)
            {
                if (lcards.Count == 9)
                    return Tuple.Create(20, CardsTypeMdFan.JiuTong);
            }

            return Tuple.Create(0, "");
        }
        #endregion

        #region 7.十同  10张同花  30贺        

        public static Tuple<int, string> ShiTong(List<Card> lc)
        {
            List<List<Card>> llc = Sort4Suits(lc);
            foreach (List<Card> lcards in llc)
            {
                if (lcards.Count == 10)
                    return Tuple.Create(30, CardsTypeMdFan.ShiTong);
            }
            return Tuple.Create(0, "");
        }
        #endregion

        #region 8.九连环  9张同花顺  30贺  
        public static Tuple<int, string> JiuLianHuan(List<Card> lc)
        {
            List<List<Card>> llc = Sort4Suits(lc);
            foreach (List<Card> lcards in llc)
            {
                if (lcards.Count == 9 && IsContinuous(lcards))
                    return Tuple.Create(30, CardsTypeMdFan.JiuLianHuan);
            }

            return Tuple.Create(0, "");
        }
        #endregion

        #region 9.十连环  10张同花顺  40贺  
        public static Tuple<int, string> ShiLianHuan(List<Card> lc)
        {
            List<List<Card>> llc = Sort4Suits(lc);
            foreach (List<Card> lcards in llc)
            {
                if (lcards.Count == 10 && IsContinuous(lcards))
                    return Tuple.Create(40, CardsTypeMdFan.JiuLianHuan);
            }

            return Tuple.Create(0, "");
        }
        #endregion

        #region 10.钱十全门  10张同花顺  50贺  
        public static Tuple<int, string> QianShiQuanMen(List<Card> lc)
        {
            List<List<Card>> llc = Sort4Suits(lc);
            foreach (List<Card> lcards in llc)
            {
                if (lcards.Count == 10 && IsContinuous(lcards))
                    return Tuple.Create(50, CardsTypeMdFan.QianShiQuanMen);
            }

            return Tuple.Create(0, "");
        }
        #endregion


        #region 11.印花  九十冲出百老，并形成四门  30贺  
        public static Tuple<int, string> YinHua(List<Card> lTotal, List<Card> lwin)
        {
            bool bChongbai = false;
            List<List<Card>> llc = Sort4Suits(lwin);

            if (lwin.Exists(c => c.Suit == PokerConst.Spade && c.Value == EngineTool.GetShangValve(PokerConst.Spade) - 1) && //有九十
                !lwin.Exists(c => c.Suit == PokerConst.Spade && c.Value == EngineTool.GetShangValve(PokerConst.Spade)) && //无百老
                lTotal.Exists(c => c.Suit == PokerConst.Spade && c.Value == EngineTool.GetShangValve(PokerConst.Spade)))
                bChongbai = true;

            if (bChongbai &&
                lwin.Exists(c => c.Suit == PokerConst.Club &&
                            (c.Value >= EngineTool.GetShangValve(PokerConst.Club) - 1 ||
                             c.Value == PokerConst.JiValve)) &&
                lwin.Exists(c => c.Suit == PokerConst.Diamond &&
                            (c.Value >= EngineTool.GetShangValve(PokerConst.Diamond) - 1 ||
                             c.Value == PokerConst.JiValve)) &&
                lwin.Exists(c => c.Suit == PokerConst.Heart &&
                            (c.Value >= EngineTool.GetShangValve(PokerConst.Heart) - 1 ||
                             c.Value == PokerConst.JiValve)))
                return Tuple.Create(30, CardsTypeMdFan.YinHua);

            return Tuple.Create(0, "");
        }
        #endregion


        #region 12.太极图  7張同花,單一錦張再冲7同   40贺        

        public static Tuple<int, string> TaiJiTu(List<Card> ltotal, List<Card> lc)
        {
            bool bTai = false;
            int nSeven = 0;
            if (lc.Count == 8 && ltotal.Count == 15)
                bTai = true;

            List<List<Card>> llc = Sort4Suits(lc);
            List<List<Card>> llt = Sort4Suits(ltotal);
            foreach (List<Card> lcards in llc)
            {
                if (lcards.Count == 7)
                    nSeven++;
            }
            foreach (List<Card> lcards in llt)
            {
                if (lcards.Count >= 7)
                    nSeven++;
            }

            if (nSeven > 2 && bTai)
                return Tuple.Create(40, CardsTypeMdFan.TaiJiTu);
            return Tuple.Create(0, "");
        }
        #endregion


        #region 13.鸳鸯七同  两张牌连冲七张  30贺        

        public static Tuple<int, string> YuanYangQiTong(List<Card> ltotal)
        {
            int nSeven = 0;

            List<List<Card>> llc = Sort4Suits(ltotal);
            foreach (List<Card> lcards in llc)
            {
                if (lcards.Count >= 7)
                    nSeven++;
            }

            if (nSeven > 1)
                return Tuple.Create(30, CardsTypeMdFan.YuanYangQiTong);
            return Tuple.Create(0, "");
        }
        #endregion


        #region 14.连环七同  同门赏极连冲七张  20贺        

        public static Tuple<int, string> LianHuanQiTong(List<Card> ltotal, List<Card> lwin)
        {
            bool bOk = false;

            List<List<Card>> llc = Sort4Suits(lwin);
            List<List<Card>> llt = Sort4Suits(ltotal);
            foreach (List<Card> lcards in llc)
            {
                if (lcards.Exists(c => c.Value == PokerConst.JiValve) &&
                    lcards.Exists(c => c.Value == EngineTool.GetShangValve(lcards[0].Suit)) &&
                    lcards.Count < 7)
                    bOk = true;
            }

            foreach (List<Card> lcards in llt)
            {
                if (bOk &&
                    lcards.Exists(c => c.Value == PokerConst.JiValve) &&
                    lcards.Exists(c => c.Value == EngineTool.GetShangValve(lcards[0].Suit))
                     && lcards.Count >= 7)
                    return Tuple.Create(20, CardsTypeMdFan.LianHuanQiTong);
            }


            return Tuple.Create(0, "");
        }
        #endregion


        #region 15.一冲七  一张牌连冲七张  13贺        

        public static Tuple<int, string> YiChuoQi(List<Card> ltotal, List<Card> lwin)
        {
            List<Card> lchong = GetChongCards(ltotal, lwin);
            if (lchong == null || lchong.Count < 1)
                return Tuple.Create(0, "");


            List<List<Card>> llc = Sort4Suits(lchong);
            foreach (List<Card> lcards in llc)
            {
                if (lcards.Count == 7)
                    return Tuple.Create(13, CardsTypeMdFan.YiChuoQi);
            }

            return Tuple.Create(0, "");
        }
        #endregion


        #region 16.一冲六  一张牌连冲6张  6贺        

        public static Tuple<int, string> YiChuoLiu(List<Card> ltotal, List<Card> lwin)
        {
            List<Card> lchong = GetChongCards(ltotal, lwin); 
            if (lchong == null || lchong.Count < 1)
                return Tuple.Create(0, "");
            //ltotal.RemoveAll(lwin.Contains);
            List<List<Card>> llc = Sort4Suits(lchong);
            foreach (List<Card> lcards in llc)
            {
                if (lcards.Count == 6)
                    return Tuple.Create(6, CardsTypeMdFan.YiChuoLiu);
            }

            return Tuple.Create(0, "");
        }
        #endregion


        #region 17.金掘藏  九十万上桌冲出顺风旗(隔青不算)  60贺        

        public static Tuple<int, string> JinJueZang(List<Card> ltotal, List<Card> lwin)
        {
            int ibegin = 0, iend = 0;        


            List<Card> lchong = GetChongCards(ltotal, lwin);
            int jian = EngineTool.GetShangValve(PokerConst.Spade) - 1;

      
            if (lchong == null || lchong.Count < 1)
                return Tuple.Create(0, "");

            if (!(lwin.Exists(c => c.Suit == PokerConst.Spade && c.Value == jian) &&
                  lchong.Where(c => c.Suit == PokerConst.Spade && c.Value > jian).Count() == 3))
                return Tuple.Create(0, "");

            for (int i = 0, j = 0; i < ltotal.Count; i++)
            {
                if (lchong[i].Suit == PokerConst.Spade && lchong[i].Value >= EngineTool.GetShangValve(PokerConst.Spade))
                    ++j;
                if (j == 1)
                    ibegin = i;
                if (j == 3)
                {
                    iend = i;
                    break;
                }
            }

            if (ibegin >= iend)
                return Tuple.Create(0, "");

            for (int i = ibegin; i <= iend; i++)
            {
                if (lchong[i].Value < EngineTool.GetShangValve(lchong[i].Suit) - 1 &&
                    lchong[i].Value > PokerConst.JiValve)
                {
                    return Tuple.Create(0, "");
                }

            }
            return Tuple.Create(60, CardsTypeMdFan.JinJueZang);
        }
        #endregion


        #region 18.銀掘藏  二十万上桌冲出顺风旗(隔青不算)  40贺        

        public static Tuple<int, string> YinJueZang(List<Card> ltotal, List<Card> lwin)
        {

            int ibegin = 0, iend = 0;       

            List<Card> lchong = GetChongCards(ltotal, lwin);
            
            if (lchong == null || lchong.Count < 1)
                return Tuple.Create(0, "");

            int jian = EngineTool.GetShangValve(PokerConst.Spade) - 1;
            if (!(lwin.Exists(c => c.Suit == PokerConst.Spade && c.Value == PokerConst.JiValve) &&
                  lchong.Where(c => c.Suit == PokerConst.Spade && c.Value > jian).Count() == 3))
                return Tuple.Create(0, "");

            for (int i = 0, j = 0; i < lchong.Count; i++)
            {
                if (lchong[i].Suit == PokerConst.Spade && lchong[i].Value >= EngineTool.GetShangValve(PokerConst.Spade))
                    ++j;
                if (j == 1)
                    ibegin = i;
                if (j == 3)
                {
                    iend = i;
                    break;
                }
            }

            if (ibegin >= iend)
                return Tuple.Create(0, "");

            for (int i = ibegin; i <= iend; i++)
            {
                if (lchong[i].Value < EngineTool.GetShangValve(lchong[i].Suit) - 1 &&
                    lchong[i].Value > PokerConst.JiValve)
                {
                    return Tuple.Create(0, "");
                }

            }
            return Tuple.Create(40, CardsTypeMdFan.YinJueZang);
        }
        #endregion


        #region 19.双掘藏 九十二十上桌冲出顺风旗(隔青不算)  100贺        

        public static Tuple<int, string> TwoJueZang(List<Card> ltotal, List<Card> lwin)
        {

            int ibegin = 0, iend = 0;        

            List<Card> lchong = GetChongCards(ltotal,lwin) ;
            if (lchong == null || lchong.Count < 1)
                return Tuple.Create(0, "");

            int jian = EngineTool.GetShangValve(PokerConst.Spade) - 1;
            if (!(lwin.Exists(c => c.Suit == PokerConst.Spade && c.Value == PokerConst.JiValve) &&
                  lwin.Exists(c => c.Suit == PokerConst.Spade && c.Value == jian) &&
                  lchong.Where(c => c.Suit == PokerConst.Spade && c.Value > jian).Count() == 3))
                return Tuple.Create(0, "");

            for (int i = 0, j = 0; i < lchong.Count; i++)
            {
                if (lchong[i].Suit == PokerConst.Spade && lchong[i].Value >= EngineTool.GetShangValve(PokerConst.Spade))
                    ++j;
                if (j == 1)
                    ibegin = i;
                if (j == 3)
                {
                    iend = i;
                    break;
                }
            }

            if (ibegin >= iend && lchong[ibegin].Value != jian + 1)
                return Tuple.Create(0, "");

            for (int i = ibegin; i <= iend; i++)
            {
                if (lchong[i].Value < EngineTool.GetShangValve(lchong[i].Suit) - 1 &&
                    lchong[i].Value > PokerConst.JiValve)
                {
                    return Tuple.Create(0, "");
                }

            }
            return Tuple.Create(100, CardsTypeMdFan.TwoJueZang);
        }
        #endregion

        #region 20.夺锦标 一冲3夺 十门五夺              

        public static Tuple<int, int, int, string> DuoJinBiao(List<List<Card>> llwin, List<List<Card>> lltotal,int idx)
        {

            int score = 0, njinzhang = 0, shangpos = 0, duopos = -1;
            
            List<string> lsuits = new List<string>(),
                lsuits4 = new List<String> { PokerConst.Club,PokerConst.Diamond,
                                            PokerConst.Heart,PokerConst.Spade};//所有花色        


            if (llwin[idx].Count < lltotal[idx].Count)//三赏家开冲
                return Tuple.Create(0, 0, 0, "");

            List<Card> lJinWin = new List<Card>(),
                lchong = new List<Card>();
            string duoSuit = null;
            foreach (var v in lsuits4)  //  找到被奪衝的花色
            {
                if (!llwin[idx].Exists(c => c.Suit == v &&
                                            c.Value == EngineTool.GetShangValve(c.Suit)))
                {
                    duoSuit = v;
                    break;
                }
            }


            for (int i = 0; i < llwin.Count; i++)
            {
                if (llwin[i].Count < lltotal[i].Count && duoSuit!=null) //三賞家以外的玩家开冲
                {
                    duopos = i;
                    //奪衝的錦張加入列表
                    lJinWin.AddRange(llwin[i].Where(c => c.Suit == duoSuit &&
                                                    (c.Value >= EngineTool.GetShangValve(c.Suit) - 1 ||
                                                    c.Value == PokerConst.JiValve) ).ToList() );
                    break;
                }
            }

            if (lJinWin != null && lJinWin.Count > 0 && duopos >=0)
            {
                lchong.AddRange(CardChong.GetChongCards(lltotal[duopos], llwin[duopos]) );//冲出牌组
                
               
                foreach (Card c in lchong)//計算衝出錦張數量
                {
                    if (c.Suit != duoSuit)
                        break;
                    if (c.Suit == PokerConst.Spade &&
                        (c.Value == PokerConst.JiValve ||
                         c.Value >= EngineTool.GetShangValve(c.Suit))) //十字门錦张不含九十
                        njinzhang += 1;
                    else if (c.Value == PokerConst.JiValve ||
                             c.Value >= EngineTool.GetShangValve(c.Suit) - 1)//三门赏肩极
                        njinzhang += 1;
                    else
                        break;
                }

                if (lJinWin.Exists(card => card.Suit == PokerConst.Spade &&    //女将夺錦
                                           card.Value == PokerConst.JiValve) &&
                     duoSuit == PokerConst.Spade)
                    score += 10 * njinzhang;
                else if (lJinWin.Exists(card => card.Suit == duoSuit &&
                                        card.Value == PokerConst.JiValve)) //小将夺錦
                    score += 5 * njinzhang;
                //赏肩百夺錦
                score += 3 * njinzhang * lJinWin.Where(c => c.Suit == duoSuit &&
                                                    c.Value >= EngineTool.GetShangValve(c.Suit) - 1).Count();
            }

            if (score > 0)
                return Tuple.Create(score, shangpos, duopos, CardsTypeMdFan.DuoJinBiao);
            else
                return Tuple.Create(0, 0, 0, "");
        }




        #endregion


        private static List<Card> GetChongJins(List<Card> ltotal, List<Card> lwin)
        {
            List<Card> lchong = GetChongCards(ltotal, lwin);
            if (lchong == null || lchong.Count <= 0)
                return lchong;


            if (lchong.Exists(card => !EngineTool.isJinZhang(card)))
            {
                int iqing = lchong.FindIndex((card) => { return EngineTool.isJinZhang(card) == false; });
                lchong = lchong.GetRange(0, iqing);
            }

            return lchong;

        }
        public static Tuple<int, string> DaCanChanChong(List<Card> ltotal, List<Card> lwin)
        {
            //必须二十萬上桌冲出
            if (!lwin.Exists(c => c.Value == PokerConst.JiValve &&
                                c.Suit == PokerConst.Spade))
                return Tuple.Create(0, "");

            List<Card> lchong = GetChongJins(ltotal, lwin);
            if (lchong == null || lchong.Count <= 0)
                return Tuple.Create(0, "");

            int jian = EngineTool.GetShangValve(PokerConst.Spade,true) - 1;


            if (lwin.Exists(cw=>cw.Suit==PokerConst.Spade &&
                                cw.Value== PokerConst.JiValve) &&
               !lwin.Exists(cw => cw.Suit == PokerConst.Spade &&
                                  cw.Value == jian) &&
               lchong.Exists(c=>c.Value ==jian&& c.Suit==PokerConst.Spade) ) 
         
                    return Tuple.Create(5, CardsTypeMdFan.MeiNvCanChan);
            return Tuple.Create(0, "");
        }


        public static Tuple<int, string> XiaoCanChan(List<Card> ltotal, List<Card> lwin)
        {
            //必须二十萬上桌冲出
            if (!lwin.Exists(c => c.Value == PokerConst.JiValve &&
                                c.Suit == PokerConst.Spade))
                return Tuple.Create(0, "");

            List<Card> lchong = GetChongJins(ltotal, lwin);
            if (lchong == null || lchong.Count <= 0)
                return Tuple.Create(0, "");


            if (lwin.Exists(cw => cw.Suit == PokerConst.Spade &&
                                cw.Value == PokerConst.JiValve) &&
               !lwin.Exists(cw => cw.Suit == PokerConst.Spade &&
                                  cw.Value == 4) &&
                lchong.Exists(c => c.Value == 4 && c.Suit == PokerConst.Spade))

                return Tuple.Create(5, CardsTypeMdFan.XiaoCanChan);
            return Tuple.Create(0, "");
        }
        public static Tuple<int, string> TianNvSanHuaChong(List<Card> ltotal, List<Card> lwin)
        {

            //必须二十萬上桌冲出
            if (!lwin.Exists(c => c.Value == PokerConst.JiValve &&
                                c.Suit == PokerConst.Spade))
                return Tuple.Create(0, "");

            int bai = EngineTool.GetShangValve(PokerConst.Spade, true) - 2;
            List<Card> lchong = GetChongJins(ltotal, lwin);
            if (lchong == null || lchong.Count <= 0)
                return Tuple.Create(0, "");

            if (lwin.Exists(cw => cw.Suit == PokerConst.Spade &&
                                cw.Value == PokerConst.JiValve) &&
               !lwin.Exists(cw => cw.Suit == PokerConst.Spade &&
                                  cw.Value == bai) &&
                lchong.Exists(c => c.Value == bai && c.Suit == PokerConst.Spade))

                return Tuple.Create(5, CardsTypeMdFan.TianNvSanHua);
            return Tuple.Create(0, "");
        }

        public static Tuple<int, string> GongSunChong(List<Card> ltotal, List<Card> lwin)
        {
            //必须双趣上桌冲出
            if (lwin.Count(c=>c.Value == PokerConst.JiValve)<2 )
                return Tuple.Create(0, "");

            List<Card> lchong = GetChongJins(ltotal, lwin);
            if (lchong == null || lchong.Count <= 0)
                return Tuple.Create(0, "");

            var vji = lwin.Where(cw => cw.Value == PokerConst.JiValve);
            var vchongshang = lchong.Where(cw => cw.Value == EngineTool.GetShangValve(cw.Suit,true));
            if (vji.Count() < 2 || vchongshang.Count() < 2)
                return Tuple.Create(0, "");


            if (vji.Count(cw => cw.Value == PokerConst.JiValve) >=2 &&
                vchongshang.Count(cx => cx.Value == EngineTool.GetShangValve(cx.Suit,true)) >=2)   
                return Tuple.Create(8, CardsTypeMdFan.GongSunDuiZuo);
            return Tuple.Create(0, "");
        }

        public static Tuple<int, string> FuZiChong(List<Card> ltotal, List<Card> lwin)
        {
            //必须双趣上桌冲出
            if (lwin.Count(c => c.Value == PokerConst.JiValve) < 2)
                return Tuple.Create(0, "");

            List<Card> lchong = GetChongJins(ltotal, lwin);
            if (lchong == null || lchong.Count <= 0)
                return Tuple.Create(0, "");


            var vji = lwin.Where(cw => cw.Value == PokerConst.JiValve);
            var vchongjian = lchong.Where(cw => cw.Value == EngineTool.GetShangValve(cw.Suit)-1);
            if (vji.Count() < 2 || vchongjian.Count() < 2)
                return Tuple.Create(0, "");


            if (vji.Count(cw => cw.Value == PokerConst.JiValve) >= 2 &&
                vchongjian.Count(cx => cx.Value == EngineTool.GetShangValve(cx.Suit, true)-1) >= 2 )
                return Tuple.Create(6, CardsTypeMdFan.FuZiTongDeng);
            return Tuple.Create(0, "");
        }

        //賞肩對座
        public static Tuple<int, string> ShangJianDuiZuo(List<Card> ltotal, List<Card> lwin)
        {
            //必须双趣上桌冲出
            if (lwin.Count(c => c.Value == PokerConst.JiValve) < 2)
                return Tuple.Create(0, "");

            List<Card> lchong = GetChongJins(ltotal, lwin);
            if (lchong == null || lchong.Count <= 0)
                return Tuple.Create(0, "");

            var vji = lwin.Where(cw => cw.Value == PokerConst.JiValve);
            var vshangjian = lchong.Where(cw => cw.Value == EngineTool.GetShangValve(cw.Suit)||
                                                cw.Value == EngineTool.GetShangValve(cw.Suit)-1);
            if (vji.Count() < 2 || vshangjian.Count() < 2)
                return Tuple.Create(0, "");


            if (vji.Count(cw => cw.Value == PokerConst.JiValve) >= 2 &&
                lchong.Count(cx => cx.Value == EngineTool.GetShangValve(cx.Suit, true) ) >= 1&&
                lchong.Count(cx => cx.Value == EngineTool.GetShangValve(cx.Suit, true) - 1) >= 1)
                return Tuple.Create(7, CardsTypeMdFan.ShangJianDuiZuo);
            return Tuple.Create(0, "");
        }

        //賞百對座
        public static Tuple<int, string> ShangBaiDuiZuo(List<Card> ltotal, List<Card> lwin)
        {
            //必须双趣且包含二十万上桌冲出
            if (lwin.Count(c => c.Value == PokerConst.JiValve) < 2 ||
                !lwin.Exists(c=>c.Value == PokerConst.JiValve &&
                                c.Suit == PokerConst.Spade) )
                return Tuple.Create(0, "");

            List<Card> lchong = GetChongJins(ltotal, lwin);
            if (lchong == null || lchong.Count <= 0)
                return Tuple.Create(0, "");

            var vji = lwin.Where(cw => cw.Value == PokerConst.JiValve);
            var vshangbai = lchong.Where(cw => cw.Value == EngineTool.GetShangValve(cw.Suit)  ||
                                            (cw.Value == EngineTool.GetShangValve(cw.Suit) - 2 &&
                                             cw.Suit == PokerConst.Spade));
            if (vji.Count() < 2 || vshangbai.Count() < 2)
                return Tuple.Create(0, "");


            if (vji.Count(cw => cw.Value == PokerConst.JiValve) >= 2 &&
                vji.Count(cw => cw.Value == PokerConst.JiValve &&
                                cw.Suit == PokerConst.Spade) == 1 &&
                vshangbai.Count(cx => cx.Value == EngineTool.GetShangValve(cx.Suit, true)) >= 1 &&
                vshangbai.Count(cx => cx.Value == EngineTool.GetShangValve(cx.Suit, true) - 2&&
                                   cx.Suit == PokerConst.Spade) > 0)
                return Tuple.Create(8, CardsTypeMdFan.ShangBaiDuiZuo);
            return Tuple.Create(0, "");
        }

       //肩百對座
       public static Tuple<int, string> JianBaiDuiZuo(List<Card> ltotal, List<Card> lwin)
        {
            //必须双趣且包含二十万上桌冲出
            if (lwin.Count(c => c.Value == PokerConst.JiValve) < 2 ||
                !lwin.Exists(c => c.Value == PokerConst.JiValve &&
                                c.Suit == PokerConst.Spade))
                return Tuple.Create(0, "");

            List<Card> lchong = GetChongJins(ltotal, lwin);
            if (lchong == null || lchong.Count <= 0)
                return Tuple.Create(0, "");

            var vji = lwin.Where(cw => cw.Value == PokerConst.JiValve);
            var vjianbai = lchong.Where(cw => cw.Value == EngineTool.GetShangValve(cw.Suit) - 1||
                                             (cw.Value == EngineTool.GetShangValve(cw.Suit)-2&&
                                              cw.Suit == PokerConst.Spade) );
            if (vji.Count() < 2 || vjianbai.Count() < 2)
                return Tuple.Create(0, "");


            if (vji.Count(cw => cw.Value == PokerConst.JiValve) >= 2 &&
                vji.Count(cw => cw.Value == PokerConst.JiValve &&
                                cw.Suit == PokerConst.Spade) == 1 &&
                vjianbai.Count(cx => cx.Value == EngineTool.GetShangValve(cx.Suit, true)) >= 1 &&
                vjianbai.Count(cx => cx.Value == EngineTool.GetShangValve(cx.Suit, true) - 2 &&
                                   cx.Suit == PokerConst.Spade) > 0)
                return Tuple.Create(10, CardsTypeMdFan.JianBaiDuiZuo);
            return Tuple.Create(0, "");
        }

        public static List<Card> GetChongCards(List<Card> ltotal,List<Card> lwin)
        {
            if (ltotal == null || lwin == null || 
                ltotal.Count == 0 ||lwin.Count == 0||
                ltotal.Count <= lwin.Count)
                return null;
            else
                return ltotal.Where(c => !lwin.Exists(card => card.Suit == c.Suit &&
                                                     card.Value == c.Value)).ToList();
            

        }

        /*
                public int JiChong(Card c)
                {
                    if (!lTab.Exists(cTab => cTab.Suit == c.Suit && cTab.Value == 1) )
                        return 0;

                    int iShang = GetShangValve(c.Suit);
                    List<Card> lc = GetSuitCards(lNew, c.Suit);
                    if (c.Value == iShang)
                        return 3;
                    else if (c.Value == iShang - 1)
                        return 2;
                    else if (c.Value == PokerConst.JiValve + 1)
                        return 2;
                    else
                        return ChongQing(lc,c);
                }


                public int ShangChong(Card c)
                {
                    int iShang = GetShangValve(c.Suit);

                    if (!lTab.Exists(cTab => cTab.Suit == c.Suit && cTab.Value == iShang))
                        return 0;


                    List<Card> lc = GetSuitCards(lNew, c.Suit);
                    if (c.Value == PokerConst.JiValve)
                        return 3;
                    else if (c.Value == iShang - 1)
                        return 2;
                    else if (c.Value == PokerConst.JianValve + 1)
                        return 2;
                    else
                        return ChongQing(lc, c);
                }

                public int JianChong(Card c)
                {
                    int iShang = GetShangValve(c.Suit);

                    if ( !lTab.Exists(cTab => cTab.Suit == c.Suit  && cTab.Value == iShang-1 ) )//无本门肩
                       return 0;
                    if (c.Value < iShang && !lNew.Exists(card => card.Value == iShang) )//肩未冲赏
                        return 0;


                    List<Card> lc = GetSuitCards(lNew, c.Suit);
                    if (c.Value == PokerConst.JiValve)
                        return 2;
                    else if (c.Value == iShang )
                        return 3;           
                    else
                        return ChongQing(lc, c);
                }

                public  int SpadeChong(Card c)
                {
                    if (c.Suit != PokerConst.Spade)
                        return 0;
                    if (!lTab.Exists(cTab => cTab.Suit == c.Suit && (cTab.Value == PokerConst.JiValve || cTab.Value >= PokerConst.ShangValve - 3)) )
                        return 0;

                    int iShang = GetShangValve(c.Suit);
                    List<Card> lc = GetSuitCards(lNew, c.Suit);


                    if ((c.Value == PokerConst.JiValve || c.Value >= PokerConst.ShangValve - 2)
                        && (lc.Exists(card => card.Value == PokerConst.JiValve || card.Value >= PokerConst.ShangValve - 2)))
                        return 5;
                    else if (lc.Exists(card => card.Value == iShang))
                        return ShangChong(c);
                    else if (lc.Exists(card => card.Value == iShang - 1))
                        return JianChong(c);
                    else if (lc.Exists(card => card.Value == PokerConst.JiValve))
                        return JiChong(c);
                    else
                        return 0;
                }
        */

    }


    public static class Ex
    {
        public static IEnumerable<IEnumerable<T>> DifferentCombinations<T>(this IEnumerable<T> elements, int k)
        {
            return k == 0 ? new[] { new T[0] } :
              elements.SelectMany((e, i) =>
                elements.Skip(i + 1).DifferentCombinations(k - 1).Select(c => (new[] { e }).Concat(c)));
        }
    }
}
