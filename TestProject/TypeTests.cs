using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YUnitTests
{
    [TestClass]
    public class TypeTests
    {
        [TestMethod]
        public void TestMethod()
        {
            Assert.AreEqual((int)-1.4d,-1);
        }
    }
}
