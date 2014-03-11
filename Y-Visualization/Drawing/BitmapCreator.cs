using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Y_Vision.BlobDescriptor;
using Y_Vision.Tracking;
using Point = Y_Vision.Core.Point;

namespace Y_Visualization.Drawing
{
    public class BitmapCreator
    {
        // We need another buffer to store the grayscale/color version of the frame (modifying the input buffer will result in a race condition)
        private short[] _depthBufferForDrawing;
        // A single Bitmap object that will be reused after every call
        private Bitmap _depthBitamp;
        public Bitmap DepthBitamp
        {
            get { return _depthBitamp; }
        }

        private Bitmap _colorBitamp;
        public Bitmap ColorBitamp
        {
            get { return _colorBitamp; }
        }


        // Graphics
        private Graphics _depthGraphics;
        private Graphics _colorGraphics;

        public void CreateBitmapFromDepthFrame(short[] depth, int height, int width)
        {
            if (depth != null)
            {
                if (_depthBufferForDrawing == null || DepthBitamp.Width != width) // Dirty Index reinit
                    _depthBufferForDrawing = new short[depth.Length];

                // This modifies _depthBufferForDrawing
                GrayscaleConversion(depth);

                ShortToBitmap(_depthBufferForDrawing, height, width);
            }
        }

        private void ShortToBitmap(short[] depth, int height, int width)
        {
            if (_depthBitamp == null || _depthBitamp.Width != width) // Width used as a dirty index
            {
                _depthBitamp = new Bitmap(width, height, PixelFormat.Format16bppRgb555);
                _depthGraphics = Graphics.FromImage(_depthBitamp);
                _depthGraphics.Clear(Color.FromArgb(0, 255, 0));
            }
            var bmapdata = _depthBitamp.LockBits(new Rectangle(0, 0, width, height),
                                                    ImageLockMode.WriteOnly,
                                                    _depthBitamp.PixelFormat);
            IntPtr ptr = bmapdata.Scan0;
            Marshal.Copy(depth, 0, ptr, width*height);
            _depthBitamp.UnlockBits(bmapdata);
        }

        private void GrayscaleConversion(short[] depth)
        {
            const ushort maxDepth = 4095;
            const ushort fiveBitsMask = 31;

            for (int i = 0; i < depth.Length; i++)
            {
                var gray = (byte) (depth[i] * fiveBitsMask / maxDepth);
                _depthBufferForDrawing[i] = (short)(gray << 10 | gray << 5 | gray);
            }
        }


        private const int ColorStreamBytesPerPixel = 4;
        public void CreateBitmapFromColorFrame(byte[] color, int height, int width)
        {
            if (color != null)
            {
                if (_colorBitamp == null || _colorBitamp.Width != width)
                {
                    _colorBitamp = new Bitmap(width, height, PixelFormat.Format32bppRgb);
                    _colorGraphics = Graphics.FromImage(_colorBitamp);
                    _colorGraphics.Clear(Color.FromArgb(255, 255, 255));
                }

                var bmapdata = _colorBitamp.LockBits(new Rectangle(0, 0, width, height),
                                                            ImageLockMode.WriteOnly,
                                                            _colorBitamp.PixelFormat);
                IntPtr ptr = bmapdata.Scan0;
                Marshal.Copy(color, 0, ptr, width * height * ColorStreamBytesPerPixel);
                _colorBitamp.UnlockBits(bmapdata);
            }
        }

        private static readonly Color[] LightColors =
            {
                Color.MistyRose, Color.LemonChiffon, Color.Lavender, Color.Gainsboro, Color.PeachPuff, Color.NavajoWhite, Color.PaleGoldenrod, Color.PaleTurquoise,Color.Pink,
                Color.Silver, Color.Khaki, Color.LightGreen, Color.Aquamarine, Color.LightBlue,Color.LightSkyBlue, Color.LightSteelBlue
            };
        private static readonly Color[] DarkColors =
            {
                Color.Red, Color.Chartreuse, Color.DeepSkyBlue, Color.RosyBrown, Color.Tomato, Color.MediumSlateBlue, Color.PaleVioletRed, Color.DarkOrange,Color.Yellow,
                Color.Turquoise, Color.Blue, Color.LightGreen, Color.Brown, Color.Goldenrod,Color.DarkViolet, Color.DarkGoldenrod
            };
        /*public void CreateBitmapFromIndex(short[,] index)
        {
            if (index != null)
            {
                int h = index.GetLength(0), w = index.GetLength(1);
                if (_depthBitamp == null)
                {
                    _depthBitamp = new Bitmap(w, h);
                    _depthGraphics = Graphics.FromImage(_depthBitamp);
                    _depthGraphics.Clear(Color.FromArgb(255, 255, 255));
                }

                for (int j = 1; j <= h - 1; j++)
                {
                    for (int i = 1; i <= w - 1; i++)
                    {
                        _depthBitamp.SetPixel(i, j, colors[Math.Max(index[j, i] & 0xF, 0)]);
                    }
                }
            }
        }*/

