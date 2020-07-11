using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace HelloWorld
{
    public partial class Stage : UserControl
    {
        private int mouseflag;
        private Point Mousepos;
        Pen p,pp;
        GraphicsPath capPath;
        private Brush brushv;

        public Stage()
        {
            InitializeComponent();
            p = new Pen(Color.Sienna,1);
            pp = new Pen(Color.Silver, 1);
           
            capPath = new GraphicsPath();
            //capPath.AddLine(-10, 0, 10, 0);
            capPath.AddLine(-2, 0, 0, 2);
            capPath.AddLine(0, 2, 2, 0);
            p.CustomEndCap = new System.Drawing.Drawing2D.CustomLineCap(null, capPath);
            //brushv
        }
        public void FillEllipseWithPathGradient(PaintEventArgs e)
        {
            // Create a path that consists of a single ellipse.
            GraphicsPath path = new GraphicsPath();
            path.AddEllipse(0, 0, 300, 300);

            // Use the path to construct a brush.
            PathGradientBrush pthGrBrush = new PathGradientBrush(path);

            // Set the color at the center of the path to blue.
            pthGrBrush.CenterColor = Color.FromArgb(250, 192, 192, 192);

            // Set the color along the entire boundary 
            // of the path to aqua.
            Color[] colors = { Color.FromArgb(0, 192, 192, 192) };
            pthGrBrush.SurroundColors = colors;

            e.Graphics.FillEllipse(pthGrBrush, 0, 0, 300, 300);
        }
        private void Stage_Load(object sender, EventArgs e)
        { 

        }
        public void FireChangesEvent()
        {
           // Raise(valueChanged, this);
        }
        private static void Raise(EventHandler handler, object sender , int x , int y)
        {
            MouseEventArgs e = new MouseEventArgs(MouseButtons.Left,1,x,y,0);
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        public event EventHandler valueChanged;
        public event EventHandler mousevalueChanged;
        private void Stage_MouseDown(object sender, MouseEventArgs e)
        {
            this.BorderStyle = BorderStyle.Fixed3D;
            mouseflag = 1;
            Mousepos = e.Location; 
            this.Update();
            
            
        }

        private void Stage_MouseUp(object sender, MouseEventArgs e)
        {
            this.BorderStyle = BorderStyle.FixedSingle;
            mouseflag = 0;
            this.Update();
            int xpos = Mousepos.X - 150;
            int ypos = -Mousepos.Y + 150;
            Raise(valueChanged, this, xpos, ypos);
                     
        }

        private void Stage_Paint(object sender, PaintEventArgs e)
        {

            //if (mouseflag == 1)
            {
                e.Graphics.DrawLine(p, 150, 150, (int)((double)((Mousepos.X - 150) / 1) + 150), (int)((double)((Mousepos.Y - 150) / 1) + 150));
                //e.Graphics.FillEllipse(Brushes.PowderBlue, 100, 100, 200, 200);
                FillEllipseWithPathGradient(e);
                e.Graphics.DrawEllipse(pp, 150 - 30, 150 - 30, 2 * 30, 2 * 30);
                e.Graphics.DrawEllipse(pp, 150 - 60, 150 - 60, 2 * 60, 2 * 60);
                e.Graphics.DrawEllipse(pp, 150 - 90, 150 - 90, 2 * 90, 2 * 90);
                e.Graphics.DrawEllipse(pp, 150 - 120, 150 - 120, 2 * 120, 2 * 120);
                e.Graphics.DrawEllipse(pp, 150 - 150, 150 - 150, 2 * 150, 2 * 150);
                e.Graphics.DrawLine(pp, 0, 150, 300, 150);
                e.Graphics.DrawLine(pp, 150, 0, 150, 300);
            }
           // else
              //  e.Graphics.Clear();
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           
        }

        private void Stage_MouseMove(object sender, MouseEventArgs e)
        {
            MouseEventArgs ee = new MouseEventArgs(MouseButtons.None, 0, e.X - 150 , e.Y - 150, 0);

            mousevalueChanged(sender, ee);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }
}
