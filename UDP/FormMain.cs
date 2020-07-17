using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using LibUsbDotNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using Windows.Devices.Enumeration;
using Windows.Devices.HumanInterfaceDevice;
using Windows.Storage;
//test master2
//using RawInput_dll;
//test master
namespace HelloWorld
{
    public partial class FormMain : Form
    {
        bool isNextAnimation = true;
        //private Capture _capture = null;
        public List<UserSettings> AllUserSettings = new List<UserSettings>();
        string UserInfo = "";
        public string UserName = "";
        int nMaxCharacters = 26; // 25 Characters + 1 \r = 26
        int count = 0, counter = 0;
        StageForm stageform = new StageForm();
        double overallMotionPixelCount = 0;
        Emgu.CV.UI.ImageBox ViewPort;
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
        //UdpClient udp;
        Socket socket;
        //UInt16 ready = 0;
        System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
        IPAddress LocalHost = IPAddress.Parse("190.100.101.2"); //My IP
        IPAddress ServerHost = IPAddress.Parse("190.100.101.1"); //My IP
                                                                 //  IPAddress LocalHost = IPAddress.Parse("192.168.1.101"); //My IP
                                                                 // IPAddress ServerHost = IPAddress.Parse("192.168.1.101"); //My IP
        IPEndPoint EIP;
        IPEndPoint TCPEIP;
        int VideoPort = 23;
        int ControllerPort = 152;
        Image<Gray, byte> frame = new Image<Gray, byte>(512, 512, new Gray(0));
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
        UInt16 multiply = 1, changed_multiply = 0, multiply_count = 0, line_ready = 0;
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

        //Settings Tab Begin
        int NumberOfDevices = 0;
        int MaximumNumberOfDevices = 20;
        string[] Device_names = new string[20];
        string[] Port_names = new string[20];
        string ReadToChar = "\r";
        public static List<SerialPort> ComPorts = new List<SerialPort> { };
        //Settings Tab End

        //Gun Alignment Begin
        bool isGunAlignment = false;
        double LastIntensity = 0;
        //Gun Alignment End

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

        TcpClient tcp;
        NetworkStream TCPnetworkStream;
        NetworkStream btstream;
        BluetoothClient remoteDevice;
        StreamReader bt_streamreader;
        byte[] bt_buf = new byte[4];
        int index = 1;
        Int32 bt_count;
        string hidstring;
        UsbEndpointReader trackball_Reader;

        bool isAdvancedMode = false;
        int SelectedLightControl = -1;
        int nLightControl = 8;
        // Emgu.CV.im
        int HVProfileLastIndex;
        int MicroscopyModeLastIndex;
        //<<<<<<< master
        //=======
        bool isUDPConnected = false;
        //>>>>>>> master

        

        public FormMain()
        {
            InitializeComponent();
            HVProfileLastIndex = HVProfile.SelectedIndex;
            MicroscopyModeLastIndex = MicroscopyMode.SelectedIndex;
            SetCustomBorder();

            int NumberOfHVProfile = HVProfile.Items.Count;
            for (int i = 0; i < NumberOfHVProfile; i++) AllUserSettings.Add(new UserSettings());

            Form1 f1 = new Form1();
            f1.Show();
            f1.Refresh();
            Thread.Sleep(6000);
            f1.Close();

            /*System.Windows.Forms.Panel border = new System.Windows.Forms.Panel();
            panel1.Controls.Add(border);
            border.Size = new System.Drawing.Size(2, 2);
            border.BackColor = Color.LightGray;
            border.BorderStyle = BorderStyle.Fixed3D;
            border.TabIndex = 1;
            border.Dock = System.Windows.Forms.DockStyle.Top;
            */

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
            
            // Emgu.CV.UI.Operation a;
            EIP = new IPEndPoint(LocalHost, VideoPort);
            TCPEIP = new IPEndPoint(LocalHost, ControllerPort);
            numericUpDown1.Value = nRow;

            //Terminal Tab Start
            RB2.Checked = true;
            TerminalTimerReceiver.Stop();
            TBOutput.Text = ">>> ";
            History.Items.Add("");
            //Terminal Tab End

            Load_Devices();

            this.CB_ADG_Terminal.SelectedIndexChanged -= new System.EventHandler(this.CB_ADG_Terminal_SelectedIndexChanged);
            this.CB_MCP_Terminal.SelectedIndexChanged -= new System.EventHandler(this.CB_Terminal_SelectedIndexChanged);
            this.CB_MCP_Gain.SelectedIndexChanged -= new System.EventHandler(this.CB_MCP_Gain_SelectedIndexChanged);
            this.Scanner_ISelect.SelectedIndexChanged -= new System.EventHandler(this.Scanner_ISelect_SelectedIndexChanged);
            CB_MCP_Terminal.SelectedIndex = 0;
            CB_ADG_Terminal.SelectedIndex = 0;
            CB_MCP_Gain.SelectedIndex = 0;
            Scanner_ISelect.SelectedIndex = 2;
            this.CB_ADG_Terminal.SelectedIndexChanged += new System.EventHandler(this.CB_ADG_Terminal_SelectedIndexChanged);
            this.CB_MCP_Terminal.SelectedIndexChanged += new System.EventHandler(this.CB_Terminal_SelectedIndexChanged);
            this.CB_MCP_Gain.SelectedIndexChanged += new System.EventHandler(this.CB_MCP_Gain_SelectedIndexChanged);
            this.Scanner_ISelect.SelectedIndexChanged += new System.EventHandler(this.Scanner_ISelect_SelectedIndexChanged);

            Connection_image.Image = new Bitmap(@".\Src\QV.bmp");
            Tools_image.Image = new Bitmap(@".\Src\QV.bmp");
            User_image.Image = new Bitmap(@".\Src\QV.bmp");
            Settings_image.Image = new Bitmap(@".\Src\QV.bmp");
            Render_image.Image = new Bitmap(@".\Src\QV.bmp");
            Help_image.Image = new Bitmap(@".\Src\QV.bmp");

            InitializeUDP();
            imgfrm = new PictureForm();
            this.panel1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseWheel);
            this.userControl11.valueChanged += new EventHandler(this.u1_valueChanged);
            this.userControl12.valueChanged += new EventHandler(this.u2_valueChanged);
            this.userControl13.valueChanged += new EventHandler(this.u3_valueChanged);
            this.userControl14.valueChanged += new EventHandler(this.u4_valueChanged);
            this.userControl15.valueChanged += new EventHandler(this.u5_valueChanged);
            //imgfrm.pictureBox1.Image = bmp;
            //imgfrm.Size = new Size(500, 500);
            //imgfrm.Show();
            //imgfrm.Update();
            //imgfrm.Refresh();
            //InTheHand.Net.Sockets
            //"80:19:34:94:30:58"BluetoothAddress.Parse("80:19:34:94:30:58"),, BluetoothService.SerialPort
            Guid service = new Guid("{00112233-4455-6677-8899-aabbccddeeff}");
            try
            {
                BluetoothListener l = new BluetoothListener(BluetoothAddress.Parse("80:19:34:94:30:58"), BluetoothService.SerialPort);
                l.Start();
                l.BeginAcceptBluetoothClient(new AsyncCallback(AcceptConnection), l);
                //l.SetPin(BluetoothAddress.Parse("80:19:34:94:30:58"), "0000");
                // bt_socket = l.Server;
            }
            catch (Exception e)
            { }
            textBox3.Text = "listening...     ";
            //  RawInput rawinput = new RawInput(this.Handle,true);
            // rawinput.
            //  RawInput_dll.Win32.
            // win
            // HidCollection hid = new HidCollection();
            // EnumerateHidDevices();
            //"USB\VID_046D&PID_C408\6&AED021B&0&5"     \??\USB#VID_046D&PID_C408#6&aed021b&0&5#{a5dcbf10-6530-11d2-901f-00c04fb951ed}
            //    \??\USB#VID_046D&PID_C408#6&aed021b&0&5#{a5dcbf10-6530-11d2-901f-00c04fb951ed}
            //  "\\\\?\\hid#vid_046d&pid_c408#7&2a268dad&0&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}"
            //System.IO.FileStream hidstreaam = new System.IO.FileStream(@"\\.\USB\VID_046D&PID_C408\6&AED021B&0&5", System.IO.FileMode.Open, System.IO.FileAccess.Read);
            //  UnmanagedFileLoader loader = new UnmanagedFileLoader("\\\\?\\hid#vid_046d&pid_c408#7&2a268dad&0&0000#{4d1e55b2-f16f-11cf-88cb-001111000030}");
            //loader.
            // LibUsbDotNet.


            this.BackgroundImage = new Bitmap(Properties.Resources.QuantaEyeBackground1);
            //System.Drawing.Image.FromFile("./QuantaEyeBackground1.png");
            //panel20.BackgroundImageLayout = ImageLayout.Stretch;
            //Border.Image = new Bitmap(Properties.Resources.bn);
            Border.Image = System.Drawing.Image.FromFile("./bn.gif");
            Border.SizeMode = PictureBoxSizeMode.StretchImage;


            //EnterFullScreenMode();

            //Title.BackColor = System.Drawing.Color.Transparent;
            //group1.BackColor = Color.Transparent;
            //group2.BackColor = Color.Transparent;
            //group3.BackColor = Color.Transparent;

            ZoomLight.SizeMode = PictureBoxSizeMode.StretchImage;
            FocusLight.SizeMode = PictureBoxSizeMode.StretchImage;
            IMLCenteringLight.SizeMode = PictureBoxSizeMode.StretchImage;
            StigLight.SizeMode = PictureBoxSizeMode.StretchImage;
            ObjectCenteringLight.SizeMode = PictureBoxSizeMode.StretchImage;
            GunShiftLight.SizeMode = PictureBoxSizeMode.StretchImage;
            GunTiltLight.SizeMode = PictureBoxSizeMode.StretchImage;
            GainLight.SizeMode = PictureBoxSizeMode.StretchImage;

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            AllOff();
            TabControl_Main.TabPages[6].VerticalScroll.Enabled = true;
            TabControl_Main.TabPages[6].VerticalScroll.Visible = true;
            //<<<<<<< master
            //            track_ball_enumeration();
            //=======
            //            
            //>>>>>>> master


            //group3.VerticalScroll.SmallChange = 300;
            numericUpDown_imagepercent_ValueChanged(null, null);

            //stage
            //<<<<<<< master
            // stageform.TopLevel = false;
            // stageform.Parent = this;
            //button_stage_Click(this, null);

            //leftpanel.AutoScroll = false;
            //=======
            //            sf.TopLevel = false;
            //            sf.Parent = this;
            //            button_stage_Click(this, null);
            //
            leftpanel.AutoScroll = false;
            //>>>>>>> master

            ApplyGeneralSettings();
            leftpanel.Enabled = false;
        }
        
        //<<<<<<< master
        //        private void ApplyGeneralSettings()
        //=======
        public void ApplyGeneralSettings()
        //>>>>>>> master
        {
            num_stig_x.Minimum = Settings1.Default.num_stig_min;
            num_stig_x.Maximum = Settings1.Default.num_stig_max;
            num_gain_x.Minimum = Settings1.Default.num_gain_min;
            num_gain_x.Maximum = Settings1.Default.num_gain_max;
            num_obj_x.Minimum = Settings1.Default.num_obj_min;
            num_obj_x.Maximum = Settings1.Default.num_obj_max;
            num_iml_x.Minimum = Settings1.Default.num_iml_min;
            num_iml_x.Maximum = Settings1.Default.num_iml_max;
            num_gunshift_x.Minimum = Settings1.Default.num_gunshift_min;
            num_gunshift_x.Maximum = Settings1.Default.num_gunshift_max;
            num_guntilt_x.Minimum = Settings1.Default.num_guntilt_min;
            num_guntilt_x.Maximum = Settings1.Default.num_guntilt_max;
            //<<<<<<< master
            //=======

            UD_HV_HV.Maximum = Settings1.Default.hv_raw_max;
            UD_HV_Filament.Maximum = Settings1.Default.fb_raw_max;
            //>>>>>>> master
        }

