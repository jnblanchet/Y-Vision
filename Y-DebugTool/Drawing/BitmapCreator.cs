using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Y_Vision.Drawing
{
    public class BitmapCreator
    {
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
                grayscaleConversion(depth);
                if(_depthBitamp == null)
                {
                    _depthBitamp = new Bitmap(width, height, PixelFormat.Format16bppRgb555);
                    _depthGraphics = Graphics.FromImage(_depthBitamp);
                    _depthGraphics.Clear(Color.FromArgb(200, 200, 0));
                }
                var bmapdata = _depthBitamp.LockBits(new Rectangle(0, 0, width, height),
                                                            ImageLockMode.WriteOnly,
                                                            _depthBitamp.PixelFormat);
                IntPtr ptr = bmapdata.Scan0;
                Marshal.Copy(depth, 0, ptr, width * height);
                _depthBitamp.UnlockBits(bmapdata);
            }
        }

        private void grayscaleConversion(short[] depth)
        {
            const ushort maxDepth = 4095;
            const ushort fiveBitsMask = 31;

            for (int i = 0; i < depth.Length; i++)
            {
                short gray = (short) (depth[i] * fiveBitsMask / maxDepth);
                depth[i] = (short)(gray << 10 | gray << 5 | gray);
            }

            for(int i = 0; i < depth.Length;i++)
            {
                /*if (depth[i] >> 3 > GreenBits) // if it overflows the green
                {
                    depth[i] <<= 4; // Display the 4 most significant bits
                    depth[i] &= ~GreenBits & ~BlueBits; // the rest is 1
                }
                else if (depth[i] >> 3 > BlueBits)
                {
                    depth[i] <<= 5;
                    depth[i] &= ~BlueBits;
                }
                else
                {*/
                //}
            }
        }


        private const int ColorStreamBytesPerPixel = 4;
        public void CreateBitmapFromColorFrame(byte[] color, int height, int width)
        {
            if (color != null)
            {
                if (_colorBitamp == null)
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

        private Color[] c =
            {
                Color.Aqua, Color.Brown, Color.Yellow, Color.Green, Color.Red, Color.Orange, Color.DarkSeaGreen, Color.Salmon,Color.DarkGray,
                Color.DeepPink, Color.LawnGreen, Color.Blue, Color.BlueViolet, Color.DeepPink,Color.BurlyWood, Color.SpringGreen
            };
        public void CreateBitmapFromIndex(short[,] index)
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
                    _depthBitamp.SetPixel(i, j, c[Math.Max(index[j, i] & 0xF, 0)]);
                }
            }
        }
    }
}
