using System;
using System.Windows.Forms;
using Y_API.DetectionAPI;
using Y_API.DetectionAPI.MessageObjects;
using Y_Vision.DetectionAPI;

namespace Y_TcpClient
{
    public partial class StreamingClient : Form
    {
        private readonly StreamClientSender _streamClientSender;
        private readonly StringProtocol _protocol;
        private readonly SingleSensor2DHumanDetector _singleSensor2DHumanDetector;


        public StreamingClient()
        {
            InitializeComponent();

            _streamClientSender = new StreamClientSender();
            _protocol = new StringProtocol();
            _singleSensor2DHumanDetector = new SingleSensor2DHumanDetector();

            _singleSensor2DHumanDetector.AllPeopleUpdated += (sender, args) =>
            {
                
                var data = (_singleSensor2DHumanDetector.DetectedPeople.Count > 0) ? 
                    _protocol.Encode(_singleSensor2DHumanDetector.DetectedPeople) :
                    _protocol.Encode(new []{ new EmptyFrameMessage() });

                if(_streamClientSender.SendAsync(data))
                    AddLine("[connected] data encoded: " + data);
                else
                    AddLine("[not connected] data encoded: " + data);
            };

            _singleSensor2DHumanDetector.Start();

        }


        private void AddLine(String line)
        {
            int numberOfItems = EventListBox.ClientSize.Height / EventListBox.ItemHeight;
            if (EventListBox.Items.Count >= numberOfItems)
                EventListBox.Items.Clear();

            EventListBox.Items.Add(line);
        }

        private void StreamingClientFormClosing(object sender, FormClosingEventArgs e)
        {
            _singleSensor2DHumanDetector.Stop();
            _streamClientSender.CloseConnection();
        }

    }
}
