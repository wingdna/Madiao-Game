using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;

namespace Assets.Scripts.Model
{


    public class GameEngine
    {

        public static CardType JifenSJBQ(List<Card> lcard)
        {

            int ishang = CardSeyang.CountShangs(lcard),
                ijian = CardSeyang.CountJians(lcard),
                iji = CardSeyang.CountJis(lcard),
                ifj = CardSeyang.CountKeys(lcard, CardSeyang.Fuji.Keys.ToList());
            int jifen = iji;//ifj * 1 + ijian * 1 + iji * 2;
            string key = "";
            //二十子极上桌得分翻倍
            if (lcard.Exists(c => c.SValue() == CardSeyang.Cardsmd[29].SValue()))
                jifen += 1;
            if (JifenBaiTu(lcard) == null)
            {
                int ibai = CardSeyang.CountBai(lcard);
                if (ibai > 0)
                {
                    key += MDNameFanConst.BaiWan;
                    jifen += ibai * 4;
                }
            }
            /*
             if (ijian > 0)
                 key += ijian.ToString() + MDNameFanConst.Jian;
             if (ifj > 0)
                 key += ifj.ToString() + MDNameFanConst.Fuji;
            */
            if (iji > 0)
                key += iji.ToString() + MDNameFanConst.Ji;

            if (jifen > 0)
                return new CardType() { CardKey = key, Weight = 1, Name = MDNameFanConst.SingleItem, Jifen = jifen };
            else
                return null;

        }
        #region 获取成突牌组 百老 九十 与(五 六 八)贯之组合
        public static CardType JifenBaiTu(List<Card> lcard,bool baijia =false )
        {
            List<CardType> lt = new List<CardType>();
            //是否全突大活
            CardType ct = CardSeyang.Quantudahuo(lcard);
            if (ct != null)
                return ct;

            //大活百突
            ct = CardSeyang.Dahuobaitu(lcard);
            if (ct != null)
                return ct;
            //大活百
            ct = CardSeyang.Dahuobai(lcard);
            if (ct != null)
                return ct;

            if (baijia == true && ct == null)
            {
                if (lcard.Count < 2)
                    ct = CardSeyang.Diaobai(lcard);
                else
                    ct = CardSeyang.ZhengBenBai(lcard);

            }
            if (ct != null)
                return ct;

            return null;
        }
        #endregion
        public static List<CardType> GetSeyang(List<Card> lcard)
        {
            List<string> ls = new List<string>();
            List<CardType> lt = new List<CardType>();
            lcard = lcard.Distinct().ToList();//去重
            lcard.ForEach(c => ls.Add(c.SValue()));

            if (ls.Count < 2)
                return lt;
            if (ls.Count > 4)//最少5张
            {
                List<CardType> lsno = SeYangNoPlay(lcard);//斗牌后出现免斗中色样 赢筹翻倍
                if (lsno != null && lsno.Count > 0)
                {
                    lsno.ForEach(ct => lt.Add(new CardType { CardKey = ct.CardKey, Weight = ct.Weight, Name = ct.Name, Jifen = ct.Jifen * 2 }));
                }
            }
            if (ls.Count >= 4)//正色样 4张按副出注
            {
                List<CardType> lc = GetSeYangPlay4(lcard);
                if (lc != null)
                {
                    lt.AddRange(lc);    //4张正色样
                    lc.Clear();
                }
            }
            lt.AddRange(SeYangPlayX(ls));


            return lt;
        }

        //public static List<CardType> Chong(Card card)
        //{
        //    switch (card.Value)
        //    {
        //        case 11:
        //            if (card.Suit == PokerConst.Spade)
        //            {

