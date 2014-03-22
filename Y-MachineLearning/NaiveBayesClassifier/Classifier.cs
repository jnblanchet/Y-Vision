using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace ProbabilityFunctions
{
    [Serializable()]
    public abstract class Classifier
    {
        public const String DefaultName = "Y-Vision.model";
        protected string pType = "abstract";
        public string type
        {
            get { return pType; }
            set { pType = value; }
        }

        protected DataSet dataSet = new DataSet();

        public DataSet DataSet
        {
            get { return dataSet; }
            set { dataSet = value; }
        }

        public Classifier(bool load = false)
        {
            if (load)
            {
                try
                {
                    using (Stream stream = File.Open(DefaultName, FileMode.Open))
                    {
                        var bin = new BinaryFormatter();

                        var loadedModel = (Classifier)bin.Deserialize(stream);

                        DataSet = loadedModel.DataSet;
                        type = loadedModel.type;
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Unable to load model: " + e.Message);
                }
            }
        }

        /// <summary>
        /// This method will attempt to serialize config. It may throw an exception!
        /// </summary>
        /// <param name="filename"></param>
        public void SaveConfig(string filename = DefaultName)
        {
            using (Stream stream = File.Open(filename, FileMode.Create))
            {
                var bin = new BinaryFormatter();
                bin.Serialize(stream, this);
            }
        }

        public int getNbAttributes()
        {
            if (dataSet.Tables.Count == 0)
            {
                return 0;
            }

            return dataSet.Tables[0].Columns.Count - 1;
        }

        public abstract void TrainClassifier(DataTable table);

        public abstract string Classify(double[] obj);

        #region Helper Function

        public IEnumerable<double> SelectRows(DataTable table, int column, string filter)
        {
            List<double> _doubleList = new List<double>();
            DataRow[] rows = table.Select(filter);
            for (int i = 0; i < rows.Length; i++)
            {
                _doubleList.Add((double)rows[i][column]);
            }

            return _doubleList;
        }

        public void Clear()
        {
            dataSet = new DataSet();
        }

        #endregion
    }
}
