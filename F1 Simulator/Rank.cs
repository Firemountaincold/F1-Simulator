using System;

namespace F1_Simulator
{
    public class Rank
    {
        public Car[] cars = new Car[20];
        private Random r = new Random(Getseed());
        public double laptime;
        public int overtake = 0;
        public int pitcut = 0;

        public void Init(Capability[] cp)
        {
            //初始化
            for (int i = 0; i < 20; i++)
            {
                cars[i] = new Car(cp[i].name);
                cars[i].quancap = cp[i].Dquan;
                cars[i].cap = cp[i].Dsin;
                cars[i].raincap = cp[i].Drain;
                cars[i].savecap = cp[i].Dsave;
            }
        }

        public void Init(Team[] tm)
        {
            //初始化车队
            for (int i = 0; i < 20; i++)
            {
                cars[i].team = tm[i / 2];
            }
        }

        public void Init(string name, Capability cp, Team tm)
        {
            //单独初始化
            int i = 0;
            for (int j = 0; j < 20; j++)
            {
                if (cars[j].name == name)
                {
                    i = j;
                }
            }
            cars[i] = new Car(cp.name);
            cars[i].quancap = cp.Dquan;
            cars[i].cap = cp.Dsin;
            cars[i].raincap = cp.Drain;
            cars[i].savecap = cp.Dsave;
            cars[i].team = tm;
        }

        public Team Getteam(string name)
        {
            int index = 0;
            for (int i = 0; i < 20; i++)
            {
                if (cars[i].name == name)
                {
                    index = i;
                }
            }
            return cars[index].team;
        }

        public void Setteam(string name, Team tm)
        {
            int index = 0;
            for (int i = 0; i < 20; i++)
            {
                if (cars[i].name == name)
                {
                    index = i;
                }
            }
            cars[index].team = tm;
        }

