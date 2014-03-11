namespace Y_Visualization
{
    public static class ArrayWriter
    {
        private static bool _once = false;
        public static void ToTextFile(short[,] array, int h, int w)
        {
            if(!_once)
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
                System.IO.File.WriteAllLines(@"C:\Users\Propriétaire\Desktop\testMatlab\TestCsOut.txt", outStrings);
            }

        }

        public static void ToTextFile(short[] array, int h, int w)
        {
            if (!_once)
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
                System.IO.File.WriteAllLines(@"C:\Users\Propriétaire\Desktop\testMatlab\TestCsOut.txt", outStrings);
            }

        }
    }
}
