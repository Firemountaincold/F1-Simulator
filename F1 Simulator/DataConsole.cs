using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms.DataVisualization.Charting;

namespace F1_Simulator
{
    public partial class DataConsole : Form
    {
        Rank r;
        Leaderboard l;
        private Team[] teams = new Team[10];
        private DCdata[] drivers = new DCdata[20];
        private DCdata[] cars = new DCdata[10];
        public bool isdriver = true;
        public int type = 1;
        public int chartnum = 0;
        public string[] names = new string[4];
        public int nameindex = 0;

        public DataConsole(Rank r, Leaderboard l)
        {
            this.r = r;
            this.l = l;
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            //初始化
            for (int i = 0; i < 20; i++)
            {
                drivers[i] = new DCdata(r.cars[i].name);
            }
            for (int i = 0; i < 10; i++)
            {
                cars[i] = new DCdata(l.lbt[i].team.name);
            }
            ComboSet(isdriver);
        }

        public void ComboSet(bool isdriver)
        {
            //添加下拉列表
            comboBox.Items.Clear();
            if (isdriver)
            {
                for (int i = 0; i < 20; i++)
                {
                    comboBox.Items.Add(drivers[i].name);
                }
            }
            else
            {
                for (int i = 0; i < 10; i++)
                {
                    comboBox.Items.Add(cars[i].name);
                }
            }
        }

