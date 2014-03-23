using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Y_Vision.Triangulation;

namespace YUnitTests
{
    [TestClass]
    public class TriangulationToolTest
    {
        [TestMethod]
        public void TestImpossiblePoints()
        {
            bool exception = false;
            try
            {
                var tt = new TriangulationTool(2);

                tt.AddTriangulationPoint(0, 40.0d, 158.0d, 1388.0d);
                tt.AddTriangulationPoint(0, 120.0d, 189.0d, 1515.0d);
                tt.AddTriangulationPoint(0, 240.0d, 145.0d, 2384.0d);

                tt.AddTriangulationPoint(1, 54.0d, 155.0d, 933.0d);
                tt.AddTriangulationPoint(1, 166.0d, 175.0d, 1515.0d);
                tt.AddTriangulationPoint(1, 261.0d, 126.0d, 2451.0d);
                double test = tt.GetSensorPosX(0);
            }
            catch (Exception)
            {
                exception = true;
            }
            Assert.AreEqual(true, exception, "Should throw and exception");
            
        }
    }
}