        //            }
        //            break;
        //    }
        //}
        public static List<CardType> GetSeyangTip(List<Card> lcard, List<Card> winCards)
        {
            List<string> ls = new List<string>();
            List<CardType> lt = new List<CardType>();

            lcard.ForEach(c => ls.Add(c.SValue()));
            if (ls.Count > 0)
                lt.AddRange(SeYangPlayX(ls));

            List<Card> lcjin = new List<Card>(winCards);
            var vcards = lcard.Where(c => c.Value == PokerConst.JiValve ||
                                             (c.Value >= EngineTool.GetShangValve(c.Suit) - 1 && c.Suit != PokerConst.Spade) ||
                                             (c.Value >= EngineTool.GetShangValve(c.Suit) && c.Suit == PokerConst.Spade));
            lcjin.AddRange(vcards.ToList());
            List<string> lsjin = new List<string>();
            lcjin.ForEach(c => lsjin.Add(c.SValue()));
            if (lsjin.Count > 0)
                lt.AddRange(SeYangPlayX(lsjin));


            //以下大小突散家开庄家，庄家开其一散家， 大活百、大活百突、全突大活散家另敲2散家
            CardType ctbaitu = GameEngine.JifenBaiTu(lcard);//各种含百老牌组
            if (ctbaitu != null)
                lt.Add(ctbaitu);
            //小突：九十 + （五 六 八） 万之一
            CardType ctxiaotu = CardSeyang.Xiaotu(lcard);
            if (ctxiaotu != null)
                lt.Add(ctxiaotu);


            List<string> lsqing = new List<string>();//清张组合
            List<Card> lc2 = new List<Card>(lcard);
            lc2.RemoveAll(it => CardSeyang.lhong.Exists(hong => it.SValue() == hong.SValue()));
            if (lc2.Count >= lcard.Count)
                return lt;

            lc2.ForEach(c => lsqing.Add(c.SValue()));

            if (ls.Count > 0 && !winCards.Exists(c => CardSeyang.lhong.Exists(hong => c.SValue() == hong.SValue())))
                lt.AddRange(SeYangPlayX(lsqing));

            //正色组合
            lsqing.RemoveAll(it => CardSeyang.Ji.Keys.ToList().Contains(it));
            ls.RemoveAll(it => lsqing.Contains(it));
            List<String> ls4 = CardSeyang.SelectSameCards(ls, 4);

            //ls3.ForEach(c => lt.AddRange(SeYangPlayX(c.Split('-').ToList())));
            //ls4.ForEach(c => lt.AddRange(SeYangPlayX(c.Split('-').ToList())));
            List<CardType> ltmp = new List<CardType>();
            foreach (var s in ls4)
            {
                List<string> lsseyang4 = s.Split('-').ToList();
                ltmp = SeYangPlay4(lsseyang4);
                if (ltmp != null)
                {
                    lt.AddRange(CheckSeyang4(ltmp, lsseyang4));
                    ltmp.Clear();
                }
            };
            //lt.Where((x, i) => lt.FindIndex(s => s.CardKey == x.CardKey) == i).ToList();
            lt.RemoveAll(it => it.Name == CardsTypeMdFan.BaQuan
                                   || it.Name == CardsTypeMdFan.QiDiao
                                   || it.Name == CardsTypeMdFan.LiuDiao
                                   || it.Name == CardsTypeMdFan.QingBaiRenJia);
            //lt = lt.Distinct().ToList();//去重;

            //lt = lt.Distinct(new ListDistinct()).ToList();

            return lt.GroupBy(x => new { x.CardKey,x.Weight,x.Name,x.Jifen }).Select(y => y.First()).ToList();
            

        }


        public static List<CardType> GetSeyang4(List<Card> lcards)
        {
            List<string> ls = new List<string>();
            foreach (Card c in lcards)
                ls.Add(c.SValue());
            return SeYangPlay4(ls);    //4张正色样

        }

        public static string ConvertMdName(string ManyCards)
        {
            int istart = ManyCards.IndexOf("("),
                iend = ManyCards.IndexOf(")");
            string sohter = "";
            if (istart >= 0 && iend > istart)
            {
                sohter = ManyCards.Substring(istart, iend - istart+1);
                ManyCards = ManyCards.Remove(istart, iend - istart+1);
            }
            List<string> ls = ManyCards.Split('-').ToList();
            string result = null;
            foreach (string s in ls)
            {
                string stmp = s.Replace(" ", "");
                if (stmp != "")
                    result += new Card(stmp).MdName() + " ";
            }
            return result+sohter;
        }



        public static List<CardType> SeyangTips(List<String> lcards, List<String> lcardswin)
        {//提示手牌所有可能组成的牌组

            List<CardType> lt = new List<CardType>();

            List<String> ls = lcards.Concat(lcardswin).ToList();
            lt = SeYangPlayX(ls);
            List<CardType> lc = SeYangPlay4(ls);

            if (lc != null && lc.Count > 0)
            {
                lt.AddRange(lc);
            }

            //CardType tup = new CardType(name,jifen);
            return lt;
        }

