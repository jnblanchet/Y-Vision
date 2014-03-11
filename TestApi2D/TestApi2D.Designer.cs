namespace TestApi2D
{
    partial class FormTestApi2D
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
            this.TrackedObjectsListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // EventListBox
            // 
            this.EventListBox.Dock = System.Windows.Forms.DockStyle.Right;
            this.EventListBox.FormattingEnabled = true;
            this.EventListBox.Location = new System.Drawing.Point(335, 0);
            this.EventListBox.Name = "EventListBox";
            this.EventListBox.Size = new System.Drawing.Size(358, 383);
            this.EventListBox.TabIndex = 0;
            this.EventListBox.SelectedIndexChanged += new System.EventHandler(this.EventListBoxSelectedIndexChanged);
            // 
            // TrackedObjectsListBox
            // 
            this.TrackedObjectsListBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.TrackedObjectsListBox.FormattingEnabled = true;
            this.TrackedObjectsListBox.Location = new System.Drawing.Point(0, 0);
            this.TrackedObjectsListBox.Name = "TrackedObjectsListBox";
            this.TrackedObjectsListBox.Size = new System.Drawing.Size(331, 383);
            this.TrackedObjectsListBox.TabIndex = 1;
            // 
            // FormTestApi2D
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 383);
            this.Controls.Add(this.TrackedObjectsListBox);
            this.Controls.Add(this.EventListBox);
            this.Name = "FormTestApi2D";
            this.Text = "TestApi2D";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTestApi2DFormClosing);
            this.Load += new System.EventHandler(this.FormTestApi2DLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox EventListBox;
        private System.Windows.Forms.ListBox TrackedObjectsListBox;
    }
}

