using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PressureTest
{
    public partial class FormCOMSetting : Form
    {
        public FormCOMSetting()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var comValue = textBox1.Text.Trim().ToUpper();
            if (string.IsNullOrEmpty(comValue)) 
            {
                MessageBox.Show("Please fill the blanks", "Warning");
                return;
            }


            Regex regex = new(@"\bCOM[1-9][0-9]*\b");

            bool isValid = regex.IsMatch(comValue);

            if (!isValid) 
            { 
                MessageBox.Show("COM Address must be valid. eg: COM1", "Warning");
                return;
            }

            Properties.Settings.Default.COM_ADDRESS = comValue;
            Properties.Settings.Default.Save();

            MessageBox.Show("Settings successfully saved", "Success");
            this.Close();
        }

        private void FormCOMSetting_Load(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Settings.Default.COM_ADDRESS;
            Properties.Settings.Default.Save();
        }
    }
}
