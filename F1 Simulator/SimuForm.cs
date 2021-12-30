using System;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace F1_Simulator
{
    public partial class SimuForm : Form
    {
        //全局对象
        public Capability cap;
        DataConsole dc;
        public Rank r = new Rank();
        public Leaderboard l = new Leaderboard();
        Circuits[] cirs = new Circuits[23];
        //全局量
        public string name = "LLZ";
        public string name2 = "HAM";
        public string seasondriver = "";
        public string seasonteam = "";
        public string[] seasondata;
        public string[] seasondata2;
        public int seasongame = 0;
        public int sdindex = 0;
        public int waitdelta = 1000;
        public int games = 0;
        public int seasons = 0;
        //车手设定
        public string careername = "LLZ";
        public int potential = 98;
        //游戏设定值
        public int careerseason = 15;
        public int carchangedelta = 3;
        public int capset = 1;     //0-high,1-mid,2-low.
        public int transset = 1;
        public int carset = 1;
        public int carchange = 1;
        public bool savelog = true;
        //标志
        public bool isspeed1 = true;
        public bool ispause = false;
        public bool career = false;
        public bool endcareer = false;
        public bool endseason = false;

        public SimuForm()
        {
            InitializeComponent();
        }

        public void ReadCap()
        {
            //读取能力值，并初始化
            string[] configs = File.ReadAllLines("./cap.txt");
            Capability[] cp = new Capability[20];
            for(int i = 0; i < 20; i++)
            {
                cp[i] = new Capability(configs[i]);
            }
            comboBox.BeginInvoke(new Action(() =>
            {
                for (int i = 0; i < 20; i++)
                {
                    comboBox.Items.Add(cp[i].name);
                }
            }));
            comboBoxcareer.BeginInvoke(new Action(() =>
            {
                comboBoxcareer.Items.Clear();
                for (int i = 0; i < 20; i++)
                {
                    comboBoxcareer.Items.Add(cp[i].name);
                }
            }));
            r.Init(cp);
            l.Init(cp);
            name = cp[19].name;
            name2 = cp[0].name;
            labelspeed.BeginInvoke(new Action(() =>
            {
                labelspeed.Text = name;
            }));
            labelspeed2.BeginInvoke(new Action(() =>
            {
                labelspeed2.Text = name2;
            }));
        }

        public void ReadTeam()
        {
            //读取能力值，并初始化
            string[] configs = File.ReadAllLines("./team.txt");
            Team[] tm = new Team[10];
            for (int i = 0; i < 10; i++)
            {
                tm[i] = new Team(configs[i]);
            }
            r.Init(tm);
            l.init(tm);
        }

        public void ReadCircuits()
        {
            //读取赛道
            string[] configs = File.ReadAllLines("./circuit.txt");
            for (int i = 0; i < 23; i++)
            {
                cirs[i] = new Circuits(configs[i]);
            }
        }

        public void SetCircuitImage(string name)
        {
            //设置背景图片
            switch (name)
            {
                case "澳大利亚":
                    panelrank.BackgroundImage = TransparentImage(Properties.Resources.澳大利亚);
                    break;
                case "巴林":
                    panelrank.BackgroundImage = TransparentImage(Properties.Resources.巴林);
                    break;
                case "伊莫拉":
                    panelrank.BackgroundImage = TransparentImage(Properties.Resources.伊莫拉);
                    break;
                case "西班牙":
                    panelrank.BackgroundImage = TransparentImage(Properties.Resources.西班牙);
                    break;
                case "葡萄牙":
                    panelrank.BackgroundImage = TransparentImage(Properties.Resources.葡萄牙);
                    break;
                case "摩纳哥":
                    panelrank.BackgroundImage = TransparentImage(Properties.Resources.摩纳哥);
                    break;
                case "阿塞拜疆":
                    panelrank.BackgroundImage = TransparentImage(Properties.Resources.阿塞拜疆);
                    break;
                case "法国":
                    panelrank.BackgroundImage = TransparentImage(Properties.Resources.法国);
                    break;
                case "奥地利":
                    panelrank.BackgroundImage = TransparentImage(Properties.Resources.奥地利);
                    break;
                case "英国":
                    panelrank.BackgroundImage = TransparentImage(Properties.Resources.英国);
                    break;
                case "德国":
                    panelrank.BackgroundImage = TransparentImage(Properties.Resources.德国);
                    break;
                case "土耳其":
                    panelrank.BackgroundImage = TransparentImage(Properties.Resources.土耳其);
                    break;
                case "匈牙利":
                    panelrank.BackgroundImage = TransparentImage(Properties.Resources.匈牙利);
                    break;
                case "比利时":
                    panelrank.BackgroundImage = TransparentImage(Properties.Resources.比利时);
                    break;
                case "荷兰":
                    panelrank.BackgroundImage = TransparentImage(Properties.Resources.荷兰);
                    break;
                case "意大利":
                    panelrank.BackgroundImage = TransparentImage(Properties.Resources.意大利);
                    break;
                case "日本":
                    panelrank.BackgroundImage = TransparentImage(Properties.Resources.日本);
                    break;
                case "中国":
                    panelrank.BackgroundImage = TransparentImage(Properties.Resources.中国);
                    break;
                case "新加坡":
                    panelrank.BackgroundImage = TransparentImage(Properties.Resources.新加坡);
                    break;
                case "美国":
                    panelrank.BackgroundImage = TransparentImage(Properties.Resources.美国);
                    break;
                case "墨西哥":
                    panelrank.BackgroundImage = TransparentImage(Properties.Resources.墨西哥);
                    break;
                case "巴西":
                    panelrank.BackgroundImage = TransparentImage(Properties.Resources.巴西);
                    break;
                case "阿布扎比":
                    panelrank.BackgroundImage = TransparentImage(Properties.Resources.阿布扎比);
                    break;
            }
            panelrank.BackgroundImageLayout = ImageLayout.Zoom;
        }

        private Image TransparentImage(Image srcImage, float opacity = (float)0.5)
        {
            //转换图片透明度
            float[][] nArray ={ new float[] {1, 0, 0, 0, 0},
                        new float[] {0, 1, 0, 0, 0},
                        new float[] {0, 0, 1, 0, 0},
                        new float[] {0, 0, 0, opacity, 0},
                        new float[] {0, 0, 0, 0, 1}};
            ColorMatrix matrix = new ColorMatrix(nArray);
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            Bitmap resultImage = new Bitmap(srcImage.Width, srcImage.Height);
            Graphics g = Graphics.FromImage(resultImage);
            g.DrawImage(srcImage, new Rectangle(0, 0, srcImage.Width, srcImage.Height), 0, 0, srcImage.Width, srcImage.Height, GraphicsUnit.Pixel, attributes);
            return resultImage;
        }

        public void simulate_Game(Circuits cir, Log log)
        {
            //比赛模拟
            textBoxmc.BeginInvoke(new Action(() =>
            {
                textBoxmc.Text = "";
            }));
            string backimage = cir.name;
            SetCircuitImage(backimage);
            Random ran = new Random(Rank.Getseed());
            int laps = cir.laps;
            progressBargame.BeginInvoke(new Action(() =>
            {
                progressBargame.Maximum = laps;
                progressBargame.Value = 0;
            }));
            bool israin = false;
            if (ran.Next(1, 101) >= 82)
            {
                israin = true;
            }
            //排位赛
            r.Start(cir, israin, true);
            //Q1
            r.Getquansingle(israin, 1);
            r.Lapfast();
            refreshrank(cir, 1, r, true, israin, 1);
            pause();
            Thread.Sleep(3 * waitdelta);
            pause();
            //Q2
            r.Getquansingle(israin, 2);
            r.Lapfast();
            refreshrank(cir, 1, r, true, israin, 2);
            pause();
            Thread.Sleep(3 * waitdelta);
            pause();
            //Q3
            string q = r.Getquansingle(israin, 3);
            addmc(q, 0);
            r.Lapfast();
            refreshrank(cir, 1, r, true, israin, 3);
            r.Setindex();
            addinfo(r, true, israin, log);
            l.Getpole(r);
            l.GetQuanData(r);
            pause();
            Thread.Sleep(3 * waitdelta);
            pause();
            //正赛
            r.Setpetrol(laps);
            if (israin)
            {
                if (ran.Next(1, 101) >= 85)
                {
                    israin = false;
                }
            }
            else
            {
                if (ran.Next(1, 101) >= 85)
                {
                    israin = true;
                }
            }
            r.Start(cir, israin, false);
            for (int i = 0; i < 20; i++)
            {
                r.cars[i].pitlap = (int)(ran.NextDouble() * 10 + laps * 0.3);
            }
            r.Clear();
            r.Startdelta();
            Thread.Sleep(waitdelta);
            string w = r.Getsingle(israin, false);//第一圈
            addmc(w, 1);
            progressBargame.BeginInvoke(new Action(() =>
            {
                progressBargame.Value++;
            }));
            r.Lapfast();
            refreshrank(cir, 1, r, false, israin, 0);
            refreshspeed(r, name, true);
            refreshspeed(r, name2, false);
            pause();
            for (int i = 1; i < laps; i++)
            {
                pause();
                Thread.Sleep(waitdelta);
                string e = r.Getsingle(israin, true);
                addmc(e, i + 1);
                progressBargame.BeginInvoke(new Action(() =>
                {
                    progressBargame.Value++;
                }));
                for (int j = 0; j < 20; j++)
                {
                    if (r.cars[j].pitlap == i)
                    {
                        string p =r.cars[j].Pit();
                        addmc(p, i + 1);
                    }
                }
                r.Lapfast();
                refreshrank(cir, i + 1, r, false, israin, 0);
                refreshspeed(r, name, true);
                refreshspeed(r, name2, false);
                pause();
            }
            games++;
            addinfo(r, false, israin, log);
            l.Getdata(r);
            refreshdata(l);
            addinfomation(r.GetOverTake(), log, savelog);
            pause();
            Thread.Sleep(3 * waitdelta);
            pause();
            r.Clear();
        }

        public void simulate_Season(Object log = null)
        {
            //模拟赛季
            Random r = new Random(Rank.Getseed());
            seasongame = r.Next(18, 24);
            addinfomation("本赛季共" + seasongame.ToString() + "场比赛。\r\n", log as Log, savelog);
            int gamed = 23 - seasongame;
            Circuits[] Array1 = cirs;
            for(int i = 0; i < gamed; i++)
            {
                int inn = r.Next(0, Array1.Length);
                Circuits[] Array2 = Array1.Where(s => Array1.ToList().IndexOf(s) != inn).ToArray();
                Array1 = Array2;
            }
            string calender = "本赛季的赛历为：";
            for(int i = 0; i < Array1.Length; i++)
            {
                calender += Array1[i].name + "  ";
            }
            addinfomation(calender + "\r\n", log as Log, savelog);
            progressBarseason.BeginInvoke(new Action(() =>
            {
                progressBarseason.Maximum = seasongame;
                progressBarseason.Value = 0;
            }));
            for (int i = 1; i <= seasongame; i++)
            {
                pause();
                addinfomation("第" + i.ToString() + "场：(" + Array1[i - 1].name + "大奖赛)\r\n", log as Log, savelog);
                textBoxspeed.BeginInvoke(new Action(() =>
                {
                    textBoxspeed.AppendText("第" + i.ToString() + "场：\r\n");
                }));
                textBoxspeed2.BeginInvoke(new Action(() =>
                {
                    textBoxspeed2.AppendText("第" + i.ToString() + "场：\r\n");
                }));
                simulate_Game(Array1[i - 1], log as Log);
                if (i == seasongame)
                {
                    refreshdata(l, career);
                }
                progressBarseason.BeginInvoke(new Action(() =>
                {
                    progressBarseason.Value++;
                }));
                pause();
                if (endseason)
                {
                    break;
                }
                Thread.Sleep(2 * waitdelta);
                pause();
            }
            endseason = false;
        }

        public void simulate_Career()
        {
            //生涯模拟
            Log log = new Log();
            dc = new DataConsole(r, l);
            textBoxcareer.BeginInvoke(new Action(() =>
            {
                textBoxcareer.Text = "";
            }));
            textBoxdata.BeginInvoke(new Action(() =>
            {
                textBoxdata.Text = "";
            }));
            seasondata = new string[careerseason + 1];
            seasondata2 = new string[careerseason + 1];
            sdindex = 0;
            addinfomation("生涯共" + careerseason.ToString() + "个赛季。\r\n", log, savelog);
            for (int i = 1; i < careerseason+1; i++)
            {
                pause();
                addinfomation("第" + i.ToString() + "赛季：\r\n", log, savelog);
                textBoxspeed.BeginInvoke(new Action(() =>
                {
                    textBoxspeed.AppendText("第" + i.ToString() + "赛季：\r\n");
                }));
                textBoxspeed2.BeginInvoke(new Action(() =>
                {
                    textBoxspeed2.AppendText("第" + i.ToString() + "赛季：\r\n");
                }));
                simulate_Season(log);
                dc.SetRank(r);
                dc.SetLB(l);
                l.Clear(career);
                seasons = i;
                refreshcareer(careername);
                if (i <= 4)
                {
                    AddCap(careername);
                }
                CapChange(r);
                Carchange(r);
                addinfomation("休赛期：\r\n", log, savelog);
                if (i % carchangedelta == 0)
                {
                    RuleChange();
                    addinfomation("        [重要信息]下赛季迎来规则大改，各车队赛车实力下降。\r\n", log, savelog);
                }
                pause();
                randomTrans(log);
                if (savelog)
                {
                    log.Save("第" + i.ToString() + "赛季车手排行榜：\r\n" + seasondata[sdindex - 1]);
                    log.Save("第" + i.ToString() + "赛季车队排行榜：\r\n" + seasondata2[sdindex - 1]);
                }
                pause();
                Thread.Sleep(2 * waitdelta);
                textBoxspeed.BeginInvoke(new Action(() =>
                {
                    textBoxspeed.Text = "";
                }));
                textBoxspeed2.BeginInvoke(new Action(() =>
                {
                    textBoxspeed2.Text = "";
                }));
                if (endcareer)
                {
                    break;
                }
                pause();
            }
            l.Sortcareer();
            refreshdata(l, true, true);
            if (savelog)
            {
                log.Save("生涯车手排行榜：\r\n" + seasondata[seasondata.Length - 1]);
                log.Save("生涯车队排行榜：\r\n" + seasondata2[seasondata.Length - 1]);
            }
            addinfomation("===========生涯结束============\r\n", log, savelog);
            buttoncareer.BeginInvoke(new Action(() =>
            {
                buttoncareer.Text = "生涯模拟";
            }));
            生涯模拟ToolStripMenuItem.Text = "生涯模拟";
            endcareer = false;
        }

        public void CareerClear()
        {
            //生涯模式清理
            comboBoxseason.BeginInvoke(new Action(() => 
            {
                comboBoxseason.Items.Clear(); 
            }));
            ReadCap();
            ReadTeam();
        }

        public void CapChange(Rank r)
        {
            //能力随机变化,但不能超过99
            int capdelta = 3;
            if (capset == 0)
            {
                capdelta = 4;
            }
            else if (capset == 2)
            {
                capdelta = 2;
            }
            Random rr = new Random(Rank.Getseed());
            for (int i = 0; i < 20; i++)
            {
                r.cars[i].cap += rr.Next(0, capdelta) - capdelta/2;
                r.cars[i].raincap += rr.Next(0, capdelta) - capdelta / 2;
                r.cars[i].quancap += rr.Next(0, capdelta) - capdelta / 2;
                r.cars[i].savecap += rr.Next(0, capdelta) - capdelta / 2;
                if (r.cars[i].cap > 99)
                {
                    r.cars[i].cap = 99;
                }
                if (r.cars[i].quancap > 99)
                {
                    r.cars[i].quancap = 99;
                }
                if (r.cars[i].raincap > 99)
                {
                    r.cars[i].raincap = 99;
                }
                if (r.cars[i].savecap > 99)
                {
                    r.cars[i].savecap = 99;
                }
            }
        }

        public void Carchange(Rank r)
        {
            //车辆改进
            int[] cars = new int[20];
            for (int i = 0; i < 20; i++)
            {
                cars[i] = 0;
            }
            for (int i = 0; i < 10; i++)
            {
                int index1 = 0;
                int index2 = 0;
                for (int j = 0; j < 20; j++)
                {
                    if(cars[j] == 0)
                    {
                        index1 = j;
                        break;
                    }
                }
                for (int j = index1+1; j < 20; j++)
                {
                    if (r.cars[index1].team.name == r.cars[j].team.name)
                    {
                        index2 = j;
                        cars[j] = 1;
                        cars[index1] = 1;
                        break;
                    }
                }
                Carchangecap(index1, index2);
            }
        }

        public void Carchangecap(int index1, int index2)
        {
            //改变车的属性
            Random rr = new Random(Rank.Getseed());
            int cardelta = 12;
            if (carset == 0)
            {
                cardelta = 18;
            }
            else if (carset == 2)
            {
                cardelta = 6;
            }
            r.cars[index1].team.Cquan += rr.Next(0, cardelta);
            r.cars[index1].team.Csin += rr.Next(0, cardelta);
            r.cars[index1].team.Crain += rr.Next(0, cardelta);
            r.cars[index1].team.Csave += rr.Next(0, cardelta);
            if (r.cars[index1].team.Cquan > 99)
            {
                r.cars[index1].team.Cquan = 99;
            }
            if (r.cars[index1].team.Csin > 99)
            {
                r.cars[index1].team.Csin = 99;
            }
            if (r.cars[index1].team.Crain > 99)
            {
                r.cars[index1].team.Crain = 99;
            }
            if (r.cars[index1].team.Csave > 99)
            {
                r.cars[index1].team.Csave = 99;
            }
            r.cars[index2].team.Cquan = r.cars[index1].team.Cquan;
            r.cars[index2].team.Csin = r.cars[index1].team.Csin;
            r.cars[index2].team.Crain = r.cars[index1].team.Crain;
            r.cars[index2].team.Csave = r.cars[index1].team.Csave;
        }

        public void RuleChange()
        {
            //规则大改，车辆实力减少
            double rulechange = 0.81;
            if (carchange == 0)
            {
                rulechange = 0.72;
            }
            else if (carchange == 2)
            {
                rulechange = 0.9;
            }
            int[] cars = new int[20];
            for (int i = 0; i < 20; i++)
            {
                cars[i] = 0;
            }
            for (int i = 0; i < 10; i++)
            {
                int index = 0;
                for (int j = 0; j < 20; j++)
                {
                    if (cars[j] == 0)
                    {
                        index = j;
                        r.cars[j].team.Cquan = (int)(r.cars[j].team.Cquan * rulechange);
                        r.cars[j].team.Csin = (int)(r.cars[j].team.Csin * rulechange);
                        r.cars[j].team.Crain = (int)(r.cars[j].team.Crain * rulechange);
                        r.cars[j].team.Csave = (int)(r.cars[j].team.Csave * rulechange);
                        break;
                    }
                }
                for (int j = index + 1; j < 20; j++)
                {
                    if (r.cars[index].team.name == r.cars[j].team.name)
                    {
                        if (r.cars[index].team.name== "Haas F1")
                        {
                            if (r.cars[j].team.Cquan < r.cars[index].team.Cquan) 
                            {
                                r.cars[index].team = r.cars[j].team;
                            }
                            else
                            {
                                r.cars[j].team = r.cars[index].team;
                            }
                        }
                        cars[j] = 1;
                        cars[index] = 1;
                        break;
                    }
                }
            }
        }

        public void AddCap(string name)
        {
            //提高能力
            int index = 0;
            for (int i = 0; i < 20; i++)
            {
                if (r.cars[i].name == name)
                {
                    index = i;
                }
            }
            if (r.cars[index].cap < potential)
            {
                int k = (int)((potential - r.cars[index].cap) / 3.0);
                if (k < 1)
                {
                    k = 1;
                }
                r.cars[index].cap += k;
            }
            if (r.cars[index].raincap < potential)
            {
                int k = (int)((potential - r.cars[index].raincap) / 3.0);
                if (k < 1)
                {
                    k = 1;
                }
                r.cars[index].raincap += k;
            }
            if (r.cars[index].quancap < potential)
            {
                int k = (int)((potential - r.cars[index].quancap) / 3.0);
                if (k < 1)
                {
                    k = 1;
                }
                r.cars[index].quancap += k;
            }
            if (r.cars[index].savecap < potential)
            {
                int k = (int)((potential - r.cars[index].savecap) / 3.0);
                if (k < 1)
                {
                    k = 1;
                }
                r.cars[index].savecap += k;
            }
        }

        public void addinfomation(string str, Log log, bool savelog=false)
        {
            //添加信息
            textBoxinfomation.BeginInvoke(new Action(() =>
            {
                textBoxinfomation.AppendText(str);
                textBoxinfomation.ScrollToCaret();
            }));
            if (savelog)
            {
                log.Save(str);
            }
        }

        public void addinfo(Rank r, bool isquan, bool israin, Log log)
        {
            //添加比赛信息
            string raininfo = "";
            if (israin)
            {
                raininfo = "(雨战)";
            }
            if (!isquan)
            {
                
                int i = r.Getfast();
                int j = r.Getname(careername);
                addinfomation("        [正赛信息] " + raininfo + "冠军为" + r.cars[0].name + "，最快圈为" + r.cars[i].name + "创造的" + r.cars[i].fastlap.ToString("f3") +
                    "。" + r.cars[j].name + "为第" + (j + 1).ToString() + "名。\r\n", log, savelog);
            }
            else
            {
                int i = r.Getfast();
                int j = r.Getname(careername);
                addinfomation("        [排位信息] " + raininfo + "杆位为" + r.cars[i].name + "创造的" + r.cars[i].fastlap.ToString("f3") + "。" + r.cars[j].name + "为第" + (j + 1).ToString() + "名。\r\n", log, savelog);
            }
        }

        public void addmc(string mc, int lap)
        {
            //显示解说
            if (mc != "")
            {
                textBoxmc.BeginInvoke(new Action(() =>
                {
                    if (lap == 0)
                    {
                        textBoxmc.AppendText("\r\n[排位赛]" + mc);
                    }
                    else
                    {
                        textBoxmc.AppendText("\r\n[第" + lap.ToString() + "圈]" + mc);
                    }
                    textBoxmc.ScrollToCaret();
                }));
            }
        }

        public void refreshcareer(string name)
        {
            //显示生涯数据
            int index = 0;
            for(int i = 0; i < 20; i++)
            {
                if (l.lb[i].name == name)
                {
                    index = i;
                }
            }
            string str = "名字：" + l.lb[index].name + " 积分：" + l.lb[index].cpoint.ToString() + " 世界冠军：" + l.lb[index].wdc.ToString() +
                "\r\n冠军：" + l.lb[index].cwins.ToString() + " 领奖台：" + l.lb[index].cpodium.ToString() + " 杆位：" + l.lb[index].cpole.ToString() + " 最快圈：" + l.lb[index].cfastlap.ToString() +
                "\r\n赛季数：" + seasons.ToString() + " 总场次：" + l.lb[index].cgames.ToString() + " 退赛： " + l.lb[index].cdnf.ToString();
            textBoxcareer.BeginInvoke(new Action(() =>
            {
                textBoxcareer.Text = str;
            }));
        }

        public void refreshrank(Circuits cir, int quan, Rank r, bool isquan, bool israin, int q)
        {
            //刷新排名
            string str = cir.name + "大奖赛，";
            if (israin)
            {
                str += "雨战-";
            }
            if (isquan)
            {
                str += "排位赛Q" + q.ToString() + "：\r\n排名 车手  圈速      差距      头车\r\n";
            }
            else
            {
                str += "第" + quan.ToString() + "圈：\r\n排名 车手 最快圈    差距      头车  停站  轮胎 汽油 位置 \r\n";
            }
            for (int i = 0; i < 20; i++)
            {
                if (isquan)
                {
                    if (i == 10 || i == 15) 
                    {
                        str += "——————————————————————\r\n";
                    }
                }
                if (!r.cars[i].isdnf)
                {
                    str += (i + 1).ToString().PadRight(5) + r.cars[i].name.PadRight(5) + r.cars[i].fastlap.ToString("f3").PadRight(9) +r.cars[i].delta.PadRight(8) + r.cars[i].delta2.PadRight(9);
                    if (!isquan)
                    {
                        str += r.cars[i].pittime.ToString("f1").PadRight(7) + r.cars[i].tyres.type.ToString().PadRight(3) + r.cars[i].allpetrol.ToString("f1").PadLeft(4) + "% " + r.cars[i].change.PadLeft(3);
                    }
                    str += "\r\n";
                }
                else
                {
                    if (isquan)
                    {
                        str += (i + 1).ToString().PadRight(5) + r.cars[i].name + "         NO       TIME          \r\n";
                    }
                    else
                    {
                        str += (i + 1).ToString().PadRight(5) + r.cars[i].name + "             Do  Not  Finish          \r\n";
                    }
                }
            }
            textBoxrank.BeginInvoke(new Action(() =>
            {
                textBoxrank.Text = str;
            }));
        }

        public void refreshspeed(Rank r, string name, bool isspeed1)
        {
            //输出每一圈圈速
            int index = 0;
            for (int i = 0; i < 20; i++)
            {
                if (r.cars[i].name == name)
                {
                    index = i;
                }
            }
            string str = r.cars[index].lastlap.ToString("f3");
            if (isspeed1)
            {
                textBoxspeed.BeginInvoke(new Action(() =>
                {
                    textBoxspeed.AppendText(str + "\r\n");
                }));
            }
            else
            {
                textBoxspeed2.BeginInvoke(new Action(() =>
                {
                    textBoxspeed2.AppendText(str + "\r\n");
                }));
            }
        }

        public void refreshdata(Leaderboard l, bool save = false, bool career = false)
        {
            //刷新数据文本框
            string str = "排名 车队 车手 冠军 领奖台 最快圈 杆位 退赛 起步  完赛  积分\r\n";
            string str2 = "排名 车队 冠军 领奖台 最快圈 杆位 退赛 积分\r\n";
            if (career)
            {
                str = "排名 车手 冠军 领奖台 最快圈 杆位 退赛 起步  完赛  积分 世界冠军\r\n";
                str2 = "排名 车队 冠军 领奖台 最快圈 杆位 退赛 积分 世界冠军\r\n";
            }
            l.Sort();
            for (int i = 0; i < 20; i++)
            {
                //车手字符串
                if (!career)
                {
                    double quanr = (double)l.lb[i].quanrank / (double)l.lb[i].games;
                    double endr = (double)l.lb[i].endrank / (double)l.lb[i].games;
                    str += (i + 1).ToString().PadRight(3) + l.lb[i].team.name.PadRight(8) + l.lb[i].name.PadRight(6) + l.lb[i].wins.ToString().PadRight(6) + l.lb[i].podium.ToString().PadRight(7) +
                        l.lb[i].fastlap.ToString().PadRight(5) + l.lb[i].pole.ToString().PadRight(5) + l.lb[i].dnf.ToString().PadRight(5) + quanr.ToString("f1").PadRight(6) +
                        endr.ToString("f1").PadRight(6) + l.lb[i].point.ToString() + "\r\n";
                }
                else
                {
                    double quanr = (double)l.lb[i].cquanrank / (double)l.lb[i].cgames;
                    double endr = (double)l.lb[i].cendrank / (double)l.lb[i].cgames;
                    str += (i + 1).ToString().PadRight(5)  + l.lb[i].name.PadRight(6) + l.lb[i].cwins.ToString().PadRight(6) + l.lb[i].cpodium.ToString().PadRight(7) +
                        l.lb[i].cfastlap.ToString().PadRight(5) + l.lb[i].cpole.ToString().PadRight(5) + l.lb[i].cdnf.ToString().PadRight(5) + quanr.ToString("f1").PadRight(6) +
                        endr.ToString("f1").PadRight(6) + l.lb[i].cpoint.ToString().PadRight(9) +l.lb[i].wdc.ToString() +"\r\n";
                }
            }
            l.Sortteam();
            for (int i = 0; i < 10; i++)
            {
                //车队字符串
                if (!career)
                {
                    str2 += (i + 1).ToString().PadRight(3) + l.lbt[i].team.name.PadRight(9) + l.lbt[i].wins.ToString().PadRight(6) + l.lbt[i].podium.ToString().PadRight(7) + l.lbt[i].fastlap.ToString().PadRight(5)
                        + l.lbt[i].pole.ToString().PadRight(5) + l.lbt[i].dnf.ToString().PadRight(5) + l.lbt[i].point.ToString() + "\r\n";
                }
                else
                {
                    str2 += (i + 1).ToString().PadRight(3) + l.lbt[i].team.name.PadRight(9) + l.lbt[i].cwins.ToString().PadRight(6) + l.lbt[i].cpodium.ToString().PadRight(7) + l.lbt[i].cfastlap.ToString().PadRight(5) +
                        l.lbt[i].cpole.ToString().PadRight(5) + l.lbt[i].cdnf.ToString().PadRight(5) + l.lbt[i].cpoint.ToString().PadRight(9) + l.lbt[i].wcc.ToString() + "\r\n";
                }
            }
            seasondriver = str;
            seasonteam = str2;
            if (radioButtondriver.Checked)
            {
                textBoxdata.BeginInvoke(new Action(() =>
                {
                    textBoxdata.Text = str;
                }));
            }
            else
            {
                textBoxdata.BeginInvoke(new Action(() =>
                {
                    textBoxdata.Text = str2;
                }));
            }
            if (save)
            {
                if (notifyIcon.Visible == true)
                {
                    if (!career)
                    {
                        notifyIcon.ShowBalloonTip(3000, "第" + (sdindex + 1).ToString() + "赛季结束。", l.lb[0].name + "获得了世界冠军。", ToolTipIcon.Info);
                    }
                    else
                    {
                        int wdci = 0;
                        for (int i = 0; i < 20; i++)
                        {
                            if (l.lb[wdci].wdc < l.lb[i].wdc)
                            {
                                wdci = i;
                            }
                        }
                        notifyIcon.ShowBalloonTip(3000, "生涯模拟结束。", l.lb[wdci].name + "获得了最多的世界冠军，共" + l.lb[wdci].wdc.ToString() + "个。", ToolTipIcon.Info);
                    }
                }
                seasondata[sdindex] = str;
                seasondata2[sdindex] = str2;
                comboBoxseason.BeginInvoke(new Action(() =>
                {
                    if (!career)
                    {
                        comboBoxseason.Items.Add("第" + (sdindex + 1).ToString() + "赛季");
                    }
                    else
                    {
                        comboBoxseason.Items.Add("生涯数据");
                    }
                    sdindex++;
                }));
            }
        }

        private void buttonsimu11_Click(object sender, EventArgs e)
        {
            //赛季模拟进程
            if (career)
            {
                MessageBox.Show("生涯模拟正在进行中！");
            }
            else
            {
                if (buttonsimu11.Text == "结束模拟") 
                {
                    EndSeason();
                }
                else
                {
                    CareerClear();
                    Log log = new Log();
                    games = 0;
                    career = false;
                    l.Clear(false);
                    ParameterizedThreadStart ts = new ParameterizedThreadStart(simulate_Season);
                    Thread tt = new Thread(ts);
                    tt.Start(log);
                    buttonsimu11.Text = "结束模拟";
                }
            }
        }

        private void buttoncareer_Click(object sender, EventArgs e)
        {
            //生涯模拟进程
            if (buttoncareer.Text == "结束生涯")
            {
                EndCareer();
            }
            else
            {
                StartCareer();
            }
        }

        public void StartCareer()
        {
            //开始生涯模拟
            CareerClear();
            games = 0;
            career = true;
            l.Clear(false);
            ThreadStart ts = new ThreadStart(simulate_Career);
            Thread tt = new Thread(ts);
            tt.Start();
            buttoncareer.Text = "结束生涯";
            生涯模拟ToolStripMenuItem.Text = "结束生涯模拟";
        }

        public void EndSeason()
        {
            //结束赛季模拟
            endseason = true;
            buttonsimu11.Text = "赛季模拟";
        }

        public void EndCareer()
        {
            //结束生涯模拟
            endcareer = true;
            career = false;
            if (ShowInTaskbar == false)
            {
                notifyIcon.ShowBalloonTip(3000, "结束生涯模拟", "将在赛季结束后结束生涯模拟。", ToolTipIcon.Info);
            }
            else
            {
                MessageBox.Show("将在赛季结束后结束生涯模拟。", "结束生涯模拟");
            }
        }

        public void randomTrans(Log log)
        {
            //随机转会
            Random rr = new Random(Rank.Getseed());
            int num = rr.Next(1, 4);
            if (transset == 0)
            {
                num = rr.Next(2, 6);
            }
            else if (transset == 2)
            {
                num = rr.Next(0, 2);
            }
            for (int i = 0; i < num; i++)
            {
                Thread.Sleep(waitdelta);
                int num1 = rr.Next(0, 20);
                int num2 = rr.Next(0, 20);
                if(r.cars[num1].team.name== r.cars[num2].team.name)
                {
                    i--;
                }
                else
                {
                    string name1 = r.cars[num1].name;
                    string name2 = r.cars[num2].name;
                    driverTrans(name1, name2, log);
                }
            }
        }

        public void driverTrans(string name1,string name2, Log log)
        {
            //两人交换车队
            Team tm1 = r.Getteam(name1);
            Team tm2 = r.Getteam(name2);
            r.Setteam(name1, tm2);
            r.Setteam(name2, tm1);
            l.Init(name1, tm2.name);
            l.Init(name2, tm1.name);
            addinfomation("        [转会消息]" + name1 + "加盟了"+ tm2.name + "车队，而" + name2 + "与其互换车队，加入"+ tm1.name + "车队效力。\r\n", log, savelog);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("是否退出？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                notifyIcon.Dispose();
                Process.GetCurrentProcess().Kill();
                Application.Exit();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void buttoncheck_Click(object sender, EventArgs e)
        {
            //选择查看谁的圈速
            if (isspeed1)
            {
                name = comboBox.SelectedItem.ToString();
                textBoxspeed.AppendText("==" + name + "==\r\n");
                labelspeed.Text = name;
                isspeed1 = !isspeed1;
            }
            else
            {
                name2 = comboBox.SelectedItem.ToString();
                textBoxspeed2.AppendText("==" + name2 + "==\r\n");
                labelspeed2.Text = name2;
                isspeed1 = !isspeed1;
            }
        }

        private void buttonpause_Click(object sender, EventArgs e)
        {
            //暂停按钮
            if (ispause)
            {
                ispause = false;
                buttonpause.Text = "暂停模拟";
                暂停模拟ToolStripMenuItem.Text = "暂停模拟";
            }
            else
            {
                ispause = true;
                buttonpause.Text = "继续模拟";
                暂停模拟ToolStripMenuItem.Text = "继续模拟";
            }
        }

        public void pause()
        {
            //用于暂停进程
            while (ispause)
            {
                Thread.Sleep(300);
            }
        }

        private void buttonloadseason_Click(object sender, EventArgs e)
        {
            //查看过去赛季的数据
            int i = comboBoxseason.SelectedIndex;
            if (radioButtondriver.Checked)
            {
                textBoxdata.Text = seasondata[i];
            }
            else
            {
                textBoxdata.Text = seasondata2[i];
            }
        }

        private void buttonreturnnow_Click(object sender, EventArgs e)
        {
            //查看当前赛季数据
            refreshdata(l);
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            //速度调节
            int k = trackBar.Value + 1;
            waitdelta = k * k * 40;
            最快速度ToolStripMenuItem.Checked = false;
            默认速度ToolStripMenuItem.Checked = false;
            最慢速度ToolStripMenuItem.Checked = false;
        }

        private void buttoncheckcareer_Click(object sender, EventArgs e)
        {
            //查看生涯数据
            if (comboBoxcareer.SelectedItem != null)
            {
                refreshcareer(comboBoxcareer.SelectedItem.ToString());
            }
        }

        private void radioButtondriver_CheckedChanged(object sender, EventArgs e)
        {
            //切换排行数据
            if (radioButtondriver.Checked)
            {
                textBoxdata.Text = seasondriver;
            }
            else
            {
                textBoxdata.Text = seasonteam;
            }
        }

        private void buttonsetting_Click(object sender, EventArgs e)
        {
            //修改游戏设定
            CareerSetting cs = new CareerSetting();
            if (cs.ShowDialog() == DialogResult.OK)
            {
                Capability cp = cs.cp;
                careername = cp.name;
                string[] configs = File.ReadAllLines("./team.txt");
                Team tm = new Team(configs[9]);
                comboBox.BeginInvoke(new Action(() =>
                {
                    comboBox.Items.Remove("LLZ");
                    comboBox.Items.Add(careername);
                }));
                comboBoxcareer.BeginInvoke(new Action(() =>
                {
                    comboBoxcareer.Items.Remove("LLZ");
                    comboBoxcareer.Items.Add(careername);
                }));
                labelspeed.BeginInvoke(new Action(() =>
                {
                    labelspeed.Text = careername;
                }));
                r.Init("LLZ", cp, tm);
                l.Init("LLZ", cp);
                l.Init(careername, "Haas F1");
                careerseason = cs.career;
                potential = cs.potential;
                capset = cs.cap;
                carset = cs.car;
                transset = cs.trans;
                savelog = cs.savelog;
            }
        }

        private void buttondataconsole_Click(object sender, EventArgs e)
        {
            //显示数据台
            if (career)
            {
                dc.Show();
            }
            else
            {
                MessageBox.Show("非生涯模式无法使用数据台。");
            }
        }

        private void SimuForm_Load(object sender, EventArgs e)
        {
            //窗口初始化后
            ReadCap();
            ReadTeam();
            ReadCircuits();
        }

        private void SimuForm_SizeChanged(object sender, EventArgs e)
        {
            //判断是否最小化到托盘
            if (WindowState == FormWindowState.Minimized)
            {
                //隐藏任务栏区图标
                ShowInTaskbar = false;
                显示主窗口ToolStripMenuItem.Text = "显示主窗口";
                notifyIcon.ShowBalloonTip(3000, "已最小化到托盘。", "双击或右键可显示主界面。", ToolTipIcon.Info);
            }
            else if(WindowState == FormWindowState.Normal)
            {
                显示主窗口ToolStripMenuItem.Text = "隐藏主窗口";
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //双击显示主界面
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
        }

        private void notifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            //点击气泡显示主界面
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
        }

        //以下为托盘右键菜单
        private void 显示主窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                显示主窗口ToolStripMenuItem.Text = "显示主窗口";
                WindowState = FormWindowState.Minimized;
                ShowInTaskbar = false;
            }
            else
            {
                显示主窗口ToolStripMenuItem.Text = "隐藏主窗口";
                WindowState = FormWindowState.Normal;
                ShowInTaskbar = true; 
            }
        }

        private void 显示数据中心ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (career)
            {
                dc.Show();
            }
            else
            {
                notifyIcon.ShowBalloonTip(3000, "非生涯模式无法使用数据台。", "右键可开始生涯模式。", ToolTipIcon.Warning);
            }
        }

        private void 退出软件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void 生涯模拟ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (buttoncareer.Text == "结束生涯")
            {
                EndCareer();
            }
            else
            {
                StartCareer();
            }
        }

        private void 暂停模拟ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ispause)
            {
                ispause = false;
                buttonpause.Text = "暂停模拟";
                暂停模拟ToolStripMenuItem.Text = "暂停模拟";
            }
            else
            {
                ispause = true;
                buttonpause.Text = "继续模拟";
                暂停模拟ToolStripMenuItem.Text = "继续模拟";
            }
        }

        private void 游戏设定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CareerSetting cs = new CareerSetting();
            if (cs.ShowDialog() == DialogResult.OK)
            {
                Capability cp = cs.cp;
                careername = cp.name;
                string[] configs = File.ReadAllLines("./team.txt");
                Team tm = new Team(configs[9]);
                r.Init("LLZ", cp, tm);
                l.Init("LLZ", cp);
                l.Init(careername, "Haas F1");
                careerseason = cs.career;
                potential = cs.potential;
                capset = cs.cap;
                carset = cs.car;
                transset = cs.trans;
                savelog = cs.savelog;
            }
        }

        private void 最快速度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            最慢速度ToolStripMenuItem.Checked = false;
            默认速度ToolStripMenuItem.Checked = false;
            waitdelta = 40;
            trackBar.Value = 0;
        }

        private void 默认速度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            最快速度ToolStripMenuItem.Checked = false;
            最慢速度ToolStripMenuItem.Checked = false;
            waitdelta = 1000;
            trackBar.Value = 5;
        }

        private void 最慢速度ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            最快速度ToolStripMenuItem.Checked = false;
            默认速度ToolStripMenuItem.Checked = false;
            waitdelta = 4000;
            trackBar.Value = 10;
        }
    }
}
