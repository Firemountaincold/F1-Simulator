namespace F1_Simulator
{
    public class Data
    {
        public string name;
        public Team team;
        //赛季数据
        public int wins = 0;
        public int podium = 0;
        public int point = 0;
        public int pole = 0;
        public int fastlap = 0;
        public int dnf = 0;
        public int games = 0;
        public int quanrank = 0;
        public int endrank = 0;
        //生涯数据
        public int cwins = 0;
        public int cpodium = 0;
        public int cpoint = 0;
        public int cpole = 0;
        public int cfastlap = 0;
        public int cdnf = 0;
        public int wdc = 0;
        public int cgames = 0;
        public int cquanrank = 0;
        public int cendrank = 0;

        public Data(string s)
        {
            name = s;
        }
    }

    public class Tdata
    {
        public Team team;
        public string name1="";
        public string name2 = "";
        //赛季数据
        public int wins = 0;
        public int podium = 0;
        public int point = 0;
        public int pole = 0;
        public int fastlap = 0;
        public int dnf = 0;
        //生涯数据
        public int cwins = 0;
        public int cpodium = 0;
        public int cpoint = 0;
        public int cpole = 0;
        public int cfastlap = 0;
        public int cdnf = 0;
        public int wcc = 0;

        public Tdata(Team s)
        {
            team = s;
        }
    }

    public class Leaderboard
    {
        public Data[] lb = new Data[20];
        public Tdata[] lbt = new Tdata[10];

        public void Init(Capability[] cp)
        {
            //初始化
            for (int i = 0; i < 20; i++)
            {
                lb[i] = new Data(cp[i].name);
            }
        }

        public void Init(string name, Capability cp)
        {
            //初始化
            int i = 0;
            for (int j = 0; j < 20; j++)
            {
                if (lb[j].name == name)
                {
                    i = j;
                }
            }
            lb[i] = new Data(cp.name);
        }

        public void Init(string name, string team)
        {
            //单独初始化
            int i = 0;
            int k = 0;
            for (int j = 0; j < 20; j++)
            {
                if (lb[j].name == name)
                {
                    i = j;
                }
            }
            for (int j = 0; j < 10; j++)
            {
                if (lbt[j].team.name == team)
                {
                    k = j;
                }
            }
            lb[i].team = lbt[k].team;
            Update();
        }

        public void init(Team[] tm)
        {
            //初始化车队
            for (int i = 0; i < 20; i++)
            {
                lb[i].team = tm[i / 2];
                lbt[i / 2] = new Tdata(tm[i / 2]);
            }
            Update();
        }

        public void Update()
        {
            //更新车队
            for (int j = 0; j < 10; j++)
            {
                lbt[j].name1 = "";
                lbt[j].name2 = "";
            }
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (lb[i].team == lbt[j].team)
                    {
                        if (lbt[j].name1 == "")
                        {
                            lbt[j].name1 = lb[i].name;
                        }
                        else
                        {
                            lbt[j].name2 = lb[i].name;
                        }
                    }
                }
            }
        }
        public void GetQuanData(Rank r)
        {
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if (r.cars[i].name == lb[j].name)
                    {
                        lb[j].quanrank += i + 1;
                    }
                }
            }
        }

        public void Getdata(Rank r)
        {
            //添加比赛数据
            int fastindex = 0;
            int fl = 0;
            int fl2 = 0;
            for (int i = 0; i < 20; i++)
            {
                if (r.cars[i].fastlap <= r.cars[fastindex].fastlap)
                {
                    fastindex = i;
                }
                for (int j = 0; j < 20; j++)
                {
                    if (r.cars[i].name == lb[j].name)
                    {
                        lb[j].games++;
                        lb[j].endrank += i + 1;
                        if (!r.cars[i].isdnf)
                        {
                            lb[j].point += Addp(i + 1);
                        }
                        if (i == 0)
                        {
                            lb[j].wins++;
                        }
                        if (i >= 0 && i <= 2)
                        {
                            lb[j].podium++;
                        }
                        if (fastindex == i)
                        {
                            fl = j;
                        }
                        if (r.Getfast() == i)
                        {
                            lb[j].fastlap++;
                        }
                        if (r.cars[i].isdnf)
                        {
                            lb[j].dnf++;
                        }
                    }
                }
                for (int j = 0; j < 10; j++)
                {
                    if (r.cars[i].name == lbt[j].name1 || r.cars[i].name == lbt[j].name2)
                    {
                        if (!r.cars[i].isdnf)
                        {
                            lbt[j].point += Addp(i + 1);
                        }
                        if (i == 0)
                        {
                            lbt[j].wins++;
                        }
                        if (i >= 0 && i <= 2)
                        {
                            lbt[j].podium++;
                        }
                        if (fastindex == i)
                        {
                            fl2 = j;
                        }
                        if (r.Getfast() == i)
                        {
                            lbt[j].fastlap++;
                        }
                        if (r.cars[i].isdnf)
                        {
                            lbt[j].dnf++;
                        }
                    }
                }
            }
            if (!r.cars[fastindex].isdnf && fastindex < 10) 
            {
                //前十且不退赛才能获得最快圈1分
                lb[fl].point++;
                lbt[fl2].point++;
            }
        }

        public void Getpole(Rank r)
        {
            //添加杆位数据
            for (int j = 0; j < 20; j++)
            {
                if (r.cars[0].name == lb[j].name)
                {
                    lb[j].pole++;
                }
            }
            for (int j = 0; j < 10; j++)
            {
                if (r.cars[0].name == lbt[j].name1|| r.cars[0].name == lbt[j].name2)
                {
                    lbt[j].pole++;
                }
            }
        }

        public void Sort()
        {
            //按分数排序
            for (int i = 0; i < 20; i++)
            {
                for (int j = i + 1; j < 20; j++)
                {
                    if (lb[i].point < lb[j].point)
                    {
                        Data temp = lb[i];
                        lb[i] = lb[j];
                        lb[j] = temp;
                    }
                    else if (lb[i].point == lb[j].point)
                    {
                        if (lb[i].wins < lb[j].wins)
                        {
                            Data temp = lb[i];
                            lb[i] = lb[j];
                            lb[j] = temp;
                        }
                        else
                        {
                            if (lb[i].podium < lb[j].podium)
                            {
                                Data temp = lb[i];
                                lb[i] = lb[j];
                                lb[j] = temp;
                            }
                            else
                            {
                                if(lb[i].endrank > lb[j].endrank)
                                {
                                    Data temp = lb[i];
                                    lb[i] = lb[j];
                                    lb[j] = temp;
                                }
                            }
                        }
                    }
                }
            }
        }

        public void Sortteam()
        {
            //车队排序
            for (int i = 0; i < 10; i++)
            {
                for (int j = i + 1; j < 10; j++)
                {
                    if (lbt[i].point < lbt[j].point)
                    {
                        Tdata temp = lbt[i];
                        lbt[i] = lbt[j];
                        lbt[j] = temp;
                    }
                    else if (lbt[i].point == lbt[j].point)
                    {
                        if (lbt[i].wins < lbt[j].wins)
                        {
                            Tdata temp = lbt[i];
                            lbt[i] = lbt[j];
                            lbt[j] = temp;
                        }
                        else
                        {
                            if (lbt[i].podium < lbt[j].podium)
                            {
                                Tdata temp = lbt[i];
                                lbt[i] = lbt[j];
                                lbt[j] = temp;
                            }
                        }
                    }
                }
            }
        }

        public void Sortcareer()
        {
            //生涯排序
            for (int i = 0; i < 20; i++)
            {
                for (int j = i + 1; j < 20; j++)
                {
                    if (lb[i].cpoint < lb[j].cpoint)
                    {
                        Data temp = lb[i];
                        lb[i] = lb[j];
                        lb[j] = temp;
                    }
                }
            }
            for (int i = 0; i < 10; i++)
            {
                for (int j = i + 1; j < 10; j++)
                {
                    if (lbt[i].cpoint < lbt[j].cpoint)
                    {
                        Tdata temp = lbt[i];
                        lbt[i] = lbt[j];
                        lbt[j] = temp;
                    }
                }
            }
        }

        public void Clear(bool career)
        {
            //清理赛季数据
            if (career)
            {
                Sort();
                Sortteam();
                lb[0].wdc++;
                lbt[0].wcc++;
            }
            for (int i = 0; i < 20; i++)
            {
                if (career)
                {
                    lb[i].cwins += lb[i].wins;
                    lb[i].cpodium += lb[i].podium;
                    lb[i].cpoint += lb[i].point;
                    lb[i].cpole += lb[i].pole;
                    lb[i].cfastlap += lb[i].fastlap;
                    lb[i].cdnf += lb[i].dnf;
                    lb[i].cgames += lb[i].games;
                    lb[i].cendrank += lb[i].endrank;
                    lb[i].cquanrank += lb[i].quanrank;
                }
                else
                {
                    lb[i].cwins = 0;
                    lb[i].cpodium = 0;
                    lb[i].cpoint = 0;
                    lb[i].cpole = 0;
                    lb[i].cfastlap = 0;
                    lb[i].cdnf = 0;
                    lb[i].wdc = 0;
                    lb[i].cgames = 0;
                    lb[i].cendrank = 0;
                    lb[i].cquanrank = 0;
                }
                lb[i].wins = 0;
                lb[i].podium = 0;
                lb[i].point = 0;
                lb[i].pole = 0;
                lb[i].fastlap = 0;
                lb[i].dnf = 0;
                lb[i].games = 0;
                lb[i].endrank = 0;
                lb[i].quanrank = 0;
            }
            for (int i = 0; i < 10; i++)
            {
                if (career)
                {
                    lbt[i].cwins += lbt[i].wins;
                    lbt[i].cpodium += lbt[i].podium;
                    lbt[i].cpoint += lbt[i].point;
                    lbt[i].cpole += lbt[i].pole;
                    lbt[i].cfastlap += lbt[i].fastlap;
                    lbt[i].cdnf += lbt[i].dnf;
                }
                else
                {
                    lbt[i].cwins = 0;
                    lbt[i].cpodium = 0;
                    lbt[i].cpoint = 0;
                    lbt[i].cpole = 0;
                    lbt[i].cfastlap = 0;
                    lbt[i].cdnf = 0;
                    lbt[i].wcc = 0;
                }
                lbt[i].wins = 0;
                lbt[i].podium = 0;
                lbt[i].point = 0;
                lbt[i].pole = 0;
                lbt[i].fastlap = 0;
                lbt[i].dnf = 0;
            }
        }

        public int Addp(int i)
        {
            //每场分数
            if (i == 1)
            {
                return 25;
            }
            if (i == 2)
            {
                return 18;
            }
            if (i == 3)
            {
                return 15;
            }
            if (i == 4)
            {
                return 12;
            }
            if (i == 5)
            {
                return 10;
            }
            if (i == 6)
            {
                return 8;
            }
            if (i == 7)
            {
                return 6;
            }
            if (i == 8)
            {
                return 4;
            }
            if (i == 9)
            {
                return 2;
            }
            if (i == 10)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
