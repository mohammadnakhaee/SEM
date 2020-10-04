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
using System.Net.Sockets;
using System.Net;
using System.Drawing.Drawing2D;

namespace HelloWorld
{
    public partial class ImageForm : Form
    {
        public Emgu.CV.UI.ImageBox ViewPort;
        public bool isthisviewport = false;
        public Size oldsize;
        private Point[] points;
        public volatile Image<Gray, byte> frame;
        //Image<Gray, byte> frame = new Image<Gray, byte>(512, 512, new Gray(0));
        bool isNextAnimation = true;
        //private Capture _capture = null;
        public List<UserSettings> AllUserSettings = new List<UserSettings>();
        string UserInfo = "";
        public string UserName = "";
        int nMaxCharacters = 26; // 25 Characters + 1 \r = 26
        int count = 0, counter = 0;
        StageForm stageform = new StageForm();
        double overallMotionPixelCount = 0;
        private double Image_multiply = 1.1;
        bool StartClick = false;
        bool StartMove = false;
        Point P1, P2;
        Point[] SelectionRec = new Point[4];
        Point[] SelectionRec2 = new Point[4];
        ImageForm imageform;
        ImageForm formmode;
        static public bool isformmode = false;
        int AcquireCnt = 0;
        public bool isAcquire = false;
        public FormMain ownerform;
        Socket socket;
        System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
        IPAddress LocalHost = IPAddress.Parse("190.100.101.2"); //My IP
        IPAddress ServerHost = IPAddress.Parse("190.100.101.1"); //My IP
        
        IPEndPoint EIP;
        IPEndPoint TCPEIP;
        int VideoPort = 23;
        int ControllerPort = 152;
        
        
        int x = 0;
        int y = 0;
        public int iX = 0;
        public int iY = 0;
        public int nX = 512;
        public int nY = 512;
        int nRow = 512;
        double FPS = 0;
        Int16 old_row = 32000, row = 0, row_ready = 0;
        UInt16 image_size = 512;
        public  UInt16 multiply = 1, changed_multiply = 0, multiply_count = 0, line_ready = 0;
        UInt16 sum = 0;
        int package_number = 0;
        public byte[] receivedData = new byte[512 * 512 * 128];
        byte[] receivedData_frame = new byte[512 * 512];
        int[] window_row = new int[512 * 128];
        byte[] Packet = new byte[512 + 2];
        MCvFont format = new MCvFont(FONT.CV_FONT_HERSHEY_PLAIN, 1, 1); //Create the font
        //Terminal Tab Begin
        bool isAllowToTick = true;
        int TotalTime = 0;
        int WaitTime = 5;
        bool isDataReceived = false;
        bool isDirectCOMPortComunication = false;
        int DirectCOMPortIndex = -1;
        //bool isDataReceivedSet = false;
        int HistoryIndex = 0;
        //Terminal Tab End

        //Login Tab Begin
        bool isAdmin = true;
        //Login Tab End

        //Single Shot
        decimal oldspeedVal = 200;
        int SingleShotnStep = 512;
        int SSPacketCnt = 0;
        int SSRow = 0;
        bool SingleShotMode = false;
        byte[,] myMatrix;
        int iMultiScan = 1;
        int MultiShotnStep = 1;
        bool MultiShotMode = false;
        decimal MultiShotTerminal = (decimal)0;
        PictureForm imgfrm;

