using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelloWorld
{
    public partial class StageForm : Form
    {
        private int count;

        public StageForm()
        {
            InitializeComponent();
            timer1.Start();
            this.stage1.valueChanged += new EventHandler(valuechanged);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (count == 0)
                pictureBox1.BackColor = Color.Red;
            else
                pictureBox1.BackColor = Color.Transparent;
            if (++count > 10) count = 0;

        }

        private void stage1_MouseClick(object sender, MouseEventArgs e)
        {

        }
        private void valuechanged(object sender, EventArgs e)
        {
            try
            {
                int x = ((MouseEventArgs)e).X;
                int y = ((MouseEventArgs)e).Y;
                // string CompleteOrder = CreateChildCommand("se", "faraday " + value.ToString() + "\r");
                FormMain.ComPorts[0].Write("us " + x.ToString("000") + "  " + y.ToString("000") + "\r");
                this.Text = "Stage (" + x.ToString("000") + " " + y.ToString("000") + ")";
                /*  if (y>0)
                      FormMain.ComPorts[0].Write("uy+ "+y.ToString()+"\r");
                 else
                      FormMain.ComPorts[0].Write("uy- " + (-y).ToString() + "\r");
                      */
            }
            catch
            { }
        }

        private void numericUpDown_step_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                FormMain.ComPorts[0].Write("set " + numericUpDown_delay.Value.ToString("00") + " " + numericUpDown_step.Value.ToString("000") + "\r");
            }
            catch { }
        }

        private void numericUpDown_delay_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                FormMain.ComPorts[0].Write("set " + numericUpDown_delay.Value.ToString("00") + " " + numericUpDown_step.Value.ToString("000") + "\r");
            }
            catch { }
        }

        private void StageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
