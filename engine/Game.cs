﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Echo.Engine
{
	public delegate IEnumerable<TickRegistration> TickRegistrationFactory(Universe universe);

	public class Game
	{
		public const long TicksPerSlice = TimeSpan.TicksPerSecond/60;
		
		private List<TickRegistration> _updateFunctions;
		private readonly IdleTimer _idle;
		private long _tick;

		public Game() : this(new Universe())
		{
		}

		public Game(Universe universe, IEnumerable<TickRegistrationFactory> tickRegistrationFactories)
		{
			Universe = universe;

			_updateFunctions = tickRegistrationFactories.SelectMany(f => f(universe)).ToList();
			_idle = new IdleTimer();
			_tick = 0;
		}

		public Game(Universe universe, params TickRegistrationFactory[] tickRegistrationFactories)
			: this(universe, (IEnumerable<TickRegistrationFactory>) tickRegistrationFactories)
		{
		}

		public Universe Universe { get; }
		public IdleTimer IdleTimer => _idle;
		public long Tick => _tick;

		public double Update()
		{
			_tick++;
			var context = new TickContext(_tick);

			var index = 0;
			var remaining = TicksPerSlice;

			var tickTimer = Stopwatch.StartNew();
			while (remaining > 0 && _updateFunctions.Count > index)
			{
				var tickMethod = _updateFunctions[index];
				if (tickMethod.Due > _tick)
				{
					break;
				}

				context.ElapsedTicks = _tick - tickMethod.Due;
				var nextTick = tickMethod.Tick(context);
				if (nextTick != 0)
				{
					context.Requeue(tickMethod, nextTick);
				}

				remaining = TicksPerSlice - tickTimer.ElapsedTicks;
				index++;
			}

			_updateFunctions = _updateFunctions
				.Skip(index)
				.Concat(context.Registrations)
				.OrderBy(x => x.Due)
				.ToList();

			_idle.Enqueue(remaining/(double) TicksPerSlice);
			return remaining;
		}
	}
}