        private void SetCustomBorder()
        {
            //this.FormBorderStyle = FormBorderStyle.None;

            //PictureBox Border = new PictureBox();
            //Border.Parent = this;
            //Border.Location = new Point(0, 0);
            //Border.Size = new Size(100, 32);
            //Border.Dock = DockStyle.Top;
            //Border.Image = global::CAngle.Properties.Resources.border;
            //Border.Image = global::CAngle.Properties.Resources.cangleiconpng;
            Border.SizeMode = PictureBoxSizeMode.StretchImage;
            Border.Paint += ToolsPicture_Paint;
            Border.DoubleClick += ToolsPicture_DoubleClick;
            Border.MouseDown += ToolsPicture_MouseDown;
            Border.MouseMove += ToolsPicture_MouseMove;
            Border.MouseUp += ToolsPicture_MouseUp;

            Font drawFont2 = new Font("Marlett", 10, FontStyle.Bold);

            Button MinimizeButton = new Button();
            MinimizeButton.Parent = Buttons;
            MinimizeButton.Size = new Size(Border.Height - 3, Border.Height - 3);
            MinimizeButton.FlatStyle = FlatStyle.Flat;
            MinimizeButton.FlatAppearance.BorderSize = 0;
            MinimizeButton.Dock = DockStyle.Right;
            MinimizeButton.Font = drawFont2;
            MinimizeButton.ForeColor = Color.White;
            char c = '\u0030';
            MinimizeButton.Text = c.ToString();
            MinimizeButton.Click += MinimizeButton_Click;

            Button RestoreButton = new Button();
            RestoreButton.Parent = Buttons;
            RestoreButton.Size = new Size(Border.Height - 3, Border.Height - 3);
            RestoreButton.FlatStyle = FlatStyle.Flat;
            RestoreButton.FlatAppearance.BorderSize = 0;
            RestoreButton.Dock = DockStyle.Right;

            RestoreButton.Font = drawFont2;
            RestoreButton.ForeColor = Color.White;
            if (this.WindowState == FormWindowState.Maximized)
                c = '\u0032';
            else
                c = '\u0031';
            RestoreButton.Text = c.ToString();
            RestoreButton.Click += RestoreButton_Click;
            RestoreButton.Paint += RestoreButton_Paint;

            Button CloseButton = new Button();
            CloseButton.Parent = Buttons;
            CloseButton.Size = new Size(Border.Height - 3, Border.Height - 3);
            CloseButton.FlatStyle = FlatStyle.Flat;
            CloseButton.FlatAppearance.BorderSize = 0;
            CloseButton.Dock = DockStyle.Right;
            CloseButton.Font = drawFont2;
            CloseButton.ForeColor = Color.White;
            c = '\u0072';
            CloseButton.Text = c.ToString();
            CloseButton.Click += CloseButton_Click;


            //LeaveFullScreenMode();
            //EnterFullScreenMode();

        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();

                try
                {
                    System.Environment.Exit(0);
                }
                catch
                {

                }

                /*if (System.Windows.Forms.Application.MessageLoop)
                {
                    // WinForms app
                    System.Windows.Forms.Application.Exit();
                }
                else
                {
                    // Console app
                    System.Environment.Exit(0);
                }*/
            }
            catch
            {
                try
                {
                    System.Environment.Exit(0);
                }
                catch
                {

                }
            }
        }

        private void RestoreButton_Paint(object sender, PaintEventArgs e)
        {
            Button RestoreButton = (Button)sender;
            char c = '\u0032';
            if (this.WindowState == FormWindowState.Maximized)
                c = '\u0032';
            else
                c = '\u0031';
            RestoreButton.Text = c.ToString();
        }

        private void RestoreButton_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
                LeaveFullScreenMode();
            else
                EnterFullScreenMode();
        }

        private void MinimizeButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void AllOff()
        {
            TurnOff(ZoomLight);
            TurnOff(FocusLight);
            TurnOff(IMLCenteringLight);
            TurnOff(StigLight);
            TurnOff(ObjectCenteringLight);
            TurnOff(GunShiftLight);
            TurnOff(GunTiltLight);
            TurnOff(GainLight);

            TurnOff(pictureBox1);
            TurnOff(pictureBox2);
            TurnOff(pictureBox3);
            TurnOff(pictureBox4);
            TurnOff(pictureBox5);
        }

        private void MaximizeWindow()
        {
            var rectangle = Screen.FromControl(this).Bounds;
            //this.FormBorderStyle = FormBorderStyle.None;
            Size = new Size(rectangle.Width, rectangle.Height);
            Location = new Point(0, 0);
            Rectangle workingRectangle = Screen.PrimaryScreen.WorkingArea;
            this.Size = new Size(workingRectangle.Width, workingRectangle.Height);
        }

        private void ResizableWindow()
        {
            this.ControlBox = false;
            //this.FormBorderStyle = FormBorderStyle.SizableToolWindow;
        }

        private void TurnOn(PictureBox light, int SelectedIndex)
        {
            SelectedLightControl = SelectedIndex;
            light.Image = new Bitmap(Properties.Resources.greenbuttonon);
            light.Parent.BackColor = Color.FromArgb(99 + 20, 112 + 20, 132 + 20);
            //System.Drawing.Image.FromFile("./greenbuttonon.png");
        }

        private void TurnOff(PictureBox light)
        {
            //<<<<<<< master
            //            SelectedLightControl = -1;
            //=======
            //SelectedLightControl = -1;
            //>>>>>>> master
            light.Image = new Bitmap(Properties.Resources.greenbuttonoff);
            light.Parent.BackColor = light.Parent.Parent.BackColor;
            //System.Drawing.Image.FromFile("./greenbuttonoff.png");
        }

        private void SwitchToNext()
        {
            isNextAnimation = true;
            SelectedLightControl++;
            if (SelectedLightControl == nLightControl) SelectedLightControl = 0;
            FocusSelected();
        }

        private void SwitchToPrevious()
        {
            isNextAnimation = false;
            SelectedLightControl--;
            if (SelectedLightControl == -2) SelectedLightControl = 0;
            if (SelectedLightControl == -1) SelectedLightControl = nLightControl - 1;
            FocusSelected();
        }

        private void FocusSelected()
        {
            if (isAdvancedMode)
            {
                if (SelectedLightControl < 5)
                    AdvGetSelectedCtrl2D().Focus();
                else
                    AdvGetSelectedCtrl1D().Focus();
            }
            else
            {
                //if (SelectedLightControl < 2)
                //    StdGetSelectedCtrl1D().Focus();
                //else
                //    StdGetSelectedCtrl2D().Focus();
                AnimationTimer.Start();
            }
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            int shift = 40;
            int BestY = 250;
            if (isNextAnimation) shift = -shift;

            double y0 = 0;
            if (SelectedLightControl < 2)
                y0 = StdGetSelectedCtrl1D().Parent.Location.Y;
            else
                y0 = StdGetSelectedCtrl2D().Parent.Location.Y;

            bool isStoped = false;
            //<<<<<<< master
            //            if (y0 > BestY && y0 < BestY + 50)
            //=======
            if (y0 > BestY && y0 < BestY + 70)
            //>>>>>>> master
            {
                isStoped = true;
                AnimationTimer.Stop();
                if (SelectedLightControl < 2)
                    StdGetSelectedCtrl1D().Focus();
                else
                    StdGetSelectedCtrl2D().Focus();
            }

            if (!isStoped)
            {
                int R = groupBox_focus.Height * 5;
                int newx = groupBox_focus.Location.X;
                int newy = groupBox_focus.Location.Y + shift;
                if (newy > leftpanel.Size.Height) newy = newy - R;
                if (newy + R < leftpanel.Size.Height) newy = newy + R;
                groupBox_focus.Location = new Point(newx, newy);

                newx = groupBox_zoom.Location.X;
                newy = groupBox_zoom.Location.Y + shift;
                if (newy > leftpanel.Size.Height) newy = newy - R;
                if (newy + R < leftpanel.Size.Height) newy = newy + R;
                groupBox_zoom.Location = new Point(newx, newy);

                newx = groupBox_stig.Location.X;
                newy = groupBox_stig.Location.Y + shift;
                if (newy > leftpanel.Size.Height) newy = newy - R;
                if (newy + R < leftpanel.Size.Height) newy = newy + R;
                groupBox_stig.Location = new Point(newx, newy);

                newx = groupBox_gain.Location.X;
                newy = groupBox_gain.Location.Y + shift;
                if (newy > leftpanel.Size.Height) newy = newy - R;
                if (newy + R < leftpanel.Size.Height) newy = newy + R;
                groupBox_gain.Location = new Point(newx, newy);

                newx = groupBox_object.Location.X;
                newy = groupBox_object.Location.Y + shift;
                if (newy > leftpanel.Size.Height) newy = newy - R;
                if (newy + R < leftpanel.Size.Height) newy = newy + R;
                groupBox_object.Location = new Point(newx, newy);

                newx = groupBox_iml.Location.X;
                newy = groupBox_iml.Location.Y + shift;
                if (newy > leftpanel.Size.Height) newy = newy - R;
                if (newy + R < leftpanel.Size.Height) newy = newy + R;
                groupBox_iml.Location = new Point(newx, newy);

                newx = groupBox_gun.Location.X;
                newy = groupBox_gun.Location.Y + shift;
                if (newy > leftpanel.Size.Height) newy = newy - R;
                if (newy + R < leftpanel.Size.Height) newy = newy + R;
                groupBox_gun.Location = new Point(newx, newy);
            }
        }

        // Find HID devices.
        private void track_ball_enumeration()
        {
            // Get a new device list each time the device dropdown is opened
            //<<<<<<< master
            // cboDevices.Items.Clear();
            try
            {
                LibUsbDotNet.Main.UsbRegDeviceList mRegDevices = UsbDevice.AllDevices;

                foreach (LibUsbDotNet.Main.UsbRegistry regDevice in mRegDevices)
                {
                    // add the Vid, Pid, and usb device description to the dropdown display.
                    // NOTE: There are many more properties available to provide you with more device information.
                    // See the LibUsbDotNet.Main.SPDRP enumeration.
                    string sItem = String.Format("Vid:{0} Pid:{1} {2}|{3}/n",
                                                 regDevice.Vid.ToString("X4"),
                                                 regDevice.Pid.ToString("X4"),
                                                 regDevice.FullName,
                                                 regDevice.Device.Info.ProductString
                                                 //  regDevice.Device.SymbolicName
                                                 );

                    //if (regDevice.Name == ("QuantaEye USB Trackball"))
                    if (regDevice.Name.Contains("QuantaEye"))
                        regDevice.Device.Open();
                    trackball_Reader = regDevice.Device.OpenEndpointReader((LibUsbDotNet.Main.ReadEndpointID)(1 | 0x80));
                    //  mEpReader = mUsbDevice.OpenEndpointReader((ReadEndpointID)(epNum | 0x80));
                    //mEpWriter = mUsbDevice.OpenEndpointWriter((WriteEndpointID)epNum);
                    trackball_Reader.DataReceived += trackball_DataReceived;
                    trackball_Reader.Flush();
                    trackball_Reader.DataReceivedEnabled = true;
                    //trackball_Reader.

                    //   foreach (var p in regDevice.DeviceProperties)
                    //      sItem += p.Key.ToString() + "---->" + p.Value.ToString() + "\n";
                    //   MessageBox.Show(sItem);
                }
                //tsNumDevices.Text = cboDevices.Items.Count.ToString();
            }
            catch (Exception e)
            { MessageBox.Show(e.Message); }
            //=======
            //           // cboDevices.Items.Clear();
            //            LibUsbDotNet.Main.UsbRegDeviceList mRegDevices = UsbDevice.AllDevices;
            //
            //            foreach (LibUsbDotNet.Main.UsbRegistry regDevice in mRegDevices)
            //            {
            //                // add the Vid, Pid, and usb device description to the dropdown display.
            //                // NOTE: There are many more properties available to provide you with more device information.
            //                // See the LibUsbDotNet.Main.SPDRP enumeration.
            //                string sItem = String.Format("Vid:{0} Pid:{1} {2}|{3}/n",
            //                                             regDevice.Vid.ToString("X4"),
            //                                             regDevice.Pid.ToString("X4"),
            //                                             regDevice.FullName,
            //                                             regDevice.Device.Info.ProductString
            //                                           //  regDevice.Device.SymbolicName
            //                                             );
            //            
            //                //if (regDevice.Name == ("QuantaEye USB Trackball"))
            //                if (regDevice.Name.Contains("QuantaEye"))
            //                        regDevice.Device.Open();
            //                    trackball_Reader = regDevice.Device.OpenEndpointReader((LibUsbDotNet.Main.ReadEndpointID)(1|0x80));
            //                //  mEpReader = mUsbDevice.OpenEndpointReader((ReadEndpointID)(epNum | 0x80));
            //                //mEpWriter = mUsbDevice.OpenEndpointWriter((WriteEndpointID)epNum);
            //                trackball_Reader.DataReceived += trackball_DataReceived;
            //                trackball_Reader.Flush();
            //                trackball_Reader.DataReceivedEnabled = true;
            //                //trackball_Reader.
            //
            //                //   foreach (var p in regDevice.DeviceProperties)
            //                //      sItem += p.Key.ToString() + "---->" + p.Value.ToString() + "\n";
            //                //   MessageBox.Show(sItem);
            //            }
            //            //tsNumDevices.Text = cboDevices.Items.Count.ToString();
            //>>>>>>> master
        }

        private void trackball_DataReceived(object sender, LibUsbDotNet.Main.EndpointDataEventArgs e)
        {
            Invoke(new OnDataReceivedDelegate(OnDataReceived), new object[] { sender, e });
        }

        private void OnDataReceived(object sender, LibUsbDotNet.Main.EndpointDataEventArgs e)
        {
            // s.Buffer.to
            // Windows.Storage.Streams.IBuffer buffer = inputReport.Data;
            hidstring = Encoding.UTF8.GetString(e.Buffer, 0, e.Count);
            //textBox4.Text = ((sbyte)e.Buffer[0]).ToString() + "|" + ((sbyte)e.Buffer[1]).ToString() + "|" + ((sbyte)e.Buffer[2]).ToString() + "||";
            //userControl21.X += (sbyte)e.Buffer[1];
            //userControl21.Y += (sbyte)e.Buffer[2];

            if (((sbyte)e.Buffer[0] & (sbyte)1) == (sbyte)1) DecreaseSensitivity();
            else if (((sbyte)e.Buffer[0] & (sbyte)2) == (sbyte)2) IncreaseSensitivity();
            else if (((sbyte)e.Buffer[0] & (sbyte)8) == (sbyte)8) SwitchToPrevious();
            else if (((sbyte)e.Buffer[0] & (sbyte)16) == (sbyte)16) SwitchToNext();

            //richTextBox1.AppendText(e.Buffer[0].ToString() + "|" + e.Buffer[1].ToString() + "\r");
            //richTextBox1.Text = e.Buffer[0].ToString() + "\r" + richTextBox1.Text;
            int deltax = (int)((double)((sbyte)e.Buffer[1]) / (11 - GetSelectedDCtrlVal()));
            int seltay = (int)((double)((sbyte)e.Buffer[2]) / (11 - GetSelectedDCtrlVal()));
            if (deltax != 0 && seltay != 0) ChangeSelectedCtrlVal(deltax, seltay);

            //GetSelectedCtrl2D().X += (int)((double)((sbyte)e.Buffer[1]) / (11 - GetSelectedDCtrlVal()));
            //GetSelectedCtrl2D().Y += (int)((double)((sbyte)e.Buffer[2]) / (11 - GetSelectedDCtrlVal()));

            // if (!backgroundWorker1.IsBusy)
            //   backgroundWorker1.RunWorkerAsync();
        }

        private void ChangeSelectedCtrlVal(int x, int y)
        {
            if (isAdvancedMode)
            {
                if (SelectedLightControl < 5)
                {
                    //UserControl1 sss = AdvGetSelectedCtrl2D();
                    //int ffffx = AdvGetSelectedCtrl2D().X;
                    //int ffffy = AdvGetSelectedCtrl2D().Y;
                    //Point ffffp = AdvGetSelectedCtrl2D().Value;
                    AdvGetSelectedCtrl2D().X += x;
                    AdvGetSelectedCtrl2D().Y += y;
                    AdvGetSelectedCtrl2D().FireChangesEvent();
                }
                else
                {
                    int val = AdvGetSelectedCtrl1D().Value;
                    val += x;
                    if (val > AdvGetSelectedCtrl1D().Maximum) val = AdvGetSelectedCtrl1D().Maximum;
                    if (val < AdvGetSelectedCtrl1D().Minimum) val = AdvGetSelectedCtrl1D().Minimum;
                    AdvGetSelectedCtrl1D().Value = val;
                }
            }
            else
            {
                if (SelectedLightControl < 2)
                {
                    int val = StdGetSelectedCtrl1D().Value;
                    val += x;
                    if (val > StdGetSelectedCtrl1D().Maximum) val = StdGetSelectedCtrl1D().Maximum;
                    if (val < StdGetSelectedCtrl1D().Minimum) val = StdGetSelectedCtrl1D().Minimum;
                    StdGetSelectedCtrl1D().Value = val;
                }
                else
                {
                    StdGetSelectedCtrl2D().X += x;
                    StdGetSelectedCtrl2D().Y += y;
                    StdGetSelectedCtrl2D().FireChangesEvent();
                }
            }
        }

        private void DecreaseSensitivity()
        {
            int val = GetSelectedDCtrlVal();
            if (val > 1) SetSelectedDCtrlVal(val - 1);
        }

        private void IncreaseSensitivity()
        {
            int val = GetSelectedDCtrlVal();
            if (val < 10) SetSelectedDCtrlVal(val + 1);
        }

        private int GetSelectedDCtrlVal()
        {
            int val = 0;

            if (isAdvancedMode)
                val = AdvGetSelectedDCtrl().Value;
            else
                val = StdGetSelectedDCtrl().Value;

            return val;
        }

        private void SetSelectedDCtrlVal(int val)
        {
            if (isAdvancedMode)
            {
                AdvGetSelectedDCtrl().Value = val;
            }
            else
            {
                StdGetSelectedDCtrl().Value = val;
            }
        }

        private TrackBar StdGetSelectedCtrl1D()
        {
            TrackBar Sel;

            if (SelectedLightControl == 0)
                Sel = Ctrl1D_Focus;
            else if (SelectedLightControl == 1)
                Sel = Ctrl1D_Zoom;
            else
                Sel = new TrackBar();

            return Sel;
        }

        private TrackBar AdvGetSelectedCtrl1D()
        {
            TrackBar Sel;

            if (SelectedLightControl == 5)
                Sel = new TrackBar();
            else
                Sel = new TrackBar();

            return Sel;
        }

        private UserControl1 AdvGetSelectedCtrl2D()
        {
            UserControl1 Sel;

            if (SelectedLightControl == 0)
                Sel = userControl11;
            else if (SelectedLightControl == 1)
                Sel = userControl12;
            else if (SelectedLightControl == 2)
                Sel = userControl14;
            else if (SelectedLightControl == 3)
                Sel = userControl15;
            else if (SelectedLightControl == 4)
                Sel = userControl13;
            else
                Sel = new UserControl1();

            return Sel;
        }

        private UserControl1 StdGetSelectedCtrl2D()
        {
            UserControl1 Sel;

            if (SelectedLightControl == 2)
                Sel = Ctrl2D_Stig;
            else if (SelectedLightControl == 3)
                Sel = Ctrl2D_Gain;
            else if (SelectedLightControl == 4)
                Sel = Ctrl2D_ObjectCentering;
            else if (SelectedLightControl == 5)
                Sel = Ctrl2D_IMLCentering;
            else if (SelectedLightControl == 6)
                Sel = Ctrl2D_GunShift;
            else if (SelectedLightControl == 7)
                Sel = Ctrl2D_GunTilt;
            else
                Sel = new UserControl1();

            return Sel;
        }

        private TrackBar StdGetSelectedDCtrl()
        {
            TrackBar Sel;
            if (SelectedLightControl == 0)
                Sel = dCtrl1D_Focus;
            else if (SelectedLightControl == 1)
                Sel = dCtrl1D_Zoom;
            else if (SelectedLightControl == 2)
                Sel = dCtrl2D_Stig;
            else if (SelectedLightControl == 3)
                Sel = dCtrl2D_Gain;
            else if (SelectedLightControl == 4)
                Sel = dCtrl2D_ObjectCentering;
            else if (SelectedLightControl == 5)
                Sel = dCtrl2D_IMLCentering;
            else if (SelectedLightControl == 6)
                Sel = dCtrl2D_GunShift;
            else if (SelectedLightControl == 7)
                Sel = dCtrl2D_GunTilt;
            else
                Sel = new TrackBar();

            return Sel;
        }

        private TrackBar AdvGetSelectedDCtrl()
        {
            TrackBar Sel;
            if (SelectedLightControl == 0)
                Sel = trackBar1;
            else if (SelectedLightControl == 1)
                Sel = trackBar2;
            else if (SelectedLightControl == 2)
                Sel = trackBar3;
            else if (SelectedLightControl == 3)
                Sel = trackBar4;
            else if (SelectedLightControl == 4)
                Sel = trackBar5;
            else
                Sel = new TrackBar();

            return Sel;
        }

        private delegate void OnDataReceivedDelegate(object sender, LibUsbDotNet.Main.EndpointDataEventArgs e);

        private void EnumerateHidDevices()
        {
            // Microsoft Input Configuration Device.
            ushort vendorId = 0x046D;
            ushort productId = 0xC408;
            ushort usagePage = 0x0001;
            ushort usageId = 0x0002;

            // Create the selector.
            string selector =
                HidDevice.GetDeviceSelector(usagePage, usageId, vendorId, productId);

            // Enumerate devices using the selector.
            //var devices = 
            // await DeviceInformation.FindAllAsync(selector);
            Windows.Foundation.IAsyncOperation<DeviceInformationCollection> aaaaa;
            aaaaa = Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(selector);
            while (aaaaa.Status != Windows.Foundation.AsyncStatus.Completed) ;
            var devices = aaaaa.GetResults();

            if (devices.Any())
            {
                // At this point the device is available to communicate with
                // So we can send/receive HID reports from it or 
                // query it for control descriptions.
                // info.Text = "HID devices found: " + devices.Count;

                // Open the target HID device.
                //
                String ID = devices.ElementAt(0).Id;
                Windows.Foundation.IAsyncOperation<HidDevice> async_hid;
                async_hid = HidDevice.FromIdAsync(ID, FileAccessMode.Read);
                while (async_hid.Status != Windows.Foundation.AsyncStatus.Completed) ;
                HidDevice device = async_hid.GetResults();
                // device.
                if (device != null)
                {
                    // Input reports contain data from the device.
                    device.InputReportReceived += new Windows.Foundation.TypedEventHandler<HidDevice, HidInputReportReceivedEventArgs>(hid_report);

                    //+=  (sender, args) =>
                    {
                        /*  HidInputReport inputReport = args.Report;
                          Windows.Storage.Streams.IBuffer buffer = inputReport.Data;*/

                        // Create a DispatchedHandler as we are interracting with the UI directly and the
                        // thread that this function is running on might not be the UI thread; 
                        // if a non-UI thread modifies the UI, an exception is thrown.

                        /* await this.Dispatcher.RunAsync(
                             CoreDispatcherPriority.Normal,
                             new DispatchedHandler(() =>
                             {
                                 info.Text += "\nHID Input Report: " + inputReport.ToString() +
                                 "\nTotal number of bytes received: " + buffer.Length.ToString();
                             }));*/
                    };
                }

            }
            else
            {
                // There were no HID devices that met the selector criteria.
                // info.Text = "HID device not found";
            }
        }
        private void hid_report(HidDevice device, HidInputReportReceivedEventArgs arg)
        {
            // HidInputReport inputReport = arg.Report;
            //  Windows.Storage.Streams.IBuffer buffer = inputReport.Data;
            //  hidstring = inputReport.ToString();
            //  if (!backgroundWorker1.IsBusy)
            //     backgroundWorker1.RunWorkerAsync();
        }
        //<<<<<<< master
        //            private void BtnStart_Click(object sender, EventArgs e)
        //        {
        //            dactimer(1);
        //=======

        private void BtnStart_Click(object sender, EventArgs e)
        {
            bool isOK = dactimer(1);
            if (!tcp.Connected)
                isUDPConnected = false;
            else
                isUDPConnected = isOK;

            //>>>>>>> master
            Play();
            //System.Threading.Thread myThread;
            //myThread = new System.Threading.Thread(new System.Threading.ThreadStart(Play));
            //myThread.Start();

            /*try
            {
                //_capture = new Capture();
                //_capture.ImageGrabbed += ProcessFrame; 
                //_capture.Start();
                Application.Idle += new EventHandler(ProcessFrame);
            }
            catch (NullReferenceException excpt)
            {
                MessageBox.Show(excpt.Message);
            }*/
            LabelUDPConnected.Visible = isUDPConnected;
            LabelUDPConnected.Update();
        }

        void Plot()
        {
            String win1 = "Test Window"; //The name of the window
            CvInvoke.cvNamedWindow(win1); //Create the window using the specific name
            Image<Bgr, Byte> img = new Image<Bgr, byte>(512, 512, new Bgr(0, 0, 0)); //Create an image of 400x200 of Blue color
            CvInvoke.cvShowImage(win1, img); //Show the image
            CvInvoke.cvWaitKey(0);  //Wait for the key pressing event
            CvInvoke.cvDestroyWindow(win1); //Destory the window
        }

        private void Play()
        {
            //LabelUDPConnected.Visible = true;
            //LabelUDPConnected.Update();
            //<<<<<<< master
            //          //  int nreceivedData = socket.Receive(Packet);
            //            socket.ReceiveBufferSize = (512 + 2)*512;
            //            socket.BeginReceive(Packet, 0, 512 + 2, SocketFlags.None, new AsyncCallback(RecieveComplete), socket);
            //=======
            //  int nreceivedData = socket.Receive(Packet);
            socket.ReceiveBufferSize = (512 + 2) * 512;
            try
            {
                socket.BeginReceive(Packet, 0, 512 + 2, SocketFlags.None, new AsyncCallback(RecieveComplete), socket);
            }
            catch
            {

            }
            //>>>>>>> master
            timer1.Start();

            //     Application.Idle += new EventHandler(ProcessFrame);
        }
        public void RecieveComplete(IAsyncResult result)
        {
            try
            {
                //    if (  >= 514)
                {
                    socket.EndReceive(result);
                    ;
                    row = (Int16)(Packet[nX + 1] | (Packet[nX] << 8));

                    //int nxm = nX/ multiply;
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
                    /*        int imatx = 0;
                            int imaty = 0;
                            if (SingleShotMode)
                            {
                                for (int ic = 0; ic < 512; ic++)
                                {
                                    if (MultiShotMode)
                                    {
                                        int nm = (int)numericUpDown2.Value;
                                        imatx = ((iMultiScan - 1) % nm);
                                        imaty = (int)((iMultiScan - 1 - imatx ) / nm);
                                        if (SingleShotMode) myMatrix[SSPacketCnt + imatx * SingleShotnStep, SSRow + imaty * SingleShotnStep] = Packet[ic];
                                    }
                                    else
                                    {
                                        if (SingleShotMode) myMatrix[SSPacketCnt, SSRow] = Packet[ic];
                                    }
                                    SSPacketCnt++;
                                    if (SSPacketCnt > SingleShotnStep - 1)
                                    {
                                        SSPacketCnt = 0;
                                        SSRow++;

                                        if (SSRow > SingleShotnStep - 1)
                                        {
                                            SingleShotMode = false;
                                            if (!MultiShotMode)
                                            {
                                                //ChangeSpeed(oldspeedVal);
                                            }
                                            else
                                            {
                                                if (iMultiScan == MultiShotnStep)
                                                {
                                                    MultiShotMode = false;
                                                    //ChangeSpeed(oldspeedVal);
                                                }
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                            */
                    // int pos = (row + iY) * 512  + iX;// + nX * multiply_count;// * multiply;// + nX* multiply_count;
                    // int pos = row + nX * multiply_count;
                    /*   for(int i=0;i<multiply;i++)
                       {

                           sum += Packet[i];
                       }
                    for (int j = 0; j < image_size; j++)
                    {
                        sum = 0;
                        for (int i = 0; i < multiply; i++)
                        {
                            sum += receivedData[j + i];
                            frame.Bytes[j] = (byte)(sum / multiply);
                        }    
                    }*/
                    //sum = 0;
                    //int j = 0;
                    /*   if (nX < 500)
                           sum=0;
                       for (int i = 0; i < (nX / multiply); i++)
                       {
                           sum = 0;
                           for (int j = 0; j < multiply; j++)
                               sum += Packet[j+i*multiply];
                           receivedData[pos + i] = (byte)(sum / multiply);
                       }*/
                    // for(int i;i<nX;i++)
                    Buffer.BlockCopy(Packet, 0, receivedData, package_number * 512, nX);
                    window_row[package_number] = row;
                    if (multiply_count == (multiply - 1)) line_ready = multiply;
                    if (line_ready > 0)
                    {
                        int num = 0;
                        int linestart = (package_number - line_ready + 1);// * 512;
                        if (linestart < 0) linestart += 65536;// 33554432;
                        int pos = (row + iY) * 512 + iX;
                        //sum = 0;
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
                    //window_nX[package_number] = nX; 
                    /*       if (isGunAlignment)
                           {
                               LastIntensity = 0;
                               for (int i = 0; i < nX; i++) LastIntensity += receivedData[pos + i];
                               LastIntensity = LastIntensity / ((double)nX);
                           }*/
                    //    ready = 1;
                    /*    if (isAcquire)
                        { 
                            if (row == 512 - 1) AcquireCnt++;
                            if (AcquireCnt == (int)UD_AcquireNumber.Value)
                            {
                                button3_Click(null, null);
                            }
                        }*/
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
            //LabelUDPConnected.Visible = true;
            //LabelUDPConnected.Update();
            //timer1.Stop();
            isUDPConnected = false;
            //   Application.Idle -= new EventHandler(ProcessFrame);
        }

        private void ProcessFrame1(object sender, EventArgs e)
        {
            //using (Image<Bgr, Byte> image = _capture.RetrieveBgrFrame())
            using (MemStorage storage = new MemStorage()) //create storage for motion components
            {
                count = count + 1;
                overallMotionPixelCount = overallMotionPixelCount + 0.1;
                //Display the amount of motions found on the current image
                UpdateText(String.Format("Total Motions found: {0}; Motion Pixel count: {1}", count, overallMotionPixelCount));

                //Display the image of the motion
                //motionImageBox.Image = motionImage;
            }
        }

        private void ProcessFrame(object sender, EventArgs arg)
        {
            try
            {

                //   if (ready == 0)
                //   return;
                //   ready = 0;
                //socket.Send(new byte[1]);
                //receivedData = udp.Receive(ref EIP);
                // watch.Stop();

                //  var watch = System.Diagnostics.Stopwatch.StartNew();

                /*    for (int block = 0; block < nRow; block++)
                    {
                        int nreceivedData = socket.Receive(Packet);
                        Int16 row = (Int16)(Packet[513] | (Packet[512] << 8));
                        Buffer.BlockCopy(Packet, 0, receivedData, row * nY, 512);
                        //int nreceivedData = socket.Receive(receivedData, count * nX * nY, nX * nY, SocketFlags.None);
                        //count = count + 1;
                        //if (count > 511) count = 0;
                    }*/
                frame.Bytes = receivedData;
                //   ViewPort.Image = frame;
                //Buffer.BlockCopy(receivedData, 0, frame.Bytes, count * nX * nY, nreceivedData);
                //count = count + 1;
                overallMotionPixelCount = overallMotionPixelCount + 1;

                //UpdateText(overallMotionPixelCount.ToString());
                //if (nX * count > 511) count = 0;
                //Image<Bgr, Byte> frame = _capture.RetrieveBgrFrame();
                //for (int i = 0; i < 512; i++) for (int j = 0; j < 512; j++) frame[i, j] = new Bgr(255 - 10 * count, count + 10 * Math.Sin(0.2 * i + 3.3 * count), count - 10 * Math.Sin(0.3 * j - 3.3 * count));

                /*for (int ix = 0; ix < nX; ix++)
                {
                    for (int iy = 0; iy < nY; iy++)
                    {
                        byte clr = receivedData[ix + nX * iy];
                        frame[x, iy] = new Bgr(clr, clr, clr);
                    }
                    x = x + 1;
                    if (x > 511) x = 0;
                }*/
                //frame = new Image<Gray, byte>(512, 512, new Gray(count));



                if (overallMotionPixelCount == 1)
                {
                    overallMotionPixelCount = -10;
                    long n = watch.ElapsedMilliseconds;
                    if (n == 0)
                        FPS = 1000;
                    else
                        FPS = 1000.0 / n;
                }

                if (StartMove)
                {
                    frame.DrawPolyline(SelectionRec, true, new Gray(255), 1);
                    frame.DrawPolyline(SelectionRec2, true, new Gray(1), 1);
                }
                // string info = String.Format("{0:0.0}FPS", FPS);
                frame.Draw("hiiiiii", ref format, new System.Drawing.Point(100, 100), new Gray(200)); //Draw on the image using the specific font
                
                ViewPort.Image = SetFilter(frame);
                // watch.Reset(); watch.Start();
                //UpdateText(info);
            }
            catch (Exception e)
            {
                Stop();
                log.Text = e.Message + "\r" + log.Text;
                // MessageBox.Show(e.Message);
            }
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
        private void UpdateText(String text)
        {
            if (InvokeRequired && !IsDisposed)
            {
                Invoke((Action<String>)UpdateText, text);
            }
            else
            {
                label1.Text = text;
            }
        }

        private void ChangeSpeed(Decimal val)
        {
            if (InvokeRequired && !IsDisposed)
            {
                Invoke((Action<Decimal>)ChangeSpeed, val);
            }
            else
            {
                UD_Speed.Value = val;
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            try
            {
                MCvFont f = new MCvFont(FONT.CV_FONT_HERSHEY_COMPLEX, 0.4, .4);
                //ViewPort.Size = new System.Drawing.Size(700, 700);
                //string info = String.Format("Quanta");
                //frame.Draw(info, ref f, new System.Drawing.Point(227, 247), new Gray(200)); //Draw on the image using a specific font
                Image<Rgb, byte> frame0 = new Image<Rgb, byte>(new Bitmap(Properties.Resources.empty512));
                frame = frame0.Convert<Gray, Byte>();
                
                ViewPort.Image = SetFilter(frame);//.PyrUp();

                ViewPort.SetZoomScale(1.1, Point.Empty);

                //frame.DrawPolyline()
            }
            catch (NullReferenceException ex)
            {
                // MessageBox.Show(excpt.Message);
                log.Text = ex.Message + "\r" + log.Text;
            }
        }

        void InitializeUDP()
        {
            //udp = new UdpClient(EIP);
            //udp.Client.ReceiveBufferSize = 65000;
            try
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                socket.ReceiveTimeout = 200;
                socket.Bind(EIP);
            }
            catch (Exception e)
            {
                if (isAdmin)
                    MessageBox.Show("Warning: It seems that the LAN cable is currently not plugged in.\r\rFunction: InitializeUDP()");
                else
                    MessageBox.Show("Warning: It seems that the LAN cable is currently not plugged in.\r");
            }
            //socket.BeginReceive(new AsyncCallback(ReceiveCallback), null);
            //socket.Connect(EIP);
            //udp.BeginReceive(new AsyncCallback(ReceiveCallback),null);
        }

        void InitializeTCP()
        {
            tcp = new TcpClient();
            tcp.ReceiveTimeout = 1000;
            tcp.ReceiveBufferSize = 100;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            nRow = Convert.ToInt32(numericUpDown1.Value);
        }

        private void terminalHistory_MouseEnter(object sender, EventArgs e)
        {
            History.Size = new Size(History.Size.Width, 160);
        }

        private void terminalHistory_MouseLeave(object sender, EventArgs e)
        {
            History.Size = new Size(History.Size.Width, 17);
        }

        private void terminalHistory_Leave(object sender, EventArgs e)
        {
            History.Size = new Size(History.Size.Width, 17);
        }

        private void terminalHistory_Click(object sender, EventArgs e)
        {
            History.Size = new Size(History.Size.Width, 160);
            if (History.SelectedIndex > -1) TBOrder.Text = History.Items[History.SelectedIndex].ToString();
        }

        private void TBOrder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter && e.KeyCode != Keys.Up && e.KeyCode != Keys.Down) return;
            if (e.KeyCode != Keys.Enter)
            {
                if (e.KeyCode == Keys.Up)
                    HistoryIndex++;
                else
                    HistoryIndex--;
                if (HistoryIndex >= History.Items.Count) HistoryIndex = History.Items.Count - 1;
                if (HistoryIndex < 0) HistoryIndex = 0;
                TBOrder.Text = History.Items[HistoryIndex].ToString();
                return;
            }

            string order = TBOrder.Text;

            string[] parts = order.Split('.');
            bool isChild = false;
            if (parts.Length == 2) isChild = true;
            if (isChild) order = CreateChildCommand(parts[0], parts[1]);

            if (order == "") return;
            if (isChild)
                History.Items.Insert(1, TBOrder.Text);
            else
                History.Items.Insert(1, order);

            HistoryIndex = 0;
            string CompleteOrder = order;

            if (RB2.Checked) CompleteOrder = order + "\r";
            if (RB3.Checked) CompleteOrder = order + "\n";

            if (isChild)
                TBOutput.Text = TBOutput.Text + TBOrder.Text + "\n" + ">>> ";
            else
                TBOutput.Text = TBOutput.Text + order + "\n" + ">>> ";

            ScrollToEnd();
            TBOrder.Text = "";

            if (!CompleteOrder.StartsWith("COM"))
            {
                try
                {
                    //Form1.Port.DiscardOutBuffer(); //Clear Buffer
                    //Form1.Port.DiscardInBuffer(); //Clear Buffer
                    //Form1.Port.Write(CompleteOrder);
                    byte[] buffer = PrepareBuffer(CompleteOrder);
                    TCPnetworkStream.Write(buffer, 0, nMaxCharacters);
                    Thread.Sleep(50);
                    isDataReceived = true;
                    isDirectCOMPortComunication = false;
                    DirectCOMPortIndex = -1;
                }
                catch { }
            }
            else
            {
                //try to connect to com ports
                string[] Parts;
                char[] delimiterChars = { '.' };
                Parts = CompleteOrder.Split(delimiterChars);
                string COMName = Parts[0];
                string DeviceName = Parts[1];
                string Command = Parts[2];
                //Command = DeviceName + "." + Command;

                int index = -1;
                foreach (SerialPort p in ComPorts)
                {
                    index++;
                    if (COMName == p.PortName) break;
                }

                ComPorts[index].DiscardInBuffer();
                ComPorts[index].DiscardOutBuffer();
                ComPorts[index].Write(Command);
                Thread.Sleep(50);
                isDataReceived = true;
                isDirectCOMPortComunication = true;
                DirectCOMPortIndex = index;
            }

            TotalTime = 0;
            TerminalTimerReceiver.Start();
        }

        private byte[] PrepareBuffer(string CompleteOrder)
        {
            byte[] buffer = new byte[nMaxCharacters];
            byte[] order = Encoding.ASCII.GetBytes(CompleteOrder);
            byte[] dot = Encoding.ASCII.GetBytes(".");
            for (int i = 0; i < order.Length; i++) buffer[i] = order[i];
            for (int i = order.Length; i < nMaxCharacters; i++) buffer[i] = dot[0];
            return buffer;
        }

        private void ScrollToEnd()
        {
            TBOutput.SelectionStart = TBOutput.Text.Length;
            TBOutput.ScrollToCaret();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            TBOutput.Text = ">>> ";
        }

        private void History_DoubleClick(object sender, EventArgs e)
        {
            if (History.SelectedIndex > -1)
            {
                TBOrder.Text = History.Items[History.SelectedIndex].ToString();

                string order = TBOrder.Text;

                string[] parts = order.Split('.');
                bool isChild = false;
                if (parts.Length == 2) isChild = true;
                if (isChild) order = CreateChildCommand(parts[0], parts[1]);

                HistoryIndex = 0;
                string CompleteOrder = order;

                if (RB2.Checked) CompleteOrder = order + "\r";
                if (RB3.Checked) CompleteOrder = order + "\n";

                if (isChild)
                    TBOutput.Text = TBOutput.Text + TBOrder.Text + "\n" + ">>> ";
                else
                    TBOutput.Text = TBOutput.Text + order + "\n" + ">>> ";

                ScrollToEnd();
                TBOrder.Text = "";

                if (!CompleteOrder.StartsWith("COM"))
                {
                    try
                    {
                        //Form1.Port.DiscardOutBuffer(); //Clear Buffer
                        //Form1.Port.DiscardInBuffer(); //Clear Buffer
                        byte[] buffer = PrepareBuffer(CompleteOrder);
                        TCPnetworkStream.Write(buffer, 0, nMaxCharacters);
                        Thread.Sleep(50);
                        isDataReceived = true;
                    }
                    catch { }
                }
                else
                {
                    //try to connect to com ports
                    string[] Parts;
                    char[] delimiterChars = { '.' };
                    Parts = CompleteOrder.Split(delimiterChars);
                    string COMName = Parts[0];
                    string DeviceName = Parts[1];
                    string Command = Parts[2];
                    Command = DeviceName + "." + Command;

                    int index = -1;
                    foreach (SerialPort p in ComPorts)
                    {
                        index++;
                        if (COMName == p.PortName) break;
                    }

                    ComPorts[index].DiscardInBuffer();
                    ComPorts[index].DiscardOutBuffer();
                    ComPorts[index].Write(Command);
                    Thread.Sleep(50);
                    isDataReceived = true;
                    isDirectCOMPortComunication = true;
                    DirectCOMPortIndex = index;
                }

                TotalTime = 0;
                History.Size = new Size(History.Size.Width, 17);
                TerminalTimerReceiver.Start();
            }
        }

        private void TerminalTimerReceiver_Tick(object sender, EventArgs e)
        {
            if (isAllowToTick)
            {
                isAllowToTick = false;
                TotalTime = TotalTime + TerminalTimerReceiver.Interval;
                if (TotalTime > WaitTime * 1000)
                {
                    TerminalTimerReceiver.Stop();
                    isAllowToTick = true;
                    return;
                }

                if (isDataReceived)
                {
                    isDataReceived = false;

                    if (!isDirectCOMPortComunication)
                    {
                        try
                        {
                            //while (Form1.Port.BytesToRead != 0)
                            //{
                            byte[] buffer = new byte[tcp.ReceiveBufferSize];
                            int bytesRead = TCPnetworkStream.Read(buffer, 0, tcp.ReceiveBufferSize);
                            //TCPnetworkStream.rec
                            String OutputBuffer = Encoding.ASCII.GetString(buffer);
                            string[] outputs = OutputBuffer.Split('\r');
                            string output = outputs[0];
                            TBOutput.Text = TBOutput.Text + output;
                            //}

                            TBOutput.Text = TBOutput.Text + "\n" + ">>> ";
                            ScrollToEnd();
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        try
                        {
                            String OutputBuffer = ComPorts[DirectCOMPortIndex].ReadTo(ReadToChar);
                            string[] outputs = OutputBuffer.Split('\r');
                            string output = outputs[0];
                            TBOutput.Text = TBOutput.Text + output;
                            //}

                            TBOutput.Text = TBOutput.Text + "\n" + ">>> ";
                            ScrollToEnd();
                        }
                        catch
                        { }
                    }
                    TerminalTimerReceiver.Stop();
                }

                isAllowToTick = true;
            }
        }

        private void TBWaitTime_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int s = Convert.ToInt32(TBWaitTime.Text);
                if (s > 0)
                    WaitTime = s;
                else
                    TBWaitTime.Text = WaitTime.ToString();
            }
            catch
            {
                TBWaitTime.Text = WaitTime.ToString();
            }
        }

        private void Btn_TCPConnect_Click(object sender, EventArgs e)
        {
            try
            {
                InitializeTCP();
                tcp.Client.Blocking = true;
                tcp.SendTimeout = 1000; tcp.ReceiveTimeout = 1000;
                tcp.Connect(ServerHost, ControllerPort);
                tcp.Client.Blocking = true;
                TCPnetworkStream = tcp.GetStream();
                TCPnetworkStream.ReadTimeout = 5000;
                TCPnetworkStream.WriteTimeout = 5000;
                if (tcp.Connected)
                {
                    LabelTCPConnected.Visible = true;
                    UpdateScanner();
                    UpdateDetector();
                    rotate(4095, 0);
                    lens_gau(2047, 2047);
                    lens_gad(2047, 2047);
                    lens_ic(0, 0);
                    lens_stig(2047, 2047);
                    //u2itmode(1);
                    //u6itmode(1);
                }
            }
            catch (Exception ex)
            {
                //notifyIcon1.ShowBalloonTip(2000, "Error", ex.Message, ToolTipIcon.Error);
                log.Text = ex.Message + "\r" + log.Text;
                // MessageBox.Show(ex.Message);
            }
        }

        private void Btn_TCPDisconnect_Click(object sender, EventArgs e)
        {
            //<<<<<<< master
            //            try
            //            {
            //                //tcp.Close();
            //                tcp.Client.Disconnect(true);
            //                LabelTCPConnected.Visible = false;
            //            }
            //            catch (Exception ex)
            //            {
            //                //notifyIcon1.ShowBalloonTip(2000, "Error", ex.Message, ToolTipIcon.Error);
            //                log.Text = ex.Message + "\r" + log.Text;
            //               // MessageBox.Show(ex.Message);
            //=======
            if (tcp != null)
            {
                try
                {
                    //tcp.Close();
                    tcp.Client.Disconnect(true);
                    LabelTCPConnected.Visible = false;
                }
                catch (Exception ex)
                {
                    //notifyIcon1.ShowBalloonTip(2000, "Error", ex.Message, ToolTipIcon.Error);
                    log.Text = ex.Message + "\r" + log.Text;
                    // MessageBox.Show(ex.Message);
                }
            }
            else
            {
                log.Text = "LAN is not connected ...";
                //>>>>>>> master
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == "semadmin001")
            {
                isAdmin = true;
                AdminLoginLabel.Visible = true;
                logoutbtn.Enabled = true;
                textBox2.Text = "";
            }
        }

        private void logoutbtn_Click(object sender, EventArgs e)
        {
            isAdmin = false;
            AdminLoginLabel.Visible = false;
            logoutbtn.Enabled = false;
        }

        private void UDSpeed_ValueChanged(object sender, EventArgs e)
        {
            dacper(UD_Speed.Value);
        }

        private void UD_dSpeed_ValueChanged(object sender, EventArgs e)
        {
            UD_Speed.Increment = UD_dSpeed.Value;
        }

        private bool dacper(decimal period)
        {
            string CompleteOrder = "dacper " + period.ToString() + " " + (((speedper.Value) * period) / 100).ToString() + "\r";
            label_rate.Text = "rate: " + ((double)108000000 / (double)(period + 1) / nX / nY).ToString("0.00000") + " / line rate: " + ((double)108000000 / (double)(period + 1) / nX).ToString("0.00000");
            return SendAndReceiveOK(CompleteOrder);
        }

        internal bool SendAndReceiveOK(string CompleteOrder)
        {
            bool isOK = false;
            //<<<<<<< master
            //            // return true; //uncomment for offline test //return
            //=======
            //return true; //uncomment for offline test //return
            if (!tcp.Connected) return true;
            //>>>>>>> master

            if (!CompleteOrder.StartsWith("COM"))
            {
                try
                {

                    byte[] buffer = PrepareBuffer(CompleteOrder);
                    //Thread.Sleep(1);
                    TCPnetworkStream.Write(buffer, 0, nMaxCharacters);
                    byte[] buffer2 = new byte[tcp.ReceiveBufferSize];
                    //Thread.Sleep(1);
                    // tcp.Client.Blocking = true;
                    //TCPnetworkStream.ReadTimeout = 5000;
                    int bytesRead = TCPnetworkStream.Read(buffer2, 0, tcp.ReceiveBufferSize);
                    String command = Encoding.ASCII.GetString(buffer2);
                    string[] outputs = command.Split('\r');
                    string output = outputs[0];
                    if ((output == "OK") || (output.StartsWith("sisel")))
                        isOK = true;
                    else
                    {
                        if (isAdmin)
                            //notifyIcon1.ShowBalloonTip(2000,"Function: SendAndReceiveOK(string CompleteOrder)",output ,ToolTipIcon.Error);
                            log.Text = "SendAndReceiveOK:\r" + output + "\r" + log.Text;
                        //  MessageBox.Show("Error: Controller connection is not correctly connected.\rAdmin Error: The 'ok' command is not received.\r" + output + "\r\r" + "Function: SendAndReceiveOK(string CompleteOrder)");
                        else
                            MessageBox.Show("Error: Bad command has been sent to device.");
                    }
                }
                catch (Exception e)
                {

                    if (isAdmin)
                        //notifyIcon1.ShowBalloonTip(2000,"Function: SendAndReceiveOK(string CompleteOrder)",e.Message ,ToolTipIcon.Error);
                        log.Text = "SendAndReceiveOK:\r" + e.Message + "\r" + log.Text;
                    //MessageBox.Show("Error: Controller connection is not correctly connected.\rAdmin Error: The 'ok' and 'er' command are not received.\r" + e.Message + "\r\r" + "Function: SendAndReceiveOK(string CompleteOrder)");
                    else
                        MessageBox.Show("Error: Controller connection is not correctly connected.");
                }
            }
            else
            {
                //try to connect to com ports
                string[] Parts;
                char[] delimiterChars = { '.' };
                Parts = CompleteOrder.Split(delimiterChars);
                string COMName = Parts[0];
                string DeviceName = Parts[1];
                string Command = Parts[2];
                // Command = DeviceName + "." + Command;

                int index = -1;
                foreach (SerialPort p in ComPorts)
                {
                    index++;
                    if (COMName == p.PortName) break;
                }

                ComPorts[index].DiscardInBuffer();
                ComPorts[index].DiscardOutBuffer();
                ComPorts[index].Write(Command);
                string ans = ComPorts[index].ReadTo(ReadToChar);
                if (ans == "OK")
                    isOK = true;
                else
                {
                    if (isAdmin)
                        //  notifyIcon1.ShowBalloonTip(2000,"Function: SendAndReceiveOK(string CompleteOrder)",ans ,ToolTipIcon.Error);
                        log.Text = "SendAndReceiveOK:\r" + ans + "\r" + log.Text;
                    //MessageBox.Show("Error: Controller connection is not correctly connected.\rAdmin Error: The 'ok' command is not received.\r" + ans + "\r\r" + "Function: SendAndReceiveOK(string CompleteOrder)");
                    else
                        //  notifyIcon1.ShowBalloonTip(2000, "Error","Bad command has been sent to device.", ToolTipIcon.Error);
                        MessageBox.Show("Error: Bad command has been sent to device.");
                }
            }

            return isOK;
        }

        private string SendAndReceiveResponse(string CompleteOrder)
        {
            if (!CompleteOrder.StartsWith("COM"))
            {
                //<<<<<<< master
                //=======
                if (TCPnetworkStream == null) return "null";

                //>>>>>>> master
                try
                {
                    byte[] buffer = PrepareBuffer(CompleteOrder);
                    TCPnetworkStream.Write(buffer, 0, nMaxCharacters);
                    byte[] buffer2 = new byte[tcp.ReceiveBufferSize];
                    int bytesRead = TCPnetworkStream.Read(buffer2, 0, tcp.ReceiveBufferSize);
                    String command = Encoding.ASCII.GetString(buffer2);
                    string[] outputs = command.Split('\r');
                    return outputs[0];
                }
                catch (Exception e)
                {
                    if (isAdmin)
                        //notifyIcon1.ShowBalloonTip(2000,"Admin Error",e.Message + "\r\r" + "Function: SendAndReceiveOK(string CompleteOrder)",ToolTipIcon.Error);
                        log.Text = "SendAndReceiveResponse:\r" + e.Message + "\r" + log.Text;
                    //MessageBox.Show("Admin Error: The 'ok' and 'er' command are not received.\r" + e.Message + "\r\r" + "Function: SendAndReceiveOK(string CompleteOrder)");
                    else
                        //notifyIcon1.ShowBalloonTip(2000, "Error", e.Message , ToolTipIcon.Error);
                        MessageBox.Show("Error: Controller connection is not correctly connected.");
                    return "null";
                }
            }
            else
            {
                //try to connect to com ports
                string[] Parts;
                char[] delimiterChars = { '.' };
                Parts = CompleteOrder.Split(delimiterChars);
                string COMName = Parts[0];
                string DeviceName = Parts[1];
                string Command = Parts[2];
                //Command = DeviceName + "." + Command;

                int index = -1;
                foreach (SerialPort p in ComPorts)
                {
                    index++;
                    if (COMName == p.PortName) break;
                }

                ComPorts[index].DiscardInBuffer();
                ComPorts[index].DiscardOutBuffer();
                ComPorts[index].Write(Command);
                string ans = ComPorts[index].ReadTo(ReadToChar);
                return ans;
            }
        }

        private int SelectedDetectorPort()
        {
            if (RB_Det_Port1.Checked) return 0;
            if (RB_Det_Port2.Checked) return 1;
            if (RB_Det_Port3.Checked) return 2;
            if (RB_Det_Port4.Checked) return 3;
            return 0;
        }

        private int SelectedGainType()
        {
            if (RB_Direct.Checked) return 0;
            if (RB_MCP.Checked) return 1;
            if (RB_ADG.Checked) return 2;
            return 1;
        }

        private bool setsignal(int DetectorPort, int GainType, int Terminal, decimal GainValue)
        {
            string CompleteOrder = "setsignal " + DetectorPort.ToString() + " " + GainType.ToString() + " " + Terminal.ToString() + " " + GainValue.ToString() + "\r";
            return SendAndReceiveOK(CompleteOrder);
        }

        private void UpdateDetector()
        {
            int DetectorPort = SelectedDetectorPort();
            int GainType = SelectedGainType();
            int MCP_Terminal = CB_MCP_Terminal.SelectedIndex;
            int ADG_Terminal = CB_ADG_Terminal.SelectedIndex;
            decimal GainValue = CB_MCP_Gain.SelectedIndex;
            if (GainType == 0) setsignal(DetectorPort, GainType, 0, 0);
            else if (GainType == 1) setsignal(DetectorPort, GainType, MCP_Terminal, GainValue);
            else if (GainType == 2) setsignal(DetectorPort, GainType, ADG_Terminal, 0);
            int coarse = (int)UD_DetectorTrim_Coarse.Value;
            int Fine = (int)UD_DetectorTrim_Fine.Value;
            if (RB_Det_Port1.Checked)
            {
                dtrim(0, coarse);
                dtrim(1, Fine);
            }
            else if (RB_Det_Port2.Checked)
            {
                dtrim(2, coarse);
                dtrim(3, Fine);
            }
            else if (RB_Det_Port3.Checked)
            {
                dtrim(4, coarse);
                dtrim(5, Fine);
            }
            else if (RB_Det_Port4.Checked)
            {
                dtrim(6, coarse);
                dtrim(7, Fine);
            }
        }

        private void RB_Det_Port1_CheckedChanged(object sender, EventArgs e)
        {
            if (RB_Det_Port1.Checked) UpdateDetector();
        }

        private void RB_Det_Port2_CheckedChanged(object sender, EventArgs e)
        {
            if (RB_Det_Port2.Checked) UpdateDetector();
        }

        private void RB_Det_Port3_CheckedChanged(object sender, EventArgs e)
        {
            if (RB_Det_Port3.Checked) UpdateDetector();
        }

        private void RB_Det_Port4_CheckedChanged(object sender, EventArgs e)
        {
            if (RB_Det_Port4.Checked) UpdateDetector();
        }

        private void RB_Direct_CheckedChanged(object sender, EventArgs e)
        {
            if (RB_Direct.Checked)
            {
                CB_ADG_Terminal.Enabled = false;
                CB_MCP_Terminal.Enabled = false;
                CB_MCP_Gain.Enabled = false;
                UpdateDetector();
            }
        }

        private void RB_MCP_CheckedChanged(object sender, EventArgs e)
        {
            if (RB_MCP.Checked)
            {
                CB_ADG_Terminal.Enabled = false;
                CB_MCP_Terminal.Enabled = true;
                CB_MCP_Gain.Enabled = true;
                UpdateDetector();
            }
        }

        private void RB_ADG_CheckedChanged(object sender, EventArgs e)
        {
            if (RB_ADG.Checked)
            {
                CB_ADG_Terminal.Enabled = true;
                CB_MCP_Terminal.Enabled = false;
                CB_MCP_Gain.Enabled = false;
                UpdateDetector();
            }
        }

        private void CB_Terminal_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDetector();
        }

        private void UD_GainValue_ValueChanged(object sender, EventArgs e)
        {
            UpdateDetector();
        }

        private void CB_ADG_Terminal_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDetector();
        }

        private void CB_MCP_Gain_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDetector();
        }

        private void UD_dRotation_ValueChanged(object sender, EventArgs e)
        {
            UD_Rotation.Increment = UD_dRotation.Value;
        }

        private bool rotate(int a00, int a10)
        {
            //Rotation Matrix=[a00 a10
            //                 a01 a11];
            //a11=a00
            //and
            //a01=-a10 but we made it in circut not in code
            //So a01=a10
            string CompleteOrder = "rotate " + a00.ToString() + " " + a10.ToString() + "\r";
            return SendAndReceiveOK(CompleteOrder);
        }

        private void UD_Rotation_ValueChanged(object sender, EventArgs e)
        {
            Decimal Dtheta = UD_Rotation.Value;
            Double theta = Decimal.ToDouble(Dtheta) * Math.Acos(-1.0) / 180.0;
            int a00 = (int)Math.Round(4095 * Math.Cos(theta));
            int a10 = (int)Math.Round(4095 * Math.Sin(theta));
            rotate(a00, a10);
        }

        private bool zoom(decimal cx, decimal cy)
        {
            string CompleteOrder = "zoom " + cx.ToString() + " " + cy.ToString() + "\r";
            return SendAndReceiveOK(CompleteOrder);
        }

        private void UD_dZoom_ValueChanged(object sender, EventArgs e)
        {
            UD_Zoom.Increment = UD_dZoom.Value;
        }

        private void UD_Zoom_ValueChanged(object sender, EventArgs e)
        {
            decimal c = UD_Zoom.Value;
            zoom(c, c);
        }

        private bool UpdateDacxRange()
        {
            decimal dacxmeanval = UD_DacxMeanVal.Value;
            decimal dacxamp = UD_DacxAmp.Value;
            string CompleteOrder = "dacxrange " + dacxmeanval.ToString() + " " + dacxamp.ToString() + "\r";
            return SendAndReceiveOK(CompleteOrder);
        }

        private bool UpdateDacyRange()
        {
            decimal dacymeanval = UD_DacyMeanVal.Value;
            decimal dacyamp = UD_DacyAmp.Value;
            string CompleteOrder = "dacyrange " + dacymeanval.ToString() + " " + dacyamp.ToString() + "\r";
            return SendAndReceiveOK(CompleteOrder);
        }

        private void UD_DacxMeanVal_ValueChanged(object sender, EventArgs e)
        {
            UpdateDacxRange();
        }

        private void UD_DacxAmp_ValueChanged(object sender, EventArgs e)
        {
            UpdateDacxRange();
        }

        private void UD_DacyMeanVal_ValueChanged(object sender, EventArgs e)
        {
            UpdateDacyRange();
        }

        private void UD_DacyAmp_ValueChanged(object sender, EventArgs e)
        {
            UpdateDacyRange();
        }

        private bool Load_Devices()
        {
            bool isOK = false;
            try
            {
                DevicesCheckList.Items.Clear();
                string[] lines = System.IO.File.ReadAllLines(@".\devices.dat");

                NumberOfDevices = 0;
                foreach (string line in lines)
                {
                    String s = line;
                    string[] parts = s.Split(' ');
                    if (parts.Length == 2)
                    {
                        string device_name = parts[0];
                        string port_name = parts[1];
                        if (device_name != " " && device_name != "" && port_name != " " && port_name != "")
                        {
                            if (port_name == "null")
                                DevicesCheckList.Items.Add("device ID:  " + device_name + "\t\tport name:  " + port_name + "\t\tNot Configured");
                            else
                                DevicesCheckList.Items.Add("device ID:  " + device_name + "\t\tport name:  " + port_name + "\t\tConfigured");


                            Device_names[NumberOfDevices] = device_name;
                            Port_names[NumberOfDevices] = port_name;
                            NumberOfDevices++;
                            isOK = true;
                        }
                    }
                }
            }
            catch { }

            return isOK;
        }

        private bool Save_Devices()
        {
            bool isOK = false;
            try
            {
                if (NumberOfDevices > 0)
                {
                    string[] lines = new string[NumberOfDevices];

                    for (int i = 0; i < NumberOfDevices; i++)
                    {
                        lines[i] = Device_names[i] + " " + Port_names[i];
                    }
                    System.IO.File.WriteAllLines(@".\devices.dat", lines);
                    isOK = true;
                }
            }
            catch { }

            return isOK;
        }

        private void Add_Device(string Device_name, string Port_name)
        {
            Devices_Load_Click(null, null);
            if (NumberOfDevices < MaximumNumberOfDevices)
            {
                Device_names[NumberOfDevices] = Device_name;
                Port_names[NumberOfDevices] = Port_name;
                NumberOfDevices++;
                if (Save_Devices())
                {
                    if (Load_Devices())
                        MessageBox.Show("The device ID: '" + Device_name + "' is added successfully ...");
                    else
                        MessageBox.Show("Device added but unable to load it ...");
                }
                else
                    MessageBox.Show("Unable to add device ...");
            }
        }

        private void Devices_Load_Click(object sender, EventArgs e)
        {
            Load_Devices();
        }

        private void Devices_Save_Click(object sender, EventArgs e)
        {
            Save_Devices();
        }

        private void Devices_Add_Click(object sender, EventArgs e)
        {
            String name = TB_NewDeviceID.Text;
            String port = TB_NewPortName.Text;
            string[] s1 = name.Split(' ');
            string[] s2 = name.Split('.');
            string[] s3 = name.Split('\r');
            string[] s4 = name.Split('\t');
            string[] s5 = name.Split(',');
            string[] p1 = port.Split(' ');
            string[] p2 = port.Split('.');
            string[] p3 = port.Split('\r');
            string[] p4 = port.Split('\t');
            string[] p5 = port.Split(',');
            if (name != " " && name != "" && s1.Length == 1 && s2.Length == 1 && s3.Length == 1 && s4.Length == 1 && s5.Length == 1 &&
                p1.Length == 1 && p2.Length == 1 && p3.Length == 1 && p4.Length == 1 && p5.Length == 1)
            {
                bool isany = false;
                for (int i = 0; i < NumberOfDevices; i++)
                {
                    if (Device_names[i] == name) isany = true;
                }
                if (isany)
                    MessageBox.Show("The given device is added before ...");
                else
                {
                    if (port == " " || port == "") port = "null";
                    Add_Device(name, port);
                    TB_NewDeviceID.Clear();
                    TB_NewPortName.Clear();
                }
            }
            else
                MessageBox.Show("The given device is not valid ...");
        }

        private void Devices_ClearAll_Click(object sender, EventArgs e)
        {
            try
            {
                string[] empty = new string[0];
                System.IO.File.WriteAllLines(@".\devices.dat", empty);
                Devices_Load_Click(null, null);
                MessageBox.Show("All devices are removed successfully ...");
            }
            catch
            {
                MessageBox.Show("Unable to delete devices ...");
            }
        }

        private void Devices_DeleteSelected_Click(object sender, EventArgs e)
        {
            int ind = DevicesCheckList.SelectedIndex;
            if (ind > -1)
            {
                foreach (SerialPort p in ComPorts)
                {
                    if (p.PortName == Port_names[ind])
                    {
                        if (p.IsOpen) p.Close();
                        ComPorts.Remove(p);
                    }
                }

                try
                {

                    if (NumberOfDevices > 0)
                    {
                        string[] lines = new string[NumberOfDevices - 1];
                        int j = -1;
                        for (int i = 0; i < NumberOfDevices; i++)
                        {
                            if (i != ind)
                            {
                                j++;
                                lines[j] = Device_names[i] + " " + Port_names[i];
                            }
                        }
                        System.IO.File.WriteAllLines(@".\devices.dat", lines);
                        Devices_Load_Click(null, null);
                        MessageBox.Show("Selected device is removed successfully ...");
                    }
                }
                catch { }
            }
        }

        //<<<<<<< master
        internal string CreateChildCommand(string Device_name, string command)
        //=======
        //        private string CreateChildCommand(string Device_name, string command)
        //>>>>>>> master
        {
            string TotalCommand = "";
            int device_ind = -1;
            for (int i = 0; i < NumberOfDevices; i++)
            {
                if (Device_names[i] == Device_name)
                {
                    device_ind = i;
                    break;
                }
            }

            if (device_ind != -1)
            {
                if (Port_names[device_ind] == "null")
                    MessageBox.Show("The device ID '" + Device_name + "' is not configured ...");
                else
                    TotalCommand = Port_names[device_ind] + "." + Device_name + "." + command;
            }
            else
                MessageBox.Show("There is no device ID by the name '" + Device_name + "' ...\rYou should first add the device in settings.");
            return TotalCommand;
        }

        private void Devices_Identify_Click(object sender, EventArgs e)
        {
            int i1 = 0;
            int i2 = 0;
            int i3 = 0;
            int i4 = 0;
            if (CB_UART2.Checked) i1 = 1;
            if (CB_UART6.Checked) i2 = 1;
            if (CB_CAN1.Checked) i3 = 1;
            if (CB_CAN2.Checked) i4 = 1;
            for (int i = 0; i < NumberOfDevices; i++)
            {
                if (!Port_names[i].StartsWith("COM"))
                {
                    string CompleteOrder = "?." + Device_names[i] + ".you? " + i1.ToString() + " " + i2.ToString() + " " + i3.ToString() + " " + i4.ToString() + "\r";
                    Thread.Sleep(100);
                    string response = SendAndReceiveResponse(CompleteOrder);
                    Port_names[i] = response;
                }
            }
            Save_Devices();
            Load_Devices();
            MessageBox.Show("Finished.");
        }

        private bool CheckAllConfiguredDevices()
        {
            bool isOk = false;
            int nNull = 0;
            if (NumberOfDevices > 0) isOk = true;
            for (int i = 0; i < NumberOfDevices; i++)
            {
                if (Port_names[i] != "null")
                {
                    string CompleteOrder = CreateChildCommand(Device_names[i], "you?\r");
                    string response = SendAndReceiveResponse(CompleteOrder);
                    if (response != Device_names[i])
                    {
                        isOk = false;
                        MessageBox.Show("Something went wrong. Check all ports and devices and configure all.");
                        break;
                    }
                }
                else
                    nNull++;
            }

            if (nNull == NumberOfDevices)
            {
                MessageBox.Show("There is not any configured device. One should add device ID(s) and configure them.");
                isOk = false;
            }
            return isOk;
        }

        private void Devices_CheckAll_Click(object sender, EventArgs e)
        {
            bool isOk = CheckAllConfiguredDevices();
            if (isOk) MessageBox.Show("All configured devices work correctly.");
        }

        private bool hv_hv(decimal value)
        {
            string CompleteOrder = CreateChildCommand("hv", "hv " + value.ToString() + "\r");
            return SendAndReceiveOK(CompleteOrder);
        }

        private bool hv_wehnelt(decimal value)
        {
            string CompleteOrder = CreateChildCommand("hv", "wehnelt " + value.ToString() + "\r");
            return SendAndReceiveOK(CompleteOrder);
        }

        private bool hv_filament(decimal value)
        {
            string CompleteOrder = CreateChildCommand("hv", "filament " + (255 - value).ToString() + "\r");
            return SendAndReceiveOK(CompleteOrder);
        }

        private void UD_HV_HV_ValueChanged(object sender, EventArgs e)
        {
            if (hv_hv(UD_HV_HV.Value))
            {
                decimal val = 30 / UD_HV_HV.Maximum * UD_HV_HV.Value;
                L_HV_HV.Text = String.Format("{0:0.0} (KV)", val);
                UD_HV_HV.Tag = UD_HV_HV.Value.ToString();
            }
            else
            {
                this.UD_HV_HV.ValueChanged -= new System.EventHandler(this.UD_HV_HV_ValueChanged);
                UD_HV_HV.Value = Decimal.Parse(UD_HV_HV.Tag.ToString());
                this.UD_HV_HV.ValueChanged += new System.EventHandler(this.UD_HV_HV_ValueChanged);
            }
            //<<<<<<< master
            //=======

            //int val2 = (int)Math.Round((double)UD_HV_HV.Value * (double)numericHV.Maximum/ (double)Settings1.Default.hv_raw_max);
            //if (val2 > numericHV.Maximum) val2 = (int)numericHV.Maximum;
            //if (val2 < numericHV.Minimum) val2 = (int)numericHV.Minimum;

            //numericHV.ValueChanged -= numericHV_ValueChanged;
            //numericHV.Value = val2;
            //numericHV.ValueChanged += numericHV_ValueChanged;
            //>>>>>>> master
        }

        private void UD_dHV_HV_ValueChanged(object sender, EventArgs e)
        {
            UD_HV_HV.Increment = UD_dHV_HV.Value;
        }

        private void UD_HV_Wehlnet_ValueChanged(object sender, EventArgs e)
        {
            if (hv_wehnelt(UD_HV_Wehnelt.Value))
            {
                decimal val = 500 / UD_HV_Wehnelt.Maximum * UD_HV_Wehnelt.Value;
                L_HV_Wehnelt.Text = String.Format("{0:0.0} (V)", val);
                UD_HV_Wehnelt.Tag = UD_HV_Wehnelt.Value.ToString();
            }
            else
            {
                this.UD_HV_Wehnelt.ValueChanged -= new System.EventHandler(this.UD_HV_Wehlnet_ValueChanged);
                UD_HV_Wehnelt.Value = Decimal.Parse(UD_HV_Wehnelt.Tag.ToString());
                this.UD_HV_Wehnelt.ValueChanged += new System.EventHandler(this.UD_HV_Wehlnet_ValueChanged);
            }
        }

        private void UD_dHV_Wehlnet_ValueChanged(object sender, EventArgs e)
        {
            UD_HV_Wehnelt.Increment = UD_dHV_Wehnelt.Value;
        }

        private void UD_HV_Filament_ValueChanged(object sender, EventArgs e)
        {
            if (hv_filament(UD_HV_Filament.Value))
            {
                decimal val = 100 / UD_HV_Filament.Maximum * UD_HV_Filament.Value;
                L_HV_Filament.Text = String.Format("{0:0.0} %", val);
                UD_HV_Filament.Tag = UD_HV_Filament.Value.ToString();
            }
            else
            {
                this.UD_HV_Filament.ValueChanged -= new System.EventHandler(this.UD_HV_Filament_ValueChanged);
                UD_HV_Filament.Value = Decimal.Parse(UD_HV_Filament.Tag.ToString());
                this.UD_HV_Filament.ValueChanged += new System.EventHandler(this.UD_HV_Filament_ValueChanged);
            }
            //<<<<<<< master
            //=======


            //int val2 = (int)Math.Round((double)UD_HV_Filament.Value * (double)numericUpDown13.Maximum / (double)Settings1.Default.fb_raw_max);
            //if (val2 > numericUpDown13.Maximum) val2 = (int)numericUpDown13.Maximum;
            //if (val2 < numericUpDown13.Minimum) val2 = (int)numericUpDown13.Minimum;

            //numericUpDown13.ValueChanged -= numericUpDown13_ValueChanged;
            //numericUpDown13.Value = val2;
            //numericUpDown13.ValueChanged += numericUpDown13_ValueChanged;
            //>>>>>>> master
        }

        private void UD_dHV_Filament_ValueChanged(object sender, EventArgs e)
        {
            UD_HV_Filament.Increment = UD_dHV_Filament.Value;
        }

        private bool lens_con1(decimal value)
        {
            string CompleteOrder = CreateChildCommand("l", "conh " + value.ToString() + "\r");
            return SendAndReceiveOK(CompleteOrder);
        }

        private bool lens_con2(decimal value)
        {
            string CompleteOrder = CreateChildCommand("l", "conl " + value.ToString() + "\r");
            return SendAndReceiveOK(CompleteOrder);
        }

        private bool lens_iml(decimal value)
        {
            string CompleteOrder = CreateChildCommand("l", "iml " + value.ToString() + "\r");
            return SendAndReceiveOK(CompleteOrder);
        }

        private bool lens_obj(decimal value)
        {
            string CompleteOrder = CreateChildCommand("l", "obj " + value.ToString() + "\r");
            return SendAndReceiveOK(CompleteOrder);
        }

        private bool lens_objc(decimal value)
        {
            string CompleteOrder = CreateChildCommand("l", "objc " + value.ToString() + "\r");
            return SendAndReceiveOK(CompleteOrder);
        }

        private bool lens_gau(decimal valuex, decimal valuey)
        {
            string CompleteOrder = CreateChildCommand("l", "gau " + valuex.ToString("0000") + " " + valuey.ToString("0000") + "\r");
            return SendAndReceiveOK(CompleteOrder);
        }

        private bool lens_gaux(decimal value)
        {
            string CompleteOrder = CreateChildCommand("l", "gaux " + value.ToString() + "\r");
            return SendAndReceiveOK(CompleteOrder);
        }

        private bool lens_gauy(decimal value)
        {
            string CompleteOrder = CreateChildCommand("l", "gauy " + value.ToString() + "\r");
            return SendAndReceiveOK(CompleteOrder);
        }

        private bool lens_gad(decimal valuex, decimal valuey)
        {
            string CompleteOrder = CreateChildCommand("l", "gad " + valuex.ToString("0000") + " " + valuey.ToString("0000") + "\r");
            return SendAndReceiveOK(CompleteOrder);
        }

        private bool lens_gadx(decimal value)
        {
            string CompleteOrder = CreateChildCommand("l", "gadx " + value.ToString() + "\r");
            return SendAndReceiveOK(CompleteOrder);
        }

        private bool lens_gady(decimal value)
        {
            string CompleteOrder = CreateChildCommand("l", "gady " + value.ToString() + "\r");
            return SendAndReceiveOK(CompleteOrder);
        }

        private bool lens_ic(decimal xvalue, decimal yvalue)
        {
            xvalue = (2047 - xvalue);
            yvalue = (2047 - yvalue);
            string CompleteOrder = CreateChildCommand("l", "ic " + xvalue.ToString("0000") + " " + yvalue.ToString("0000") + "\r");
            return SendAndReceiveOK(CompleteOrder);
        }

        private bool lens_stig(decimal xvalue, decimal yvalue)
        {
            string CompleteOrder = CreateChildCommand("l", "stig " + xvalue.ToString("0000") + " " + yvalue.ToString("0000") + "\r");
            return SendAndReceiveOK(CompleteOrder);
        }

        private bool lens_wobble(decimal value)
        {
            string CompleteOrder = CreateChildCommand("l", "wobbel 0 " + value.ToString() + "\r");
            return SendAndReceiveOK(CompleteOrder);
        }

        private void UD_Lens_CON1_ValueChanged(object sender, EventArgs e)
        {
            if (lens_con1(UD_Lens_CON1.Value))
            {
                double val = (2.5 / (2 * 0.62) / 1.0) / (double)UD_Lens_CON1.Maximum * (double)UD_Lens_CON1.Value;
                L_Lens_CON1.Text = String.Format("{0:0.00} A,{1:0.0} V", val, val * 2 * 0.62);
                UD_Lens_CON1.Tag = UD_Lens_CON1.Value.ToString();
                UD_Lens_CON2.Value = (decimal)(int)((double)UD_Lens_CON1.Value * 3.0 / (2.0 * 0.62) / ((double)numericUpDown7.Value - (0.12 / 10.0) * ((double)numericUpDown11.Value - 10)));
            }
            else
            {
                this.UD_Lens_CON1.ValueChanged -= new System.EventHandler(this.UD_Lens_CON1_ValueChanged);
                UD_Lens_CON1.Value = Decimal.Parse(UD_Lens_CON1.Tag.ToString());
                this.UD_Lens_CON1.ValueChanged += new System.EventHandler(this.UD_Lens_CON1_ValueChanged);
            }

            numeric_PCF.ValueChanged -= numeric_PCF_ValueChanged;
            numeric_PCF.Value = UD_Lens_CON1.Value;
            numeric_PCF.ValueChanged += numeric_PCF_ValueChanged;
        }

        private void UD_dLens_CON1_ValueChanged(object sender, EventArgs e)
        {
            UD_Lens_CON1.Increment = UD_dLens_CON1.Value;
        }

        private void UD_Lens_CON2_ValueChanged(object sender, EventArgs e)
        {
            if (lens_con2(UD_Lens_CON2.Value))
            {
                double val = (2.5 / (3.0)) / (double)UD_Lens_CON2.Maximum * (double)UD_Lens_CON2.Value;
                L_Lens_CON2.Text = String.Format("{0:0.00} A,{1:0.0} V", val, val * 3.0);
                UD_Lens_CON2.Tag = UD_Lens_CON2.Value.ToString();
            }
            else
            {
                this.UD_Lens_CON2.ValueChanged -= new System.EventHandler(this.UD_Lens_CON2_ValueChanged);
                UD_Lens_CON2.Value = Decimal.Parse(UD_Lens_CON2.Tag.ToString());
                this.UD_Lens_CON2.ValueChanged += new System.EventHandler(this.UD_Lens_CON2_ValueChanged);
            }

            numericUpDown12.ValueChanged -= numericUpDown12_ValueChanged;
            numericUpDown12.Value = UD_Lens_CON2.Value;
            numericUpDown12.ValueChanged += numericUpDown12_ValueChanged;
        }

        private void UD_dLens_CON2_ValueChanged(object sender, EventArgs e)
        {
            UD_Lens_CON2.Increment = UD_dLens_CON2.Value;
        }

        private void UD_Lens_IML_ValueChanged(object sender, EventArgs e)
        {
            if (lens_iml(UD_Lens_IML.Value))
            {
                double val = 2.0 / (double)UD_Lens_IML.Maximum * (double)UD_Lens_IML.Value;
                L_Lens_IML.Text = String.Format("{0:0.00} A,{1:0.0} V", val / (0.62 * 2.0), val);
                UD_Lens_IML.Tag = UD_Lens_IML.Value.ToString();
            }
            else
            {
                this.UD_Lens_IML.ValueChanged -= new System.EventHandler(this.UD_Lens_IML_ValueChanged);
                UD_Lens_IML.Value = Decimal.Parse(UD_Lens_IML.Tag.ToString());
                this.UD_Lens_IML.ValueChanged += new System.EventHandler(this.UD_Lens_IML_ValueChanged);
            }
        }

        private void UD_dLens_IML_ValueChanged(object sender, EventArgs e)
        {
            UD_Lens_IML.Increment = UD_dLens_IML.Value;
        }

        private void UD_Lens_OBJ_ValueChanged(object sender, EventArgs e)
        {
            if (lens_obj(UD_Lens_OBJ.Value))
            {
                //<<<<<<< master
                double val = (double)UD_Lens_OBJ.Value * Settings1.Default.Coef_res_fine + (double)numericUpDown_objc.Value * Settings1.Default.Coef_res_course;//2.0 / (double)UD_Lens_OBJ.Maximum * (double)UD_Lens_OBJ.Value;
                L_Lens_OBJ.Text = String.Format("{0:0.00} A|{1:0.0} V\nI^2:{2:0.00}", val, val * 0.62, val * val);
                //=======
                //                double val = 2.0 / (double)UD_Lens_OBJ.Maximum * (double)UD_Lens_OBJ.Value;
                //                L_Lens_OBJ.Text = String.Format("{0:0.00} A,{1:0.0} V", val/0.62, val);
                //>>>>>>> master
                UD_Lens_OBJ.Tag = UD_Lens_OBJ.Value.ToString();
            }
            else
            {
                this.UD_Lens_OBJ.ValueChanged -= new System.EventHandler(this.UD_Lens_OBJ_ValueChanged);
                UD_Lens_OBJ.Value = Decimal.Parse(UD_Lens_OBJ.Tag.ToString());
                this.UD_Lens_OBJ.ValueChanged += new System.EventHandler(this.UD_Lens_OBJ_ValueChanged);
            }
        }

        private void UD_dLens_OBJ_ValueChanged(object sender, EventArgs e)
        {
            UD_Lens_OBJ.Increment = UD_dLens_OBJ.Value;
        }

        private bool se_pmt(decimal value)
        {
            string CompleteOrder = CreateChildCommand("se", "pmt " + value.ToString() + "\r");
            return SendAndReceiveOK(CompleteOrder);
        }

        private bool se_faraday(decimal value)
        {
            string CompleteOrder = CreateChildCommand("se", "faraday " + value.ToString() + "\r");
            return SendAndReceiveOK(CompleteOrder);
        }

        private void UD_SE_PMT_ValueChanged(object sender, EventArgs e)
        {
            if (se_pmt(UD_SE_PMT.Value))
            {
                decimal val = 100 / UD_SE_PMT.Maximum * UD_SE_PMT.Value;
                L_SE_PMT.Text = String.Format("{0:0.0} %", val);
                UD_SE_PMT.Tag = UD_SE_PMT.Value.ToString();
            }
            else
            {
                this.UD_SE_PMT.ValueChanged -= new System.EventHandler(this.UD_SE_PMT_ValueChanged);
                UD_SE_PMT.Value = Decimal.Parse(UD_SE_PMT.Tag.ToString());
                this.UD_SE_PMT.ValueChanged += new System.EventHandler(this.UD_SE_PMT_ValueChanged);
            }
        }

        private void UD_dSE_PMT_ValueChanged(object sender, EventArgs e)
        {
            UD_SE_PMT.Increment = UD_dSE_PMT.Value;
        }

        private void UD_SE_Faradus_ValueChanged(object sender, EventArgs e)
        {
            if (se_faraday(UD_SE_Faraday.Value))
            {
                decimal val = 100 / UD_SE_Faraday.Maximum * UD_SE_Faraday.Value;
                L_SE_Faraday.Text = String.Format("{0:0.0} %", val);
                UD_SE_Faraday.Tag = UD_SE_Faraday.Value.ToString();
            }
            else
            {
                this.UD_SE_Faraday.ValueChanged -= new System.EventHandler(this.UD_SE_Faradus_ValueChanged);
                UD_SE_Faraday.Value = Decimal.Parse(UD_SE_Faraday.Tag.ToString());
                this.UD_SE_Faraday.ValueChanged += new System.EventHandler(this.UD_SE_Faradus_ValueChanged);
            }
        }

        private void UD_dSE_Faradus_ValueChanged(object sender, EventArgs e)
        {
            UD_SE_Faraday.Increment = UD_dSE_Faraday.Value;
        }

        private bool dactimer(int state)
        {
            string CompleteOrder = "dactimer " + state.ToString() + "\r";
            return SendAndReceiveOK(CompleteOrder);
        }

        private void Btn_UDPDisconnect_Click(object sender, EventArgs e)
        {

            dactimer(0);
            // Array.Clear(receivedData, 2, 512);
            Thread.Sleep(100); Stop();
            LabelUDPConnected.Visible = isUDPConnected;
            LabelUDPConnected.Update();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // ProcessFrame(null, null);
            string info = "";

            if (isUDPConnected) frame.Bytes = receivedData_frame;
            if (isAcquire)
            {
                imageform.ViewPort.Image = SetFilter(frame);
            }
            else
            {
                if (StartMove)
                {
                    frame.DrawPolyline(SelectionRec, true, new Gray(255), 1);
                    frame.DrawPolyline(SelectionRec2, true, new Gray(1), 1);
                }
                if (overallMotionPixelCount++ == 10)
                {
                    overallMotionPixelCount = -10;

                }

                info = String.Format("{0:0000.0}FPS", FPS);

                Image<Gray,byte> frame0= SetFilter(frame);
                frame0.Draw(info, ref format, new System.Drawing.Point(1, 500), new Gray(200)); //Draw on the image using the specific font

                if (isformmode)
                    formmode.ViewPort.Image = frame0;
                else
                    ViewPort.Image = frame0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Start")
            {
                string CompleteOrder = CreateChildCommand("se", "kv 1" + "\r");
                if (SendAndReceiveOK(CompleteOrder))
                {
                    button1.Text = "Stop";
                    button1.BackColor = Color.Beige;
                }
            }
            else
            if (button1.Text == "Stop")
            {
                string CompleteOrder = CreateChildCommand("se", "kv 0" + "\r");
                if (SendAndReceiveOK(CompleteOrder))
                {
                    button1.Text = "Start";
                    button1.BackColor = Color.Azure;
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (SerialPort p in ComPorts)
            {
                if (p.IsOpen) p.Close();
            }
            ComPorts.Clear();

            for (int i = 0; i < NumberOfDevices; i++)
            {
                string port_name = Port_names[i];
                if (port_name.StartsWith("COM"))
                {
                    try
                    {
                        SerialPort Port = new SerialPort(port_name, 9600, Parity.None, 8, StopBits.One);
                        if (Port.IsOpen) Port.Close();
                        Port.WriteTimeout = 2500;
                        Port.ReadTimeout = 5000;
                        Port.Open();
                        ComPorts.Add(Port);
                        log.Text = ("Device " + Device_names[i] + " is connected successfully.");
                    }
                    catch (Exception e2)
                    {

                        if (isAdmin)
                            MessageBox.Show("Unable to connect the device " + Device_names[i] + "\r\r Exception: " + e2.Message);
                        else
                            MessageBox.Show("Unable to connect the device " + Device_names[i]);
                    }
                }
            }

        }

        private int SelectedScannerPort()
        {
            int state = Scanner_ISelect.SelectedIndex;
            if (state == 0) return 0;
            if (state == 1) return 4;
            if (state == 2) return 2;
            if (state == 3) return 1;
            return 0;
        }

        private bool sisel(int state)
        {
            string CompleteOrder = "sisel " + state.ToString() + "\r";
            SendAndReceiveOK(CompleteOrder);
            return true;
        }

        private bool strim(int state, int val)
        {
            string CompleteOrder = "strim " + state.ToString() + " " + val.ToString() + "\r";
            SendAndReceiveOK(CompleteOrder);
            return true;
        }

        private bool dtrim(int state, int val)
        {
            string CompleteOrder = "dtrim " + state.ToString() + " " + val.ToString() + "\r";
            SendAndReceiveOK(CompleteOrder);
            return true;
        }

        private bool acquire(int state)
        {
            string CompleteOrder = "acquire " + state.ToString() + "\r";
            SendAndReceiveOK(CompleteOrder);
            return true;
        }

        private void UpdateScanner()
        {
            int state = SelectedScannerPort();
            int xtrim = (int)UD_STrim_Val1.Value + 2047;
            int xtrimf = (int)UD_STrim_Val2.Value + 2047;
            int ytrim = (int)UD_STrim_Val3.Value + 2047;
            int ytrimf = (int)UD_STrim_Val4.Value + 2047;
            int x2trim = (int)UD_STrim_Val5.Value + 2047;
            int y2trim = (int)UD_STrim_Val6.Value + 2047;
            sisel(state);
            strim(0, xtrim);
            strim(1, xtrimf);
            strim(2, ytrim);
            strim(3, ytrimf);
            strim(4, x2trim);
            strim(6, y2trim);
        }

        private void Scanner_ISelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateScanner();
        }

        private void UD_STrim_dVal1_ValueChanged(object sender, EventArgs e)
        {
            UD_STrim_Val1.Increment = UD_STrim_dVal1.Value;
        }

        private void UD_STrim_dVal2_ValueChanged(object sender, EventArgs e)
        {
            UD_STrim_Val2.Increment = UD_STrim_dVal2.Value;
        }

        private void UD_STrim_dVal3_ValueChanged(object sender, EventArgs e)
        {
            UD_STrim_Val3.Increment = UD_STrim_dVal3.Value;
        }

        private void UD_STrim_dVal4_ValueChanged(object sender, EventArgs e)
        {
            UD_STrim_Val4.Increment = UD_STrim_dVal4.Value;
        }

        private void UD_STrim_Val1_ValueChanged(object sender, EventArgs e)
        {
            userControl13.X = (int)this.UD_STrim_Val1.Value;
            UpdateScanner();
        }

        private void UD_STrim_Val2_ValueChanged(object sender, EventArgs e)
        {
            UpdateScanner();
        }

        private void UD_STrim_Val3_ValueChanged(object sender, EventArgs e)
        {
            userControl13.Y = (int)this.UD_STrim_Val3.Value;
            UpdateScanner();
        }

        private void UD_STrim_Val4_ValueChanged(object sender, EventArgs e)
        {
            UpdateScanner();
        }

        private void UD_DetectorTrim_dCoarse_ValueChanged(object sender, EventArgs e)
        {
            UD_DetectorTrim_Coarse.Increment = UD_DetectorTrim_dCoarse.Value;
        }

        private void UD_DetectorTrim_dFine_ValueChanged(object sender, EventArgs e)
        {
            UD_DetectorTrim_Fine.Increment = UD_DetectorTrim_dFine.Value;
        }

        private void UD_DetectorTrim_Coarse_ValueChanged(object sender, EventArgs e)
        {
            UpdateDetector();
        }

        private void UD_DetectorTrim_Fine_ValueChanged(object sender, EventArgs e)
        {
            UpdateDetector();
        }

        private void Btn_Acquire_Click(object sender, EventArgs e)
        {
            ChangeWindow(0, 0, 512, 512);

            /*
            isAcquire = true;
            imageform = new ImageForm();
            imageform.Owner = this;
            imageform.Text = "Render";
            imageform.Show();
            AcquireCnt = 0;
            acquire((int)UD_AcquireNumber.Value);
            */
        }

        private void button3_Click(object sender, EventArgs e)
        {
            isAcquire = false;
            if (isUDPConnected)
                DisConnect_UDP();
            else
                Connect_UDP();
        }

        private void UD_Lens_ux_ValueChanged(object sender, EventArgs e)
        {
            userControl14.X = (int)this.UD_Lens_shiftx.Value;
            Set_ShiftAndTilt(true);
        }

        private void UD_dLens_ux_ValueChanged(object sender, EventArgs e)
        {
            UD_Lens_shiftx.Increment = UD_dLens_shiftx.Value;
        }

        private void UD_Lens_uy_ValueChanged(object sender, EventArgs e)
        {
            userControl14.Y = (int)this.UD_Lens_shifty.Value;
            Set_ShiftAndTilt(true);
        }

        private void UD_dLens_uy_ValueChanged(object sender, EventArgs e)
        {
            UD_Lens_shifty.Increment = UD_dLens_shifty.Value;
        }

        private void UD_Lens_dx_ValueChanged(object sender, EventArgs e)
        {
            userControl15.X = (int)this.UD_Lens_tiltx.Value;
            if (!(CON1lock.Checked))
                Set_ShiftAndTilt(false);
        }

        private void UD_dLens_dx_ValueChanged(object sender, EventArgs e)
        {
            UD_Lens_tiltx.Increment = UD_dLens_tiltx.Value;
        }

        private void UD_Lens_dy_ValueChanged(object sender, EventArgs e)
        {
            userControl15.Y = (int)this.UD_Lens_tilty.Value;
            if (!(CON1lock.Checked))
                Set_ShiftAndTilt(false);
        }

        private void UD_dLens_dy_ValueChanged(object sender, EventArgs e)
        {
            UD_Lens_tilty.Increment = UD_dLens_tilty.Value;
        }

        private void Set_Lens_icxAndy()
        {
            double x = ((double)UD_Lens_icx.Value) * (ic_invert_x.Checked ? -1 : 1);
            double y = ((double)UD_Lens_icy.Value) * (ic_invert_y.Checked ? -1 : 1);
            double teta = (double)ic_teta.Value;

            int ux = (int)(Math.Cos(Math.PI * teta / 180.0) * x + Math.Sin(Math.PI * teta / 180.0) * y);
            int uy = (int)(-Math.Sin(Math.PI * teta / 180.0) * x + Math.Cos(Math.PI * teta / 180.0) * y);
            if (ux >= -2048 && ux <= 2047 && uy >= -2048 && uy <= 2047)
            {
                if (lens_ic(ux, uy))
                {
                    decimal val = 100 / UD_Lens_icx.Maximum * UD_Lens_icx.Value;
                    L_Lens_icx.Text = String.Format("{0:0.0} %", val);
                    UD_Lens_icx.Tag = UD_Lens_icx.Value.ToString();

                    val = 100 / UD_Lens_icy.Maximum * UD_Lens_icy.Value;
                    L_Lens_icy.Text = String.Format("{0:0.0} %", val);
                    UD_Lens_icy.Tag = UD_Lens_icy.Value.ToString();
                }
                else
                {
                    this.UD_Lens_icy.ValueChanged -= new System.EventHandler(this.UD_Lens_icy_ValueChanged);
                    UD_Lens_icy.Value = Decimal.Parse(UD_Lens_icy.Tag.ToString());
                    this.UD_Lens_icy.ValueChanged += new System.EventHandler(this.UD_Lens_icy_ValueChanged);

                    this.UD_Lens_icx.ValueChanged -= new System.EventHandler(this.UD_Lens_icx_ValueChanged);
                    UD_Lens_icx.Value = Decimal.Parse(UD_Lens_icx.Tag.ToString());
                    this.UD_Lens_icx.ValueChanged += new System.EventHandler(this.UD_Lens_icx_ValueChanged);
                }
            }
            else
            {
                this.UD_Lens_icy.ValueChanged -= new System.EventHandler(this.UD_Lens_icy_ValueChanged);
                UD_Lens_icy.Value = Decimal.Parse(UD_Lens_icy.Tag.ToString());
                this.UD_Lens_icy.ValueChanged += new System.EventHandler(this.UD_Lens_icy_ValueChanged);

                this.UD_Lens_icx.ValueChanged -= new System.EventHandler(this.UD_Lens_icx_ValueChanged);
                UD_Lens_icx.Value = Decimal.Parse(UD_Lens_icx.Tag.ToString());
                this.UD_Lens_icx.ValueChanged += new System.EventHandler(this.UD_Lens_icx_ValueChanged);
            }

            IMLCon();
        }

        private void UD_Lens_icx_ValueChanged(object sender, EventArgs e)
        {
            userControl11.X = (int)this.UD_Lens_icx.Value;
            if (!(IMLLock.Checked))
                Set_Lens_icxAndy();
        }

        private void UD_dLens_icx_ValueChanged(object sender, EventArgs e)
        {
            UD_Lens_icx.Increment = UD_dLens_icx.Value;
        }

        private void UD_Lens_icy_ValueChanged(object sender, EventArgs e)
        {
            userControl11.Y = (int)this.UD_Lens_icy.Value;
            if (!(IMLLock.Checked))
                Set_Lens_icxAndy();
        }

        private void UD_dLens_icy_ValueChanged(object sender, EventArgs e)
        {
            UD_Lens_icy.Increment = UD_dLens_icy.Value;
        }

        private void Set_Lens_stig()
        {
            int ux = (int)(2047 - UD_Lens_stigx.Value);
            int uy = (int)(2047 - UD_Lens_stigy.Value);
            if (ux >= 0 && ux < 4096 && uy >= 0 && uy < 4096)
            {
                if (lens_stig(ux, uy))
                {
                    decimal val = 100 / UD_Lens_stigx.Maximum * UD_Lens_stigx.Value;
                    L_Lens_stigx.Text = String.Format("{0:0.0} %", val);
                    UD_Lens_stigx.Tag = UD_Lens_stigx.Value.ToString();

                    val = 100 / UD_Lens_stigy.Maximum * UD_Lens_stigy.Value;
                    L_Lens_stigy.Text = String.Format("{0:0.0} %", val);
                    UD_Lens_stigy.Tag = UD_Lens_stigy.Value.ToString();
                }
                else
                {
                    this.UD_Lens_stigx.ValueChanged -= new System.EventHandler(this.UD_Lens_stigx_ValueChanged);
                    UD_Lens_stigx.Value = Decimal.Parse(UD_Lens_stigx.Tag.ToString());
                    this.UD_Lens_stigx.ValueChanged += new System.EventHandler(this.UD_Lens_stigx_ValueChanged);

                    this.UD_Lens_stigy.ValueChanged -= new System.EventHandler(this.UD_Lens_stigy_ValueChanged);
                    UD_Lens_stigy.Value = Decimal.Parse(UD_Lens_stigy.Tag.ToString());
                    this.UD_Lens_stigy.ValueChanged += new System.EventHandler(this.UD_Lens_stigy_ValueChanged);
                }
            }
        }

        private void UD_Lens_stigx_ValueChanged(object sender, EventArgs e)
        {
            userControl12.X = (int)this.UD_Lens_stigx.Value;
            Set_Lens_stig();
        }

        private void UD_dLens_stigx_ValueChanged(object sender, EventArgs e)
        {
            UD_Lens_stigx.Increment = UD_dLens_stigx.Value;
        }

        private void UD_Lens_stigy_ValueChanged(object sender, EventArgs e)
        {
            userControl12.Y = (int)this.UD_Lens_stigy.Value;
            Set_Lens_stig();
        }

        private void UD_dLens_stigy_ValueChanged(object sender, EventArgs e)
        {
            UD_Lens_stigy.Increment = UD_dLens_stigy.Value;
        }

        private void TB_Wobbler_Scroll(object sender, EventArgs e)
        {
            if (lens_wobble(5 * (Decimal)TB_Wobbler.Value))
            {
                //decimal val = 100 / TB_Wobbler.Maximum * TB_Wobbler.Value;
                //L_Lens_stigy.Text = String.Format("{0:0.0} %", val);
                TB_Wobbler.Tag = TB_Wobbler.Value.ToString();
            }
            else
            {
                this.TB_Wobbler.ValueChanged -= new System.EventHandler(this.TB_Wobbler_Scroll);
                TB_Wobbler.Value = Int32.Parse(TB_Wobbler.Tag.ToString());
                this.TB_Wobbler.ValueChanged += new System.EventHandler(this.TB_Wobbler_Scroll);
            }
        }

        private void ConvertToUD(ref int shift, ref int tilt, ref int u, ref int d)
        {
            /* if (shift < -2047)
                 shift = -2047;
             else if (shift > 2048)
                 shift = 2048;

             if (tilt < -4095)
                 tilt = -4095;
             else if (tilt > 4095)
                 tilt = 4095;*/

            d = 2047 + tilt - shift;
            /*if (d < 0)
            {
                d = 0;
                tilt = d - 2047 + shift;
            }
            else if (d > 4095)
            {
                d = 4095;
                tilt = d - 2047 + shift;
            }*/

            u = 2047 + shift;
        }

        private void Set_ShiftAndTilt(bool isShift)
        {
            int ux = 0;
            int dx = 0;
            int uy = 0;
            int dy = 0;
            int shiftX;
            int tiltX;
            int shiftY;
            int tiltY;
            double shX = (double)UD_Lens_shiftx.Value;
            double ttX = (double)UD_Lens_tiltx.Value;
            double shY = (double)UD_Lens_shifty.Value;
            double ttY = (double)UD_Lens_tilty.Value;
            double teta = (double)ga_teta.Value;
            //  this.UD_Lens_tiltx.ValueChanged -= new System.EventHandler(this.UD_Lens_dx_ValueChanged);
            // this.UD_Lens_tilty.ValueChanged -= new System.EventHandler(this.UD_Lens_dy_ValueChanged);
            // this.UD_Lens_icx.ValueChanged -= new System.EventHandler(this.UD_Lens_icx_ValueChanged);
            // this.UD_Lens_icy.ValueChanged -= new System.EventHandler(this.UD_Lens_icy_ValueChanged);
            try
            {
                if (CON1lock.Checked)
                {
                    UD_Lens_tiltx.Value = (conscon1x * (s_coef1.Value + t_coef1.Value) - (int)s_coef1.Value * (int)UD_Lens_shiftx.Value) / ((int)t_coef1.Value);
                    UD_Lens_tilty.Value = (conscon1y * (s_coef1.Value + t_coef1.Value) - (int)s_coef1.Value * (int)UD_Lens_shifty.Value) / ((int)t_coef1.Value);

                }
                if (CON2lock.Checked)
                {
                    UD_Lens_tiltx.Value = (conscon2x * (s_coef2.Value + t_coef2.Value) - (int)s_coef2.Value * (int)UD_Lens_shiftx.Value) / ((int)t_coef2.Value);
                    UD_Lens_tilty.Value = (conscon2y * (s_coef2.Value + t_coef2.Value) - (int)s_coef2.Value * (int)UD_Lens_shifty.Value) / ((int)t_coef2.Value);

                }
                if (IMLLock.Checked)
                {
                    // if (isShift)
                    {
                        //  UD_Lens_tiltx.Value = (cons_con1x * (s_coef.Value + t_coef.Value) - (int)s_coef.Value * (int)UD_Lens_shiftx.Value ) / ((int)t_coef.Value);
                        //  UD_Lens_tilty.Value = (cons_con1y * (s_coef.Value + t_coef.Value) - (int)s_coef.Value * (int)UD_Lens_shifty.Value ) / ((int)t_coef.Value); 
                        UD_Lens_icx.Value = (ilmconx * (s_coef.Value + t_coef.Value + lt_coef.Value) - (int)s_coef.Value * (int)UD_Lens_shiftx.Value - ((int)t_coef.Value - (int)lt_coef.Value) * (int)UD_Lens_tiltx.Value) / (lt_coef.Value);
                        UD_Lens_icy.Value = (ilmcony * (s_coef.Value + t_coef.Value + lt_coef.Value) - (int)s_coef.Value * (int)UD_Lens_shifty.Value - ((int)t_coef.Value - (int)lt_coef.Value) * (int)UD_Lens_tilty.Value) / (lt_coef.Value);

                    }
                    //  else
                    {
                        // UD_Lens_shiftx.Value = (int)((ilmconx - (int)t_coef.Value * (int)UD_Lens_tiltx.Value + (int)lt_coef.Value * ((int)UD_Lens_icx.Value - (int)UD_Lens_tiltx.Value)) / (int)s_coef.Value);
                        // UD_Lens_shifty.Value = (int)((ilmcony - (int)t_coef.Value * (int)UD_Lens_tilty.Value + (int)lt_coef.Value * ((int)UD_Lens_icy.Value - (int)UD_Lens_tilty.Value)) / (int)s_coef.Value);
                    }
                }
            }
            catch (Exception ee)
            { }
            // this.UD_Lens_tiltx.ValueChanged += new System.EventHandler(this.UD_Lens_dx_ValueChanged);
            // this.UD_Lens_tilty.ValueChanged += new System.EventHandler(this.UD_Lens_dy_ValueChanged);
            // this.UD_Lens_icx.ValueChanged += new System.EventHandler(this.UD_Lens_icx_ValueChanged);
            // this.UD_Lens_icy.ValueChanged += new System.EventHandler(this.UD_Lens_icy_ValueChanged);
            shiftX = (int)(Math.Cos(Math.PI * teta / 180.0) * shX + Math.Sin(Math.PI * teta / 180.0) * shY);
            tiltX = (int)(Math.Cos(Math.PI * teta / 180.0) * ttX + Math.Sin(Math.PI * teta / 180.0) * ttY);
            shiftY = (int)(-Math.Sin(Math.PI * teta / 180.0) * shX + Math.Cos(Math.PI * teta / 180.0) * shY);
            tiltY = (int)(-Math.Sin(Math.PI * teta / 180.0) * ttX + Math.Cos(Math.PI * teta / 180.0) * ttY);
            ConvertToUD(ref shiftY, ref tiltY, ref uy, ref dy);
            ConvertToUD(ref shiftX, ref tiltX, ref ux, ref dx);

            //this.UD_Lens_shiftx.ValueChanged -= new System.EventHandler(this.UD_Lens_ux_ValueChanged);
            //UD_Lens_shiftx.Value = shiftX;
            //this.UD_Lens_shiftx.ValueChanged += new System.EventHandler(this.UD_Lens_ux_ValueChanged);

            //this.UD_Lens_tiltx.ValueChanged -= new System.EventHandler(this.UD_Lens_dx_ValueChanged);
            //UD_Lens_tiltx.Value = tiltX;
            //this.UD_Lens_tiltx.ValueChanged += new System.EventHandler(this.UD_Lens_dx_ValueChanged);
            if (isShiftAndTiltValid(ux, uy, dx, dy))
            {
                if (lens_gau(ux, uy) && lens_gad(dx, dy))
                {
                    decimal ShiftVal = 200 / UD_Lens_shiftx.Maximum * UD_Lens_shiftx.Value - 100;
                    L_Lens_shiftx.Text = String.Format("{0:0.0} %", ShiftVal);
                    UD_Lens_shiftx.Tag = UD_Lens_shiftx.Value.ToString();

                    decimal TiltVal = 200 / UD_Lens_tiltx.Maximum * UD_Lens_tiltx.Value - 100;
                    L_Lens_tiltx.Text = String.Format("{0:0.0} %", TiltVal);
                    UD_Lens_tiltx.Tag = UD_Lens_tiltx.Value.ToString();

                    ShiftVal = 200 / UD_Lens_shifty.Maximum * UD_Lens_shifty.Value - 100;
                    L_Lens_shifty.Text = String.Format("{0:0.0} %", ShiftVal);
                    UD_Lens_shifty.Tag = UD_Lens_shifty.Value.ToString();

                    TiltVal = 200 / UD_Lens_tilty.Maximum * UD_Lens_tilty.Value - 100;
                    L_Lens_tilty.Text = String.Format("{0:0.0} %", TiltVal);
                    UD_Lens_tilty.Tag = UD_Lens_tilty.Value.ToString();
                }
                else
                    ReturnLensShiftAndTilt();
            }
            else
                ReturnLensShiftAndTilt();
            IMLCon();
        }

        private void IMLCon()
        {
            if (!(IMLLock.Checked))
            {
                ilmconx = (int)s_coef.Value * (int)UD_Lens_shiftx.Value + (int)t_coef.Value * (int)UD_Lens_tiltx.Value + (int)lt_coef.Value * ((int)UD_Lens_icx.Value - (int)UD_Lens_tiltx.Value);
                ilmcony = (int)s_coef.Value * (int)UD_Lens_shifty.Value + (int)t_coef.Value * (int)UD_Lens_tilty.Value + (int)lt_coef.Value * ((int)UD_Lens_icy.Value - (int)UD_Lens_tilty.Value);
                ilmconx = ilmconx / ((int)s_coef.Value + (int)t_coef.Value + (int)lt_coef.Value);
                ilmcony = ilmcony / ((int)s_coef.Value + (int)t_coef.Value + (int)lt_coef.Value);
                UDilmconx.Value = ilmconx;
                UDilmcony.Value = ilmcony;
            }
            if (!(CON1lock.Checked))
            {
                conscon1x = (int)s_coef1.Value * (int)UD_Lens_shiftx.Value + (int)t_coef1.Value * (int)UD_Lens_tiltx.Value;
                conscon1y = (int)s_coef1.Value * (int)UD_Lens_shifty.Value + (int)t_coef1.Value * (int)UD_Lens_tilty.Value;
                conscon1x = conscon1x / ((int)s_coef1.Value + (int)t_coef1.Value);
                conscon1y = conscon1y / ((int)s_coef1.Value + (int)t_coef1.Value);
                cons_con1x.Value = conscon1x;
                cons_con1y.Value = conscon1y;
            }
            if (!(CON2lock.Checked))
            {
                conscon2x = (int)s_coef2.Value * (int)UD_Lens_shiftx.Value + (int)t_coef2.Value * (int)UD_Lens_tiltx.Value;
                conscon2y = (int)s_coef2.Value * (int)UD_Lens_shifty.Value + (int)t_coef2.Value * (int)UD_Lens_tilty.Value;
                conscon2x = conscon2x / ((int)s_coef2.Value + (int)t_coef2.Value);
                conscon2y = conscon2y / ((int)s_coef2.Value + (int)t_coef2.Value);
                cons_con2x.Value = conscon2x;
                cons_con2y.Value = conscon2y;
            }
        }

        private bool isShiftAndTiltValid(int ux, int uy, int dx, int dy)
        {
            bool isvalid = false;
            if (ux >= 0 && ux < 4096 && uy >= 0 && uy < 4096 && dx >= 0 && dx < 4096 && dy >= 0 && dy < 4096) isvalid = true;
            return isvalid;
        }

        private void ReturnLensShiftAndTilt()
        {
            this.UD_Lens_shiftx.ValueChanged -= new System.EventHandler(this.UD_Lens_ux_ValueChanged);
            UD_Lens_shiftx.Value = Decimal.Parse(UD_Lens_shiftx.Tag.ToString());
            this.UD_Lens_shiftx.ValueChanged += new System.EventHandler(this.UD_Lens_ux_ValueChanged);

            this.UD_Lens_tiltx.ValueChanged -= new System.EventHandler(this.UD_Lens_dx_ValueChanged);
            UD_Lens_tiltx.Value = Decimal.Parse(UD_Lens_tiltx.Tag.ToString());
            this.UD_Lens_tiltx.ValueChanged += new System.EventHandler(this.UD_Lens_dx_ValueChanged);

            this.UD_Lens_shifty.ValueChanged -= new System.EventHandler(this.UD_Lens_uy_ValueChanged);
            UD_Lens_shifty.Value = Decimal.Parse(UD_Lens_shifty.Tag.ToString());
            this.UD_Lens_shifty.ValueChanged += new System.EventHandler(this.UD_Lens_uy_ValueChanged);

            this.UD_Lens_tilty.ValueChanged -= new System.EventHandler(this.UD_Lens_dy_ValueChanged);
            UD_Lens_tilty.Value = Decimal.Parse(UD_Lens_tilty.Tag.ToString());
            this.UD_Lens_tilty.ValueChanged += new System.EventHandler(this.UD_Lens_dy_ValueChanged);
        }

        private void FindTheBestGunAlignment()
        {
            int MaxIttrGunAlignment = (int)UDMaxNumItt.Value;
            int dxy = (int)UDdx.Value;
            dxy = (int)(dxy * 4096 / 100);
            int shiftx = (int)UD_Lens_shiftx.Value;
            int tiltx = (int)UD_Lens_tiltx.Value;
            int shifty = (int)UD_Lens_shifty.Value;
            int tilty = (int)UD_Lens_tilty.Value;

            bool isFound = false;
            int it = 0;
            while (!isFound || (it < MaxIttrGunAlignment))
            {
                it++;

                UD_Lens_tilty.Value = (decimal)(tilty);

                UD_Lens_tiltx.Value = (decimal)(tiltx - dxy);
                Thread.Sleep(50);
                double fxmdx = LastIntensity;
                UD_Lens_tiltx.Value = (decimal)(tiltx);
                Thread.Sleep(50);
                double fxy = LastIntensity;
                UD_Lens_tiltx.Value = (decimal)(tiltx + dxy);
                Thread.Sleep(50);
                double fxpdx = LastIntensity;

                UD_Lens_tilty.Value = (decimal)(tilty + dxy);
                Thread.Sleep(50);
                double fxpdx_ypdy = LastIntensity;

                UD_Lens_tiltx.Value = (decimal)(tiltx);

                //UD_Lens_tilty.Value = (decimal)(tilty + dxy);
                Thread.Sleep(50);
                double fypdy = LastIntensity;

                UD_Lens_tilty.Value = (decimal)(tilty - dxy);
                Thread.Sleep(50);
                double fymdy = LastIntensity;

                UD_Lens_tiltx.Value = (decimal)(tiltx - dxy);
                Thread.Sleep(50);
                double fxmdx_ymdy = LastIntensity;

                double v1 = (fxpdx - fxmdx) / dxy / 2.0;
                double v2 = (fypdy - fymdy) / dxy / 2.0;
                double a = (fxpdx - 2 * fxy + fxmdx) / dxy / dxy;
                double c = (fypdy - 2 * fxy + fymdy) / dxy / dxy;
                double b = (fxpdx_ypdy - fxpdx - fypdy + 2 * fxy - fxmdx - fymdy + fxmdx_ymdy) / dxy / dxy / 2.0;

                double delta = a * c - b * b;
                if (Math.Abs(delta) < 0.00000001)
                {
                    MessageBox.Show("Jacobian is not inversable. Maybe you need to increase dx.");
                    return;
                }

                tiltx = (int)(tiltx - (c * v1 - b * v2) / delta);
                tilty = (int)(tilty - (a * v2 - b * v1) / delta);

                UD_Lens_tiltx.Value = (decimal)(tiltx);
                UD_Lens_tilty.Value = (decimal)(tilty);
            }
        }

        private void BtnFind_Click(object sender, EventArgs e)
        {
            FindTheBestGunAlignment();
        }

        private void UD_STrim_dVal5_ValueChanged(object sender, EventArgs e)
        {
            UD_STrim_Val5.Increment = UD_STrim_dVal5.Value;
        }

        private void UD_STrim_dVal6_ValueChanged(object sender, EventArgs e)
        {
            UD_STrim_Val6.Increment = UD_STrim_dVal6.Value;
        }

        private void UD_STrim_Val5_ValueChanged(object sender, EventArgs e)
        {
            UpdateScanner();
        }

        private void UD_STrim_Val6_ValueChanged(object sender, EventArgs e)
        {
            UpdateScanner();
        }

        private void ViewPort_DoubleClick(object sender, EventArgs e)
        {
            Point p = ViewPort.PointToClient(Cursor.Position);
            //  MessageBox.Show(p.X.ToString() + "   " + p.Y.ToString());
            ChangeWindow(0, 0, 512, 512);//(decimal)p.X, (decimal)p.Y);
        }

        private void ViewPort_MouseDown(object sender, MouseEventArgs e)
        {
            P1 = new Point((int)(e.X / Image_multiply), (int)(e.Y / Image_multiply));
            StartClick = true;
        }

        private void ViewPort_MouseMove(object sender, MouseEventArgs e)
        {
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

        private void ViewPort_MouseUp(object sender, MouseEventArgs e)
        {

            StartClick = false;
            if (StartMove && (SelectionRec[2].X > SelectionRec[0].X) && (SelectionRec[2].Y > SelectionRec[0].Y))
            {
                StartMove = false;
                ChangeWindow(SelectionRec[0].X, SelectionRec[0].Y, SelectionRec[2].X - SelectionRec[0].X, SelectionRec[2].Y - SelectionRec[0].Y);
            }
        }

        private bool window(decimal wix, decimal wiy, decimal wnx, decimal wny)
        {
            label_winsize.Text = "window(X:{" + wnx.ToString() + "}{Y:" + wny.ToString() + "})";
            string CompleteOrder = "window " + wix.ToString() + " " + wiy.ToString() + " " + wnx.ToString() + " " + wny.ToString() + "\r";
            return SendAndReceiveOK(CompleteOrder);
        }

        private bool ChangeWindow(decimal wix, decimal wiy, decimal wnx, decimal wny)
        {
            try
            {
                //        dactimer(0);
                bool isOK = window(wix, wiy, wnx, wny);
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

        private void button4_Click(object sender, EventArgs e)
        {
            UD_HV_HV.Value = 0; counter = 0;
            string CompleteOrder = CreateChildCommand("hv", "hv 0" + "\r");
            if (SendAndReceiveOK(CompleteOrder) == false) return;
            if (button4.Text == "HV is OFF")
            {
                CompleteOrder = CreateChildCommand("hv", "kv 1" + "\r");
                if (SendAndReceiveOK(CompleteOrder) == true)
                {
                    button4.Text = "HV is ON";
                    button4.BackColor = Color.Coral;
                }
            }
            else
                if (button4.Text == "HV is ON")
            {
                CompleteOrder = CreateChildCommand("hv", "kv 0" + "\r");
                if (SendAndReceiveOK(CompleteOrder) == true)
                {
                    button4.Text = "HV is OFF";
                    button4.BackColor = SystemColors.Menu;
                }
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            //  UD_HV_Filament.Value = 0;
            string CompleteOrder;// = CreateChildCommand("hv", " filament 255" + "\r");
                                 // SendAndReceiveOK(CompleteOrder);
            if (button5.Text == "FB is OFF")
            {
                CompleteOrder = CreateChildCommand("hv", "fb 1" + "\r");
                if (SendAndReceiveOK(CompleteOrder) == true)
                {
                    button5.Text = "FB is ON";
                    button5.BackColor = Color.Coral;
                }
            }
            else
                if (button5.Text == "FB is ON")
            {
                CompleteOrder = CreateChildCommand("hv", "fb 0" + "\r");
                if (SendAndReceiveOK(CompleteOrder) == true)
                {
                    button5.Text = "FB is OFF";
                    button5.BackColor = SystemColors.Menu;
                }
            }
            UD_HV_Filament.Value = 0;
        }

        private void blink_timer_Tick(object sender, EventArgs e)
        {
            //
            if (button4.Text == "HV is ON")
                button4.BackColor = Color.FromArgb((Math.Abs(counter) * 255) / 50 + 50, 255, 0, 0);
            if (button5.Text == "FB is ON")
                button5.BackColor = Color.FromArgb((Math.Abs(counter) * 255) / 50 + 50, 0, 255, 0);
            if (buttonHV.Text == "HV is ON")
                buttonHV.BackColor = Color.FromArgb((Math.Abs(counter) * 255) / 50 + 50, 255, 0, 0);
            if (buttonFB.Text == "HEAT is ON")
                buttonFB.BackColor = Color.FromArgb((Math.Abs(counter) * 255) / 50 + 50, 0, 255, 0);
            if ((counter++) >= 25) counter = -25;
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Logout();

            Properties.Settings.Default.m2 = "000";
            Properties.Settings.Default.Save();
            //Application.Exit();
            Properties.Settings s = new Properties.Settings();
            s.m2 = "1000";
            s.Save();
        }

        private void ic_teta_ValueChanged(object sender, EventArgs e)
        {
            UD_Lens_icx_ValueChanged(sender, e);
            // UD_Lens_icy_ValueChanged(sender, e);
        }

        private void ga_teta_ValueChanged(object sender, EventArgs e)
        {
            UD_Lens_ux_ValueChanged(sender, e);
            UD_Lens_uy_ValueChanged(sender, e);
            UD_Lens_dx_ValueChanged(sender, e);
            UD_Lens_dy_ValueChanged(sender, e);
        }

        private void groupBox16_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox9_Enter(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void BtnSingleShot_Click(object sender, EventArgs e)
        {
            oldspeedVal = UD_Speed.Value;
            int nstep = (int)SSNumStep.Value;
            myMatrix = new byte[nstep, nstep];
            SSPacketCnt = 0;
            SSRow = 0;
            SingleShotnStep = nstep;
            SingleShotMode = true;
            StartSingleShot();
            //imgfrm.pictureBox1.Image = bmp;
            //
            if (imgfrm.IsDisposed)
                imgfrm = new PictureForm();
            imgfrm.Size = new Size(500, 500);
            imgfrm.Show();
            imgfrm.BringToFront();
            //imgfrm.Update();
            //imgfrm.Refresh();

        }

        private void SwipSecondTerminal(int ix, int iy, int n)
        {
            int dxy = (int)(4096 / n / 2);
            decimal vx = (2 * ix + 1) * dxy;
            decimal vy = (2 * iy + 1) * dxy;
            if (MultiShotTerminal == (decimal)0)
                lens_gau(vx, vy);
            else if (MultiShotTerminal == (decimal)1)
                lens_gad(vx, vy);
            else if (MultiShotTerminal == (decimal)2)
                lens_ic(vx, vy);
            else if (MultiShotTerminal == (decimal)3)
                lens_stig(vx, vy);
        }

        private void StartSingleShot()
        {
            if (MultiShotMode)
            {
                int nm = (int)numericUpDown2.Value;
                int imatx = ((iMultiScan - 1) % nm);
                int imaty = (int)((iMultiScan - 1 - imatx) / nm);
                //Thread.Sleep(100);
                SwipSecondTerminal(imatx, imaty, nm);
            }

            // decimal SSSpeedValue = SSSpeed.Value;
            // UD_Speed.Value = SSSpeedValue;
            // Thread.Sleep(10000);
            decimal terminal = (decimal)0;
            if (radioButton0.Checked) terminal = (decimal)1;
            else if (radioButton1.Checked) terminal = (decimal)0;
            else if (radioButton2.Checked) terminal = (decimal)2;
            else if (radioButton3.Checked) terminal = (decimal)3;

            decimal xmin = SSxmin.Value;
            decimal ymin = SSymin.Value;
            int window = (int)SSWindow.Value;
            int nstep = (int)SSNumStep.Value;

            double dxy = (double)window / ((double)nstep);
            int idxy = (int)dxy;

            timerSSEndCheck.Start();

            int xmin10 = (int)(xmin / 10);
            int ymin10 = (int)(ymin / 10);
            ss(terminal, (decimal)nstep, (decimal)idxy, (decimal)xmin10, (decimal)ymin10);
        }

        private void ss(decimal terminal, decimal stepnumber, decimal step, decimal xmin, decimal ymin)
        {
            string CompleteOrder = CreateChildCommand("l", "ss " + terminal.ToString("0") + " " + stepnumber.ToString("000") + " " + step.ToString("000") + " " + xmin.ToString("000") + " " + ymin.ToString("000") + "\r");
            //string CompleteOrder = CreateChildCommand("l", "ss " + terminal.ToString() + " " + stepnumber.ToString() + " " + step.ToString("000") + "\r");
            string response = SendAndReceiveResponse(CompleteOrder);
        }

        private void SSWindow_ValueChanged(object sender, EventArgs e)
        {
            CheckWindow();
        }

        private void CheckWindow()
        {
            int maxxy = (int)SSxmin.Value;
            int ymin0 = (int)SSymin.Value;
            int window = (int)SSWindow.Value;
            if (maxxy < ymin0) maxxy = ymin0;
            if (window + maxxy > 4095) window = 4095 - maxxy;
            SSWindow.Value = (decimal)window;
        }

        private void SSxmin_ValueChanged(object sender, EventArgs e)
        {
            CheckWindow();
        }

        private void SSymin_ValueChanged(object sender, EventArgs e)
        {
            CheckWindow();
        }

        private void ShowSingleShot()
        {
            //now we have to convert the 2 dimensional array into a one dimensional byte-array for use with 8bpp bitmaps
            int nmat1 = myMatrix.GetLength(0);
            int nmat2 = myMatrix.GetLength(1);
            byte[] pixels = new byte[nmat1 * nmat2];
            for (int y = 0; y < nmat1; y++)
            {
                for (int x = 0; x < nmat2; x++)
                {
                    pixels[y * nmat1 + x] = myMatrix[x, y];
                }
            }

            //create a new Bitmap
            Bitmap bmp = new Bitmap(myMatrix.GetLength(0), myMatrix.GetLength(1), System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

            System.Drawing.Imaging.ColorPalette pal = bmp.Palette;

            //create grayscale palette
            for (int i = 0; i < 256; i++)
            {
                pal.Entries[i] = Color.FromArgb((int)255, i, i, i);
            }

            //assign to bmp
            bmp.Palette = pal;

            //lock it to get the BitmapData Object
            System.Drawing.Imaging.BitmapData bmData =
                bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);

            //copy the bytes
            System.Runtime.InteropServices.Marshal.Copy(pixels, 0, bmData.Scan0, bmData.Width * bmData.Height);

            //never forget to unlock the bitmap
            bmp.UnlockBits(bmData);

            //display
            //this.pictureBox1.Image = bmp;
            //PictureForm imgfrm = new PictureForm();
            imgfrm.pictureBox1.Image = bmp;
            imgfrm.Size = new Size(500, 500);
            //imgfrm.Show();
            imgfrm.Update();
            imgfrm.Refresh();

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            SSSpeed.Increment = SSdSpeed.Value;
        }

        private void timerSSEndCheck_Tick(object sender, EventArgs e)
        {
            if (!SingleShotMode)
            {
                timerSSEndCheck.Stop();
                ShowSingleShot();

                if (MultiShotMode)
                {
                    if (iMultiScan != MultiShotnStep)
                    {
                        iMultiScan++;
                        SSPacketCnt = 0;
                        SSRow = 0;
                        SingleShotMode = true;
                        StartSingleShot();
                    }
                }
            }

        }

        private bool u2itmode(decimal val)
        {
            string CompleteOrder = "u2itmode " + val.ToString() + "\r";
            return SendAndReceiveOK(CompleteOrder);
        }

        private bool u6itmode(decimal val)
        {
            string CompleteOrder = "u6itmode " + val.ToString() + "\r";
            return SendAndReceiveOK(CompleteOrder);
        }

        private void Uitmode_CheckedChanged(object sender, EventArgs e)
        {
            //  if (U2itmode.Checked)
            u2itmode(1);
            //  else
            u2itmode(0);
        }

        private void Upitmode_CheckedChanged(object sender, EventArgs e)
        {
            //  if (U6itmode.Checked)
            u6itmode(1);
            //  else
            u6itmode(0);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            // log.Update();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            radioButton_0.Enabled = false;
            radioButton_1.Enabled = false;
            radioButton_2.Enabled = false;
            radioButton_3.Enabled = false;

            decimal terminal = (decimal)0;
            if (radioButton_0.Checked)
            {
                terminal = (decimal)1;
                radioButton_0.Enabled = true;
            }
            else if (radioButton_1.Checked)
            {
                terminal = (decimal)0;
                radioButton_1.Enabled = true;
            }
            else if (radioButton_2.Checked)
            {
                terminal = (decimal)2;
                radioButton_2.Enabled = true;
            }
            else if (radioButton_3.Checked)
            {
                terminal = (decimal)3;
                radioButton_3.Enabled = true;
            }

            if (button6.Text == "Start")
            {
                cs(CS_Window.Value, terminal);
                button6.Text = "Stop";
            }
            else
            {
                radioButton_0.Enabled = true;
                radioButton_1.Enabled = true;
                radioButton_2.Enabled = true;
                radioButton_3.Enabled = true;
                cs((decimal)0, terminal);
                button6.Text = "Start";
            }
        }

        private void cs(decimal window, decimal terminal)
        {
            string CompleteOrder = CreateChildCommand("l", "cs " + window.ToString("000") + " " + terminal.ToString() + "\r");
            //string CompleteOrder = CreateChildCommand("l", "ss " + terminal.ToString() + " " + stepnumber.ToString() + " " + step.ToString("000") + "\r");
            string response = SendAndReceiveResponse(CompleteOrder);
        }

        private void panel20_Scroll(object sender, ScrollEventArgs e)
        {


        }

        private void panel20_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel20_MouseCaptureChanged(object sender, EventArgs e)
        {
            counter++;
            if (counter > 255 || counter < 0) counter = 0;
            // IML_Pad.BackColor = Color.FromArgb(255,counter, 100);
        }

        private void radioButton0_CheckedChanged(object sender, EventArgs e)
        {
            radioButton2_0.Enabled = false;
            radioButton2_1.Enabled = true;
            radioButton2_1.Checked = true;
            radioButton2_2.Enabled = true;
            radioButton2_3.Enabled = true;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            radioButton2_0.Enabled = true;
            radioButton2_0.Checked = true;
            radioButton2_1.Enabled = false;
            radioButton2_2.Enabled = true;
            radioButton2_3.Enabled = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            radioButton2_0.Enabled = true;
            radioButton2_0.Checked = true;
            radioButton2_1.Enabled = true;
            radioButton2_2.Enabled = false;
            radioButton2_3.Enabled = true;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            radioButton2_0.Enabled = true;
            radioButton2_0.Checked = true;
            radioButton2_1.Enabled = true;
            radioButton2_2.Enabled = true;
            radioButton2_3.Enabled = false;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            oldspeedVal = UD_Speed.Value;

            if (radioButton2_0.Checked) MultiShotTerminal = (decimal)1;
            else if (radioButton2_1.Checked) MultiShotTerminal = (decimal)0;
            else if (radioButton2_2.Checked) MultiShotTerminal = (decimal)2;
            else if (radioButton2_3.Checked) MultiShotTerminal = (decimal)3;

            int nscan = (int)numericUpDown2.Value;
            int nstep = (int)SSNumStep.Value;
            myMatrix = new byte[nstep * nscan, nstep * nscan];
            SSPacketCnt = 0;
            SSRow = 0;
            SingleShotnStep = nstep;
            SingleShotMode = true;
            iMultiScan = 1;
            MultiShotnStep = nscan * nscan;
            MultiShotMode = true;
            StartSingleShot();
            if (imgfrm.IsDisposed)
                imgfrm = new PictureForm();
            imgfrm.Size = new Size(500, 500);
            imgfrm.Show();
            imgfrm.BringToFront();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            UD_Lens_icy.Value = 0;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            UD_Lens_stigx.Value = 0;

        }

        private void button10_Click(object sender, EventArgs e)
        {
            UD_Lens_icx.Value = 0;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            UD_Lens_stigy.Value = 0;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            UD_Lens_shiftx.Value = 0;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            UD_Lens_shifty.Value = 0;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            UD_Lens_tiltx.Value = 0;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            UD_Lens_tilty.Value = 0;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Scroll(object sender, ScrollEventArgs e)
        {

        }
        private void panel1_MouseWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            // System.Windows.Input.Touch a;
            //this.Text = e.X.ToString() + "|" + e.X.ToString() + "|" + e.Delta.ToString() + "|" + e.Clicks.ToString();

        }

        private void userControl1BindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void userControl1BindingSource1_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void userControl11_Load(object sender, EventArgs e)
        {

        }
        private void u1_valueChanged(object sender, EventArgs e)
        {
            //    this.Text = userControl11.X.ToString() +"|" +userControl11.Y.ToString();
            this.UD_Lens_icx.Value = userControl11.X;
            this.UD_Lens_icy.Value = userControl11.Y;

        }
        private void u2_valueChanged(object sender, EventArgs e)
        {
            this.UD_Lens_stigx.Value = userControl12.X;
            this.UD_Lens_stigy.Value = userControl12.Y;
        }
        private void u3_valueChanged(object sender, EventArgs e)
        {
            this.UD_STrim_Val1.Value = userControl13.X;
            this.UD_STrim_Val3.Value = userControl13.Y;
        }
        private void u4_valueChanged(object sender, EventArgs e)
        {
            this.UD_Lens_shiftx.Value = userControl14.X;
            this.UD_Lens_shifty.Value = userControl14.Y;
        }
        private void u5_valueChanged(object sender, EventArgs e)
        {
            this.UD_Lens_tiltx.Value = userControl15.X;
            this.UD_Lens_tilty.Value = userControl15.Y;
        }
        private void IMLLock_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void UDilmconx_ValueChanged(object sender, EventArgs e)
        {

        }

        private void groupBox29_Enter(object sender, EventArgs e)
        {

        }

        private void userControl21_Load(object sender, EventArgs e)
        {

        }

        private void button16_Click(object sender, EventArgs e)
        {
            // new Socket()
            //InTheHand.Net.
            timer2.Start();

        }

        private void userControl13_Load(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //int n;
            // btstream.ReadAsync(bt_buf, 1, 2);remoteDevice.Available
            // textBox3.Text += "a";
            // int x[3];//, y;
            //int i = 0;
            /*   while (btstream.DataAvailable)
               {
                   if (index == 0)
                   {
                       if (btstream.ReadByte() == 127)
                           index = 2;
                   }
                   else
                   {
                       bt_buf[index--] = (sbyte)btstream.ReadByte();
                       if(index == 0)
                       {
                           userControl21.X -= 2*((int)bt_buf[2]);
                           userControl21.Y -= 2*((int)bt_buf[1]);
                           textBox3.Text += (sbyte)bt_buf[2] + "|";
                       }
                   }

                   //bt_buf[i++] = (char)btstream.ReadByte();
                 //  textBox3.Text += btstream.ReadByte() + "_";
                   // MessageBox.Show("jjj");
                  // i++;
               }*/
            //  if (i==3) 
            //textBox3.Text += "|";//.ToString();*/
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }


        void AcceptConnection(IAsyncResult result)
        {
            // textBox3.Text += result.ToString();
            // MessageBox.Show(result.ToString());
            if (result.IsCompleted)
            {
                if (btstream != null)
                {
                    //btstream.EndRead(result);
                    //btstream.Dispose();
                }
                if (remoteDevice != null)
                {
                    remoteDevice.Close();
                    remoteDevice.Dispose();
                }


                remoteDevice = ((BluetoothListener)result.AsyncState).EndAcceptBluetoothClient(result);


                // textBox3.Text += remoteDevice.RemoteMachineName + " | ";
                // MessageBox.Show(remoteDevice.RemoteMachineName);
                //remoteDevice.RemoteEndPoint
                // MessageBox.Show();

                //notifyIcon1.Text = 
                notifyIcon1.BalloonTipText = "Connected to:" + remoteDevice.RemoteMachineName;
                notifyIcon1.ShowBalloonTip(2000);
                // bt_buf[0] = (char)50;
                //  remoteDevice.Client.Send(bt_buf);
                // remoteDevice.SetPin("0000");
                //MessageBox.Show(remoteDevice.InquiryAccessCode.ToString() + remoteDevice.LinkPolicy.ToString());
                btstream = remoteDevice.GetStream();
                btstream.ReadTimeout = 100;
                // bt_streamreader = new StreamReader(btstream);
                btstream.BeginRead(bt_buf, 0, 1, new AsyncCallback(bt_RecieveComplete), btstream);

            }
            ((BluetoothListener)result.AsyncState).BeginAcceptBluetoothClient(new AsyncCallback(AcceptConnection), (BluetoothListener)result.AsyncState);
        }


        public void bt_RecieveComplete(IAsyncResult result)
        {
            btstream.EndRead(result);

            if ((index >= 10))
            {
                index = 1;
                remoteDevice.Close();
                remoteDevice.Dispose();
                btstream.Flush();
                btstream.Close();
                btstream.Dispose();
                return;
            }

            if (bt_buf[0] == 127)
            {
                btstream.BeginRead(bt_buf, 0, 4, new AsyncCallback(bt_RecieveComplete), btstream);
                index = 0;
            }
            else
            {
                if (index == 0)
                {
                    if (!backgroundWorker2.IsBusy)
                        backgroundWorker2.RunWorkerAsync();
                    index = 1;
                }
                else
                    index++;
                btstream.BeginRead(bt_buf, 0, 1, new AsyncCallback(bt_RecieveComplete), btstream);
            }


        }

        private void speedper_ValueChanged(object sender, EventArgs e)
        {
            UDSpeed_ValueChanged(sender, e);
        }

        private void label90_Click(object sender, EventArgs e)
        {

        }

        private void clkdelay_ValueChanged(object sender, EventArgs e)
        {
            string CompleteOrder = "adctime " + clkdelay.Value.ToString() + " " + clkdelay.Value.ToString() + "\r";
            SendAndReceiveOK(CompleteOrder);
        }

        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            UD_Lens_CON2.Value = (decimal)(int)((double)UD_Lens_CON1.Value * 3.0 / (2.0 * 0.62) / ((double)numericUpDown7.Value - (0.12 / 10.0) * ((double)numericUpDown11.Value - 10)));

            numericUpDown13.ValueChanged -= numericUpDown13_ValueChanged_1;
            numericUpDown13.Value = numericUpDown11.Value;
            numericUpDown13.ValueChanged += numericUpDown13_ValueChanged_1;
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            UD_Lens_CON2.Value = (decimal)(int)((double)UD_Lens_CON1.Value * 3.0 / (2.0 * 0.62) / ((double)numericUpDown7.Value - (0.12 / 10.0) * ((double)numericUpDown11.Value - 10)));

            numericUpDown14.ValueChanged -= numericUpDown14_ValueChanged;
            numericUpDown14.Value = numericUpDown7.Value;
            numericUpDown14.ValueChanged += numericUpDown14_ValueChanged;
        }

        private void log_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            log.Clear();
        }

        private void b_form_Click(object sender, EventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            textBox4.Text = "aaaaaaaaaaaaa";// hidstring;
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Yessssssssssssssssss");
        }

        private void ToolsPicture_Paint(object sender, PaintEventArgs e)
        {

            // Create string to draw.
            String drawString = "SEM Technology" + UserInfo;

            // Create font and brush.
            Font drawFont = new Font("Arial", 11, FontStyle.Bold);

            SolidBrush drawBrush = new SolidBrush(Color.FromArgb(255, 255, 255));

            // Create point for upper-left corner of drawing.
            int x0 = 5;
            int y0 = 7;
            float x = 59.0F;
            float y = 8.0F;

            // Set format of string.
            StringFormat drawFormat = new StringFormat();
            drawFormat.FormatFlags = StringFormatFlags.NoWrap;

            // Draw string to screen.
            e.Graphics.DrawString(drawString, drawFont, drawBrush, x, y, drawFormat);
            //e.Graphics.DrawImage(System.Drawing.Image.FromFile("./QuantaEye.png"), (int)x0, (int)y0,50,20);
            e.Graphics.DrawImage(new Bitmap(Properties.Resources.QuantaEye), (int)x0, (int)y0, 50, 20);
        }

        bool FirstDoubleClick = false;
        private void ToolsPicture_DoubleClick(object sender, EventArgs e)
        {
            if (FirstDoubleClick)
            {
                if (this.WindowState == FormWindowState.Maximized)
                    LeaveFullScreenMode();
                else
                    EnterFullScreenMode();
            }

            FirstDoubleClick = !FirstDoubleClick;
        }

        private void EnterFullScreenMode()
        {
            //this.Hide();
            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            //this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //this.WindowState = FormWindowState.Normal;
            this.ControlBox = true;
            //this.ControlBox = true;
            this.WindowState = FormWindowState.Maximized;
            this.ControlBox = false;
            //this.ControlBox = false;
            //this.Show();
        }

        private void LeaveFullScreenMode()
        {
            this.WindowState = FormWindowState.Normal;
        }

        int mouseX = 0; int mouseY = 0; int thisLocationX = 0; int thisLocationY = 0; bool isMove = false;
        private double Image_multiply = 1.1;
        private int counter_hv;

        private void ToolsPicture_MouseDown(object sender, MouseEventArgs e)
        {
            isMove = true;
            //mouseX = e.X;
            //mouseY = e.Y;
            mouseX = System.Windows.Forms.Control.MousePosition.X;
            mouseY = System.Windows.Forms.Control.MousePosition.Y;
            thisLocationX = this.Location.X;
            thisLocationY = this.Location.Y;
        }

        private void ToolsPicture_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMove)
            {
                int dx = System.Windows.Forms.Control.MousePosition.X - mouseX;
                int dy = System.Windows.Forms.Control.MousePosition.Y - mouseY;
                //richTextBox1.AppendText(mouseX.ToString() + " " + mouseY.ToString() + " " + System.Windows.Forms.Control.MousePosition.X.ToString() + " " + System.Windows.Forms.Control.MousePosition.Y.ToString() + " " + dx.ToString() + " " + dy.ToString() + "\n");
                //this.SetDesktopLocation(this.Location.X + dx, this.Location.Y + dy);
                this.Location = new Point(thisLocationX + dx, thisLocationY + dy);
            }
        }

        private void ToolsPicture_MouseUp(object sender, MouseEventArgs e)
        {
            isMove = false;
        }

        private void FormMain_Resize(object sender, EventArgs e)
        {
            ResizeApp();
        }

        private void ResizeApp()
        {
            ResizeControlPanel();
        }

        private void ResizeControlPanel()
        {
            int h0 = this.ClientSize.Height;
            //int w0 = this.ClientSize.Width;
            group1.Height = h0 - 20 - panel1.Height - Border.Height;
            //buttompanel.Width = w0 - 15 - panel1.Width - rightpanel.Width;
        }

        private void button3_MouseEnter(object sender, EventArgs e)
        {
            button3.ForeColor = Color.OrangeRed;
        }

        private void button3_MouseLeave(object sender, EventArgs e)
        {
            button3.ForeColor = Color.White;
        }

        private void label92_MouseEnter(object sender, EventArgs e)
        {
            label92.ForeColor = Color.OrangeRed;
        }

        private void label92_MouseLeave(object sender, EventArgs e)
        {
            label92.ForeColor = Color.Moccasin;
        }

        private void label92_MouseDown(object sender, MouseEventArgs e)
        {
            label92.ForeColor = Color.White;
        }

        private void label92_Click(object sender, EventArgs e)
        {
            isformmode = true;
            formmode = new ImageForm();
            formmode.Owner = this;
            formmode.isthisviewport = true;
            formmode.Text = "Render";
            formmode.Show();
            /*  MCvFont f = new MCvFont(FONT.CV_FONT_HERSHEY_COMPLEX, 0.4, .4);
              string info = String.Format("viewport");
              frame.Draw(info, ref f, new System.Drawing.Point(227, 247), new Gray(200)); //Draw on the image using the specific font
              formmode.ViewPort.Image = frame;
              */
        }

        private void Btn_Acquire_MouseEnter(object sender, EventArgs e)
        {
            Btn_Acquire.ForeColor = Color.OrangeRed;
        }

        private void Btn_Acquire_MouseLeave(object sender, EventArgs e)
        {
            Btn_Acquire.ForeColor = Color.White;
        }

        private void Ctrl1D_Zoom_Enter(object sender, EventArgs e)
        {
            TurnOn(ZoomLight, 1);
        }

        private void Ctrl1D_Zoom_Leave(object sender, EventArgs e)
        {
            TurnOff(ZoomLight);
        }

        private void Ctrl1D_Focus_Enter(object sender, EventArgs e)
        {
            TurnOn(FocusLight, 0);
        }

        private void Ctrl1D_Focus_Leave(object sender, EventArgs e)
        {
            TurnOff(FocusLight);
        }

        private void Ctrl2D_Gain_Enter(object sender, EventArgs e)
        {
            TurnOn(GainLight, 3);
        }

        private void Ctrl2D_Gain_Leave(object sender, EventArgs e)
        {
            TurnOff(GainLight);
        }

        private void Ctrl2D_IMLCentering_Enter(object sender, EventArgs e)
        {
            TurnOn(IMLCenteringLight, 5);
        }

        private void Ctrl2D_IMLCentering_Leave(object sender, EventArgs e)
        {
            TurnOff(IMLCenteringLight);
        }

        private void Ctrl2D_Stig_Enter(object sender, EventArgs e)
        {
            TurnOn(StigLight, 2);
        }

        private void Ctrl2D_Stig_Leave(object sender, EventArgs e)
        {
            TurnOff(StigLight);
        }

        private void Ctrl2D_ObjectCentering_Enter(object sender, EventArgs e)
        {
            TurnOn(ObjectCenteringLight, 4);
        }

        private void Ctrl2D_ObjectCentering_Leave(object sender, EventArgs e)
        {
            TurnOff(ObjectCenteringLight);
        }

        private void Ctrl2D_GunShift_Enter(object sender, EventArgs e)
        {
            TurnOn(GunShiftLight, 6);
        }

        private void Ctrl2D_GunShift_Leave(object sender, EventArgs e)
        {
            TurnOff(GunShiftLight);
        }

        private void Ctrl2D_GunTilt_Enter(object sender, EventArgs e)
        {
            TurnOn(GunTiltLight, 7);
        }

        private void Ctrl2D_GunTilt_Leave(object sender, EventArgs e)
        {
            TurnOff(GunTiltLight);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            SelectedLightControl = -1;
            if (isAdvancedMode)
            {
                isAdvancedMode = false;
                TabControl_Main.Visible = false;
                tabPage3.AutoScroll = true;
                // leftpanel.Width = 376;
                nLightControl = 8;
                button17.Text = "Advanced mode";
            }
            else
            {
                isAdvancedMode = true;
                TabControl_Main.Visible = true;
                // leftpanel.Width = 5;
                nLightControl = 5;
                button17.Text = "Standard mode";
            }
        }

        private void userControl11_Enter(object sender, EventArgs e)
        {
            TurnOn(pictureBox1, 0);
        }

        private void userControl11_Leave(object sender, EventArgs e)
        {
            TurnOff(pictureBox1);
        }

        private void userControl12_Enter(object sender, EventArgs e)
        {
            TurnOn(pictureBox2, 1);
        }

        private void userControl12_Leave(object sender, EventArgs e)
        {
            TurnOff(pictureBox2);
        }

        private void userControl14_Enter(object sender, EventArgs e)
        {
            TurnOn(pictureBox3, 2);
        }

        private void userControl14_Leave(object sender, EventArgs e)
        {
            TurnOff(pictureBox3);
        }

        private void userControl15_Enter(object sender, EventArgs e)
        {
            TurnOn(pictureBox4, 3);
        }

        private void userControl15_Leave(object sender, EventArgs e)
        {
            TurnOff(pictureBox4);
        }

        private void userControl13_Enter(object sender, EventArgs e)
        {
            TurnOn(pictureBox5, 4);
        }

        private void userControl13_Leave(object sender, EventArgs e)
        {
            TurnOff(pictureBox5);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.m2 = "000";
            Properties.Settings.Default.Save();
            //Application.Exit();
            Properties.Settings s = new Properties.Settings();
            s.m2 = "1000";
            s.Save();
        }

        private void ToolsPicture_Click(object sender, EventArgs e)
        {

        }

        private void Ctrl2D_IMLCentering_Load(object sender, EventArgs e)
        {

        }

        private void Ctrl2D_Gain_Load(object sender, EventArgs e)
        {

        }

        private void groupBox38_Enter(object sender, EventArgs e)
        {

        }

        private void c_focus(object sender, EventArgs e)
        {
            //  group3.ScrollControlIntoView(((GroupBox)sender));
            //   group3.VerticalScroll.Value = 300;
            //group3.VerticalScroll.Value = (int)(Math.Round((double)group3.VerticalScroll.Value / 300.0) * 300 );
        }

        private void group3_Scroll(object sender, ScrollEventArgs e)
        {

        }

        private void leftpanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Ctrl1D_Zoom_Scroll(object sender, EventArgs e)
        {
            UpdateZoom();
        }

        private void UpdateZoom()
        {
            int mode = MicroscopyMode.SelectedIndex;
            double vf = Calculate_Vf(mode);

            numericZoom.ValueChanged -= numericZoom_ValueChanged;
            numericZoom.Value = (decimal)(100 / vf);
            numericZoom.ValueChanged += numericZoom_ValueChanged;
            numericVF.ValueChanged -= numericVF_ValueChanged;
            numericVF.Value = (decimal)vf;
            numericVF.ValueChanged += numericVF_ValueChanged;

            double vf_log = Math.Log(vf / 3, 10);
            double vf_deci = Math.Pow(10, Math.Floor(vf_log));
            if (((vf / 3) / vf_deci) > 2)
            {
                vf_deci *= 2;
            }


            scalelabel.Text = vf_deci.ToString() + "um";
            scalebar.Width = (int)(panel1.Width / 3 * vf_deci / (vf / 3));
            labelscalekx.Text = numericZoom.Value.ToString("0.0") + "kx";
        }

        private void numericZoom_ValueChanged(object sender, EventArgs e)
        {
            int mode = MicroscopyMode.SelectedIndex;
            double vf = 100.0 / (double)numericZoom.Value;
            int zoomval = Vf_Zoom(mode, vf);
            Ctrl1D_Zoom.Value = zoomval;
        }

        private void numericVF_ValueChanged(object sender, EventArgs e)
        {
            int mode = MicroscopyMode.SelectedIndex;
            double vf = (double)numericVF.Value;
            int zoomval = Vf_Zoom(mode, vf);
            Ctrl1D_Zoom.Value = zoomval;
        }

        private int Vf_Zoom(int mode, double vf)
        {
            int HVindex = HVProfile.SelectedIndex;
            double zoom;
            //double vf= Settings1.Default.vf_max /Math.Pow(10, zoom);//vf_max in zoom_min
            //double I= Settings1.Default.I_max *vf/ Settings1.Default.vf_max;
            if (mode == 0) //Resolution mode
            {
                double I_log = Math.Log10(vf / AllUserSettings[HVindex].WD_real * Settings1.Default.scan_d / Settings1.Default.vf_max / Math.Sqrt(Settings1.Default.kV_max) * Math.Pow(10, Settings1.Default.LogI_Max_Resolution) * Math.Sqrt(AllUserSettings[HVindex].HV));
                scanner_current_log(I_log);
                zoom = (I_log - Settings1.Default.LogI_Max_Resolution) * Ctrl1D_Zoom.Maximum / (-Settings1.Default.LogI_Max_Resolution + Settings1.Default.LogI_Min_Resolution);
            }
            else if (mode == 1) //Wide-Field mode
            {
                double I_log = Math.Log10(vf / AllUserSettings[HVindex].WD_real * Settings1.Default.scan_d / Settings1.Default.vf_max / Math.Sqrt(Settings1.Default.kV_max) * Math.Pow(10, Settings1.Default.LogI_Max_WideField) * Math.Sqrt(AllUserSettings[HVindex].HV));
                scanner_current_log(I_log);
                zoom = (I_log - Settings1.Default.LogI_Max_WideField) * Ctrl1D_Zoom.Maximum / (-Settings1.Default.LogI_Max_WideField + Settings1.Default.LogI_Min_WideField);
            }
            else if (mode == 2) //Field mode
            {
                double I_log = Math.Log10(vf / AllUserSettings[HVindex].WD_real * Settings1.Default.scan_d / Settings1.Default.vf_max / Math.Sqrt(Settings1.Default.kV_max) * Math.Pow(10, Settings1.Default.LogI_Max_Field) * Math.Sqrt(AllUserSettings[HVindex].HV));
                scanner_current_log(I_log);
                zoom = (I_log - Settings1.Default.LogI_Max_Field) * Ctrl1D_Zoom.Maximum / (-Settings1.Default.LogI_Max_Field + Settings1.Default.LogI_Min_Field);
            }
            else //Rokveld mode
            {
                double I_log = Math.Log10(vf / AllUserSettings[HVindex].WD_real * Settings1.Default.scan_d / Settings1.Default.vf_max / Math.Sqrt(Settings1.Default.kV_max) * Math.Pow(10, Settings1.Default.LogI_Max_Rokveld) * Math.Sqrt(AllUserSettings[HVindex].HV));
                scanner_current_log(I_log);
                zoom = (I_log - Settings1.Default.LogI_Max_Rokveld) * Ctrl1D_Zoom.Maximum / (-Settings1.Default.LogI_Max_Rokveld + Settings1.Default.LogI_Min_Rokveld);
            }

            int izoom = (int)Math.Round(zoom);
            return izoom;
        }

        private void scanner_current_log(double i_log)
        {
            double I = Math.Pow(10, i_log);
            int newindex;
            if (I >= (100 * Settings1.Default.I_scale_max))
                newindex = 3;
            else if (I >= (10 * Settings1.Default.I_scale_max))
                newindex = 2;
            else
                newindex = 1;

            if (newindex < Settings1.Default.Scanner_ISelect_Min) newindex = Settings1.Default.Scanner_ISelect_Min;
            if (newindex > Settings1.Default.Scanner_ISelect_Max) newindex = Settings1.Default.Scanner_ISelect_Max;

            Scanner_ISelect.SelectedIndex = newindex;

            UD_Zoom.Value = (decimal)(4095 * I / (Settings1.Default.I_scale_max * Math.Pow(10, Scanner_ISelect.SelectedIndex)));
        }

        private double Calculate_Iobj()
        {
            double f_1 = (1.0 / Settings1.Default.f_min) * Math.Pow(Ctrl1D_Focus.Value, 2) / Math.Pow(Ctrl1D_Focus.Maximum, 2) * (Settings1.Default.kV_max) / (Settings1.Default.kV);
            double q_1 = f_1 - (1.0 / Settings1.Default.p);
            if (q_1 <= (1.0 / 30.0)) q_1 = 1.0 / 30.0;
            double WD_real = 1.0 / q_1;
            return WD_real;
        }

        private double Focus_To_WDreal(int mode)
        {
            double WD_real;
            double IObj;
            double IIML;
            if (mode == 0) //Resolution mode
            {
                IObj = Settings1.Default.Coef_res_fine * Ctrl1D_Focus.Value + Settings1.Default.Coef_res_course * trackBar_focus_course.Value;
                IIML = 0.0;
                double IObj_max = Settings1.Default.Coef_res_fine * Settings1.Default.Focus_Max_res_fine + Settings1.Default.Coef_res_course * Settings1.Default.Focus_Max_res_course;
                double f_1 = (1.0 / Settings1.Default.f_min) * Math.Pow(IObj, 2) / Math.Pow(IObj_max, 2) * (Settings1.Default.kV_max) / (Settings1.Default.kV);
                double q_1 = f_1 - (1.0 / Settings1.Default.p);
                if (q_1 <= (1.0 / Settings1.Default.q_max)) q_1 = 1.0 / Settings1.Default.q_max;
                WD_real = 1.0 / q_1;
            }
            else if (mode == 1) //Wide-Field mode
            {
                IObj = Settings1.Default.IObj_WF;
                IIML = Settings1.Default.Coef_IML_fine * Ctrl1D_Focus.Value;
                WD_real = Settings1.Default.WD_real_IML;
            }
            else if (mode == 2) //Field mode
            {
                IObj = 0;
                IIML = Settings1.Default.Coef_IML_fine * Ctrl1D_Focus.Value;
                WD_real = Settings1.Default.WD_real_IML;
            }
            else //Rokveld mode
            {
                IObj = 0;
                IIML = Settings1.Default.Coef_IML_fine * Ctrl1D_Focus.Value;
                WD_real = Settings1.Default.WD_real_IML;
            }

            AllUserSettings[mode].IObj = IObj;
            AllUserSettings[mode].IIML = IIML;

            return WD_real;
        }

        private void WDreal_To_Focus(int mode, double WD_real, ref int FocusCourse, ref int FocusFine)
        {
            double IObj = 0;
            double IIML = 0;
            if (mode == 0) //Resolution mode
            {
                double q_1 = 1.0 / WD_real;
                //if (q_1 <= (1.0 / Settings1.Default.q_max)) q_1 = 1.0 / Settings1.Default.q_max;
                double f_1 = q_1 + (1.0 / Settings1.Default.p);
                double IObj_max = Settings1.Default.Coef_res_fine * Settings1.Default.Focus_Max_res_fine + Settings1.Default.Coef_res_course * Settings1.Default.Focus_Max_res_course;
                IObj = Math.Pow(f_1 * (Settings1.Default.kV) / (Settings1.Default.kV_max) * Math.Pow(IObj_max, 2) * Settings1.Default.f_min, 0.5);
                int FocusFineMiddle = (int)(Ctrl1D_Focus.Maximum / 2);
                FocusCourse = (int)Math.Round((IObj - Settings1.Default.Coef_res_fine * FocusFineMiddle) / Settings1.Default.Coef_res_course);
                FocusFine = (int)((IObj - Settings1.Default.Coef_res_course * FocusCourse) / Settings1.Default.Coef_res_fine);
            }
            else if (mode == 1) //Wide-Field mode
            {
                FocusFine = (int)(IIML / Settings1.Default.Coef_IML_fine);
            }
            else if (mode == 2) //Field mode
            {
                FocusFine = (int)(IIML / Settings1.Default.Coef_IML_fine);
            }
            else //Rokveld mode
            {
                FocusFine = (int)(IIML / Settings1.Default.Coef_IML_fine);
            }

        }

        private double WDReal_To_WDPrint(int mode, double WD_real)
        {
            double WD_print;
            if (mode == 0) //Resolution mode
                WD_print = WD_real - Settings1.Default.WD_offset_OBJ;
            else //Field, Wide-Field and Rokveld modes
                WD_print = WD_real - Settings1.Default.WD_offset_IML;

            return WD_print;
        }

        private double WDPrint_To_WDReal(int mode, double WD_print)
        {
            double WD_real;
            if (mode == 0) //Resolution mode
                WD_real = WD_print + Settings1.Default.WD_offset_OBJ;
            else //Field, Wide-Field and Rokveld modes
                WD_real = WD_print + Settings1.Default.WD_offset_IML;

            return WD_real;
        }

        private double Calculate_Vf(int mode)
        {
            int HVindex = HVProfile.SelectedIndex;
            double vf;
            //double vf= Settings1.Default.vf_max /Math.Pow(10, zoom);//vf_max in zoom_min
            //double I= Settings1.Default.I_max *vf/ Settings1.Default.vf_max;
            if (mode == 0) //Resolution mode
            {
                double I_log = Ctrl1D_Zoom.Value * (-Settings1.Default.LogI_Max_Resolution + Settings1.Default.LogI_Min_Resolution) / Ctrl1D_Zoom.Maximum + Settings1.Default.LogI_Max_Resolution;
                scanner_current_log(I_log);
                vf = AllUserSettings[HVindex].WD_real / Settings1.Default.scan_d * Settings1.Default.vf_max * Math.Sqrt(Settings1.Default.kV_max) / Math.Pow(10, Settings1.Default.LogI_Max_Resolution) * Math.Pow(10, I_log) / Math.Sqrt(AllUserSettings[HVindex].HV);
                if (vf < Settings1.Default.vf_min) vf = Settings1.Default.vf_min;
            }
            else if (mode == 1) //Wide-Field mode
            {
                double I_log = Ctrl1D_Zoom.Value * (-Settings1.Default.LogI_Max_WideField + Settings1.Default.LogI_Min_WideField) / Ctrl1D_Zoom.Maximum + Settings1.Default.LogI_Max_WideField;
                scanner_current_log(I_log);
                vf = AllUserSettings[HVindex].WD_real / Settings1.Default.scan_d * Settings1.Default.vf_max * Math.Sqrt(Settings1.Default.kV_max) / Math.Pow(10, Settings1.Default.LogI_Max_WideField) * Math.Pow(10, I_log) / Math.Sqrt(AllUserSettings[HVindex].HV);
                if (vf < Settings1.Default.vf_min) vf = Settings1.Default.vf_min;
            }
            else if (mode == 2) //Field mode
            {
                double I_log = Ctrl1D_Zoom.Value * (-Settings1.Default.LogI_Max_Field + Settings1.Default.LogI_Min_Field) / Ctrl1D_Zoom.Maximum + Settings1.Default.LogI_Max_Field;
                scanner_current_log(I_log);
                vf = AllUserSettings[HVindex].WD_real / Settings1.Default.scan_d * Settings1.Default.vf_max * Math.Sqrt(Settings1.Default.kV_max) / Math.Pow(10, Settings1.Default.LogI_Max_Field) * Math.Pow(10, I_log) / Math.Sqrt(AllUserSettings[HVindex].HV);
                if (vf < Settings1.Default.vf_min) vf = Settings1.Default.vf_min;
            }
            else //Rokveld mode
            {
                double I_log = Ctrl1D_Zoom.Value * (-Settings1.Default.LogI_Max_Rokveld + Settings1.Default.LogI_Min_Rokveld) / Ctrl1D_Zoom.Maximum + Settings1.Default.LogI_Max_Rokveld;
                scanner_current_log(I_log);
                vf = AllUserSettings[HVindex].WD_real / Settings1.Default.scan_d * Settings1.Default.vf_max * Math.Sqrt(Settings1.Default.kV_max) / Math.Pow(10, Settings1.Default.LogI_Max_Rokveld) * Math.Pow(10, I_log) / Math.Sqrt(AllUserSettings[HVindex].HV);
                if (vf < Settings1.Default.vf_min) vf = Settings1.Default.vf_min;
            }
            return vf;
        }

        private void Ctrl1D_Focus_Scroll(object sender, EventArgs e)
        {
            int mode = MicroscopyMode.SelectedIndex;
            int HVindex = HVProfile.SelectedIndex;
            double WD_real = Focus_To_WDreal(mode);
            AllUserSettings[HVindex].WD_real = WD_real;
            numericFocus.ValueChanged -= numericFocus_ValueChanged;
            numericFocus.Value = (decimal)WDReal_To_WDPrint(mode, WD_real);
            numericFocus.ValueChanged += numericFocus_ValueChanged;
            if (mode == 0)
                UD_Lens_OBJ.Value = Ctrl1D_Focus.Value;
            else
                UD_Lens_IML.Value = Ctrl1D_Focus.Value;
            UpdateZoom();
        }

        private void group1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void buttonHV_Click(object sender, EventArgs e)
        {
            UD_HV_HV.Value = 0; counter = 0;
            string CompleteOrder = CreateChildCommand("hv", "hv 0" + "\r");
            if (SendAndReceiveOK(CompleteOrder) == false) return;
            if (buttonHV.Text == "HV")
            {
                CompleteOrder = CreateChildCommand("hv", "kv 1" + "\r");
                if (SendAndReceiveOK(CompleteOrder) == true)
                {
                    buttonHV.Text = "HV is ON";
                    buttonHV.BackColor = Color.Coral;
                    //<<<<<<< master
                    //                    hv_init();
                    //                }
                    //            }
                    //            else
                    //                if (buttonHV.Text == "HV is ON")
                    //=======
                    buttonFB_Click(null, null);
                    hv_init();
                }
            }
            else if (buttonHV.Text == "HV is ON")
            //>>>>>>> master
            {
                CompleteOrder = CreateChildCommand("hv", "kv 0" + "\r");
                if (SendAndReceiveOK(CompleteOrder) == true)
                {
                    buttonHV.Text = "HV";
                    //<<<<<<< master
                    //                    buttonHV.BackColor = SystemColors.Menu;
                    //=======
                    buttonHV.BackColor = Color.Transparent;
                    progressBar_HV.Value = 0;
                    //>>>>>>> master
                }
            }
        }

        private void hv_init()
        {
            button1_Click(null, null);
            UD_SE_Faraday.Value = 1000;
            UD_SE_PMT.Value = 2500;
            counter_hv = 0;
            //<<<<<<< master
            //            buttonFB_Click(null, null);
            //=======
            //>>>>>>> master
            timer_hv.Start();
        }

        private void buttonFB_Click(object sender, EventArgs e)
        {
            //  UD_HV_Filament.Value = 0;
            string CompleteOrder;// = CreateChildCommand("hv", " filament 255" + "\r");
                                 // SendAndReceiveOK(CompleteOrder);
            if (buttonFB.Text == "HEAT")
            {
                CompleteOrder = CreateChildCommand("hv", "fb 1" + "\r");
                if (SendAndReceiveOK(CompleteOrder) == true)
                {
                    buttonFB.Text = "HEAT is ON";
                    buttonFB.BackColor = Color.Coral;
                    //<<<<<<< master
                    //                }
                    //            }
                    //            else
                    //                if (buttonFB.Text == "HEAT is ON")
                    //            {
                    //=======
                    hv_init();
                }
            }
            else if (buttonFB.Text == "HEAT is ON")
            {
                if (buttonHV.Text == "HV is ON") return; //Do not allow to turn of Heat when HV is ON ...

                //>>>>>>> master
                CompleteOrder = CreateChildCommand("hv", "fb 0" + "\r");
                if (SendAndReceiveOK(CompleteOrder) == true)
                {
                    buttonFB.Text = "HEAT";
                    //<<<<<<< master
                    //                    buttonFB.BackColor = SystemColors.Menu;
                    //                }
                    //            }
                    //            UD_HV_Filament.Value = 0;
                    //           // button5_Click(sender,e);
                    //=======
                    buttonFB.BackColor = Color.Transparent;
                    progressBar_FB.Value = 0;
                }
            }
            UD_HV_Filament.Value = 0;
            // button5_Click(sender,e);
            //>>>>>>> master
        }

        private void buttompanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void numericUpDown_imagepercent_ValueChanged(object sender, EventArgs e)
        {
            Image_multiply = (double)numericUpDown_imagepercent.Value;
            panel1.Size = new Size((int)((Image_multiply >= 1) ? 512 * Image_multiply + 1 : 512 + 1), (int)((Image_multiply >= 1) ? (512 * Image_multiply + 35) : (512 + 35)));
            ViewPort.Size = new Size((int)((Image_multiply >= 1) ? 512 * Image_multiply : 512), (int)((Image_multiply >= 1) ? (512 * Image_multiply) : (512)));
            ViewPort.SetZoomScale(Image_multiply, Point.Empty);
        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button_analyse_Click(object sender, EventArgs e)
        {
            Analyse f1 = new Analyse();
            f1.Show();
            f1.receivedData = receivedData;
            f1.window_row = window_row;
            f1.nX = nX;
            f1.nY = nY;

            //f1.chart1.Series[0].Points.AddXY
            // Thread.Sleep(6000);
            // f1.Close();
        }

        private void numericUpDown_objc_ValueChanged(object sender, EventArgs e)
        {
            if (lens_objc(numericUpDown_objc.Value))
            {

            }
            else
            {
                this.numericUpDown_objc.ValueChanged -= new System.EventHandler(this.numericUpDown_objc_ValueChanged);
                numericUpDown_objc.Value = Decimal.Parse(numericUpDown_objc.Tag.ToString());
                this.numericUpDown_objc.ValueChanged += new System.EventHandler(this.numericUpDown_objc_ValueChanged);
            }
        }

        private void button_stage_Click(object sender, EventArgs e)
        {
            //<<<<<<< master
            //button2_Click(this, null);
            //stageform.Show();
            if (!stageform.Visible)
            {
                stageform.Show(this);
                stageform.mainform = this;
            }
            else
            {
                stageform.Hide();
                //=======
                //            button2_Click(this, null);
                //            if (!sf.Visible)
                //            {
                //                sf.Show();
                //            }
                //            else
                //            {
                //                sf.Hide();
                //>>>>>>> master
            }

        }

        private void numericUpDown13_ValueChanged(object sender, EventArgs e)
        {
            //<<<<<<< master
            //
            //=======
            if (buttonFB.Text == "HEAT is ON") TimerFBUpdater.Start();
            //>>>>>>> master
        }

        private void backgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {/*
            buttonFB_Click(null, null);
            for (int i = 1; i < 10; i++)
            {
                numericFilament.Value = (decimal)((Settings1.Default.filament * i) / 10);
                Thread.Sleep(250); 
            }
            for (int i = 1; i < 10; i++)
            {
                numericHV.Value = (decimal)((Settings1.Default.kV* i) / 10);
                Thread.Sleep(250);
            }
            */
        }

        private void button_VAC_Click(object sender, EventArgs e)
        {
            if (button_VAC.Text == "START")
            {
                button_VAC.Text = "Pumping ...";
                button_VAC.Refresh();
                Thread.Sleep(5000);
                button_VAC.Text = "Ready";
                buttonHV.Enabled = true;
                buttonFB.Enabled = true;
            }
            else
            {
                button_VAC.Text = "START";
                buttonHV.Enabled = false;
                buttonFB.Enabled = false;
            }

        }

        private void timer_hv_Tick(object sender, EventArgs e)
        {
            int hv = 0, fb = 0;
            try
            {

                counter_hv++;

                if ((counter_hv <= 20) && (counter_hv > 10))
                {
                    //<<<<<<< master
                    //                    fb = (int)((numericFilament.Value * (counter_hv - 10)) / 10);
                    //                    progressBar_FB.Value = fb;
                    //                    if (!(hv_filament(fb * 256 / 100))) throw new Exception("filament");
                    //                }
                    //                else
                    //                if ((counter_hv <= 30) && (counter_hv > 20))
                    //                { 
                    //                hv = (int)((numericHV.Value * (counter_hv - 20)) / 10);
                    //                progressBar_HV.Value = hv;
                    //                if (hv > 20) hv = 20;
                    //                if (!(hv_hv(hv * (Settings1.Default.hv_raw_max) / 30))) throw new Exception("hv");
                    //                }
                    //                else
                    //                if (counter_hv > 30) timer_hv.Stop();
                    //            }
                    //            catch { timer_hv.Stop();MessageBox.Show("?"); }
                    //             
                    //=======
                    //fb = (int)((numericFilament.Value * (counter_hv - 10)) / 10);
                    //progressBar_FB.Value = fb;
                    //if (!(hv_filament(fb * 256 / 100))) throw new Exception("filament");
                    if (buttonFB.Text != "HEAT is ON") return;

                    fb = (int)((double)numericFilament.Value * (double)(counter_hv - 10) / 10.0);
                    int val = (int)((double)fb * (double)Settings1.Default.fb_raw_max / (double)numericFilament.Maximum);
                    if (val > Settings1.Default.fb_raw_max) val = Settings1.Default.fb_raw_max;
                    if (val < 0) val = 0;
                    UD_HV_Filament.Value = val;
                    progressBar_FB.Value = fb;
                    progressBar_FB.Refresh();
                }
                else if ((counter_hv <= 30) && (counter_hv > 20))
                {
                    //hv = (int)((numericHV.Value * (counter_hv - 20)) / 10);
                    //progressBar_HV.Value = hv;
                    //if (hv > 20) hv = 20;
                    //if (!(hv_hv(hv * (Settings1.Default.hv_raw_max) / 30))) throw new Exception("hv");

                    if (buttonHV.Text != "HV is ON") return;

                    hv = (int)((double)numericHV.Value * (double)(counter_hv - 20) / 10.0);
                    int val = (int)Math.Round((double)hv * (double)Settings1.Default.hv_raw_max / (double)numericHV.Maximum);
                    if (val > Settings1.Default.hv_raw_max) val = (int)Settings1.Default.hv_raw_max;
                    if (val < 0) val = 0;
                    UD_HV_HV.Value = val;
                    progressBar_HV.Value = hv;
                    progressBar_HV.Refresh();
                }
                else if (counter_hv > 30) timer_hv.Stop();
            }
            catch { timer_hv.Stop(); MessageBox.Show("Something went wrong. Try again!"); }

            //>>>>>>> master
        }

        private void trackBar_focus_course_Scroll(object sender, EventArgs e)
        {
            int mode = MicroscopyMode.SelectedIndex;
            //<<<<<<< master
            //            int HVindex = HVProfile.SelectedIndex;
            //            double WD_real = Focus_To_WDreal(mode);
            //            AllUserSettings[HVindex].WD_real = WD_real;
            //            numericFocus.ValueChanged -= numericFocus_ValueChanged;
            //            numericFocus.Value = (decimal)WDReal_To_WDPrint(mode, WD_real);
            //            numericFocus.ValueChanged += numericFocus_ValueChanged;
            //            if (mode == 0)
            //                numericUpDown_objc.Value = trackBar_focus_course.Value;
            //            else
            //                numericUpDown_objc.Value = trackBar_focus_course.Value;
            //            UpdateZoom();
            //=======
            if (mode == 0)
            {
                int HVindex = HVProfile.SelectedIndex;
                double WD_real = Focus_To_WDreal(mode);
                AllUserSettings[HVindex].WD_real = WD_real;
                numericFocus.ValueChanged -= numericFocus_ValueChanged;
                numericFocus.Value = (decimal)WDReal_To_WDPrint(mode, WD_real);
                numericFocus.ValueChanged += numericFocus_ValueChanged;
                numericUpDown_objc.Value = trackBar_focus_course.Value;
                UpdateZoom();
            }
            //>>>>>>> master
        }

        private void button_settings_Click(object sender, EventArgs e)
        {
            //<<<<<<< master
            //         Settings sf = new Settings();
            //=======
            Settings sf = new Settings(this);
            //>>>>>>> master
            sf.Show();
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

        private void trackBar_gamma_Scroll(object sender, EventArgs e)
        {
           // frame._GammaCorrect((trackBar_gamma.Value) / 100.0 * 1.5 + 0.5);
        }

        private void trackBar__Scroll(object sender, EventArgs e)
        {
            //frame._SmoothGaussian(trackBar_smooth.Value);
        }

        private void Ctrl2D_Stig_Load(object sender, EventArgs e)
        {

        }

        private void Ctrl2D_Stig_valueChanged(object sender, EventArgs e)
        {
            num_stig_x.ValueChanged -= num_stig_x_ValueChanged;
            num_stig_y.ValueChanged -= num_stig_y_ValueChanged;
            double coef = (double)(Settings1.Default.num_stig_max - Settings1.Default.num_stig_min) / 4095.0;
            num_stig_x.Value = (decimal)((double)Settings1.Default.num_stig_min + coef * ((double)Ctrl2D_Stig.Value.X + 2047.0));
            num_stig_y.Value = (decimal)((double)Settings1.Default.num_stig_max - coef * ((double)Ctrl2D_Stig.Value.Y + 2047.0));
            num_stig_x.ValueChanged += num_stig_x_ValueChanged;
            num_stig_y.ValueChanged += num_stig_y_ValueChanged;

            userControl12.Value = Ctrl2D_Stig.Value;
            // u2_valueChanged( sender,  e);  
            //  this.UD_Lens_stigx.Value = userControl12.X;
            // this.UD_Lens_stigy.Value = userControl12.Y;
        }

        private void userControl12_Load(object sender, EventArgs e)
        {

        }

        private void Ctrl2D_GunShift_Load(object sender, EventArgs e)
        {

        }

        private void Ctrl2D_GunShift_valueChanged(object sender, EventArgs e)
        {
            num_gunshift_x.ValueChanged -= num_gunshift_x_ValueChanged;
            num_gunshift_y.ValueChanged -= num_gunshift_y_ValueChanged;
            double coef = (double)(Settings1.Default.num_gunshift_max - Settings1.Default.num_gunshift_min) / 4095.0;
            num_gunshift_x.Value = (decimal)((double)Settings1.Default.num_gunshift_min + coef * ((double)Ctrl2D_GunShift.Value.X + 2047.0));
            num_gunshift_y.Value = (decimal)((double)Settings1.Default.num_gunshift_max - coef * ((double)Ctrl2D_GunShift.Value.Y + 2047.0));
            num_gunshift_x.ValueChanged += num_gunshift_x_ValueChanged;
            num_gunshift_y.ValueChanged += num_gunshift_y_ValueChanged;

            userControl14.Value = Ctrl2D_GunShift.Value;
        }

        private void Ctrl2D_GunTilt_valueChanged(object sender, EventArgs e)
        {
            num_guntilt_x.ValueChanged -= num_guntilt_x_ValueChanged;
            num_guntilt_y.ValueChanged -= num_guntilt_y_ValueChanged;
            double coef = (double)(Settings1.Default.num_guntilt_max - Settings1.Default.num_guntilt_min) / 4095.0;
            num_guntilt_x.Value = (decimal)((double)Settings1.Default.num_guntilt_min + coef * ((double)Ctrl2D_GunTilt.Value.X + 2047.0));
            num_guntilt_y.Value = (decimal)((double)Settings1.Default.num_guntilt_max - coef * ((double)Ctrl2D_GunTilt.Value.Y + 2047.0));
            num_guntilt_x.ValueChanged += num_guntilt_x_ValueChanged;
            num_guntilt_y.ValueChanged += num_guntilt_y_ValueChanged;

            userControl15.Value = Ctrl2D_GunTilt.Value;
        }

        private void Ctrl2D_IMLCentering_valueChanged(object sender, EventArgs e)
        {
            num_iml_x.ValueChanged -= num_iml_x_ValueChanged;
            num_iml_y.ValueChanged -= num_iml_y_ValueChanged;
            double coef = (double)(Settings1.Default.num_iml_max - Settings1.Default.num_iml_min) / 4095.0;
            num_iml_x.Value = (decimal)((double)Settings1.Default.num_iml_min + coef * ((double)Ctrl2D_IMLCentering.Value.X + 2047.0));
            num_iml_y.Value = (decimal)((double)Settings1.Default.num_iml_max - coef * ((double)Ctrl2D_IMLCentering.Value.Y + 2047.0));
            num_iml_x.ValueChanged += num_iml_x_ValueChanged;
            num_iml_y.ValueChanged += num_iml_y_ValueChanged;

            userControl11.Value = Ctrl2D_IMLCentering.Value;
        }

        private void Ctrl2D_ObjectCentering_Load(object sender, EventArgs e)
        {

        }

        private void Ctrl2D_ObjectCentering_valueChanged(object sender, EventArgs e)
        {
            num_obj_x.ValueChanged -= num_obj_x_ValueChanged;
            num_obj_y.ValueChanged -= num_obj_y_ValueChanged;
            double coef = (double)(Settings1.Default.num_obj_max - Settings1.Default.num_obj_min) / 4095.0;
            num_obj_x.Value = (decimal)((double)Settings1.Default.num_obj_min + coef * ((double)Ctrl2D_ObjectCentering.Value.X + 2047.0));
            num_obj_y.Value = (decimal)((double)Settings1.Default.num_obj_max - coef * ((double)Ctrl2D_ObjectCentering.Value.Y + 2047.0));
            num_obj_x.ValueChanged += num_obj_x_ValueChanged;
            num_obj_y.ValueChanged += num_obj_y_ValueChanged;

            userControl13.Value = Ctrl2D_ObjectCentering.Value;
        }

        private void trackBar_wob_Scroll(object sender, EventArgs e)
        {
            TB_Wobbler.Value = trackBar_wob.Value;
        }

        private void FormMain_Activated(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                //this.ControlBox = true;
                //this.WindowState = FormWindowState.Normal;
                this.WindowState = FormWindowState.Maximized;
                this.ControlBox = false;
            }
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            this.Text = String.Empty;
            EnterFullScreenMode();
            //<<<<<<< master
            //           // button20_Click(null, null);
            //=======

            track_ball_enumeration();

            button20_Click(null, null);
            //>>>>>>> master
        }

        private void button20_Click(object sender, EventArgs e)
        {
            Form4 UserAccountForm = new Form4(false, this);
            UserAccountForm.StartPosition = FormStartPosition.CenterParent;
            UserAccountForm.ShowDialog();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            Form4 createaccountform = new Form4(true, this);
            createaccountform.StartPosition = FormStartPosition.CenterParent;
            createaccountform.ShowDialog();
        }

        public void CreateAccount(string username, string hash)
        {
            for (int i = 0; i < AllUserSettings.Count; i++)
            {
                AllUserSettings[i].UserName = username;
                AllUserSettings[i].PassHashCode = hash;
            }

            System.IO.Directory.CreateDirectory(".\\Accounts");
            string filename = ".\\Accounts\\" + username + ".xml";
            XmlSerializer mySerializer = new XmlSerializer(typeof(List<UserSettings>));
            //To write to a file, create a StreamWriter object.
            StreamWriter xmlWriter = new StreamWriter(filename);
            mySerializer.Serialize(xmlWriter, AllUserSettings);
            xmlWriter.Close();
        }

        public void Fill_AllUserSettings()
        {
            int HVindex = HVProfileLastIndex;
            int microscopy_mode = MicroscopyModeLastIndex;

            for (int i = 0; i < AllUserSettings.Count; i++)
            {
                AllUserSettings[i].HVProfile = HVProfile.SelectedIndex;
            }

            AllUserSettings[HVindex].MicroscopyMode = MicroscopyMode.SelectedIndex;

            HVProfileLastIndex = HVProfile.SelectedIndex;
            MicroscopyModeLastIndex = AllUserSettings[HVindex].MicroscopyMode;

            AllUserSettings[HVindex].Focus_course = trackBar_focus_course.Value;
            if (microscopy_mode == 0)
                AllUserSettings[HVindex].FocusFine_Resolution = Ctrl1D_Focus.Value;
            else if (microscopy_mode == 1)
                AllUserSettings[HVindex].FocusFine_WideField = Ctrl1D_Focus.Value;
            else if (microscopy_mode == 2)
                AllUserSettings[HVindex].FocusFine_Field = Ctrl1D_Focus.Value;
            else if (microscopy_mode == 3)
                AllUserSettings[HVindex].FocusFine_Rokveld = Ctrl1D_Focus.Value;
            AllUserSettings[HVindex].DFocus = dCtrl1D_Focus.Value;

            if (microscopy_mode == 0)
                AllUserSettings[HVindex].Vf_Resolution = Ctrl1D_Zoom.Value;
            else if (microscopy_mode == 1)
                AllUserSettings[HVindex].Vf_WideField = Ctrl1D_Zoom.Value;
            else if (microscopy_mode == 2)
                AllUserSettings[HVindex].Vf_Field = Ctrl1D_Zoom.Value;
            else if (microscopy_mode == 3)
                AllUserSettings[HVindex].Vf_Rokveld = Ctrl1D_Zoom.Value;

            AllUserSettings[HVindex].DZoom = dCtrl1D_Zoom.Value;

            AllUserSettings[HVindex].Stig_x = Ctrl2D_Stig.X;
            AllUserSettings[HVindex].Stig_y = Ctrl2D_Stig.Y;
            AllUserSettings[HVindex].DStig = dCtrl2D_Stig.Value;

            AllUserSettings[HVindex].Gain_x = Ctrl2D_Gain.X;
            AllUserSettings[HVindex].Gain_y = Ctrl2D_Gain.Y;
            AllUserSettings[HVindex].DGain = dCtrl2D_Gain.Value;

            AllUserSettings[HVindex].ObjectCentering_x = Ctrl2D_ObjectCentering.X;
            AllUserSettings[HVindex].ObjectCentering_y = Ctrl2D_ObjectCentering.Y;
            AllUserSettings[HVindex].DObjectCentering = dCtrl2D_ObjectCentering.Value;

            AllUserSettings[HVindex].IMLCentering_x = Ctrl2D_IMLCentering.X;
            AllUserSettings[HVindex].IMLCentering_y = Ctrl2D_IMLCentering.Y;
            AllUserSettings[HVindex].DIMLCentering = dCtrl2D_IMLCentering.Value;

            AllUserSettings[HVindex].GunShift_x = Ctrl2D_GunShift.X;
            AllUserSettings[HVindex].GunShift_y = Ctrl2D_GunShift.Y;
            AllUserSettings[HVindex].DGunShift = dCtrl2D_GunShift.Value;

            AllUserSettings[HVindex].GunTilt_x = Ctrl2D_GunTilt.X;
            AllUserSettings[HVindex].GunTilt_y = Ctrl2D_GunTilt.Y;
            AllUserSettings[HVindex].DGunTilt = dCtrl2D_GunTilt.Value;

            AllUserSettings[HVindex].Heat = (int)numericFilament.Value;
            AllUserSettings[HVindex].HV = (int)numericHV.Value;

            //<<<<<<< master
            //            AllUserSettings[HVindex].PC = (int)numeric_PCF.Value;
            //=======
            AllUserSettings[HVindex].PC1 = (int)numeric_PCF.Value;
            AllUserSettings[HVindex].PC2 = (int)numericUpDown12.Value;
            AllUserSettings[HVindex].PC1Coef = (double)numericUpDown14.Value;
            AllUserSettings[HVindex].PC2Coef = (int)numericUpDown13.Value;
            //>>>>>>> master
            AllUserSettings[HVindex].Speed = (int)numeric_Speed.Value;
        }

        public void TryToConnect()
        {
            try
            {
                Btn_TCPDisconnect_Click(null, null);
            }
            catch
            {

            }

            try
            {
                //<<<<<<< master
                //                Btn_TCPConnect_Click(null, null);
                //=======
                Form5 cf = new Form5(this);
                cf.StartPosition = FormStartPosition.CenterParent;
                cf.ShowDialog();
                //>>>>>>> master
            }
            catch
            {

            }

        }

        //<<<<<<< master
        //=======
        public bool Connect_TCP()
        {
            Btn_TCPConnect_Click(null, null);
            return tcp.Connected;
        }

        public bool Connect_UDP()
        {
            BtnStart_Click(null, null);
            return isUDPConnected;
        }

        public bool DisConnect_UDP()
        {
            Btn_UDPDisconnect_Click(null, null);
            return isUDPConnected;
        }

        public bool Connect_Lens()
        {
            if (!tcp.Connected) return false;
            string CompleteOrder = CreateChildCommand("l", "you?\r");
            string response = SendAndReceiveResponse(CompleteOrder);
            if (response != "lens") return false;
            return true;
        }

        public bool Connect_HV()
        {
            if (!tcp.Connected) return false;
            string CompleteOrder = CreateChildCommand("hv", "you?\r");
            string response = SendAndReceiveResponse(CompleteOrder);
            if (response != "hv") return false;
            return true;
        }

        public bool Connect_FB()
        {
            if (!tcp.Connected) return false;
            string CompleteOrder = CreateChildCommand("hv", "fb 1" + "\r");
            SendAndReceiveOK(CompleteOrder);
            bool isOK = hv_filament(0);
            CompleteOrder = CreateChildCommand("hv", "fb 0" + "\r");
            SendAndReceiveOK(CompleteOrder);
            return isOK;
        }

        public bool Connect_Stage()
        {
            string CompleteOrder = CreateChildCommand("st", "you?\r");
            string response = SendAndReceiveResponse(CompleteOrder);
            if (!response.StartsWith("Spray")) return false;
            return true;
        }

        //>>>>>>> master
        public void UpdateUserName(bool isSwitchBetweenMode)
        {
            UserInfo = "  |  Logged in as: " + UserName;

            string filename = ".\\Accounts\\" + UserName + ".xml";
            XmlSerializer mySerializer = new XmlSerializer(typeof(List<UserSettings>));
            StreamReader xmlReader = new StreamReader(filename);
            AllUserSettings = (List<UserSettings>)mySerializer.Deserialize(xmlReader);
            xmlReader.Close();

            int HVindex = AllUserSettings[0].HVProfile;
            HVProfile.SelectedIndexChanged -= HVProfile_SelectedIndexChanged;
            HVProfile.SelectedIndex = HVindex;
            HVProfileLastIndex = HVProfile.SelectedIndex;
            HVProfile.SelectedIndexChanged += HVProfile_SelectedIndexChanged;

            if (!isSwitchBetweenMode)
            {
                MicroscopyMode.SelectedIndexChanged -= MicroscopyMode_SelectedIndexChanged;
                MicroscopyMode.SelectedIndex = AllUserSettings[HVindex].MicroscopyMode;
                MicroscopyModeLastIndex = MicroscopyMode.SelectedIndex;
                MicroscopyMode.SelectedIndexChanged += MicroscopyMode_SelectedIndexChanged;
            }

            int microscopy_mode = AllUserSettings[HVindex].MicroscopyMode;

            if (microscopy_mode == 0)
            {
                trackBar_focus_course.Enabled = true;
                numericFocus.Maximum = (int)Settings1.Default.q_max;
                //Ctrl1D_Focus.Minimum = Settings1.Default.Focus_Min_res_fine;
                //Ctrl1D_Focus.Maximum = Settings1.Default.Focus_Max_res_fine;
                //Ctrl1D_Zoom.Minimum = Settings1.Default.Zoom_Min_res;
                //Ctrl1D_Zoom.Maximum = Settings1.Default.Zoom_Max_res;
            }
            else
            {
                trackBar_focus_course.Enabled = false;
                numericFocus.Maximum = (int)(Settings1.Default.WD_real_IML - Settings1.Default.WD_offset_IML + 1);
                //Ctrl1D_Focus.Minimum = Settings1.Default.Focus_Min_IML_fine;
                //Ctrl1D_Focus.Maximum = Settings1.Default.Focus_Max_IML_fine;
                //Ctrl1D_Zoom.Minimum = Settings1.Default.Zoom_Min_IML;
                //Ctrl1D_Zoom.Maximum = Settings1.Default.Zoom_Max_IML;
            }

            trackBar_focus_course.Value = AllUserSettings[HVindex].Focus_course;
            if (microscopy_mode == 0)
                Ctrl1D_Focus.Value = AllUserSettings[HVindex].FocusFine_Resolution;
            else if (microscopy_mode == 1)
                Ctrl1D_Focus.Value = AllUserSettings[HVindex].FocusFine_WideField;
            else if (microscopy_mode == 2)
                Ctrl1D_Focus.Value = AllUserSettings[HVindex].FocusFine_Field;
            else if (microscopy_mode == 3)
                Ctrl1D_Focus.Value = AllUserSettings[HVindex].FocusFine_Rokveld;
            dCtrl1D_Focus.Value = AllUserSettings[HVindex].DFocus;

            if (microscopy_mode == 0)
                Ctrl1D_Zoom.Value = AllUserSettings[HVindex].Vf_Resolution;
            else if (microscopy_mode == 1)
                Ctrl1D_Zoom.Value = AllUserSettings[HVindex].Vf_WideField;
            else if (microscopy_mode == 2)
                Ctrl1D_Zoom.Value = AllUserSettings[HVindex].Vf_Field;
            else if (microscopy_mode == 3)
                Ctrl1D_Zoom.Value = AllUserSettings[HVindex].Vf_Rokveld;

            dCtrl1D_Zoom.Value = AllUserSettings[HVindex].DZoom;

            userControl12.X = AllUserSettings[HVindex].Stig_x;
            userControl12.Y = AllUserSettings[HVindex].Stig_y;
            Ctrl2D_Stig.Value = new Point(AllUserSettings[HVindex].Stig_x, AllUserSettings[HVindex].Stig_y);
            dCtrl2D_Stig.Value = AllUserSettings[HVindex].DStig;

            //Ctrl2D_Gain.X = AllUserSettings[HVindex].Gain_x;
            //Ctrl2D_Gain.Y = AllUserSettings[HVindex].Gain_y;
            Ctrl2D_Gain.Value = new Point(AllUserSettings[HVindex].Gain_x, AllUserSettings[HVindex].Gain_y);
            dCtrl2D_Gain.Value = AllUserSettings[HVindex].DGain;

            userControl13.X = AllUserSettings[HVindex].ObjectCentering_x;
            userControl13.Y = AllUserSettings[HVindex].ObjectCentering_y;
            Ctrl2D_ObjectCentering.Value = new Point(AllUserSettings[HVindex].ObjectCentering_x, AllUserSettings[HVindex].ObjectCentering_y);
            dCtrl2D_ObjectCentering.Value = AllUserSettings[HVindex].DObjectCentering;

            userControl11.X = AllUserSettings[HVindex].IMLCentering_x;
            userControl11.Y = AllUserSettings[HVindex].IMLCentering_y;
            Ctrl2D_IMLCentering.Value = new Point(AllUserSettings[HVindex].IMLCentering_x, AllUserSettings[HVindex].IMLCentering_y);
            dCtrl2D_IMLCentering.Value = AllUserSettings[HVindex].DIMLCentering;

            userControl14.X = AllUserSettings[HVindex].GunShift_x;
            userControl14.Y = AllUserSettings[HVindex].GunShift_y;
            Ctrl2D_GunShift.Value = new Point(AllUserSettings[HVindex].GunShift_x, AllUserSettings[HVindex].GunShift_y);
            dCtrl2D_GunShift.Value = AllUserSettings[HVindex].DGunShift;

            userControl15.X = AllUserSettings[HVindex].GunTilt_x;
            userControl15.Y = AllUserSettings[HVindex].GunTilt_y;
            Ctrl2D_GunTilt.Value = new Point(AllUserSettings[HVindex].GunTilt_x, AllUserSettings[HVindex].GunTilt_y);
            dCtrl2D_GunTilt.Value = AllUserSettings[HVindex].DGunTilt;

            numericFilament.Value = AllUserSettings[HVindex].Heat;
            numericHV.Value = AllUserSettings[HVindex].HV;

            //<<<<<<< master
            //            numeric_PCF.Value = AllUserSettings[HVindex].PC;
            //=======
            numeric_PCF.Value = AllUserSettings[HVindex].PC1;
            numericUpDown12.Value = AllUserSettings[HVindex].PC2;
            numericUpDown14.Value = (decimal)AllUserSettings[HVindex].PC1Coef;
            numericUpDown13.Value = AllUserSettings[HVindex].PC2Coef;

            //>>>>>>> master
            numeric_Speed.Value = AllUserSettings[HVindex].Speed;

            if (HVindex != -1)
            {
                if(AllUserSettings[HVindex].MicroscopyMode != -1)
                    leftpanel.Enabled = true;
                else
                    leftpanel.Enabled = false;
            }
            else
                leftpanel.Enabled = false;
        }

        public void Logout()
        {
            if (UserName != "") SaveCurrentAccountSettings();
            UserName = "";
            UserInfo = "";
        }

        public void SaveCurrentAccountSettings()
        {
            if (HVProfileLastIndex < 0) return;
            Fill_AllUserSettings();
            System.IO.Directory.CreateDirectory(".\\Accounts");
            string filename = ".\\Accounts\\" + UserName + ".xml";
            XmlSerializer mySerializer = new XmlSerializer(typeof(List<UserSettings>));
            StreamWriter xmlWriter = new StreamWriter(filename);
            mySerializer.Serialize(xmlWriter, AllUserSettings);
            xmlWriter.Close();
        }

        private void HVProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveCurrentAccountSettings();
            HVProfileLastIndex = HVProfile.SelectedIndex;
            UpdateUserName(false);
        }

        private void MicroscopyMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            SaveCurrentAccountSettings();
            MicroscopyModeLastIndex = MicroscopyMode.SelectedIndex;
            UpdateUserName(true);
        }

        private void dCtrl1D_Focus_Scroll(object sender, EventArgs e)
        {

        }

        private void dCtrl2D_Stig_Scroll(object sender, EventArgs e)
        {
            trackBar2.Value = dCtrl2D_Stig.Value;
        }

        private void dCtrl2D_IMLCentering_Scroll(object sender, EventArgs e)
        {
            trackBar1.Value = dCtrl2D_IMLCentering.Value;
        }

        private void dCtrl2D_GunShift_Scroll(object sender, EventArgs e)
        {
            trackBar3.Value = dCtrl2D_GunShift.Value;
        }

        private void dCtrl2D_GunTilt_Scroll(object sender, EventArgs e)
        {
            trackBar4.Value = dCtrl2D_GunTilt.Value;
        }

        private void dCtrl2D_ObjectCentering_Scroll(object sender, EventArgs e)
        {
            trackBar5.Value = dCtrl2D_ObjectCentering.Value;
        }

        private void userControl11_MouseHover(object sender, EventArgs e)
        {
            //<<<<<<< master
            //            label118.Text = userControl11.X.ToString();
            //            label119.Text = userControl11.Y.ToString();
            //=======
            //label118.Text = userControl11.X.ToString();
            //label119.Text = userControl11.Y.ToString();
            //>>>>>>> master
        }

        private void numericFocus_ValueChanged(object sender, EventArgs e)
        {
            int mode = MicroscopyMode.SelectedIndex;
            double WD_real = WDPrint_To_WDReal(mode, (double)numericFocus.Value);
            int FocusCourse = 0;
            int FocusFine = 0;
            WDreal_To_Focus(mode, WD_real, ref FocusCourse, ref FocusFine);
            trackBar_focus_course.Value = FocusCourse;
            Ctrl1D_Focus.Value = FocusFine;
        }

        private void num_stig_x_ValueChanged(object sender, EventArgs e)
        {
            double coef = (double)(Settings1.Default.num_stig_max - Settings1.Default.num_stig_min) / 4095.0;
            int x = (int)Math.Round(((double)num_stig_x.Value - (double)Settings1.Default.num_stig_min) / coef - 2047.0);
            //int y = (int)Math.Round(-((double)num_stig_y.Value - (double)Settings1.Default.num_stig_max) / coef - 2047.0);

            Ctrl2D_Stig.Value = new Point(x, Ctrl2D_Stig.Value.Y);
        }

        private void num_stig_y_ValueChanged(object sender, EventArgs e)
        {
            double coef = (double)(Settings1.Default.num_stig_max - Settings1.Default.num_stig_min) / 4095.0;
            //int x = (int)Math.Round(((double)num_stig_x.Value - (double)Settings1.Default.num_stig_min) / coef - 2047.0);
            int y = (int)Math.Round(-((double)num_stig_y.Value - (double)Settings1.Default.num_stig_max) / coef - 2047.0);

            Ctrl2D_Stig.Value = new Point(Ctrl2D_Stig.Value.X, y);
        }

        private void num_gain_x_ValueChanged(object sender, EventArgs e)
        {
            double coef = (double)(Settings1.Default.num_gain_max - Settings1.Default.num_gain_min) / 4095.0;
            int x = (int)Math.Round(((double)num_gain_x.Value - (double)Settings1.Default.num_gain_min) / coef - 2047.0);
            //int y = (int)Math.Round(-((double)num_gain_y.Value - (double)Settings1.Default.num_gain_max) / coef - 2047.0);

            Ctrl2D_Gain.Value = new Point(x, Ctrl2D_Gain.Value.Y);
        }

        private void num_gain_y_ValueChanged(object sender, EventArgs e)
        {
            double coef = (double)(Settings1.Default.num_gain_max - Settings1.Default.num_gain_min) / 4095.0;
            //int x = (int)Math.Round(((double)num_gain_x.Value - (double)Settings1.Default.num_gain_min) / coef - 2047.0);
            int y = (int)Math.Round(-((double)num_gain_y.Value - (double)Settings1.Default.num_gain_max) / coef - 2047.0);

            Ctrl2D_Gain.Value = new Point(Ctrl2D_Gain.Value.X, y);
        }

        private void num_obj_x_ValueChanged(object sender, EventArgs e)
        {
            double coef = (double)(Settings1.Default.num_obj_max - Settings1.Default.num_obj_min) / 4095.0;
            int x = (int)Math.Round(((double)num_obj_x.Value - (double)Settings1.Default.num_obj_min) / coef - 2047.0);
            //int y = (int)Math.Round(-((double)num_obj_y.Value - (double)Settings1.Default.num_obj_max) / coef - 2047.0);

            Ctrl2D_ObjectCentering.Value = new Point(x, Ctrl2D_ObjectCentering.Value.Y);
        }

        private void num_obj_y_ValueChanged(object sender, EventArgs e)
        {
            double coef = (double)(Settings1.Default.num_obj_max - Settings1.Default.num_obj_min) / 4095.0;
            //int x = (int)Math.Round(((double)num_obj_x.Value - (double)Settings1.Default.num_obj_min) / coef - 2047.0);
            int y = (int)Math.Round(-((double)num_obj_y.Value - (double)Settings1.Default.num_obj_max) / coef - 2047.0);

            Ctrl2D_ObjectCentering.Value = new Point(Ctrl2D_ObjectCentering.Value.X, y);
        }

        private void num_iml_x_ValueChanged(object sender, EventArgs e)
        {
            double coef = (double)(Settings1.Default.num_iml_max - Settings1.Default.num_iml_min) / 4095.0;
            int x = (int)Math.Round(((double)num_iml_x.Value - (double)Settings1.Default.num_iml_min) / coef - 2047.0);
            //int y = (int)Math.Round(-((double)num_iml_y.Value - (double)Settings1.Default.num_iml_max) / coef - 2047.0);

            Ctrl2D_IMLCentering.Value = new Point(x, Ctrl2D_IMLCentering.Value.Y);
        }

        private void num_iml_y_ValueChanged(object sender, EventArgs e)
        {
            double coef = (double)(Settings1.Default.num_iml_max - Settings1.Default.num_iml_min) / 4095.0;
            //int x = (int)Math.Round(((double)num_iml_x.Value - (double)Settings1.Default.num_iml_min) / coef - 2047.0);
            int y = (int)Math.Round(-((double)num_iml_y.Value - (double)Settings1.Default.num_iml_max) / coef - 2047.0);

            Ctrl2D_IMLCentering.Value = new Point(Ctrl2D_IMLCentering.Value.X, y);
        }

        private void num_gunshift_x_ValueChanged(object sender, EventArgs e)
        {
            double coef = (double)(Settings1.Default.num_gunshift_max - Settings1.Default.num_gunshift_min) / 4095.0;
            int x = (int)Math.Round(((double)num_gunshift_x.Value - (double)Settings1.Default.num_gunshift_min) / coef - 2047.0);
            //int y = (int)Math.Round(-((double)num_gunshift_y.Value - (double)Settings1.Default.num_gunshift_max) / coef - 2047.0);

            Ctrl2D_GunShift.Value = new Point(x, Ctrl2D_GunShift.Value.Y);
        }

        private void num_gunshift_y_ValueChanged(object sender, EventArgs e)
        {
            double coef = (double)(Settings1.Default.num_gunshift_max - Settings1.Default.num_gunshift_min) / 4095.0;
            //int x = (int)Math.Round(((double)num_gunshift_x.Value - (double)Settings1.Default.num_gunshift_min) / coef - 2047.0);
            int y = (int)Math.Round(-((double)num_gunshift_y.Value - (double)Settings1.Default.num_gunshift_max) / coef - 2047.0);

            Ctrl2D_GunShift.Value = new Point(Ctrl2D_GunShift.Value.X, y);
        }

        private void num_guntilt_x_ValueChanged(object sender, EventArgs e)
        {
            double coef = (double)(Settings1.Default.num_guntilt_max - Settings1.Default.num_guntilt_min) / 4095.0;
            int x = (int)Math.Round(((double)num_guntilt_x.Value - (double)Settings1.Default.num_guntilt_min) / coef - 2047.0);
            //int y = (int)Math.Round(-((double)num_guntilt_y.Value - (double)Settings1.Default.num_guntilt_max) / coef - 2047.0);

            Ctrl2D_GunTilt.Value = new Point(x, Ctrl2D_GunTilt.Value.Y);
        }

        private void num_guntilt_y_ValueChanged(object sender, EventArgs e)
        {
            double coef = (double)(Settings1.Default.num_guntilt_max - Settings1.Default.num_guntilt_min) / 4095.0;
            //int x = (int)Math.Round(((double)num_guntilt_x.Value - (double)Settings1.Default.num_guntilt_min) / coef - 2047.0);
            int y = (int)Math.Round(-((double)num_guntilt_y.Value - (double)Settings1.Default.num_guntilt_max) / coef - 2047.0);

            Ctrl2D_GunTilt.Value = new Point(Ctrl2D_GunTilt.Value.X, y);
        }

        private void button23_Click(object sender, EventArgs e)
        {
            Analyse an = new Analyse();
            an.Show();
        }

        private void numeric_PCF_ValueChanged(object sender, EventArgs e)
        {
            UD_Lens_CON1.Value = numeric_PCF.Value;
        }

        private void numericUpDown14_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown7.Value = numericUpDown14.Value;
        }

        private void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            UD_Lens_CON2.Value = numericUpDown12.Value;
        }

        private void numericUpDown13_ValueChanged_1(object sender, EventArgs e)
        {
            numericUpDown11.Value = numericUpDown13.Value;
        }

        private void numeric_Speed_ValueChanged(object sender, EventArgs e)
        {
            speed_multiply.Value = numeric_Speed.Value;
        }

        //<<<<<<< master
        //        private void label44_Click(object sender, EventArgs e)
        //        {
        //
        //=======
        private bool GetDirection(int oldind, int newind)
        {
            int dp = newind - oldind;
            if (dp < 0) dp += 8;

            int dm = oldind - newind;
            if (dm < 0) dm += 8;

            if (dp < dm)
                return true;
            else
                return false;
        }

        private void button24_Click(object sender, EventArgs e)
        {
            isNextAnimation = GetDirection(SelectedLightControl, 0);
            SelectedLightControl = 0;
            AnimationTimer.Start();
        }

        private void button25_Click(object sender, EventArgs e)
        {
            isNextAnimation = GetDirection(SelectedLightControl, 1);
            SelectedLightControl = 1;
            AnimationTimer.Start();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            isNextAnimation = GetDirection(SelectedLightControl, 2);
            SelectedLightControl = 2;
            AnimationTimer.Start();
        }

        private void button29_Click(object sender, EventArgs e)
        {
            isNextAnimation = GetDirection(SelectedLightControl, 3);
            SelectedLightControl = 3;
            AnimationTimer.Start();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            isNextAnimation = GetDirection(SelectedLightControl, 4);
            SelectedLightControl = 4;
            AnimationTimer.Start();
        }

        private void button27_Click(object sender, EventArgs e)
        {
            isNextAnimation = GetDirection(SelectedLightControl, 5);
            SelectedLightControl = 5;
            AnimationTimer.Start();
        }

        private void button31_Click(object sender, EventArgs e)
        {
            isNextAnimation = GetDirection(SelectedLightControl, 6);
            SelectedLightControl = 6;
            AnimationTimer.Start();
        }

        private void button30_Click(object sender, EventArgs e)
        {
            isNextAnimation = GetDirection(SelectedLightControl, 7);
            SelectedLightControl = 7;
            AnimationTimer.Start();
        }

        private void TimerHVUpdater_Tick(object sender, EventArgs e)
        {
            if (numericHV.Value == progressBar_HV.Value)
                TimerHVUpdater.Stop();
            else if (buttonHV.Text != "HV is ON")
                TimerHVUpdater.Stop();
            else
            {
                int newHV = 0;
                if (progressBar_HV.Value < numericHV.Value)
                    newHV = (int)progressBar_HV.Value + 1;
                else
                    newHV = (int)progressBar_HV.Value - 1;

                int val = (int)Math.Round((double)newHV * (double)Settings1.Default.hv_raw_max / (double)numericHV.Maximum);
                if (val > Settings1.Default.hv_raw_max) val = (int)Settings1.Default.hv_raw_max;
                if (val < 0) val = 0;
                UD_HV_HV.Value = val;
                progressBar_HV.Value = newHV;
                progressBar_HV.Refresh();
            }
        }

        private void TimerFBUpdater_Tick(object sender, EventArgs e)
        {
            if (numericFilament.Value == progressBar_FB.Value)
                TimerFBUpdater.Stop();
            else if (buttonFB.Text != "HEAT is ON")
                TimerFBUpdater.Stop();
            else
            {
                int newFB = 0;
                if (progressBar_FB.Value < numericFilament.Value)
                    newFB = (int)progressBar_FB.Value + 3;
                else
                    newFB = (int)progressBar_FB.Value - 3;

                if (Math.Abs((int)newFB - (int)numericFilament.Value) < 4) newFB = (int)numericFilament.Value;

                int val = (int)Math.Round((double)newFB * (double)Settings1.Default.fb_raw_max / (double)numericFilament.Maximum);
                if (val > Settings1.Default.fb_raw_max) val = (int)Settings1.Default.fb_raw_max;
                if (val < 0) val = 0;

                UD_HV_Filament.Value = val;
                progressBar_FB.Value = newFB;
                progressBar_FB.Refresh();
            }
            //>>>>>>> master
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
            Filter_Median_Size = 2*trackBar11.Value - 1;
        }

        private void button_load_Click(object sender, EventArgs e)
        {
            openFileDialog_Images.ShowDialog();
        }

        private void openFileDialog_Images_FileOk(object sender, CancelEventArgs e)
        {
            Image<Gray, byte> image0 = new Image<Gray, byte>(openFileDialog_Images.FileName);
            frame = image0.GetSubRect(new Rectangle(0, 0, 512, 512));
            ViewPort.Image = SetFilter(frame);
        }

        private void labelscalekx_Click(object sender, EventArgs e)
        {

        }

        private void scalebar_Click(object sender, EventArgs e)
        {

        }

        private void numericHV_ValueChanged(object sender, EventArgs e)
        {
            //<<<<<<< master
            //            Settings1.Default.kV = (double) numericHV.Value; 
            //=======
            //Settings1.Default.kV = (double) numericHV.Value; 
            if (buttonHV.Text == "HV is ON") TimerHVUpdater.Start();
            //>>>>>>> master
        }

        private void Ctrl2D_Gain_valueChanged(object sender, EventArgs e)
        {
            num_gain_x.ValueChanged -= num_gain_x_ValueChanged;
            num_gain_y.ValueChanged -= num_gain_y_ValueChanged;
            double coef = (double)(Settings1.Default.num_stig_max - Settings1.Default.num_stig_min) / 4095.0;
            num_gain_x.Value = (decimal)((double)Settings1.Default.num_stig_min + coef * ((double)Ctrl2D_Gain.Value.X + 2047.0));
            num_gain_y.Value = (decimal)((double)Settings1.Default.num_stig_max - coef * ((double)Ctrl2D_Gain.Value.Y + 2047.0));
            num_gain_x.ValueChanged += num_gain_x_ValueChanged;
            num_gain_y.ValueChanged += num_gain_y_ValueChanged;

            UD_DetectorTrim_Coarse.Value = Ctrl2D_Gain.X + 2047;
            UD_SE_PMT.Value = Ctrl2D_Gain.Y + 2047;
        }

        private void spead_multiply_ValueChanged(object sender, EventArgs e)
        {
            if (dactimer(0))
            {
                Thread.Sleep(100);
                multiply = (UInt16)Math.Pow(2, (UInt16)speed_multiply.Value);
                multiply_count = 0;
                string CompleteOrder = "multiply " + multiply.ToString() + "\r";
                SendAndReceiveOK(CompleteOrder);
            }
            dactimer(1);

            numeric_Speed.ValueChanged -= numeric_Speed_ValueChanged;
            numeric_Speed.Value = speed_multiply.Value;
            numeric_Speed.ValueChanged += numeric_Speed_ValueChanged;
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // textBox3.Text += "{" + (byte)bt_buf[0] + "|" + (byte)bt_buf[1] + "|" + (sbyte)bt_buf[2] + "|" + (sbyte)bt_buf[3] + "}";
            textBox3.Text = "data recieved:{" + (bt_count++).ToString("00000000") + "}";
            switch (bt_buf[0])
            {
                case 1:
                    //<<<<<<< master
                    //userControl21.X -= (byte)bt_buf[1] * ((int)(sbyte)bt_buf[2])/10;
                    //userControl21.Y -= (byte)bt_buf[1] * ((int)(sbyte)bt_buf[3])/10;
                    //=======
                    //                    userControl21.X -= (byte)bt_buf[1] * ((int)(sbyte)bt_buf[2])/10;
                    //                    userControl21.Y -= (byte)bt_buf[1] * ((int)(sbyte)bt_buf[3])/10;
                    //>>>>>>> master
                    break;
                case 2:

                    break;
                default:
                    break;
            }

        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        /*
                private void component_DiscoverDevicesProgress(object sender, DiscoverDevicesEventArgs e)
                {
                    // log and save all found devices
                    for (int i = 0; i < e.Devices.Length; i++)
                    {
                        if (e.Devices[i].Remembered)
                        {
                            textBox3.Text += (e.Devices[i].DeviceName + " (" + e.Devices[i].DeviceAddress + "): Device is known");
                        }
                        else
                        {
                            textBox3.Text += (e.Devices[i].DeviceName + " (" + e.Devices[i].DeviceAddress + "): Device is unknown");
                        }
                       // this.deviceList.Add(e.Devices[i]);
                    }
                }

                private void component_DiscoverDevicesComplete(object sender, DiscoverDevicesEventArgs e)
                {
                    // log some stuff
                }*/

        /*  protected override void WndProc(ref Message m)
          {
              base.WndProc(ref m);
              //if (m.HWnd != this.Handle)
              {
             //ض‍      return;
              }
              switch (m.Msg)
              {

                  case 0x020E:
                     // FireMouseHWheel(m.WParam, m.LParam);
                      Int64 a = (Int64)((m.WParam.ToInt64())>>16);
                      this.Text = a.ToString() +"|" + m.LParam.ToString();
                      m.Result = (IntPtr)1;
                      break;
                  default:
                      break;

              }
          }*/
    }




    public static class SecurePasswordHasher
    {
        /// <summary>
        /// Size of salt.
        /// </summary>
        private const int SaltSize = 16;

        /// <summary>
        /// Size of hash.
        /// </summary>
        private const int HashSize = 20;

        /// <summary>
        /// Creates a hash from a password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="iterations">Number of iterations.</param>
        /// <returns>The hash.</returns>
        public static string Hash(string password, int iterations)
        {
            // Create salt
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

            // Create hash
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            var hash = pbkdf2.GetBytes(HashSize);

            // Combine salt and hash
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            // Convert to base64
            var base64Hash = Convert.ToBase64String(hashBytes);

            // Format hash with extra information
            return string.Format("$MYHASH$V1${0}${1}", iterations, base64Hash);
        }

        /// <summary>
        /// Creates a hash from a password with 10000 iterations
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns>The hash.</returns>
        public static string Hash(string password)
        {
            return Hash(password, 10000);
        }

        /// <summary>
        /// Checks if hash is supported.
        /// </summary>
        /// <param name="hashString">The hash.</param>
        /// <returns>Is supported?</returns>
        public static bool IsHashSupported(string hashString)
        {
            return hashString.Contains("$MYHASH$V1$");
        }

        /// <summary>
        /// Verifies a password against a hash.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="hashedPassword">The hash.</param>
        /// <returns>Could be verified?</returns>
        public static bool Verify(string password, string hashedPassword)
        {
            // Check hash
            if (!IsHashSupported(hashedPassword))
            {
                throw new NotSupportedException("The hashtype is not supported");
            }

            // Extract iteration and Base64 string
            var splittedHashString = hashedPassword.Replace("$MYHASH$V1$", "").Split('$');
            var iterations = int.Parse(splittedHashString[0]);
            var base64Hash = splittedHashString[1];

            // Get hash bytes
            var hashBytes = Convert.FromBase64String(base64Hash);

            // Get salt
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            // Create hash with given salt
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            byte[] hash = pbkdf2.GetBytes(HashSize);

            // Get result
            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
//First board
//
//scanner  board 74hc595 spi3 selection port f1 should be 0 (Scanner Current selector) Command:iscan Range:0,1,2
//scanner  board ad5328  spi3 selection port f2 should be 0 (Scanner Trimer selector) Command:(a:xpos b:xfin c:ypos d:yfin e:xref g:yref) Range:0,1,2,3,4,5,6,7
//scanner  board ad5449  spi3 selection port f0 should be 0 [dazy chain 3] (zoom and rotation) Command:(zr i,j,w) i=0:1(x,y) j=0:2 w=word
//detector board 74hc595 spi2 selection port g1 should be 0 (Detector mux selector) Command:mux Range:0,1,2,3,4,5,6,7
//detector board mcp6s28 spi2 selection port g0 should be 0 [dazy chain 2] (Detector gain) Command:(detgain i,j,w) i=0:1 j=0:3 w=word
//detector board ad5328  spi2 selection port g2 should be 0 (Detector Trimer selector) Command:(detoffset i,j,w) i=0:1 j=0:3 w=word
//
//And also spi4 , spi5 should be just configured to be used later
//
//detector board det0 spi4 selection port e2 should be 0
//detector board det1 spi5 selection port e3 should be 0
//detector board det2 spi4 selection port e4 should be 0
//detector board det3 spi5 selection port e5 should be 0
//
//Start UArt2=port1 and UArt6=port2
//
//Can1 and Can2
//
//vehlnet [uint12]
//filament [uint12]
//filamentboard [0:1]
//