        #region 判断是否捉极,必须副极打极且上桌才算捉极
        public static int IsZhuoji(List<string> ls, List<string> lswin)
        {
            int zhuoji = -1;

            if (ls.Count < 4) //未打完一圈
                return zhuoji;
            if (ls.Count == 4
                && CardSeyang.Fuji.ContainsKey(ls[0]))    //副极上桌
            {
                foreach (String s in ls)
                {
                    if (CardSeyang.Ji.ContainsKey(s))     //有极被捉
                        zhuoji = lswin.Count;
                }
            }

            return zhuoji;
        }
        #endregion



        #region 检查所有免斗色样
        public static List<CardType> SeYangNoPlay(List<Card> lcards)
        {
            List<string> input = new List<string>();
            foreach (Card c in lcards) { input.Add(c.SValue()); }

            String formated_input = EngineTool.FormatCardStrmd(String.Join("-", input.ToArray()));
            List<CardType> result = new List<CardType>();

            if (CardSeyang.TianDiJiaoTai.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.TianDiJiaoTai[formated_input]);
            }
            else if (CardSeyang.RenJieDiLing.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.RenJieDiLing[formated_input]);
            }
            else if (CardSeyang.TianRenHeYi.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.TianRenHeYi[formated_input]);
            }
            else if (CardSeyang.JiQuanShengTian.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.JiQuanShengTian[formated_input]);
            }
            else if (CardSeyang.AoYuanYang.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.AoYuanYang[formated_input]);
            }

            else if (CardSeyang.QiHongZuiYangFei.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.QiHongZuiYangFei[formated_input]);
            }
            else if (CardSeyang.Jjjin.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.Jjjin[formated_input]);
            }
            else if (CardSeyang.GetSeYangPlay4(lcards, CardSeyang.QiLinZhong) != null)//未满八张的色样需用此函数
            {
                bool ishaveJian = false;//带肩不算麒麟种
                foreach (string strjian in CardSeyang.Jian.Keys)
                {
                    if (lcards.Exists(c => c.SValue() == strjian))
                    { ishaveJian = true; break; }
                }
                if (!ishaveJian)
                    result.Add(CardSeyang.GetSeYangPlay4(lcards, CardSeyang.QiLinZhong));
            }
            else if (CardSeyang.GetSeYangPlay4(lcards, CardSeyang.FengHuangChu) != null)//未满八张的色样需用此函数
            {
                bool ishaveshang = false;//带赏不算凤凰雏
                foreach (string strshang in CardSeyang.Shang.Keys)
                {
                    if (lcards.Exists(c => c.SValue() == strshang))
                    { ishaveshang = true; break; }
                }
                if (!ishaveshang)
                    result.Add(CardSeyang.GetSeYangPlay4(lcards, CardSeyang.FengHuangChu));
            }
            else if (CardSeyang.GetSeYangPlay4(lcards, CardSeyang.XueZhongTan) != null)//未满八张的色样需用此函数
            {
                bool ishaveshang = false;//带赏不算雪中炭
                foreach (string strshang in CardSeyang.Shang.Keys)
                {
                    if (lcards.Exists(c => c.SValue() == strshang))
                    { ishaveshang = true; break; }
                }
                if (!ishaveshang)
                    result.Add(CardSeyang.GetSeYangPlay4(lcards, CardSeyang.XueZhongTan));
            }

            else if (CardSeyang.GetSeYangPlay4(lcards, CardSeyang.LiuHong) != null)
            {
                result.Add(CardSeyang.GetSeYangPlay4(lcards, CardSeyang.LiuHong));
            }
            else if (CardSeyang.GetSeYangPlay4(lcards, CardSeyang.QiHong) != null)
            {
                result.Add(CardSeyang.GetSeYangPlay4(lcards, CardSeyang.QiHong));
            }
            else if (CardSeyang.BaHong.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.BaHong[formated_input]);
            }

            else if (CardSeyang.ShunFengQi8.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.ShunFengQi8[formated_input]);
            }
            else if (CardSeyang.GetSeYangPlay4(lcards, CardSeyang.ShunFengQi7) != null)
            {
                result.Add(CardSeyang.GetSeYangPlay4(lcards, CardSeyang.ShunFengQi7));
            }
            else if (CardSeyang.GetSeYangPlay4(lcards, CardSeyang.ShunFengQi6) != null)
            {
                result.Add(CardSeyang.GetSeYangPlay4(lcards, CardSeyang.ShunFengQi6));
            }
            else if (CardSeyang.BaTong.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.BaTong[formated_input]);
            }

            else if (CardSeyang.QiTongHua(formated_input).Count > 0)
            {
                result.AddRange(CardSeyang.QiTongHua(formated_input));
            }
            else if (CardSeyang.QiShun.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.QiShun[formated_input]);
            }
            else if (CardSeyang.BaShun.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.BaShun[formated_input]);
            }
            else if (CardSeyang.BaDa.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.BaDa[formated_input]);
            }

            else if (CardSeyang.BaXiao.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.BaXiao[formated_input]);
            }
            else if (CardSeyang.BaYao.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.BaYao[formated_input]);
            }

            if (result.Count == 0)
                return null;
            else return result;
        }
        #endregion

        #region 检查所有斗上正色样 只有4张牌上桌才可成色的情况
        public static List<CardType> SeYangPlay4(List<String> input)
        {
            String formated_input = EngineTool.FormatCardStrmd(String.Join("-", input.ToArray()));
            List<CardType> result = new List<CardType>();

            if (!CardSeyang.IsAllHong(input) && !CardSeyang.Ji.Keys.ToList().Exists(c => input.Contains(c)))
                return null;

            //else if (input.Count != 4)
            //    return null;

            if (CardSeyang.HuangHuiTu.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.HuangHuiTu[formated_input]);
            }
            else if (CardSeyang.QianJunZhu.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.QianJunZhu[formated_input]);
            }
            else if (CardSeyang.HuaDuDou.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.HuaDuDou[formated_input]);
            }
            else if (CardSeyang.HuaBiJian.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.HuaBiJian[formated_input]);
            }
            else if (CardSeyang.QiaoSiShang.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.QiaoSiShang[formated_input]);
            }
            else if (CardSeyang.QiaoSiJian.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.QiaoSiJian[formated_input]);
            }
            else if (CardSeyang.TianDiFen.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.TianDiFen[formated_input]);
            }
            else if (CardSeyang.JianTianDiFen.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.JianTianDiFen[formated_input]);
            }
            else if (CardSeyang.XiaoTianDiFen.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.XiaoTianDiFen[formated_input]);
            }
            else if (CardSeyang.BaiDuanJian.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.BaiDuanJian[formated_input]);
            }
            else if (CardSeyang.BaiChangJian.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.BaiChangJian[formated_input]);
            }
            else if (CardSeyang.JiChangJian.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.JiChangJian[formated_input]);
            }
            else if (CardSeyang.JiDuanJian.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.JiDuanJian[formated_input]);
            }
            else if (CardSeyang.DuiJian.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.DuiJian[formated_input]);
            }

            else if (CardSeyang.ChangJian.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.ChangJian[formated_input]);
            }
            else if (CardSeyang.DuanJian.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.DuanJian[formated_input]);
            }
            else if (CardSeyang.BaiJiSiJian.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.BaiJiSiJian[formated_input]);
            }
            else if (CardSeyang.BaiJiSiShang.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.BaiJiSiShang[formated_input]);
            }
            else if (CardSeyang.BaiJianSiJi.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.BaiJianSiJi[formated_input]);
            }
            else if (CardSeyang.BaiShangBaiJi.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.BaiShangBaiJi[formated_input]);
            }
            else if (CardSeyang.JieJieGao.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.JieJieGao[formated_input]);
            }
            else if (CardSeyang.BaiJiYuBei.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.BaiJiYuBei[formated_input]);
            }
            else if (CardSeyang.JianJiYuBei.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.JianJiYuBei[formated_input]);
            }
            else if (CardSeyang.ShangJiYuBei.ContainsKey(formated_input))
            {
                result.AddRange(CardSeyang.ShangJiYuBei[formated_input]);
            }


            if (result.Count != 0)
                return result;
            else return null;
        }
        #endregion

        #region 检查所有斗上正色样 不限上桌牌数 只要其中有某4张成色即可
        public static List<CardType> GetSeYangPlay4(List<Card> lcards)
        {
            CardType ct = new CardType();
            List<CardType> result = new List<CardType>();

            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.HuangHuiTu);
            if (ct != null) result.Add(ct);

            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.QianJunZhu);
            if (ct != null) result.Add(ct);
            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.HuaDuDou);
            if (ct != null) result.Add(ct);

            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.HuaBiJian);
            if (ct != null) result.Add(ct);

            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.QiaoSiShang);
            if (ct != null) result.Add(ct);

            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.QiaoSiJian);
            if (ct != null) result.Add(ct);
            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.TianDiFen);
            if (ct != null) result.Add(ct);
            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.JianTianDiFen);
            if (ct != null) result.Add(ct);
            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.XiaoTianDiFen);
            if (ct != null) result.Add(ct);
            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.BaiDuanJian);
            if (ct != null) result.Add(ct);
            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.BaiChangJian);
            if (ct != null) result.Add(ct);
            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.JiChangJian);
            if (ct != null) result.Add(ct);
            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.JiDuanJian);
            if (ct != null) result.Add(ct);
            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.DuanJian);
            if (ct != null) result.Add(ct);
            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.ChangJian);
            if (ct != null) result.Add(ct);
            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.DuiJian);
            if (ct != null) result.Add(ct);
            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.BaiJiSiJian);
            if (ct != null) result.Add(ct);
            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.BaiJiSiShang);
            if (ct != null) result.Add(ct);
            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.BaiJianSiJi);
            if (ct != null) result.Add(ct);
            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.BaiShangBaiJi);
            if (ct != null) result.Add(ct);
            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.JieJieGao);
            if (ct != null) result.Add(ct);
            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.BaiJiYuBei);
            if (ct != null) result.Add(ct);
            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.JianJiYuBei);
            if (ct != null) result.Add(ct);
            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.ShangJiYuBei);
            if (ct != null) result.Add(ct);

            if (result != null && result.Count > 0)
            {
                List<string> ls = new List<string>();
                lcards.ForEach(c => ls.Add(c.SValue()));
                if (ls.Count >= 8 || result.Count <= 1)
                {
                    result = CheckSeyang4(result, ls);
                }
                else if (result.Count > 1)
                {
                    //取出最大值
                    result = CheckSeyang4(result, ls);
                    var maxValue = result.Max(t => t.Jifen);
                    //从列表中匹配值等于最大值的第一项
                    CardType maxItem = result.Where(x => x.Jifen == maxValue).FirstOrDefault();

                    result.Clear();
                    result.Add(maxItem);
                }

            }
            //三代荣封需正好六张，其余4张，杂色样另算
            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.SanDaiRongFeng);
            if (ct != null) result.Add(ct);
            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.GongSunDuiZuo);
            if (ct != null) result.Add(ct);
            ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.FuZiTongDeng);
            if (ct != null) result.Add(ct);
            //ct = CardSeyang.GetSeYangPlay4(lcards, CardSeyang.XiongDiQiXin);
            //if (ct != null) result.Add(ct);

            if (result.Count > 0)
                return result;
            else
                return null;
        }
        #endregion

        private static List<CardType> CheckSeyang4(List<CardType> lt, List<string> ls)
        {
            for (int i = 0; i < lt.Count; i++)
            {
                if (!(lt[i].CardKey.Contains(PokerConst.Club)
                    && lt[i].CardKey.Contains(PokerConst.Diamond)
                    && lt[i].CardKey.Contains(PokerConst.Heart)
                    && lt[i].CardKey.Contains(PokerConst.Spade)))
                {
                    /* if (lt[i].Name.Contains(MDNameFanConst.Zha))
                         continue;
                     lt[i].Name = MDNameFanConst.Zha + lt[i].Name;
                     lt[i].Jifen /= 4;
                    */
                    lt.RemoveAt(i);
                }
                else
                {
                    //ct.CardKey.Split('-').ToList().ForEach(s => ls.RemoveAll() .Remove(s));
                    ls.RemoveAll(it => lt[i].CardKey.Split('-').ToList().Contains(it));
                    lt[i].Jifen += CardSeyang.CountKeys(ls, CardSeyang.Shang.Keys.ToList())
                        + CardSeyang.CountKeys(ls, CardSeyang.Jian.Keys.ToList()) * 2
                        + CardSeyang.CountKeys(ls, CardSeyang.Ji.Keys.ToList()) * 3
                        + CardSeyang.CountKeys(ls, CardSeyang.Bai.Keys.ToList()) * 4;
                }
            }
            return lt;
        }

        #region 检查所有斗上杂色样(部分需要结合上桌牌组以外的情况)
        public static List<CardType> SeYangPlayX(List<String> input)
        {
            List<CardType> lt = new List<CardType>();
            CardType ct = new CardType();
            List<String> result = new List<string>();

            ct = CardSeyang.NeiShengWaiWang(input);
            if (ct != null) lt.Add(ct);

            ct = CardSeyang.TianRanQu(input);
            if (ct != null) lt.Add(ct);

            ct = CardSeyang.SanDieQu(input);
            if (ct != null) lt.Add(ct);

            ct = CardSeyang.LongMenYue(input);
            if (ct != null) lt.Add(ct);
            else
            {
                ct = CardSeyang.FoDingZhu(input);
                if (ct != null) lt.Add(ct);
            }

            ct = CardSeyang.SanHuaTianNv(input);
            if (ct != null) lt.Add(ct);

            ct = CardSeyang.TianNvSanHua(input);
            if (ct != null) lt.Add(ct);

            if (CardSeyang.ShuangFeiQu(input) != null)
                lt.AddRange(CardSeyang.ShuangFeiQu(input));

            ct = CardSeyang.GuoQiaoLong(input);
            if (ct != null) lt.Add(ct);

            ct = CardSeyang.QuhouBai(input);
            if (ct != null) lt.Add(ct);

            ct = CardSeyang.BaiHouQu(input);
            if (ct != null) lt.Add(ct);

            ct = CardSeyang.ShuangQuHou(input);
            if (ct != null) lt.Add(ct);

            ct = CardSeyang.ShuangBaiHou(input);
            if (ct != null) lt.Add(ct);

            ct = CardSeyang.HuangDaoJuan(input);
            if (ct != null) lt.Add(ct);
            else if (CardSeyang.DaoJuanLian(input) != null)
                lt.AddRange(CardSeyang.DaoJuanLian(input));



            ct = CardSeyang.JuanLianFei(input);
            if (ct != null) lt.Add(ct);

            ct = CardSeyang.BaDiao(input);
            if (ct != null) lt.Add(CardSeyang.BaDiao(input));
            else if (CardSeyang.QiDiao(input) != null)
            {
                lt.Add(CardSeyang.QiDiao(input));
            }
            else if (CardSeyang.LiuDiao(input) != null)
            {
                lt.Add(CardSeyang.LiuDiao(input));
            }

            ct = CardSeyang.JingZRhongRen(input);
            if (ct != null) lt.Add(ct);

            ct = CardSeyang.JieHuaXianFo(input);
            if (ct != null) lt.Add(ct);

            ct = CardSeyang.FuQiTuanYuan(input);
            if (ct != null) lt.Add(ct);

            ct = CardSeyang.RenMianTaoHua(input);
            if (ct != null) lt.Add(ct);

            ct = CardSeyang.MeiNvCanChan(input);
            if (ct != null) lt.Add(ct);

            ct = CardSeyang.XiaoCanChan(input);
            if (ct != null) lt.Add(ct);

            ct = CardSeyang.ErFoTanJing(input);
            if (ct != null) lt.Add(ct);

            ct = CardSeyang.TianXianSongZi(input);
            if (ct != null) lt.Add(ct);

            if (CardSeyang.GongLingSun(input).Count != 0)  //公领孙返回CardType列表
            {
                lt.AddRange(CardSeyang.GongLingSun(input));
            }

            ct = CardSeyang.LianHuanSuo(input);
            if (ct != null) lt.Add(ct); List<string> scardswin = new List<string>();

            ct = CardSeyang.TongQiLianZhi(input);
            if (ct != null) lt.Add(ct);

            ct = CardSeyang.QingBaiRenJia(input);
            if (ct != null) lt.Add(ct);

            String formated_input = EngineTool.FormatCardStrmd(String.Join("-", input.ToArray()));
            //三代荣封需正好六张，其余4张，杂色样另算
            if (CardSeyang.SanDaiRongFeng.ContainsKey(formated_input))
            {
                lt.AddRange(CardSeyang.SanDaiRongFeng[formated_input]);
            }
            else if (CardSeyang.GongSunDuiZuo.ContainsKey(formated_input))
            {
                lt.AddRange(CardSeyang.GongSunDuiZuo[formated_input]);
            }
            else if (CardSeyang.FuZiTongDeng.ContainsKey(formated_input))
            {
                lt.AddRange(CardSeyang.FuZiTongDeng[formated_input]);
            }
            else if (CardSeyang.XiongDiQiXin.ContainsKey(formated_input))
            {
                lt.AddRange(CardSeyang.XiongDiQiXin[formated_input]);
            }
            else if (CardSeyang.ShangJianDuiZuo.ContainsKey(formated_input))
            {
                lt.AddRange(CardSeyang.ShangJianDuiZuo[formated_input]);
            }
            else if (CardSeyang.ShangBaiDuiZuo.ContainsKey(formated_input))
            {
                lt.AddRange(CardSeyang.ShangBaiDuiZuo[formated_input]);
            }
            else if (CardSeyang.JianBaiDuiZuo.ContainsKey(formated_input))
            {
                lt.AddRange(CardSeyang.JianBaiDuiZuo[formated_input]);
            }

            return lt;
        }


        #endregion

        #region 捉极献极 与 捉极献百
        public static CardType ZhuoXian(List<Card> lcwin, int pos)
        {

            if (pos >= 0 && lcwin.Count > pos + 1)
            {
                List<string> scardswin = new List<String>();
                lcwin.ForEach(c => scardswin.Add(c.SValue()));
                if (CardSeyang.ZhuoJiXianJi(scardswin, pos) != null)

                    return CardSeyang.ZhuoJiXianJi(scardswin, pos);

                else if (CardSeyang.ZhuoJiXianBai(scardswin, pos) != null)
                    return CardSeyang.ZhuoJiXianJi(scardswin, pos);

                else return null;
            }
            return null;
        }
        #endregion

        #region 查找百老发给谁家
        public static int Whoisbai(List<Card> c0, List<Card> c1, List<Card> c2, List<Card> c3)
        {
            if (c0.Exists(c => c.Suit.Equals(PokerConst.Spade) && c.Value == 9))
                return 0;
            if (c1.Exists(c => c.Suit.Equals(PokerConst.Spade) && c.Value == 9))
                return 1;
            if (c2.Exists(c => c.Suit.Equals(PokerConst.Spade) && c.Value == 9))
                return 2;
            if (c3.Exists(c => c.Suit.Equals(PokerConst.Spade) && c.Value == 9))
                return 3;
            return -1;
        }
        #endregion
        public static EpicCards CheckEpicCards(List<Card> cards)
        {

            EpicCards epics = EpicCards.NoEpic;

            if (cards.Exists(c => c.Suit == PokerConst.Spade &&
                              c.Value == PokerConst.ShangValve - 2))

                epics = EpicCards.Bailaojia;



            if (cards.Where(c => c.Suit == PokerConst.Spade &&
                                 c.Value >= PokerConst.ShangValve - 2).Count() == 3)

                epics = EpicCards.ShunFengQi;


            if (cards.Where(c => c.Value == 1).Count() == 4)
            {
                if (epics == EpicCards.ShunFengQi)
                    epics = EpicCards.ShunFeng4Qu;
                else
                    epics = EpicCards.FourQu;
            }


            if (cards.Where(c => c.Value == EngineTool.GetShangValve(c.Suit) - 1).Count() == 4)
            {
                if (epics == EpicCards.ShunFengQi)
                    return EpicCards.ShunFeng4Jian;
                else
                    epics = EpicCards.FourJian;
            }

            return epics;
        }

        #region 在手杂色
        //顺风旗
        public static CardType ShunFeng(List<Card> wincards)
        {

            return CardSeyang.ShunFeng(wincards);

        }

        //顺风旗
        public static CardType FoChiJiao(List<Card> wincards)
        {

            return CardSeyang.FoChiJiao(wincards);

        }

        //四趣
        public static CardType FuLuShouXi(List<Card> wincards)
        {

            return CardSeyang.FuLuShouXi(wincards);

        }

        //四肩在手
        public static CardType FourJian(List<Card> wincards)
        {

            return CardSeyang.FourJian(wincards);

        }
        #endregion
        //香炉脚
        public static CardType XiangLuJiao(List<Card> c1, List<Card> c2, List<Card> c3, List<Card> c4)
        {
            List<int> lNum = new List<int>() { c1.Count, c2.Count, c3.Count, c4.Count };
            if (lNum.Exists(num => num < 1 || (num > 2 && num < 5) || num > 5)) //一家5桌 3家各1桌
                return null;
            if (!lNum.Exists(num => num == 5) || !lNum.Exists(num => num == 1))
                return null;

            List<string> s1 = new List<string>(),
                s2 = new List<string>(),
                s3 = new List<string>(),
               s4 = new List<string>();
            c1.ForEach(c => s1.Add(c.SValue()));
            c2.ForEach(c => s2.Add(c.SValue()));
            c3.ForEach(c => s3.Add(c.SValue()));
            c4.ForEach(c => s4.Add(c.SValue()));
            return CardSeyang.XiangLuJiao(s1, s2, s3, s4);
        }
        #region  求整型数组中的最大值
        public static int MaxValue(int[] Array)
        {
            int max = Array[0];
            int pos = 0;
            for (int i = 1; i < Array.Length; i++)
            {
                if (Array[i] > max)
                {
                    max = Array[i];
                    pos = i;
                }
                else if (Array[i] == max)
                    pos = -1;
            }
            return pos;
        }
        #endregion

        //检查灭张

        public static Card[] ReplaceCard(Card[] cards, Card c1, Card c2)
        {
            List<Card> lcards = new List<Card>(cards);
            for (int i = 0; i < cards.Length; i++)
            {
                if (cards[i].Suit == c1.Suit && cards[i].Value == c1.Value)
                    cards[i] = c2;
            }
            return cards;
        }
        #region  升代  露面底牌为赏肩百趣者 则需挨近之牌代替 副极为极 肩升赏 次肩升肩 九十升百千万
        public static Card FindReplaceFace(Card c)
        {
            Card c0 = new Card();
            switch (c.Value)
            {
                case 1:
                    c0.Value = 2;
                    c0.Suit = c.Suit;
                    break;
                default: 
                    c0.Value = c.Value;
                    c0.Suit = c.Suit;
                    break;
            }
            if (c.Suit == PokerConst.Spade)
            {
                switch (c.Value)
                {
                    case 9:
                    case 10:
                    case 11:
                        c0.Value = 8;
                        c0.Suit = c.Suit;
                        break;
                    case 8:
                        c0.Value = 7;
                        c0.Suit = c.Suit;
                        break;
                    
                    default:
                        c0.Value = c.Value;
                        c0.Suit = c.Suit;
                        break;
                }
            }
            if (c.Suit == PokerConst.Diamond || c.Suit == PokerConst.Heart)
            {
                switch (c.Value)
                {
                    case 9:
                        c0.Value = 8;
                        c0.Suit = c.Suit;
                        break;
                    case 8:
                        c0.Value = 7;
                        c0.Suit = c.Suit;
                        break;
                    default:
                        c0.Value = c.Value;
                        c0.Suit = c.Suit;
                        break;
                }

            }
            if (c.Suit == PokerConst.Club)
            {
                switch (c.Value)
                {
                    case 11:
                        c0.Value = 10;
                        c0.Suit = c.Suit;
                        break;
                    case 10:
                        c0.Value = 9;
                        c0.Suit = c.Suit;
                        break;
                    default:
                        c0.Value = c.Value;
                        c0.Suit = c.Suit;
                        break;
                }

            }
            return c0;
        }
        #endregion

        #region 起手提示
        public static string GetStartPlayerInfo(List<List<Card>> llc, List<string> lname, Card c)
        {
            string sInfo = " 面張為" + c.MdName() + "\r\n 由";
            int iOrder = c.Value % 4;
            switch (iOrder)
            {
                case 0:
                    sInfo += "四八家" + lname[0];
                    break;
                case 1:
                    sInfo += "一五家" + lname[1];
                    break;
                case 2:
                    sInfo += "三七家" + lname[2];
                    break;
                case 3:
                    sInfo += "二六家" + lname[3];
                    break;
            }
            sInfo += "起手出牌. \r\n";
            Card oldc = new Card(c.Suit,c.Value);
            Card newc = FindReplaceFace(c);
            if (newc.Value != oldc.Value)
            {
                sInfo += c.MdName() + "由在手" + newc.MdName() + "昇代.";
            }
            return sInfo;
        }
        #endregion

        #region 斗牌结束后 由最后接家开始看冲
        public static List<CardType> KanChong(List<Card> wincards, List<Card> basecards)
        {
            List<CardType> lct = new List<CardType>();
            CardType ct = new CardType();
            //兄弟冲
            for (int i = 0; i < basecards.Count; i++)
            {
                if (i == 0)
                    ct = EngineTool.XiongDiChong(wincards, basecards[i]);
                else if (ct != null)
                {
                    lct.Add(ct);

                    ct = EngineTool.XiongDiChong(wincards, basecards[i], ct.Weight);
                }
            }
            for (int j = 0; j < lct.Count; j++) { basecards.RemoveAt(j); }


            return lct;
        }

        #endregion
    }
}