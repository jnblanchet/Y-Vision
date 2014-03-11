using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Y_Vision.Tracking;

namespace YUnitTests
{
    [TestClass]
    public class BranchAndBoundTest
    {
        private readonly BranchAndBound _bnb = new BranchAndBound(5);

        private void ArrayAssert(int[] result, int[] expected)
        {
            Assert.IsTrue(result.Length == expected.Length, "Invalid array size");

            for (int i = 0; i < result.Length; i++)
            {
                Assert.IsTrue(result[i] == expected[i],String.Format("Error, index {0} returned {1}, expected {2}",i,result[i],expected[i]));
            }
        }

        [TestMethod]
        public void TestSimpleMatch()
        {
            var input = new[,]
                        {
                          //|0 | 1|
                            {1, 10},// 0
                            {20, 2},// 1
                        };

            var expectedOutput = new[]{0,1}; // the first element is expected to be matched with the element at 0, and the second with the second one.

            var result = _bnb.Solve(input);
            ArrayAssert(result,expectedOutput);
        }

        [TestMethod]
        public void TestMax()
        {
            var input = new[,]
                        {
                          //|0 | 1|
                            {1, 10},// 0
                            {20, 30},// 1
                        };

            var expectedOutput = new[] { 0, BranchAndBound.NoMatchSolution }; // the first element is expected to be matched with the element at 0, and the second not matched at all.

            var result = _bnb.Solve(input);
            ArrayAssert(result, expectedOutput);
        }

        [TestMethod]
        public void TestMultipleUntracked()
        {
            var input = new[,]
                        {
                          //|0 | 1|
                            {30, 30},// 0
                            {30, 30},// 1
                        };

            var expectedOutput = new[] { BranchAndBound.NoMatchSolution, BranchAndBound.NoMatchSolution };
            var result = _bnb.Solve(input);
            ArrayAssert(result, expectedOutput);
        }

        [TestMethod]
        public void TestMixedOrder()
        {
            var input = new[,]
                        {
                          //|0 | 1|
                            {30, 1},// 0
                            {2, 30},// 1
                        };

            var expectedOutput = new[] { 1, 0 };

            var result = _bnb.Solve(input);
            ArrayAssert(result, expectedOutput);
        }

        [TestMethod]
        public void TestLargerArray()
        {
            var input = new[,]
                        {
                          //| 0| 1| 2| 3|
                            {9, 9, 9, 1},// 0
                            {1, 9, 9, 9},// 1
                            {9, 9, 1, 9},// 2
                            {9, 1, 9, 9},// 3
                        };

            var expectedOutput = new[] { 3, 0, 2, 1}; 

            var result = _bnb.Solve(input);
            ArrayAssert(result, expectedOutput);
        }

        [TestMethod]
        public void TestMixedValues()
        {
            var input = new[,]
                        {
                          //| 0| 1| 2| 3|
                            {8, 7, 5, 4},// 0
                            {4, 5, 6, 7},// 1
                            {7, 6, 4, 5},// 2
                            {7, 4, 6, 5},// 3
                        };

            var expectedOutput = new[] { 3, 0, 2, 1 };

            var result = _bnb.Solve(input);
            ArrayAssert(result, expectedOutput);
        }

        [TestMethod]
        public void TestMixedTrickyValues()
        {
            var input = new[,]
                        {
                          //| 0| 1| 2| 3|
                            {4, 3, 2, 1},// 0
                            {1, 2, 3, 4},// 1
                            {3, 2, 1, 2},// 2
                            {4, 1, 2, 3},// 3
                        };

            var expectedOutput = new[] { 3, 0, 2, 1 }; 

            var result = _bnb.Solve(input);
            ArrayAssert(result, expectedOutput);
        }

        [TestMethod]
        public void TestCollision()
        {
            var input = new[,]
                        {
                          //| 0| 1| 2| 3|
                            {1, 1, 4, 4},// 0
                            {4, 1, 1, 4},// 1
                            {4, 4, 1, 1},// 2
                            {4, 4, 4, 1},// 3
                        };

            var expectedOutput = new[] { 0, 1, 2, 3 };

            var result = _bnb.Solve(input);
            ArrayAssert(result, expectedOutput);
        }

        [TestMethod]
        public void TestCollisionWithUnmachted()
        {
            var input = new[,]
                        {
                          //| 0| 1| 2|
                            {1, 1, 4},// 0
                            {4, 1, 1},// 1
                            {4, 4, 1},// 2
                            {4, 4, 4},// 3
                        };

            var expectedOutput = new[] { 0, 1, 2, BranchAndBound.NoMatchSolution};

            var result = _bnb.Solve(input);
            ArrayAssert(result, expectedOutput);
        }

        [TestMethod]
        public void TestComplexeScenario()
        {
            var input = new[,]
                        {
                          //| 0| 1| 2| 3| 4| 5|
                            {1, 3, 3, 1, 5, 6},// 0
                            {6, 1, 2, 3, 4, 5},// 1
                            {6, 5, 2, 3, 2, 6},// 2
                            {0, 1, 2, 2, 2, 2},// 3
                        };

            var expectedOutput = new[] { 3, 1, 4, 0};

            var result = _bnb.Solve(input);
            ArrayAssert(result, expectedOutput);
        }

        [TestMethod]
        public void TestFirstCall()
        {
            var input = new int[,]
                        {
                            {},
                            {},
                        };

            var expectedOutput = new[] { BranchAndBound.NoMatchSolution, BranchAndBound.NoMatchSolution };

            var result = _bnb.Solve(input);
            ArrayAssert(result, expectedOutput);
        }
    }
}
