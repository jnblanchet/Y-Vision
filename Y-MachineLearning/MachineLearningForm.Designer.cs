namespace Y_MachineLearning
{
    partial class MachineLearningForm
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
            this.trainingSetPreviewTB = new System.Windows.Forms.TextBox();
            this.trainingSetTB = new System.Windows.Forms.TextBox();
            this.trainingSetBtn = new System.Windows.Forms.Button();
            this.classifierCB = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.firstLineAttributesCB = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.classColumnCB = new System.Windows.Forms.ComboBox();
            this.IDColumnCB = new System.Windows.Forms.ComboBox();
            this.learnBtn = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.testLineLB = new System.Windows.Forms.ListBox();
            this.testLineBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.classificationTB = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // trainingSetPreviewTB
            // 
            this.trainingSetPreviewTB.Location = new System.Drawing.Point(6, 19);
            this.trainingSetPreviewTB.Multiline = true;
            this.trainingSetPreviewTB.Name = "trainingSetPreviewTB";
            this.trainingSetPreviewTB.ReadOnly = true;
            this.trainingSetPreviewTB.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.trainingSetPreviewTB.Size = new System.Drawing.Size(533, 119);
            this.trainingSetPreviewTB.TabIndex = 3;
            // 
            // trainingSetTB
            // 
            this.trainingSetTB.Location = new System.Drawing.Point(149, 20);
            this.trainingSetTB.Name = "trainingSetTB";
            this.trainingSetTB.ReadOnly = true;
            this.trainingSetTB.Size = new System.Drawing.Size(311, 20);
            this.trainingSetTB.TabIndex = 1;
            // 
            // trainingSetBtn
            // 
            this.trainingSetBtn.Location = new System.Drawing.Point(466, 18);
            this.trainingSetBtn.Name = "trainingSetBtn";
            this.trainingSetBtn.Size = new System.Drawing.Size(75, 23);
            this.trainingSetBtn.TabIndex = 2;
            this.trainingSetBtn.Text = "Training Set";
            this.trainingSetBtn.UseVisualStyleBackColor = true;
            this.trainingSetBtn.Click += new System.EventHandler(this.trainingSetBtn_Click);
            // 
            // classifierCB
            // 
            this.classifierCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.classifierCB.FormattingEnabled = true;
            this.classifierCB.Items.AddRange(new object[] {
            "NaiveBayes"});
            this.classifierCB.Location = new System.Drawing.Point(6, 19);
            this.classifierCB.Name = "classifierCB";
            this.classifierCB.Size = new System.Drawing.Size(121, 21);
            this.classifierCB.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.classifierCB);
            this.groupBox1.Controls.Add(this.trainingSetTB);
            this.groupBox1.Controls.Add(this.trainingSetBtn);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(547, 55);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Step 1";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.learnBtn);
            this.groupBox2.Controls.Add(this.IDColumnCB);
            this.groupBox2.Controls.Add(this.classColumnCB);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.firstLineAttributesCB);
            this.groupBox2.Controls.Add(this.trainingSetPreviewTB);
            this.groupBox2.Location = new System.Drawing.Point(12, 74);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(545, 204);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Step 2";
            // 
            // firstLineAttributesCB
            // 
            this.firstLineAttributesCB.AutoSize = true;
            this.firstLineAttributesCB.Checked = true;
            this.firstLineAttributesCB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.firstLineAttributesCB.Enabled = false;
            this.firstLineAttributesCB.Location = new System.Drawing.Point(6, 145);
            this.firstLineAttributesCB.Name = "firstLineAttributesCB";
            this.firstLineAttributesCB.Size = new System.Drawing.Size(103, 17);
            this.firstLineAttributesCB.TabIndex = 4;
            this.firstLineAttributesCB.Text = "Tags on first line";
            this.firstLineAttributesCB.UseVisualStyleBackColor = true;
            this.firstLineAttributesCB.CheckedChanged += new System.EventHandler(this.firstLineAttributesCB_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(130, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Class column:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(130, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "ID column:";
            // 
            // classColumnCB
            // 
            this.classColumnCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.classColumnCB.Enabled = false;
            this.classColumnCB.FormattingEnabled = true;
            this.classColumnCB.Location = new System.Drawing.Point(209, 145);
            this.classColumnCB.Name = "classColumnCB";
            this.classColumnCB.Size = new System.Drawing.Size(142, 21);
            this.classColumnCB.TabIndex = 7;
            this.classColumnCB.SelectedIndexChanged += new System.EventHandler(this.classColumnCB_SelectedIndexChanged);
            // 
            // IDColumnCB
            // 
            this.IDColumnCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.IDColumnCB.Enabled = false;
            this.IDColumnCB.FormattingEnabled = true;
            this.IDColumnCB.Location = new System.Drawing.Point(209, 173);
            this.IDColumnCB.Name = "IDColumnCB";
            this.IDColumnCB.Size = new System.Drawing.Size(142, 21);
            this.IDColumnCB.TabIndex = 8;
            this.IDColumnCB.SelectedIndexChanged += new System.EventHandler(this.IDColumnCB_SelectedIndexChanged);
            // 
            // learnBtn
            // 
            this.learnBtn.Enabled = false;
            this.learnBtn.Location = new System.Drawing.Point(466, 171);
            this.learnBtn.Name = "learnBtn";
            this.learnBtn.Size = new System.Drawing.Size(73, 23);
            this.learnBtn.TabIndex = 9;
            this.learnBtn.Text = "Learn!";
            this.learnBtn.UseVisualStyleBackColor = true;
            this.learnBtn.Click += new System.EventHandler(this.learnBtn_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.classificationTB);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.testLineBtn);
            this.groupBox3.Controls.Add(this.testLineLB);
            this.groupBox3.Location = new System.Drawing.Point(12, 285);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(545, 163);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Step 3";
            // 
            // testLineLB
            // 
            this.testLineLB.Enabled = false;
            this.testLineLB.FormattingEnabled = true;
            this.testLineLB.Location = new System.Drawing.Point(7, 20);
            this.testLineLB.Name = "testLineLB";
            this.testLineLB.Size = new System.Drawing.Size(344, 134);
            this.testLineLB.TabIndex = 0;
            // 
            // testLineBtn
            // 
            this.testLineBtn.Enabled = false;
            this.testLineBtn.Location = new System.Drawing.Point(357, 20);
            this.testLineBtn.Name = "testLineBtn";
            this.testLineBtn.Size = new System.Drawing.Size(75, 23);
            this.testLineBtn.TabIndex = 1;
            this.testLineBtn.Text = "Test this line";
            this.testLineBtn.UseVisualStyleBackColor = true;
            this.testLineBtn.Click += new System.EventHandler(this.testLineBtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(358, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Classification:";
            // 
            // classificationTB
            // 
            this.classificationTB.Location = new System.Drawing.Point(361, 133);
            this.classificationTB.Name = "classificationTB";
            this.classificationTB.ReadOnly = true;
            this.classificationTB.Size = new System.Drawing.Size(180, 20);
            this.classificationTB.TabIndex = 3;
            // 
            // MachineLearningForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 453);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "MachineLearningForm";
            this.Text = "Machine Learning";
            this.Load += new System.EventHandler(this.MachineLearningForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox trainingSetPreviewTB;
        private System.Windows.Forms.TextBox trainingSetTB;
        private System.Windows.Forms.Button trainingSetBtn;
        private System.Windows.Forms.ComboBox classifierCB;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox firstLineAttributesCB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox classColumnCB;
        private System.Windows.Forms.ComboBox IDColumnCB;
        private System.Windows.Forms.Button learnBtn;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ListBox testLineLB;
        private System.Windows.Forms.Button testLineBtn;
        private System.Windows.Forms.TextBox classificationTB;
        private System.Windows.Forms.Label label3;
    }
}

