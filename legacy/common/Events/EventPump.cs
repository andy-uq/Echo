using System.Collections.Generic;

using Echo.Objects;

namespace Echo.Events
{
	public class EventPump
	{
		private readonly List<IEventListener> listeners;

		public EventPump()
		{
			this.listeners = new List<IEventListener>();
		}

		public void RemoveListener(IEventListener listener)
		{
			this.listeners.Remove(listener);
		}

		public void AddListener(IEventListener listener)
		{
			this.listeners.Add(listener);
		}

		public void RaiseEvent(BaseObject @object, EventType eventType, string message, params object[] args)
		{
			string formattedMessage = (args.Length == 0) ? message : string.Format(message, args);
			this.listeners.FindAll(l => l.Filter.Filter(@object, eventType, formattedMessage)).ForEach(l => l.WriteEvent(@object, eventType, formattedMessage));
		}
	}
}