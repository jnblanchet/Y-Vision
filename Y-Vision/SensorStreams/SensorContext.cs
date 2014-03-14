namespace Y_Vision.SensorStreams
{
    public abstract class SensorContext
    {
        public const int MinDepthValue = 400;
        public const int MaxDepthValue = 4050;
        protected const int ConstDepthWidth = 320;
        protected const int ConstDepthHeight = 240;
        public double VerticalFieldOfViewRad;
        public double HorizontalFieldOfViewRad;
        public double VerticalFieldOfViewDeg;
        public double HorizontalFieldOfViewDeg;

        protected SensorContext(bool isDefaultSetup)
        {
            RotateFieldOfView(isDefaultSetup);
        }

        public int DepthWidth { get; protected set; }
        public int DepthHeight { get; protected set; }
        public abstract void RotateFieldOfView(bool isDefaultSetup);
    }
}