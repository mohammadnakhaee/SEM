namespace HelloWorld
{
    partial class ImageForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.group1 = new System.Windows.Forms.Panel();
            this.button_load = new System.Windows.Forms.Button();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.label136 = new System.Windows.Forms.Label();
            this.trackBar9 = new System.Windows.Forms.TrackBar();
            this.label137 = new System.Windows.Forms.Label();
            this.trackBar10 = new System.Windows.Forms.TrackBar();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.label135 = new System.Windows.Forms.Label();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.trackBar8 = new System.Windows.Forms.TrackBar();
            this.label134 = new System.Windows.Forms.Label();
            this.trackBar7 = new System.Windows.Forms.TrackBar();
            this.CheckBox_EqualizeHist = new System.Windows.Forms.CheckBox();
            this.label133 = new System.Windows.Forms.Label();
            this.CheckBox_FLIP_VERTICAL = new System.Windows.Forms.CheckBox();
            this.trackBar6 = new System.Windows.Forms.TrackBar();
            this.CheckBox_FLIP_HORIZONTAL = new System.Windows.Forms.CheckBox();
            this.label119 = new System.Windows.Forms.Label();
            this.Filter_Contrast = new System.Windows.Forms.TrackBar();
            this.label118 = new System.Windows.Forms.Label();
            this.Filter_Brightness = new System.Windows.Forms.TrackBar();
            this.button23 = new System.Windows.Forms.Button();
            this.label117 = new System.Windows.Forms.Label();
            this.label116 = new System.Windows.Forms.Label();
            this.button_save = new System.Windows.Forms.Button();
            this.numericUpDown_imagepercent = new System.Windows.Forms.NumericUpDown();
            this.ImageSize = new System.Windows.Forms.NumericUpDown();
            this.button3 = new System.Windows.Forms.Button();
            this.label52 = new System.Windows.Forms.Label();
            this.Btn_Acquire = new System.Windows.Forms.Button();
            this.UD_AcquireNumber = new System.Windows.Forms.NumericUpDown();
            this.trackBar11 = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.group1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar9)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Filter_Contrast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Filter_Brightness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_imagepercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UD_AcquireNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar11)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(579, 362);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            this.panel1.Resize += new System.EventHandler(this.panel1_Resize);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.group1);
            this.splitContainer1.Size = new System.Drawing.Size(579, 537);
            this.splitContainer1.SplitterDistance = 362;
            this.splitContainer1.TabIndex = 1;
            // 
            // group1
            // 
            this.group1.AutoScroll = true;
            this.group1.AutoSize = true;
            this.group1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(105)))), ((int)(((byte)(128)))));
            this.group1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.group1.Controls.Add(this.button_load);
            this.group1.Controls.Add(this.checkBox6);
            this.group1.Controls.Add(this.label136);
            this.group1.Controls.Add(this.trackBar9);
            this.group1.Controls.Add(this.label137);
            this.group1.Controls.Add(this.trackBar10);
            this.group1.Controls.Add(this.checkBox5);
            this.group1.Controls.Add(this.label135);
            this.group1.Controls.Add(this.checkBox4);
            this.group1.Controls.Add(this.trackBar8);
            this.group1.Controls.Add(this.label134);
            this.group1.Controls.Add(this.trackBar7);
            this.group1.Controls.Add(this.CheckBox_EqualizeHist);
            this.group1.Controls.Add(this.label133);
            this.group1.Controls.Add(this.CheckBox_FLIP_VERTICAL);
            this.group1.Controls.Add(this.trackBar6);
            this.group1.Controls.Add(this.CheckBox_FLIP_HORIZONTAL);
            this.group1.Controls.Add(this.label119);
            this.group1.Controls.Add(this.Filter_Contrast);
            this.group1.Controls.Add(this.label118);
            this.group1.Controls.Add(this.Filter_Brightness);
            this.group1.Controls.Add(this.button23);
            this.group1.Controls.Add(this.label117);
            this.group1.Controls.Add(this.label116);
            this.group1.Controls.Add(this.button_save);
            this.group1.Controls.Add(this.numericUpDown_imagepercent);
            this.group1.Controls.Add(this.ImageSize);
            this.group1.Controls.Add(this.button3);
            this.group1.Controls.Add(this.label52);
            this.group1.Controls.Add(this.Btn_Acquire);
            this.group1.Controls.Add(this.UD_AcquireNumber);
            this.group1.Controls.Add(this.trackBar11);
            this.group1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.group1.Location = new System.Drawing.Point(0, 0);
            this.group1.Margin = new System.Windows.Forms.Padding(0);
            this.group1.Name = "group1";
            this.group1.Size = new System.Drawing.Size(579, 171);
            this.group1.TabIndex = 1;
            // 
            // button_load
            // 
            this.button_load.BackColor = System.Drawing.Color.Transparent;
            this.button_load.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime;
            this.button_load.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.button_load.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_load.ForeColor = System.Drawing.Color.White;
            this.button_load.Location = new System.Drawing.Point(8, 30);
            this.button_load.Name = "button_load";
            this.button_load.Size = new System.Drawing.Size(88, 22);
            this.button_load.TabIndex = 87;
            this.button_load.Text = "Load";
            this.button_load.UseVisualStyleBackColor = false;
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.ForeColor = System.Drawing.Color.White;
            this.checkBox6.Location = new System.Drawing.Point(209, 89);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(100, 17);
            this.checkBox6.TabIndex = 84;
            this.checkBox6.Text = "Median Smooth";
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // label136
            // 
            this.label136.AutoSize = true;
            this.label136.ForeColor = System.Drawing.Color.White;
            this.label136.Location = new System.Drawing.Point(324, 137);
            this.label136.Name = "label136";
            this.label136.Size = new System.Drawing.Size(44, 13);
            this.label136.TabIndex = 83;
            this.label136.Text = "Linking:";
            // 
            // trackBar9
            // 
            this.trackBar9.Enabled = false;
            this.trackBar9.Location = new System.Drawing.Point(306, 156);
            this.trackBar9.Maximum = 100;
            this.trackBar9.Name = "trackBar9";
            this.trackBar9.Size = new System.Drawing.Size(97, 45);
            this.trackBar9.TabIndex = 82;
            this.trackBar9.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar9.Value = 60;
            // 
            // label137
            // 
            this.label137.AutoSize = true;
            this.label137.ForeColor = System.Drawing.Color.White;
            this.label137.Location = new System.Drawing.Point(261, 138);
            this.label137.Name = "label137";
            this.label137.Size = new System.Drawing.Size(57, 13);
            this.label137.TabIndex = 81;
            this.label137.Text = "Threshold:";
            // 
            // trackBar10
            // 
            this.trackBar10.Enabled = false;
            this.trackBar10.Location = new System.Drawing.Point(209, 156);
            this.trackBar10.Maximum = 200;
            this.trackBar10.Name = "trackBar10";
            this.trackBar10.Size = new System.Drawing.Size(100, 45);
            this.trackBar10.TabIndex = 80;
            this.trackBar10.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar10.Value = 100;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.ForeColor = System.Drawing.Color.White;
            this.checkBox5.Location = new System.Drawing.Point(209, 137);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(56, 17);
            this.checkBox5.TabIndex = 79;
            this.checkBox5.Text = "Edges";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // label135
            // 
            this.label135.AutoSize = true;
            this.label135.ForeColor = System.Drawing.Color.White;
            this.label135.Location = new System.Drawing.Point(-1, 158);
            this.label135.Name = "label135";
            this.label135.Size = new System.Drawing.Size(73, 13);
            this.label135.TabIndex = 77;
            this.label135.Text = "Space Sigma:";
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.ForeColor = System.Drawing.Color.White;
            this.checkBox4.Location = new System.Drawing.Point(8, 89);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(96, 17);
            this.checkBox4.TabIndex = 78;
            this.checkBox4.Text = "Bilatral Smooth";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // trackBar8
            // 
            this.trackBar8.Enabled = false;
            this.trackBar8.Location = new System.Drawing.Point(70, 156);
            this.trackBar8.Maximum = 100;
            this.trackBar8.Minimum = 10;
            this.trackBar8.Name = "trackBar8";
            this.trackBar8.Size = new System.Drawing.Size(136, 45);
            this.trackBar8.TabIndex = 76;
            this.trackBar8.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar8.Value = 50;
            // 
            // label134
            // 
            this.label134.AutoSize = true;
            this.label134.ForeColor = System.Drawing.Color.White;
            this.label134.Location = new System.Drawing.Point(6, 133);
            this.label134.Name = "label134";
            this.label134.Size = new System.Drawing.Size(66, 13);
            this.label134.TabIndex = 75;
            this.label134.Text = "Color Sigma:";
            // 
            // trackBar7
            // 
            this.trackBar7.Enabled = false;
            this.trackBar7.Location = new System.Drawing.Point(70, 131);
            this.trackBar7.Maximum = 100;
            this.trackBar7.Minimum = 10;
            this.trackBar7.Name = "trackBar7";
            this.trackBar7.Size = new System.Drawing.Size(136, 45);
            this.trackBar7.TabIndex = 74;
            this.trackBar7.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar7.Value = 50;
            // 
            // CheckBox_EqualizeHist
            // 
            this.CheckBox_EqualizeHist.AutoSize = true;
            this.CheckBox_EqualizeHist.ForeColor = System.Drawing.Color.White;
            this.CheckBox_EqualizeHist.Location = new System.Drawing.Point(310, 5);
            this.CheckBox_EqualizeHist.Name = "CheckBox_EqualizeHist";
            this.CheckBox_EqualizeHist.Size = new System.Drawing.Size(87, 17);
            this.CheckBox_EqualizeHist.TabIndex = 76;
            this.CheckBox_EqualizeHist.Text = "Equalize Hist";
            this.CheckBox_EqualizeHist.UseVisualStyleBackColor = true;
            // 
            // label133
            // 
            this.label133.AutoSize = true;
            this.label133.ForeColor = System.Drawing.Color.White;
            this.label133.Location = new System.Drawing.Point(7, 109);
            this.label133.Name = "label133";
            this.label133.Size = new System.Drawing.Size(63, 13);
            this.label133.TabIndex = 73;
            this.label133.Text = "Kernel Size:";
            // 
            // CheckBox_FLIP_VERTICAL
            // 
            this.CheckBox_FLIP_VERTICAL.AutoSize = true;
            this.CheckBox_FLIP_VERTICAL.ForeColor = System.Drawing.Color.White;
            this.CheckBox_FLIP_VERTICAL.Location = new System.Drawing.Point(222, 6);
            this.CheckBox_FLIP_VERTICAL.Name = "CheckBox_FLIP_VERTICAL";
            this.CheckBox_FLIP_VERTICAL.Size = new System.Drawing.Size(87, 17);
            this.CheckBox_FLIP_VERTICAL.TabIndex = 75;
            this.CheckBox_FLIP_VERTICAL.Text = "Flip Vertically";
            this.CheckBox_FLIP_VERTICAL.UseVisualStyleBackColor = true;
            // 
            // trackBar6
            // 
            this.trackBar6.Enabled = false;
            this.trackBar6.Location = new System.Drawing.Point(70, 107);
            this.trackBar6.Maximum = 7;
            this.trackBar6.Name = "trackBar6";
            this.trackBar6.Size = new System.Drawing.Size(136, 45);
            this.trackBar6.TabIndex = 72;
            this.trackBar6.Value = 5;
            // 
            // CheckBox_FLIP_HORIZONTAL
            // 
            this.CheckBox_FLIP_HORIZONTAL.AutoSize = true;
            this.CheckBox_FLIP_HORIZONTAL.ForeColor = System.Drawing.Color.White;
            this.CheckBox_FLIP_HORIZONTAL.Location = new System.Drawing.Point(125, 6);
            this.CheckBox_FLIP_HORIZONTAL.Name = "CheckBox_FLIP_HORIZONTAL";
            this.CheckBox_FLIP_HORIZONTAL.Size = new System.Drawing.Size(99, 17);
            this.CheckBox_FLIP_HORIZONTAL.TabIndex = 74;
            this.CheckBox_FLIP_HORIZONTAL.Text = "Flip Horizontally";
            this.CheckBox_FLIP_HORIZONTAL.UseVisualStyleBackColor = true;
            // 
            // label119
            // 
            this.label119.AutoSize = true;
            this.label119.ForeColor = System.Drawing.Color.White;
            this.label119.Location = new System.Drawing.Point(403, 6);
            this.label119.Name = "label119";
            this.label119.Size = new System.Drawing.Size(49, 13);
            this.label119.TabIndex = 73;
            this.label119.Text = "Contrast:";
            // 
            // Filter_Contrast
            // 
            this.Filter_Contrast.Location = new System.Drawing.Point(423, 20);
            this.Filter_Contrast.Maximum = 100;
            this.Filter_Contrast.Name = "Filter_Contrast";
            this.Filter_Contrast.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.Filter_Contrast.Size = new System.Drawing.Size(45, 179);
            this.Filter_Contrast.TabIndex = 72;
            this.Filter_Contrast.Value = 50;
            // 
            // label118
            // 
            this.label118.AutoSize = true;
            this.label118.ForeColor = System.Drawing.Color.White;
            this.label118.Location = new System.Drawing.Point(451, 6);
            this.label118.Name = "label118";
            this.label118.Size = new System.Drawing.Size(59, 13);
            this.label118.TabIndex = 71;
            this.label118.Text = "Brightness:";
            // 
            // Filter_Brightness
            // 
            this.Filter_Brightness.Location = new System.Drawing.Point(465, 20);
            this.Filter_Brightness.Maximum = 100;
            this.Filter_Brightness.Name = "Filter_Brightness";
            this.Filter_Brightness.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.Filter_Brightness.Size = new System.Drawing.Size(45, 179);
            this.Filter_Brightness.TabIndex = 70;
            this.Filter_Brightness.Value = 50;
            // 
            // button23
            // 
            this.button23.BackColor = System.Drawing.Color.Transparent;
            this.button23.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime;
            this.button23.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.button23.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button23.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.button23.ForeColor = System.Drawing.Color.White;
            this.button23.Location = new System.Drawing.Point(301, 55);
            this.button23.Name = "button23";
            this.button23.Size = new System.Drawing.Size(79, 25);
            this.button23.TabIndex = 66;
            this.button23.Text = "Analyze";
            this.button23.UseVisualStyleBackColor = false;
            // 
            // label117
            // 
            this.label117.AutoSize = true;
            this.label117.ForeColor = System.Drawing.Color.White;
            this.label117.Location = new System.Drawing.Point(203, 34);
            this.label117.Name = "label117";
            this.label117.Size = new System.Drawing.Size(49, 13);
            this.label117.TabIndex = 62;
            this.label117.Text = "Rescale:";
            // 
            // label116
            // 
            this.label116.AutoSize = true;
            this.label116.ForeColor = System.Drawing.Color.White;
            this.label116.Location = new System.Drawing.Point(307, 34);
            this.label116.Name = "label116";
            this.label116.Size = new System.Drawing.Size(30, 13);
            this.label116.TabIndex = 61;
            this.label116.Text = "Size:";
            // 
            // button_save
            // 
            this.button_save.BackColor = System.Drawing.Color.Transparent;
            this.button_save.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime;
            this.button_save.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.button_save.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button_save.ForeColor = System.Drawing.Color.White;
            this.button_save.Location = new System.Drawing.Point(98, 29);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(94, 23);
            this.button_save.TabIndex = 4;
            this.button_save.Text = "Save";
            this.button_save.UseVisualStyleBackColor = false;
            // 
            // numericUpDown_imagepercent
            // 
            this.numericUpDown_imagepercent.DecimalPlaces = 1;
            this.numericUpDown_imagepercent.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown_imagepercent.Location = new System.Drawing.Point(257, 32);
            this.numericUpDown_imagepercent.Margin = new System.Windows.Forms.Padding(2);
            this.numericUpDown_imagepercent.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            65536});
            this.numericUpDown_imagepercent.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericUpDown_imagepercent.Name = "numericUpDown_imagepercent";
            this.numericUpDown_imagepercent.Size = new System.Drawing.Size(42, 20);
            this.numericUpDown_imagepercent.TabIndex = 57;
            this.numericUpDown_imagepercent.Value = new decimal(new int[] {
            11,
            0,
            0,
            65536});
            // 
            // ImageSize
            // 
            this.ImageSize.Increment = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.ImageSize.Location = new System.Drawing.Point(343, 32);
            this.ImageSize.Maximum = new decimal(new int[] {
            2048,
            0,
            0,
            0});
            this.ImageSize.Minimum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.ImageSize.Name = "ImageSize";
            this.ImageSize.Size = new System.Drawing.Size(54, 20);
            this.ImageSize.TabIndex = 56;
            this.ImageSize.Tag = "512";
            this.ImageSize.Value = new decimal(new int[] {
            512,
            0,
            0,
            0});
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Transparent;
            this.button3.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime;
            this.button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.button3.ForeColor = System.Drawing.Color.White;
            this.button3.Location = new System.Drawing.Point(206, 56);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(86, 24);
            this.button3.TabIndex = 54;
            this.button3.Text = "Run time";
            this.button3.UseVisualStyleBackColor = false;
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.ForeColor = System.Drawing.Color.White;
            this.label52.Location = new System.Drawing.Point(10, 65);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(54, 13);
            this.label52.TabIndex = 53;
            this.label52.Text = "Itteration :";
            // 
            // Btn_Acquire
            // 
            this.Btn_Acquire.BackColor = System.Drawing.Color.Transparent;
            this.Btn_Acquire.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Lime;
            this.Btn_Acquire.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.Btn_Acquire.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.Btn_Acquire.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.Btn_Acquire.ForeColor = System.Drawing.Color.White;
            this.Btn_Acquire.Location = new System.Drawing.Point(124, 55);
            this.Btn_Acquire.Name = "Btn_Acquire";
            this.Btn_Acquire.Size = new System.Drawing.Size(79, 24);
            this.Btn_Acquire.TabIndex = 1;
            this.Btn_Acquire.Text = "Acquire";
            this.Btn_Acquire.UseVisualStyleBackColor = false;
            // 
            // UD_AcquireNumber
            // 
            this.UD_AcquireNumber.Location = new System.Drawing.Point(70, 58);
            this.UD_AcquireNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UD_AcquireNumber.Name = "UD_AcquireNumber";
            this.UD_AcquireNumber.Size = new System.Drawing.Size(42, 20);
            this.UD_AcquireNumber.TabIndex = 47;
            this.UD_AcquireNumber.Tag = "100";
            this.UD_AcquireNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // trackBar11
            // 
            this.trackBar11.Enabled = false;
            this.trackBar11.LargeChange = 1;
            this.trackBar11.Location = new System.Drawing.Point(209, 107);
            this.trackBar11.Maximum = 5;
            this.trackBar11.Minimum = 1;
            this.trackBar11.Name = "trackBar11";
            this.trackBar11.Size = new System.Drawing.Size(194, 45);
            this.trackBar11.TabIndex = 85;
            this.trackBar11.Value = 3;
            // 
            // ImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(579, 537);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ImageForm";
            this.Text = "ImageForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ImageForm_FormClosed);
            this.Load += new System.EventHandler(this.ImageForm_Load);
            this.ResizeBegin += new System.EventHandler(this.ImageForm_ResizeBegin);
            this.ResizeEnd += new System.EventHandler(this.ImageForm_ResizeEnd);
            this.SizeChanged += new System.EventHandler(this.ImageForm_SizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ImageForm_Paint);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.group1.ResumeLayout(false);
            this.group1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar9)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Filter_Contrast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Filter_Brightness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_imagepercent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UD_AcquireNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar11)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel group1;
        private System.Windows.Forms.Button button_load;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.Label label136;
        private System.Windows.Forms.TrackBar trackBar9;
        private System.Windows.Forms.Label label137;
        private System.Windows.Forms.TrackBar trackBar10;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.Label label135;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.TrackBar trackBar8;
        private System.Windows.Forms.Label label134;
        private System.Windows.Forms.TrackBar trackBar7;
        private System.Windows.Forms.CheckBox CheckBox_EqualizeHist;
        private System.Windows.Forms.Label label133;
        private System.Windows.Forms.CheckBox CheckBox_FLIP_VERTICAL;
        private System.Windows.Forms.TrackBar trackBar6;
        private System.Windows.Forms.CheckBox CheckBox_FLIP_HORIZONTAL;
        private System.Windows.Forms.Label label119;
        private System.Windows.Forms.TrackBar Filter_Contrast;
        private System.Windows.Forms.Label label118;
        private System.Windows.Forms.TrackBar Filter_Brightness;
        private System.Windows.Forms.Button button23;
        private System.Windows.Forms.Label label117;
        private System.Windows.Forms.Label label116;
        private System.Windows.Forms.Button button_save;
        private System.Windows.Forms.NumericUpDown numericUpDown_imagepercent;
        private System.Windows.Forms.NumericUpDown ImageSize;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.Button Btn_Acquire;
        private System.Windows.Forms.NumericUpDown UD_AcquireNumber;
        private System.Windows.Forms.TrackBar trackBar11;
    }
}