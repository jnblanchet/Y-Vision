using Microsoft.VisualStudio.TestTools.UnitTesting;
using Y_Vision.ConnectedComponentLabeling;

namespace YUnitTests
{
    [TestClass]
    public class CclTest
    {
        ConnectedComponentLabling _ccl = new ConnectedComponentLabling();
        private short[,] _image;
        private short[,] _expected;
        public CclTest()
        {
            _image = new short[,]
                        {
                            {0, 0, 0, 0},
                            {0, 0, 4000, 0},
                            {0, 4000, 4000, 0},
                            {0, 0, 0, 0}
                        };

            _expected = new short[,]
                        {
                            {0, 0, 0, 0},
                            {0, 0, 1, 0},
                            {0, 1, 1, 0},
                            {0, 0, 0, 0}
                        };
        }

        [TestMethod]
        public void TestCcl1()
        {
            var c = new ConnectedComponentLabling();
            var result = c.FindConnectedComponents(_image);
            Assert.IsTrue(result[1, 2] == 1, "1,2 incorrect");
            Assert.IsTrue(result[2, 1] == 1, "2,1 incorrect");
            Assert.IsTrue(result[2, 2] == 1, "2,2 incorrect");
            Assert.IsTrue(result[0, 0] == 0, "0,0 incorrect");
        }
    }
}