        public void SetRank(Rank r)
        {
            //更新数据
            this.r = r;
            foreach (var driver in drivers)
            {
                for (int i = 0; i < 20; i++)
                {
                    if (driver.name == r.cars[i].name)
                    {
                        driver.AddrData(r.cars[i].AverCap());
                    }
                }
            }
            Saveteam();
            foreach (var car in cars)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (car.name == teams[i].name)
                    {
                        car.AddrData(teams[i].AverCap());
                    }
                }
            }
        }

        public void SetLB(Leaderboard l)
        {
            //更新数据
            this.l = l;
            foreach (var driver in drivers)
            {
                for (int i = 0; i < 20; i++)
                {
                    if (driver.name == l.lb[i].name)
                    {
                        driver.AddlData(l.lb[i].point, i);
                    }
                }
            }
            foreach (var car in cars)
            {
                for (int i = 0; i < 10; i++) 
                {
                    if (car.name == l.lbt[i].team.name)
                    {
                        car.AddlData(l.lbt[i].point, i);
                    }
                }
            }
        }

        public void PrintCap()
        {
            //显示车手能力
            SortDriver();
            label1.Text = r.cars[0].name;
            progressBar1.Value = (int)r.cars[0].AverCap();
            label2.Text = r.cars[1].name;
            progressBar2.Value = (int)r.cars[1].AverCap();
            label3.Text = r.cars[2].name;
            progressBar3.Value = (int)r.cars[2].AverCap();
            label4.Text = r.cars[3].name;
            progressBar4.Value = (int)r.cars[3].AverCap();
            label5.Text = r.cars[4].name;
            progressBar5.Value = (int)r.cars[4].AverCap();
            label6.Text = r.cars[5].name;
            progressBar6.Value = (int)r.cars[5].AverCap();
            label7.Text = r.cars[6].name;
            progressBar7.Value = (int)r.cars[6].AverCap();
            label8.Text = r.cars[7].name;
            progressBar8.Value = (int)r.cars[7].AverCap();
            label9.Text = r.cars[8].name;
            progressBar9.Value = (int)r.cars[8].AverCap();
            label10.Text = r.cars[9].name;
            progressBar10.Value = (int)r.cars[9].AverCap();
            label11.Text = r.cars[10].name;
            progressBar11.Value = (int)r.cars[10].AverCap();
            label12.Text = r.cars[11].name;
            progressBar12.Value = (int)r.cars[11].AverCap();
            label13.Text = r.cars[12].name;
            progressBar13.Value = (int)r.cars[12].AverCap();
            label14.Text = r.cars[13].name;
            progressBar14.Value = (int)r.cars[13].AverCap();
            label15.Text = r.cars[14].name;
            progressBar15.Value = (int)r.cars[14].AverCap();
            label16.Text = r.cars[15].name;
            progressBar16.Value = (int)r.cars[15].AverCap();
            label17.Text = r.cars[16].name;
            progressBar17.Value = (int)r.cars[16].AverCap();
            label18.Text = r.cars[17].name;
            progressBar18.Value = (int)r.cars[17].AverCap();
            label19.Text = r.cars[18].name;
            progressBar19.Value = (int)r.cars[18].AverCap();
            label20.Text = r.cars[19].name;
            progressBar20.Value = (int)r.cars[19].AverCap();
        }

        public void PrintCarCap()
        {
            //显示赛车能力
            Saveteam();
            SortCar();
            label001.Text = teams[0].name;
            progressBar001.Value = (int)teams[0].AverCap();
            label002.Text = teams[1].name;
            progressBar002.Value = (int)teams[1].AverCap();
            label003.Text = teams[2].name;
            progressBar003.Value = (int)teams[2].AverCap();
            label004.Text = teams[3].name;
            progressBar004.Value = (int)teams[3].AverCap();
            label005.Text = teams[4].name;
            progressBar005.Value = (int)teams[4].AverCap();
            label006.Text = teams[5].name;
            progressBar006.Value = (int)teams[5].AverCap();
            label007.Text = teams[6].name;
            progressBar007.Value = (int)teams[6].AverCap();
            label008.Text = teams[7].name;
            progressBar008.Value = (int)teams[7].AverCap();
            label009.Text = teams[8].name;
            progressBar009.Value = (int)teams[8].AverCap();
            label010.Text = teams[9].name;
            progressBar010.Value = (int)teams[9].AverCap();
        }

        public void SortDriver()
        {
            //车手能力排序
            for (int i = 0; i < 20; i++)
            {
                for (int j = i + 1; j < 20; j++)
                {
                    if (r.cars[i].AverCap() < r.cars[j].AverCap())
                    {
                        Car temp = r.cars[i];
                        r.cars[i] = r.cars[j];
                        r.cars[j] = temp;
                    }
                }
            }
        }

        public void SortCar()
        {
            //按赛车能力排序
            for (int i = 0; i < 10; i++)
            {
                for (int j = i + 1; j < 10; j++)
                {
                    if (teams[i].AverCap() < teams[j].AverCap())
                    {
                        Team temp = teams[i];
                        teams[i] = teams[j];
                        teams[j] = temp;
                    }
                }
            }
        }

        public void Saveteam()
        {
            //把rank里的team保存下来
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
                        cars[j] = 1;
                        break;
                    }
                }
                for (int j = index + 1; j < 20; j++)
                {
                    if (r.cars[index].team.name == r.cars[j].team.name)
                    {
                        cars[j] = 1;
                        cars[index] = 1;
                        break;
                    }
                }
                teams[i] = r.cars[index].team;
            }
        }

        private void buttonexit_Click(object sender, EventArgs e)
        {
            //隐藏窗口
            Hide();
        }

        private void DriverCap_Load(object sender, EventArgs e)
        {
            //载入
            PrintCap();
            DataTable dt = GetTable(isdriver, drivers[0].name, type);
            DrawChart(dt, drivers[0].name);
            timerrefresh.Start();
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            //切换选项卡
            if (tabControl.SelectedTab == tabPage1)
            {
                PrintCap();
            }
            else
            {
                PrintCarCap();
            }
        }

        private void timerrefresh_Tick(object sender, EventArgs e)
        {
            //刷新能力数据
            timerrefresh.Interval = 5000;
            if (tabControl.SelectedTab == tabPage1)
            {
                PrintCap();
            }
            else
            {
                PrintCarCap();
            }
            RefreshChart();
        }

        private void radioButtondriver_CheckedChanged(object sender, EventArgs e)
        {
            //添加下拉列表
            ClearChart();
            if (radioButtondriver.Checked)
            {
                isdriver = true;
            }
            else if (radioButtonteam.Checked)
            {
                isdriver = false;
            }
            ComboSet(isdriver);
        }

        private void radioButtoncap_CheckedChanged(object sender, EventArgs e)
        {
            //图表类别
            ClearChart();
            if (radioButtoncap.Checked)
            {
                type = 1;
            }
            else if (radioButtonpoint.Checked)
            {
                type = 2;
            }
            else if (radioButtonrank.Checked)
            {
                type = 3;
            }
        }

        private void radioButtonpoint_CheckedChanged(object sender, EventArgs e)
        {
            //图表类别
            ClearChart();
            if (radioButtoncap.Checked)
            {
                type = 1;
            }
            else if (radioButtonpoint.Checked)
            {
                type = 2;
            }
            else if (radioButtonrank.Checked)
            {
                type = 3;
            }
        }

        public DataTable GetTable(bool isdriver, string name, int type)
        {
            //取得数据表，type=1，能力；2，积分；3，排名。
            DataTable dt = new DataTable();
            dt.Columns.Add("season");
            dt.Columns.Add("data");
            int index = 10;
            if (isdriver)
            {
                index = 20;
            }
            for (int i = 0; i < index; i++)
            {
                if (isdriver)
                {
                    if (drivers[i].name == name)
                    {
                        index = i;
                        break;
                    }
                }
                else
                {
                    if (cars[i].name == name)
                    {
                        index = i;
                        break;
                    }
                }
            }
            if (isdriver)
            {
                if (type == 1)
                {
                    for (int i = 0; i < drivers[index].cap.Length; i++) 
                    {
                        DataRow dr = dt.NewRow();
                        dr["season"] = (i + 1).ToString();
                        dr["data"] = drivers[index].cap[i];
                        dt.Rows.Add(dr);
                    }
                }
                else if (type == 2)
                {
                    for (int i = 0; i < drivers[index].point.Length; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["season"] = (i + 1).ToString();
                        dr["data"] = drivers[index].point[i];
                        dt.Rows.Add(dr);
                    }
                }
                else if (type == 3)
                {
                    for (int i = 0; i < drivers[index].rank.Length; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["season"] = (i + 1).ToString();
                        dr["data"] = drivers[index].rank[i] + 1;
                        dt.Rows.Add(dr);
                    }
                }
            }
            else
            {
                if (type == 1)
                {
                    for (int i = 0; i < cars[index].cap.Length; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["season"] = (i + 1).ToString();
                        dr["data"] = cars[index].cap[i];
                        dt.Rows.Add(dr);
                    }
                }
                else if (type == 2)
                {
                    for (int i = 0; i < cars[index].point.Length; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["season"] = (i + 1).ToString();
                        dr["data"] = cars[index].point[i];
                        dt.Rows.Add(dr);
                    }
                }
                else if (type == 3)
                {
                    for (int i = 0; i < cars[index].rank.Length; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr["season"] = (i + 1).ToString();
                        dr["data"] = cars[index].rank[i] + 1;
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;
        }

        public void DrawChart(DataTable dt, string name, bool lite=false)
        {
            //绘制图表
            if (chartnum < 4)
            {
                for(int i = 0; i < chartdata.Series.Count; i++)
                {
                    if (chartdata.Series[i].Name == name)
                    {
                        MessageBox.Show(name + "已存在。");
                        return;
                    }
                }
                if (!lite)
                {
                    names[nameindex] = name;
                    nameindex++;
                }
                chartdata.ChartAreas[0].AxisY.IsReversed = false;
                Series srs = new Series(name);
                srs.Points.DataBind(dt.AsEnumerable(), "season", "data", "");
                srs.ChartType = SeriesChartType.Line;
                srs.BorderWidth = 3;
                srs.MarkerStyle = MarkerStyle.Diamond;
                chartdata.ChartAreas[0].AxisY.MinorGrid.Enabled = true;
                chartdata.ChartAreas[0].AxisY.MinorGrid.LineDashStyle = ChartDashStyle.Dash;
                if (type == 1)
                {
                    chartdata.ChartAreas[0].AxisY.Maximum = 100;
                    chartdata.ChartAreas[0].AxisY.Minimum = 50;
                    chartdata.ChartAreas[0].AxisY.MajorGrid.Interval = 10;
                    chartdata.ChartAreas[0].AxisY.MinorGrid.Interval = 5;
                    chartdata.ChartAreas[0].AxisY.Interval = 10;
                    chartdata.ChartAreas[0].AxisY.Title = "能力";
                }
                else if (type == 2)
                {
                    chartdata.ChartAreas[0].AxisY.Maximum = 1100;
                    chartdata.ChartAreas[0].AxisY.MinorGrid.Interval = 50;
                    chartdata.ChartAreas[0].AxisY.MajorGrid.Interval = 200;
                    chartdata.ChartAreas[0].AxisY.Interval = 200;
                    if (isdriver)
                    {
                        chartdata.ChartAreas[0].AxisY.Maximum = 650;
                        chartdata.ChartAreas[0].AxisY.MajorGrid.Interval = 100;
                        chartdata.ChartAreas[0].AxisY.MinorGrid.Interval = 25;
                        chartdata.ChartAreas[0].AxisY.Interval = 100;
                    }
                    chartdata.ChartAreas[0].AxisY.Minimum = 0;
                    chartdata.ChartAreas[0].AxisY.Title = "积分";
                }
                else if (type == 3)
                {
                    chartdata.ChartAreas[0].AxisY.IsReversed = true;
                    chartdata.ChartAreas[0].AxisY.Maximum = 20;
                    chartdata.ChartAreas[0].AxisY.MajorGrid.Interval = 5;
                    chartdata.ChartAreas[0].AxisY.MinorGrid.Interval = 1;
                    chartdata.ChartAreas[0].AxisY.Interval = 2;
                    if (!isdriver)
                    {
                        chartdata.ChartAreas[0].AxisY.Maximum = 10;
                        chartdata.ChartAreas[0].AxisY.MajorGrid.Interval = 2;
                        chartdata.ChartAreas[0].AxisY.MinorGrid.Interval = 1;
                        chartdata.ChartAreas[0].AxisY.Interval = 1;
                    }
                    chartdata.ChartAreas[0].AxisY.Minimum = 1;
                    chartdata.ChartAreas[0].AxisY.Title = "排名";
                }
                chartdata.Series.Add(srs);
                chartnum++;
            }
            else
            {
                MessageBox.Show("显示数量已到达上限。");
            }
        }

        private void buttonchart_Click(object sender, EventArgs e)
        {
            //查看图表
            if (comboBox.SelectedItem != null)
            {
                DataTable dt = GetTable(isdriver, comboBox.SelectedItem.ToString(), type);
                DrawChart(dt, comboBox.SelectedItem.ToString());
            }
            else
            {
                MessageBox.Show("请在下拉列表中选择一个车手或车队。");
            }
        }

        public void ClearChart(bool lite = false)
        {
            //清除图表
            if (!lite)
            {
                Array.Clear(names, 0, 4);
                nameindex = 0;
            }
            chartdata.Series.Clear();
            chartnum = 0;
        }

        private void DataConsole_FormClosing(object sender, FormClosingEventArgs e)
        {
            //关闭改为隐藏
            e.Cancel = true;
            Hide();
        }

        private void buttonclear_Click(object sender, EventArgs e)
        {
            //清理图表
            ClearChart();
        }

        private void buttonsave_Click(object sender, EventArgs e)
        {
            //保存图表
            if (nameindex != 0)
            {
                string Dirpath = Path.Combine(Application.StartupPath, "chart");
                if (!Directory.Exists(Dirpath))
                {
                    Directory.CreateDirectory(Dirpath);
                }
                string namestr = "";
                for (int i = 0; i < nameindex; i++)
                {
                    namestr += names[i] + "-";
                }
                string typestr = "";
                if (type == 1)
                {
                    typestr = "能力变化";
                }
                else if (type == 2)
                {
                    typestr = "历年积分";
                }
                else if (type == 3)
                {
                    typestr = "历年排名";
                }
                string Chartpath = Path.Combine(Dirpath, namestr + typestr + ".png");
                chartdata.SaveImage(Chartpath, ChartImageFormat.Png);
                MessageBox.Show("已保存到" + Chartpath);
            }
            else
            {
                MessageBox.Show("图中没有内容！");
            }
        }

        public void RefreshChart()
        {
            //刷新图表
            ClearChart(true);
            for (int i = 0; i < nameindex; i++)
            {
                DataTable dt = GetTable(isdriver, names[i], type);
                DrawChart(dt, names[i], true);
            }
        }
    }

    public class DCdata
    {
        public string name;
        public double[] cap = new double[0];
        public int[] point = new int[0];
        public int[] rank = new int[0];
        int lindex = 0;
        int rindex = 0;

        public DCdata(string name)
        {
            this.name = name;
        }

        public void AddlData(int point, int rank)
        {
            //添加数据
            lindex++;
            Array.Resize(ref this.point, lindex);
            Array.Resize(ref this.rank, lindex);
            this.point[lindex - 1] = point;
            this.rank[lindex - 1] = rank;
        }

        public void AddrData(double cap)
        {
            //添加数据
            rindex++;
            Array.Resize(ref this.cap, rindex);
            this.cap[rindex - 1] = cap;
        }
    }
}
