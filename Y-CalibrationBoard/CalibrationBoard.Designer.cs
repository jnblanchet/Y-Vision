namespace Y_CalibrationBoard
{
    partial class CalibrationBoard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CalibrationBoard));
            this.tabControl = new System.Windows.Forms.TabControl();
            this.SensorSetupTab = new System.Windows.Forms.TabPage();
            this.setupRgbdViewer = new Y_Visualization.Drawing.RgbdViewer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.cbSensorId = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripFpsLabel = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.savetoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.rotatetoolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.GroundThresholdTextBox = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDropDownButtonMode = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripComboBoxTracking = new System.Windows.Forms.ToolStripComboBox();
            this.CalibrationTab = new System.Windows.Forms.TabPage();
            this.calibrationSplitContainer = new System.Windows.Forms.SplitContainer();
            this.comboBoxCalibrationRight = new System.Windows.Forms.ComboBox();
            this.comboBoxCalibrationLeft = new System.Windows.Forms.ComboBox();
            this.ParallaxContainer = new Y_Visualization.Drawing.RgbdViewer();
            this.VisionBoardTab = new System.Windows.Forms.TabPage();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.userSceneScale = new System.Windows.Forms.ToolStripComboBox();
            this.averageButton = new System.Windows.Forms.ToolStripButton();
            this.PrintPointsButton = new System.Windows.Forms.ToolStripButton();
            this.SetupPictureBox = new System.Windows.Forms.PictureBox();
            this.DisplayTabPage = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.numericUpDownE = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.numericUpDownD = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDownC = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numericUpDownB = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownA = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.distanceSetupPictureBox = new System.Windows.Forms.PictureBox();
            this.tabControl.SuspendLayout();
            this.SensorSetupTab.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.CalibrationTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.calibrationSplitContainer)).BeginInit();
            this.calibrationSplitContainer.Panel1.SuspendLayout();
            this.calibrationSplitContainer.Panel2.SuspendLayout();
            this.calibrationSplitContainer.SuspendLayout();
            this.VisionBoardTab.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SetupPictureBox)).BeginInit();
            this.DisplayTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.distanceSetupPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.SensorSetupTab);
            this.tabControl.Controls.Add(this.CalibrationTab);
            this.tabControl.Controls.Add(this.VisionBoardTab);
            this.tabControl.Controls.Add(this.DisplayTabPage);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(984, 462);
            this.tabControl.TabIndex = 1;
            this.tabControl.SelectedIndexChanged += new System.EventHandler(this.TabControlSelectedIndexChanged);
            // 
            // SensorSetupTab
            // 
            this.SensorSetupTab.Controls.Add(this.setupRgbdViewer);
            this.SensorSetupTab.Controls.Add(this.toolStrip1);
            this.SensorSetupTab.Location = new System.Drawing.Point(4, 22);
            this.SensorSetupTab.Name = "SensorSetupTab";
            this.SensorSetupTab.Padding = new System.Windows.Forms.Padding(3);
            this.SensorSetupTab.Size = new System.Drawing.Size(976, 436);
            this.SensorSetupTab.TabIndex = 0;
            this.SensorSetupTab.Text = "Sensor Setup";
            this.SensorSetupTab.UseVisualStyleBackColor = true;
            // 
            // setupRgbdViewer
            // 
            this.setupRgbdViewer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.setupRgbdViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.setupRgbdViewer.Location = new System.Drawing.Point(3, 28);
            this.setupRgbdViewer.Name = "setupRgbdViewer";
            this.setupRgbdViewer.Size = new System.Drawing.Size(970, 405);
            this.setupRgbdViewer.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cbSensorId,
            this.toolStripFpsLabel,
            this.toolStripSeparator1,
            this.savetoolStripButton,
            this.toolStripSeparator2,
            this.rotatetoolStripButton,
            this.toolStripSeparator3,
            this.toolStripLabel1,
            this.GroundThresholdTextBox,
            this.toolStripSeparator4,
            this.toolStripDropDownButtonMode,
            this.toolStripLabel2,
            this.toolStripComboBoxTracking});
            this.toolStrip1.Location = new System.Drawing.Point(3, 3);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(970, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // cbSensorId
            // 
            this.cbSensorId.AutoToolTip = true;
            this.cbSensorId.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSensorId.Name = "cbSensorId";
            this.cbSensorId.Size = new System.Drawing.Size(221, 25);
            this.cbSensorId.SelectedIndexChanged += new System.EventHandler(this.CbSensorIdSelectedIndexChanged);
            // 
            // toolStripFpsLabel
            // 
            this.toolStripFpsLabel.Name = "toolStripFpsLabel";
            this.toolStripFpsLabel.Size = new System.Drawing.Size(35, 22);
            this.toolStripFpsLabel.Text = "0 FPS";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // savetoolStripButton
            // 
            this.savetoolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.savetoolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("savetoolStripButton.Image")));
            this.savetoolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.savetoolStripButton.Name = "savetoolStripButton";
            this.savetoolStripButton.Size = new System.Drawing.Size(23, 22);
            this.savetoolStripButton.Text = "Save Config";
            this.savetoolStripButton.Click += new System.EventHandler(this.SavetoolStripButtonClick);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // rotatetoolStripButton
            // 
            this.rotatetoolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.rotatetoolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("rotatetoolStripButton.Image")));
            this.rotatetoolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.rotatetoolStripButton.Name = "rotatetoolStripButton";
            this.rotatetoolStripButton.Size = new System.Drawing.Size(23, 22);
            this.rotatetoolStripButton.Text = "rotate";
            this.rotatetoolStripButton.Click += new System.EventHandler(this.RotatetoolStripButtonClick);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(106, 22);
            this.toolStripLabel1.Text = "Ground Threshold:";
            // 
            // GroundThresholdTextBox
            // 
            this.GroundThresholdTextBox.Enabled = false;
            this.GroundThresholdTextBox.Name = "GroundThresholdTextBox";
            this.GroundThresholdTextBox.Size = new System.Drawing.Size(30, 25);
            this.GroundThresholdTextBox.ToolTipText = "The threshold for ground removal in mm";
            this.GroundThresholdTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.GroundThresholdTextBoxKeyPress);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripDropDownButtonMode
            // 
            this.toolStripDropDownButtonMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButtonMode.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2,
            this.toolStripMenuItem3});
            this.toolStripDropDownButtonMode.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButtonMode.Image")));
            this.toolStripDropDownButtonMode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButtonMode.Name = "toolStripDropDownButtonMode";
            this.toolStripDropDownButtonMode.Size = new System.Drawing.Size(29, 22);
            this.toolStripDropDownButtonMode.Text = "Depth Mode";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Checked = true;
            this.toolStripMenuItem2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItem2.Image = global::Y_CalibrationBoard.Properties.Resources.grayscale;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(127, 22);
            this.toolStripMenuItem2.Tag = "1";
            this.toolStripMenuItem2.Text = "Gray scale";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.SelectionModeClick);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Image = global::Y_CalibrationBoard.Properties.Resources.blob;
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(127, 22);
            this.toolStripMenuItem3.Tag = "2";
            this.toolStripMenuItem3.Text = "Blob";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.SelectionModeClick);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(48, 22);
            this.toolStripLabel2.Text = "Display:";
            // 
            // toolStripComboBoxTracking
            // 
            this.toolStripComboBoxTracking.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toolStripComboBoxTracking.Items.AddRange(new object[] {
            "None",
            "Blobs",
            "TrackedObjects"});
            this.toolStripComboBoxTracking.Name = "toolStripComboBoxTracking";
            this.toolStripComboBoxTracking.Size = new System.Drawing.Size(121, 25);
            // 
            // CalibrationTab
            // 
            this.CalibrationTab.Controls.Add(this.calibrationSplitContainer);
            this.CalibrationTab.Location = new System.Drawing.Point(4, 22);
            this.CalibrationTab.Name = "CalibrationTab";
            this.CalibrationTab.Padding = new System.Windows.Forms.Padding(3);
            this.CalibrationTab.Size = new System.Drawing.Size(976, 436);
            this.CalibrationTab.TabIndex = 1;
            this.CalibrationTab.Text = "Calibration";
            this.CalibrationTab.UseVisualStyleBackColor = true;
            // 
            // calibrationSplitContainer
            // 
            this.calibrationSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.calibrationSplitContainer.Location = new System.Drawing.Point(3, 3);
            this.calibrationSplitContainer.Name = "calibrationSplitContainer";
            this.calibrationSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // calibrationSplitContainer.Panel1
            // 
            this.calibrationSplitContainer.Panel1.Controls.Add(this.comboBoxCalibrationRight);
            this.calibrationSplitContainer.Panel1.Controls.Add(this.comboBoxCalibrationLeft);
            // 
            // calibrationSplitContainer.Panel2
            // 
            this.calibrationSplitContainer.Panel2.Controls.Add(this.ParallaxContainer);
            this.calibrationSplitContainer.Size = new System.Drawing.Size(970, 430);
            this.calibrationSplitContainer.SplitterDistance = 25;
            this.calibrationSplitContainer.TabIndex = 0;
            // 
            // comboBoxCalibrationRight
            // 
            this.comboBoxCalibrationRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.comboBoxCalibrationRight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCalibrationRight.FormattingEnabled = true;
            this.comboBoxCalibrationRight.Location = new System.Drawing.Point(733, 0);
            this.comboBoxCalibrationRight.Name = "comboBoxCalibrationRight";
            this.comboBoxCalibrationRight.Size = new System.Drawing.Size(237, 21);
            this.comboBoxCalibrationRight.TabIndex = 1;
            this.comboBoxCalibrationRight.SelectedIndexChanged += new System.EventHandler(this.ComboBoxCalibrationSelectedIndexChanged);
            // 
            // comboBoxCalibrationLeft
            // 
            this.comboBoxCalibrationLeft.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCalibrationLeft.FormattingEnabled = true;
            this.comboBoxCalibrationLeft.Location = new System.Drawing.Point(5, 3);
            this.comboBoxCalibrationLeft.Name = "comboBoxCalibrationLeft";
            this.comboBoxCalibrationLeft.Size = new System.Drawing.Size(237, 21);
            this.comboBoxCalibrationLeft.TabIndex = 0;
            this.comboBoxCalibrationLeft.SelectedIndexChanged += new System.EventHandler(this.ComboBoxCalibrationSelectedIndexChanged);
            // 
            // ParallaxContainer
            // 
            this.ParallaxContainer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ParallaxContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ParallaxContainer.Location = new System.Drawing.Point(0, 0);
            this.ParallaxContainer.Name = "ParallaxContainer";
            this.ParallaxContainer.Size = new System.Drawing.Size(970, 401);
            this.ParallaxContainer.TabIndex = 0;
            // 
            // VisionBoardTab
            // 
            this.VisionBoardTab.Controls.Add(this.toolStrip2);
            this.VisionBoardTab.Controls.Add(this.SetupPictureBox);
            this.VisionBoardTab.Location = new System.Drawing.Point(4, 22);
            this.VisionBoardTab.Name = "VisionBoardTab";
            this.VisionBoardTab.Size = new System.Drawing.Size(976, 436);
            this.VisionBoardTab.TabIndex = 2;
            this.VisionBoardTab.Text = "Vision Board";
            this.VisionBoardTab.UseVisualStyleBackColor = true;
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel3,
            this.userSceneScale,
            this.averageButton,
            this.PrintPointsButton});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(976, 25);
            this.toolStrip2.TabIndex = 1;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(37, 22);
            this.toolStripLabel3.Text = "Scale:";
            // 
            // userSceneScale
            // 
            this.userSceneScale.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.userSceneScale.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6"});
            this.userSceneScale.Name = "userSceneScale";
            this.userSceneScale.Size = new System.Drawing.Size(121, 25);
            this.userSceneScale.SelectedIndexChanged += new System.EventHandler(this.UserSceneScaleSelectedIndexChanged);
            // 
            // averageButton
            // 
            this.averageButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.averageButton.Image = ((System.Drawing.Image)(resources.GetObject("averageButton.Image")));
            this.averageButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.averageButton.Name = "averageButton";
            this.averageButton.Size = new System.Drawing.Size(23, 22);
            this.averageButton.Text = "Auto average points";
            this.averageButton.Click += new System.EventHandler(this.AverageButtonClick);
            // 
            // PrintPointsButton
            // 
            this.PrintPointsButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.PrintPointsButton.Image = ((System.Drawing.Image)(resources.GetObject("PrintPointsButton.Image")));
            this.PrintPointsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.PrintPointsButton.Name = "PrintPointsButton";
            this.PrintPointsButton.Size = new System.Drawing.Size(23, 22);
            this.PrintPointsButton.Text = "Print Points";
            this.PrintPointsButton.Click += new System.EventHandler(this.PrintPointsButtonClick);
            // 
            // SetupPictureBox
            // 
            this.SetupPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SetupPictureBox.Location = new System.Drawing.Point(0, 0);
            this.SetupPictureBox.Name = "SetupPictureBox";
            this.SetupPictureBox.Size = new System.Drawing.Size(976, 436);
            this.SetupPictureBox.TabIndex = 0;
            this.SetupPictureBox.TabStop = false;
            // 
            // DisplayTabPage
            // 
            this.DisplayTabPage.Controls.Add(this.label9);
            this.DisplayTabPage.Controls.Add(this.numericUpDownE);
            this.DisplayTabPage.Controls.Add(this.label10);
            this.DisplayTabPage.Controls.Add(this.label7);
            this.DisplayTabPage.Controls.Add(this.numericUpDownD);
            this.DisplayTabPage.Controls.Add(this.label8);
            this.DisplayTabPage.Controls.Add(this.label5);
            this.DisplayTabPage.Controls.Add(this.numericUpDownC);
            this.DisplayTabPage.Controls.Add(this.label6);
            this.DisplayTabPage.Controls.Add(this.label3);
            this.DisplayTabPage.Controls.Add(this.numericUpDownB);
            this.DisplayTabPage.Controls.Add(this.label4);
            this.DisplayTabPage.Controls.Add(this.label2);
            this.DisplayTabPage.Controls.Add(this.numericUpDownA);
            this.DisplayTabPage.Controls.Add(this.label1);
            this.DisplayTabPage.Controls.Add(this.distanceSetupPictureBox);
            this.DisplayTabPage.Location = new System.Drawing.Point(4, 22);
            this.DisplayTabPage.Name = "DisplayTabPage";
            this.DisplayTabPage.Size = new System.Drawing.Size(976, 436);
            this.DisplayTabPage.TabIndex = 3;
            this.DisplayTabPage.Text = "Display Calibration";
            this.DisplayTabPage.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(87, 112);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(23, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "mm";
            // 
            // numericUpDownE
            // 
            this.numericUpDownE.Location = new System.Drawing.Point(27, 106);
            this.numericUpDownE.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownE.Name = "numericUpDownE";
            this.numericUpDownE.Size = new System.Drawing.Size(59, 20);
            this.numericUpDownE.TabIndex = 14;
            this.numericUpDownE.ValueChanged += new System.EventHandler(this.NumericUpDownValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(5, 110);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(14, 13);
            this.label10.TabIndex = 13;
            this.label10.Text = "E";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(87, 86);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(23, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "mm";
            // 
            // numericUpDownD
            // 
            this.numericUpDownD.Location = new System.Drawing.Point(27, 80);
            this.numericUpDownD.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownD.Name = "numericUpDownD";
            this.numericUpDownD.Size = new System.Drawing.Size(59, 20);
            this.numericUpDownD.TabIndex = 11;
            this.numericUpDownD.ValueChanged += new System.EventHandler(this.NumericUpDownValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(5, 84);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(15, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "D";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(87, 60);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "mm";
            // 
            // numericUpDownC
            // 
            this.numericUpDownC.Location = new System.Drawing.Point(27, 54);
            this.numericUpDownC.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownC.Name = "numericUpDownC";
            this.numericUpDownC.Size = new System.Drawing.Size(59, 20);
            this.numericUpDownC.TabIndex = 8;
            this.numericUpDownC.ValueChanged += new System.EventHandler(this.NumericUpDownValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(14, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "C";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(87, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(23, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "mm";
            // 
            // numericUpDownB
            // 
            this.numericUpDownB.Location = new System.Drawing.Point(27, 28);
            this.numericUpDownB.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownB.Name = "numericUpDownB";
            this.numericUpDownB.Size = new System.Drawing.Size(59, 20);
            this.numericUpDownB.TabIndex = 5;
            this.numericUpDownB.ValueChanged += new System.EventHandler(this.NumericUpDownValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "B";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(87, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "mm";
            // 
            // numericUpDownA
            // 
            this.numericUpDownA.Location = new System.Drawing.Point(27, 2);
            this.numericUpDownA.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numericUpDownA.Name = "numericUpDownA";
            this.numericUpDownA.Size = new System.Drawing.Size(59, 20);
            this.numericUpDownA.TabIndex = 2;
            this.numericUpDownA.ValueChanged += new System.EventHandler(this.NumericUpDownValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "A";
            // 
            // distanceSetupPictureBox
            // 
            this.distanceSetupPictureBox.BackgroundImage = global::Y_CalibrationBoard.Properties.Resources.setup;
            this.distanceSetupPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.distanceSetupPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.distanceSetupPictureBox.InitialImage = null;
            this.distanceSetupPictureBox.Location = new System.Drawing.Point(0, 0);
            this.distanceSetupPictureBox.Name = "distanceSetupPictureBox";
            this.distanceSetupPictureBox.Size = new System.Drawing.Size(976, 436);
            this.distanceSetupPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.distanceSetupPictureBox.TabIndex = 0;
            this.distanceSetupPictureBox.TabStop = false;
            // 
            // CalibrationBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 462);
            this.Controls.Add(this.tabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CalibrationBoard";
            this.Text = "Y-CalibrationBoard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CalibrationBoardFormClosing);
            this.Load += new System.EventHandler(this.CalibrationBoardLoad);
            this.tabControl.ResumeLayout(false);
            this.SensorSetupTab.ResumeLayout(false);
            this.SensorSetupTab.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.CalibrationTab.ResumeLayout(false);
            this.calibrationSplitContainer.Panel1.ResumeLayout(false);
            this.calibrationSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.calibrationSplitContainer)).EndInit();
            this.calibrationSplitContainer.ResumeLayout(false);
            this.VisionBoardTab.ResumeLayout(false);
            this.VisionBoardTab.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SetupPictureBox)).EndInit();
            this.DisplayTabPage.ResumeLayout(false);
            this.DisplayTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.distanceSetupPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage SensorSetupTab;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox cbSensorId;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton rotatetoolStripButton;
        private System.Windows.Forms.TabPage VisionBoardTab;
        private Y_Visualization.Drawing.RgbdViewer setupRgbdViewer;
        private System.Windows.Forms.ToolStripLabel toolStripFpsLabel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton savetoolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButtonMode;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox GroundThresholdTextBox;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBoxTracking;
        private System.Windows.Forms.TabPage CalibrationTab;
        private System.Windows.Forms.SplitContainer calibrationSplitContainer;
        private System.Windows.Forms.ComboBox comboBoxCalibrationRight;
        private System.Windows.Forms.ComboBox comboBoxCalibrationLeft;
        private Y_Visualization.Drawing.RgbdViewer ParallaxContainer;
        private System.Windows.Forms.PictureBox SetupPictureBox;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripComboBox userSceneScale;
        private System.Windows.Forms.ToolStripButton averageButton;
        private System.Windows.Forms.ToolStripButton PrintPointsButton;
        private System.Windows.Forms.TabPage DisplayTabPage;
        private System.Windows.Forms.PictureBox distanceSetupPictureBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numericUpDownE;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numericUpDownD;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numericUpDownC;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numericUpDownB;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownA;
        private System.Windows.Forms.Label label1;
    }
}

