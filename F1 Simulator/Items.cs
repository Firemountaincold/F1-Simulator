using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace F1_Simulator
{
    public class Circuits
    {
        //赛道类
        public string name;
        public int laptimes;
        public int difficulty;
        public int laps;

        public Circuits(string circuitstr)
        {
            string[] cirs = circuitstr.Split('/');
            name = cirs[0];
            laptimes = Convert.ToInt32(cirs[1]);
            difficulty = Convert.ToInt32(cirs[2]);
            laps = Convert.ToInt32(cirs[3]);
        }
    }

    public class Capability
    {
        //车手类
        public string name;
        public int Dquan = 0;
        public int Dsin = 0;
        public int Drain = 0;
        public int Dsave = 0;

        public Capability(string cap)
        {
            //字段初始化
            string[] capa = cap.Split('/');
            name = capa[0];
            Dquan = Convert.ToInt32(capa[1]);
            Dsin = Convert.ToInt32(capa[2]);
            Drain = Convert.ToInt32(capa[3]);
            Dsave = Convert.ToInt32(capa[4]);
        }

        public Capability(string name, bool a)
        {
            //仅用名字初始化
            this.name = name;
        }
    }

    public class Team
    {
        //车队类
        public string name;
        public int Cquan = 0;
        public int Csin = 0;
        public int Crain = 0;
        public int Csave = 0;

        public Team(string teams)
        {
            //字段初始化
            string[] team = teams.Split('/');
            name = team[0];
            Cquan = Convert.ToInt32(team[1]);
            Csin = Convert.ToInt32(team[2]);
            Crain = Convert.ToInt32(team[3]);
            Csave = Convert.ToInt32(team[4]);
        }

        public Team(string name, bool a)
        {
            //用名字初始化
            this.name = name;
        }

        public double AverCap()
        {
            //返回平均能力
            return (Cquan + Csave * 0.5 + Csin + Crain * 0.5) / 3.0;
        }
    }

    public class Tyre
    {
        //轮胎类
        public Color type;

        public enum Color
        {
            //轮胎类别
            s = 0,
            m = 1,
            h = 2,
        }

        public double Getdecay(Color type)
        {
            //每圈慢多少
            if (type == Color.s)
            {
                return 0.11;
            }
            else if (type == Color.m)
            {
                return 0.1;
            }
            else
            {
                return 0.09;
            }
        }

        public double Getquicker(Color type)
        {
            //第一圈快多少
            if (type == Color.s)
            {
                return 2;
            }
            else if (type == Color.m)
            {
                return 1.6;
            }
            else
            {
                return 1.2;
            }
        }

        public int Gettyrelaps(Color type)
        {
            //每种轮胎可以稳定多少圈
            if (type == Color.s)
            {
                return 8;
            }
            else if (type == Color.m)
            {
                return 16;
            }
            else
            {
                return 28;
            }
        }
    }

    public class TransTextBox : TextBox
    {
        //重绘的透明背景TextBox
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr LoadLibrary(string lpFileName);

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams prams = base.CreateParams;
                if (LoadLibrary("msftedit.dll") != IntPtr.Zero)
                {
                    prams.ExStyle |= 0x020;
                    prams.ClassName = "RICHEDIT50W";
                }
                return prams;
            }
        }
    }
}
