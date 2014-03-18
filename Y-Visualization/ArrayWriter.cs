namespace Y_Visualization
{
    public class ArrayWriter
    {
        private static bool _once = false;
        private static bool _onlyWriteOnce = false;

        public ArrayWriter(bool onlyWriteOnce = false)
        {
            _onlyWriteOnce = onlyWriteOnce;
        }
        public static void ToTextFile(short[,] array, int h, int w)
        {
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
                System.IO.File.WriteAllLines(@"~\", outStrings);
            }

        }

        public static void ToTextFile(short[] array, int h, int w)
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
                System.IO.File.WriteAllLines(@"~\", outStrings);
            }

        }
    }
}
