using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Y_Vision.GroundRemoval;
using Y_Vision.SensorStreams;

namespace YUnitTests
{
    [TestClass]
    public class CoordidateSystemConverterTest
    {
        readonly CoordinateSystemConverter _csc = new CoordinateSystemConverter(new KinectSensorContext(true));

        [TestMethod]
        public void TestNoRotation()
        {
            var xyzPoint = _csc.ToXyz(160, 120, 1, 240, 320);

            Assert.AreEqual(0, xyzPoint.X);
            Assert.AreEqual(0, xyzPoint.Y);
            Assert.AreEqual(1, xyzPoint.Z);
        }

        [TestMethod]
        public void TestRotationXPositive()
        {
            var xyzPoint = _csc.ToXyz(320, 120, 1, 240, 320);

            Assert.AreEqual(.477, xyzPoint.X,0.1);
            Assert.AreEqual(0, xyzPoint.Y);
            Assert.AreEqual(0.879, xyzPoint.Z,0.1);
        }

        [TestMethod]
        public void TestRotationXNegative()
        {
            var xyzPoint = _csc.ToXyz(0, 120, 1, 240, 320);

            Assert.AreEqual(-.477, xyzPoint.X, 0.1);
            Assert.AreEqual(0, xyzPoint.Y);
            Assert.AreEqual(0.879, xyzPoint.Z, 0.1);
        }

        [TestMethod]
        public void TestRotationYPositive()
        {
            var xyzPoint = _csc.ToXyz(160, 240, 1, 240, 320);

            Assert.AreEqual(0, xyzPoint.X);
            Assert.AreEqual(0.367, xyzPoint.Y, 0.1);
            Assert.AreEqual(0.93, xyzPoint.Z, 0.1);
        }

        [TestMethod]
        public void TestRotationYNegative()
        {
            var xyzPoint = _csc.ToXyz(160, 0, 1, 240, 320);

            Assert.AreEqual(0, xyzPoint.X);
            Assert.AreEqual(-0.367, xyzPoint.Y, 0.1);
            Assert.AreEqual(0.93, xyzPoint.Z, 0.1);
        }

        [TestMethod]
        public void TestRotationXWithZ()
        {
            var xyzPoint = _csc.ToXyz(320, 120, 1, 240, 320);

            Assert.AreEqual(.477, xyzPoint.X, 0.1);
            Assert.AreEqual(0, xyzPoint.Y);
            Assert.AreEqual(0.879, xyzPoint.Z, 0.1);
        }
    }
}