        int ilmconx = 0;
        int ilmcony = 0;
        int conscon1x = 0;
        int conscon1y = 0;
        int conscon2x = 0;
        int conscon2y = 0;


        
        bool isUDPConnected = false;
        public IList<DrawObject> drawObj = new List<DrawObject>();
        

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
            this.Owner = ownerform;
            InitializeUDP();
            frame = new Image<Gray, byte>(512, 512, new Gray(100));
            ViewPort = new Emgu.CV.UI.ImageBox();
            panel1.Controls.Add(ViewPort);
            //ViewPort.Dock = System.Windows.Forms.DockStyle.Fill;
            //ViewPort.Dock = System.Windows.Forms.DockStyle.Fill;
            ViewPort.Image = null;
            ViewPort.Location = new System.Drawing.Point(0, 0);
            ViewPort.Name = "ViewPort";
            ViewPort.Size = new System.Drawing.Size((int)(512), (int)(512));
            //ViewPort.Size = new System.Drawing.Size(100, 100);
            ViewPort.TabIndex = 2;
            //ViewPort.Dock = System.Windows.Forms.DockStyle.Fill;
            ViewPort.SetZoomScale(1, Point.Empty);
            ViewPort.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.Minimum;
            //ViewPort.FunctionalMode = Emgu.CV.UI.ImageBox.FunctionalModeOption.RightClickMenu;
            ViewPort.DoubleClick += new System.EventHandler(this.ViewPort_DoubleClick);
            ViewPort.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ViewPort_MouseDown);
            ViewPort.MouseMove += new System.Windows.Forms.MouseEventHandler(this.ViewPort_MouseMove);
            ViewPort.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ViewPort_MouseUp);
            ViewPort.MouseClick+= new System.Windows.Forms.MouseEventHandler(this.ViewPort_MouseClick);
            ViewPort.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ViewPort_MouseDoubleClick);
            ViewPort.Paint += new PaintEventHandler(this.ViewPort_paint);

            points = new Point[4];
            points[0].X = 0; points[0].Y = 513; points[1].X = 512; points[1].Y = 513;
            points[3].X = 0; points[3].Y = 512+50; points[2].X = 512; points[2].Y = 512+50;
           // frame.FillConvexPoly(points, new Gray(255));
           // MCvFont f = new MCvFont(FONT.CV_FONT_HERSHEY_COMPLEX, 0.5, .5);
            //ViewPort.Size = new System.Drawing.Size(700, 700);
           // string info = String.Format("Qunta");
          // frame.
          //  frame.Draw(info, ref f, new System.Drawing.Point(400, 512+10), new Gray(100)); //Draw on the image using a specific font
          //  ViewPort.Image = frame;//.PyrUp();

            //ViewPort.SetZoomScale(1.1, Point.Empty);

            //frame.DrawPolyline()
        }
        void InitializeUDP()
        {
            
            try
            {
                EIP = new IPEndPoint(LocalHost, VideoPort);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socket.ReceiveTimeout = 200;
                socket.Bind(EIP);
            }
            catch (Exception e)
            {
             
            }
            
        }
        private void Play()
        {
            
            isUDPConnected = true;
            socket.ReceiveBufferSize = (512 + 2) * 512;
            try
            {
                socket.BeginReceive(Packet, 0, 512 + 2, SocketFlags.None, new AsyncCallback(RecieveComplete), socket);
            }
            catch
            {

            }
            
            frame_refresh_timer.Start();

            object[] parms = new object[] { 1 };
            Invoke(new _dactimer(((FormMain)(this.ownerform)).dactimer), parms);

        }
        public void RecieveComplete(IAsyncResult result)
        {
            try
            {
                {
                    socket.EndReceive(result);
                    ;
                    row = (Int16)(Packet[nX + 1] | (Packet[nX] << 8));

                    if ((old_row == row))
                        multiply_count++;
                    else
                        multiply_count = 0;
                    if (multiply_count >= multiply) multiply_count = 0;
                    if (row < old_row)
                    {

                        watch.Stop();
                        FPS += 1000.0 / watch.ElapsedMilliseconds;
                        FPS /= 2;
                        watch.Reset();
                        watch.Start();
                    }
                    old_row = row;

                    if (row < 0 || row >= nY)
                        row = 0;
                   
                    Buffer.BlockCopy(Packet, 0, receivedData, package_number * 512, nX);
                    window_row[package_number] = row;
                    if (multiply_count == (multiply - 1)) line_ready = multiply;
                    if (line_ready > 0)
                    {
                        int num = 0;
                        int linestart = (package_number - line_ready + 1);// * 512;
                        if (linestart < 0) linestart += 65536;// 33554432;
                        int pos = (row + iY) * 512 + iX;
                        
                        for (int i = 0; i < (nX); i++)
                        {
                            sum = 0;
                            for (int j = 0; j < line_ready; j++)
                            {
                                sum += receivedData[linestart * 512 + num];
                                if ((++num) >= nX) { num = 0; linestart++; if (linestart >= 65536) linestart = 0; }
                            }
                            receivedData_frame[pos + i] = (byte)(sum / line_ready);
                        }
                        line_ready = 0;
                    }
                    
                    if ((++package_number) >= 65536) package_number = 0;

                }
                // watch.Reset(); watch.Start();// label26.Text= watch.ElapsedMilliseconds.ToString();
                    socket.BeginReceive(Packet, 0, 512 + 2, SocketFlags.None, new AsyncCallback(RecieveComplete), socket);
            }
            catch (Exception Exception)
            {
                MessageBox.Show(Exception.Message);
            }

        }

        private void Stop()
        {
            
            // dactimer(0);
            try
            {
                object[] parms = new object[] { 0 };
                Invoke(new _dactimer(((FormMain)(this.ownerform)).dactimer), parms);
                // Array.Clear(receivedData, 2, 512);
                System.Threading.Thread.Sleep(200);
                isUDPConnected = false;
               // Stop();
                //socket.EndReceive(null);
                //socket.Disconnect(true);
               // 
            }
            catch
            { }
            

        }
        public delegate bool _dactimer(int state);

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            //if (pic_edit == false) return;

            //e.Graphics.DrawPath(Pens.Blue, drawObj.Last().gp);
            //e.Graphics.RotateTransform((float)Math.Atan2(rec.Size.Height, rec.Size.Width));e.Graphics.DrawEllipse(Pens.Blue, rec);
            
            
        }

        private void ImageForm_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Resize(object sender, EventArgs e)
        {

          //  if(ViewPort != null)
          //  ViewPort.SetZoomScale((double)Math.Min(panel1.Size.Height,panel1.Size.Width)/562.0, Point.Empty);
       
        }

        private void ImageForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Stop();
            try
            {
                socket.Close();
            }
            catch
            {


            }

            if (isthisviewport) FormMain.isformmode = false;
        }

        private void ImageForm_Load(object sender, EventArgs e)
        {
            try
            {
                MCvFont f = new MCvFont(FONT.CV_FONT_HERSHEY_COMPLEX, 0.4, .4);
                //ViewPort.Size = new System.Drawing.Size(700, 700);
                //string info = String.Format("Quanta");
                //frame.Draw(info, ref f, new System.Drawing.Point(227, 247), new Gray(200)); //Draw on the image using a specific font
                Image<Gray, byte> frame0 = new Image<Gray, byte>(new Bitmap(Properties.Resources.empty512));

                frame.Bytes = (frame0.GetSubRect(new Rectangle(0, 0, 512, 512))).Bytes;
               // frame = frame0.Convert<Gray, Byte>();

                ViewPort.Image = SetFilter(frame);//.PyrUp();

                ViewPort.SetZoomScale(1, Point.Empty);
                numericUpDown_imagepercent_ValueChanged(this, null);

                //frame.DrawPolyline()
            }
            catch (NullReferenceException ex)
            {
                
            }
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

        private void button23_Click(object sender, EventArgs e)
        {
            Analyse an = new Analyse();
            an.Show();
        }


        double Filter_Brightness_Val = 1.0;
        double Filter_Contrast_Val = 1.0;
        bool Filter_FLIP_HORIZONTAL = false;
        bool Filter_FLIP_VERTICAL = false;
        bool Filter_EqualizeHist = false;
        bool Filter_Bilatral = false;
        int Filter_Bilatral_KernelSize = 2;
        int Filter_Bilatral_ColorSigma = 50;
        int Filter_Bilatral_SpaceSigma = 50;
        bool Filter_EdgeDetector = false;
        int Filter_EdgeDetector_Thresh = 100;
        int Filter_EdgeDetector_Linking = 60;
        bool Filter_Median = false;
        int Filter_Median_Size = 1;
        private int Circle_clicked;
        private bool pic_edit;
        private int pic_edit_step;
        private Point move_point;
        private bool moving;
        private bool Edit_mode;
        private double viewfield;
        private bool hided;

        private Image<Gray, Byte> SetFilter(Image<Gray, Byte> frame)
        {
            //return frame;
            Image<Gray, Byte> outFrame = frame.Clone();

            outFrame._Mul(Filter_Brightness_Val);
            outFrame._GammaCorrect(Filter_Contrast_Val);

            if (Filter_Bilatral)
                outFrame = outFrame.SmoothBilatral(Filter_Bilatral_KernelSize, Filter_Bilatral_ColorSigma, Filter_Bilatral_SpaceSigma);

            if (Filter_Median)
                outFrame = outFrame.SmoothMedian(Filter_Median_Size);

            if (Filter_EqualizeHist) outFrame._EqualizeHist();

            /*
            if (Filter_Smooth.Checked)
            {
                Image<Gray, Byte> smallGrayFrame = frame.PyrDown();
                outFrame = smallGrayFrame.PyrUp();
            }
            else if (Filter_EdgeDetector.Checked)
            {
                Image<Gray, Byte> smallGrayFrame = frame.PyrDown();
                Image<Gray, Byte> smoothedGrayFrame = smallGrayFrame.PyrUp();
                outFrame = smoothedGrayFrame.Canny(100, 60);
            }
            else //isFilterNormal
            {
                outFrame = frame;
            }

            if (Filter_Normal.Checked)
            {
                Filter_Normal.Checked = true;
            }*/

            if (Filter_EdgeDetector)
            {
                //Image<Gray, Byte> smallGrayFrame = frame.PyrDown();
                //Image<Gray, Byte> smoothedGrayFrame = smallGrayFrame.PyrUp();
                //outFrame = smoothedGrayFrame.Canny(Filter_EdgeDetector_Thresh, Filter_EdgeDetector_Linking);
                outFrame = outFrame.Canny(Filter_EdgeDetector_Thresh, Filter_EdgeDetector_Linking);
            }

            if (Filter_FLIP_HORIZONTAL) outFrame._Flip(FLIP.HORIZONTAL);
            if (Filter_FLIP_VERTICAL) outFrame._Flip(FLIP.VERTICAL);

            return outFrame;
        }








        private void button_save_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(this.panel1.Width, this.panel1.Height);

            this.panel1.DrawToBitmap(bmp, new Rectangle(0, 0, this.panel1.Width, this.panel1.Height));
            // Displays a SaveFileDialog so the user can save the Image
            // assigned to Button2.
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                System.IO.FileStream fs =
                    (System.IO.FileStream)saveFileDialog1.OpenFile();
                // Saves the Image in the appropriate ImageFormat based upon the
                // File type selected in the dialog box.
                // NOTE that the FilterIndex property is one-based.
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        bmp.Save(fs, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case 2:
                        bmp.Save(fs, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 3:
                        bmp.Save(fs, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                }

                fs.Close();
            }

            // panel1.DrawToBitmap()
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (isUDPConnected)
            {
                Stop();
                button3.Text = "Run";
            }
            else
            {
                Play();
                button3.Text = "Stop";
            }
        }

        private void Btn_Acquire_Click(object sender, EventArgs e)
        {

        }

        private void Filter_Brightness_Scroll(object sender, EventArgs e)
        {
            Filter_Brightness_Val = 2.0 * Filter_Brightness.Value / 100.0;
        }
        private void Filter_Contrast_Scroll(object sender, EventArgs e)
        {
            Filter_Contrast_Val = 2.0 * Filter_Contrast.Value / 100.0;
        }

        private void CheckBox_FLIP_HORIZONTAL_CheckedChanged(object sender, EventArgs e)
        {
            Filter_FLIP_HORIZONTAL = CheckBox_FLIP_HORIZONTAL.Checked;
        }

        private void CheckBox_FLIP_VERTICAL_CheckedChanged(object sender, EventArgs e)
        {
            Filter_FLIP_VERTICAL = CheckBox_FLIP_VERTICAL.Checked;
        }

        private void CheckBox_EqualizeHist_CheckedChanged(object sender, EventArgs e)
        {
            Filter_EqualizeHist = CheckBox_EqualizeHist.Checked;
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            Filter_Bilatral = checkBox4.Checked;
            trackBar6.Enabled = Filter_Bilatral;
            trackBar7.Enabled = Filter_Bilatral;
            trackBar8.Enabled = Filter_Bilatral;
        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            Filter_Bilatral_KernelSize = trackBar6.Value;
        }

        private void trackBar7_Scroll(object sender, EventArgs e)
        {
            Filter_Bilatral_ColorSigma = trackBar7.Value;
        }

        private void trackBar8_Scroll(object sender, EventArgs e)
        {
            Filter_Bilatral_SpaceSigma = trackBar8.Value;
        }

        private void trackBar10_Scroll(object sender, EventArgs e)
        {
            Filter_EdgeDetector_Thresh = trackBar10.Value;
        }

        private void trackBar9_Scroll(object sender, EventArgs e)
        {
            Filter_EdgeDetector_Linking = trackBar9.Value;
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            Filter_EdgeDetector = checkBox5.Checked;
            trackBar10.Enabled = Filter_EdgeDetector;
            trackBar9.Enabled = Filter_EdgeDetector;
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            Filter_Median = checkBox6.Checked;
            trackBar11.Enabled = Filter_Median;
        }

        private void trackBar11_Scroll(object sender, EventArgs e)
        {
            Filter_Median_Size = 2 * trackBar11.Value - 1;
        }

        private void button_load_Click(object sender, EventArgs e)
        {
            openFileDialog_Images.ShowDialog();
        }

        private void openFileDialog_Images_FileOk(object sender, CancelEventArgs e)
        {
            Image<Gray, byte> image0 = new Image<Gray, byte>(openFileDialog_Images.FileName);
            frame = (image0.GetSubRect(new Rectangle(0, 0, 512, 512))).Clone();
            ViewPort.Image = SetFilter(frame);
        }

        private void numericUpDown_imagepercent_ValueChanged(object sender, EventArgs e)
        {
            Image_multiply = (double)numericUpDown_imagepercent.Value;
            panel1.Size = new Size((int)((Image_multiply >= 1) ? 512 * Image_multiply + 1 : 512 + 1), (int)((Image_multiply >= 1) ? (512 * Image_multiply + 35) : (512 + 35)));
            this.Size = panel1.Size + new Size(splitContainer1.Size.Width-splitContainer1.SplitterDistance, 35);
            ViewPort.Size = new Size((int)((Image_multiply >= 1) ? 512 * Image_multiply : 512), (int)((Image_multiply >= 1) ? (512 * Image_multiply) : (512)));
            ViewPort.SetZoomScale(Image_multiply, Point.Empty);
        }

        private void group1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frame_refresh_timer_Tick(object sender, EventArgs e)
        {
            // ProcessFrame(null, null);
            string info = "";
            //if (count++ > 10) this.Text = count.ToString();
            if (isUDPConnected) frame.Bytes = receivedData_frame;
            if (isAcquire)
            {
                ViewPort.Image = SetFilter(frame);
            }
            else
            {
                if (StartMove)
                {
                    //frame.Draw()
                    frame.DrawPolyline(SelectionRec, true, new Gray(255), 1);
                    frame.DrawPolyline(SelectionRec2, true, new Gray(1), 1);
                }


                info = String.Format("{0:0000.0}FPS", FPS);

                Image<Gray, byte> frame0 = SetFilter(frame);
                // frame0.Draw(info, MCvFon, new System.Drawing.Point(100, 100), new Gray(200)); //Draw on the image using the specific font


                ViewPort.Image = frame0;
            }
        }
            private void ViewPort_DoubleClick(object sender, EventArgs e)
            {
                
            }
        private void ViewPort_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(e.Button==MouseButtons.Right)
            { ChangeWindow((image_size-nX)/2, (image_size-nY) / 2 , nX, nY); }
            else
            //Point p = ViewPort.PointToClient(Cursor.Position);
            //  MessageBox.Show(p.X.ToString() + "   " + p.Y.ToString());
            ChangeWindow(0, 0, 512, 512);//(decimal)p.X, (decimal)p.Y);
        }
        
        private void ViewPort_MouseDown(object sender, MouseEventArgs e)
            {
                P1 = new Point((int)(e.X / Image_multiply), (int)(e.Y / Image_multiply));
                StartClick = true;
            }

        private void ImageSize_ValueChanged(object sender, EventArgs e)
        {

        }

        private void ViewPort_MouseMove(object sender, MouseEventArgs e)
        {
            if (Edit_mode == true)
            { 
                if (moving == false) return;
            move_point = new Point((int)(e.X / Image_multiply), (int)(e.Y / Image_multiply));
            pic_edit_paint(drawObj.Last().ObjectType, move_point, pic_edit_step);

            ViewPort.Invalidate();
            }
            else
            if (StartClick)
                {
                    P2 = new Point((int)(e.X / Image_multiply), (int)(e.Y / Image_multiply));
                    double adjustmentx = 1;
                    double adjustmenty = 1;
                    int x1 = (int)(Math.Min(P1.X, P2.X) * adjustmentx);
                    int x2 = (int)(Math.Max(P1.X, P2.X) * adjustmentx);
                    int y1 = (int)(Math.Min(P1.Y, P2.Y) * adjustmenty);
                    int y2 = (int)(Math.Max(P1.Y, P2.Y) * adjustmenty);
                    if (x1 < 1) x1 = 1;
                    if (x2 < 1) x2 = 1;
                    if (y1 < 1) y1 = 1;
                    if (y2 < 1) y2 = 1;
                    if (x1 > 510) x1 = 510;
                    if (x2 > 510) x2 = 510;
                    if (y1 > 510) y1 = 510;
                    if (y2 > 510) y2 = 510;
                    SelectionRec[0] = new Point(x1, y1);
                    SelectionRec[1] = new Point(x1, y2);
                    SelectionRec[2] = new Point(x2, y2);
                    SelectionRec[3] = new Point(x2, y1);
                    SelectionRec2[0] = new Point(x1 - 1, y1 - 1);
                    SelectionRec2[1] = new Point(x1 + 1, y2 - 1);
                    SelectionRec2[2] = new Point(x2 + 1, y2 + 1);
                    SelectionRec2[3] = new Point(x2 - 1, y1 + 1);
                    StartMove = true;
                }
            }

        private void button1_Click(object sender, EventArgs e)
        {
            

        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (moving == false) return;
            move_point = e.Location;
            
            //gp.tr
            panel1.Invalidate();

        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            return;
            if (pic_edit == true)
            { 
                drawObj.Last().points.Add(e.Location);
                if (pic_edit_step-- == 0)
                    pic_edit = false;
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            return;
            if (pic_edit == true)
                drawObj.Last().points.Add(e.Location);
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            return;
            if (pic_edit == true)
            {
                drawObj.Last().points.Add(e.Location);
                moving = true;
                if (pic_edit_step-- == 0)
                {
                    pic_edit = false;
                    moving = false;
                }
            }
        }

        private void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void ViewPort_MouseUp(object sender, MouseEventArgs e)
            {

                StartClick = false;
            

                if (StartMove && (SelectionRec[2].X > SelectionRec[0].X) && (SelectionRec[2].Y > SelectionRec[0].Y))
                {
                    StartMove = false;
                    ChangeWindow(SelectionRec[0].X, SelectionRec[0].Y, SelectionRec[2].X - SelectionRec[0].X, SelectionRec[2].Y - SelectionRec[0].Y);
                }
            
            }

            private bool Window (decimal wix, decimal wiy, decimal wnx, decimal wny)
            {
                label_winsize.Text = "window(X:{" + wnx.ToString() + "}{Y:" + wny.ToString() + "})";
                string CompleteOrder = "window " + wix.ToString() + " " + wiy.ToString() + " " + wnx.ToString() + " " + wny.ToString() + "\r";
            // Invoke()
            // ((FormMain)(this.ParentForm)).in.SendAndReceiveOK(CompleteOrder);
            //Onnotify(CompleteOrder);
            object[] parms = new object[] { CompleteOrder };
            return (bool)Invoke(new tcpcommand(((FormMain)(this.ownerform)).SendAndReceiveOK), parms);
            // you'll get a new value of 'x' here (incremented by 10)
            return true;// SendAndReceiveOK(CompleteOrder);
            }

            private bool ChangeWindow(decimal wix, decimal wiy, decimal wnx, decimal wny)
            {
                try
                {
                    //        dactimer(0);
                    bool isOK = Window(wix, wiy, wnx, wny);
                    if (!isOK) throw new Exception("Error in changing window");


                    iX = (int)wix;
                    iY = (int)wiy;
                    nX = (int)wnx;
                    nY = (int)wny;
                    //nRow = (int)wny;

                    //      dactimer(1);
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

        private void checkBox_Measure_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_Measure.Checked)
            {
                if (isUDPConnected) button3_Click(this, null);
                Edit_mode = true;
                button_circle.Enabled = true;
                button_distance.Enabled = true;
            }
            else
            {
                drawObj.Clear();
                Edit_mode = false;
                button_circle.Enabled = false;
                button_distance.Enabled = false;
            }
        }

        private void button_distance_Click(object sender, EventArgs e)
        {

            if (pic_edit == true) drawObj.RemoveAt(drawObj.Count - 1);
            pic_edit = true;
            pic_edit_step = 4;
            DrawObject obj = new DrawObject("line");
            drawObj.Add(obj);
        }

        private void button_circle_Click(object sender, EventArgs e)
        {

            if (pic_edit == true) drawObj.RemoveAt(drawObj.Count - 1);
            pic_edit = true;
            pic_edit_step = 4;
            DrawObject obj = new DrawObject("circle");
            drawObj.Add(obj);
        }

        private void panel1_DoubleClick(object sender, EventArgs e)
        {
            //if (splitContainer1.Panel2. == true)
            //{ splitContainer1.Panel2.Enabled = false; }
            //else
            //{ splitContainer1.Panel2.Enabled = true; }
            //splitContainer1.Panel2.Hide();
        /*    if (hided == true)
            { this.Size = panel1.Size + new Size(splitContainer1.Size.Width - splitContainer1.SplitterDistance, 35); hided = false; }
            else
            {
                this.Size = panel1.Size + new Size(0, 35); ; hided = true;
            }*/
        }

        public void update_image_scale(double vf, double wd)
        {
            viewfield = vf;
            double vf_log = Math.Log(vf / 3, 10);
            double vf_deci = Math.Pow(10, Math.Floor(vf_log));
            if (((vf / 3) / vf_deci) > 2)
            {
                vf_deci *= 2;
            }
            if (((vf / 3) / vf_deci) > 2)
            {
                vf_deci /= 2;
                vf_deci *= 5;
            }
            if(vf_deci>=1)
            scalelabel.Text = vf_deci.ToString() + "um";
            else
            scalelabel.Text = (vf_deci*1000).ToString() + "nm";
            scalebar.Width = (int)(panel1.Width / 3 * vf_deci / (vf / 3));
            labelscalekx.Text = (150 / vf).ToString("0.0") + "kx";
        }

        public delegate bool tcpcommand(string command);  // delegate

        
         public event tcpcommand tcpavailable; // event
        void Onnotify(string command)
        {
            tcpavailable?.Invoke(command);
        }
        private void ViewPort_MouseClick(object sender, MouseEventArgs e)
        {
            if (pic_edit == true)
            {
                Point p= new Point((int)(e.X / Image_multiply), (int)(e.Y / Image_multiply));
                drawObj.Last().points.Add(p);
                moving = true;
                if (--pic_edit_step == 0)
                {
                    pic_edit = false;
                    moving = false;
                }
            }
        }
        void ViewPort_paint(Object sender,PaintEventArgs e)
        {
            //e.Graphics.DrawLine(Pens.White, Point.Empty, move_point);
            //if (pic_edit == false) return;
            if(Edit_mode==true)
            foreach (DrawObject obj in drawObj)
            {
                e.Graphics.DrawPath(Pens.White, obj.gp);
                e.Graphics.DrawPath(Pens.LightGoldenrodYellow, obj.gp_Text);
            }
        }
        void pic_edit_paint(String s,Point move_point,int pic_edit_step)
        {
            Matrix T = new Matrix();
            Rectangle rec = Rectangle.Empty;
            GraphicsPath gp = drawObj.Last().gp;
            GraphicsPath gp_text = drawObj.Last().gp_Text;
            double D1, D2;
            float teta = 0;
            if (s == "circle")
            {
                if (pic_edit_step == 3)
                {
                    rec.Location = drawObj.Last().points[0];
                    rec.Height = 0;
                    rec.Width = (int)Math.Sqrt(Math.Pow(move_point.X - drawObj.Last().points[0].X, 2) + Math.Pow(move_point.Y - drawObj.Last().points[0].Y, 2));
                    teta = (float)(Math.Atan2(move_point.Y - drawObj.Last().points[0].Y, move_point.X - drawObj.Last().points[0].X) * 180 / Math.PI);
                    T.RotateAt(teta, drawObj.Last().points[0]);
                    gp.Reset();
                    gp.AddEllipse(rec);
                    gp.Transform(T);

                }
                else if (pic_edit_step == 2)
                {
                    rec.X = drawObj.Last().points[0].X;
                    
                    rec.Width = (int)Math.Sqrt(Math.Pow(drawObj.Last().points[1].X - drawObj.Last().points[0].X, 2) + Math.Pow(drawObj.Last().points[1].Y - drawObj.Last().points[0].Y, 2));
                    teta = (float)(Math.Atan2(drawObj.Last().points[1].Y - drawObj.Last().points[0].Y, drawObj.Last().points[1].X - drawObj.Last().points[0].X) * 180 / Math.PI);
                    double rteta = (Math.Atan2(move_point.Y - drawObj.Last().points[1].Y, move_point.X - drawObj.Last().points[1].X) * 180 / Math.PI);
                    rec.Height = 2 * (int)((Math.Sqrt(Math.Pow(drawObj.Last().points[1].X - move_point.X, 2) + Math.Pow(drawObj.Last().points[1].Y - move_point.Y, 2))) * Math.Sin((rteta - teta) / 180 * Math.PI));
                    rec.Y = drawObj.Last().points[0].Y - rec.Height / 2 ;
                    //rec.Height = -drawObj.Last().points[1].Y + move_point.Y;
                    //rec.Width = (int)Math.Sqrt(Math.Pow(drawObj.Last().points[1].X - drawObj.Last().points[0].X, 2) + Math.Pow(drawObj.Last().points[1].Y - drawObj.Last().points[0].Y, 2));
                    //teta = (float)(Math.Atan2(drawObj.Last().points[1].Y - drawObj.Last().points[0].Y, drawObj.Last().points[1].X - drawObj.Last().points[0].X) * 180 / Math.PI);
                    T.RotateAt(teta, drawObj.Last().points[0]);
                    gp.Reset();
                    gp.AddEllipse(rec);
                    gp.Transform(T);
                }
                else if (pic_edit_step == 1)
                {
                    teta = (float)(Math.Atan2(drawObj.Last().points[1].Y - drawObj.Last().points[0].Y, drawObj.Last().points[1].X - drawObj.Last().points[0].X) * 180 / Math.PI);
                    double rteta = (Math.Atan2(drawObj.Last().points[2].Y - drawObj.Last().points[1].Y, drawObj.Last().points[2].X - drawObj.Last().points[1].X) * 180 / Math.PI);
                    // rec.Height = (int)((Math.Sqrt(Math.Pow(drawObj.Last().points[1].X - move_point.X, 2) + Math.Pow(drawObj.Last().points[1].Y - move_point.Y, 2))) * Math.Sin((rteta - teta) / 180 * Math.PI));
                    D2 = 2 * Math.Sin((rteta - teta) / 180 * Math.PI) * Math.Sqrt(Math.Pow(drawObj.Last().points[2].X - drawObj.Last().points[1].X, 2) + Math.Pow(drawObj.Last().points[2].Y - drawObj.Last().points[1].Y, 2));

                    D1 = Math.Sqrt(Math.Pow(drawObj.Last().points[1].X - drawObj.Last().points[0].X, 2) + Math.Pow(drawObj.Last().points[1].Y - drawObj.Last().points[0].Y, 2));
                    //D2 = Math.Sqrt(Math.Pow(drawObj.Last().points[2].X - drawObj.Last().points[2].X, 2) + Math.Pow(drawObj.Last().points[2].Y - drawObj.Last().points[1].Y, 2));
                    D1 = viewfield * D1 / image_size;
                    D2 = viewfield * D2 / image_size;
                    if (D1 < 1)
                        drawObj.Last().Text = "D1:" + (D1 * 1000).ToString("0.0") + "nm|";
                    else
                        drawObj.Last().Text = "D1:" + (D1).ToString("0.0") + "um|";
                    if (D2 < 1)
                        drawObj.Last().Text += "D2:" + (D2* 1000).ToString("0.0") + "nm";
                    else
                        drawObj.Last().Text += "D2:" + (D2).ToString("0.0") + "um";
                    gp_text.Reset();
                    gp_text.AddString(drawObj.Last().Text, FontFamily.GenericMonospace, (int)FontStyle.Regular, 12, move_point, StringFormat.GenericDefault);

                }
            }
            else if(s == "line")
            {
                if (pic_edit_step == 3)
                {
                    
                    gp.Reset();
                    gp.AddLine(drawObj.Last().points[0], move_point);
                   // gp.Transform(T);

                }
                else if (pic_edit_step == 2)
                {
                    rec.X = drawObj.Last().points[0].X;
                    rec.Y = drawObj.Last().points[0].Y;
                    //rec.Height = drawObj.Last().points[1].Y - move_point.Y;
                    rec.Width = (int)Math.Sqrt(Math.Pow(drawObj.Last().points[1].X - drawObj.Last().points[0].X, 2) + Math.Pow(drawObj.Last().points[1].Y - drawObj.Last().points[0].Y, 2));
                    teta = (float)(Math.Atan2(drawObj.Last().points[1].Y - drawObj.Last().points[0].Y, drawObj.Last().points[1].X - drawObj.Last().points[0].X) * 180 / Math.PI);
                    double rteta = (Math.Atan2(move_point.Y - drawObj.Last().points[1].Y, move_point.X - drawObj.Last().points[1].X) * 180 / Math.PI);
                    rec.Height=(int)((Math.Sqrt(Math.Pow(drawObj.Last().points[1].X - move_point.X, 2) + Math.Pow(drawObj.Last().points[1].Y - move_point.Y, 2)))*Math.Sin((rteta-teta) / 180 * Math.PI));
                    T.RotateAt(teta, drawObj.Last().points[0]);
                    gp.Reset();
                    gp.AddLine(rec.X,rec.Y,rec.X+rec.Width,rec.Y);
                    gp.AddLine(rec.X + rec.Width, rec.Y, rec.X+rec.Width, rec.Y + rec.Height);
                    gp.AddLine(rec.X + rec.Width, rec.Y + rec.Height , rec.X , rec.Y + rec.Height );
                    gp.Transform(T);
                }
                else if (pic_edit_step == 1)
                {
                    teta = (float)(Math.Atan2(drawObj.Last().points[1].Y - drawObj.Last().points[0].Y, drawObj.Last().points[1].X - drawObj.Last().points[0].X) * 180 / Math.PI);
                    double rteta = (Math.Atan2(drawObj.Last().points[2].Y - drawObj.Last().points[1].Y, drawObj.Last().points[2].X - drawObj.Last().points[1].X) * 180 / Math.PI);
                   // rec.Height = (int)((Math.Sqrt(Math.Pow(drawObj.Last().points[1].X - move_point.X, 2) + Math.Pow(drawObj.Last().points[1].Y - move_point.Y, 2))) * Math.Sin((rteta - teta) / 180 * Math.PI));
                    D1 = Math.Sin((rteta - teta) / 180 * Math.PI) * Math.Sqrt(Math.Pow(drawObj.Last().points[2].X - drawObj.Last().points[1].X, 2) + Math.Pow(drawObj.Last().points[2].Y - drawObj.Last().points[1].Y, 2));
                    D1 = viewfield * D1 / image_size;
                    if (D1 < 1)
                        drawObj.Last().Text = "D:" + (D1 * 1000).ToString("0.0") + " nm";
                    else
                        drawObj.Last().Text = "D:" + (D1).ToString("0.0") + " um";
                    
                    T.RotateAt((teta<0 ? teta : teta+180) + 90 , move_point);
                    gp_text.Reset();
                    gp_text.AddString(drawObj.Last().Text, FontFamily.GenericMonospace, (int)FontStyle.Regular, 12, move_point, StringFormat.GenericDefault);
                    gp_text.Transform(T);

                }
            }



        }

    }
    public class DrawObject:Object
    {
        public IList<Point> points = new List<Point>();
        
        public string Text { get;  set; }
        public string ObjectType { get;  set; }
        public GraphicsPath gp = new GraphicsPath();
        public GraphicsPath gp_Text = new GraphicsPath();
        public DrawObject(String s)
        {
            ObjectType = s;
        }

        
    }
}
