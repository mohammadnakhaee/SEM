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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StageForm));
            this.button_rot_cw = new System.Windows.Forms.Button();
            this.button_rot_ccw = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.stage1 = new HelloWorld.Stage();
            this.numericUpDown_step = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_delay = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_step)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_delay)).BeginInit();
            this.SuspendLayout();
            // 
            // button_rot_cw
            // 
            this.button_rot_cw.BackgroundImage = global::HelloWorld.Properties.Resources.pad;
            this.button_rot_cw.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_rot_cw.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.button_rot_cw.Location = new System.Drawing.Point(408, 15);
            this.button_rot_cw.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_rot_cw.Name = "button_rot_cw";
            this.button_rot_cw.Size = new System.Drawing.Size(100, 92);
            this.button_rot_cw.TabIndex = 1;
            this.button_rot_cw.Text = "ROT_CW";
            this.button_rot_cw.UseVisualStyleBackColor = true;
            // 
            // button_rot_ccw
            // 
            this.button_rot_ccw.BackgroundImage = global::HelloWorld.Properties.Resources.pad;
            this.button_rot_ccw.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button_rot_ccw.Location = new System.Drawing.Point(408, 261);
            this.button_rot_ccw.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button_rot_ccw.Name = "button_rot_ccw";
            this.button_rot_ccw.Size = new System.Drawing.Size(100, 92);
            this.button_rot_ccw.TabIndex = 2;
            this.button_rot_ccw.Text = "ROT_CCW";
            this.button_rot_ccw.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Honeydew;
            this.pictureBox1.Location = new System.Drawing.Point(345, 347);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(33, 6);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // stage1
            // 
            this.stage1.BackColor = System.Drawing.Color.Gainsboro;
            this.stage1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("stage1.BackgroundImage")));
            this.stage1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.stage1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.stage1.Location = new System.Drawing.Point(0, -1);
            this.stage1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.stage1.Name = "stage1";
            this.stage1.Size = new System.Drawing.Size(400, 369);
            this.stage1.TabIndex = 0;
            this.stage1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.stage1_MouseClick);
            // 
            // numericUpDown_step
            // 
            this.numericUpDown_step.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.numericUpDown_step.Location = new System.Drawing.Point(408, 144);
            this.numericUpDown_step.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.numericUpDown_step.Maximum = new decimal(new int[] {
            800,
            0,
            0,
            0});
            this.numericUpDown_step.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numericUpDown_step.Name = "numericUpDown_step";
            this.numericUpDown_step.Size = new System.Drawing.Size(100, 22);
            this.numericUpDown_step.TabIndex = 5;
            this.numericUpDown_step.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericUpDown_step.ValueChanged += new System.EventHandler(this.numericUpDown_step_ValueChanged);
            // 
            // numericUpDown_delay
            // 
            this.numericUpDown_delay.Location = new System.Drawing.Point(408, 196);
            this.numericUpDown_delay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.numericUpDown_delay.Size = new System.Drawing.Size(100, 22);
            this.numericUpDown_delay.TabIndex = 6;
            this.numericUpDown_delay.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown_delay.ValueChanged += new System.EventHandler(this.numericUpDown_delay_ValueChanged);
            // 
            // StageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 368);
            this.Controls.Add(this.numericUpDown_delay);
            this.Controls.Add(this.numericUpDown_step);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button_rot_ccw);
            this.Controls.Add(this.button_rot_cw);
            this.Controls.Add(this.stage1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "StageForm";
            this.Text = "Stage";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StageForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_step)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_delay)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Stage stage1;
        private System.Windows.Forms.Button button_rot_cw;
        private System.Windows.Forms.Button button_rot_ccw;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.NumericUpDown numericUpDown_step;
        private System.Windows.Forms.NumericUpDown numericUpDown_delay;
    }
}