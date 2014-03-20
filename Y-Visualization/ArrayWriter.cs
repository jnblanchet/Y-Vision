namespace Y_Visualization
{
    public class ArrayWriter
    {
        private bool _once = false;
        private readonly bool _onlyWriteOnce = false;

        public ArrayWriter(bool onlyWriteOnce = false)
        {
            _onlyWriteOnce = onlyWriteOnce;
        }
        public void ToTextFile(int[,] array, string fileName = "log.out")
        {
            int h = array.GetLength(0);
            int w = array.GetLength(1);
            if(!_onlyWriteOnce || !_once)
            {
                _once = true;
                var outStrings = new string[h];
                for (int j = 0; j < h; j++)
                {
                    outStrings[j] = "";
                    for (int i = 0; i < w; i++)
                    {
                        outStrings[j] += array[j, i] + ",";
                    }
                    outStrings[j] = outStrings[j].Substring(0, outStrings[j].Length - 1);
                }
                System.IO.File.WriteAllLines(fileName, outStrings);
            }
        }

        public void ToTextFileInverted(int[,] array, string fileName = "log.out")
        {
            int h = array.GetLength(0);
            int w = array.GetLength(1);
            if (!_onlyWriteOnce || !_once)
            {
                _once = true;
                var outStrings = new string[h];
                for (int i = 0; i < w; i++)
                {
                    outStrings[i] = "";
                    for (int j = 0; j < h; j++)
                    {
                        outStrings[i] += array[j, i] + ",";
                    }
                    outStrings[i] = outStrings[i].Substring(0, outStrings[i].Length - 1);
                }
                System.IO.File.WriteAllLines(fileName, outStrings);
            }
        }

        public void ToTextFile(int[] array, int h, int w)
        {
            if (!_onlyWriteOnce || !_once)
            {
                _once = true;
                var outStrings = new string[h];
                for (int j = 0; j < h; j++)
                {
                    outStrings[j] = "";
                    for (int i = 0; i < w; i++)
                    {
                        outStrings[j] += array[j * w + i] + ",";
                    }
                    outStrings[j] = outStrings[j].Substring(0, outStrings[j].Length - 1);
                }
                System.IO.File.WriteAllLines(@"out.log", outStrings);
            }

        }
    }
}
