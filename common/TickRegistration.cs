using System;
using System.Collections.Generic;

namespace Echo
{
	public delegate long TickMethod(TickContext tick);

	public class TickRegistration
	{
		public TickRegistration(TickMethod tick, long due = 0)
		{
			Tick = tick;
		}

		public long LastTick { get; set; }
		public long Due { get; set; }
		public TickMethod Tick { get; set; }
	}

	public class TickContext
	{
		private readonly List<TickRegistration> _registrations;
		private readonly long _tick;

		public TickContext(long tick)
		{
			_registrations = new List<TickRegistration>();
			_tick = tick;
		}

		public long ElapsedTicks { get; set; }
		public IEnumerable<TickRegistration> Registrations { get { return _registrations; } }

		public void Register(TickMethod tick, long due = 1)
		{
			_registrations.Add(new TickRegistration(tick, _tick + due));
		}

		public void Requeue(TickRegistration tick, long nextTick)
		{
			_registrations.Add(new TickRegistration(tick.Tick, _tick + nextTick) { LastTick = _tick });
		}
	}
}