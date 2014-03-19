using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Y_Vision.BlobDescriptor
{
    public class HoGManager
    {
        private Cell[,] _cells;
        public HoGManager()
        {
            
        }

        private const int PixelsPerCell = 6; // square area
        private const int CellsPerBlock = 3; // square area: this constant is implicit, it's not really used...todo: see if adding flexibility to use it affects performance a lot

        public void ComputeCells(byte[] rgbImage, int w, int h)
        {
            // Handle invalid call
            if (rgbImage == null)
                return;

            w -= 2; // we need to exclude a 1x1 px frame for sobel
            h -= 2;

            // Reinit Cell structure
            int cellArrayW = w/PixelsPerCell;
            int cellArrayH = h/PixelsPerCell;

            if (_cells == null || _cells.GetLength(1) != cellArrayW)
                _cells = new Cell[cellArrayH,cellArrayW];
            else
                foreach (var cell in _cells)
                    cell.Reinit();

            //TODO: MAJOR OPTIMIZATION IDEA: USE "PROXY PATTERN" TO LOAD CELLS AS THEY ARE QUERIED INSTEAD OF GENERATING MILLIONS OF CELLS ALL DAY EVERY DAY
            // Apply sobel to generate cells (start at 1 to to have a 1x1 padding border)
            int y0 = 1; // x0,y0 is the top left corner pixel of the cell, x1,y1 is the bottom right corner
            int y1 = y0 + PixelsPerCell;
            Parallel.For(0, cellArrayH, j => // Parallel.For Begin
                                            {
                                                int x0 = 1;
                                                int x1 = x0 + PixelsPerCell;
                                                for (int i = 0; i < cellArrayW; i++)
                                                {
                                                    _cells[j, i].Reinit();
                                                    for (int y = y0; y < y1; y++)
                                                        for (int x = x0; x < x1; x++)
                                                        {
                                                            double dx = GetPixelAt(rgbImage, x + 1, y, w) -
                                                                        GetPixelAt(rgbImage, x - 1, y, w);
                                                            double dy = GetPixelAt(rgbImage, x, y + 1, w) -
                                                                        GetPixelAt(rgbImage, x, y - 1, w);

                                                            double mag = Math.Sqrt(dx*dx + dy*dy);
                                                            double orien = Math.Atan(dy/dx);
                                                            _cells[j, i].AddSample(mag, orien);
                                                        }
                                                    _cells[j, i].FinalizeCell();
                                                    x0 += PixelsPerCell;
                                                    x1 += PixelsPerCell;
                                                }
                                                y0 += PixelsPerCell;
                                                y1 += PixelsPerCell;
                                            }); // Parallel.For End


            // Form blocs and normalize magnitude
            for (int j = 0; j < cellArrayH; j++)
            {
                for (int i = 0; i < cellArrayW; i++)
                {
                    double neighborMag = 0;
                    int sampleCount = 0;
                    for (int y = Math.Max(j - 1, 0); y <= Math.Min(j + 1, cellArrayH - 1); y++)
                    {
                        for (int x = Math.Max(i - 1, 0); x <= Math.Min(i + 1, cellArrayW - 1); x++)
                        {
                            if(y != 0 || x != 0)
                            {
                                neighborMag += _cells[y, x].Magnitude;
                                sampleCount++;
                            }

                        }
                    }
                    _cells[j, i].Magnitude = (neighborMag + _cells[j, i].Magnitude)/(sampleCount+1); // average between neighborhood average magnitude and local magnitude
                }
            }
        }

        static Pixel GetPixelAt(byte[] img, int x, int y, int w)
        {
            int index = (w*y + x) << 2;//4 byte per pixel (shift for 2 instead of multiplying by 4)
            return new Pixel {R = img[index], G = img[index + 1], B = img[index + 2]};
        }

        public const int BinCount = 9;
        private static readonly double Min = -1*Math.PI/2;
        private static readonly double Max = Math.PI/2;
        private static readonly double Step = (Max - Min) / BinCount;
        private static readonly float[] BinThresholds = new[]{ // contains the threshold (angles from -Pi to Pi) for the bins
                                                            -1,570796327f,
                                                            -1,221730476f,
                                                            -0,872664626f,
                                                            -0,523598776f,
                                                            -0,174532925f,
                                                            0,174532925f,
                                                            0,523598776f,
                                                            0,872664626f,
                                                            1,221730476};

        public float[] GetHistogram(int x0, int y0, int x1, int y1)
        {
            var histogram = new float[BinCount];

            const int roundPadding = PixelsPerCell/2;

            int i0 = (x0 + roundPadding)/PixelsPerCell;
            int i1 = (x1 + roundPadding)/PixelsPerCell;
            int j0 = (y0 + roundPadding)/PixelsPerCell;
            int j1 = (y1 + roundPadding)/PixelsPerCell;

            for (int j = j0; j <= j1; j++)
            {
                for (int i = i0; i <= i1; i++)
                {
                    int lowerBinIndex = (int)((_cells[j, i].Orientation - Min)/Step);
                    histogram[lowerBinIndex] += (float)(_cells[j, i].Magnitude * Math.Abs(_cells[j, i].Orientation - BinThresholds[lowerBinIndex]) / Step);
                    histogram[lowerBinIndex+1] += (float)(_cells[j, i].Magnitude * Math.Abs(_cells[j, i].Orientation - BinThresholds[lowerBinIndex+1]) / Step);
                }
            }

            return histogram;
        }

        // TODO: speed up atan with an approximation (also unit test the method?)
        /*float em_atan(float y)
        {
            float ax, ay;
            float t0, t1;
            ax = Math.(1.0f);
            ay = em_fabs(y);
            t0 = em_fmax(ax, ay);
            t1 = em_fmin(ax, ay);

            t0 = 1.0F / t0;
            t0 = arctanapx_(t0 * t1);

            t0 = (ax < ay) ? EM_PI_2 - t0 : t0;
            t0 = (y < 0.0F) ? -t0 : t0;
            return t0;
        }*/


        private struct Pixel
        {
            public byte R, G, B;

            public static int operator -(Pixel px1, Pixel px2)
            {
                return (px1.R - px2.R) + (px1.G - px2.G) + (px1.B - px2.B); // manhattan heuristic for better performance
            }
        }

        private struct Cell
        {
            public double Magnitude;
            public double Orientation;
            public double Count;
            
            public void AddSample(double mag, double orien)
            {
                Magnitude += mag;
                Orientation += orien;
                Count++;
            }

            public void FinalizeCell()
            {
                if (Count > 0)
                {
                    Magnitude /= Count;
                    Orientation /= Count;
                    Count = 0;
                }
            }

            public void Reinit()
            {
                Magnitude = 0;
                Orientation = 0;
                Count = 0;
            }
        }


    }
}
