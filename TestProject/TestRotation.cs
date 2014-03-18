using Microsoft.VisualStudio.TestTools.UnitTesting;
using Y_Vision.SensorStreams;

namespace YUnitTests
{
    [TestClass]
    public class TestRotation
    {
        [TestMethod]
        public void TestRotationOrigin()
        {
            var rotation = DiscreteRotation.Cw90;

            var p = rotation.Rotate(0, 0, 1, 1);
            Assert.AreEqual(1, p.X);
            Assert.AreEqual(0, p.Y);
        }

        [TestMethod]
        public void TestRotation1()
        {
            var rotation = DiscreteRotation.Cw90;

            var p = rotation.Rotate(1, 0, 1, 1);
            Assert.AreEqual(1, p.X);
            Assert.AreEqual(1, p.Y);
        }

        [TestMethod]
        public void TestRotation2()
        {
            var rotation = DiscreteRotation.Cw90;

            var p = rotation.Rotate(1, 1, 1, 1);
            Assert.AreEqual(0, p.X);
            Assert.AreEqual(1, p.Y);
        }

        [TestMethod]
        public void TestRotation3()
        {
            var rotation = DiscreteRotation.Cw90;

            var p = rotation.Rotate(0, 1, 1, 1);
            Assert.AreEqual(0, p.X);
            Assert.AreEqual(0, p.Y);
        }

    }
}
