using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Y_Vision.Tracking
{
    class BranchAndBoundMatcher : BranchAndBoundTracker
    {
        public BranchAndBoundMatcher(int maxDistance) : base(maxDistance){}

        public override void GeneratedMatchesAndUpdateTrackedObjects(IEnumerable<TrackableObject> newObjects)
        {
            throw new Exception("You cannot call this method on the matcher type. Try using a BranchAndBoundTracker instead.");
        }

        public override List<TrackedObject> TrackObjects(IEnumerable<TrackableObject> newObjects)
        {
            throw new Exception("You cannot call this method on the matcher type. Try using a BranchAndBoundTracker instead.");
        }

        public IEnumerable<TrackableObject> GenerateMatches(IEnumerable<TrackedObject> firstSet, IEnumerable<TrackedObject> secondSet)
        {
            TrackedObjects.Clear();

            TrackedObjects.AddRange(firstSet.Select(o => o.ToTrackedObject()));
            base.GeneratedMatchesAndUpdateTrackedObjects(secondSet);

            return TrackedObjects;
        }
    }
}
