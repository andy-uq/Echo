using Echo.Objects;

namespace Echo.Events
{
	public interface IEventListener
	{
		EventFilter Filter { get; }
		void WriteEvent(BaseObject sender, EventType eventType, string message);
	}
}