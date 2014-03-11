using System;
using Y_Visualization.Drawing;

namespace Y_DebugTool
{
    partial class DebugTool
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
            this.displayPanel = new System.Windows.Forms.Panel();
            this.horizontalSplit = new System.Windows.Forms.SplitContainer();
            this.rbgdViewerTop = new Y_Visualization.Drawing.RgbdViewer();
            this.rbgdViewerBottom = new Y_Visualization.Drawing.RgbdViewer();
            this.displayPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.horizontalSplit)).BeginInit();
            this.horizontalSplit.Panel1.SuspendLayout();
            this.horizontalSplit.Panel2.SuspendLayout();
            this.horizontalSplit.SuspendLayout();
            this.SuspendLayout();
            // 
            // displayPanel
            // 
            this.displayPanel.Controls.Add(this.horizontalSplit);
            this.displayPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.displayPanel.Location = new System.Drawing.Point(0, 0);
            this.displayPanel.Name = "displayPanel";
            this.displayPanel.Size = new System.Drawing.Size(1008, 730);
            this.displayPanel.TabIndex = 0;
            this.displayPanel.Resize += new System.EventHandler(this.DisplayPanelResize);
            // 
            // horizontalSplit
            // 
            this.horizontalSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.horizontalSplit.Location = new System.Drawing.Point(0, 0);
            this.horizontalSplit.Name = "horizontalSplit";
            this.horizontalSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // horizontalSplit.Panel1
            // 
            this.horizontalSplit.Panel1.Controls.Add(this.rbgdViewerTop);
            // 
            // horizontalSplit.Panel2
            // 
            this.horizontalSplit.Panel2.Controls.Add(this.rbgdViewerBottom);
            this.horizontalSplit.Size = new System.Drawing.Size(1008, 730);
            this.horizontalSplit.SplitterDistance = 345;
            this.horizontalSplit.TabIndex = 0;
            // 
            // rbgdViewerTop
            // 
            this.rbgdViewerTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.rbgdViewerTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbgdViewerTop.Location = new System.Drawing.Point(0, 0);
            this.rbgdViewerTop.Name = "rbgdViewerTop";
            this.rbgdViewerTop.Size = new System.Drawing.Size(1008, 345);
            this.rbgdViewerTop.TabIndex = 0;
            // 
            // rbgdViewerBottom
            // 
            this.rbgdViewerBottom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.rbgdViewerBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbgdViewerBottom.Location = new System.Drawing.Point(0, 0);
            this.rbgdViewerBottom.Name = "rbgdViewerBottom";
            this.rbgdViewerBottom.Size = new System.Drawing.Size(1008, 381);
            this.rbgdViewerBottom.TabIndex = 0;
            // 
            // DebugTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 730);
            this.Controls.Add(this.displayPanel);
            this.Name = "DebugTool";
            this.ShowIcon = false;
            this.Text = "Y-DebugTool";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DebugToolFormClosing);
            this.Load += new System.EventHandler(this.DebugToolLoad);
            this.displayPanel.ResumeLayout(false);
            this.horizontalSplit.Panel1.ResumeLayout(false);
            this.horizontalSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.horizontalSplit)).EndInit();
            this.horizontalSplit.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel displayPanel;
        private System.Windows.Forms.SplitContainer horizontalSplit;
        private String _applicationText = "Y-DebugTool";
        private RgbdViewer rbgdViewerTop;
        private RgbdViewer rbgdViewerBottom;
    }
}

