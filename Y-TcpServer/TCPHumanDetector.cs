using System;
using System.Collections.Generic;
using System.Linq;
using Y_API.DetectionAPI;
using Y_API.DetectionAPI.MessageObjects;

namespace Y_TcpServer
{
    public class TcpHumanDetector : HumanDetector
    {
        private readonly StringProtocol _stringProtocol;
        private readonly StreamServerListener _streamServerListener;
	
        public TcpHumanDetector()
        {
            _stringProtocol = new StringProtocol();
            _streamServerListener = new StreamServerListener();
        }

        private void StreamServerListenerOnNewStringRecieved(object sender, StreamServerListener.StringReadyEventArgs args)
        {
            var empty = _stringProtocol.Decode(args.NewString).OfType<EmptyFrameMessage>();
            if (empty.Count(e => true) > 0)
            {
                PersonUpdate(new List<Person>());
            }
            else
            {
                var people = _stringProtocol.Decode(args.NewString).OfType<Person>();
                PersonUpdate(people);
            }
        }

        public override void Start()
        {
            _streamServerListener.NewStringRecieved += StreamServerListenerOnNewStringRecieved;
            _streamServerListener.ConnectionClosed += StreamServerListenerOnConnectionClosed;
        }

        public override void Stop()
        {
            _streamServerListener.NewStringRecieved -= StreamServerListenerOnNewStringRecieved;
            _streamServerListener.ConnectionClosed -= StreamServerListenerOnConnectionClosed;
            _streamServerListener.CloseConnection();
        }

        private void StreamServerListenerOnConnectionClosed(object sender, EventArgs eventArgs)
        {
            // Empty list of detected people
            PersonUpdate(new List<Person>());
        }

        private void PersonUpdate(IEnumerable<Person> persons)
        {
            if (persons == null)
                return;
		
            var alreadyHandled = new List<Person>();
            foreach (var person in persons)
            {
                var result = DetectedPeople.Find(p => p.UniqueId == person.UniqueId);
                if(result == null)
                {
                    alreadyHandled.Add(person);
                    DetectedPeople.Add(person);
                    if (PersonEnter != null)
                        PersonEnter.Invoke(this, new PersonEventArgs(person));
                }
                else
                {
                    alreadyHandled.Add(result);
                    result.UpdateFrom(person);
                    if (PersonUpdated != null)
                        PersonUpdated.Invoke(this, new PersonEventArgs(result));
                }
			
            }
            var deleted = DetectedPeople.Except(alreadyHandled).ToList();
            foreach (var old in deleted)
            {
                DetectedPeople.Remove(old);
                if (PersonLeft != null)
                    PersonLeft.Invoke(this, new PersonEventArgs(old));
            }

            if (AllPeopleUpdated != null)
                AllPeopleUpdated.Invoke(this, new EventArgs());
        }
	
        public override event EventHandler<PersonEventArgs> PersonEnter;
        public override event EventHandler<PersonEventArgs> PersonLeft;
        public override event EventHandler<PersonEventArgs> PersonUpdated;
        public override event EventHandler<EventArgs> AllPeopleUpdated;

    }
}
