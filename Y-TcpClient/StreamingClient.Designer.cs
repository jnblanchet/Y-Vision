namespace Y_TcpClient
{
    partial class StreamingClient
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
            this.EventListBox = new System.Windows.Forms.ListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.selectDetectorTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dSingleSensorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dParallaxToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // EventListBox
            // 
            this.EventListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EventListBox.FormattingEnabled = true;
            this.EventListBox.Location = new System.Drawing.Point(0, 24);
            this.EventListBox.Name = "EventListBox";
            this.EventListBox.Size = new System.Drawing.Size(780, 326);
            this.EventListBox.TabIndex = 1;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectDetectorTypeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(780, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // selectDetectorTypeToolStripMenuItem
            // 
            this.selectDetectorTypeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dSingleSensorToolStripMenuItem,
            this.dParallaxToolStripMenuItem});
            this.selectDetectorTypeToolStripMenuItem.Name = "selectDetectorTypeToolStripMenuItem";
            this.selectDetectorTypeToolStripMenuItem.Size = new System.Drawing.Size(124, 20);
            this.selectDetectorTypeToolStripMenuItem.Text = "Select Detector type";
            // 
            // dSingleSensorToolStripMenuItem
            // 
            this.dSingleSensorToolStripMenuItem.Name = "dSingleSensorToolStripMenuItem";
            this.dSingleSensorToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.dSingleSensorToolStripMenuItem.Text = "2D single sensor";
            this.dSingleSensorToolStripMenuItem.Click += new System.EventHandler(this.TwoDSingleSensorToolStripMenuItemClick);
            // 
            // dParallaxToolStripMenuItem
            // 
            this.dParallaxToolStripMenuItem.Name = "dParallaxToolStripMenuItem";
            this.dParallaxToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.dParallaxToolStripMenuItem.Text = "3D parallax";
            this.dParallaxToolStripMenuItem.Click += new System.EventHandler(this.ThreeDParallaxToolStripMenuItemClick);
            // 
            // StreamingClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(780, 350);
            this.Controls.Add(this.EventListBox);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "StreamingClient";
            this.Text = "StreamingClient";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StreamingClientFormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox EventListBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem selectDetectorTypeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dSingleSensorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dParallaxToolStripMenuItem;
    }
}