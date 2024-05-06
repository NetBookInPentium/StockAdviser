using StockAdviser.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StockAdviser
{
    public partial class AutoReg : Form
    {
        Call_DB Call_db = new Call_DB();
        public AutoReg()
        {
            InitializeComponent();
            checkBox1.Checked = true;
            label5.Hide();

            textBox1.Text = "first_user";
            textBox2.Text = "userPassword";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked) { textBox2.PasswordChar = '*'; }
            else { textBox2.PasswordChar = default; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Call_db.Open();
            if(Call_db.Select_user(textBox1.Text, textBox2.Text)) 
            {
                Form1 form1 = new Form1();
                form1.ShowDialog();
                this.Close();
            }
            else
            {
                textBox2.Text = "";
                label5.Show();
            }
            Call_db.Close();
        }
    }
}
