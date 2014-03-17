namespace Y_Emulator
{
    partial class EmulatorForm
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
            this.SuspendLayout();
            // 
            // EventListBox
            // 
            this.EventListBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.EventListBox.FormattingEnabled = true;
            this.EventListBox.Location = new System.Drawing.Point(0, 613);
            this.EventListBox.Name = "EventListBox";
            this.EventListBox.Size = new System.Drawing.Size(912, 108);
            this.EventListBox.TabIndex = 2;
            // 
            // EmulatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(912, 721);
            this.Controls.Add(this.EventListBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "EmulatorForm";
            this.Text = "Y-Vision Emulator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EmulatorForm_FormClosing);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.EmulatorForm_MouseClick);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.EmulatorForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.EmulatorForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.EmulatorForm_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox EventListBox;
    }
}

