using System;
using System.Collections.Generic;
using System.Linq;

namespace Y_Vision.Tracking
{
    internal class BranchAndBoundTracker : Tracker
    {
        private readonly BranchAndBound _bnb;

        public BranchAndBoundTracker(): base() //TODO: maxdistance threshold here
        {
            _bnb = new BranchAndBound(MaxDistance);
        }

        public override void GeneratedMatchesAndUpdateTrackedObjects(IEnumerable<TrackableObject> newObjects)
        {
            var newObjs = newObjects.ToArray();
            var matches = _bnb.Solve(GenratedScores(newObjs));

            // Attempt to match every new object with an object being tracked
            for (int i = 0; i < newObjs.Count(); i++)
            {
                if (matches[i] == BranchAndBound.NoMatchSolution)
                {
                    // Create new object
                    TrackedObjects.Add(newObjs[i].ToTrackedObject());
                }
                else
                {
                    // Valid match: update tracking!
                    TrackedObjects.ElementAt(matches[i]).UpdateTrackedFrame(newObjs[i]);
                    Console.WriteLine("Object Tracked with a distance of " + TrackedObjects.ElementAt(matches[i]).ComputeDistanceWith(newObjs[i]));
                }
            }
        }

        private int[,] GenratedScores(TrackableObject[] newObjects)
        {
            var scores = new int[newObjects.Count(), TrackedObjects.Count];

            for (int j = 0; j < newObjects.Length; j++)
            {
                for (int i = 0; i < TrackedObjects.Count; i++)
                {
                    scores[j, i] = TrackedObjects.ElementAt(i).ComputeDistanceWith(newObjects.ElementAt(j));
                    Console.WriteLine("Computed Distance=" + scores[j, i]);
                }
            }
            return scores;
        }
    }
}
