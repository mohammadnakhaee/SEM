using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HelloWorld
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
            pictureBox1.Visible = false;
            autocross = true;
        }

        private Point p, vp;
        private int x, y, px, py, cx, cy;
        public Point Value
        {
            get { vp.X = x; vp.Y = y; return vp; }
            set
            {
                if (value.X > 2048) x = 2048; else if (value.X < -2047) x = -2047;
                if (value.Y > 2048) y = 2048; else if (value.Y < -2047) y = -2047;
                x = value.X; y = value.Y;
                p.X = (int)((double)x / 40.96 + 45.0);
                p.Y = (int)((double)y / 40.96 + 45.0);
                pictureBox1.Location = p;
                Refresh();
                Raise(valueChanged, this);
            }
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
                if (value > 2048) y = 2048; else if (value < -2047) y = -2047; else y = value;
                p.Y = (int)((double)y / 40.96 + 45.0);
                pictureBox1.Location = p;
            }
        }

        public bool autocross { get; set; }

        public void FireChangesEvent()
        {
            Refresh();
            Raise(valueChanged, this);
        }
        private static void Raise(EventHandler handler, object sender)
        {
            if (handler != null)
            {
                handler(sender, EventArgs.Empty);
            }
        }

        public event EventHandler valueChanged;
        private void UserControl1_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void UserControl1_Load(object sender, EventArgs e)
        {

            p = pictureBox1.Location;
            x = 0; y = 0;

        }
        protected override void WndProc(ref Message m)
        {
            int a;


            switch (m.Msg)
            {
                case 0x020E:
                    // FireMouseHWheel(m.WParam, m.LParam);0x020A

                    //a = (short)((((double)m.WParam.ToInt64()) - 0.5) * 20);
                    a = (short)((m.WParam.ToInt64()) >> 16);
                    x -= (int)((double)a / 5.0);
                    if (x > 2048) x = 2048; if (x < -2047) x = -2047;
                    p.X = (int)((double)x / 40.96 + 45.0);
                    pictureBox1.Location = p;
                    Refresh();
                    Raise(valueChanged, this);
                    m.Result = (IntPtr)1;
                    //   return;
                    break;
                case 0x0114:
                    //0x020E
                    // FireMouseHWheel(m.WParam, m.LParam);0x020A
                    //    a = (short)((m.WParam.ToInt64()) >> 16);
                    // label1.Text = a.ToString();

                    //   x -= (int)((double)a / 5.0);
                    a = (short)((((double)m.WParam.ToInt64()) - 0.5) * 20);


                    x -= (int)((double)a);
                    if (x > 2048) x = 2048; if (x < -2047) x = -2047;
                    p.X = (int)((double)x / 40.96 + 45.0);
                    pictureBox1.Location = p;
                    Refresh();
                    Raise(valueChanged, this);
                    m.Result = (IntPtr)1;
                    return;
                    break;
                case 0x020A:
                    // FireMouseHWheel(m.WParam, m.LParam);0x020A
                    a = (short)((m.WParam.ToInt64()) >> 16);
                    // label2.Text = a.ToString();
                    y += (int)((double)a / 5.0);
                    if (y > 2048) y = 2048; if (y < -2047) y = -2047;
                    p.Y = (int)((double)y / 40.96 + 45.0);
                    pictureBox1.Location = p;
                    Refresh();
                    Raise(valueChanged, this);
                    m.Result = (IntPtr)1;
                    return;
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

        private void ovalShape2_Click(object sender, EventArgs e)
        {

        }

        private void UserControl1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            p.X = e.X - 5;
            p.Y = e.Y - 5;

            pictureBox1.Location = p;
            x = (int)(((double)p.X - 45.0) * 40.96);
            y = (int)(((double)p.Y - 45.0) * 40.96);
            if (x > 2048) x = 2048; if (x < -2047) x = -2047;
            if (y > 2048) y = 2048; if (y < -2047) y = -2047;
            Refresh();
            Raise(valueChanged, this);
        }

        private void ovalShape2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            UserControl1_MouseDoubleClick(sender, e);
        }

        private void UserControl1_MouseDown(object sender, MouseEventArgs e)
        {
            px = e.X;
            py = e.Y;
            cx = p.X;
            cy = p.Y;
        }

        private void pictureBox1_LocationChanged(object sender, EventArgs e)
        {
            // FireChangesEvent();
        }

        private void lineShape1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            p.X = e.X - 5;
            p.Y = e.Y + 50 - 5;

            pictureBox1.Location = p;
            x = (int)(((double)p.X - 45.0) * 40.96);
            y = (int)(((double)p.Y - 45.0) * 40.96);
            if (x > 2048) x = 2048; if (x < -2047) x = -2047;
            if (y > 2048) y = 2048; if (y < -2047) y = -2047;
            Refresh();
            Raise(valueChanged, this);
        }

        private void UserControl1_Paint(object sender, PaintEventArgs e)
        {
            Pen myPen = new Pen(Color.Beige);
            myPen.Width = 1;
            if (autocross == true)
            {
                e.Graphics.DrawLine(myPen, 0, 50, 100, 50);
                e.Graphics.DrawLine(myPen, 50, 0, 50, 100);
            }
            Pen myPen2 = new Pen(Color.DeepPink, 1);
            int ix = (int)Math.Round((double)pictureBox1.Location.X + (double)pictureBox1.Size.Width / 2.0);
            int iy = (int)Math.Round((double)pictureBox1.Location.Y + (double)pictureBox1.Size.Height / 2.0);
            e.Graphics.DrawLine(myPen2, ix - 10, iy, ix + 10, iy);
            e.Graphics.DrawLine(myPen2, ix, iy - 10, ix, iy + 10);

            Pen myPen3 = new Pen(Color.DeepPink, 2);
            e.Graphics.DrawArc(myPen3, ix - 6, iy - 6, 12, 12, 0, 360f);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {


        }

        private void UserControl1_MouseMove(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Left)
            {
                int dmin = (int)Math.Round((double)pictureBox1.Height / 2.0);
                int dmax = (int)Math.Round(this.Height - (double)pictureBox1.Height / 2.0) + 2;
                p.X = cx + (e.Location.X - px);
                p.Y = cy + (e.Location.Y - py);
                if (p.X > dmax) p.X = dmax; if (p.Y > dmax) p.Y = dmax;
                if (p.X < -dmin) p.X = -dmin; if (p.Y < -dmin) p.Y = -dmin;
                pictureBox1.Location = p;
                x = (int)(((double)p.X - 45.0) * 40.96);
                y = (int)(((double)p.Y - 45.0) * 40.96);
                if (x > 2048) x = 2048; if (x < -2047) x = -2047;
                if (y > 2048) y = 2048; if (y < -2047) y = -2047;
                Refresh();
                Raise(valueChanged, this);
            }
        }

        private void lineShape2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            p.X = e.X + 50 - 5;
            p.Y = e.Y - 5;

            pictureBox1.Location = p;
            x = (int)(((double)p.X - 45.0) * 40.96);
            y = (int)(((double)p.Y - 45.0) * 40.96);
            if (x > 2048) x = 2048; if (x < -2047) x = -2047;
            if (y > 2048) y = 2048; if (y < -2047) y = -2047;
            Refresh();
            Raise(valueChanged, this);
        }
    }
}
