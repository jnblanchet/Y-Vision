using System;

namespace Y_Vision.SensorStreams
{
    public interface IKinectStream
    {
        void Start();
        void Stop();
        event EventHandler<EventArgs> FramesReady;
    }

    public class FramesReadyEventArgs : EventArgs
    {
        private readonly short[] _depth;
        private readonly int _depthWidth, _depthHeight, _colorWidth, _colorHeight;
        private readonly byte[] _color;
        private short[,] _depth2DBuffer;

        public short[] DepthArray { get { return _depth; } }
        public int DepthWidth { get { return _depthWidth; } }
        public int DepthHeight { get { return _depthHeight; } }
        public byte[] ColorArray { get { return _color; } }
        public int ColorWidth { get { return _colorWidth; } }
        public int ColorHeight { get { return _colorHeight; } }

        public FramesReadyEventArgs(short[] depth, int depthH, int depthW, byte[] color, int colorH, int colorW)
        {
            _depth = depth;
            _depthHeight = depthH;
            _depthWidth = depthW;
            _color = color;
            _colorHeight = colorH;
            _colorWidth = colorW;
        }

        public short[,] DepthTo2DArray()
        {
            if (_depth2DBuffer == null || _depth2DBuffer.Length != _depth.Length)
                _depth2DBuffer = new short[_depthHeight, _depthWidth];

            int i = 0, j = 0;
            foreach (short d in _depth)
            {
                _depth2DBuffer[j, i] = d;
                if (++i == _depthWidth)
                {
                    i = 0;
                    j++;
                }
            }
            return _depth2DBuffer;
        }
    }
}