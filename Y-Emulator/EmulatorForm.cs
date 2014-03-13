using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Y_TcpClient;
using Y_API.DetectionAPI;
using Y_API.DetectionAPI.MessageObjects;

namespace Y_Emulator
{
    public partial class EmulatorForm : Form
    {

        private class Circle
        {
            static ulong idCount = 0;
            public ulong uniqueId;
            public Point center;
            public Point oldCenter;
            public Brush color;
            public int age;
            public float VelocityX = 0.0f;
            public float VelocityY = 0.0f;

            public Circle(Point center, Brush color)
            {
                this.center = center;
                this.color = color;
                uniqueId = idCount++;
                age = 0;
                oldCenter = center;
            }
        }

        /***** static objects (so) in the virtual scene *****/
        private const int so_screenOriginX = 50;
        private const int so_screenOriginY = 250;
        private const int so_screenWidth = 100;
        private const int so_screenDepth = 10; // purely aesthetics
        private const string so_screenLabel = "SCREEN";
        private const int so_Xaxis_X1 = 10;
        private const int so_Xaxis_Y1 = 10;
        private const int so_Xaxis_X2 = 10;
        private const int so_Xaxis_Y2 = 60;
        private const int so_Zaxis_X1 = 10;
        private const int so_Zaxis_Y1 = 10;
        private const int so_Zaxis_X2 = 60;
        private const int so_Zaxis_Y2 = 10;
        /****************************************************/
        private const int circleRadius = 10;
        private const int maxCircles = 10;
        private const int updateMs = 50;
        private readonly StreamClientSender _streamClientSender;
        private readonly StringProtocol _protocol;
        private List<Circle> circles; // the list of circles added by the user
        private int selectedIndex = -1; // the index of the currently selected circle in circles, -1 if none
        private System.Threading.Timer updateTimer;
        private Label screenLabel, xAxisLabel, zAxisLabel, coord0Label, coord1Label;

        // This delegate enables asynchronous calls for setting
        // the text property on a ListBox control.
        private delegate void SetTextCallback(string text);

        private Brush[] circleColors =
            {
                Brushes.Red,
                Brushes.Green,
                Brushes.Blue,
                Brushes.Brown,
                Brushes.Gray,
                Brushes.Cyan,
                Brushes.Magenta,
                Brushes.Yellow
            };
        private int currentColorIndex = 0;

        public EmulatorForm()
        {
            InitializeComponent();
            DoubleBuffered = true;

            _streamClientSender = new StreamClientSender();
            _protocol = new StringProtocol();
            circles = new List<Circle>();

            /********* Timer thread to do work periodically *********/
            // Create an event to signal the timeout count threshold in the 
            // timer callback.
            AutoResetEvent autoEvent = new AutoResetEvent(false);
            // Create an inferred delegate that invokes methods for the timer.
            TimerCallback tcb = this.Update;
            // Create a timer that signals the delegate to invoke  
            // CheckStatus after zero second, and every updateMs ms 
            // thereafter.
            updateTimer = new System.Threading.Timer(tcb, autoEvent, 0, updateMs);
            /********************************************************/

            /***** create labels for the static objects in the scene *****/
            
            // Screen label
            screenLabel = new Label();
            screenLabel.Parent = this;
            screenLabel.AutoSize = true;
            screenLabel.Text = so_screenLabel;
            Point newLocation = new Point();
            newLocation.X = so_screenOriginX - screenLabel.Width;
            newLocation.Y = so_screenOriginY + so_screenWidth / 2 - screenLabel.Height / 2;
            screenLabel.Location = newLocation;

            // Axis labels
            xAxisLabel = new Label();
            zAxisLabel = new Label();
            xAxisLabel.Parent = this;
            zAxisLabel.Parent = this;
            xAxisLabel.AutoSize = true;
            zAxisLabel.AutoSize = true;
            xAxisLabel.Text = "X";
            zAxisLabel.Text = "Z";
            newLocation.X = so_Xaxis_X2 - xAxisLabel.Width / 2;
            newLocation.Y = so_Xaxis_Y2 + 1;
            xAxisLabel.Location = newLocation;
            newLocation.X = so_Zaxis_X2 + 1;
            newLocation.Y = so_Zaxis_Y2 - zAxisLabel.Height / 2;
            zAxisLabel.Location = newLocation;

            // Coordinates
            coord0Label = new Label();
            coord1Label = new Label();
            coord0Label.Parent = this;
            coord1Label.Parent = this;
            coord0Label.AutoSize = true;
            coord1Label.AutoSize = true;
            coord0Label.Text = "(0, 0)";
            coord1Label.Text = "(1, 0)";
            newLocation.X = so_screenOriginX - coord0Label.Width;
            newLocation.Y = so_screenOriginY - coord0Label.Height / 2;
            coord0Label.Location = newLocation;
            newLocation.X = so_screenOriginX - coord1Label.Width;
            newLocation.Y = so_screenOriginY + so_screenWidth - coord1Label.Height / 2;
            coord1Label.Location = newLocation;

            /*************************************************************/

            draw();
        }

