using System.Collections.Generic;

using Echo.Objects;

namespace Echo.Events
{
	public class EventFilter
	{
		private List<ObjectType> excludeObjectTypes;
		private List<EventType> excludeEvents;
		private List<ulong> excludeObjects;

		public EventFilter()
		{
			this.excludeObjectTypes = new List<ObjectType>();
			this.excludeEvents = new List<EventType>();
			this.excludeObjects = new List<ulong>();
		}

		public List<ObjectType> ExcludeObjectTypes
		{
			get { return this.excludeObjectTypes; }
			set { this.excludeObjectTypes = value ?? new List<ObjectType>(); }
		}

		public List<EventType> ExcludeEvents
		{
			get { return this.excludeEvents; }
			set { this.excludeEvents = value ?? new List<EventType>(); }
		}

		public List<ulong> ExcludeObjects
		{
			get { return this.excludeObjects; }
			set { this.excludeObjects = value ?? new List<ulong>(); }
		}

		public virtual bool Filter(BaseObject @object, EventType eventType, string message)
		{
			if (this.excludeObjectTypes.Exists(o => @object.ObjectType == o))
				return false;

			if (this.excludeObjects.Exists(o => @object.ObjectID == o))
				return false;

			if (this.excludeEvents.Exists(e => e == eventType))
				return false;

			return true;
		}
	}
}