using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Model
{

    public class CardType
    {
        // 牌型关键字（无花色无顺序）
        public string CardKey
        {
            get; set;
        }
        // 牌型权重值
        public int Weight
        {
            get; set;
        }
        // 牌型名
        public string Name
        {
            get; set;
        }

        private int jifen = 0;
        public int Jifen
        {
            get { return jifen; }
            set { jifen = value; }
        }


    }


    public class ListDistinct : IEqualityComparer<CardType>
    {
        public bool Equals(CardType ct1, CardType ct2)
        {
            return (ct1.Name == ct2.Name && ct1.CardKey == ct2.CardKey);
        }

        public int GetHashCode(CardType ct)
        {
            return ct == null ? 0 : ct.ToString().GetHashCode();
        }
    }
    public class CardSeyang
    {
        public static List<String> mdSuit = new List<string> { PokerConst.Club, PokerConst.Diamond,
                                                               PokerConst.Heart, PokerConst.Spade };
        public static List<String> CardValuesmd = new List<string> { "A", "2", "3", "4", "5", "6", "7", "8", "9", "Q", "K" };

        public static List<Card> lhong = new List<Card> { Cardsmd[39],Cardsmd[38],
                                                          Cardsmd[37],Cardsmd[28],//九红集合
                                                          Cardsmd[27],Cardsmd[19] ,
                                                          Cardsmd[18] ,Cardsmd[10],Cardsmd[9] };

        public static List<Card> Cardsmd
        {
            get
            {
                List<Card> sourceCards = new List<Card>();
                string[] suit = {
            PokerConst.Club,//方块
            PokerConst.Diamond,//梅花
            PokerConst.Heart,//红桃
            PokerConst.Spade};//黑桃
                int count = 11;
                for (int i = 0; i < 4; i++)
                {
                    if (i == 1 || i == 2) count = 9;//方块 黑桃11张 梅花 红桃 9张
                    else count = 11;
                    for (int j = 1; j <= count; j++)
                    {
                        sourceCards.Add(new Card(suit[i], j));//每种花色生成13张
                    }
                }

                return sourceCards;
            }
        }

        #region 零:色样组合基本单元：赏 肩 百 趣 红 及各种基础函数
        #region  赏 (牌型key - 牌型entity)  共4种
        private static Dictionary<string, List<CardType>> shang = null;

        public static Dictionary<string, List<CardType>> Shang
        {
            get
            {
                if (shang == null)
                {
                    shang = new Dictionary<string, List<CardType>>();
                    int weight = 1;

                    foreach (Card card in Cardsmd)
                    {

                        if (card.SValue() == Cardsmd[39].SValue() ||
                              card.SValue() == Cardsmd[28].SValue() ||
                              card.SValue() == Cardsmd[19].SValue() ||
                              card.SValue() == Cardsmd[10].SValue())
                        {
                            string key = card.SValue();
                            shang.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.Shang } });
                        }
                    }
                }
                return shang;
            }
        }
        #endregion

        #region  肩 (牌型key - 牌型entity)  共4种
        private static Dictionary<string, List<CardType>> jian = null;

        public static Dictionary<string, List<CardType>> Jian
        {
            get
            {
                if (jian == null)
                {
                    jian = new Dictionary<string, List<CardType>>();
                    int weight = 1;

                    foreach (Card card in Cardsmd)
                    {
                        if (card.SValue() == Cardsmd[38].SValue() ||
                            card.SValue() == Cardsmd[27].SValue() ||
                            card.SValue() == Cardsmd[18].SValue() ||
                            card.SValue() == Cardsmd[9].SValue())
                        {
                            string key = card.SValue();
                            jian.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.Shang } });
                        }
                    }
                }
                return jian;
            }
        }
        #endregion

        #region  极 (牌型key - 牌型entity)  共4种
        private static Dictionary<string, List<CardType>> ji = null;

        public static Dictionary<string, List<CardType>> Ji
        {
            get
            {
                if (ji == null)
                {
                    ji = new Dictionary<string, List<CardType>>();
                    int weight = 1;

                    foreach (Card card in Cardsmd)
                    {
                        if (card.SValue() == Cardsmd[29].SValue() ||
                            card.SValue() == Cardsmd[20].SValue() ||
                            card.SValue() == Cardsmd[11].SValue() ||
                            card.SValue() == Cardsmd[0].SValue())
                        {
                            string key = card.SValue();
                            ji.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.Ji } });
                        }
                    }
                }
                return ji;
            }
        }
        #endregion

        #region  百 (牌型key - 牌型entity)  仅1种
        private static Dictionary<string, List<CardType>> bai = null;

        public static Dictionary<string, List<CardType>> Bai
        {
            get
            {
                if (bai == null)
                {
                    bai = new Dictionary<string, List<CardType>>();
                    int weight = 1;

                    foreach (Card card in Cardsmd)
                    {
                        if (card.SValue() == Cardsmd[37].SValue())
                        {
                            string key = card.SValue();
                            bai.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.Bai } });
                        }
                    }
                }
                return bai;
            }
        }
        #endregion


        #region 副极 ：八文  二索  二贯  三十万四种
        private static Dictionary<string, List<CardType>> fuji = null;

        public static Dictionary<string, List<CardType>> Fuji
        {
            get
            {
                if (fuji == null)
                {
                    fuji = new Dictionary<string, List<CardType>>();
                    int weight = 1;

                    foreach (Card card in Cardsmd)
                    {
                        if (card.SValue() == Cardsmd[30].SValue() ||
                            card.SValue() == Cardsmd[21].SValue() ||
                            card.SValue() == Cardsmd[12].SValue() ||
                            card.SValue() == Cardsmd[1].SValue())
                        {
                            string key = card.SValue();
                            fuji.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.Fuji } });
                        }
                    }
                }
                return fuji;
            }
        }
        #endregion

        #region 雌突 ：五贯  六贯 八贯 3种
        private static Dictionary<string, List<CardType>> citu = null;

        public static Dictionary<string, List<CardType>> Citu
        {
            get
            {
                if (citu == null)
                {
                    citu = new Dictionary<string, List<CardType>>();
                    int weight = 1;

                    foreach (Card card in Cardsmd)
                    {
                        if (card.SValue() == Cardsmd[27].SValue() ||
                            card.SValue() == Cardsmd[25].SValue() ||
                            card.SValue() == Cardsmd[24].SValue())
                        {
                            string key = card.SValue();
                            citu.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.Citu } });
                        }
                    }
                }
                return citu;
            }
        }
        #endregion

        #region 小突 ：九十万 + （五 六 八）万之一上桌 2注
        public static CardType Xiaotu(List<Card> lcards)
        {
            List<string> lcitu = SelectSameCards(Citu.Keys.ToList(), 1);

            foreach (string s in lcitu)
            {
                if (lcards.Count > 1 && lcards.Exists(c => c.SValue().Equals(s))
                && lcards.Exists(c => c.SValue().Equals(Cardsmd[36].SValue())))
                    return new CardType() { CardKey = Cardsmd[36].SValue() + "-" + s, Weight = 1, Name = CardsTypeMdFan.XiaoTu, Jifen = 2 };

            }
            return null;
        }
        #endregion

        #region 吊百 ：百老未上且未正本        1注 

        public static CardType Diaobai(List<Card> lcards)
        {
            if (lcards.Count >= 2 ||//已正本
                lcards.Exists(c => c.SValue() == Cardsmd[37].SValue()))//百老已上桌
                return null;

            return new CardType() { CardKey = Cardsmd[37].SValue(), Weight = 1, Name = CardsTypeMdFan.DiaoBai, Jifen = 1 };
        }

        #endregion


        #region 正本百 ：百老未上且正本        2注 

        public static CardType ZhengBenBai(List<Card> lcards)
        {
            if (lcards.Count < 2 ||//已正本
                lcards.Exists(c => c.SValue() == Cardsmd[37].SValue()))//百老已上桌
                return null;

            return new CardType() { CardKey = Cardsmd[37].SValue(), Weight = 1, Name = CardsTypeMdFan.ZhengBenBai, Jifen = 2 };
        }

        #endregion

        #region 大活百 ：百老上桌且正本        3注 敲门1注

        public static CardType Dahuobai(List<Card> lcards)
        {
            if (lcards.Count < 2 ||//未正本
                !lcards.Exists(c => c.SValue() == Cardsmd[37].SValue()))//百老未上桌
                return null;

            return new CardType() { CardKey = Cardsmd[37].SValue(), Weight = 1, Name = CardsTypeMdFan.DaHuoBai, Jifen = 3 };
        }

        #endregion

        #region 大活百突 ：百老 + 雌突（五 六 八）万之一上桌 6注 敲门2注


        public static CardType Dahuobaitu(List<Card> lcards)
        {
            List<string> lcitu = SelectSameCards(Citu.Keys.ToList(), 1);

            foreach (string s in lcitu)
            {
                if (lcards.Count > 1 && lcards.Exists(c => c.SValue() == Cardsmd[37].SValue())
                    && lcards.Exists(c => c.SValue().Equals(s)))
                    return new CardType() { CardKey = Cardsmd[37].SValue() + "-" + s, Weight = 1, Name = CardsTypeMdFan.DaHuoBaiTu, Jifen = 6 };

            }
            return null;
        }

        #endregion

        #region 全突大活 ：百老 九十 + 雌突（五 六 八）万全部上桌 =》5牌  32注 敲门16注

        public static CardType Quantudahuo(List<Card> lcards)
        {
            //全突大活包含5张牌，少于5张则不是
            if (lcards.Count < 5)
                return null;

            List<string> lkey = new List<string>();
            lkey.Add(Cardsmd[37].SValue());
            lkey.Add(Cardsmd[36].SValue());
            lkey = lkey.Union(Citu.Keys.ToList()).ToList();


            foreach (string s in lkey)
            {
                if (!lcards.Exists(c => c.SValue().Equals(s)))
                    return null;
            }
            string key = string.Join("-", lkey.ToArray());
            return new CardType { CardKey = key, Name = CardsTypeMdFan.QuanTuDaHuo, Weight = 1, Jifen = 32 };

        }

        #endregion
        #region  红：赏、肩、百 (牌型key - 牌型entity)  9种
        private static Dictionary<string, List<CardType>> hong = null;

        public static Dictionary<string, List<CardType>> Hong
        {
            get
            {
                if (hong == null)
                {
                    hong = new Dictionary<string, List<CardType>>();
                    int weight = 1;

                    foreach (Card card in Cardsmd)
                    {
                        string s = card.SValue();
                        if (IsHong(s))
                            hong.Add(s, new List<CardType> { new CardType() { CardKey = s, Weight = weight++, Name = CardsTypeMdFan.Hong } });
                    }
                }
                return hong;
            }
        }
        #endregion

        #region 判断是否红张
        public static bool IsHong(string s)
        {

            //4种花色的所有红张集合
            string key = Cardsmd[39].SValue() + "-" + Cardsmd[38].SValue() + "-"
                + Cardsmd[37].SValue() + "-" + Cardsmd[28].SValue() + "-"
                + Cardsmd[27].SValue() + "-" + Cardsmd[19].SValue() + "-"
                + Cardsmd[18].SValue() + "-" + Cardsmd[10].SValue() + "-" + Cardsmd[9].SValue();
            if (key.Contains(s))
                return true;
            else return false;
        }
        #endregion


        #region 判断是否全红
        public static bool IsAllHong(List<string> scards)
        {

            //4种花色的所有红张集合
            string key = Cardsmd[39].SValue() + "-" + Cardsmd[38].SValue() + "-"
               + Cardsmd[37].SValue() + "-" + Cardsmd[28].SValue() + "-"
               + Cardsmd[27].SValue() + "-" + Cardsmd[19].SValue() + "-"
               + Cardsmd[18].SValue() + "-" + Cardsmd[10].SValue() + "-" + Cardsmd[9].SValue();


            foreach (string s in scards)
            {
                if (!key.Contains(s))
                    return false;
            }
            return true;
        }
        #endregion

        #region 判断是否全是錦张
        public static bool isAllJin(List<string> scards)
        {
            List<string> ljin = new List<string>(Ji.Keys.ToList());
            ljin.AddRange(Jian.Keys.ToList());
            ljin.AddRange(Bai.Keys.ToList());
            ljin.AddRange(Shang.Keys.ToList());

            foreach (string s in scards)
            {
                if (!ljin.Contains(s))
                    return false;
            }
            return true;
        }

        #endregion

        #region 求赏肩极百数目
        public static int CountKeys(List<Card> lc, List<string> lkey)
        {
            int n = 0;

            foreach (string s in lkey)
            {
                if (lc.Exists(c => c.SValue().Equals(s)))
                    n++;
            }
            return n;
        }

        public static int CountKeys(List<string> ls, List<string> lkey)
        {
            int n = 0;

            foreach (string s in lkey)
            {
                if (ls.Exists(c => c.Equals(s)))
                    n++;
            }
            return n;
        }
        public static int CountShangs(List<Card> lc)
        {
            int n = 0;
            List<string> lshang = Shang.Keys.ToList();

            foreach (string s in lshang)
            {
                if (lc.Exists(c => c.SValue().Equals(s)))
                    n++;
            }
            return n;
        }

        public static int CountJians(List<Card> lc)
        {
            int n = 0;
            List<string> ljian = Jian.Keys.ToList();

            foreach (string s in ljian)
            {
                if (lc.Exists(c => c.SValue().Equals(s)))
                    n++;
            }
            return n;
        }

        public static int CountJis(List<Card> lc)
        {
            int n = 0;
            List<string> lji = Ji.Keys.ToList();
            foreach (string s in lji)
            {
                if (lc.Exists(c => c.SValue().Equals(s)))
                    n++;
            }
            return n;
        }

        public static int CountBai(List<Card> lc)
        {
            int n = 0;
            List<string> lbai = Bai.Keys.ToList();
            foreach (string s in lbai)
            {
                if (lc.Exists(c => c.SValue().Equals(s)))
                    n++;
            }
            return n;
        }
        #endregion


        #region 清张组合1-n张
        public static List<string> QingZhangZuHe(int n)
        {
            List<string> lqing = new List<string>();
            foreach (Card card in Cardsmd)
            {
                if (!IsHong(card.SValue()) && !ji.ContainsKey(card.SValue()))
                {
                    lqing.Add(card.SValue());
                }
            }
            return SelectSameCards(lqing, n);

        }
        #endregion

        #region 按张数获取对子或豹子 count = 2对子 3豹子 4豆  
        public static List<string> SelectSameCards(List<string> lcards, int n)
        {
            List<string> lsr = new List<string>();

            var result = lcards.Select(x => new string[] { x });
            for (int i = 0; i < n - 1; i++)
            {
                result = result.SelectMany(x => lcards.Where(y => y.CompareTo(x.First()) < 0).Select(y => new string[] { y }.Concat(x).ToArray()));
            }

            string stmp = null;
            foreach (var v in result)
            {
                stmp = EngineTool.FormatCardStrmd(string.Join("-", v));
                if (!lsr.Contains(stmp))
                    lsr.Add(stmp);
            }

            return lsr;
        }


        #endregion

        #region 顺风旗： 百老 千僧 红万在手或上桌  在手*1 上桌*2 带柄上桌递加 (需分别计算是否百千万上桌)
        public static CardType ShunFeng(List<Card> ls)
        {
            int thejifen = 1;
            string key1 = Cardsmd[39].SValue(), key2 = Cardsmd[38].SValue(), key3 = Cardsmd[37].SValue();
            string key = key1 + "-" + key2 + "-" + key3;

            int winjifen = ls.Where(c => c.Suit == PokerConst.Spade &&
                                    c.Value >= PokerConst.ShangValve - 2).Count();

            if (winjifen == 3)
                winjifen += ls.Where(c => c.Suit == PokerConst.Spade &&
                                    c.Value < PokerConst.ShangValve - 2).Count();
            thejifen += winjifen;


            return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.ShunFengQi, Jifen = thejifen };
        }
        #endregion

        #region 清一色
        public static List<string> TongHuas(int color, int colorcount)
        {

            //4种花色的所有牌分别列出             
            int m = CardValuesmd.Count;
            List<string> ls = new List<string>();
            for (int j = 0; j < CardValuesmd.Count; j++)
            {
                if ((color == 1 || color == 2) && j > 8)
                {
                    m -= 2;
                    break;
                }
                //svalues += mdSuit[color] + "*" + CardValuesmd[j] + "-";
                ls.Add(mdSuit[color] + "*" + CardValuesmd[j]);

            }
            List<string> lsout = SelectSameCards(ls, colorcount);
            //svalues = EngineTool.FormatCardStrmd(svalues);
            //String key = null;
            //List<string> ls = SelectSameCards(svalues, colorcount);new ();
            //List < List < int >> lli = CardZuHe(colorcount, m);
            //foreach (List<int> li in lli )
            //{
            //    foreach(int x in li)
            //    {
            //        key += mdSuit[color] + "*"+ x.ToString()+"-";
            //    }
            //    ls.Add(key);
            //    key = null;
            //}


            return lsout;
        }
        #endregion

        #region 同花顺
        public static List<string> ShunZi(int suit, int count)
        {
            //4种花色的所有牌分别列出 
            string key = null;
            List<string> lkey = new List<string>();
            int icount = 11 - count;
            if ((mdSuit[suit] == PokerConst.Diamond || mdSuit[suit] == PokerConst.Heart))
                icount -= 2;
            for (int i = 1; i < icount; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    if (j < count - 1)
                        key += mdSuit[suit] + "*" + (j + i).ToString() + "-";
                    else if (j == count - 1)
                        key += mdSuit[suit] + "*" + (j + i).ToString();
                }
                lkey.Add(key);
                key = null;
            }

            return lkey;
        }
        #endregion

        #region 求n个数之和等于m的所有组合
        public static IEnumerable<IEnumerable<int>> GetZuhe(int sum, int count)
        {
            return GetZuhe(sum, count, sum);
        }
        public static IEnumerable<IEnumerable<int>> GetZuhe(int sum, int count, int maxNum)
        {
            if (count < 1)
                throw new InvalidOperationException();
            else if (sum <= 0)
                yield break;
            else if (count == 1)
            {
                if (sum <= maxNum)
                    yield return new List<int> { sum };
            }
            else
                foreach (var x in from n in Enumerable.Range(1, maxNum)
                                  let sub = GetZuhe(sum - n, count - 1, n)
                                  from lst in sub
                                  select lst.Concat(new int[] { n }))
                    yield return x;
        }

        public static List<List<int>> CardZuHe(int n, int m, bool iszero = false)
        {
            int istart = 1;
            if (iszero)
                istart = 0;
            List<int> li = new List<int>();
            List<List<int>> lc = new List<List<int>>();

            if (n == 2)
            {
                for (int i = istart; i < m; ++i)
                {
                    li.Add(i);
                    li.Add(m - i);
                    lc.Add(li);

                }
                return lc;
            }
            for (int beg = 1; beg <= m - 2; ++beg)
            {
                li.Add(beg);
                List<List<int>> lc1 = CardZuHe(n - 1, m - beg);

                for (int j = 0; j < lc1.Count; j++)
                {
                    lc.Add(lc1[j]);
                }
            }
            return lc;
        }
        #endregion

        #region 顶配(底配)同花顺
        public static string TopShunZi(int color, int count, bool istop)
        {
            //4种花色的所有牌分别列出 
            string key = null;
            int icount = 11 - count;
            if ((color == 1 || color == 2))
                icount -= 2;

            if (istop)
            {
                for (int i = icount; i < CardValuesmd.Count; i++)
                {
                    key += mdSuit[color] + "*" + CardValuesmd[i] + "-";
                }
            }
            else
            {
                for (int i = 0; i < icount; i++)
                {
                    key += mdSuit[color] + "*" + CardValuesmd[i] + "-";
                }
            }

            return key;
        }
        #endregion
        #endregion

        #region 一:天胜：免斗色样 不需打牌

        #region  天地交泰：四赏四极 红万9贯9索空文 20万1贯1索9文 (牌型key - 牌型entity)  共1种  60贺
        private static Dictionary<string, List<CardType>> tiandijiaotai = null;

        public static Dictionary<string, List<CardType>> TianDiJiaoTai
        {
            get
            {
                if (tiandijiaotai == null)
                {
                    tiandijiaotai = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    string key = "";


                    /*  0 九文 1  2 3 4 5 6 7 8一文 9 枝花 10空堂    11一索 12 13 14 15 16 17    18八索 19九索
                        20一贯 21 22 23 24 25 26    27八贯 28九贯     29二十万 30 31 32 33 34 35 36九十 37百老 38千僧 39红万                 
                    */
                    //天地交泰：四赏四级 红万9贯9索空文 20万1贯1索9文
                    key = Cardsmd[39].SValue() + "-" + Cardsmd[29].SValue() + "-"
                        + Cardsmd[28].SValue() + "-" + Cardsmd[20].SValue() + "-"
                            + Cardsmd[19].SValue() + "-" + Cardsmd[11].SValue() + "-"
                            + Cardsmd[10].SValue() + "-" + Cardsmd[0].SValue();
                    tiandijiaotai.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.TianDiJiaoTai, Jifen = 60 } });

                }
                return tiandijiaotai;
            }
        }
        #endregion

        #region  七红醉杨妃  七红+20万  50贺
        private static Dictionary<string, List<CardType>> qihongzuiyangfei = null;

        public static Dictionary<string, List<CardType>> QiHongZuiYangFei
        {
            get
            {
                if (qihongzuiyangfei == null)
                {
                    qihongzuiyangfei = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    string key = null;

                    //七红醉杨妃：（九红 红万9贯9索空文 千僧8贯8索枝花百老）之7 + 20万

                    key = Cardsmd[39] + "-" + Cardsmd[38] + "-" + Cardsmd[37] + "-" + Cardsmd[28] + "-"//九红集合
                           + Cardsmd[27] + "-" + Cardsmd[19] + "-" + Cardsmd[18] + "-" + Cardsmd[10] + "-" + Cardsmd[9];

                    List<string> ls7 = SelectSameCards(key.Split('-').ToList(), 7);
                    foreach (string skey7 in ls7)
                    {
                        string skey = EngineTool.FormatCardStrmd(skey7 + "-" + Cardsmd[29].SValue());
                        qihongzuiyangfei.Add(skey, new List<CardType> { new CardType() { CardKey = skey, Weight = weight++, Name = CardsTypeMdFan.QiHongZuiYangFei, Jifen = 50 } });
                    }
                }
                return qihongzuiyangfei;
            }
        }
        #endregion

        #region  七连同花顺：（同花顺清一色）之7  10贺
        private static Dictionary<string, List<CardType>> qishun = null;

        public static Dictionary<string, List<CardType>> QiShun
        {
            get
            {
                if (qishun == null)
                {
                    qishun = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    List<string> lkey = new List<string>();

                    for (int i = 0; i < mdSuit.Count; i++)
                    {
                        lkey = ShunZi(i, 7);
                        foreach (string key in lkey)
                            qishun.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.QiShun, Jifen = 10 } });
                    }
                }
                return qishun;
            }
        }
        #endregion

        #region  八连同花顺：（同花顺清一色）之8  20贺
        private static Dictionary<string, List<CardType>> bashun = null;

        public static Dictionary<string, List<CardType>> BaShun
        {
            get
            {
                if (bashun == null)
                {
                    bashun = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    List<string> lkey = null;

                    for (int i = 0; i < mdSuit.Count; i++)
                    {
                        lkey = ShunZi(i, 8);
                        foreach (string key in lkey)
                            bashun.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.BaShun, Jifen = 20 } });
                    }
                }
                return bashun;
            }
        }
        #endregion

        #region  八大：八张大牌，任何一张都没人接得住  8贺
        private static Dictionary<string, List<CardType>> bada = null;

        public static Dictionary<string, List<CardType>> BaDa
        {
            get
            {
                if (bada == null)
                {
                    bada = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    string key = null;
                    int i = 0;

                    //List<List<int>> licard = new List<List<int>>();
                    //licard = CardZuHe(4, 8, false);//八张 四种花色
                    foreach (var licard in GetZuhe(4, 8))
                    //for (int i = 0; i < licard.Count; i++)
                    {
                        //List<int> li = licard[i];
                        //for(int j = 1; j < li.Count; j++)
                        foreach (var li in licard)
                        {
                            key = TopShunZi(i, li, true);
                            bada.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.BaDa, Jifen = 8 } });
                            i++;
                        }
                    }
                }
                return bada;
            }
        }
        #endregion

        #region  八小：八张牌小,接不上别家任何一张牌   8贺
        private static Dictionary<string, List<CardType>> baxiao = null;

        public static Dictionary<string, List<CardType>> BaXiao
        {
            get
            {
                if (baxiao == null)
                {
                    baxiao = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    string key = null;
                    int i = 0;

                    foreach (var licard in GetZuhe(4, 8))
                    {
                        foreach (var li in licard)
                        {
                            key = TopShunZi(i, li, false);
                            baxiao.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.BaXiao, Jifen = 8 } });
                            i++;
                        }
                    }




                }

                return baxiao;
            }
        }
        #endregion

        #region  七同：（清一色）之7   7贺
        private static Dictionary<string, List<CardType>> qitong = null;

        public static Dictionary<string, List<CardType>> QiTong
        {
            get
            {
                if (qitong == null)
                {
                    qitong = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    List<string> lkey = new List<string>();

                    for (int i = 0; i < mdSuit.Count; i++)
                    {
                        lkey = TongHuas(i, 7);
                        foreach (string key in lkey)
                            qitong.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.QiTong, Jifen = 7 } });
                    }
                }
                return qitong;
            }
        }
        #endregion

        public static List<CardType> QiTongHua(string scards)
        {
            List<CardType> lct = new List<CardType>();
            foreach (string s in QiTong.Keys.ToList())
            {
                if (scards.Contains(s))
                    lct.AddRange(QiTong[s]);
            }
            return lct;
        }

        #region  八同：（清一色）之8         8贺
        private static Dictionary<string, List<CardType>> batong = null;

        public static Dictionary<string, List<CardType>> BaTong
        {
            get
            {
                if (batong == null)
                {
                    batong = new Dictionary<string, List<CardType>>();
                    int weight = 1;

                    for (int i = 0; i < mdSuit.Count; i++)
                    {
                        List<string> lkey = TongHuas(i, 8);
                        foreach (string key in lkey)
                            batong.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.BaTong, Jifen = 8 } });
                    }
                }
                return batong;
            }
        }
        #endregion


        #region  八红：（九红 红万9贯9索空文 千僧8贯8索枝花百老）之8          8贺
        private static Dictionary<string, List<CardType>> bahong = null;

        public static Dictionary<string, List<CardType>> BaHong
        {
            get
            {
                if (bahong == null)
                {
                    bahong = new Dictionary<string, List<CardType>>();
                    int weight = 1;

                    List<string> lshong = new List<string>();
                    foreach (Card c in lhong)
                        lshong.Add(c.SValue());
                    List<string> lkey = SelectSameCards(lshong, 8);
                    foreach (string key in lkey)
                        bahong.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.BaHong, Jifen = 8 } });

                }

                return bahong;
            }
        }
        #endregion

        #region  七红：（九红 红万9贯9索空文 千僧8贯8索枝花百老）之7          7贺
        private static Dictionary<string, List<CardType>> qihong = null;

        public static Dictionary<string, List<CardType>> QiHong
        {
            get
            {
                if (qihong == null)
                {
                    qihong = new Dictionary<string, List<CardType>>();
                    int weight = 1;

                    List<string> lshong = new List<string>();
                    foreach (Card c in lhong)
                        lshong.Add(c.SValue());
                    List<string> lkey = SelectSameCards(lshong, 7);
                    foreach (string key in lkey)
                        qihong.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.QiHong, Jifen = 7 } });

                }
                return qihong;
            }
        }
        #endregion

        #region  六红：（九红 红万9贯9索空文 千僧8贯8索枝花百老）之6          6贺
        private static Dictionary<string, List<CardType>> liuhong = null;

        public static Dictionary<string, List<CardType>> LiuHong
        {
            get
            {
                if (liuhong == null)
                {

                    liuhong = new Dictionary<string, List<CardType>>();
                    int weight = 1;

                    List<string> lshong = new List<string>();
                    foreach (Card c in lhong)
                        lshong.Add(c.SValue());
                    List<string> lkey = SelectSameCards(lshong, 6);
                    foreach (string key in lkey)
                        liuhong.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.LiuHong, Jifen = 6 } });
                }
                return liuhong;
            }
        }
        #endregion

        #region  天人合一：四赏四肩 红万9贯9索空文 千僧8贯8索枝花        60贺
        private static Dictionary<string, List<CardType>> tianrenheyi = null;

        public static Dictionary<string, List<CardType>> TianRenHeYi
        {
            get
            {
                if (tianrenheyi == null)
                {
                    tianrenheyi = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    string key = null;

                    //天人合一：四赏四肩 红万9贯9索空文 千僧8贯8索枝花
                    key = Cardsmd[39].SValue() + "-" + Cardsmd[38].SValue() + "-"
                        + Cardsmd[28].SValue() + "-" + Cardsmd[27].SValue() + "-"
                            + Cardsmd[19].SValue() + "-" + Cardsmd[18].SValue() + "-"
                            + Cardsmd[10].SValue() + "-" + Cardsmd[9].SValue();
                    tianrenheyi.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.TianRenHeYi, Jifen = 60 } });

                }
                return tianrenheyi;
            }
        }
        #endregion

        #region  鸡犬升天：四极四副极         60贺
        private static Dictionary<string, List<CardType>> jiquanshengtian = null;

        public static Dictionary<string, List<CardType>> JiQuanShengTian
        {
            get
            {
                if (jiquanshengtian == null)
                {
                    jiquanshengtian = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    string key = null;

                    //天人合一：四赏四肩 红万9贯9索空文 千僧8贯8索枝花
                    key = Cardsmd[30].SValue() + "-" + Cardsmd[29].SValue() + "-"
                        + Cardsmd[21].SValue() + "-" + Cardsmd[20].SValue() + "-"
                            + Cardsmd[12].SValue() + "-" + Cardsmd[11].SValue() + "-"
                            + Cardsmd[1].SValue() + "-" + Cardsmd[0].SValue();
                    jiquanshengtian.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.JiQuanSehngTian, Jifen = 60 } });

                }
                return jiquanshengtian;
            }
        }
        #endregion

        #region  拗鸳鸯：八红上下四门 四赏四肩 红万千僧百老之二 9贯9索空文 8贯8索枝花     40贺
        private static Dictionary<string, List<CardType>> aoyuanyang = null;

        public static Dictionary<string, List<CardType>> AoYuanYang
        {
            get
            {
                if (aoyuanyang == null)
                {
                    aoyuanyang = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    string key = null;
                    List<string> ls = new List<string>();

                    ls.Add(Cardsmd[39].SValue() + "-" + Cardsmd[37].SValue() + "-");
                    ls.Add(Cardsmd[38].SValue() + "-" + Cardsmd[37].SValue() + "-");

                    foreach (string s in ls)
                    {
                        //天人合一：四赏四肩 红万9贯9索空文 千僧8贯8索枝花
                        key = s + Cardsmd[28].SValue() + "-" + Cardsmd[27].SValue() + "-"
                                + Cardsmd[19].SValue() + "-" + Cardsmd[18].SValue() + "-"
                                + Cardsmd[10].SValue() + "-" + Cardsmd[9].SValue();
                        aoyuanyang.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.AoYuanYang, Jifen = 40 } });
                    }
                }
                return aoyuanyang;
            }
        }
        #endregion

        #region  人杰地灵 (牌型key - 牌型entity)  共1种    四肩四级   50贺
        private static Dictionary<string, List<CardType>> renjiediling = null;

        public static Dictionary<string, List<CardType>> RenJieDiLing
        {
            get
            {
                if (renjiediling == null)
                {
                    renjiediling = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    string key = null;

                    //人杰地灵：四肩四级 千僧8贯8索枝花 20万1贯1索9文
                    key = Cardsmd[38].SValue() + "-" + Cardsmd[29].SValue() + "-"
                        + Cardsmd[27].SValue() + "-" + Cardsmd[20].SValue() + "-"
                            + Cardsmd[18].SValue() + "-" + Cardsmd[11].SValue() + "-"
                            + Cardsmd[9].SValue() + "-" + Cardsmd[0].SValue();
                    renjiediling.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.RenJieDiLing, Jifen = 50 } });

                }
                return renjiediling;
            }
        }
        #endregion

        #region  八幺：红万千僧百老一贯一索一文枝花空文   20贺
        private static Dictionary<string, List<CardType>> bayao = null;

        public static Dictionary<string, List<CardType>> BaYao
        {
            get
            {
                if (bayao == null)
                {
                    bayao = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    string key = null;

                    //八幺：红万千僧百老一贯一索一文枝花空文
                    key = Cardsmd[39].SValue() + "-" + Cardsmd[38].SValue() + "-"
                        + Cardsmd[37].SValue() + "-" + Cardsmd[20].SValue() + "-"
                           + Cardsmd[11].SValue() + "-" + Cardsmd[10].SValue() + "-"
                           + Cardsmd[9].SValue() + "-" + Cardsmd[8].SValue();
                    bayao.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.BaYao, Jifen = 20 } });
                }
                return bayao; ;
            }
        }
        #endregion

        #region  锦鸳鸯：八张全是红或极[趣]，且每门2张   40贺
        private static Dictionary<string, List<CardType>> jinyuanyang = null;

        public static Dictionary<string, List<CardType>> JinYuanYang
        {
            get
            {
                if (jinyuanyang == null)
                {
                    jinyuanyang = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    List<string> ls = new List<string>();// Hong.Keys.ToList().Union(Ji.Keys.ToList()).ToList<string>();
                    ls.AddRange(Hong.Keys.ToList());
                    ls.AddRange(Ji.Keys.ToList());
                    List<string> lsout = SelectSameCards(ls, 8);


                    foreach (string key in lsout)
                    {
                        List<string> lskey = key.Split('-').ToList();
                        //基于lambda表达式的写法
                        int clubcount = lskey.Count(s => s.Contains(PokerConst.Club));
                        int diamondcount = lskey.Count(s => s.Contains(PokerConst.Diamond));
                        int heartcount = lskey.Count(s => s.Contains(PokerConst.Heart));
                        int spadecount = lskey.Count(s => s.Contains(PokerConst.Spade));

                        if (clubcount == 2 && diamondcount == 2 && heartcount == 2 && spadecount == 2)
                            jinyuanyang.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.Jjjin, Jifen = 40 } });

                    }

                }
                return jinyuanyang; ;
            }
        }
        #endregion

        #region  片片锦：八张全是红或极[趣]  30贺
        private static Dictionary<string, List<CardType>> jjjin = null;

        public static Dictionary<string, List<CardType>> Jjjin
        {
            get
            {
                if (jjjin == null)
                {
                    jjjin = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    List<string> ls = new List<string>();// Hong.Keys.ToList().Union(Ji.Keys.ToList()).ToList<string>();
                    ls.AddRange(Hong.Keys.ToList());
                    ls.AddRange(Ji.Keys.ToList());
                    List<string> lsout = SelectSameCards(ls, 8);


                    foreach (string key in lsout)
                    {
                        jjjin.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.Jjjin, Jifen = 30 } });

                    }

                }
                return jjjin; ;
            }
        }
        #endregion

        #region  麒麟种：百老+四赏  16贺
        private static Dictionary<string, List<CardType>> qilinzhong = null;

        public static Dictionary<string, List<CardType>> QiLinZhong
        {
            get
            {
                if (qilinzhong == null)
                {
                    qilinzhong = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    List<string> ls = Shang.Keys.ToList().Union(Bai.Keys.ToList()).ToList<string>();
                    List<string> lsout = SelectSameCards(ls, 5);

                    foreach (string key in lsout)
                    {
                        qilinzhong.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.QiLingZhong, Jifen = 16 } });

                    }

                }
                return qilinzhong; ;
            }
        }
        #endregion

        #region  凤凰雏：百老+四肩 12贺
        private static Dictionary<string, List<CardType>> fenghuangchu = null;

        public static Dictionary<string, List<CardType>> FengHuangChu
        {
            get
            {
                if (fenghuangchu == null)
                {
                    fenghuangchu = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    List<string> ls = Jian.Keys.ToList().Union(Bai.Keys.ToList()).ToList<string>();
                    List<string> lsout = SelectSameCards(ls, 5);

                    foreach (string key in lsout)
                    {
                        fenghuangchu.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.FengHuangChu, Jifen = 12 } });
                    }

                }
                return fenghuangchu;
            }
        }
        #endregion

        #region 雪中炭：百老+四极   8贺
        private static Dictionary<string, List<CardType>> xuezhongtan = null;

        public static Dictionary<string, List<CardType>> XueZhongTan
        {
            get
            {
                if (xuezhongtan == null)
                {
                    xuezhongtan = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    List<string> ls = Ji.Keys.ToList().Union(Bai.Keys.ToList()).ToList<string>();
                    List<string> lsout = SelectSameCards(ls, 5);

                    foreach (string key in lsout)
                    {
                        xuezhongtan.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.XueZhongTan, Jifen = 8 } });
                    }

                }
                return xuezhongtan;
            }
        }
        #endregion

        #region 八红顺风旗：百老 千僧 红万 且八红张
        private static Dictionary<string, List<CardType>> shunfengqi8 = null;

        public static Dictionary<string, List<CardType>> ShunFengQi8
        {
            get
            {
                if (shunfengqi8 == null)
                {
                    shunfengqi8 = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    List<string> lshong = new List<string>();
                    foreach (Card c in lhong)
                        lshong.Add(c.SValue());
                    List<string> lsout = SelectSameCards(lshong, 8);

                    foreach (string key in lsout)
                    {

                        if (key.Contains(Cardsmd[37].SValue()) &&
                            key.Contains(Cardsmd[38].SValue()) &&
                            key.Contains(Cardsmd[39].SValue()))
                        {
                            shunfengqi8.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.ShunFengBaHong, Jifen = 8 } });
                        }
                    }

                }
                return shunfengqi8;
            }
        }
        #endregion

        #region 七红顺风旗：百老 千僧 红万  且 7红张
        private static Dictionary<string, List<CardType>> shunfengqi7 = null;

        public static Dictionary<string, List<CardType>> ShunFengQi7
        {
            get
            {
                if (shunfengqi7 == null)
                {
                    shunfengqi7 = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    List<string> lshong = new List<string>();
                    foreach (Card c in lhong)
                        lshong.Add(c.SValue());
                    List<string> lsout = SelectSameCards(lshong, 7);

                    foreach (string key in lsout)
                    {

                        if (key.Contains(Cardsmd[37].SValue()) &&
                            key.Contains(Cardsmd[38].SValue()) &&
                            key.Contains(Cardsmd[39].SValue()))
                        {
                            shunfengqi7.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.ShunFengQiHong, Jifen = 7 } });
                        }
                    }

                }
                return shunfengqi7;
            }
        }
        #endregion

        #region 六红顺风旗：百老 千僧 红万  且 6红张
        private static Dictionary<string, List<CardType>> shunfengqi6 = null;

        public static Dictionary<string, List<CardType>> ShunFengQi6
        {
            get
            {
                if (shunfengqi6 == null)
                {
                    shunfengqi6 = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    List<string> lshong = new List<string>();
                    foreach (Card c in lhong)
                        lshong.Add(c.SValue());
                    List<string> lsout = SelectSameCards(lshong, 6);

                    foreach (string key in lsout)
                    {

                        if (key.Contains(Cardsmd[37].SValue()) &&
                            key.Contains(Cardsmd[38].SValue()) &&
                            key.Contains(Cardsmd[39].SValue()))
                        {
                            shunfengqi6.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.ShunFengQiHong, Jifen = 6 } });
                        }
                    }

                }
                return shunfengqi6;
            }
        }
        #endregion

        #endregion


        #region 二、斗上正色样

        #region  1.皇会图(四大天王)：四赏 红万9贯9索空文  8贺 (牌型key - 牌型entity)  共1种
        private static Dictionary<string, List<CardType>> huanghuitu = null;

        public static Dictionary<string, List<CardType>> HuangHuiTu
        {
            get
            {
                if (huanghuitu == null)
                {
                    huanghuitu = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    string key = "";
                    //皇会图：四赏 红万9贯9索空文 
                    List<string> ls = SelectSameCards(Shang.Keys.ToList(), 4);
                    foreach (string s in ls)
                    {
                        key = EngineTool.FormatCardStrmd(s);
                        huanghuitu.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.HuangHuiTu, Jifen = 8 } });
                    }
                }
                return huanghuitu;
            }
        }

        public static CardType GetSeYangPlay4(List<Card> lcards, Dictionary<string, List<CardType>> dic)
        {
            //CardType ct = new CardType();

            foreach (string cards in dic.Keys.Distinct().ToList())//
            {
                List<string> lseyang = cards.Split('-').ToList();
                lseyang = lseyang.Distinct().ToList();//过滤重复项

                if (!(lseyang.Exists(s => s.Contains(PokerConst.Club))
                    && lseyang.Exists(s => s.Contains(PokerConst.Diamond))
                    && lseyang.Exists(s => s.Contains(PokerConst.Heart))
                    && lseyang.Exists(s => s.Contains(PokerConst.Spade))))
                    continue;
                int i = 0;
                foreach (string cardsy in lseyang)
                {
                    //如4张正色样中某张牌不存在于上桌牌组内   
                    if (lcards.Exists(card => card.SValue() == cardsy))
                        ++i;
                    else break;
                    if (i == lseyang.Count)
                        return dic[cards][0];
                }

            }
            return null;
        }
        #endregion

        #region  2.千钧柱(四大金刚)：四肩 千僧8贯8索枝花 16贺  (牌型key - 牌型entity)  共1种
        private static Dictionary<string, List<CardType>> qianjunzhu = null;

        public static Dictionary<string, List<CardType>> QianJunZhu
        {
            get
            {
                if (qianjunzhu == null)
                {
                    qianjunzhu = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    string key = "";

                    List<string> ls = SelectSameCards(Jian.Keys.ToList(), 4);
                    foreach (string s in ls)
                    {
                        key = EngineTool.FormatCardStrmd(s);
                        qianjunzhu.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.QianJunZhu, Jifen = 16 } });
                    }
                }
                return qianjunzhu;
            }
        }
        #endregion

        #region  3.花肚兜：一百三赏 12贺 (牌型key - 牌型entity)  共1种
        private static Dictionary<string, List<CardType>> huadudou = null;

        public static Dictionary<string, List<CardType>> HuaDuDou
        {
            get
            {
                if (huadudou == null)
                {
                    huadudou = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    string key = "";
                    List<string> ls0 = SelectSameCards(Shang.Keys.ToList(), 3);
                    List<string> ls1 = SelectSameCards(Bai.Keys.ToList(), 1);
                    foreach (string s0 in ls0)
                    {
                        foreach (string s1 in ls1)
                        {
                            key = EngineTool.FormatCardStrmd(s0 + "-" + s1);
                            huadudou.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.HuaDuDou, Jifen = 12 } });
                        }
                    }
                }
                return huadudou;
            }
        }
        #endregion

        #region 4. 花比肩：一百三肩 20贺 (牌型key - 牌型entity)  共1种
        private static Dictionary<string, List<CardType>> huabijian = null;

        public static Dictionary<string, List<CardType>> HuaBiJian
        {
            get
            {
                if (huabijian == null)
                {
                    huabijian = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    string key = null;

                    List<string> ls0 = SelectSameCards(Jian.Keys.ToList(), 3);
                    List<string> ls1 = SelectSameCards(Bai.Keys.ToList(), 1);
                    foreach (string s0 in ls0)
                    {
                        foreach (string s1 in ls1)
                        {
                            key = EngineTool.FormatCardStrmd(s0 + "-" + s1);
                            huabijian.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.HuaBiJian, Jifen = 20 } });
                        }
                    }
                }
                return huabijian;
            }
        }
        #endregion

        #region  5.巧四赏：一极三赏  12贺
        private static Dictionary<string, List<CardType>> qiaosishang = null;

        public static Dictionary<string, List<CardType>> QiaoSiShang
        {
            get
            {
                if (qiaosishang == null)
                {
                    qiaosishang = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    string key = "";
                    int jifen = 12;

                    List<string> ls0 = SelectSameCards(Shang.Keys.ToList(), 3);
                    List<string> ls1 = SelectSameCards(Ji.Keys.ToList(), 1);
                    foreach (string s0 in ls0)
                    {
                        foreach (string s1 in ls1)
                        {
                            //二十子极成四门色样 加5贺
                            if (s1 == Cardsmd[29].SValue()) jifen = 17;
                            else jifen = 12;

                            key = EngineTool.FormatCardStrmd(s0 + "-" + s1);
                            qiaosishang.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.QiaoSiShang, Jifen = jifen } });
                        }
                    }
                }
                return qiaosishang;
            }
        }
        #endregion

        #region  6.巧四肩：一极三肩  20贺
        private static Dictionary<string, List<CardType>> qiaosijian = null;

        public static Dictionary<string, List<CardType>> QiaoSiJian
        {
            get
            {
                if (qiaosijian == null)
                {
                    qiaosijian = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    string key = "";
                    int jifen = 20;

                    List<string> ls0 = SelectSameCards(Jian.Keys.ToList(), 3);
                    List<string> ls1 = SelectSameCards(Ji.Keys.ToList(), 1);
                    foreach (string s0 in ls0)
                    {
                        foreach (string s1 in ls1)
                        {
                            //二十子极成四门色样 加5贺
                            if (s1 == Cardsmd[29].SValue()) jifen = 25;
                            else jifen = 20;
                            key = EngineTool.FormatCardStrmd(s0 + "-" + s1);
                            qiaosijian.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.QiaoSiJian, Jifen = jifen } });
                        }
                    }
                }
                return qiaosijian;
            }
        }
        #endregion

        #region  7.天地分：2极2赏  24贺
        private static Dictionary<string, List<CardType>> tiandifen = null;

        public static Dictionary<string, List<CardType>> TianDiFen
        {
            get
            {
                if (tiandifen == null)
                {
                    tiandifen = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    string key = null;
                    int jifen = 24;

                    List<string> ls0 = SelectSameCards(Shang.Keys.ToList(), 2);
                    List<string> ls1 = SelectSameCards(Ji.Keys.ToList(), 2);
                    foreach (string s0 in ls0)
                    {
                        foreach (string s1 in ls1)
                        {
                            //二十子极成四门色样 加5贺
                            if (isHave20(s1)) jifen = 29;
                            else jifen = 24;
                            key = EngineTool.FormatCardStrmd(s0 + "-" + s1);
                            tiandifen.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.TianDiFen, Jifen = jifen } });
                        }
                    }
                }
                return tiandifen;
            }
        }
        #endregion

        #region  8.肩天地分：2极2肩  32贺
        private static Dictionary<string, List<CardType>> jiantiandifen = null;

        public static Dictionary<string, List<CardType>> JianTianDiFen
        {
            get
            {
                if (jiantiandifen == null)
                {
                    jiantiandifen = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    int jifen = 32;

                    string key = "";
                    List<string> ls0 = SelectSameCards(Jian.Keys.ToList(), 2);
                    List<string> ls1 = SelectSameCards(Ji.Keys.ToList(), 2);
                    foreach (string s0 in ls0)
                    {
                        foreach (string s1 in ls1)
                        {
                            //二十子极成四门色样 加5贺
                            if (isHave20(s1)) jifen = 37;
                            else jifen = 32;
                            key = EngineTool.FormatCardStrmd(s0 + "-" + s1);
                            jiantiandifen.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.JianTianDiFen, Jifen = jifen } });
                        }
                    }
                }
                return jiantiandifen;
            }
        }
        #endregion

        #region  7.小天地分：2极1肩1赏  28贺
        private static Dictionary<string, List<CardType>> xiaotiandifen = null;

        public static Dictionary<string, List<CardType>> XiaoTianDiFen
        {
            get
            {
                if (xiaotiandifen == null)
                {
                    xiaotiandifen = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    string key = null;
                    int jifen = 28;

                    List<string> lss = SelectSameCards(Shang.Keys.ToList(), 1);
                    List<string> ls0 = SelectSameCards(Jian.Keys.ToList(), 1);
                    List<string> ls1 = SelectSameCards(Ji.Keys.ToList(), 2);
                    foreach (string ss in lss)
                    {
                        foreach (string s0 in ls0)
                        {
                            foreach (string s1 in ls1)
                            {
                                //二十子极成四门色样 加5贺
                                if (isHave20(s1)) jifen = 33;
                                else jifen = 28;
                                key = EngineTool.FormatCardStrmd(ss + "-" + s0 + "-" + s1);
                                xiaotiandifen.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.XiaoTianDiFen, Jifen = jifen } });
                            }
                        }
                    }
                }
                return xiaotiandifen;
            }
        }
        #endregion

        #region  9.百短肩：百老 一肩 二赏 14贺
        private static Dictionary<string, List<CardType>> baiduanjian = null;

        public static Dictionary<string, List<CardType>> BaiDuanJian
        {
            get
            {
                if (baiduanjian == null)
                {
                    baiduanjian = new Dictionary<string, List<CardType>>();
                    int weight = 1;

                    List<string> lsb = SelectSameCards(Bai.Keys.ToList(), 1);
                    List<string> lsj = SelectSameCards(Jian.Keys.ToList(), 1);
                    List<string> lss = SelectSameCards(Shang.Keys.ToList(), 2);
                    List<string> lskey = new List<string>();
                    foreach (string keyb in lsb)
                    {
                        foreach (string keys in lss)
                        {
                            foreach (string keyj in lsj)
                            {
                                string key = keys + "-" + keyj + "-" + keyb;
                                key = EngineTool.FormatCardStrmd(key);
                                baiduanjian.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.BaiDuanJian, Jifen = 14 } });
                            }
                        }
                    }
                }
                return baiduanjian;
            }
        }
        #endregion

        #region 10. 百长肩：百老 2肩 1赏  16贺
        private static Dictionary<string, List<CardType>> baichangjian = null;

        public static Dictionary<string, List<CardType>> BaiChangJian
        {
            get
            {
                if (baichangjian == null)
                {
                    baichangjian = new Dictionary<string, List<CardType>>();
                    int weight = 1;

                    List<string> lsb = SelectSameCards(Bai.Keys.ToList(), 1);
                    List<string> lsj = SelectSameCards(Jian.Keys.ToList(), 2);
                    List<string> lss = SelectSameCards(Shang.Keys.ToList(), 1);
                    List<string> lskey = new List<string>();
                    foreach (string keyb in lsb)
                    {
                        foreach (string keys in lss)
                        {
                            foreach (string keyj in lsj)
                            {
                                string key = EngineTool.FormatCardStrmd(keys + "-" + keyj + "-" + keyb);
                                baichangjian.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.BaiChangJian, Jifen = 16 } });

                            }
                        }
                    }
                }
                return baichangjian;
            }

        }

        #endregion

        #region  11.极长肩：1极 2肩 1赏  16贺
        private static Dictionary<string, List<CardType>> jichangjian = null;

        public static Dictionary<string, List<CardType>> JiChangJian
        {
            get
            {
                if (jichangjian == null)
                {
                    jichangjian = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    int jifen = 16;

                    List<string> lsji = SelectSameCards(Ji.Keys.ToList(), 1);
                    List<string> lsj = SelectSameCards(Jian.Keys.ToList(), 2);
                    List<string> lss = SelectSameCards(Shang.Keys.ToList(), 1);
                    List<string> lskey = new List<string>();
                    foreach (string keyji in lsji)
                    {
                        foreach (string keys in lss)
                        {
                            foreach (string keyj in lsj)
                            {
                                //二十子极成四门色样 加5贺
                                if (keyji == Cardsmd[29].SValue()) jifen = 21;
                                else jifen = 16;
                                string key = EngineTool.FormatCardStrmd(keys + "-" + keyj + "-" + keyji);
                                jichangjian.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.JiChangJian, Jifen = jifen } });
                            }
                        }
                    }


                }
                return jichangjian;
            }


        }
        #endregion

        #region  12.极短肩：1极 1肩 2赏 14贺
        private static Dictionary<string, List<CardType>> jiduanjian = null;

        public static Dictionary<string, List<CardType>> JiDuanJian
        {
            get
            {
                if (jiduanjian == null)
                {
                    jiduanjian = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    int jifen = 14;

                    List<string> lsji = SelectSameCards(Ji.Keys.ToList(), 1);
                    List<string> lsj = SelectSameCards(Jian.Keys.ToList(), 1);
                    List<string> lss = SelectSameCards(Shang.Keys.ToList(), 2);
                    List<string> lskey = new List<string>();
                    foreach (string keyji in lsji)
                    {
                        foreach (string keys in lss)
                        {
                            foreach (string keyj in lsj)
                            {
                                //二十子极上桌成正色 加5贺
                                if (isHave20(keyji)) jifen = 19;
                                else jifen = 14;
                                string key = EngineTool.FormatCardStrmd(keys + "-" + keyj + "-" + keyji);
                                jiduanjian.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.JiDuanJian, Jifen = jifen } });
                            }
                        }
                    }
                }
                return jiduanjian;
            }
        }
        #endregion

        #region  13.百极四肩：百老 1极 2肩   24贺
        private static Dictionary<string, List<CardType>> baijisijian = null;

        public static Dictionary<string, List<CardType>> BaiJiSiJian
        {
            get
            {
                if (baijisijian == null)
                {
                    baijisijian = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    int jifen = 24;

                    List<string> lsji = SelectSameCards(Ji.Keys.ToList(), 1);
                    List<string> lsj = SelectSameCards(Jian.Keys.ToList(), 2);
                    List<string> lsb = SelectSameCards(Bai.Keys.ToList(), 1);
                    List<string> lskey = new List<string>();
                    foreach (string keyb in lsb)
                    {
                        foreach (string keyj in lsj)
                        {
                            foreach (string keyji in lsji)
                            {
                                //二十子极上桌成正色 加5贺
                                if (isHave20(keyji)) jifen = 29;
                                else jifen = 24;
                                string key = EngineTool.FormatCardStrmd(keyb + "-" + keyj + "-" + keyji);
                                baijisijian.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.BaiJiSiJian, Jifen = jifen } });
                            }
                        }
                    }
                }
                return baijisijian;
            }
        }
        #endregion

        #region  14.百极四赏：百老 1极 2赏  16贺
        private static Dictionary<string, List<CardType>> baijisishang = null;

        public static Dictionary<string, List<CardType>> BaiJiSiShang
        {
            get
            {
                if (baijisishang == null)
                {
                    baijisishang = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    int jifen = 16;

                    List<string> lss = SelectSameCards(Shang.Keys.ToList(), 2);
                    List<string> lsji = SelectSameCards(Ji.Keys.ToList(), 1);
                    List<string> lsb = SelectSameCards(Bai.Keys.ToList(), 1);
                    List<string> lskey = new List<string>();
                    foreach (string keyb in lsb)
                    {
                        foreach (string keys in lss)
                        {
                            foreach (string keyji in lsji)
                            {
                                //二十子极上桌成正色 加5贺
                                if (isHave20(keyji))
                                    jifen = 21;
                                else jifen = 16;
                                string key = EngineTool.FormatCardStrmd(keys + "-" + keyb + "-" + keyji);
                                baijisishang.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.BaiJiSiShang, Jifen = jifen } });
                            }
                        }
                    }
                }
                return baijisishang;
            }
        }
        #endregion

        #region  15.百赏百极：百老 2极 1赏  32贺
        private static Dictionary<string, List<CardType>> baishangbaiji = null;

        public static Dictionary<string, List<CardType>> BaiShangBaiJi
        {
            get
            {
                if (baishangbaiji == null)
                {
                    baishangbaiji = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    int jifen = 32;

                    List<string> lss = SelectSameCards(Shang.Keys.ToList(), 1);
                    List<string> lsji = SelectSameCards(Ji.Keys.ToList(), 2);
                    List<string> lsb = SelectSameCards(Bai.Keys.ToList(), 1);
                    List<string> lskey = new List<string>();
                    foreach (string keyb in lsb)
                    {
                        foreach (string keys in lss)
                        {
                            foreach (string keyji in lsji)
                            {
                                //二十子极上桌成正色 加5贺
                                if (isHave20(keyji)) jifen = 37;
                                else jifen = 32;
                                string key = EngineTool.FormatCardStrmd(keys + "-" + keyb + "-" + keyji);
                                baishangbaiji.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.BaiShangBaiJi, Jifen = jifen } });
                            }
                        }
                    }
                }
                return baishangbaiji;
            }
        }
        #endregion

        #region  16.百肩四极：百老 2极 1肩  48贺
        private static Dictionary<string, List<CardType>> baijiansiji = null;

        public static Dictionary<string, List<CardType>> BaiJianSiJi
        {
            get
            {
                if (baijiansiji == null)
                {
                    baijiansiji = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    int jifen = 48;

                    List<string> lsj = SelectSameCards(Jian.Keys.ToList(), 1);
                    List<string> lsji = SelectSameCards(Ji.Keys.ToList(), 2);
                    List<string> lsb = SelectSameCards(Bai.Keys.ToList(), 1);
                    List<string> lskey = new List<string>();
                    foreach (string keyb in lsb)
                    {
                        foreach (string keyj in lsj)
                        {
                            foreach (string keyji in lsji)
                            {
                                //二十子极上桌成正色 加5贺
                                if (isHave20(keyji)) jifen = 53;
                                else jifen = 48;
                                string key = EngineTool.FormatCardStrmd(keyj + "-" + keyb + "-" + keyji);
                                baijiansiji.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.BaiJianSiJi, Jifen = jifen } });
                            }
                        }
                    }
                }
                return baijiansiji;
            }
        }
        #endregion

        #region  17.节节高：百老 1极 1肩 1赏  16贺
        private static Dictionary<string, List<CardType>> jiejiegao = null;

        public static Dictionary<string, List<CardType>> JieJieGao
        {
            get
            {
                if (jiejiegao == null)
                {
                    jiejiegao = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    int jifen = 16;

                    List<string> lss = SelectSameCards(Shang.Keys.ToList(), 1);
                    List<string> lsj = SelectSameCards(Jian.Keys.ToList(), 1);
                    List<string> lsji = SelectSameCards(Ji.Keys.ToList(), 1);
                    List<string> lsb = SelectSameCards(Bai.Keys.ToList(), 1);
                    List<string> lskey = new List<string>();
                    foreach (string keyb in lsb)
                    {
                        foreach (string keys in lss)
                        {
                            foreach (string keyj in lsj)
                            {
                                foreach (string keyji in lsji)
                                {
                                    //二十子极上桌成正色 加5贺
                                    if (isHave20(keyji)) jifen = 21;
                                    else jifen = 16;
                                    string key = EngineTool.FormatCardStrmd(keyji + "-" + keyj + "-" + keys + "-" + keyb);
                                    jiejiegao.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.JieJieGao, Jifen = jifen } });
                                }
                            }
                        }
                    }
                }
                return jiejiegao;
            }
        }
        #endregion

        #region  18.对肩：2肩2赏  10贺
        private static Dictionary<string, List<CardType>> duijian = null;

        public static Dictionary<string, List<CardType>> DuiJian
        {
            get
            {
                if (duijian == null)
                {
                    duijian = new Dictionary<string, List<CardType>>();
                    int weight = 1;

                    List<string> lss = SelectSameCards(Shang.Keys.ToList(), 2);
                    List<string> lsj = SelectSameCards(Jian.Keys.ToList(), 2);

                    List<string> lskey = new List<string>();
                    foreach (string keys in lss)
                    {
                        foreach (string keyj in lsj)
                        {
                            string key = EngineTool.FormatCardStrmd(keys + "-" + keyj);
                            duijian.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.DuiJian, Jifen = 10 } });
                        }
                    }
                }
                return duijian;
            }
        }
        #endregion

        #region  19.长肩：3肩1赏  12贺
        private static Dictionary<string, List<CardType>> changjian = null;

        public static Dictionary<string, List<CardType>> ChangJian
        {
            get
            {
                if (changjian == null)
                {
                    changjian = new Dictionary<string, List<CardType>>();
                    int weight = 1;

                    List<string> lss = SelectSameCards(Shang.Keys.ToList(), 1);
                    List<string> lsj = SelectSameCards(Jian.Keys.ToList(), 3);

                    List<string> lskey = new List<string>();
                    foreach (string keys in lss)
                    {
                        foreach (string keyj in lsj)
                        {
                            string key = EngineTool.FormatCardStrmd(keys + "-" + keyj);
                            changjian.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.ChangJian, Jifen = 12 } });
                        }
                    }
                }
                return changjian;
            }
        }
        #endregion

        #region  20.短肩：1肩3赏  10贺
        private static Dictionary<string, List<CardType>> duanjian = null;

        public static Dictionary<string, List<CardType>> DuanJian
        {
            get
            {
                if (duanjian == null)
                {
                    duanjian = new Dictionary<string, List<CardType>>();
                    int weight = 1;

                    List<string> lss = SelectSameCards(Shang.Keys.ToList(), 3);
                    List<string> lsj = SelectSameCards(Jian.Keys.ToList(), 1);

                    List<string> lskey = new List<string>();
                    foreach (string keys in lss)
                    {
                        foreach (string keyj in lsj)
                        {
                            string key = EngineTool.FormatCardStrmd(keys + "-" + keyj);
                            duanjian.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.DuanJian, Jifen = 10 } });
                        }
                    }
                }
                return duanjian;
            }
        }
        #endregion

        #region  21.赏鲫鱼背：3极1赏  60贺
        private static Dictionary<string, List<CardType>> shangjiyubei = null;

        public static Dictionary<string, List<CardType>> ShangJiYuBei
        {
            get
            {
                if (shangjiyubei == null)
                {
                    shangjiyubei = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    int jifen = 60;

                    List<string> ls1 = SelectSameCards(Shang.Keys.ToList(), 1);
                    List<string> ls2 = SelectSameCards(Ji.Keys.ToList(), 3);

                    List<string> lskey = new List<string>();
                    foreach (string key1 in ls1)
                    {
                        foreach (string key2 in ls2)
                        {
                            //二十子极上桌成正色 加5贺
                            if (isHave20(key2))
                                jifen = 65;
                            else jifen = 60;
                            string key = EngineTool.FormatCardStrmd(key1 + "-" + key2);
                            shangjiyubei.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.ShangJiYUBei, Jifen = jifen } });
                        }
                    }
                }
                return shangjiyubei;
            }
        }
        #endregion

        #region  22.肩鲫鱼背：3极1肩  80贺
        private static Dictionary<string, List<CardType>> jianjiyubei = null;

        public static Dictionary<string, List<CardType>> JianJiYuBei
        {
            get
            {
                if (jianjiyubei == null)
                {
                    jianjiyubei = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    int jifen = 80;

                    List<string> ls1 = SelectSameCards(Jian.Keys.ToList(), 1);
                    List<string> ls2 = SelectSameCards(Ji.Keys.ToList(), 3);

                    List<string> lskey = new List<string>();
                    foreach (string key1 in ls1)
                    {
                        foreach (string key2 in ls2)
                        {
                            //二十子极上桌成正色 加5贺
                            if (isHave20(key2))
                                jifen = 85;
                            else jifen = 80;
                            string key = EngineTool.FormatCardStrmd(key1 + "-" + key2);
                            jianjiyubei.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.JianJiYuBei, Jifen = jifen } });
                        }
                    }
                }
                return jianjiyubei;
            }
        }
        #endregion

        #region  23.百鲫鱼背：3极1百  100贺
        private static Dictionary<string, List<CardType>> baijiyubei = null;

        public static Dictionary<string, List<CardType>> BaiJiYuBei
        {
            get
            {
                if (baijiyubei == null)
                {
                    baijiyubei = new Dictionary<string, List<CardType>>();
                    int weight = 1;
                    int jifen = 100;

                    List<string> ls1 = SelectSameCards(Bai.Keys.ToList(), 1);
                    List<string> ls2 = SelectSameCards(Ji.Keys.ToList(), 3);

                    List<string> lskey = new List<string>();
                    foreach (string key1 in ls1)
                    {
                        foreach (string key2 in ls2)
                        {
                            //二十子极上桌成正色 加5贺
                            if (isHave20(key2))
                                jifen = 105;
                            else jifen = 100;
                            string key = EngineTool.FormatCardStrmd(key1 + "-" + key2);
                            baijiyubei.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.BaiJiYuBei, Jifen = jifen } });
                        }
                    }
                }
                return baijiyubei;
            }
        }
        #endregion

        #endregion

        #region 三、斗上杂色样
        #region 1.天然趣  极首上桌且正本  160贺
        public static CardType NeiShengWaiWang(List<string> ls)
        {
            int jifen = 160;
            //极首张上桌  正本  包含长短门
            if (ls.Count > 1 && Ji.Keys.ToList().Exists(s => s == ls[0]) && Ji.Keys.ToList().Exists(s => s == ls[ls.Count - 1])
                && (ls.Exists(c => c.Contains(PokerConst.Club) || c.Contains(PokerConst.Spade)) &&
                    ls.Exists(c => c.Contains(PokerConst.Diamond) || c.Contains(PokerConst.Heart))))
            {
                //string key = string.Join("-", ls.ToArray());
                string key = ls[0] + CardsTypeMdFan.FirstWin + "-" + ls[ls.Count - 1] + CardsTypeMdFan.LastWin;
                return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.NeiShengWaiWang, Jifen = jifen };
            }
            return null;
        }
        #endregion
        #region 2.天然趣  首张牌出趣即上桌  15贺       
        public static CardType TianRanQu(List<string> ls, List<String> lsother = null)
        {
            int jifen = 15;
            //首张牌出极即上桌 
            if (ls.Count > 0 && Ji.Keys.ToList().Exists(s => s == ls[0]))
            {
                string key = ls[0] + CardsTypeMdFan.FirstWin;
                Card card = new Card(ls[0]);
                if (lsother != null && lsother.Contains(card.Suit))//二十子翻倍
                    return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.FeiLaiQu, Jifen = 10 };//飛來趣
                if (ls[0] == Cardsmd[29].SValue())//二十子翻倍
                    return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.NvDiDengJi, Jifen = jifen * 2 };//女帝登基
                return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.TianRanQu, Jifen = jifen };//天然趣
            }
            return null;
        }
        #endregion


        #region 3.三叠趣  3极  38贺(30+双飞*4*2)        

        public static CardType SanDieQu(List<string> ls)
        {
            string sCardswin = string.Join("-", ls.ToArray());
            int jifen = 0;

            List<string> ls1 = SelectSameCards(Ji.Keys.ToList(), 3);
            List<string> lkey = null;
            foreach (string s in ls1)
            {
                lkey = s.Split('-').ToList();

                if (ls.Contains(lkey[0]) &&
                     ls.Contains(lkey[1]) &&
                     ls.Contains(lkey[2]))
                {
                    jifen = 30;
                    return new CardType { CardKey = string.Join("-", lkey.ToArray()), Weight = 1, Name = CardsTypeMdFan.SanDieQu, Jifen = jifen };
                }
            }

            return null;
        }

        #endregion

        #region 3.龙门跃  百老 二十最先上桌  68贺(60+散花天女*5 +百后趣*3)        

        public static CardType LongMenYue(List<string> ls, bool bTip = false, List<string> ls2 = null)
        {
            int jifen = 0;
            string key = Cardsmd[37].SValue() + "-" + Cardsmd[29].SValue();
            List<string> lkey = key.Split('-').ToList();
            lkey.Add(CardsTypeMdFan.FirstWin);

            if (bTip)
            {
                if (ls2 == null)
                    ls = ls.Where(s => s == lkey[0] || s == lkey[1]).ToList();
                else if (ls2 != null && ls2.Count == 1 &&
                         (ls2.Contains(lkey[0]) || ls2.Contains(lkey[1])))
                    ls.Add(lkey[0]);
                else if (ls2 != null && ls2.Count == 2 &&
                        (ls2.Contains(lkey[0]) && ls2.Contains(lkey[1])))
                    ls = ls2;
                else
                    return null;

            }
            string sCardswin = string.Join("-", ls.ToArray());

            if (sCardswin.StartsWith(key) && ls.Contains(lkey[0]) && ls.Contains(lkey[1]))
            {
                jifen = 60;
                return new CardType { CardKey = key, Name = CardsTypeMdFan.LongMenYue, Weight = 1, Jifen = jifen };
            }

            return null;
        }
        #endregion

        #region 4.  佛顶珠 百老 一极先上桌  23贺(20+百后趣*3)        

        public static CardType FoDingZhu(List<string> ls)
        {

            int jifen = 0;

            List<string> ls0 = SelectSameCards(Bai.Keys.ToList(), 1);  //百： 一种
            List<string> ls1 = SelectSameCards(Ji.Keys.ToList(), 1);   //趣： 四种

            List<string> lkey = new List<string>();
            string key = null;


            string sCardswin = string.Join("-", ls.ToArray());  //将上桌牌组序列化(杂色不排序)如 Q*D-A*S-K*H-9*C-K*S

            foreach (string key0 in ls0)
            {
                foreach (string key1 in ls1)
                {
                    if (!key1.Contains(Cardsmd[29].SValue()))  //不含二十，百老二十先上属龙门跃
                        lkey.Add(key0 + "-" + key1);
                }
            }
            foreach (string keyf in lkey)
            {
                List<string> lkeyf = keyf.Split('-').ToList();
                if (sCardswin.StartsWith(keyf) && ls.Contains(lkeyf[0]) && ls.Contains(lkeyf[1]))  //百老＆一趣先上
                {
                    key = keyf + " " + CardsTypeMdFan.FirstWin;
                    jifen = 20;
                    return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.FoDingZhu, Jifen = jifen };
                }
            }
            return null;
        }

        #endregion

        #region 5.  散花天女 百老 二十上桌（不是最先上  百老、二十之间不隔红）  8贺(5+百后趣*3)        

        public static CardType SanHuaTianNv(List<string> ls)
        {
            string sCardswin = string.Join("-", ls.ToArray());  //将上桌牌组序列化(杂色不排序)如 Q*D-A*S-K*H-9*C-K*S
            string key = Cardsmd[37].SValue() + "-" + Cardsmd[29].SValue();
            List<string> lkey = key.Split('-').ToList();
            int jifen = 0;


            if (sCardswin.Contains(key) && ls.Contains(lkey[0]) && ls.Contains(lkey[1]))   //百老＆二十同上
            {
                jifen = 5;
                return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.SanHuaTianNv, Jifen = jifen };
            }

            return null;
        }

        #endregion

        #region 6.  天女散花  二十 百老上桌（不是最先上）  15贺(10+趣后百*5)        
        public static CardType TianNvSanHua(List<string> ls)
        {
            string sCardswin = string.Join("-", ls.ToArray());  //将上桌牌组序列化(杂色不排序)如 Q*D-A*S-K*H-9*C-K*S
            int jifen = 0;
            string key = Cardsmd[29].SValue() + "-" + Cardsmd[37].SValue();
            List<string> lkey = key.Split('-').ToList();
            if (sCardswin.Contains(key)        //百老＆二十同上
                && !sCardswin.StartsWith(key) && ls.Contains(lkey[0]) && ls.Contains(lkey[1]))     //百老二十非第一二桌
            {
                jifen = 10;
                return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.TianNvSanHua, Jifen = jifen };
            }

            return null;
        }


        #endregion

        #region 7.捉极献百   5贺
        public static CardType ZhuoJiXianBai(List<string> ls, int zhuojipos)//zhuojipos为捉极时上桌牌的位置
        {
            string sCardswin = string.Join("-", ls.ToArray());  //将上桌牌组序列化(杂色不排序)如 Q*D-A*S-K*H-9*C-K*S
            int jifen = 0;
            if (zhuojipos < ls.Count)
                return null;

            List<string> ls1 = SelectSameCards(Bai.Keys.ToList(), 1);   //百： 一种

            List<string> lskey = new List<string>();
            string key = null;
            foreach (string sbai in ls1)
            {

                if (Fuji.Keys.Contains(ls[zhuojipos]) && ls[zhuojipos + 1].Contains(sbai))    //捉极之后下一张献百
                {
                    key = ls[zhuojipos] + ls[zhuojipos + 1];
                    jifen = 5;
                }
            }
            return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.ZhuoJiXianBai, Jifen = jifen };
        }

        #endregion
        #region 8.捉极献极   3贺
        public static CardType ZhuoJiXianJi(List<string> ls, int zhuojipos)//zhuojipos为捉极时上桌牌的位置
        {
            string sCardswin = string.Join("-", ls.ToArray());  //将上桌牌组序列化(杂色不排序)如 Q*D-A*S-K*H-9*C-K*S
            int jifen = 0;
            if (zhuojipos >= ls.Count - 1)//最后一桌捉极 直接返回空 否则越界
                return null;

            List<string> ls1 = SelectSameCards(Ji.Keys.ToList(), 1);   //捉极之后下一张献极

            List<string> lskey = new List<string>();
            string skey = null;
            foreach (string key in ls1)
            {
                if (Fuji.Keys.Contains(ls[zhuojipos]) && ls[zhuojipos + 1].Contains(key))    //
                {
                    skey = ls[zhuojipos] + ls[zhuojipos + 1];
                    jifen = 3;
                    return new CardType { CardKey = skey, Weight = 1, Name = CardsTypeMdFan.ZhuoJiXianJi, Jifen = jifen };
                }
            }
            return null;
        }

        #endregion

        #region 9.  公孙对座 同门二赏二极  20贺(16+双飞*4)        
        private static Dictionary<string, List<CardType>> gongsunduizuo = null;

        public static Dictionary<string, List<CardType>> GongSunDuiZuo
        {
            get
            {
                if (gongsunduizuo == null)
                {
                    gongsunduizuo = new Dictionary<string, List<CardType>>();
                    int weight = 1;

                    List<string> ls0 = SelectSameCards(Shang.Keys.ToList(), 2);
                    List<string> ls1 = SelectSameCards(Ji.Keys.ToList(), 2);

                    List<string> lskey = new List<string>();
                    foreach (string key0 in ls0)
                    {
                        foreach (string key1 in ls1)
                        {
                            List<string> lkey0 = key0.Split('-').ToList();
                            List<string> lkey1 = key1.Split('-').ToList();

                            if (lkey0[0].Substring(0, lkey0[0].IndexOf("*")) == lkey1[0].Substring(0, lkey1[0].IndexOf("*")) &&
                                lkey0[1].Substring(0, lkey0[1].IndexOf("*")) == lkey1[1].Substring(0, lkey1[1].IndexOf("*")))
                            {
                                string key = EngineTool.FormatCardStrmd(key0 + "-" + key1);
                                gongsunduizuo.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.GongSunDuiZuo, Jifen = 20 } });
                            }
                        }
                    }
                }
                return gongsunduizuo;
            }
        }

        #endregion

        #region 10.  父子同登 同门二肩二极  28贺(24+双飞*4)        
        private static Dictionary<string, List<CardType>> fuzitongdeng = null;

        public static Dictionary<string, List<CardType>> FuZiTongDeng
        {
            get
            {
                if (fuzitongdeng == null)
                {
                    fuzitongdeng = new Dictionary<string, List<CardType>>();
                    int weight = 1;

                    List<string> ls0 = SelectSameCards(Jian.Keys.ToList(), 2);
                    List<string> ls1 = SelectSameCards(Ji.Keys.ToList(), 2);
                    List<string> lskey = new List<string>();
                    foreach (string key0 in ls0)
                    {
                        foreach (string key1 in ls1)
                        {
                            List<string> lkey0 = key0.Split('-').ToList();
                            List<string> lkey1 = key1.Split('-').ToList();

                            if (lkey0[0].Substring(0, lkey0[0].IndexOf("*")) == lkey1[0].Substring(0, lkey1[0].IndexOf("*")) &&
                                lkey0[1].Substring(0, lkey0[1].IndexOf("*")) == lkey1[1].Substring(0, lkey1[1].IndexOf("*")))
                            {
                                string key = EngineTool.FormatCardStrmd(key0 + "-" + key1);
                                fuzitongdeng.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.FuZiTongDeng, Jifen = 24 } });
                            }
                        }
                    }
                }
                return fuzitongdeng;
            }
        }

        #endregion


        #region 10.  兄弟齐心 同门二赏二肩  2贺        
        private static Dictionary<string, List<CardType>> xiongdiqixin = null;

        public static Dictionary<string, List<CardType>> XiongDiQiXin
        {
            get
            {
                if (xiongdiqixin == null)
                {
                    xiongdiqixin = new Dictionary<string, List<CardType>>();
                    int weight = 1;

                    List<string> ls0 = SelectSameCards(Shang.Keys.ToList(), 2);
                    List<string> ls1 = SelectSameCards(Jian.Keys.ToList(), 2);

                    List<string> lskey = new List<string>();
                    foreach (string key0 in ls0)
                    {
                        foreach (string key1 in ls1)
                        {
                            List<string> lkey0 = key0.Split('-').ToList();
                            List<string> lkey1 = key1.Split('-').ToList();

                            if (lkey0[0].Substring(0, lkey0[0].IndexOf("*")) == lkey1[0].Substring(0, lkey1[0].IndexOf("*")) &&
                                 lkey0[1].Substring(0, lkey0[1].IndexOf("*")) == lkey1[1].Substring(0, lkey1[1].IndexOf("*")))
                            {
                                string key = EngineTool.FormatCardStrmd(key0 + "-" + key1);
                                xiongdiqixin.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.XiongDiQiXin, Jifen = 2 } });
                            }
                        }
                    }
                }
                return xiongdiqixin;
            }
        }

        #endregion

        #region 11.  三代荣封 同门二赏二肩二极  36贺(32+双飞*4)        
        private static Dictionary<string, List<CardType>> sandairongfeng = null;

        public static Dictionary<string, List<CardType>> SanDaiRongFeng
        {
            get
            {
                if (sandairongfeng == null)
                {
                    sandairongfeng = new Dictionary<string, List<CardType>>();
                    int weight = 1;

                    List<string> ls0 = SelectSameCards(Shang.Keys.ToList(), 2);
                    List<string> ls1 = SelectSameCards(Jian.Keys.ToList(), 2);
                    List<string> ls2 = SelectSameCards(Ji.Keys.ToList(), 2);

                    List<string> lskey = new List<string>();
                    foreach (string key0 in ls0)
                    {
                        foreach (string key1 in ls1)
                        {
                            foreach (string key2 in ls2)
                            {
                                List<string> lkey0 = key0.Split('-').ToList();
                                List<string> lkey1 = key1.Split('-').ToList();
                                List<string> lkey2 = key2.Split('-').ToList();


                                if (lkey0[0].Substring(0, lkey0[0].IndexOf("*")) == lkey1[0].Substring(0, lkey1[0].IndexOf("*")) &&//第1对赏肩同门
                                lkey0[0].Substring(0, lkey0[0].IndexOf("*")) == lkey2[0].Substring(0, lkey2[0].IndexOf("*")) &&//第1对赏极同门
                                lkey0[1].Substring(0, lkey0[1].IndexOf("*")) == lkey1[1].Substring(0, lkey1[1].IndexOf("*")) &&//第2对赏肩同门
                                lkey0[1].Substring(0, lkey0[1].IndexOf("*")) == lkey2[1].Substring(0, lkey2[1].IndexOf("*"))) //第2对赏极同门
                                {
                                    string key = EngineTool.FormatCardStrmd(key0 + "-" + key1 + "-" + key2);
                                    sandairongfeng.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.SanDaiRongFeng, Jifen = 32 } });
                                }
                            }
                        }
                    }
                }
                return sandairongfeng;
            }
        }

        #endregion

        #region 12.  顺风旗 百老 千僧 红万  各牌在手1上桌2
        public static CardType ShunFengQi(List<string> ls)
        {
            string sCardswin = string.Join("-", ls.ToArray());  //将上桌牌组序列化(杂色不排序)如 Q*D-A*S-K*H-9*C-K*S
            int jifen = 1;
            string key = Cardsmd[39].SValue() + Cardsmd[38].SValue() + Cardsmd[37].SValue();

            //百老千僧红万同上 上一贺二 带柄递加
            if (ls.Contains(Cardsmd[39].SValue()) &&
                ls.Contains(Cardsmd[38].SValue()) &&
                ls.Contains(Cardsmd[37].SValue()))
            {
                foreach (string s in ls)
                { if (s.Contains(PokerConst.Spade)) jifen++; }
                jifen += 3;
            }
            else   //百老千僧红万 上一贺二
            {
                if (ls.Contains(Cardsmd[39].SValue())) jifen += 2;
                if (ls.Contains(Cardsmd[38].SValue())) jifen += 2;
                if (ls.Contains(Cardsmd[37].SValue())) jifen += 2;

            }
            return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.ShunFengQi, Jifen = jifen };
        }

        #endregion
        #region 13.  香炉脚 5吊三家各1  5贺(8红8素 +8  5红3素5素3红 +5)   
        public static CardType XiangLuJiao(List<string> ls1, List<string> ls2, List<string> ls3, List<string> ls4)
        {
            int jifen = 0;
            string key = null;
            string seyangname = CardsTypeMdFan.XiangLuJiao;
            List<string> lsding = new List<string>(), lsjiao = new List<string>();
            List<List<string>> list = new List<List<string>>();
            list.Add(ls1);
            list.Add(ls2);
            list.Add(ls3);
            list.Add(ls4);


            int ding = 0, jiao = 0;
            foreach (List<string> l in list)
            {
                if (l.Count == 5) { ++ding; lsding = l; key = string.Join("-", l.ToArray()); }
                if (l.Count == 1) { ++jiao; lsjiao = lsjiao.Concat(l).ToList(); }
            }

            if (ding == 1 && jiao == 3)
            {
                jifen = 5;
                int hong = 0, hong2 = 0, njians = 0, nshangs = 0;
                foreach (string s in lsding)
                {
                    if (IsHong(s)) hong++;
                }
                foreach (string s in lsjiao)
                {
                    if (IsHong(s))
                        hong2++;
                    if (Shang.Keys.ToList().Contains(s))
                        nshangs++;
                    if (Jian.Keys.ToList().Contains(s))
                        njians++;
                }


                if (hong == 5 && hong2 == 0) { jifen = 10; seyangname = CardsTypeMdFan.YuanyangDing; }      //五红吊三素
                else if (hong == 0 && njians == 3) { jifen = 24; seyangname = CardsTypeMdFan.YuwangDing; }
                else if (hong == 0 && nshangs == 3) { jifen = 22; seyangname = CardsTypeMdFan.WenwangDing; }
                else if (hong == 0 && hong2 == 3) jifen = 20;     //五素吊三红
                else if (hong == 5 && hong2 == 3) { jifen = 8; seyangname = CardsTypeMdFan.ZhuShaDing; }     //全红 朱砂鼎
                else if (hong == 0 && hong2 == 0) { jifen = 18; seyangname = CardsTypeMdFan.TieXiangLu; }//全素 铁香炉
                return new CardType { CardKey = key, Weight = 1, Name = seyangname, Jifen = jifen };
            }
            return null;
        }

        #endregion

        #region 14.  双飞趣 二极上桌  4贺        
        public static List<CardType> ShuangFeiQu(List<string> ls)
        {
            List<CardType> lct = new List<CardType>();

            string sCardswin = string.Join("-", ls.ToArray());  //将上桌牌组序列化(杂色不排序)如 Q*D-A*S-K*H-9*C-K*S
            int jifen = 4;
            List<string> lqu = SelectSameCards(Ji.Keys.ToList(), 2);//所有2个趣的可能组合，每组2个趣

            List<string> lkey = new List<string>();
            foreach (string squ in lqu)
            {
                lkey = squ.Split('-').ToList();   //每一组趣拆分进入列表
                if (ls.Contains(lkey[0]) && ls.Contains(lkey[1]))  //存在二极上桌
                {
                    //int i = sCardswin.IndexOf(lkey[0]), j = sCardswin.IndexOf(lkey[1]);
                    int i = ls.IndexOf(lkey[0]), j = ls.IndexOf(lkey[1]);
                    //保证i小于j，便于分离2极之间的牌组
                    if (i > j)
                    {
                        i = ls.IndexOf(lkey[1]); j = ls.IndexOf(lkey[0]);
                    }
                    //2极相连，中间无隔张
                    string key = lkey[0] + "-" + lkey[1];
                    if (i + 1 == j)
                    {

                        // key = lkey[0] + "-" + lkey[1];
                        lct.Add(new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.ShuangFeiQu, Jifen = jifen });
                    }

                    else//截取2趣中间的牌组
                    {
                        List<string> lstmp = ls.GetRange(i, j - i + 1);
                        if (isAllJin(lstmp))          //双趣之间可以连红，不能间清
                        {
                            // key = string.Join("-", lstmp.ToArray() );
                            lct.Add(new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.ShuangFeiQu, Jifen = jifen });
                        }
                    }


                }
            }
            return lct;
        }

        #endregion

        #region 15.  过桥龙 一趣一百又一趣  24贺 （20+双飞*4）       

        public static CardType GuoQiaoLong(List<string> ls)
        {
            string sCardswin = string.Join("-", ls.ToArray());  //将上桌牌组序列化(杂色不排序)如 Q*D-A*S-K*H-9*C-K*S
            int jifen = 0;
            List<string> lqu = SelectSameCards(Ji.Keys.ToList(), 2);//所有2个趣的可能组合，每组2个趣
            string bai = Cardsmd[37].SValue();
            string key = "";
            List<string> lkey = new List<string>();
            foreach (string squ in lqu)
            {
                lkey = squ.Split('-').ToList();   //每一组趣拆分进入列表
                if (ls.Contains(lkey[0]) && ls.Contains(lkey[1]))  //存在二极上桌
                {
                    int i = ls.IndexOf(lkey[0]), j = ls.IndexOf(lkey[1]);
                    //保证i小于j，便于分离2极之间的牌组
                    if (i > j)
                    {
                        i = ls.IndexOf(lkey[1]); j = ls.IndexOf(lkey[0]);
                    }

                    //截取2趣中间的牌组                    
                    List<string> s = ls.GetRange(i, j - i + 1);
                    //= sCardswin.Substring(i + lkey[0].Length + 1, j - i - lkey[0].Length -1);
                    if (s.Count > 0 && s.Contains(bai) && isAllJin(s))           // 趣 百 趣  双趣之间有百老
                    {
                        jifen = 20;
                        //key = string.Join("-", s.ToArray() );
                        key = lkey[0] + "-" + bai + "-" + lkey[1];
                        return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.GuoQiaoLong, Jifen = jifen };
                    }
                }
            }
            return null;
        }

        #endregion

        #region 16.  趣后百   5贺        

        public static CardType QuhouBai(List<string> ls)
        {
            string sCardswin = string.Join("-", ls.ToArray());  //将上桌牌组序列化(杂色不排序)如 Q*D-A*S-K*H-9*C-K*S
            int jifen = 0;
            List<string> lqu = SelectSameCards(Ji.Keys.ToList(), 1);//所有1个趣的可能情形，每组1个趣共四组
            string bai = Cardsmd[37].SValue();
            string key = lqu[0] + "-" + bai;
            List<string> lkey = new List<string>();
            foreach (string squ in lqu)
            {
                lkey = squ.Split('-').ToList();   //每一组趣拆分进入列表
                if (ls.Contains(lkey[0]) && ls.Contains(bai))  //存在二极上桌
                {
                    int i = ls.IndexOf(lkey[0]), j = ls.IndexOf(bai);
                    //保证i小于j，便于分离2极之间的牌组                    
                    if (i < j)
                    {
                        //趣 百相连，中间无隔张
                        if (i + 1 == j)
                        {
                            key = string.Join("-", ls.GetRange(i, j - i + 1).ToArray());
                            jifen = 5;
                            return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.QuHouBai, Jifen = jifen };
                        }
                        else
                        {

                            //截取2趣中间的牌组   
                            //int ipos = i + lkey[0].Length + 1;
                            List<string> s = ls.GetRange(i, j - i + 1);//sCardswin.Substring(ipos, j - ipos);
                            if (s.Count > 0 && isAllJin(s))              // 趣和百老之间全红
                            {
                                //key = string.Join("-", ls.GetRange(i, j - i + 1).ToArray() );
                                jifen = 5;
                                return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.QuHouBai, Jifen = jifen };
                            }
                        }
                    }
                }
            }
            return null;
        }

        #endregion

        #region 17.  百后趣   3贺        

        public static CardType BaiHouQu(List<string> ls)
        {
            string sCardswin = string.Join("-", ls.ToArray());  //将上桌牌组序列化(杂色不排序)如 Q*D-A*S-K*H-9*C-K*S
            int jifen = 0;
            List<string> lqu = SelectSameCards(Ji.Keys.ToList(), 1);//所有1个趣的可能情形，每组1个趣共四组
            string bai = Cardsmd[37].SValue();
            string key = bai + "-" + lqu[0];

            List<string> lkey = new List<string>();
            foreach (string squ in lqu)
            {
                lkey = squ.Split('-').ToList();   //每一组趣拆分进入列表
                if (ls.Contains(lkey[0]) && ls.Contains(bai))  //存在二极上桌
                {
                    int i = ls.IndexOf(bai), j = ls.IndexOf(lkey[0]);
                    //保证i小于j，便于分离百、极之间的牌组

                    if (i < j)
                    {
                        // 百 趣相连，中间无隔张
                        if (i + 1 == j)
                        {
                            key = string.Join("-", ls.GetRange(i, j - i + 1).ToArray());
                            jifen = 3;
                            return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.BaiHouQu, Jifen = jifen };
                        }
                        else
                        {
                            int ipos = i + 1;
                            //截取百 趣中间的牌组                    
                            List<string> s = ls.GetRange(i, j - i + 1);//.Substring(ipos, j - ipos);
                            if (s.Count > 0 && s.Contains(bai) && isAllJin(s))          // 趣和百老之间全红
                            {
                                //key = string.Join("-", ls.GetRange(i, j - i + 1).ToArray());
                                key = string.Join("-", ls.GetRange(i, j - i + 1).ToArray());
                                jifen = 3;
                                return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.BaiHouQu, Jifen = jifen };
                            }
                        }
                    }
                }
            }
            return null;
        }


        #endregion

        #region 18.  双趣后  趣 趣 百  24贺  （20+双飞*4）      

        public static CardType ShuangQuHou(List<string> ls)
        {
            string sCardswin = string.Join("-", ls.ToArray());  //将上桌牌组序列化(杂色不排序)如 Q*D-A*S-K*H-9*C-K*S
            int jifen = 0;
            List<string> lqu = SelectSameCards(Ji.Keys.ToList(), 2);//所有2个趣的可能组合情形，每组1个趣共四组
            string bai = Cardsmd[37].SValue();


            List<string> lkey = new List<string>();
            foreach (string squ in lqu)
            {
                lkey = squ.Split('-').ToList();   //每一组趣拆分进入列表
                string key = squ + "-" + bai;

                if (ls.Contains(lkey[0]) && ls.Contains(lkey[1])
                    && ls.Contains(bai))  //存在二极上桌
                {
                    int i = ls.IndexOf(lkey[0]), j = ls.IndexOf(lkey[1]), k = ls.IndexOf(bai);
                    if (i > j) { i = ls.IndexOf(lkey[1]); j = ls.IndexOf(lkey[0]); }
                    //保证i小于j，便于分离2极之间的牌组                    
                    if (i < k && j < k)
                    {
                        if (i + 1 == j && j + 1 == k) //趣百趣相连无隔张
                        {
                            //key = string.Join("-", ls.GetRange(i,k-i+1).ToArray() );
                            jifen = 20;
                            return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.ShuangQuHou, Jifen = jifen };
                        }
                        else
                        {

                            //截取2趣中间的牌组  
                            if (j > i + 1 || k > j + 1)
                            {
                                List<string> s = ls.GetRange(i, j - i + 1);//.Substring(kpos, i - kpos),


                                if (s.Count > 0 && isAllJin(s))// 趣与趣及趣和百老之间相连或全红
                                {
                                    //key = string.Join("-", ls.GetRange(i, k - i + 1).ToArray());
                                    jifen = 20;
                                    return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.ShuangQuHou, Jifen = jifen };
                                }
                            }
                            else
                                return null;
                        }

                    }
                }
            }
            return null;
        }


        #endregion

        #region 19.  双百后: 百 趣 趣   16贺  （12+双飞*4）      

        public static CardType ShuangBaiHou(List<string> ls)
        {
            string sCardswin = string.Join("-", ls.ToArray());  //将上桌牌组序列化(杂色不排序)如 Q*D-A*S-K*H-9*C-K*S
            int jifen = 0;
            List<string> lqu = SelectSameCards(Ji.Keys.ToList(), 2);//所有2个趣的可能组合情形，每组1个趣共四组
            string bai = Cardsmd[37].SValue();

            List<string> lkey = new List<string>();
            foreach (string squ in lqu)
            {
                lkey = squ.Split('-').ToList();   //每一组趣拆分进入列表
                string key = bai + "-" + squ;

                if (ls.Contains(lkey[0]) && ls.Contains(lkey[1])
                    && ls.Contains(bai))  //存在二极上桌
                {
                    int i = ls.IndexOf(lkey[0]), j = ls.IndexOf(lkey[1]), k = ls.IndexOf(bai);
                    if (i > j) { i = ls.IndexOf(lkey[1]); j = ls.IndexOf(lkey[0]); }
                    //保证i小于j，便于分离2极之间的牌组                    
                    if (i > k && j > k)
                    {
                        if (k + 1 == i && i + 1 == j) //趣百趣相连无隔张
                        {
                            //key = string.Join("-", ls.GetRange(i, k - i + 1).ToArray());
                            jifen = 12;
                            return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.ShuangBaiHou, Jifen = jifen };
                        }
                        else
                        {

                            //截取百-趣 及 2趣中间的牌组  
                            List<string> ltmp1 = new List<string>(), ltmp2 = new List<string>();
                            if (j > i + 1)
                                ltmp1 = ls.GetRange(i + 1, j - i - 1);//.Substring(kpos, i - kpos),
                            if (k > j + 1)
                                ltmp2 = ls.GetRange(j + 1, k - j - 1);//sCardswin.Substring(ipos, j - ipos);

                            // 趣与趣及趣和百老之间相连或全红
                            if ((IsAllHong(ltmp1) && IsAllHong(ltmp2)) ||
                                  (k + bai.Length + 1 == i && IsAllHong(ltmp2)) ||
                                  (IsAllHong(ltmp1) && i + bai.Length + 1 == j))
                            {
                                //key = string.Join("-", ls.GetRange(i, k - i + 1).ToArray());
                                jifen = 20;
                                return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.ShuangBaiHou, Jifen = jifen };
                            }
                        }
                    }
                }
            }
            return null;
        }

        #endregion

        #region 20.  倒卷帘: 同门先趣后赏   5贺  

        public static List<CardType> DaoJuanLian(List<string> ls)
        {
            string sCardswin = string.Join("-", ls.ToArray());  //将上桌牌组序列化(杂色不排序)如 Q*D-A*S-K*H-9*C-K*S
            int jifen = 0;
            List<string> lqu = SelectSameCards(Ji.Keys.ToList(), 1);//所有1个趣的可能情形，每组1个趣共四组
            List<string> lshang = SelectSameCards(Shang.Keys.ToList(), 1);
            List<string> lkey = new List<string>();
            string key = null;

            List<CardType> lct = new List<CardType>();
            foreach (string qu in lqu)
            {
                lkey = qu.Split('-').ToList();   //每一组趣拆分进入列表
                foreach (string shang in lshang)
                {
                    List<string> keyshang = shang.Split('-').ToList();
                    Card cji = new Card(lkey[0]), cshang = new Card(keyshang[0]);
                    if (ls.Contains(lkey[0]) && ls.Contains(keyshang[0])      //赏 极 上桌
                            && cji.Suit.Equals(cshang.Suit)) //赏极为同门
                    {
                        int i = ls.IndexOf(lkey[0]), j = ls.IndexOf(keyshang[0]);
                        //保证i小于j，便于分离2极之间的牌组                    
                        List<string> ltmp = ls.GetRange(0, i);//获得趣赏之前上桌牌组
                        if (i + 1 == j)//趣赏之前上桌牌组内不含同门
                        {
                            //趣 赏相连，中间无隔张
                            if (ltmp != null && ltmp.Exists(c => c.Contains(cji.Suit)))
                                break;

                            key = lkey[0] + "-" + keyshang[0];
                            jifen = 5;
                            lct.Add(new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.DaoJuanLian, Jifen = jifen });

                            //else  趣赏需同上 连红亦不算倒卷
                            //{
                            //    int ipos = i + lkey[0].Length + 1;
                            //    //截取趣赏中间的牌组                    
                            //    List<string> s = ls.GetRange(i + 1, j - i - 1);
                            //    if (IsAllHong(s))          // 趣和赏之间全红
                            //    {
                            //        key = string.Join("-",ls.GetRange(i, j - i + 1).ToArray() ) ;
                            //        jifen = 5;
                            //        return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.DaoJuanLian, Jifen = jifen };
                            //    }
                            //}


                        }
                    }
                }
            }
            return lct;
        }


        #endregion

        #region 21.  美人卷帘: 先二十 后红万   10贺  

        public static CardType HuangDaoJuan(List<string> ls)
        {
            string sCardswin = string.Join("-", ls.ToArray());  //将上桌牌组序列化(杂色不排序)如 Q*D-A*S-K*H-9*C-K*S
            int jifen = 0;
            List<string> lqu = SelectSameCards(Ji.Keys.ToList(), 1);//所有1个趣的可能情形，每组1个趣共四组
            List<string> lshang = SelectSameCards(Shang.Keys.ToList(), 1);
            List<string> lkey = new List<string>();
            string key = null;
            foreach (string qu in lqu)
            {
                lkey = qu.Split('-').ToList();   //每一组趣拆分进入列表
                foreach (string shang in lshang)
                {
                    List<string> keyshang = shang.Split('-').ToList();
                    Card cji = new Card(lkey[0]), cshang = new Card(keyshang[0]);
                    if (ls.Contains(lkey[0]) && ls.Contains(keyshang[0])      //赏 极 上桌
                            && cji.Suit.Equals(PokerConst.Spade) //赏极为同门
                            && cshang.Suit.Equals(PokerConst.Spade)) //十字门  二十 红万上桌(连红)
                    {
                        int i = ls.IndexOf(lkey[0]), j = ls.IndexOf(keyshang[0]);
                        //保证i小于j，便于分离2极之间的牌组                    
                        if (i < j)
                        {
                            //趣 赏相连，中间无隔张
                            if (i + 1 == j)
                                jifen = 10;
                            else
                            {
                                List<string> lstmp = ls.GetRange(i + 1, j - i - 1);
                                if (IsAllHong(lstmp))    // 趣和赏之间全红
                                { jifen = 10; }
                            }
                            if (jifen > 0)
                            {
                                key = string.Join("-", ls.GetRange(i, j - i + 1).ToArray());
                                return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.HuangDaoJuan, Jifen = jifen };
                            }

                        }
                    }
                }
            }
            return null;
        }

        #endregion

        #region 22.  卷帘飞: 趣 赏 趣 上桌   14贺 （10 + 双飞*4）  

        public static CardType JuanLianFei(List<string> ls)
        {
            string sCardswin = string.Join("-", ls.ToArray());  //将上桌牌组序列化(杂色不排序)如 Q*D-A*S-K*H-9*C-K*S
            int jifen = 0;
            List<string> lqu = SelectSameCards(Ji.Keys.ToList(), 2);//所有2个趣的可能组合，每组2个趣
            List<string> lshang = SelectSameCards(Shang.Keys.ToList(), 2);//1赏组合4种
            List<string> lkey = new List<string>();
            string key = null;

            foreach (string squ in lqu)
            {
                lkey = squ.Split('-').ToList();   //每一组趣拆分进入列表
                if (ls.Contains(lkey[0]) && ls.Contains(lkey[1]))  //存在二极上桌
                {
                    int i = ls.IndexOf(lkey[0]), j = ls.IndexOf(lkey[1]);
                    //保证i小于j，便于分离2极之间的牌组
                    if (i > j)
                    {
                        i = ls.IndexOf(lkey[1]); j = ls.IndexOf(lkey[0]);
                    }
                    foreach (string shang in lshang)//列举4赏之一
                    {
                        int ipos = i + lkey[0].Length + 1;
                        //截取2趣中间的牌组                    
                        List<string> s = ls.GetRange(i + 1, j - i - 1);//sCardswin.Substring(ipos, j - ipos);
                        if (s.Contains(shang))          // 趣 赏 趣  双趣之间有赏
                        {
                            key = string.Join("-", ls.GetRange(i, j - i + 1).ToArray());
                            jifen = 10;
                            return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.JuanLianFei, Jifen = jifen };
                        }
                    }
                }
            }
            return null;
        }


        #endregion

        #region 24.六桌吊 3贺
        public static CardType LiuDiao(List<string> ls)
        {
            int jifen = 0;
            string key = string.Join("-", ls.ToArray());
            if (ls.Count == 6)
            {
                jifen = 3;
                return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.LiuDiao, Jifen = jifen };
            }
            return null;
        }
        #endregion
        #region 25.七桌吊 2贺
        public static CardType QiDiao(List<string> ls)
        {
            int jifen = 0;
            string key = string.Join("-", ls.ToArray());
            if (ls.Count == 7)
            {
                jifen = 2;
                return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.QiDiao, Jifen = jifen };
            }
            return null;
        }
        #endregion

        #region 26.全吊(8吊) 4贺
        public static CardType BaDiao(List<string> ls)
        {
            int jifen = 0;
            string key = string.Join("-", ls.ToArray());
            if (ls.Count == 8)
            {
                jifen = 4;
                return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.BaQuan, Jifen = jifen };
            }
            return null;
        }
        #endregion

        #region 34.  公领孙: 同门 先赏 后极   3贺  

        public static List<CardType> GongLingSun(List<string> ls)
        {
            string sCardswin = string.Join("-", ls.ToArray());  //将上桌牌组序列化(杂色不排序)如 Q*D-A*S-K*H-9*C-K*S
            int jifen = 0;
            List<string> lqu = SelectSameCards(Ji.Keys.ToList(), 1);//所有1个趣的可能情形，每组1个趣共四组
            List<string> lshang = SelectSameCards(Shang.Keys.ToList(), 1);
            List<string> lkey = new List<string>();
            string key = null;

            List<CardType> lct = new List<CardType>();
            foreach (string qu in lqu)
            {
                lkey = qu.Split('-').ToList();   //每一组趣拆分进入列表
                foreach (string shang in lshang)
                {
                    List<string> keyshang = shang.Split('-').ToList();
                    Card cji = new Card(lkey[0]), cshang = new Card(keyshang[0]);
                    if (ls.Contains(lkey[0]) && ls.Contains(keyshang[0])      //赏 极 上桌
                            && cji.Suit.Equals(cshang.Suit)) //赏极为同门
                    {
                        int j = ls.IndexOf(lkey[0]), i = ls.IndexOf(keyshang[0]);
                        //保证i小于j，便于分离2极之间的牌组                    
                        List<string> ltmp = ls.GetRange(0, i);//获得趣赏之前上桌牌组
                        if (i + 1 == j)//趣赏之前上桌牌组内不含同门
                        {
                            //趣 赏相连，中间无隔张
                            if (ltmp != null && ltmp.Exists(c => c.Contains(cji.Suit)))
                                break;

                            key = keyshang[0] + "-" + lkey[0];
                            jifen = 3;
                            lct.Add(new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.GongLingSun, Jifen = jifen });
                        }
                    }
                }
            }
            return lct;
        }


        #endregion

        #region 27.镜中人  一文 二十万 同上桌  8贺
        public static CardType JingZRhongRen(List<string> ls)
        {  
            string key0 = Cardsmd[8].SValue(), key1 = Cardsmd[29].SValue();
            int idx0 = ls.FindIndex(x=>x.Equals(key0)),
                idx1 = ls.FindIndex(x => x.Equals(key1));
            if ( idx0 >= 0 && idx1 >= 0 &&
                (idx0 == idx1 + 1 || idx0 == idx1 - 1))
            {
                int jifen = 8;
                string key = key0 + "-" + key1;
                return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.JingZhongRen, Jifen = jifen };
            }

            return null;
        }
        #endregion

        #region 28.借花献佛  枝花 五十万 同上桌  4贺
        public static CardType JieHuaXianFo(List<string> ls)
        {

            string key0 = Cardsmd[32].SValue(), key1 = Cardsmd[9].SValue();
            int idx0 = ls.FindIndex(x => x.Equals(key0)),
                 idx1 = ls.FindIndex(x => x.Equals(key1));
            if (idx0 >= 0 && idx1 >= 0 &&
                (idx0 == idx1 + 1 || idx0 == idx1 - 1))
            {
                int jifen = 4;
                string key = key0 + "-" + key1;
                jifen = 4;
                return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.JieHuaXianFo, Jifen = jifen };
            }

            return null;
        }
        #endregion

        #region 29.夫妻团圆  空文 二十万 同上桌  6贺
        public static CardType FuQiTuanYuan(List<string> ls)
        {
            string key0 = Cardsmd[10].SValue(), key1 = Cardsmd[29].SValue();

            int idx0 = ls.FindIndex(x => x.Equals(key0)),
                 idx1 = ls.FindIndex(x => x.Equals(key1));
            if (idx0 >= 0 && idx1 >= 0 &&
                (idx0 == idx1 + 1 || idx0 == idx1 - 1))
            {
                string key = key0 + "-" + key1;
                int jifen = 6;
                return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.FuQiTuanYuan, Jifen = jifen };
            }
            return null;
        }
        #endregion

        #region 29.人面桃花  枝花 二十万 同上桌  8贺
        public static CardType RenMianTaoHua(List<string> ls)
        {

            string key0 = Cardsmd[9].SValue(), key1 = Cardsmd[29].SValue();

            int idx0 = ls.FindIndex(x => x.Equals(key0)),
                idx1 = ls.FindIndex(x => x.Equals(key1));
            if (idx0 >= 0 && idx1 >= 0 &&
                (idx0 == idx1 + 1 || idx0 == idx1 - 1))
            {
                string key = key0 + "-" + key1;
                int jifen = 7;
                return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.RenMianTaoHua, Jifen = jifen };
            }
            return null;
        }
        #endregion

        #region 30.美女参禅  千僧 二十万 同上桌  10贺
        public static CardType MeiNvCanChan(List<string> ls)
        {
            string key0 = Cardsmd[38].SValue(), key1 = Cardsmd[29].SValue();
            int idx0 = ls.FindIndex(x => x.Equals(key0)),
                idx1 = ls.FindIndex(x => x.Equals(key1));
            if ( idx0 >= 0 && idx1 >= 0 &&
                (idx0 == idx1 + 1 || idx0 == idx1 - 1))
            {
                string key = key0 + "-" + key1;
                int jifen = 10;
                return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.MeiNvCanChan, Jifen = jifen };
            }
            return null;
        }
        #endregion

        #region 31.小参禅  五十万 二十万 同上桌  10贺
        public static CardType XiaoCanChan(List<string> ls)
        {
           
            string key0 = Cardsmd[32].SValue(), key1 = Cardsmd[29].SValue();
            int idx0 = ls.FindIndex(x => x.Equals(key0)),
                idx1 = ls.FindIndex(x => x.Equals(key1));
            if (idx0 >= 0 && idx1 >= 0 &&
                (idx0 == idx1 + 1 || idx0 == idx1 - 1))
            {
                string key = key0 + "-" + key1;
                int jifen = 10;
                return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.XiaoCanChan, Jifen = jifen };
            }
            return null;
        }
        #endregion

        #region 32.二佛谈经  五十万 千万 同上桌  6贺
        public static CardType ErFoTanJing(List<string> ls)
        {
            
            string key0 = Cardsmd[32].SValue(), key1 = Cardsmd[38].SValue();
            int idx0 = ls.FindIndex(x => x.Equals(key0)),
                idx1 = ls.FindIndex(x => x.Equals(key1));

            if (idx0 >= 0 && idx1 >= 0 &&
                (idx0 == idx1 + 1 || idx0 == idx1 - 1))
            {
                
                string key = key0 + "-" + key1;
                int jifen = 6;
                return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.ErFoTanJing, Jifen = jifen };
            }
            return null;
        }
        #endregion

        #region 33.天仙送子  九十万 二十万 同上桌  20贺
        public static CardType TianXianSongZi(List<string> ls)
        {
          
            string key0 = Cardsmd[29].SValue(), key1 = Cardsmd[36].SValue();
            int idx0 = ls.FindIndex(x => x.Equals(key0)),
                idx1 = ls.FindIndex(x => x.Equals(key1));

            if (idx0 >= 0 && idx1 >= 0 &&
                (idx0 == idx1 + 1 || idx0 == idx1 - 1))
            {
                string key = key0 + "-" + key1;
                int jifen = 20;
                return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.TianXianSongZi, Jifen = jifen };
            }
            return null;
        }
        #endregion

        #region 35.福禄寿喜
        public static CardType FuLuShouXi(List<Card> ls)
        {

            int jifen = 1;
            List<string> Lji = Ji.Keys.ToList();
            string key = string.Join("-", Lji.ToArray());
            //四极 上一贺2
            foreach (string s in Lji)
            {
                if (ls.Exists(c => c.SValue() == s))
                    jifen += 1;
            }

            return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.FuLuShouXi, Jifen = jifen };
        }
        #endregion


        #region 36.连环索
        public static CardType LianHuanSuo(List<string> ls)
        {

            ls = EngineTool.FormatCardStrmd(String.Join("-", ls.ToArray())).Split('-').ToList();
            if (ls.Count < 3)//最少3张上桌
                return null;

            int weight = 1;
            List<String> lHong = Hong.Keys.ToList();
            foreach (string sHong in lHong)//上桌之牌全青张，有红则不符
            {
                if (ls.Contains(sHong))
                    return null;
            }


            for (int i = ls.Count; i >= 3; i--)
            {
                List<string> lkey = ShunZi(1, i);//同花顺索子
                foreach (string key in lkey)
                {
                    List<string> lk = key.Split('-').ToList();
                    for (int k = 0; k < lk.Count; k++)
                    {
                        if (!ls.Exists(s => s == lk[k]))
                            break;
                        if (k == lk.Count - 1)
                            return new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.LianHuanSuo, Jifen = (i - 2) * 4 };
                    }
                }
            }
            return null;
        }
        #endregion

        #region 36.五弦连环索 10贺  
        public static CardType DaLianHuanSuo(List<string> ls)
        {
            int weight = 1;

            List<String> lHong = Hong.Keys.ToList();
            List<string> lkey = ShunZi(1, 5);//同花顺索子

            foreach (string sHong in lHong)
            {
                if (ls.Contains(sHong))
                    return null;
            }


            foreach (string key in lkey)
            {
                List<string> lk = key.Split('-').ToList();
                for (int k = 0; k < lk.Count; k++)
                {
                    if (!ls.Exists(s => s == lk[k]))
                        break;
                    if (k == lk.Count - 1)
                        return new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.DaLianHuanSuo, Jifen = 10 };
                }
            }
            return null;
        }
        #endregion


        #region 37.同气连枝 4青同值 24贺
        public static CardType TongQiLianZhi(List<string> ls)
        {
            int weight = 1;

            if (ls.Count < 4)
                return null;
            List<String> lHong = Hong.Keys.ToList();
            foreach (string sHong in lHong)
            {
                if (ls.Contains(sHong))
                    return null;
            }

            List<string> LQing4 = new List<string>();
            for (int y = 2; y < 8; y++)
            {
                string skey = "";
                for (int x = 0; x < 4; x++)
                {
                    Card c = new Card(mdSuit[x], y);
                    if (!ls.Contains(c.SValue()))
                        break;
                    skey += c.SValue();
                    if (x < 3) skey += "-";
                    if (x == 3)
                        return new CardType() { CardKey = skey, Weight = weight++, Name = CardsTypeMdFan.TongQiLianZhi, Jifen = 24 };
                }

            }

            return null;
        }
        #endregion

        #region 38.清白人家  全清张  1贺
        public static CardType QingBaiRenJia(List<string> ls)
        {
            List<string> lshong = new List<string>();
            lhong.ForEach(hong => lshong.Add(hong.SValue()));
            if (ls.Count > 2 && !EngineTool.IsListsMixed(ls, lshong))
            {
                string key = string.Join("-", ls.ToArray());

                return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.QingBaiRenJia, Jifen = ls.Count - 2 };
            }

            return null;
        }
        #endregion


        #region 39.佛赤腳  百老上桌且未正本  -5贺
        public static CardType FoChiJiao(List<Card> lc)
        {
            int jifen = -5;
            string key = Cardsmd[37].SValue();
            //极首张上桌  正本  包含长短门
            if (lc.Count == 1 &&
                lc.Exists(c => c.Suit == PokerConst.Spade &&
                             c.Value == PokerConst.ShangValve - 2))
            {

                return new CardType { CardKey = key, Weight = 1, Name = StringFanConst.FoChiJiao, Jifen = jifen };
            }
            return null;
        }
        #endregion

        #region 40.四肩在手
        public static CardType FourJian(List<Card> ls)
        {

            int jifen = 1;
            List<string> Ljian = Ji.Keys.ToList();
            string key = string.Join("-", Ljian.ToArray());
            //四极 上一贺2
            int n = ls.Where(c => c.Value == EngineTool.GetShangValve(c.Suit) - 1).Count();
            jifen += n;

            return new CardType { CardKey = key, Weight = 1, Name = CardsTypeMdFan.FourJian, Jifen = jifen };
        }
        #endregion

        #region 41.  賞肩對座 同门一赏1肩二极  12贺(12+双飞*4)        
        private static Dictionary<string, List<CardType>> shangjianduizuo = null;

        public static Dictionary<string, List<CardType>> ShangJianDuiZuo
        {
            get
            {
                if (shangjianduizuo == null)
                {
                    shangjianduizuo = new Dictionary<string, List<CardType>>();
                    int weight = 1;

                    List<string> ls0 = SelectSameCards(Shang.Keys.ToList(), 1);
                    List<string> ls1 = SelectSameCards(Jian.Keys.ToList(), 1);
                    List<string> ls2 = SelectSameCards(Ji.Keys.ToList(), 2);

                    List<string> lskey = new List<string>();
                    foreach (string key0 in ls0)
                    {
                        foreach (string key1 in ls1)
                        {
                            foreach (string key2 in ls2)
                            {
                                List<string> lkey0 = key0.Split('-').ToList();
                                List<string> lkey1 = key1.Split('-').ToList();
                                List<string> lkey2 = key2.Split('-').ToList();


                                if ((lkey0[0].Substring(0, lkey0[0].IndexOf("*")) == lkey2[0].Substring(0, lkey2[0].IndexOf("*")) &&//第1对赏肩同门
                                     lkey1[0].Substring(0, lkey1[0].IndexOf("*")) == lkey2[1].Substring(0, lkey2[1].IndexOf("*"))) ||//第1对赏极同门
                                    (lkey0[0].Substring(0, lkey0[0].IndexOf("*")) == lkey2[1].Substring(0, lkey2[1].IndexOf("*")) &&//第2对赏肩同门
                                     lkey1[0].Substring(0, lkey1[0].IndexOf("*")) == lkey2[0].Substring(0, lkey2[0].IndexOf("*"))))//第2对赏极同门
                                {
                                    string key = EngineTool.FormatCardStrmd(key0 + "-" + key1 + "-" + key2);
                                    shangjianduizuo.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.ShangJianDuiZuo, Jifen = 12 } });
                                }
                            }
                        }
                    }
                }
                return shangjianduizuo;
            }
        }

        #endregion


        #region 42.  賞百對座 同门一赏1肩二极  16贺(12+双飞*4)        
        private static Dictionary<string, List<CardType>> shangbaiduizuo = null;

        public static Dictionary<string, List<CardType>> ShangBaiDuiZuo
        {
            get
            {
                if (shangbaiduizuo == null)
                {
                    shangbaiduizuo = new Dictionary<string, List<CardType>>();
                    int weight = 1;

                    List<string> ls0 = SelectSameCards(Shang.Keys.ToList(), 1);
                    List<string> ls1 = SelectSameCards(Bai.Keys.ToList(), 1);
                    List<string> ls2 = SelectSameCards(Ji.Keys.ToList(), 2);

                    List<string> lskey = new List<string>();
                    foreach (string key0 in ls0)
                    {
                        foreach (string key1 in ls1)
                        {
                            foreach (string key2 in ls2)
                            {
                                List<string> lkey0 = key0.Split('-').ToList();
                                List<string> lkey1 = key1.Split('-').ToList();
                                List<string> lkey2 = key2.Split('-').ToList();

                                if ((lkey0[0].Substring(0, lkey0[0].IndexOf("*")) == lkey2[0].Substring(0, lkey2[0].IndexOf("*")) &&//第1对赏肩同门
                                                              lkey1[0].Substring(0, lkey1[0].IndexOf("*")) == lkey2[1].Substring(0, lkey2[1].IndexOf("*"))) ||//第1对赏极同门
                                                             (lkey0[0].Substring(0, lkey0[0].IndexOf("*")) == lkey2[1].Substring(0, lkey2[1].IndexOf("*")) &&//第2对赏肩同门
                                                              lkey1[0].Substring(0, lkey1[0].IndexOf("*")) == lkey2[0].Substring(0, lkey2[0].IndexOf("*"))))//第2对赏极同门
                                {
                                    string key = EngineTool.FormatCardStrmd(key0 + "-" + key1 + "-" + key2);
                                    shangbaiduizuo.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.ShangBaiDuiZuo, Jifen = 12 } });
                                }
                            }
                        }
                    }
                }
                return shangbaiduizuo;
            }
        }

        #endregion


        #region 43.  肩百對座 同门一赏1肩二极  16贺(12+双飞*4)        
        private static Dictionary<string, List<CardType>> jianbaiduizuo = null;

        public static Dictionary<string, List<CardType>> JianBaiDuiZuo
        {
            get
            {
                if (jianbaiduizuo == null)
                {
                    jianbaiduizuo = new Dictionary<string, List<CardType>>();
                    int weight = 1;

                    List<string> ls0 = SelectSameCards(Jian.Keys.ToList(), 1);
                    List<string> ls1 = SelectSameCards(Bai.Keys.ToList(), 1);
                    List<string> ls2 = SelectSameCards(Ji.Keys.ToList(), 2);

                    List<string> lskey = new List<string>();
                    foreach (string key0 in ls0)
                    {
                        foreach (string key1 in ls1)
                        {
                            foreach (string key2 in ls2)
                            {
                                List<string> lkey0 = key0.Split('-').ToList();
                                List<string> lkey1 = key1.Split('-').ToList();
                                List<string> lkey2 = key2.Split('-').ToList();


                                if ((lkey0[0].Substring(0, lkey0[0].IndexOf("*")) == lkey2[0].Substring(0, lkey2[0].IndexOf("*")) &&//第1对赏肩同门
                                                              lkey1[0].Substring(0, lkey1[0].IndexOf("*")) == lkey2[1].Substring(0, lkey2[1].IndexOf("*"))) ||//第1对赏极同门
                                                             (lkey0[0].Substring(0, lkey0[0].IndexOf("*")) == lkey2[1].Substring(0, lkey2[1].IndexOf("*")) &&//第2对赏肩同门
                                                              lkey1[0].Substring(0, lkey1[0].IndexOf("*")) == lkey2[0].Substring(0, lkey2[0].IndexOf("*"))))//第2对赏极同门
                                {
                                    string key = EngineTool.FormatCardStrmd(key0 + "-" + key1 + "-" + key2);
                                    jianbaiduizuo.Add(key, new List<CardType> { new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.ShangBaiDuiZuo, Jifen = 12 } });
                                }
                            }
                        }
                    }
                }
                return jianbaiduizuo;
            }
        }

        #endregion


        #endregion



        #region 四、冲成奇色
        //免斗与斗上正色皆可冲 冲出副数减半
        //大参禅 小参禅 天女散花 需要二十万上桌冲出才算
        //公孙对坐 父子同登 肩对坐 百对坐 小百对坐 俱需极上桌冲赏才算
        #region 1.十红聚会  九红+九十万  100贺(60)        

        public static CardType ShiHongJuHui(List<string> ls)
        {
            List<String> lc = new List<String>(Hong.Keys.ToList());
            lc.Add(Cardsmd[36].SValue());
            string key = string.Join("-", lc.ToArray());
            string schong = string.Join("-", ls.ToArray());

            if (schong == key)
            {

                return new CardType { CardKey = key, Name = CardsTypeMdFan.ShiHongJuHui, Weight = 1, Jifen = 100 };
            }

            return null;
        }
        #endregion

        #region 2.满堂红  九张红牌  80贺        

        public static CardType JiuHong(List<string> ls)
        {
            List<String> lc = new List<String>(Hong.Keys.ToList());
            string key = string.Join("-", lc.ToArray());
            string schong = string.Join("-", ls.ToArray());

            if (schong == key)
            {

                return new CardType { CardKey = key, Name = CardsTypeMdFan.JiuHong, Weight = 1, Jifen = 80 };
            }

            return null;
        }
        #endregion

        #region 3.全红醉杨妃  九张红牌+九十万+二十万  80贺        

        public static CardType QuanHongZuiYangFei(List<string> ls)
        {
            List<String> lc = new List<String>(Hong.Keys.ToList());
            string key = string.Join("-", lc.ToArray());
            string schong = string.Join("-", ls.ToArray());

            if (schong == key)
            {

                return new CardType { CardKey = key, Name = CardsTypeMdFan.QuanHongZuiYangFei, Weight = 1, Jifen = 80 };
            }

            return null;
        }
        #endregion

        #region 4.九红醉杨妃  九张红牌+二十万  60贺        

        public static CardType JiuHongZuiYangFei(List<string> ls)
        {
            List<String> lc = new List<String>(Hong.Keys.ToList());
            string key = string.Join("-", lc.ToArray());
            string schong = string.Join("-", ls.ToArray());

            if (schong == key)
            {

                return new CardType { CardKey = key, Name = CardsTypeMdFan.JiuHongZuiYangFei, Weight = 1, Jifen = 60 };
            }

            return null;
        }
        #endregion

        #region 5.八红醉杨妃  8张红牌+二十万  40贺        

        public static List<CardType> BaHongZuiYangFei(List<string> ls)
        {
            List<String> lc = SelectSameCards(Hong.Keys.ToList(), 8);

            List<CardType> lct = new List<CardType>();

            string key = null;
            foreach (string s in lc)
            {
                List<string> lkey = s.Split('-').ToList();
                lkey.Add(Cardsmd[29].SValue());
                key = string.Join("-", lkey.ToArray());
                string schong = string.Join("-", ls.ToArray());

                for (int i = 0; i < lkey.Count; i++)
                {
                    if (!schong.Contains(lkey[i]))
                        break;
                    if (i == lkey.Count - 1)
                        lct.Add(new CardType { CardKey = key, Name = CardsTypeMdFan.BaHongZuiYangFei, Weight = 1, Jifen = 40 });
                }

            }
            return lct;
        }
        #endregion

        #region 6.九同  九牌同花  20贺        

        public static List<CardType> JiuTong(List<string> ls)
        {
            int weight = 1;
            List<CardType> lct = new List<CardType>();

            for (int i = 0; i < mdSuit.Count; i++)
            {
                if (i == 1 || i == 2)//9牌贯索必为同花顺
                    break;
                List<string> lkey = TongHuas(i, 9);
                foreach (string key in lkey)
                {
                    List<string> lk = key.Split('-').ToList();
                    for (int k = 0; k < lk.Count; k++)
                    {
                        if (!ls.Contains(lk[k]))
                            break;
                        if (k == lk.Count - 1)
                            lct.Add(new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.JiuTong, Jifen = 20 });
                    }
                }

            }
            return lct;
        }
        #endregion

        #region 7.十同  十牌同花  30贺        

        public static List<CardType> ShiTong(List<string> ls)
        {
            int weight = 1;
            List<CardType> lct = new List<CardType>();

            for (int i = 0; i < mdSuit.Count; i++)
            {
                if (i == 1 || i == 2)//贯 索 只有9张，不存在10同
                    break;
                List<string> lkey = TongHuas(i, 10);
                foreach (string key in lkey)
                {
                    List<string> lk = key.Split('-').ToList();
                    for (int k = 0; k < lk.Count; k++)
                    {
                        if (!ls.Contains(lk[k]))
                            break;
                        if (k == lk.Count - 1)
                            lct.Add(new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.JiuTong, Jifen = 30 });
                    }
                }

            }
            return lct;
        }
        #endregion

        #region 8.十连环  十牌同花顺  40贺        

        public static List<CardType> ShiLianHuang(List<string> ls)
        {
            int weight = 1;
            List<CardType> lct = new List<CardType>();

            for (int i = 0; i < mdSuit.Count; i++)
            {
                if (i == 1 || i == 2)//贯 索 只有9张，不存在10同
                    break;
                List<string> lkey = ShunZi(i, 10);
                foreach (string key in lkey)
                {
                    List<string> lk = key.Split('-').ToList();
                    for (int k = 0; k < lk.Count; k++)
                    {
                        if (!ls.Contains(lk[k]))
                            break;
                        if (k == lk.Count - 1)
                            lct.Add(new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.JiuTong, Jifen = 40 });
                    }
                }

            }
            return lct;
        }
        #endregion

        #region 8.九连环  十牌同花顺  25贺        

        public static List<CardType> JiuLianHuang(List<string> ls)
        {
            int weight = 1;
            List<CardType> lct = new List<CardType>();

            for (int i = 0; i < mdSuit.Count; i++)
            {
                List<string> lkey = ShunZi(i, 9);
                foreach (string key in lkey)
                {
                    List<string> lk = key.Split('-').ToList();
                    for (int k = 0; k < lk.Count; k++)
                    {
                        if (!ls.Contains(lk[k]))
                            break;
                        if (k == lk.Count - 1)
                            lct.Add(new CardType() { CardKey = key, Weight = weight++, Name = CardsTypeMdFan.JiuTong, Jifen = 25 });
                    }
                }

            }
            return lct;
        }
        #endregion

        #region 9.钱十全门  8张红牌+二十万  50贺        

        public static CardType QianShiQuanMen(List<string> ls)
        {

            List<CardType> lct = new List<CardType>();

            string scards = string.Join("-", ls.ToArray());
            if (ls.Count < 11)
                return null;
            if (scards.Contains(PokerConst.Diamond) || scards.Contains(PokerConst.Heart))
                return null;

            if (!scards.Contains(PokerConst.Spade) || !scards.Contains(PokerConst.Club))
                return new CardType { CardKey = scards, Name = CardsTypeMdFan.QianShiQuanMen, Weight = 1, Jifen = 50 };
            else
                return null;
        }
        #endregion

        #region 9.贯索全门  8张红牌+二十万  20贺        

        public static CardType GuanSuoQuanMen(List<string> ls)
        {

            List<CardType> lct = new List<CardType>();

            string scards = string.Join("-", ls.ToArray());
            if (ls.Count < 11)
                return null;
            if (scards.Contains(PokerConst.Spade) || scards.Contains(PokerConst.Club))
                return null;

            if (!scards.Contains(PokerConst.Diamond) || !scards.Contains(PokerConst.Heart))
                return new CardType { CardKey = scards, Name = CardsTypeMdFan.QianShiQuanMen, Weight = 1, Jifen = 20 };
            else
                return null;
        }
        #endregion

        #region 10.太极图  免斗色样含七同 另一张开冲再冲七同  40贺        

        public static CardType TaiJiTu(List<string> ls, List<string> lswin)
        {
            string scardswin = string.Join("-", lswin.ToArray());
            if (QiTongHua(scardswin).Count <= 0)
                return null;
            List<string> lqitongwin = QiTongHua(scardswin)[0].CardKey.Split('-').ToList();
            List<string> lchong = ls.Except(lqitongwin).ToList();
            string schong = string.Join("-", lchong.ToArray());
            if (QiTongHua(schong).Count > 0)
            {
                string key = string.Join("-", lqitongwin.ToArray()) + "\r\n" + schong;
                return new CardType { CardKey = key, Name = CardsTypeMdFan.TaiJiTu, Weight = 1, Jifen = 40 };
            }
            else
                return null;
        }
        #endregion

        #region 11.鸳鸯七同  两路冲出七同  30贺        

        public static CardType YuanYangQiTong(List<string> ls)
        {

            List<CardType> lct = QiTongHua(string.Join("-", ls.ToArray()));

            if (lct.Count == 2)
            {
                string key = lct[0].CardKey + "\r\n" + lct[1].CardKey;
                return new CardType { CardKey = key, Name = CardsTypeMdFan.YuanYangQiTong, Weight = 1, Jifen = 30 };
            }
            else
                return null;
        }
        #endregion

        #region 12.连环七同  冲出本门赏极各冲出七同  20贺        

        public static CardType LianHuanQiTong(List<string> ls)
        {

            List<CardType> lct = QiTongHua(string.Join("-", ls.ToArray()));
            List<string> lji = SelectSameCards(Ji.Keys.ToList(), 1);
            List<string> lshang = SelectSameCards(Shang.Keys.ToList(), 1);
            foreach (string sji in lji)
            {
                foreach (string sshang in lshang)
                {
                    if (lct.Count >= 2)
                    {
                        string key = lct[0].CardKey + "\r\n" + lct[1].CardKey;
                        int pos = sji.IndexOf('*');
                        if (key.Contains(sshang) && key.Contains(sji) &&
                            sji.Substring(0, pos) == sshang.Substring(0, pos))
                            return new CardType { CardKey = key, Name = CardsTypeMdFan.LianHuanQiTong, Weight = 1, Jifen = 20 };
                    }
                }
            }
            return null;
        }
        #endregion

        #region 13.金掘藏  九十万上桌冲出顺风旗(隔青不算)  100贺        

        public static CardType JinJueZang(List<string> ls, List<string> lswin)
        {
            string scards = string.Join("-", ls.ToArray());
            if (lswin.Contains(Cardsmd[36].SValue()) && !lswin.Contains(Cardsmd[29].SValue())
                && ls.Contains(Cardsmd[37].SValue())
                && ls.Contains(Cardsmd[38].SValue())
                && ls.Contains(Cardsmd[39].SValue()))
            {
                string key = Cardsmd[36].SValue()
                    + "-" + Cardsmd[37].SValue()
                    + "-" + Cardsmd[38].SValue()
                    + "-" + Cardsmd[39].SValue();
                return new CardType { CardKey = scards, Name = CardsTypeMdFan.JinJueZang, Weight = 1, Jifen = 100 };
            }
            return null;
        }
        #endregion

        #region 14.银掘藏  二十万上桌冲出顺风旗(隔青不算，不必百老顶冲)  60贺        

        public static CardType YinJueZang(List<string> ls, List<string> lswin)
        {
            string scards = string.Join("-", ls.ToArray());
            if (lswin.Contains(Cardsmd[29].SValue()) && !lswin.Contains(Cardsmd[36].SValue())
                && ls.Contains(Cardsmd[37].SValue())
                && ls.Contains(Cardsmd[38].SValue())
                && ls.Contains(Cardsmd[39].SValue()))
            {
                string key = Cardsmd[29].SValue()
                    + "-" + Cardsmd[37].SValue()
                    + "-" + Cardsmd[38].SValue()
                    + "-" + Cardsmd[39].SValue();
                return new CardType { CardKey = scards, Name = CardsTypeMdFan.YinJueZang, Weight = 1, Jifen = 60 };
            }
            return null;
        }
        #endregion

        #region 15、一冲六  6贺
        #endregion

        #region 16、一冲七(另算一冲六)  7贺
        #endregion

        #region 17、九十万上桌冲出百老且生成色样   30贺
        #endregion

        //18.大参禅 19.小参禅 20.天女散花   必须二十万上桌开冲才算
        //21.公孙对坐 22.父子同登 23.肩对坐 24.百对坐 25.小百对坐 俱需极上桌冲赏才算

        #region 26.夺锦标 一冲n夺(赏三百六肩极九十俱五)
        #endregion
        #endregion

        private static bool isHave20(string str)
        {
            List<string> ls = str.Split('-').ToList();
            if (ls.Exists(s => s == Cardsmd[29].SValue()))
                return true;
            else return false;
        }


    }
}
