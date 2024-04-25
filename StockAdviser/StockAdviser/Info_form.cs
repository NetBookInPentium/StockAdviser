using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockAdviser
{
    public partial class Info_form : Form
    {
        public Info_form()
        {
            InitializeComponent();
        }
        public int page = 1;
        public int total = 3;
        private void Info_form_Load(object sender, EventArgs e)
        {
            pictureBox1.BackgroundImage = new Bitmap(Properties.Resources._1_page_);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(page != 1)
            {
                page--;
                page_list(page);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (page < total)
            {
                page++;
                page_list(page);
            }
        }
        public void page_list(int pages)
        {
            switch (pages)
            { 
                case 1:
                    pictureBox1.BackgroundImage = new Bitmap(Properties.Resources._1_page_);
                    break;
                case 2:
                    pictureBox1.BackgroundImage = new Bitmap(Properties.Resources._2_page_);
                    break;
                case 3:
                    pictureBox1.BackgroundImage = new Bitmap(Properties.Resources._3_page_);
                    break;
                case 4:
                    break;
                case 5:
                    break;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://t.me/Xen0nChan/");
            Process.Start(sInfo);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://vk.com/imegejpg/");
            Process.Start(sInfo);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://www.alphavantage.co/");
            Process.Start(sInfo);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ProcessStartInfo sInfo = new ProcessStartInfo("https://github.com/NetBookInPentium/StockAdviser/");
            Process.Start(sInfo);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
