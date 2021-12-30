using System;
using System.Windows.Forms;

namespace F1_Simulator
{
    public partial class CareerSetting : Form
    {
        //车手设定
        public Capability cp;
        public int potential = 98;
        //游戏设定
        public int career = 15;
        public int changedelta = 3;
        public int cap = 1;     //0-high,1-mid,2-low.
        public int trans = 1;
        public int car = 1;
        public int change = 1;
        public bool savelog = true;

        public CareerSetting()
        {
            InitializeComponent();
        }

        public void Checkset()
        {
            //保存设定值
            if (radioButtoncaphigh.Checked)
            {
                cap = 0;
            }
            else if (radioButtoncapmid.Checked)
            {
                cap = 1;
            }
            else
            {
                cap = 2;
            }
            if (radioButtoncarhigh.Checked)
            {
                car = 0;
            }
            else if (radioButtoncarmid.Checked)
            {
                car = 1;
            }
            else
            {
                car = 2;
            }
            if (radioButtontranshigh.Checked)
            {
                trans = 0;
            }
            else if (radioButtontransmid.Checked)
            {
                trans = 1;
            }
            else
            {
                trans = 2;
            }
            if (radioButtonchghigh.Checked)
            {
                change = 0;
            }
            else if (radioButtonchgmid.Checked)
            {
                change = 1;
            }
            else
            {
                change = 2;
            }
        }

        private void buttonok_Click(object sender, EventArgs e)
        {
            //返回设定
            cp = new Capability(textBoxname.Text, true);
            cp.Dsin = Convert.ToInt32(textBoxcap.Text);
            cp.Dquan = Convert.ToInt32(textBoxquan.Text);
            cp.Drain = Convert.ToInt32(textBoxrain.Text);
            cp.Dsave = Convert.ToInt32(textBoxsave.Text);
            career = Convert.ToInt32(textBoxcareer.Text);
            potential = Convert.ToInt32(textBoxpote.Text);
            Checkset();
            changedelta = Convert.ToInt32(textBoxchange.Text);
            savelog = checkBox.Checked;
            DialogResult = DialogResult.OK;
        }

        private void buttoncancel_Click(object sender, EventArgs e)
        {
            //取消
            DialogResult = DialogResult.Cancel;
        }
    }
}
