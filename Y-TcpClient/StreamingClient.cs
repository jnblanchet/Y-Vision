using System;
using System.Windows.Forms;
using Y_API.DetectionAPI;
using Y_API.DetectionAPI.MessageObjects;
using Y_Vision.DetectionAPI;

namespace Y_TcpClient
{
    public partial class StreamingClient : Form
    {
        private StreamClientSender _streamClientSender;
        private StringProtocol _protocol;
        private HumanDetector _detector;


        public StreamingClient()
        {
            InitializeComponent();
        }


        private void AddLine(String line)
        {
            int numberOfItems = EventListBox.ClientSize.Height / EventListBox.ItemHeight;
            if (EventListBox.Items.Count >= numberOfItems)
                EventListBox.Items.Clear();

            EventListBox.Items.Add(line);
        }

        private void StopEverything()
        {            
            if(_detector != null)
                _detector.Stop();
            if(_streamClientSender!= null)
                _streamClientSender.CloseConnection();
        }

        private void StreamingClientFormClosing(object sender, FormClosingEventArgs e)
        {
            _detector.Stop();
            _streamClientSender.CloseConnection();
        }

        private void TwoDSingleSensorToolStripMenuItemClick(object sender, EventArgs e)
        {
            StopEverything();
            _streamClientSender = new StreamClientSender();
            _protocol = new StringProtocol();
            _detector = new SingleSensor2DHumanDetector();

            _detector.AllPeopleUpdated += DetectorOnAllPeopleUpdated;

            _detector.Start();
        }

        private void DetectorOnAllPeopleUpdated(object sender, EventArgs eventArgs)
        {

                var data = (_detector.DetectedPeople.Count > 0) ?
                    _protocol.Encode(_detector.DetectedPeople) :
                    _protocol.Encode(new[] { new EmptyFrameMessage() });

                if (_streamClientSender.SendAsync(data))
                    AddLine("[connected] data encoded: " + data);
                else
                    AddLine("[not connected] data encoded: " + data);
        }

        private void ThreeDParallaxToolStripMenuItemClick(object sender, EventArgs e)
        {
            StopEverything();
            _streamClientSender = new StreamClientSender();
            _protocol = new StringProtocol();
            _detector = new ParallaxHumanDetector(null);

            _detector.AllPeopleUpdated += DetectorOnAllPeopleUpdated;

            _detector.Start();
        }

    }
}
