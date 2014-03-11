using System;
using System.Collections.Generic;
using Y_API.DetectionAPI.MessageObjects;

namespace Y_API.DetectionAPI
{
    public abstract class HumanDetector
    {
        protected HumanDetector()
        {
            DetectedPeople = new List<Person>();
        }

        public List<Person> DetectedPeople { protected set; get; }
        public abstract void Start();
        public abstract void Stop();
        public virtual event EventHandler<PersonEventArgs> PersonEnter;
        public virtual event EventHandler<PersonEventArgs> PersonLeft;
        public virtual event EventHandler<PersonEventArgs> PersonUpdated;
        public virtual event EventHandler<EventArgs> AllPeopleUpdated;
	
        public class PersonEventArgs : EventArgs
        {
            public Person Person { get; private set; }
		
            public PersonEventArgs(Person source)
            {
                Person = source;
            }
        }
    }
}