using System;

namespace Y_Vision.ConnectedComponentLabeling
{
    // This class finds connected components using a conectivity threshold
    public class ConnectedComponentLabling
    {
        private const short ForeGroundThreshold = 400;
        private const short BackGroundThreshold = 4090;
        private const short SingleBlobThreshold = 150;
        private const short UnknownLabel = 0;
        private const int MaxBlob = 1000;
        //private const short startX = 25;

        public struct Blob
        {
            public int X, Y, Z, Count;
            public int MinX, MaxX, MinY, MaxY;

            internal void Init()
            {
                MinX = int.MaxValue;
                MinY = int.MaxValue;

                MaxX = int.MinValue;
                MaxY = int.MinValue;
                X = Y = Z = Count = 0;
            }

            /// <summary>
            /// Do not call this method. It should only be called once!
            /// (no check is performed for optimization purposes)
            /// </summary>
            internal void FinalizeBlob()
            {
                if (Count > 1)
                {
                    // Average
                    X /= Count;
                    Y /= Count;
                    Z /= Count;

                    // StDev Approximation based on a normal distribution
                    //int stDev = (MaxX - MinX) / 6;
                    //MaxX -= 2*stDev + X;
                    //MaxX += 2*stDev + X;
                }
            }

            internal void AddPixel(int x, int y, int z)
            {
                X += x;
                Y += y;
                Z += z;
                Count++;

                if (x > MaxX)
                    MaxX = x;
                else if (x < MinX)
                    MinX = x;

                if (y > MaxY)
                    MaxY = y;
                else if (y < MinY)
                    MinY = y;

            }
        }

        public Blob[] BlobInfo { get; private set; }

        //private Blob[] _detectedObjects;

        public short[,] FindConnectedComponents (short[,] image)
        {
            int h = image.GetLength(0), w = image.GetLength(1);

            BlobInfo = new Blob[MaxBlob];
            for (int i = 0; i < BlobInfo.Length;i++ )
                BlobInfo[i].Init();

            //algorithm TwoPass(data)
            var linked = new DisjointSet(MaxBlob);

            //labels = structure with dimensions of data, initialized with the value of Background
            var label = new short[h,w];
            short nextLabel = 1;

            //First pass

            for(int j = 1; j < h-1; j++)
            {
                for (int i = 1; i < w - 1; i++)
                {
                    short currentValue = image[j, i];
                    // If data[row][column] is not Background
                    if (currentValue < BackGroundThreshold && currentValue > ForeGroundThreshold)
                    {
                        short neighborLeft = (label[j, i - 1] != UnknownLabel && Math.Abs(image[j, i - 1] - currentValue) < SingleBlobThreshold) ? label[j, i - 1] : UnknownLabel;
                        short neighborTop = (label[j - 1, i] != UnknownLabel && Math.Abs(image[j - 1, i] - currentValue) < SingleBlobThreshold) ? label[j - 1, i] : UnknownLabel;

                        // If neighbors is empty
                        if ((neighborLeft | neighborTop) == UnknownLabel)
                        {
                            label[j, i] = nextLabel++;
                            if (nextLabel == MaxBlob)
                                goto FrameTooNoisy;
                        }
                        else
                        {
                            if (neighborLeft != UnknownLabel)
                            {
                                label[j, i] = neighborLeft;
                                if (neighborTop != UnknownLabel)
                                {
                                    linked.Union(neighborLeft, neighborTop);
                                }
                            } else if (neighborTop != UnknownLabel)
                            {
                                label[j, i] = neighborTop;
                                if (neighborLeft != UnknownLabel)
                                {
                                    linked.Union(neighborLeft, neighborTop);
                                }
                            }
                        }
                    }
                    else
                    {
                        label[j, i] = UnknownLabel;
                    }
                }
            }

            FrameTooNoisy:

            //Second pass
            for (int j = 1; j < h-1; j++)
            {
                for (int i = 1; i < w - 1; i++)
                {
                    var currIndex = label[j, i];
                    // Rename merged index
                    if (currIndex != UnknownLabel && currIndex < MaxBlob)
                    {
                        var newIndex = (short)linked.Find(currIndex);
                        BlobInfo[newIndex].AddPixel(i,j,image[j,i]);
                        label[j, i] = newIndex;
                    }      
                }
            }

            // Finalize blobs
            for (int i = 0; i < BlobInfo.Length;i++)
                BlobInfo[i].FinalizeBlob();

            return label;
        }
    }
}