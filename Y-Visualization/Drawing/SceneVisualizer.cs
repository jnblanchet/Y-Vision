﻿using System;
using System.Drawing;
using System.Linq;

namespace Y_Visualization.Drawing
{
    public class SceneVisualizer
    {
        private Bitmap _scene;
        
        public SceneVisualizer() {
            
        }

        public Bitmap drawScene(int w, int h, Point positionSensor1, Point positionSensor2, double angle1, double angle2, double range/*, double FoV1, double FoV2*/)
        {
            // Init
            if (_scene == null || _scene.Width != w || _scene.Height != h)
                _scene = new Bitmap(w,h);

            Graphics g = Graphics.FromImage(_scene);
            g.Clear(Color.White);

            // Draw center
            Point center = new Point(w / 2, h / 2);
            var bluePen = new Pen(Brushes.DodgerBlue, 2);
            g.DrawLine(bluePen, center.X, center.Y, center.X + 100, center.Y);
            g.DrawLine(bluePen, center.X, center.Y, center.X, center.Y - 100);
            g.DrawString("X", new Font("Arial", 10), Brushes.DodgerBlue, center.X + 10, center.Y + 10);
            g.DrawString("Z", new Font("Arial", 10), Brushes.DodgerBlue, center.X - 20, center.Y - 30);

            // Draw sensors
            // Find the the drawing ratio in Pixel per PointUnit7
            double scale = (Math.Min(w * 0.9d, h * 0.9d) / 2) / (new[] { Math.Abs(positionSensor1.X), Math.Abs(positionSensor1.Y), Math.Abs(positionSensor2.X), Math.Abs(positionSensor2.Y) }).Max();

            var redPen = new Pen(Brushes.Red, 2);
            var drawnCoords1 = new Point(center.X + (int)(positionSensor1.X * -scale), center.Y + (int)(positionSensor1.Y * scale));
            g.DrawRectangle(redPen, drawnCoords1.X - 10, drawnCoords1.Y - 10, 20, 20);

            var orangePen = new Pen(Brushes.Orange, 2);
            var drawnCoords2 = new Point(center.X + (int)(positionSensor2.X * -scale), center.Y + (int)(positionSensor2.Y * scale));
            g.DrawRectangle(orangePen, drawnCoords2.X - 10, drawnCoords2.Y - 10, 20, 20);

            
            //Draw field of view
            //Compute start angle
            //var alpha1 = (float)(Math.Atan2((center.Y - drawnCoords1.Y),(center.X - drawnCoords1.X)) * 180.0d / Math.PI - ((double)offSetLeft1 / 320 * 57.0d));
            //var alpha2 = (float)(Math.Atan2((center.Y - drawnCoords2.Y),(center.X - drawnCoords2.X)) * 180.0d / Math.PI - ((double)offSetLeft2 / 320 * 57.0d));


            g.DrawPie(redPen, (float)(drawnCoords1.X - range * scale), (float)(drawnCoords1.Y - range * scale), (float)(range * scale * 2), (float)(range * scale * 2), (float)(angle1), 57.0f);
            g.DrawPie(orangePen, (float)(drawnCoords2.X - range * scale), (float)(drawnCoords2.Y - range * scale), (float)(range * scale * 2), (float)(range * scale * 2), (float)(angle2), 57.0f);

            g.Flush();

            return _scene;
        }
    }
}