        private readonly short[] _shrtColors = LightColors.Select(c => (short)((c.R >> 3) << 10 | (c.G >> 3) << 5 | (c.B >> 3))).ToArray();
        // Alternative version of the method above. Hopefully faster.
        public void CreateBitmapFromIndex(short[,] index)
        {
            if (index != null)
            {
                int h = index.GetLength(0), w = index.GetLength(1);

                if (_depthBufferForDrawing == null || DepthBitamp.Width != w) // Dirty Index reinit
                    _depthBufferForDrawing = new short[index.Length];

                int k = 0;
                for (int j = 0; j <= h - 1; j++)
                {
                    for (int i = 0; i <= w - 1; i++, k++)
                    {
                        _depthBufferForDrawing[k] = _shrtColors[index[j, i] & 0xF];
                    }
                }
                ShortToBitmap(_depthBufferForDrawing,h,w);
            }
        }
        
        public void DrawTrackedPeople(Bitmap bmp, IEnumerable<TrackedObject> trackedObjects, double scaleFactor = 1)
        {
            if (bmp != null && trackedObjects != null)
            {
                Graphics g = Graphics.FromImage(bmp);

                var pen = new Pen(Brushes.GreenYellow, 4);
                foreach (var p in trackedObjects)
                {
                    var rect = new Rectangle((int)(p.X - (p.Width >> 1)), (int)(p.Y - (p.Height >> 1)), p.Width, p.Height);
                    rect.Width = (int)(rect.Width * scaleFactor);
                    rect.Height = (int)(rect.Height * scaleFactor);
                    rect.X = (int)(rect.X * scaleFactor);
                    rect.Y = (int)(rect.Y * scaleFactor);
                    g.DrawRectangle(pen, rect);
                }
                g.Flush();
            }
        }

        public void DrawTrackedPeople(Bitmap bmp, IEnumerable<BlobObject> blobs, double scaleFactor = 1)
        {
            if (bmp != null && blobs != null)
            {
                Graphics g = Graphics.FromImage(bmp);

                var pen = new Pen(Brushes.Red, (int)(4 * scaleFactor));
                var pen2 = new Pen(Brushes.DarkRed, (int)(4 * scaleFactor));
                foreach (var p in blobs)
                {
                    var rect = new Rectangle(p.MinX, p.MinY, p.Width, p.Height);
                    rect.Width = (int) (rect.Width*scaleFactor);
                    rect.Height = (int) (rect.Height*scaleFactor);
                    rect.X = (int) (rect.X*scaleFactor);
                    rect.Y = (int) (rect.Y*scaleFactor);
                    g.DrawRectangle(pen, rect);
                    g.DrawRectangle(pen2, new Rectangle((int)((p.X - 2) * scaleFactor), (int)((p.Y - 2) * scaleFactor), (int) (4 * scaleFactor), (int) (4 * scaleFactor)));
                }
                g.Flush();
            }
        }

        public void DrawPoint(Bitmap bmp, Point p, double scaleFactor = 1)
        {
            if (bmp != null)
            {
                Graphics g = Graphics.FromImage(bmp);

                var pen2 = new Pen(Brushes.Chartreuse, (int)(4 * scaleFactor));
                g.DrawRectangle(pen2, new Rectangle((int)((p.X - 2) * scaleFactor), (int)((p.Y - 2) * scaleFactor), (int)(4 * scaleFactor), (int)(4 * scaleFactor)));
                g.Flush();
            }
        }

        public void DrawPointsWithUniqueColor(Bitmap bmp, IEnumerable<Point> points, double scaleFactor = 1)
        {
            if (bmp != null && points != null)
            {
                Graphics g = Graphics.FromImage(bmp);

                var i = 0;
                    foreach (var point in points)
                    {
                        var pen2 = new Pen(DarkColors[i++ & 0xF], (int)(4 * scaleFactor));
                        g.DrawRectangle(pen2, new Rectangle((int)((point.X - 2) * scaleFactor), (int)((point.Y - 2) * scaleFactor), (int)(4 * scaleFactor), (int)(4 * scaleFactor)));
                        g.Flush();
                    }
            }
        }
    }
}
