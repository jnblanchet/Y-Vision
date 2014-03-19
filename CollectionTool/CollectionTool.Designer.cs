namespace CollectionTool
{
    partial class MainForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toggleStartStopButton = new System.Windows.Forms.Button();
            this.OutputPictureBox = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.sampleRate = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.OutputPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleRate)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.sampleRate);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.toggleStartStopButton);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.OutputPictureBox);
            this.splitContainer1.Size = new System.Drawing.Size(444, 290);
            this.splitContainer1.SplitterDistance = 99;
            this.splitContainer1.TabIndex = 0;
            // 
            // toggleStartStopButton
            // 
            this.toggleStartStopButton.Location = new System.Drawing.Point(13, 13);
            this.toggleStartStopButton.Name = "toggleStartStopButton";
            this.toggleStartStopButton.Size = new System.Drawing.Size(75, 23);
            this.toggleStartStopButton.TabIndex = 0;
            this.toggleStartStopButton.Text = "Start";
            this.toggleStartStopButton.UseVisualStyleBackColor = true;
            this.toggleStartStopButton.Click += new System.EventHandler(this.ToggleStartStopButtonClick);
            // 
            // OutputPictureBox
            // 
            this.OutputPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutputPictureBox.Location = new System.Drawing.Point(0, 0);
            this.OutputPictureBox.Name = "OutputPictureBox";
            this.OutputPictureBox.Size = new System.Drawing.Size(341, 290);
            this.OutputPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.OutputPictureBox.TabIndex = 0;
            this.OutputPictureBox.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "sample once every";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "frame(s)";
            // 
            // sampleRate
            // 
            this.sampleRate.Location = new System.Drawing.Point(7, 68);
            this.sampleRate.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.sampleRate.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.sampleRate.Name = "sampleRate";
            this.sampleRate.Size = new System.Drawing.Size(41, 20);
            this.sampleRate.TabIndex = 3;
            this.sampleRate.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(444, 290);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainForm";
            this.Text = "CollectionTool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainFormFormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.OutputPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sampleRate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button toggleStartStopButton;
        private System.Windows.Forms.PictureBox OutputPictureBox;
        private System.Windows.Forms.NumericUpDown sampleRate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}

