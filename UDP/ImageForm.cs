using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace HelloWorld
{
    public partial class ImageForm : Form
    {
        public Emgu.CV.UI.ImageBox ViewPort;
        public bool isthisviewport = false;
        public Size oldsize;
        private Point[] points;

        public Image<Gray, byte> frame { get;  set; }

        public ImageForm()
        {
            InitializeComponent();

            /*  Panel border = new Panel();
              panel1.Controls.Add(border);
              border.Size = new System.Drawing.Size(2, 2);
              border.BackColor = Color.LightGray;
              border.BorderStyle = BorderStyle.Fixed3D;
              border.TabIndex = 1;
              border.Dock = System.Windows.Forms.DockStyle.Top;
              */
            frame = new Image<Gray, byte>(512, 512+50, new Gray(0));
            ViewPort = new Emgu.CV.UI.ImageBox();
            panel1.Controls.Add(ViewPort);
           // panel1.dr
            ViewPort.Dock = System.Windows.Forms.DockStyle.Fill;
            ViewPort.Image = null;
            ViewPort.Location = new System.Drawing.Point(0, 0);
            ViewPort.Name = "ViewPort";
            ViewPort.Size = new System.Drawing.Size(512, 512 + 50);
            ViewPort.TabIndex = 2;
            ViewPort.SetZoomScale((double)Math.Min(panel1.Size.Height, panel1.Size.Width) / 512.0, Point.Empty);
            // propertyGrid1.SelectedObject = ViewPort.ContextMenu;

            points = new Point[4];
            points[0].X = 0; points[0].Y = 513; points[1].X = 512; points[1].Y = 513;
            points[3].X = 0; points[3].Y = 512+50; points[2].X = 512; points[2].Y = 512+50;
            frame.FillConvexPoly(points, new Gray(255));
            MCvFont f = new MCvFont(FONT.CV_FONT_HERSHEY_COMPLEX, 0.5, .5);
            //ViewPort.Size = new System.Drawing.Size(700, 700);
            string info = String.Format("Qunta");
          // frame.
            frame.Draw(info, ref f, new System.Drawing.Point(400, 512+10), new Gray(100)); //Draw on the image using a specific font
            ViewPort.Image = frame;//.PyrUp();

            ViewPort.SetZoomScale(1.1, Point.Empty);

            //frame.DrawPolyline()
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
            
        }

        private void ImageForm_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Resize(object sender, EventArgs e)
        {

            if(ViewPort != null)
            ViewPort.SetZoomScale((double)Math.Min(panel1.Size.Height,panel1.Size.Width)/562.0, Point.Empty);
       
        }

        private void ImageForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (isthisviewport) FormMain.isformmode = false;
        }

        private void ImageForm_Load(object sender, EventArgs e)
        {
        }

        private void ImageForm_ResizeBegin(object sender, EventArgs e)
        {
            oldsize = this.Size;
        }

        private void ImageForm_SizeChanged(object sender, EventArgs e)
        {
            
        }

        private void ImageForm_ResizeEnd(object sender, EventArgs e)
        {
          /*  Size s=this.Size;
            if (oldsize.Height == s.Height)
                s.Height = s.Width;
            else
                s.Width = s.Height;

            this.Size = s;*/
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked_1(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
          //ViewPort.Image.
        }
    }
}
