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
        private int count,last_x=0,last_y=0;
        public FormMain mainform;
        public StageForm()
        {
            InitializeComponent();
            //mainform = (FormMain) this.Owner;
            timer1.Start();
            this.stage1.valueChanged += new EventHandler(valuechanged);
            this.stage1.mousevalueChanged += new EventHandler(mousevaluechanged);
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
        private void mousevaluechanged(object sender, EventArgs e)
        {
            label_span.Text = (((MouseEventArgs)e).X/150.0*(int)numericUpDown_step.Value).ToString("0.0") + "(um)," + (-((MouseEventArgs)e).Y / 150.0 * (int)numericUpDown_step.Value).ToString("0.0") + "(um)";
            label_span.Text +=" r:" + (   Math.Sqrt( ((MouseEventArgs)e).X * ((MouseEventArgs)e).X + ((MouseEventArgs)e).Y * ((MouseEventArgs)e).Y) / 150.0 * (int)numericUpDown_step.Value).ToString("0.0") + "(um)";

        }
        private void valuechanged(object sender, EventArgs e)

        { 
           // try
            {
                int hys = (int) numericUpDown_hys.Value;
                int mx = ((MouseEventArgs)e).X;
                int my = ((MouseEventArgs)e).Y;
                double teta = ((double)numericUpDown_rot.Value) * Math.PI / 180 ;
                int rx = (int)(   Math.Cos(teta) * mx + Math.Sin(teta) * my);
                int ry = (int)( - Math.Sin(teta) * mx + Math.Cos(teta) * my);

                int x = - rx * (int)numericUpDown_step.Value / 5 ;
                int y = ry * (int)numericUpDown_step.Value / 5 ;

                
                if ((last_x * x) < 0) x += (x / Math.Abs(x)) * hys;
                if ((last_y * y) < 0) y += (y / Math.Abs(y)) * hys;

                last_x = x;
                last_y = y;
                string CompleteOrder = mainform.CreateChildCommand("st", "us " + x.ToString("+00000;-00000") + " " + y.ToString("+00000;-00000") + "\r");
                if (mainform.SendAndReceiveOK(CompleteOrder))
                { }
                    //FormMain.ComPorts[0].Write("us " + x.ToString("000") + "  " + y.ToString("000") + "\r");
                    Text = CompleteOrder;// x.ToString("+000;-000") + " " + y.ToString("+000;-000");//;+#;-#;+0= "Stage (" + x.ToString("000") + " " + y.ToString("000") + ")";
                /*  if (y>0)
                      FormMain.ComPorts[0].Write("uy+ "+y.ToString()+"\r");
                 else
                      FormMain.ComPorts[0].Write("uy- " + (-y).ToString() + "\r");
                      */
            }
          //  catch(Exception ee)
            { //MessageBox.Show(ee.Message);
            }
        }

        private void numericUpDown_step_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //string CompleteOrder = mainform.CreateChildCommand("st", "set " + numericUpDown_delay.Value.ToString("00") + " " + "4000" + "\r");
                //mainform.SendAndReceiveOK("CompleteOrder");
               // label_span.Text = numericUpDown_step.Value.ToString() + "(um)*" + numericUpDown_step.Value.ToString() + "(um)";
                    //FormMain.ComPorts[0].Write("set " + numericUpDown_delay.Value.ToString("00") + " " + numericUpDown_step.Value.ToString("000") + "\r");
            }
            catch { }
        }

        private void numericUpDown_delay_ValueChanged(object sender, EventArgs e)
        { 
            try
            {
                string CompleteOrder = mainform.CreateChildCommand("st", "set " + numericUpDown_delay.Value.ToString("000") + " " + "4000" + "\r");
                mainform.SendAndReceiveOK(CompleteOrder);
                Text = CompleteOrder;
                // FormMain.ComPorts[0].Write("set " + numericUpDown_delay.Value.ToString("00") + " " + numericUpDown_step.Value.ToString("000") + "\r");
            }
            catch { }
        }

        private void StageForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void stage1_Load(object sender, EventArgs e)
        {

        }

        private void numericUpDown_hys_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
