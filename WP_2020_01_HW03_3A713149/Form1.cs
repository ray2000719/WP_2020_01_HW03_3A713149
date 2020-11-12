using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace WP_2020_01_HW03_3A713149
{
    public partial class Form1 : Form
    {
        List<Image> pokerpicture = new List<Image>();
        Poker p = null;
        List<int> poker; //抽牌公式
        List<int> playercard; //玩家手牌
        List<int> comcard1; //電腦1手牌
        List<int> comcard2; //電腦2手牌
        List<int> comcard3; //電腦3手牌
        List<int> playercardcheck;
        List<string> pokercolor;
        int playercardlength; //玩家手牌數
        int comcard1length; //電腦1手牌數
        int comcard2length; //電腦2手牌數
        int comcard3length; //電腦3手牌數
        int playercardchecklength;
        int card; //檯面上的卡代表的數字
        int cardmax;
        int roundnext;
        int go;
        int gocheck;
        int choose;
        int mode; //出牌模式
        string spade = "黑桃";
        string heart = "愛心";
        string diamond = "菱形";
        string club = "梅花";
        bool cardout;


        void Gamestart()//遊戲開始
        {
            DialogResult result;
            result = MessageBox.Show("按下是開始遊戲\n按下否退出遊戲", "大老2", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                this.Close();
            }
            for (int i = 8; i < 60; i++)
            {
                string resourceName = i.ToString();
                string resourceNum = "p" + resourceName;
                Bitmap bmp = (Bitmap)Properties.Resources.ResourceManager.GetObject(resourceNum);
                pokerpicture.Add(bmp);
                p = new Poker();
                poker = p.GetPoker(52);
            } 
            Playercard();
            Comcard();
            playercardlength = 13;
            comcard1length = 13;
            comcard2length = 13;
            comcard3length = 13;
            Mycard();
            labelmycard.Text = "我\n剩餘牌數:\n" + playercardlength;
            labelcom1.Text = "COM1\n剩餘牌數:\n" + comcard1length;
            labelcom2.Text = "COM2\n剩餘牌數:\n" + comcard2length;
            labelcom3.Text = "COM1\n剩餘牌數:\n" + comcard3length;
            pokercolor = new List<string>();
            pokercolor.Add(club);
            pokercolor.Add(diamond);
            pokercolor.Add(heart);
            pokercolor.Add(spade);
            labelcardout.Text = null;
            cardout = false;
            Playcards();

        }

        void Playercard()//玩家選擇的卡
        {
            playercard = new List<int>();
            for (int i = 0; i < 13; i++)
            {
                playercard.Add(poker[(i * 4)] + 8);
            }
            playercard.Sort(); ;
        }

        void Comcard()//COM選擇的卡
        {
            comcard1 = new List<int>();
            for (int i = 0; i < 13; i++)
            {
                comcard1.Add(poker[(i * 4) + 1] + 8);
            }
            comcard1.Sort();

            comcard2 = new List<int>();
            for (int i = 0; i < 13; i++)
            {
                comcard2.Add(poker[(i * 4) + 2] + 8);
            }
            comcard2.Sort();

            comcard3 = new List<int>();
            for (int i = 0; i < 13; i++)
            {
                comcard3.Add(poker[(i * 4) + 3] + 8);
            }
            comcard3.Sort();
        }

        void Playcards()//先出梅花三
        {
            card = 8;
            go = 0;
            for (int i = 0; i < 12; i++)
            {
                if (playercard[i] == 8)
                {
                    pbplaycards.Image = pokerpicture[playercard[i] - 8];
                    playercard.RemoveAt(i);
                    playercardlength -= 1;
                    roundnext = 2;
                    labelmycard.Text = "我\n剩餘牌數:\n" + playercardlength;
                    Mycard();
                }

                if (comcard1[i] == 8)
                {
                    pbplaycards.Image = pokerpicture[comcard1[i] - 8];
                    comcard1.RemoveAt(i);
                    comcard1length -= 1;
                    roundnext = 3;
                    labelcom1.Text = "COM1\n剩餘牌數:\n" + comcard1length;
                }

                if (comcard2[i] == 8)
                {
                    pbplaycards.Image = pokerpicture[comcard2[i] - 8];
                    comcard2.RemoveAt(i);
                    comcard2length -= 1;
                    roundnext = 4;
                    labelcom2.Text = "COM2\n剩餘牌數:\n" + comcard2length;
                }

                if (comcard3[i] == 8)
                {
                    pbplaycards.Image = pokerpicture[comcard3[i] - 8];
                    comcard3.RemoveAt(i);
                    comcard3length -= 1;
                    roundnext = 1;
                    labelcom3.Text = "COM3\n剩餘牌數:\n" + comcard3length;
                }
                
            }
            Pokercolor();
        }

        void Playercardcheck()//找出所有能打的牌
        {
            playercardcheck = new List<int>();
            playercardchecklength = 0;
            choose = 0;
            for (int i = 0; i < playercardlength; i++)
            {
                if (playercard[i] > card)
                {
                    playercardchecklength += 1;
                    playercardcheck.Add(playercard[i]);
                }
            }
            Playercardchoose();
            cardout = true;
            mode = 0;
        }

        void Playercardchoose()//將選擇打出的牌顯示在文字上
        {
            if (roundnext == 1 && playercardchecklength != 0)
            {
                int pkcolor = playercardcheck[choose] % 4;
                int cardshow = (playercardcheck[choose] / 4) + 1;

                if (cardshow < 11)
                {
                    labelcardout.Text = "選擇打出" + pokercolor[pkcolor] + cardshow;
                }
                if (cardshow == 11)
                {
                    labelcardout.Text = "選擇打出" + pokercolor[pkcolor] + "J";
                }
                if (cardshow == 12)
                {
                    labelcardout.Text = "選擇打出" + pokercolor[pkcolor] + "Q";
                }
                if (cardshow == 13)
                {
                    labelcardout.Text = "選擇打出" + pokercolor[pkcolor] + "K";
                }
                if (cardshow == 14)
                {
                    labelcardout.Text = "選擇打出" + pokercolor[pkcolor] + "A";
                }
                if (cardshow == 15)
                {
                    labelcardout.Text = "選擇打出" + pokercolor[pkcolor] + "2";
                }
            }
            if (roundnext == 1 && playercardchecklength == 0)
            {
                labelcardout.Text = "選擇 PASS這回合";
            }
        }

        void Playercardout()//在玩家打出牌後改變手牌
        {
            if (playercardchecklength != 0)
            {
                for (int i = 0; i < playercardlength; i++)
                {
                    if (playercard[i] == playercardcheck[choose])
                    {
                        pbplaycards.Image = pokerpicture[playercardcheck[choose] - 8];
                        card = playercardcheck[choose];
                        playercard.RemoveAt(i);
                        playercardlength -= 1;
                        cardmax = 0;
                        Mycard();
                    }
                }
            }
            else
            {
                cardmax += 1;
            }
            roundnext = 2;
            gocheck = 0;
            labelmycard.Text = "我\n剩餘牌數:\n" + playercardlength;
            Pokercolor();
            Gameover();
            cardout = false;
        }

        void Comcard1out()//在COM1打出牌後改變手牌
        {
            for (int i = 0; i < comcard1length; i++)
            {
                if (go == 1)
                {
                    if (comcard1[i] > card)
                    {
                        pbplaycards.Image = pokerpicture[comcard1[i] - 8];
                        card = comcard1[i];
                        comcard1.RemoveAt(i);
                        comcard1length -= 1;
                        go = 0;
                        cardmax = 0;
                    }
                    else
                    {
                        if (i == comcard1length - 1)
                        {
                            cardmax += 1;
                        }
                    }  
                }
            }
            roundnext = 3;
            gocheck = 0;
            labelcom1.Text = "COM1\n剩餘牌數:\n" + comcard1length;
            Pokercolor();
            Gameover();
        }

        void Comcard2out()//在COM2打出牌後改變手牌
        {
            for (int i = 0; i < comcard2length; i++)
            {
                if (go == 1)
                {
                    if (comcard2[i] > card)
                    {
                        pbplaycards.Image = pokerpicture[comcard2[i] - 8];
                        card = comcard2[i];
                        comcard2.RemoveAt(i);
                        comcard2length -= 1;
                        go = 0;
                        cardmax = 0;
                    }
                    else
                    {
                        if (i == comcard2length - 1)
                        {
                            cardmax += 1;
                        }
                    }
                }
            }
            roundnext = 4;
            gocheck = 0;
            labelcom2.Text = "COM2\n剩餘牌數:\n" + comcard2length;
            Pokercolor();
            Gameover();
        }

        void Comcard3out()//在COM3打出牌後改變手牌
        {
            for (int i = 0; i < comcard3length; i++)
            {
                if (go == 1)
                {
                    if (comcard3[i] > card)
                    {
                        pbplaycards.Image = pokerpicture[comcard3[i] - 8];
                        card = comcard3[i];
                        comcard3.RemoveAt(i);
                        comcard3length -= 1;
                        go = 0;
                        cardmax = 0;
                    }
                    else
                    {
                        if (i == comcard3length - 1)
                        {
                            cardmax += 1;
                        }
                    }                 
                }
            }
            roundnext = 1;
            gocheck = 0;
            labelcom3.Text = "COM3\n剩餘牌數:\n" + comcard3length;
            Pokercolor();
            Gameover();
        }

        public int Compare(int x, int y)//將陣列排序成從A~K的判斷式
        {
            if (x > 51)
            {
                if (y > 51)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return 1;
            }
        }

        void Mycard()//改變玩家手牌的圖案
        {
            List<int> playercardpicture = new List<int>(playercard);
            if (playercardlength<4)
            {
                playercardpicture.Reverse();
            }
            playercardpicture.Sort(Compare);
            if (playercardlength == 13)
            {
                pbmycard1.Image = pokerpicture[playercardpicture[0] - 8];
                pbmycard2.Image = pokerpicture[playercardpicture[1] - 8];
                pbmycard3.Image = pokerpicture[playercardpicture[2] - 8];
                pbmycard4.Image = pokerpicture[playercardpicture[3] - 8];
                pbmycard5.Image = pokerpicture[playercardpicture[4] - 8];
                pbmycard6.Image = pokerpicture[playercardpicture[5] - 8];
                pbmycard7.Image = pokerpicture[playercardpicture[6] - 8];
                pbmycard8.Image = pokerpicture[playercardpicture[7] - 8];
                pbmycard9.Image = pokerpicture[playercardpicture[8] - 8];
                pbmycard10.Image = pokerpicture[playercardpicture[9] - 8];
                pbmycard11.Image = pokerpicture[playercardpicture[10] - 8];
                pbmycard12.Image = pokerpicture[playercardpicture[11] - 8];
                pbmycard13.Image = pokerpicture[playercardpicture[12] - 8];
            }
            if (playercardlength == 12)
            {
                pbmycard1.Image = pokerpicture[playercardpicture[0] - 8];
                pbmycard2.Image = pokerpicture[playercardpicture[1] - 8];
                pbmycard3.Image = pokerpicture[playercardpicture[2] - 8];
                pbmycard4.Image = pokerpicture[playercardpicture[3] - 8];
                pbmycard5.Image = pokerpicture[playercardpicture[4] - 8];
                pbmycard6.Image = pokerpicture[playercardpicture[5] - 8];
                pbmycard7.Image = pokerpicture[playercardpicture[6] - 8];
                pbmycard8.Image = pokerpicture[playercardpicture[7] - 8];
                pbmycard9.Image = pokerpicture[playercardpicture[8] - 8];
                pbmycard10.Image = pokerpicture[playercardpicture[9] - 8];
                pbmycard11.Image = pokerpicture[playercardpicture[10] - 8];
                pbmycard12.Image = pokerpicture[playercardpicture[11] - 8];
                pbmycard13.Image = null;
            }
            if (playercardlength == 11)
            {
                pbmycard1.Image = pokerpicture[playercardpicture[0] - 8];
                pbmycard2.Image = pokerpicture[playercardpicture[1] - 8];
                pbmycard3.Image = pokerpicture[playercardpicture[2] - 8];
                pbmycard4.Image = pokerpicture[playercardpicture[3] - 8];
                pbmycard5.Image = pokerpicture[playercardpicture[4] - 8];
                pbmycard6.Image = pokerpicture[playercardpicture[5] - 8];
                pbmycard7.Image = pokerpicture[playercardpicture[6] - 8];
                pbmycard8.Image = pokerpicture[playercardpicture[7] - 8];
                pbmycard9.Image = pokerpicture[playercardpicture[8] - 8];
                pbmycard10.Image = pokerpicture[playercardpicture[9] - 8];
                pbmycard11.Image = pokerpicture[playercardpicture[10] - 8];
                pbmycard12.Image = null;
                pbmycard13.Image = null;
            }
            if (playercardlength == 10)
            {
                pbmycard1.Image = pokerpicture[playercardpicture[0] - 8];
                pbmycard2.Image = pokerpicture[playercardpicture[1] - 8];
                pbmycard3.Image = pokerpicture[playercardpicture[2] - 8];
                pbmycard4.Image = pokerpicture[playercardpicture[3] - 8];
                pbmycard5.Image = pokerpicture[playercardpicture[4] - 8];
                pbmycard6.Image = pokerpicture[playercardpicture[5] - 8];
                pbmycard7.Image = pokerpicture[playercardpicture[6] - 8];
                pbmycard8.Image = pokerpicture[playercardpicture[7] - 8];
                pbmycard9.Image = pokerpicture[playercardpicture[8] - 8];
                pbmycard10.Image = pokerpicture[playercardpicture[9] - 8];
                pbmycard11.Image = null;
                pbmycard12.Image = null;
                pbmycard13.Image = null;
            }
            if (playercardlength == 9)
            {
                pbmycard1.Image = pokerpicture[playercardpicture[0] - 8];
                pbmycard2.Image = pokerpicture[playercardpicture[1] - 8];
                pbmycard3.Image = pokerpicture[playercardpicture[2] - 8];
                pbmycard4.Image = pokerpicture[playercardpicture[3] - 8];
                pbmycard5.Image = pokerpicture[playercardpicture[4] - 8];
                pbmycard6.Image = pokerpicture[playercardpicture[5] - 8];
                pbmycard7.Image = pokerpicture[playercardpicture[6] - 8];
                pbmycard8.Image = pokerpicture[playercardpicture[7] - 8];
                pbmycard9.Image = pokerpicture[playercardpicture[8] - 8];
                pbmycard10.Image = null;
                pbmycard11.Image = null;
                pbmycard12.Image = null;
                pbmycard13.Image = null;
            }
            if (playercardlength == 8)
            {
                pbmycard1.Image = pokerpicture[playercardpicture[0] - 8];
                pbmycard2.Image = pokerpicture[playercardpicture[1] - 8];
                pbmycard3.Image = pokerpicture[playercardpicture[2] - 8];
                pbmycard4.Image = pokerpicture[playercardpicture[3] - 8];
                pbmycard5.Image = pokerpicture[playercardpicture[4] - 8];
                pbmycard6.Image = pokerpicture[playercardpicture[5] - 8];
                pbmycard7.Image = pokerpicture[playercardpicture[6] - 8];
                pbmycard8.Image = pokerpicture[playercardpicture[7] - 8];
                pbmycard9.Image = null;
                pbmycard10.Image = null;
                pbmycard11.Image = null;
                pbmycard12.Image = null;
                pbmycard13.Image = null;
            }
            if (playercardlength == 7)
            {
                pbmycard1.Image = pokerpicture[playercardpicture[0] - 8];
                pbmycard2.Image = pokerpicture[playercardpicture[1] - 8];
                pbmycard3.Image = pokerpicture[playercardpicture[2] - 8];
                pbmycard4.Image = pokerpicture[playercardpicture[3] - 8];
                pbmycard5.Image = pokerpicture[playercardpicture[4] - 8];
                pbmycard6.Image = pokerpicture[playercardpicture[5] - 8];
                pbmycard7.Image = pokerpicture[playercardpicture[6] - 8];
                pbmycard8.Image = null;
                pbmycard9.Image = null;
                pbmycard10.Image = null;
                pbmycard11.Image = null;
                pbmycard12.Image = null;
                pbmycard13.Image = null;
            }
            if (playercardlength == 6)
            {
                pbmycard1.Image = pokerpicture[playercardpicture[0] - 8];
                pbmycard2.Image = pokerpicture[playercardpicture[1] - 8];
                pbmycard3.Image = pokerpicture[playercardpicture[2] - 8];
                pbmycard4.Image = pokerpicture[playercardpicture[3] - 8];
                pbmycard5.Image = pokerpicture[playercardpicture[4] - 8];
                pbmycard6.Image = pokerpicture[playercardpicture[5] - 8];
                pbmycard7.Image = null;
                pbmycard8.Image = null;
                pbmycard9.Image = null;
                pbmycard10.Image = null;
                pbmycard11.Image = null;
                pbmycard12.Image = null;
                pbmycard13.Image = null;
            }
            if (playercardlength == 5)
            {
                pbmycard1.Image = pokerpicture[playercardpicture[0] - 8];
                pbmycard2.Image = pokerpicture[playercardpicture[1] - 8];
                pbmycard3.Image = pokerpicture[playercardpicture[2] - 8];
                pbmycard4.Image = pokerpicture[playercardpicture[3] - 8];
                pbmycard5.Image = pokerpicture[playercardpicture[4] - 8];
                pbmycard6.Image = null;
                pbmycard7.Image = null;
                pbmycard8.Image = null;
                pbmycard9.Image = null;
                pbmycard10.Image = null;
                pbmycard11.Image = null;
                pbmycard12.Image = null;
                pbmycard13.Image = null;
            }
            if (playercardlength == 4)
            {
                pbmycard1.Image = pokerpicture[playercardpicture[0] - 8];
                pbmycard2.Image = pokerpicture[playercardpicture[1] - 8];
                pbmycard3.Image = pokerpicture[playercardpicture[2] - 8];
                pbmycard4.Image = pokerpicture[playercardpicture[3] - 8];
                pbmycard5.Image = null;
                pbmycard6.Image = null;
                pbmycard7.Image = null;
                pbmycard8.Image = null;
                pbmycard9.Image = null;
                pbmycard10.Image = null;
                pbmycard11.Image = null;
                pbmycard12.Image = null;
                pbmycard13.Image = null;
            }
            if (playercardlength == 3)
            {
                pbmycard1.Image = pokerpicture[playercardpicture[0] - 8];
                pbmycard2.Image = pokerpicture[playercardpicture[1] - 8];
                pbmycard3.Image = pokerpicture[playercardpicture[2] - 8];
                pbmycard4.Image = null;
                pbmycard5.Image = null;
                pbmycard6.Image = null;
                pbmycard7.Image = null;
                pbmycard8.Image = null;
                pbmycard9.Image = null;
                pbmycard10.Image = null;
                pbmycard11.Image = null;
                pbmycard12.Image = null;
                pbmycard13.Image = null;
            }
            if (playercardlength == 2)
            {
                pbmycard1.Image = pokerpicture[playercardpicture[0] - 8];
                pbmycard2.Image = pokerpicture[playercardpicture[1] - 8];
                pbmycard3.Image = null;
                pbmycard4.Image = null;
                pbmycard5.Image = null;
                pbmycard6.Image = null;
                pbmycard7.Image = null;
                pbmycard8.Image = null;
                pbmycard9.Image = null;
                pbmycard10.Image = null;
                pbmycard11.Image = null;
                pbmycard12.Image = null;
                pbmycard13.Image = null;
            }
            if (playercardlength == 1)
            {
                pbmycard1.Image = pokerpicture[playercardpicture[0] - 8];
                pbmycard2.Image = null;
                pbmycard3.Image = null;
                pbmycard4.Image = null;
                pbmycard5.Image = null;
                pbmycard6.Image = null;
                pbmycard7.Image = null;
                pbmycard8.Image = null;
                pbmycard9.Image = null;
                pbmycard10.Image = null;
                pbmycard11.Image = null;
                pbmycard12.Image = null;
                pbmycard13.Image = null;
            }
            if (playercardlength == 0)
            {
                pbmycard1.Image = null;
                pbmycard2.Image = null;
                pbmycard3.Image = null;
                pbmycard4.Image = null;
                pbmycard5.Image = null;
                pbmycard6.Image = null;
                pbmycard7.Image = null;
                pbmycard8.Image = null;
                pbmycard9.Image = null;
                pbmycard10.Image = null;
                pbmycard11.Image = null;
                pbmycard12.Image = null;
                pbmycard13.Image = null;
            }
        }

        void Round()//回合
        {
            go = 1;
            gocheck = 1;
            if (roundnext == 1 && cardmax == 3)
            {
                if (gocheck == 1)
                {
                    card = 0;
                    cardmax = 0;
                    Playercardcheck();
                }
            }
            if (roundnext == 2 && cardmax == 3)
            {
                if (gocheck == 1)
                {
                    card = 0;
                    cardmax = 0;
                    Comcard1out();
                }
            }
            if (roundnext == 3 && cardmax == 3)
            {
                if (gocheck == 1)
                {
                    card = 0;
                    cardmax = 0;
                    Comcard2out();
                }
            }
            if (roundnext == 4 && cardmax == 3)
            {
                if (gocheck == 1)
                {
                    card = 0;
                    cardmax = 0;
                    Comcard3out();
                }
            }
            if (roundnext == 1 && cardmax != 3)
            {
                if (gocheck == 1)
                {
                    Playercardcheck();
                }
            }
            if (roundnext == 2 && cardmax != 3)
            {
                if (gocheck == 1)
                {
                    Comcard1out();
                }
            }
            if (roundnext == 3 && cardmax != 3)
            {
                if (gocheck == 1)
                {
                    Comcard2out();
                }
            }
            if (roundnext == 4 && cardmax != 3)
            {
                if (gocheck == 1)
                {
                    Comcard3out();
                }
            }
        }

        void Gameover()//遊戲結束
        {
            if (roundnext == 2 && playercardlength == 0)
            {
                    DialogResult result;
                    result = MessageBox.Show("玩家獲勝", "遊戲結束", MessageBoxButtons.OK);
                    Gamestart();
            }
            if (roundnext == 3 && comcard1length == 0)
            {
                    DialogResult result;
                    result = MessageBox.Show("COM1獲勝", "遊戲結束", MessageBoxButtons.OK);
                    Gamestart();
            }
            if (roundnext == 4 && comcard2length == 0)
            {
                    DialogResult result;
                    result = MessageBox.Show("COM2獲勝", "遊戲結束", MessageBoxButtons.OK);
                    Gamestart();
            }
            if (roundnext == 1 && comcard3length == 0)
            {
                    DialogResult result;
                    result = MessageBox.Show("COM3獲勝", "遊戲結束", MessageBoxButtons.OK);
                    Gamestart();
            }
        }

        void Pokercolor()//將當回合的人和出牌的卡顯示在文字框裡
        {
            int pkcolor = card % 4;
            int cardshow = (card / 4) + 1;
            if (roundnext == 2 && cardmax == 0)
            {
                if (cardshow < 11)
                {
                    rtbround.Text = "玩家 打出" + pokercolor[pkcolor] + cardshow + "\n";
                }
                if (cardshow == 11)
                {
                    rtbround.Text = "玩家 打出" + pokercolor[pkcolor] + "J" + "\n";
                }
                if (cardshow == 12)
                {
                    rtbround.Text = "玩家 打出" + pokercolor[pkcolor] + "Q" + "\n";
                }
                if (cardshow == 13)
                {
                    rtbround.Text = "玩家 打出" + pokercolor[pkcolor] + "K" + "\n";
                }
                if (cardshow == 14)
                {
                    rtbround.Text = "玩家 打出" + pokercolor[pkcolor] + "A" + "\n";
                }
                if (cardshow == 15)
                {
                    rtbround.Text = "玩家 打出" + pokercolor[pkcolor] + "2" + "\n";
                }
            }
            if (roundnext == 2 && cardmax != 0)
            {
                rtbround.Text = "玩家 PASS這回合";
            }

            if (roundnext == 3 && cardmax == 0)
            {
                if (cardshow < 11)
                {
                    rtbround.Text = "COM1 打出" + pokercolor[pkcolor] + cardshow + "\n";
                }
                if (cardshow == 11)
                {
                    rtbround.Text = "COM1 打出" + pokercolor[pkcolor] + "J" + "\n";
                }
                if (cardshow == 12)
                {
                    rtbround.Text = "COM1 打出" + pokercolor[pkcolor] + "Q" + "\n";
                }
                if (cardshow == 13)
                {
                    rtbround.Text = "COM1 打出" + pokercolor[pkcolor] + "K" + "\n";
                }
                if (cardshow == 14)
                {
                    rtbround.Text = "COM1 打出" + pokercolor[pkcolor] + "A" + "\n";
                }
                if (cardshow == 15)
                {
                    rtbround.Text = "COM1 打出" + pokercolor[pkcolor] + "2" + "\n";
                }
            }
            if (roundnext == 3 && cardmax != 0)
            {
                rtbround.Text = "COM1 PASS這回合" + "\n";
            }

            if (roundnext == 4 && cardmax == 0)
            {
                if (cardshow < 11)
                {
                    rtbround.Text = "COM2 打出" + pokercolor[pkcolor] + cardshow + "\n";
                }
                if (cardshow == 11)
                {
                    rtbround.Text = "COM2 打出" + pokercolor[pkcolor] + "J" + "\n";
                }
                if (cardshow == 12)
                {
                    rtbround.Text = "COM2 打出" + pokercolor[pkcolor] + "Q" + "\n";
                }
                if (cardshow == 13)
                {
                    rtbround.Text = "COM2 打出" + pokercolor[pkcolor] + "K" + "\n";
                }
                if (cardshow == 14)
                {
                    rtbround.Text = "COM2 打出" + pokercolor[pkcolor] + "A" + "\n";
                }
                if (cardshow == 15)
                {
                    rtbround.Text = "COM2 打出" + pokercolor[pkcolor] + "2" + "\n";
                }
            }
            if (roundnext == 4 && cardmax != 0)
            {
                rtbround.Text = "COM2 PASS這回合" + "\n";
            }

            if (roundnext == 1 && cardmax == 0)
            {
                if (cardshow < 11)
                {
                    rtbround.Text = "COM3 打出" + pokercolor[pkcolor] + cardshow + "\n";
                }
                if (cardshow == 11)
                {
                    rtbround.Text = "COM3 打出" + pokercolor[pkcolor] + "J" + "\n";
                }
                if (cardshow == 12)
                {
                    rtbround.Text = "COM3 打出" + pokercolor[pkcolor] + "Q" + "\n";
                }
                if (cardshow == 13)
                {
                    rtbround.Text = "COM3 打出" + pokercolor[pkcolor] + "K" + "\n";
                }
                if (cardshow == 14)
                {
                    rtbround.Text = "COM3 打出" + pokercolor[pkcolor] + "A" + "\n";
                }
                if (cardshow == 15)
                {
                    rtbround.Text = "COM3 打出" + pokercolor[pkcolor] + "2" + "\n";
                }
            }
            if (roundnext == 1 && cardmax != 0)
            {
                rtbround.Text = "COM3 PASS這回合" + "\n";
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Gamestart();
        }

        private void btnprompt_Click(object sender, EventArgs e)
        {
            MessageBox.Show("A鍵是開啟提示\n" +
                    "S鍵是重新開始\n" +
                    "Z鍵是下一步\n" +
                    "X鍵是將牌打出\n" +
                    "C鍵是選擇要打出的牌\n", "大老2說明", MessageBoxButtons.OK);
        }

        private void btnrestart_Click(object sender, EventArgs e)
        {
            Gamestart();
        }

        private void btnnext_Click(object sender, EventArgs e)
        {
            Round();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A)
            {
                MessageBox.Show("A鍵是開啟提示\n" +
                    "S鍵是重新開始\n" +
                    "Z鍵是下一步\n" +
                    "X鍵是將牌打出\n" +
                    "C鍵是選擇要打出的牌\n" +
                    "V鍵是選擇模式\n(單張,兔胚,葫蘆,順子,鐵支,一條龍,PASS)\n", "大老2說明", MessageBoxButtons.OK);
            }
            if (e.KeyCode == Keys.Z)
            {
                Round();
            }
            if (e.KeyCode == Keys.S)
            {
                Gamestart();
            }
            if (cardout == true)
            {
                if (e.KeyCode == Keys.V && cardmax ==3)
                {
                    mode += 1;
                    if (mode == 2)
                    {
                        mode = 0;
                    }
                    if (mode == 0)
                    {
                        Playercardcheck();
                    }
                    if (mode == 1)
                    {
                        playercardchecklength = 0;
                    }
                    Playercardchoose();
                }
                if (e.KeyCode == Keys.C)
                {
                    choose += 1;
                    if (choose == playercardchecklength)
                    {
                        choose = 0;
                    }
                    Playercardchoose();
                }
                if (e.KeyCode == Keys.X)
                {
                    Playercardout();
                    labelcardout.Text = null;
                }
            }
        }
    }
}
