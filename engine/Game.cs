using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Echo.Engine
{
	public class Game
	{
		public const long TicksPerSlice = TimeSpan.TicksPerSecond/60;
		
		private List<TickRegistration> _updateFunctions;
		private long _tick;

		public Game()
		{
			Universe = new Universe();
			_updateFunctions = new List<TickRegistration>();
			_tick = 0;
		}

		public Universe Universe { get; private set; }

		public double Update()
		{
			_tick++;
			var context = new TickContext(_tick);

			var index = 0;
			long remaining = TicksPerSlice;

			var tickTimer = Stopwatch.StartNew();
			while (remaining > 0 && _updateFunctions.Count > index)
			{
				var peek = _updateFunctions[index];
				if (peek.Due > _tick)
				{
					break;
				}

				var nextTick = peek.TicketMethod(context);
				if (nextTick != 0)
				{
					peek.LastTick = _tick;
					peek.Due += nextTick;
					context.Register(peek);
				}

				remaining = TicksPerSlice - tickTimer.ElapsedTicks;
				index++;
			}

			_updateFunctions = context.Registrations;
			return (remaining/(double) TicksPerSlice);
		}

	}
}
