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
        Pen p;
        GraphicsPath capPath;
        private Brush brushv;

        public Stage()
        {
            InitializeComponent();
            p = new Pen(Color.Azure,2);
            capPath = new GraphicsPath();
            //capPath.AddLine(-10, 0, 10, 0);
            capPath.AddLine(-2, 0, 0, 2);
            capPath.AddLine(0, 2, 2, 0);
            p.CustomEndCap = new System.Drawing.Drawing2D.CustomLineCap(null, capPath);
            //brushv
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

            if (mouseflag == 1)
            {
                e.Graphics.DrawLine(p, 150, 150, (int)((double)((Mousepos.X - 150) / 1.1) + 150), (int)((double)((Mousepos.Y - 150) / 1.1) + 150));
              //  e.Graphics.FillEllipse(Brushes.PowderBlue, 100, 100, 200, 200);
            }
           // else
              //  e.Graphics.Clear();
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }
}
