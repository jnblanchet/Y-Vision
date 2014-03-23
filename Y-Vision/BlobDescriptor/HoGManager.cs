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
        public int[,] CellMagnitude;

        public HoGManager()
        {

        }

        public const int PixelsPerCell = 6; // square area

        private const int CellsPerBlock = 3;
                          // square area: this constant is implicit, it's not really used...todo: see if adding flexibility to use it affects performance a lot

        public void ComputeCells(byte[] rgbImage, int w, int h)
        {
            // Handle invalid call
            if (rgbImage == null)
                return;
            int realWidth = w;
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
            //List<String> output = new List<string>();
            Parallel.For(0, cellArrayH, j => // Parallel.For Begin
                                        //for (int j = 0; j < cellArrayH; j++)
                                            {
                                                int x0 = 1;
                                                int x1 = x0 + PixelsPerCell;
                                                for (int i = 0; i < cellArrayW; i++)
                                                {
                                                    //output.Add("----------- Starting new cell computation [" + i + "," + j + "] -----------" + Environment.NewLine);
                                                    _cells[j, i].Reinit();
                                                    int dx = 0;
                                                    double dy = 0;
                                                    for (int y = y0; y < y1; y++)
                                                        for (int x = x0; x < x1; x++)
                                                        {
                                                            //output.Add("pixel at " + x + "," + y + " has neihborgs:" + Environment.NewLine);
                                                            //output.Add("     Left:" + GetPixelAt(rgbImage, x + 1, y, w) + Environment.NewLine);
                                                            //output.Add("     Right:" + GetPixelAt(rgbImage, x - 1, y, w) + Environment.NewLine);
                                                            //output.Add("     Top:" + GetPixelAt(rgbImage, x, y + 1, w) + Environment.NewLine);
                                                            //output.Add("     Bottom:" + GetPixelAt(rgbImage, x, y - 1, w) + Environment.NewLine);
                                                            dx += GetPixelAt(rgbImage, x + 1, y, realWidth) -
                                                                  GetPixelAt(rgbImage, x - 1, y, realWidth);
                                                            dy += GetPixelAt(rgbImage, x, y + 1, realWidth) -
                                                                  GetPixelAt(rgbImage, x, y - 1, realWidth);
                                                            //output.Add("     Results: dx=" + (GetPixelAt(rgbImage, x + 1, y, w) - GetPixelAt(rgbImage, x - 1, y, w)) + Environment.NewLine);
                                                            //output.Add("              dy=" + (GetPixelAt(rgbImage, x, y + 1, w) - GetPixelAt(rgbImage, x, y - 1, w)) + Environment.NewLine);
                                                        }
                                                    double mag = Math.Sqrt(dx*dx + dy*dy);
                                                    double orien = dx != 0 ? Math.Atan(dy/dx) : Min;
                                                    _cells[j, i].SetCell(mag, orien);
                                                    //output.Add("CELL RESULTS: dx=" + dx + Environment.NewLine);
                                                    //output.Add("              dy=" + dy + Environment.NewLine);
                                                    //output.Add("              mag=" + mag + Environment.NewLine);
                                                    //output.Add("              Orientation=" + 180 * orien / Math.PI + Environment.NewLine);
                                                    //Console.WriteLine("before - " + _cells[j, i].ToString());
                                                    //_cells[j, i].FinalizeCell();

                                                    x0 += PixelsPerCell;
                                                    x1 += PixelsPerCell;
                                                }
                                                y0 += PixelsPerCell;
                                                y1 += PixelsPerCell;
                                            }); // Parallel.For End
            //System.IO.File.WriteAllLines("HogOutput.log", output);
            //output.Clear();

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
                            if (y != 0 || x != 0)
                            {
                                neighborMag += _cells[y, x].Magnitude;
                                sampleCount++;
                            }

                        }
                    }
                    _cells[j, i].SmoothedMag = (neighborMag + _cells[j, i].Magnitude)/(sampleCount + 1);
                        // average between neighborhood average magnitude and local magnitude
                }
            }

            //make cell mag for debug
            CellMagnitude = new int[cellArrayH,cellArrayW];
            for (int j = 0; j < cellArrayH; j++)
            {
                for (int i = 0; i < cellArrayW; i++)
                {
                    CellMagnitude[j, i] = (int) _cells[j, i].Magnitude;
                }
            }

        }

        private static Pixel GetPixelAt(byte[] img, int x, int y, int w)
        {
            int index = (w*y + x) << 2; //4 byte per pixel (shift for 2 instead of multiplying by 4)
            return new Pixel {R = img[index], G = img[index + 1], B = img[index + 2]};
        }

        public const int BinCount = 9;
        private static readonly double Min = -1*Math.PI/2;
        private static readonly double Max = Math.PI/2;
        private static readonly double Step = (Max - Min)/BinCount;

        private static readonly float[] BinThresholds = new[]
                                                            {
                                                                // contains the threshold (angles from -Pi to Pi) for the bins
                                                                -1.570796327f,
                                                                -1.221730476f,
                                                                -0.872664626f,
                                                                -0.523598776f,
                                                                -0.174532925f,
                                                                0.174532925f,
                                                                0.523598776f,
                                                                0.872664626f,
                                                                1.221730476f
                                                            };

        public float[] GetHistogram(int x0, int y0, int x1, int y1, out double totalMagnitude)
        {
            totalMagnitude = 0;
            var histogram = new float[BinCount];

            //const int roundPadding = PixelsPerCell/2;

            int cellArrayW = _cells.GetLength(1) - 1;
            int cellArrayH = _cells.GetLength(0) - 1;

            int i0 = Math.Max((x0 - PixelsPerCell)/PixelsPerCell, 0);
            int i1 = Math.Min((x1 + PixelsPerCell)/PixelsPerCell, cellArrayW);
            int j0 = Math.Max((y0 - PixelsPerCell)/PixelsPerCell, 0);
            int j1 = Math.Min((y1 + PixelsPerCell)/PixelsPerCell, cellArrayH);

            // Compute histogram by checking every cell
            for (int j = j0; j <= j1; j++)
            {
                for (int i = i0; i <= i1; i++)
                {
                    int lowerBinIndex = (int) ((_cells[j, i].Orientation - Min)/Step);
                    double distanceLowerBin = Math.Abs(_cells[j, i].Orientation - BinThresholds[lowerBinIndex])/Step;
                    histogram[lowerBinIndex] += (float) (_cells[j, i].SmoothedMag*(1 - distanceLowerBin));
                    histogram[lowerBinIndex + 1 == BinCount ? 0 : lowerBinIndex + 1] +=
                        (float) (_cells[j, i].SmoothedMag*distanceLowerBin);
                }
            }

            // Normalize histogram
            double total = histogram.Sum();
            totalMagnitude = total;
            return histogram.Select(v => (float) (v/total)).ToArray();
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
                return (px1.R - px2.R) + (px1.G - px2.G) + (px1.B - px2.B);
                    // manhattan heuristic for better performance
            }

            public override string ToString()
            {
                return "R:" + R + ",G:" + G + ",B:" + B;
            }
        }

        private struct Cell
        {
            public double Magnitude;
            public double SmoothedMag;
            public double Orientation;
            //public double Count;

            public void SetCell(double mag, double orien)
            {
                Magnitude = mag;
                Orientation = orien;
                //Count++;
            }

            /*public void FinalizeCell()
            {
                if (Count > 0)
                {
                    Orientation /= Math.Max(Magnitude,1); // weighted average
                    Magnitude /= Count;
                    Count = 0;
                }
            }*/

            public void Reinit()
            {
                Magnitude = 0;
                Orientation = 0;
                SmoothedMag = 0;
            }

            public override string ToString()
            {
                return "Cell: Magnitude = " + Magnitude + ", Orientation = " + Orientation + ", SmoothedMag = " +
                       SmoothedMag;
            }

        }

        // TODO: make a single method for both RBG and D (without terrible performence ideally)
        public void ComputeCells(short[,] depthImage, int w, int h)
        {
            // Handle invalid call
            if (depthImage == null)
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

            int y0 = 1; // x0,y0 is the top left corner pixel of the cell, x1,y1 is the bottom right corner
            int y1 = y0 + PixelsPerCell;
            //List<String> output = new List<string>();
            Parallel.For(0, cellArrayH, j => // Parallel.For Begin
                                        //for (int j = 0; j < cellArrayH; j++)
                                            {
                                                int x0 = 1;
                                                int x1 = x0 + PixelsPerCell;
                                                for (int i = 0; i < cellArrayW; i++)
                                                {
                                                    //output.Add("----------- Starting new cell computation [" + i + "," + j + "] -----------" + Environment.NewLine);
                                                    _cells[j, i].Reinit();
                                                    long dx = 0;
                                                    double dy = 0;
                                                    for (int y = y0; y < y1; y++)
                                                        for (int x = x0; x < x1; x++)
                                                        {
                                                            //output.Add("pixel at " + x + "," + y + " has neihborgs:" + Environment.NewLine);
                                                            //output.Add("     Left:" + GetPixelAt(rgbImage, x + 1, y, w) + Environment.NewLine);
                                                            //output.Add("     Right:" + GetPixelAt(rgbImage, x - 1, y, w) + Environment.NewLine);
                                                            //output.Add("     Top:" + GetPixelAt(rgbImage, x, y + 1, w) + Environment.NewLine);
                                                            //output.Add("     Bottom:" + GetPixelAt(rgbImage, x, y - 1, w) + Environment.NewLine);
                                                            dx += depthImage[y, x + 1] - depthImage[y, x - 1];
                                                            dy += depthImage[y + 1, x] - depthImage[y - 1, x];
                                                            //output.Add("     Results: dx=" + (GetPixelAt(rgbImage, x + 1, y, w) - GetPixelAt(rgbImage, x - 1, y, w)) + Environment.NewLine);
                                                            //output.Add("              dy=" + (GetPixelAt(rgbImage, x, y + 1, w) - GetPixelAt(rgbImage, x, y - 1, w)) + Environment.NewLine);
                                                        }
                                                    double mag = Math.Sqrt(dx*dx + dy*dy);
                                                    double orien = dx != 0 ? Math.Atan(dy/dx) : Min;
                                                    _cells[j, i].SetCell(mag, orien);
                                                    //output.Add("CELL RESULTS: dx=" + dx + Environment.NewLine);
                                                    //output.Add("              dy=" + dy + Environment.NewLine);
                                                    //output.Add("              mag=" + mag + Environment.NewLine);
                                                    //output.Add("              Orientation=" + 180 * orien / Math.PI + Environment.NewLine);
                                                    //Console.WriteLine("before - " + _cells[j, i].ToString());
                                                    //_cells[j, i].FinalizeCell();

                                                    x0 += PixelsPerCell;
                                                    x1 += PixelsPerCell;
                                                }
                                                y0 += PixelsPerCell;
                                                y1 += PixelsPerCell;
                                            }); // Parallel.For End
            //System.IO.File.WriteAllLines("HogOutput.log", output);
            //output.Clear();

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
                            if (y != 0 || x != 0)
                            {
                                neighborMag += _cells[y, x].Magnitude;
                                sampleCount++;
                            }

                        }
                    }
                    _cells[j, i].SmoothedMag = (neighborMag + _cells[j, i].Magnitude)/(sampleCount + 1);
                        // average between neighborhood average magnitude and local magnitude
                }
            }
        }
    }
}
