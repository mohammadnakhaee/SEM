using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelloWorld
{
    public partial class UserControl2 : UserControl
    {
        public UserControl2()
        {
            InitializeComponent();
        }
        private Point p;
        private int x, y ,b,c;
        public Point Value
        {
            get { pictureBox1.Location = p; return p; }
            set { p = value; }
        }
        public int X
        {
            get { return x; }
            set
            {
                if (value > 2048) x = 2048; else if (value < -2047) x = -2047; else x = value;
                p.X = (int)((double)x / 40.96 + 45.0);
                pictureBox1.Location = p;
            }
        }
        public int Y
        {
            get { return y; }
            set
            {
                if (value > 2048) value = 2048; else if (Y < -2047) y = -2047; else y = value;
                p.Y = (int)((double)y / 40.96 + 45.0);
                pictureBox1.Location = p;
            }
        }
        private static void Raise(EventHandler handler, object sender)
        {
            if (handler != null)
            {
                handler(sender, EventArgs.Empty);
            }
        }

        public event EventHandler valueChanged;
        protected override void WndProc(ref Message m)
        {
            int a;

           
            switch (m.Msg)
            {

                case 0x020E:
                    // FireMouseHWheel(m.WParam, m.LParam);0x020A

                    //a = (short)((((double)m.WParam.ToInt64()) - 0.5) * 20);
                    a = (short)((m.WParam.ToInt64()) >> 16);
                    label1.Text = a.ToString();

                    x -= (int)((double)a/5.0 );
                    if (x > 2048) x = 2048; if (x < -2047) x = -2047;
                    p.X = (int)((double)x / 40.96 + 45.0);
                    pictureBox1.Location = p;
                    Raise(valueChanged, this);
                    m.Result = (IntPtr)1;
                 //   return;
                    break;
                case 0x020A:
                    // FireMouseHWheel(m.WParam, m.LParam);0x020A
             //       this.label1.Text = b.ToString();
                    a = (short)((m.WParam.ToInt64()) >> 16);
                    // label2.Text = a.ToString();
                    y += (int)((double)a / 5.0);
                    if (y > 2048) y = 2048; if (y < -2047) y = -2047;
                    p.Y = (int)((double)y / 40.96 + 45.0);
                    pictureBox1.Location = p;
                    Raise(valueChanged, this);
                    m.Result = (IntPtr)1;
                 //   return;
                    break;
                default:
                  
                    
                    //return;
                    break;

            }
            base.WndProc(ref m);
            if (m.HWnd != this.Handle)
            {
                return;
            }

        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void UserControl2_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void UserControl2_Load(object sender, EventArgs e)
        {
            p = pictureBox1.Location;
            x = 0; y = 0;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
