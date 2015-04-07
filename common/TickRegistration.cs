using System;
using System.Collections.Generic;

namespace Echo
{
	public delegate long TickMethod(TickContext tick);

	public class TickRegistration
	{
		public TickRegistration(TickMethod ticketMethod, long due = 0)
		{
			TicketMethod = ticketMethod;
		}

		public long LastTick { get; set; }
		public long Due { get; set; }
		public TickMethod TicketMethod { get; set; }
	}

	public class TickContext
	{
		private readonly List<TickRegistration> _registrations;

		public TickContext(long tick)
		{
			_registrations = new List<TickRegistration>();
			Tick = tick;
		}

		public long Tick { get; private set; }
		public long ElapsedTicks { get; set; }
		public List<TickRegistration> Registrations { get { return _registrations; } }

		public void Register(TickRegistration registration)
		{
			_registrations.Add(registration);
		}
	}
}