        private void AddLineThreadSafe(String line)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (EventListBox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(AddLine);
                try
                {
                    this.Invoke(d, new object[] { line });
                }
                catch (ObjectDisposedException)
                {
                    // can happen while closing
                    // do nothing except preventing the Exception to propagate
                }
            }
            else
            {
                AddLine(line);
            }
        }

        private void AddLine(String line)
        {
            int numberOfItems = EventListBox.ClientSize.Height / EventListBox.ItemHeight;
            if (EventListBox.Items.Count >= numberOfItems)
                EventListBox.Items.Clear();

            EventListBox.Items.Add(line);
        }

        // returns true if aPoint is inside a circle of radius circleRadius at position aCircleCenter
        private bool inCircle(Point aPoint, Point aCircleCenter)
        {
            double square_dist = Math.Pow((double)(aCircleCenter.X - aPoint.X), 2.0d) + Math.Pow((double)(aCircleCenter.Y - aPoint.Y), 2.0d);
            return square_dist <= Math.Pow((double)circleRadius, 2.0d);
        }

        // this will ensure that there is room for a new circle in circles
        private void makeRoomInCircles()
        {
            while (circles.Count >= maxCircles)
            {
                PersonLeft(circles[0]);
                circles.RemoveAt(0);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            // call super method
            base.OnPaint(e);

            // clear the virtual scene
            e.Graphics.Clear(Color.White);

            // draw circles in the received order
            foreach (Circle c in circles)
            {
                e.Graphics.FillEllipse(c.color, c.center.X - circleRadius, c.center.Y - circleRadius, circleRadius * 2, circleRadius * 2);
            }

            // draw the static objects on top of everything
            e.Graphics.FillRectangle(Brushes.Black, so_screenOriginX, so_screenOriginY, so_screenDepth, so_screenWidth);
            e.Graphics.DrawLine(Pens.Black, so_Xaxis_X1, so_Xaxis_Y1, so_Xaxis_X2, so_Xaxis_Y2);
            e.Graphics.DrawLine(Pens.Black, so_Xaxis_X2 - 5, so_Xaxis_Y2 - 5, so_Xaxis_X2, so_Xaxis_Y2); // arrow
            e.Graphics.DrawLine(Pens.Black, so_Xaxis_X2 + 5, so_Xaxis_Y2 - 5, so_Xaxis_X2, so_Xaxis_Y2); // arrow
            e.Graphics.DrawLine(Pens.Black, so_Zaxis_X1, so_Zaxis_Y1, so_Zaxis_X2, so_Zaxis_Y2);
            e.Graphics.DrawLine(Pens.Black, so_Zaxis_X2 - 5, so_Zaxis_Y2 - 5, so_Zaxis_X2, so_Zaxis_Y2); // arrow
            e.Graphics.DrawLine(Pens.Black, so_Zaxis_X2 - 5, so_Zaxis_Y2 + 5, so_Zaxis_X2, so_Zaxis_Y2); // arrow
        }

        // as of now this function only redirects
        // but it could be used for other processing
        private void draw()
        {
            Refresh();
        }

        // Update at set intervals
        private void Update(Object stateInfo)
        {
            AutoResetEvent autoEvent = (AutoResetEvent)stateInfo;

            foreach (Circle c in circles)
            {
                c.age += updateMs;
                if (c.center != c.oldCenter)
                {
                    // the circle moved since last update
                    // calculate velocity in px / frame (maybe it should be in px/ms...)
                    c.VelocityX = (float)(c.center.X - c.oldCenter.X);
                    c.VelocityY = (float)(c.center.Y - c.oldCenter.Y);
                    c.oldCenter = c.center;
                }
                else
                {
                    c.VelocityX = c.VelocityY = 0.0f;
                }
            }

            PersonUpdated(circles);
        }

        private void EmulatorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            updateTimer.Dispose();
            _streamClientSender.CloseConnection();
        }

