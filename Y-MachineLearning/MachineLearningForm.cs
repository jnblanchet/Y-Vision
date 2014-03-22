using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ProbabilityFunctions;
using System.Globalization;

namespace Y_MachineLearning
{
    public partial class MachineLearningForm : Form
    {
        private int currentStep = 1;
        private StreamReader trainingSetStream = null;
        private Dictionary<string, int> attributes;
        private int nbAttributes;
        private DataTable table;
        private NumberFormatInfo provider;

        public MachineLearningForm()
        {
            InitializeComponent();
        }

        private void updateStep(int step)
        {
            firstLineAttributesCB.Enabled = false;
            classColumnCB.Enabled = false;
            IDColumnCB.Enabled = false;
            learnBtn.Enabled = false;
            testLineBtn.Enabled = false;
            testLineLB.Enabled = false;

            if (step >= 2)
            {
                firstLineAttributesCB.Enabled = true;
                classColumnCB.Enabled = true;
                IDColumnCB.Enabled = true;
                learnBtn.Enabled = true;
            }
            if (step >= 3)
            {
                testLineBtn.Enabled = true;
                testLineLB.Enabled = true;
            }
            currentStep = step;
        }

        private void updateTrainingSetModifiers()
        {
            string firstLine = trainingSetPreviewTB.Text.Substring(0, trainingSetPreviewTB.Text.IndexOf('\n') - 1);
            string[] firstLineSplit = firstLine.Split(',');
            attributes = new Dictionary<string, int>(firstLineSplit.Length + 1);

            classColumnCB.Items.Clear();
            IDColumnCB.Items.Clear();
            IDColumnCB.Items.Add("None");
            attributes.Add("None", -1);

            int index = -1;
            foreach (string atr in firstLineSplit)
            {
                index++;

                string atrName = "";
                if (firstLineAttributesCB.Checked)
                {
                    atrName = atr;
                }
                else
                {
                    atrName = "Attribute " + index;
                }

                attributes.Add(atrName, index);
                classColumnCB.Items.Add(atrName);
                IDColumnCB.Items.Add(atrName);
            }

            classColumnCB.SelectedIndex = 0;
            IDColumnCB.SelectedIndex = 0;
        }

        private void updateTests()
        {
            testLineLB.Items.Clear();
            string text = trainingSetPreviewTB.Text.Replace("\r", "");
            string[] lines = text.Split('\n');
            for (int i = (firstLineAttributesCB.Checked ? 1 : 0); i < lines.Length; i++)
            {
                testLineLB.Items.Add(lines[i]);
            }
            testLineLB.SelectedIndex = 0;

            classificationTB.Text = "";
        }








        private void MachineLearningForm_Load(object sender, EventArgs e)
        {
            // We need this for french settings
            provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            classifierCB.SelectedIndex = 0;
        }

        private void trainingSetBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            if (file.ShowDialog() == DialogResult.OK)
            {
                trainingSetTB.Text = file.FileName;
                try
                {
                    using (trainingSetStream = new StreamReader(file.FileName))
                    {
                        trainingSetPreviewTB.Text = trainingSetStream.ReadToEnd();
                        if (trainingSetPreviewTB.Text.IndexOf('\n') < 0)
                        {
                            MessageBox.Show("Error: Training set should have multiple lines.");
                        }
                        else
                        {
                            updateStep(2);
                            updateTrainingSetModifiers();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void firstLineAttributesCB_CheckedChanged(object sender, EventArgs e)
        {
            updateStep(2);
            updateTrainingSetModifiers();
        }

        private void classColumnCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateStep(2);
        }

        private void IDColumnCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateStep(2);
        }

        private void learnBtn_Click(object sender, EventArgs e)
        {
            updateStep(2);

            Classifier classifier;

            if (classifierCB.Text == "NaiveBayes")
            {
                classifier = new NaiveBayesClassifier();
            }
            else
            {
                MessageBox.Show("Error: you must select a valid classifier");
                return;
            }

            table = new DataTable();

            // add the class column
            table.Columns.Add(classColumnCB.Text);

            // add the other columns
            foreach (KeyValuePair<string, int> atr in attributes)
            {
                if (atr.Key != classColumnCB.Text && atr.Key != IDColumnCB.Text && atr.Value >= 0)
                {
                    table.Columns.Add(atr.Key, typeof(double));
                }
            }

            nbAttributes = table.Columns.Count;

            // for each line...
            string text = trainingSetPreviewTB.Text.Replace("\r", "");
            string[] lines = text.Split('\n');
            for (int i = (firstLineAttributesCB.Checked ? 1 : 0); i < lines.Length; i++ )
            {
                string[] values = lines[i].Split(',');
                object[] row = new object[nbAttributes];
                row[0] = values[attributes[classColumnCB.Text]]; // value for the class
                int index = 0;
                foreach (KeyValuePair<string, int> atr in attributes) // values for the other columns
                {
                    if (atr.Key != classColumnCB.Text && atr.Key != IDColumnCB.Text && atr.Value >= 0)
                    {
                        index++;
                        string toConvert = values[atr.Value];
                        row[index] = Convert.ToDouble(toConvert, provider);
                    }
                }

                // add the row to the table
                table.Rows.Add(row);
            }

            classifier.TrainClassifier(table);
            classifier.SaveConfig();

            updateStep(3);
            updateTests();

            //Classifier classifier2 = new NaiveBayesClassifier(true);
            //MessageBox.Show(classifier2.Classify(new double[] { 4, 150, 12 }));
        }

        private void testLineBtn_Click(object sender, EventArgs e)
        {
            Classifier classifier = new NaiveBayesClassifier(true);

            string line = testLineLB.SelectedItem.ToString();
            string[] values = line.Split(',');

            double[] row = new double[nbAttributes - 1];
            int index = -1;
            foreach (KeyValuePair<string, int> atr in attributes) // values for each attribute
            {
                if (atr.Key != classColumnCB.Text && atr.Key != IDColumnCB.Text && atr.Value >= 0)
                {
                    index++;
                    string toConvert = values[atr.Value];
                    row[index] = Convert.ToDouble(toConvert, provider);
                }
            }

            // classify
            classificationTB.Text = classifier.Classify(row);
        }
    }
}
