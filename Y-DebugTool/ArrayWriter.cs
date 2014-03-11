using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Y_DebugTool
{
    static class ArrayWriter
    {
        private static bool once = false;
        public static void ToTextFile(short[,] array, int h, int w)
        {
            if(!once)
            {
                once = true;
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
    }
}