        private void EmulatorForm_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void EmulatorForm_MouseDown(object sender, MouseEventArgs e)
        {
            Point newClick = new Point(e.X, e.Y);

            // check if we are on a circle, in reverse drawing order
            int index = circles.Count;
            foreach (Circle c in circles.Reverse<Circle>())
            {
                index--;
                if (inCircle(newClick, c.center))
                {
                    selectedIndex = index;
                    break;
                }
            }

            if (ModifierKeys == Keys.Control)
            {
                // We pressed Ctrl, delete mode
                if (selectedIndex >= 0) // a circle is selected
                {
                    // delete and redraw
                    PersonLeft(circles[selectedIndex]);
                    circles.RemoveAt(selectedIndex);
                    // make sure to unselect
                    selectedIndex = -1;
                    draw();
                }
            }
            else
            {
                if (selectedIndex == -1) // add a circle
                {
                    // get the right color
                    if (currentColorIndex >= circleColors.Length)
                    {
                        currentColorIndex = 0;
                    }
                    Brush newColor = circleColors[currentColorIndex];
                    currentColorIndex++;

                    makeRoomInCircles();
                    Circle newCircle = new Circle(newClick, newColor);
                    circles.Add(newCircle);
                    PersonEnter(newCircle);
                    selectedIndex = circles.Count - 1;
                    draw();
                }
            }
        }

        private void EmulatorForm_MouseUp(object sender, MouseEventArgs e)
        {
            // unselect
            selectedIndex = -1;
        }

        private void EmulatorForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedIndex >= 0) // a circle is selected
            {
                circles[selectedIndex].center = new Point(e.X, e.Y);
                draw();
            }
        }

        private void PersonEnter(Circle c)
        {
            
        }

        private void PersonLeft(Circle c)
        {

        }

        private void PersonUpdated(List<Circle> circles)
        {
            List<Person> persons = new List<Person>();
            foreach (Circle c in circles)
            {
                persons.Add(new EmulatedPerson(c.VelocityY / (float)so_screenWidth, 0.0f, c.VelocityX / (float)so_screenWidth, c.age, 0, 
                    c.uniqueId, (float)(c.center.Y - so_screenOriginY) / (float)so_screenWidth, 0.0f, (float)(c.center.X - so_screenOriginX) / (float)so_screenWidth));
            }

            var data = (persons.Count > 0) ?
                    _protocol.Encode(persons) :
                    _protocol.Encode(new[] { new EmptyFrameMessage() });

            if (_streamClientSender.SendAsync(data))
                AddLineThreadSafe("[connected] data encoded: " + data);
            else
                AddLineThreadSafe("[not connected] data encoded: " + data);
        }
    }
}
