using System.Diagnostics;

using Echo.Objects;

namespace Echo.Ships
{
	[DebuggerDisplay("Target: {Target}")]
	public class ShipTask
	{
		private int index;

		public ShipTask(ILocation target, ShipAction action)
		{
			Target = target;
			Action = action;
		}

		public ILocation Target { get; set; }
		public ShipAction Action { get; set; }
		public ShipTask NextTask { get; set; }

		public ShipTask Tail
		{
			get
			{
				ShipTask tail = this;

				while (tail.NextTask != null)
				{
					if (tail.NextTask.index == this.index)
						return tail;

					tail = tail.NextTask;
				}

				return tail;
			}
		}

		public ShipTask Join(ShipTask args)
		{
			Tail.NextTask = args;
			args.index = Tail.index + 1;

			return this;
		}

		public ShipTask Join(ILocation target, ShipAction action)
		{
			return Join(new ShipTask(target, action));
		}
	}
}