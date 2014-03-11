using System;
using System.Drawing;
using System.Windows.Forms;

namespace Y_Visualization.Drawing
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
            if (left != null)
                leftPictureBox.Image = left;
            if (right != null)
                rightPictureBox.Image = right;
        }

        private void LeftPictureBoxClick(object sender, MouseEventArgs mouseEventArgs)
        {
            if (leftPictureBox.Image != null && PointSelected != null && mouseEventArgs.Button == MouseButtons.Left)
            {
                var pointOnImage = TranslateZoomMousePosition(mouseEventArgs.X, mouseEventArgs.Y, leftPictureBox.Image.Width, leftPictureBox.Image.Height, leftPictureBox.Width, leftPictureBox.Height);
                if (pointOnImage.X >= 0 && pointOnImage.X <= leftPictureBox.Image.Width && pointOnImage.Y >= 0 && pointOnImage.Y <= leftPictureBox.Image.Height)
                    PointSelected.Invoke(this, new PointClickEventArgs(1, (float)pointOnImage.X / leftPictureBox.Image.Width, (float)pointOnImage.Y / leftPictureBox.Image.Height));
            }
        }
        private void RightPictureBoxClick(object sender, MouseEventArgs mouseEventArgs)
        {
            if (rightPictureBox.Image != null && PointSelected != null && mouseEventArgs.Button == MouseButtons.Left)
            {
                var pointOnImage = TranslateZoomMousePosition(mouseEventArgs.X, mouseEventArgs.Y, rightPictureBox.Image.Width, rightPictureBox.Image.Height, rightPictureBox.Width, rightPictureBox.Height);
                if (pointOnImage.X >= 0 && pointOnImage.X <= rightPictureBox.Image.Width && pointOnImage.Y >= 0 && pointOnImage.Y <= rightPictureBox.Image.Height)
                    PointSelected.Invoke(this, new PointClickEventArgs(2, (float)pointOnImage.X / rightPictureBox.Image.Width, (float)pointOnImage.Y / rightPictureBox.Image.Height));
            }
        }

        public event EventHandler<PointClickEventArgs> PointSelected;

        public class PointClickEventArgs : EventArgs
        {
            public float X { private set; get; }        
            public float Y { private set; get; }
            /// <summary>
            /// Defines the source of the event, 1 for left, 2 for right.
            /// </summary>
            public int SourceId { private set; get; }
            
            public PointClickEventArgs(int source, float x, float y)
            {
                X = x;
                Y = y;
                SourceId = source;
            }
        }

        protected Point TranslateZoomMousePosition(int pointX, int pointY, int imageW, int imageH, int containerW, int containerH)
        {
            var unscaledP = new Point(); 

            float imageRatio = imageW / (float)imageH; // image W:H ratio
            float containerRatio = containerW / (float)containerH; // container W:H ratio

            if (imageRatio >= containerRatio)
            {
                // horizontal image
                float scaleFactor = containerW / (float)imageW;
                float scaledHeight = imageH * scaleFactor;
                // calculate gap between top of container and top of image
                float filler = Math.Abs(containerH - scaledHeight) / 2;
                unscaledP.X = (int)(pointX / scaleFactor);
                unscaledP.Y = (int)((pointY - filler) / scaleFactor);
            }
            else
            {
                // vertical image
                float scaleFactor = containerH / (float)imageH;
                float scaledWidth = imageW * scaleFactor;
                float filler = Math.Abs(containerW - scaledWidth) / 2;
                unscaledP.X = (int)((pointX - filler) / scaleFactor);
                unscaledP.Y = (int)(pointY / scaleFactor);
            }
            return unscaledP;
        }
    }
}