        public static int Getseed()
        {
            //随机种子
            byte[] bytes = new byte[4];
            System.Security.Cryptography.RNGCryptoServiceProvider rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            rng.GetBytes(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public void Start(Circuits circuit, bool israin, bool isquan)
        {
            //初始化赛道和圈速
            for (int i = 0; i < 20; i++)
            {
                int tc = r.Next(0, 3);
                if (i < 10)
                {
                    tc = 0;
                }
                if (isquan)
                {
                    tc = 0;
                }
                cars[i].Setcircuit(circuit, israin);
                cars[i].Start((Tyre.Color)tc);
            }
            overtake = 0;
            pitcut = 0;
        }

        public void Setpetrol(int laps)
        {
            //设置汽油量
            for (int i = 0; i < 20; i++)
            {
                cars[i].Setpetrol(laps);
            }
        }

        public string Getsingle(bool israin, bool exchange)
        {
            //普通单圈
            string str = "";
            cars[0].Getsingle(israin);
            for (int i = 1; i < 20; i++)
            {
                double l = cars[i].Getsingle(israin);
                int j = i - 1;
                if (l < cars[j].lastlap)
                {
                    if (cars[i].alllap < cars[j].alllap)
                    {
                        if ((!cars[i].inpit) && (!cars[j].inpit) && (!cars[i].isdnf) && (!cars[j].isdnf))
                        {
                            //超车成功
                            int diff = 24;
                            if (l < cars[j].lastlap - 2)
                            {
                                diff = cars[i].circuit.difficulty * 4;
                            }
                            if (r.Next(0, 31) > cars[i].circuit.difficulty + diff)
                            {
                                overtake++;
                                str += cars[i].name + "超过了" + cars[j].name + "。";
                            }
                            else
                            {
                                //超车失败
                                double delta = cars[j].alllap - cars[i].alllap;
                                cars[i].Settime(delta + 0.1 * r.NextDouble());
                            }
                        }
                        else if ((!cars[i].isdnf) && (!cars[j].isdnf))
                        {
                            pitcut++;
                        }
                    }
                    cars[j].Checkpit();
                }
                cars[19].Checkpit();
            }
            Sort(exchange, 20);
            Getdelta(20);
            return str;
        }

        public string GetOverTake()
        {
            //获取超车数据
            return "        [正赛信息]本场比赛共有" + overtake.ToString() + "次赛道超车，和" + pitcut.ToString() + "次维修区超车。\r\n";
        }

        public string Getquansingle(bool israin, int q)
        {
            //排位单圈
            int mem = 20;
            if (q == 2)
            {
                mem = 15;
            }
            if (q == 3)
            {
                mem = 10;
            }
            for (int i = 0; i < mem; i++)
            {
                cars[i].Getquansingle(israin, q);
            }
            Sort(false, mem);
            Getdelta(mem);
            return cars[0].name + "获得了杆位。";
        }

        public void Startdelta()
        {
            //发车差距
            for (int i = 0; i < 20; i++)
            {
                cars[i].alllap += i * 0.3;
            }
        }

        public void Lapfast()
        {
            //上一圈是最快圈吗？
            for (int i = 0; i < 20; i++)
            {
                cars[i].Lapfast();
            }
        }

        public void Sort(bool exchange, int mem)
        {
            //排序
            for (int i = 0; i < mem; i++)
            {
                for (int j = i + 1; j < mem; j++)
                {
                    if (cars[i].alllap > cars[j].alllap)
                    {
                        Car temp = cars[i];
                        cars[i] = cars[j];
                        cars[j] = temp;
                    }
                }
            }
        }

        public void Setindex()
        {
            //保存发车位置
            for (int i = 0; i < 20; i++)
            {
                cars[i].index = i;
            }
        }

        public void Getdelta(int mem)
        {
            //获取差距
            cars[0].delta = "      ";
            cars[0].delta2 = "      ";
            cars[0].Getchange(0);
            for (int i = 1; i < mem; i++)
            {
                double d = cars[i].alllap - cars[i - 1].alllap;
                double e = cars[i].alllap - cars[0].alllap;
                cars[i].delta = "+" + d.ToString("f3");
                cars[i].delta2 = "+" + e.ToString("f3");
                if (e > cars[i].circuit.laptimes + 3)
                {
                    cars[i].delta2 = "+1 lap";
                }
                if (e > cars[i].circuit.laptimes * 2 + 6)
                {
                    cars[i].delta2 = "+2 laps";
                }
                if (e > cars[i].circuit.laptimes * 3 + 9)
                {
                    cars[i].delta2 = "+3 laps";
                }
                cars[i].Getchange(i);
            }
        }

        public void Clear()
        {
            //清理数据
            for (int i = 0; i < 20; i++)
            {
                cars[i].Clear();
            }
        }

        public int Getfast()
        {
            //获取最快圈
            int fastindex = 0;
            for (int i = 0; i < 20; i++)
            {
                if (cars[i].fastlap <= cars[fastindex].fastlap)
                {
                    fastindex = i;
                }
            }
            return fastindex;
        }

        public int Getname(string name)
        {
            //获取车手编号
            int index = 0;
            for (int i = 0; i < 20; i++)
            {
                if (cars[i].name == name)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
    }

    public class Car
    {
        //基本属性
        public string name;
        public int quancap;
        public int cap;
        public int raincap;
        public int savecap;
        public Team team;
        public Circuits circuit;
        //正赛数据
        public double lastlap;
        public double alllap = 0;
        public double fastlap = 999;
        public string delta;
        public string delta2;
        public int laps = 0;
        public int alllaps = 0;
        //停站数据
        public double pittime = 0;
        public int pitlap = 0;
        public bool pited = false;
        public bool inpit = false;
        //排名变化
        public int index;
        public string change;
        //单圈属性
        public double laptime0;
        public double laptime;
        public double decay;
        //轮胎属性
        public int tyrelap = 0;
        public Tyre tyres = new Tyre();
        //汽油属性
        public double allpetrol = 100;
        public double standardpetrol = 0;
        public double lastlappetrol = 0;
        //DNF属性
        public bool isdnf = false;
        //随机种子
        Random rr = new Random(Rank.Getseed());

        public Car(string str)
        {
            this.name = str;
        }

        public double AverCap()
        {
            //返回平均能力
            return (cap + raincap * 0.5 + quancap + savecap * 0.5) / 3.0;
        }

        public void Clear()
        {
            //清理
            lastlap = 0;
            alllap = 0;
            fastlap = 999;
            pittime = 0;
            pited = false;
            tyrelap = 0;
            laps = 0;
            allpetrol = 100;
            isdnf = false;
        }

        public void Setcircuit(Circuits circuit, bool israin)
        {
            //初始化赛道
            this.circuit = circuit;
            laptime = circuit.laptimes + 9;
            if (israin)
            {
                laptime = (int)(1.2 * laptime);
            }
        }

        public void Start(Tyre.Color tc)
        {
            //初始化圈速和轮胎
            laptime0 = laptime;
            this.laptime = laptime - tyres.Getquicker(tc);
            decay = tyres.Getdecay(tc);
            tyres.type = tc;
        }

        public double Getquansingle(bool israin, int q)
        {
            //排位单圈
            alllap = 0;
            double lt = laptime;
            if (q == 2)
            {
                lt = lt - 0.15;
            }
            if (q == 3)
            {
                lt = lt - 0.3;
            }
            if (israin)
            {
                lastlap = lt - lt * (0.8 * raincap + 0.9 * quancap + 1.1 * team.Cquan + 1.2 * team.Crain - 100) / 3750 + 0.4 * rr.NextDouble();
                if (rr.Next(0, 300) < (100 - raincap))
                {
                    isdnf = true;
                    lastlap = 999;
                }
            }
            else
            {
                lastlap = lt - lt * (0.5 * quancap + 1.5 * team.Cquan - 50) / 1875 + 0.2 * rr.NextDouble();
                if (rr.Next(0, 400) < (100 - cap))
                {
                    isdnf = true;
                    lastlap = 999;
                }
            }
            alllap += lastlap;
            tyrelap++;
            return lastlap;
        }

        public double Getsingle(bool israin)
        {
            //单圈
            Usepetrol();
            double petrolratio = lastlappetrol / standardpetrol;
            if (israin)
            {
                if (tyrelap < tyres.Gettyrelaps(tyres.type))
                {
                    laptime += decay * (280 - 0.5 * savecap - 1.5 * team.Csave) * 0.00005;
                }
                else
                {
                    laptime += decay * (280 - 0.5 * savecap - 1.5 * team.Csave) * 0.006;
                }
                lastlap = laptime - laptime * (raincap + team.Crain - allpetrol + 50 * petrolratio - 102) / 1875 + 0.5 * rr.NextDouble();
                if (rr.Next(0, 5000) < (107 - raincap)/2)
                {
                    isdnf = true;
                    lastlap = 999;
                }
            }
            else
            {
                if (tyrelap < tyres.Gettyrelaps(tyres.type))
                {
                    laptime += decay * (320 - 0.5 * savecap - 1.5 * team.Csave) * 0.00005;
                }
                else
                {
                    laptime += decay * (320 - 0.5 * savecap - 1.5 * team.Csave) * 0.006;
                }
                lastlap = laptime - laptime * (0.5 * cap + 1.5 * team.Csin - allpetrol + 50 * petrolratio - 102) / 1875 + 0.25 * rr.NextDouble();
                if (rr.Next(0, 5000) < (107 - cap)/2)
                {
                    isdnf = true;
                    lastlap = 999;
                }
            }
            if (isdnf)
            {
                lastlap = 999;
            }
            alllap += lastlap;
            tyrelap++;
            laps++;
            return lastlap;
        }

        public void Checkpit()
        {
            if (inpit)
            {
                inpit = false;
            }
        }

        public void Settime(double delta)
        {
            //设定时间变化
            alllap += delta;
            lastlap += delta;
        }

        public void Lapfast()
        {
            //上一圈是最快圈吗？
            if (lastlap < fastlap)
            {
                fastlap = lastlap;
            }
        }

        public void Setpetrol(int laps)
        {
            //设定每圈标准耗油
            standardpetrol = allpetrol / (laps + 1);
            alllaps = laps;
        }

        public void Usepetrol()
        {
            //设定此圈耗油，是标准的0.95-1.05倍。
            double laststandard = allpetrol / (alllaps - laps + 1);
            double usedpetrol = rr.NextDouble() * 0.1 * laststandard + 0.95 * laststandard;
            if (usedpetrol > allpetrol)
            {
                usedpetrol = allpetrol;
            }
            lastlappetrol = usedpetrol;
            allpetrol -= usedpetrol;
        }

        public string Pit()
        {
            //停站损失
            string str = name + "进站了，换上了";
            inpit = true;
            Tyre.Color tc = 0;
            if ((int)tyres.type == 0)
            {
                tc = (Tyre.Color)rr.Next(1, 3);
            }
            else if ((int)tyres.type == 1)
            {
                if (rr.Next(0, 2) > 0)
                {
                    tc = (Tyre.Color)2;
                }
                else
                {
                    tc = (Tyre.Color)0;
                }
            }
            else if ((int)tyres.type == 2)
            {
                tc = (Tyre.Color)rr.Next(0, 2);
            }
            if (tc == 0)
            {
                str += "软胎，";
            }
            else if (tc == (Tyre.Color)1)
            {
                str += "中性胎，";
            }
            else
            {
                str += "硬胎，";
            }
            int pt = rr.Next(19, 21);
            if (rr.NextDouble() > 0.85)
            {
                pt += rr.Next(1, 4);
            }
            pittime = (double)pt + rr.NextDouble();
            str += "耗时" + pittime.ToString("f1") + "。";
            alllap += pittime;
            lastlap += pittime;
            pited = true;
            laptime = laptime0 - tyres.Getquicker(tc);
            decay = tyres.Getdecay(tc);
            tyres.type = tc;
            tyrelap = 0;
            return str;
        }

        public void Getchange(int i)
        {
            //计算排名变化
            int c = i - index;
            if (c > 0)
            {
                change = "↓" + c.ToString();
            }
            else if (c < 0)
            {
                change = "↑" + (-c).ToString();
            }
            else
            {
                change = "  ";
            }
        }
    }
}
