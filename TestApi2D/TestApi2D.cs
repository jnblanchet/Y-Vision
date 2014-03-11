using System;
using System.Linq;
using System.Windows.Forms;
using Y_TcpServer;

namespace TestApi2D
{
    public partial class FormTestApi2D : Form
    {
        private readonly TcpHumanDetector _detector;
        public FormTestApi2D()
        {
            InitializeComponent();
            _detector = new TcpHumanDetector();

            _detector.PersonEnter += (sender, args) => AddLine(String.Format("PersonEnter: {0}", args.Person));
            _detector.PersonLeft += (sender, args) => AddLine(String.Format("PersonLeft: {0}", args.Person));
            _detector.AllPeopleUpdated += (sender, args) => this.Invoke((MethodInvoker)delegate
                                                                                      {
                                                                                          TrackedObjectsListBox.Items.Clear();
                                                                                          TrackedObjectsListBox.Items.Add("Tracked People:");

                                                                                          var ppl = _detector.DetectedPeople.Select(p => p.ToString());
                                                                                          int i = 1;
                                                                                          foreach (var s in ppl)
                                                                                          {
                                                                                              TrackedObjectsListBox.Items.Add("#" + i++ + ": " + s);
                                                                                          }
                                                                                      });
        }

        private void AddLine(String line)
        {
            this.Invoke((MethodInvoker)delegate
            {
                int numberOfItems = EventListBox.ClientSize.Height / EventListBox.ItemHeight;
                if (EventListBox.Items.Count >= numberOfItems)
                    EventListBox.Items.Clear();

                EventListBox.Items.Add(line);
            });

        }

        private void FormTestApi2DLoad(object sender, EventArgs e)
        {
            _detector.Start();
        }

        private void FormTestApi2DFormClosing(object sender, FormClosingEventArgs e)
        {
            _detector.Stop();
        }

        private void EventListBoxSelectedIndexChanged(object sender, EventArgs e)
        {
            EventListBox.Items.Clear();
        }
    }
}
