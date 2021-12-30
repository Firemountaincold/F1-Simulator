using System;
using System.IO;
using System.Windows.Forms;

namespace F1_Simulator
{
    public class Log
    {
        //日志记录类
        public string logpath = Path.Combine(Application.StartupPath, "log");
        public string filename = DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".txt";

        public Log()
        {
            //初始化
            if (!Directory.Exists(logpath))
            {
                Directory.CreateDirectory(logpath);
            }
        }

        public void Save(string content)
        {
            //保存日志
            StreamWriter sw = new StreamWriter(Path.Combine(logpath, filename), true);
            sw.Flush();
            sw.Write(content);
            sw.Close();
        }
    }
}
