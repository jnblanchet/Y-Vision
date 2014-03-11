using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Y_Vision.Tracking
{
    public abstract class Tracker
    {
        protected readonly int MaxDistance;        // The maximum distance threshold between two objects for tracking See ComputeDistanceWith() method
        //protected const int MaxUntrackedAge = 5;    // The object will dispear after being untracked for this long SEE METHOD FOR DYNAMIC VALUE
        protected const int MinAge = 2;             // New objects youger than this age will not be returned

        protected List<TrackedObject> TrackedObjects;

        protected Tracker(int maxDistance = 200000)
        {
            MaxDistance = maxDistance;
            TrackedObjects = new List<TrackedObject>();
        }

        public List<TrackedObject> TrackObjects(IEnumerable<TrackableObject> newObjects)
        {
            // new Tracking frame: increment age of all old frames
            TrackedObjects.ForEach(o => o.PrepareForNewTrackingFrame());

            // foregound stuff is more important: orderby distance
            newObjects = newObjects.OrderBy(o => o.Z);

            // Execute custom tracking logic
            GeneratedMatchesAndUpdateTrackedObjects(newObjects);

            for (int i = TrackedObjects.Count - 1; i >= 0; i--)
            {
                var obj = TrackedObjects[i];
                if (obj.LastSeen >= 1)
                {
                    obj.UpdateUntrackedFrame();
                    if (obj.LastSeen >= MaxUntrackedAge(obj.Age))
                        TrackedObjects.RemoveAt(i);
                }
            }
            // Filter to keep old elements only: very new elements are still unreliable
            return TrackedObjects.FindAll(o => o.Age > MinAge);
        }

        public abstract void GeneratedMatchesAndUpdateTrackedObjects(IEnumerable<TrackableObject> newObjects);

        private int MaxUntrackedAge(int age)
        {
            return Math.Min(30, age/3);
        }
    }

}
