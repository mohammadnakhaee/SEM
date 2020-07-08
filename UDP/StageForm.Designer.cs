namespace HelloWorld
{
    partial class StageForm
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
            this.components = new System.ComponentModel.Container();
            this.button_rot_cw = new System.Windows.Forms.Button();
            this.button_rot_ccw = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.numericUpDown_step = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_delay = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label_span = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDown_hys = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_rot = new System.Windows.Forms.NumericUpDown();
            this.stage1 = new HelloWorld.Stage();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_step)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_delay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_hys)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_rot)).BeginInit();
            this.SuspendLayout();
            // 
            // button_rot_cw
            // 
            this.button_rot_cw.BackgroundImage = global::HelloWorld.Properties.Resources.pad;
            this.button_rot_cw.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_rot_cw.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_rot_cw.Location = new System.Drawing.Point(310, 2);
            this.button_rot_cw.Name = "button_rot_cw";
            this.button_rot_cw.Size = new System.Drawing.Size(75, 75);
            this.button_rot_cw.TabIndex = 1;
            this.button_rot_cw.Text = "ROT_CW";
            this.button_rot_cw.UseVisualStyleBackColor = true;
            // 
            // button_rot_ccw
            // 
            this.button_rot_ccw.BackgroundImage = global::HelloWorld.Properties.Resources.pad;
            this.button_rot_ccw.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_rot_ccw.Location = new System.Drawing.Point(310, 228);
            this.button_rot_ccw.Name = "button_rot_ccw";
            this.button_rot_ccw.Size = new System.Drawing.Size(75, 75);
            this.button_rot_ccw.TabIndex = 2;
            this.button_rot_ccw.Text = "ROT_CCW";
            this.button_rot_ccw.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Honeydew;
            this.pictureBox1.Location = new System.Drawing.Point(277, 309);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(25, 5);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // numericUpDown_step
            // 
            this.numericUpDown_step.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDown_step.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown_step.Location = new System.Drawing.Point(312, 94);
            this.numericUpDown_step.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown_step.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown_step.Name = "numericUpDown_step";
            this.numericUpDown_step.Size = new System.Drawing.Size(65, 20);
            this.numericUpDown_step.TabIndex = 5;
            this.numericUpDown_step.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown_step.ValueChanged += new System.EventHandler(this.numericUpDown_step_ValueChanged);
            // 
            // numericUpDown_delay
            // 
            this.numericUpDown_delay.Location = new System.Drawing.Point(312, 133);
            this.numericUpDown_delay.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDown_delay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_delay.Name = "numericUpDown_delay";
            this.numericUpDown_delay.Size = new System.Drawing.Size(65, 20);
            this.numericUpDown_delay.TabIndex = 6;
            this.numericUpDown_delay.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown_delay.ValueChanged += new System.EventHandler(this.numericUpDown_delay_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(309, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "span(um):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(309, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "delay:";
            // 
            // label_span
            // 
            this.label_span.AutoSize = true;
            this.label_span.Location = new System.Drawing.Point(-1, 304);
            this.label_span.Name = "label_span";
            this.label_span.Size = new System.Drawing.Size(51, 13);
            this.label_span.TabIndex = 9;
            this.label_span.Text = "5um*5um";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(309, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "rotation:";
            // 
            // numericUpDown_hys
            // 
            this.numericUpDown_hys.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::HelloWorld.Properties.Settings.Default, "stage_hys", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown_hys.Location = new System.Drawing.Point(313, 202);
            this.numericUpDown_hys.Name = "numericUpDown_hys";
            this.numericUpDown_hys.Size = new System.Drawing.Size(63, 20);
            this.numericUpDown_hys.TabIndex = 12;
            this.numericUpDown_hys.Value = global::HelloWorld.Properties.Settings.Default.stage_hys;
            this.numericUpDown_hys.ValueChanged += new System.EventHandler(this.numericUpDown_hys_ValueChanged);
            // 
            // numericUpDown_rot
            // 
            this.numericUpDown_rot.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::HelloWorld.Properties.Settings.Default, "stage_rot", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown_rot.Location = new System.Drawing.Point(312, 172);
            this.numericUpDown_rot.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.numericUpDown_rot.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.numericUpDown_rot.Name = "numericUpDown_rot";
            this.numericUpDown_rot.Size = new System.Drawing.Size(64, 20);
            this.numericUpDown_rot.TabIndex = 10;
            this.numericUpDown_rot.Value = global::HelloWorld.Properties.Settings.Default.stage_rot;
            // 
            // stage1
            // 
            this.stage1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.stage1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.stage1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.stage1.Location = new System.Drawing.Point(2, 2);
            this.stage1.Margin = new System.Windows.Forms.Padding(4);
            this.stage1.Name = "stage1";
            this.stage1.Size = new System.Drawing.Size(301, 301);
            this.stage1.TabIndex = 0;
            this.stage1.Load += new System.EventHandler(this.stage1_Load);
            this.stage1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.stage1_MouseClick);
            // 
            // StageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 318);
            this.Controls.Add(this.numericUpDown_hys);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numericUpDown_rot);
            this.Controls.Add(this.label_span);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericUpDown_delay);
            this.Controls.Add(this.numericUpDown_step);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button_rot_ccw);
            this.Controls.Add(this.button_rot_cw);
            this.Controls.Add(this.stage1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "StageForm";
            this.Text = "1";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StageForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_step)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_delay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_hys)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_rot)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Stage stage1;
        private System.Windows.Forms.Button button_rot_cw;
        private System.Windows.Forms.Button button_rot_ccw;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.NumericUpDown numericUpDown_step;
        private System.Windows.Forms.NumericUpDown numericUpDown_delay;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label_span;
        private System.Windows.Forms.NumericUpDown numericUpDown_rot;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDown_hys;
    }
}