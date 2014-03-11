using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Y_DebugTool
{
    public partial class RgbdViewer : UserControl
    {
        public RgbdViewer()
        {
            InitializeComponent();
            leftPictureBox.MouseDown += LeftPictureBoxClick;
            rightPictureBox.MouseDown += RightPictureBoxClick;
        }

        private void RgbdViewerResize(object sender, EventArgs e)
        {
            splitter.SplitterDistance = Width/2;
        }

        public void DisplayFrames(Image left, Image right)
        {
            leftPictureBox.Image = left;
            rightPictureBox.Image = right;
        }

        private void LeftPictureBoxClick(object sender, MouseEventArgs mouseEventArgs)
        {
            if (leftPictureBox.Image != null && PointSelected != null && mouseEventArgs.Button == MouseButtons.Left)
                PointSelected.Invoke(this, new PointClickEventArgs((float)mouseEventArgs.X / leftPictureBox.Width, (float)mouseEventArgs.Y / leftPictureBox.Height));
        }
        private void RightPictureBoxClick(object sender, MouseEventArgs mouseEventArgs)
        {
            if (leftPictureBox.Image != null && PointSelected != null && mouseEventArgs.Button == MouseButtons.Left)
                PointSelected.Invoke(this, new PointClickEventArgs((float)mouseEventArgs.X / rightPictureBox.Width, (float)mouseEventArgs.Y / rightPictureBox.Height));

        }

        public event EventHandler<PointClickEventArgs> PointSelected;

        public class PointClickEventArgs : EventArgs
        {
            public float X { private set; get; }        
            public float Y { private set; get; }
            
            public PointClickEventArgs(float x, float y)
            {
                X = x;
                Y = y;
            }
        }
    }
}
