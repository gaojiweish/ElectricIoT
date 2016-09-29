using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormServerDemo
{
    public partial class SystemConfig : UserControl
    {
        public SystemConfig()
        {
            InitializeComponent();
        }

        private void Button_MouseEnter(object sender, EventArgs e)
        {
            ((Button)sender).Font = new Font("Calibri", 14, FontStyle.Bold);
        }

        private void Button_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).Font = new Font("Calibri", 14, FontStyle.Regular);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.EDInterface = comboBox1.Text;
            Properties.Settings.Default.IPInterface = comboBox2.Text;
            Properties.Settings.Default.OMInterface = comboBox3.Text;
            Properties.Settings.Default.GPSInterface = comboBox4.Text;
            Properties.Settings.Default.EDProperty = textBox1.Text;
            Properties.Settings.Default.IPProperty = textBox2.Text;
            Properties.Settings.Default.OMProperty = textBox3.Text;
            Properties.Settings.Default.GPSProperty = textBox4.Text;
            Properties.Settings.Default.Upload = checkBox1.Checked;
            Properties.Settings.Default.Save();
        }

        private void SystemConfig_Load(object sender, EventArgs e)
        {
            comboBox1.Text = Properties.Settings.Default.EDInterface;
            comboBox2.Text = Properties.Settings.Default.IPInterface;
            comboBox3.Text = Properties.Settings.Default.OMInterface;
            comboBox4.Text = Properties.Settings.Default.GPSInterface;
            textBox1.Text = Properties.Settings.Default.EDProperty;
            textBox2.Text = Properties.Settings.Default.IPProperty;
            textBox3.Text = Properties.Settings.Default.OMProperty;
            textBox4.Text = Properties.Settings.Default.GPSProperty;
            checkBox1.Checked = Properties.Settings.Default.Upload;
        }
    }
}
