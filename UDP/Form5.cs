using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HelloWorld
{
    public partial class Form5 : Form
    {
        FormMain main;
        int tcp = -1;
        int udp = -1;
        int lens = -1;
        int hv = -1;
        int fb = -1;
        int stage = -1;
        int counter = 0;
        bool is4 = false;
        bool is5 = false;
        bool is6 = false;
        bool is7 = false;
        bool is8 = false;
        bool is9 = false;

        public Form5(FormMain form)
        {
            main = form;
            InitializeComponent();
            Application.EnableVisualStyles();
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.MarqueeAnimationSpeed = 30;
        }

        private void Form5_Shown(object sender, EventArgs e)
        {
            Refresh();
            counter = 0;
            timer1.Start();
        }

        private void CheckAll()
        {
            Refresh();
            Thread.Sleep(1000);
            
            if (main.Connect_TCP())
                tcp = 1;
            else
                tcp = 0;
            Refresh();

            if (tcp == 1)
            {
                Thread.Sleep(1000);
                if (main.Connect_HV())
                    hv = 1;
                else
                    hv = 0;
                Refresh();

                Thread.Sleep(1000);
                if (main.Connect_FB())
                    fb = 1;
                else
                    fb = 0;
                Refresh();

                Thread.Sleep(1000);
                if (main.Connect_Lens())
                    lens = 1;
                else
                    lens = 0;
                Refresh();

                Thread.Sleep(1000);
                if (main.Connect_Stage())
                    stage = 1;
                else
                    stage = 0;
                Refresh();

                Thread.Sleep(1000);
                if (main.Connect_UDP())
                    udp = 1;
                else
                    udp = 0;
                Refresh();

                Thread.Sleep(3000);
            }
            Close();
        }

        private void Form5_Paint(object sender, PaintEventArgs e)
        {
            if (tcp == 1)
                DrawString("LAN", Color.Green, 160, 340, 20, e.Graphics);
            else if (tcp == 0)
                DrawString("LAN", Color.Maroon, 160, 340, 20, e.Graphics);
            else
                DrawString("LAN", Color.Gray, 160, 340, 20, e.Graphics);

            if (udp == 1)
                DrawString("Video", Color.Green, 355, 340, 20, e.Graphics);
            else if (udp == 0)
                DrawString("Video", Color.Maroon, 355, 340, 20, e.Graphics);
            else
                DrawString("Video", Color.Gray, 355, 340, 20, e.Graphics);

            if (hv == 1)
                DrawString("High Voltage", Color.Green, 125, 230, 11, e.Graphics);
            else if (hv == 0)
                DrawString("High Voltage", Color.Maroon, 125, 230, 11, e.Graphics);
            else
                DrawString("High Voltage", Color.Gray, 125, 230, 11, e.Graphics);

            if (fb == 1)
                DrawString("Filament", Color.Green, 210, 155, 12, e.Graphics);
            else if (fb == 0)
                DrawString("Filament", Color.Maroon, 210, 155, 12, e.Graphics);
            else
                DrawString("Filament", Color.Gray, 210, 155, 12, e.Graphics);
            
            if (lens == 1)
                DrawString("Lens", Color.Green, 320, 150, 14, e.Graphics);
            else if (lens == 0)
                DrawString("Lens", Color.Maroon, 320, 150, 14, e.Graphics);
            else
                DrawString("Lens", Color.Gray, 320, 150, 14, e.Graphics);

            if (stage == 1)
                DrawString("Stage", Color.Green, 390, 225, 14, e.Graphics);
            else if (stage == 0)
                DrawString("Stage", Color.Maroon, 390, 225, 14, e.Graphics);
            else
                DrawString("Stage", Color.Gray, 390, 225, 14, e.Graphics);
        }

        public void DrawString(String drawString, Color color, int x, int y, int size, Graphics formGraphics)
        {
            System.Drawing.Font drawFont = new System.Drawing.Font("Arial", size);
            System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(color);
            System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
            formGraphics.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
            drawFont.Dispose();
            drawBrush.Dispose();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (counter <= 3)
            {
                Refresh();
                counter++;
            }
            else if (counter == 4)
            {
                if (!is4)
                {
                    is4 = true;
                    if (main.Connect_TCP())
                        tcp = 1;
                    else
                        tcp = 0;
                    Refresh();
                    counter++;
                }
            }
            else if (counter == 5)
            {
                if (!is5)
                {
                    is5 = true;
                    if (main.Connect_HV())
                        hv = 1;
                    else
                        hv = 0;
                    Refresh();
                    counter++;
                }
            }
            else if (counter == 6)
            {
                if (!is6)
                {
                    is6 = true;
                    if (main.Connect_FB())
                        fb = 1;
                    else
                        fb = 0;
                    Refresh();
                    counter++;
                }
            }
            else if (counter == 7)
            {
                if (!is7)
                {
                    is7 = true;
                    if (main.Connect_Lens())
                        lens = 1;
                    else
                        lens = 0;
                    Refresh();
                    counter++;
                }
            }
            else if (counter == 8)
            {
                if (!is8)
                {
                    is8 = true;
                    if (main.Connect_Stage())
                        stage = 1;
                    else
                        stage = 0;
                    Refresh();
                    counter++;
                }
            }
            else if (counter == 9)
            {
                if (!is9)
                {
                    is9 = true;
                    if (main.Connect_UDP())
                        udp = 1;
                    else
                        udp = 0;
                    Refresh();
                    counter++;
                }
            }
            else if (counter > 9 && counter < 14)
            {
                counter++;
            }
            else if (counter == 14)
            {
                timer1.Stop();
                Close();
            }
        }
    }
}
