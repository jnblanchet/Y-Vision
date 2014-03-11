using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Y_Vision.Tracking
{
    public class BranchAndBound
    {
        public const int NoMatchSolution = -1; // The best solution for the element is to not match it at all
        private const int NoMatchFoundYet = -2; // Initial value (0 reserved for matching index 0, so something else is required)
        private const int InvalidScore = int.MaxValue; // The solution is invalid
        private readonly int _maximumScoreThreshold; // The error square distance upper bound;

        public BranchAndBound(int maximumThreshold = 50000)
        {
            _maximumScoreThreshold = maximumThreshold;
        }

        /// <summary>S
        /// Branch and Bound Solver by Jean-Nicola Blanchet.
        /// Finds a good appriximation of the best matches possible (lowest total match score)
        /// </summary>
        /// <param name="matchScores"></param>
        /// <returns>Returns a 2d list of matches e.g. (0,4) (1,1) (2,3) (3,2). Unmatched values are flagged with the NoMatchSolution const </returns>
        public int[] Solve(int[,] matchScores)
        {
            _bestSolution = null;
            _bestSolutionScore = int.MaxValue;

            var solution = new int[matchScores.GetLength(0)];

            for (int i = 0; i < matchScores.GetLength(0); i++)
                solution[i] = NoMatchFoundYet;

            RecursiveBranchAndBound(matchScores, 0, new bool[matchScores.GetLength(1)], solution, 0);
            return _bestSolution;
        }

        private int[] _bestSolution;
        private int _bestSolutionScore;

        private void RecursiveBranchAndBound(int[,] matchScores, int y, bool[] xBlackList, int[] solution, int currentScore)
        {
            // Exit condition: if the current level does not exists, we've reached the end!
            if(y == matchScores.GetLength(0))
            {
                if(currentScore <= _bestSolutionScore)
                {
                    _bestSolution = (int[]) solution.Clone();
                    _bestSolutionScore = currentScore;
                }
                return;
            }

            // Prepare solution approximations at the current level
            var length = matchScores.GetLength(1) + 1;
            var possibleSolutions = new int[length]; // the last value represents the no match option (worst case)
            possibleSolutions[length-1] = currentScore + _maximumScoreThreshold + ApproximateSolution(matchScores, y, xBlackList);
            // Generate approximated score for each decision at the current level.
            for (int i = 0; i < length - 1; i++)
            {
                if(!xBlackList[i])
                {
                    xBlackList[i] = true;
                    possibleSolutions[i] = currentScore + matchScores[y, i] + ApproximateSolution(matchScores, y, xBlackList);
                    xBlackList[i] = false;
                }
                else
                {
                    possibleSolutions[i] = InvalidScore;
                }
            }

            // For each possible solution
            for (int i = 0; i < length - 1; i++) // exclude the last solution (no match solution) for now
            {
                // If it looks better than the best solution so far
                if(possibleSolutions[i] != InvalidScore && possibleSolutions[i] <= _bestSolutionScore)
                {
                    // explore it!
                    xBlackList[i] = true;
                    solution[y] = i;
                    RecursiveBranchAndBound(matchScores, y + 1, xBlackList, solution, currentScore + matchScores[y, i]);
                    solution[y] = NoMatchFoundYet;
                    xBlackList[i] = false;
                }
            }
            // Also consider the scenario where the item is not matched
            if (possibleSolutions[length-1] <= _bestSolutionScore)
            {
                // explore it!
                solution[y] = NoMatchSolution;
                RecursiveBranchAndBound(matchScores, y + 1, xBlackList, solution, currentScore + _maximumScoreThreshold); // The score for a new object is the worst case bound
                solution[y] = NoMatchFoundYet;
            }
        }

        private int ApproximateSolution(int[,] matchScores, int y, bool[] xBlackList)
        {
            int approximatedScore = 0;
            for (int j = y+1; j < matchScores.GetLength(0)-1; j++)
            {
                int lowest = _maximumScoreThreshold;
                for (int i = 0; i < matchScores.GetLength(1) - 1; i++)
                {
                    if (!xBlackList[i])
                        lowest = Math.Min(lowest, matchScores[j, i]);
                }
                approximatedScore += lowest;
            }
            return approximatedScore;
        }
    }
}
