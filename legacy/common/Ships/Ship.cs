using System;
using System.Collections.Generic;
using System.Diagnostics;

using Echo.Entities;
using Echo.Events;
using Echo.Objects;
using Echo.Vectors;

namespace Echo.Ships
{
	public delegate bool ShipAction(Ship ship, ILocation target);

	[DebuggerDisplay("{Name}, Position: {Position}")]
	public class Ship : BaseLocation, IItem
	{
		private ItemCollection<IItem> cargo;
		private double cargoHoldSize;
		private Vector? destination;
		private Corporation owner;
		private Agent pilot;

		private ILocation target;
		private ShipAction targetAction;
		private ShipTask task;
		private List<HardPoint> hardPoints;
		private ShipStatistics stats;

		private Ship()
		{
			Speed = 1d;
			this.hardPoints = new List<HardPoint>();
			this.stats = new ShipStatistics();
		}

		public Ship(Agent pilot) : this()
		{
			if (pilot == null)
				throw new ArgumentNullException("pilot");

			Owner = pilot.Employer;
			Pilot = pilot;
		}

		public Ship(Corporation owner) : this()
		{
			if (owner == null)
				throw new ArgumentNullException("owner");

			Owner = owner;
		}

		public Ship(ShipBlueprint blueprint) : this()
		{
			Owner = blueprint.Owner;
			SetHardPoints(blueprint.HardPoints);
			Speed = blueprint.Speed;

			stats = new ShipStatistics(blueprint.Stats);
		}

		public double Speed { get; set; }

		public Agent Pilot
		{
			get { return this.pilot; }
			set
			{
				if ( value != null )
				{
					if (value.Employer != Owner)
						throw new InvalidOperationException("This agent is unable to pilot this ship because it is owned by another corporation.");

					value.Location = this;
				}

				this.pilot = value;
			}
		}

		public List<Agent> Passengers { get; set; }

		public ILocation Target
		{
			get { return this.target; }
		}

		/// <summary>Current destination for the ship.  If the ship is following a target, this property is the target's position</summary>
		public Vector? Destination
		{
			get { return (this.target == null) ? this.destination : this.target.UniversalCoordinates; }
			set
			{
				this.destination = value;
				if (value != null)
				{
					Universe.EventPump.RaiseEvent(this, EventType.ShipNewDestination, "Moving towards {0} |{1}|", Destination, DistanceToDestination);
				}

				ClearCurrentTask();
			}
		}

		/// <summary>
		/// Distance to ships destination.  Returns 0 if object is stationary
		/// </summary>
		public double DistanceToDestination
		{
			get { return (Destination == null) ? 0d : (Destination.Value - UniversalCoordinates).Magnitude; }
		}

		/// <summary>Size of this ships cargo hold</summary>
		public double CargoHoldSize
		{
			get { return this.cargoHoldSize; }
			set
			{
				this.cargoHoldSize = value;

				CargoHoldRemaining = value;
				this.cargo.ForEach(item => CargoHoldRemaining -= item.CargoSpace);
			}
		}

		/// <summary>Amount of cargo hold remaining</summary>
		public double CargoHoldRemaining { get; private set; }

		/// <summary>Contents of the ships cargo hold</summary>
		public IReadOnlyList<IItem> Cargo
		{
			get { return this.cargo; }
		}

		public bool IsIdle
		{
			get { return this.targetAction == null; }
		}

		protected override string SystematicNamePrefix
		{
			get { return "SHP"; }
		}

		#region IItem Members

		public override ObjectType ObjectType
		{
			get { return ObjectType.Ship; }
		}

		/// <summary>Amount of cargo space taken by this ship (when carried by another)</summary>
		public double CargoSpace { get; set; }

		uint IItem.Quantity
		{
			get { return 1; }
		}

		public int ItemID
		{
			get { return -1; }
		}

		bool IItem.Stackable
		{
			get { return false; }
		}

		void IItem.Stack(IItem item)
		{
			throw new NotSupportedException("Ships don't support stacking");
		}

		IItem IItem.Unstack(uint quantity)
		{
			throw new NotSupportedException("Ships don't support stacking");
		}

		public void Destroy()
		{
			Owner.ShipDestroyed(this);
		}

		public Corporation Owner
		{
			get { return this.owner; }
			set
			{
				if ( this.owner != null )
				{
					if (this.pilot != null)
						throw new InvalidOperationException("Cannot change ship ownership when the ship still has a pilot.");

					if (this.cargo.Count > 0)
						throw new InvalidOperationException("Cannot change ship ownership when the ship still has cargo");
			
					if ( this.owner.Assets.Contains(this) )
						throw new InvalidOperationException("The previous owner still holds this ship as an asset.");
				}

				this.owner = value;
				this.owner.AddAsset(this);
				this.cargo = new ItemCollection<IItem>();
			}
		}

		public bool HasDestination
		{
			get { return this.destination.HasValue; }
		}

		public override void Tick(ulong tick)
		{
			if (Destination == null || Pilot == null)
				return;

			if (DistanceToDestination < Speed)
			{
				UniversalCoordinates = Destination.Value;
			}
			else
			{
				Vector delta = (Destination.Value - UniversalCoordinates).ToUnitVector().Scale(Speed);
				UniversalCoordinates += delta;

				Universe.EventPump.RaiseEvent(this, EventType.ShipPosition, "{0} |{1}|", UniversalCoordinates, DistanceToDestination);
			}

			if (UniversalCoordinates != Destination)
				return;

			if (this.target != null)
			{
				Universe.EventPump.RaiseEvent(this, EventType.ShipPosition, "Tracking {0}", target);

				bool actionComplete = this.targetAction(this, this.target);

				if (this.task != null)
				{
					if (actionComplete)
						GetNextTask();
				}
			}
			else
			{
				Universe.EventPump.RaiseEvent(this, EventType.ShipArrived, "Arrived at {0}", target);
				Destination = null;
			}
		}

		#endregion

		#region Ship configuration

		public void SetHardPoints(params HardPoint[] shipHardPoints)
		{
			this.hardPoints = new List<HardPoint>();
			foreach (var h in shipHardPoints)
				this.hardPoints.Add(h.Clone());
		}

		#endregion

		public void Jump(JumpGate gate)
		{
			if (UniversalCoordinates == gate.UniversalCoordinates)
				gate.Jump(this);
			else
			{
				this.destination = null;

				this.target = gate;
				this.targetAction = ((ship, shipTarget) =>
				                     {
				                     	gate.Jump(ship);
				                     	return false;
				                     });
			}
		}

		public void AddCargo(IItem item)
		{
			if (item.CargoSpace > CargoHoldRemaining)
				throw new InvalidOperationException("No room remains for this item");

			CargoHoldRemaining -= item.CargoSpace;

			item.Location = this;
			this.cargo.Add(item);
		}

		public IItem RemoveCargo(int itemID)
		{
			IItem item = this.cargo.Find(cargoItem => cargoItem.ItemID == itemID);

			if (item != null)
			{
				CargoHoldRemaining += item.CargoSpace;
				this.cargo.Remove(item);
			}

			return item;
		}

		public void SetTarget(ILocation newTarget, ShipAction action)
		{
			if (action == null)
				throw new ArgumentNullException("action");

			if (Pilot == null)
				throw new InvalidOperationException("Unable to set ship target without a pilot");

			this.target = newTarget;
			this.targetAction = action;
		}

		public void SetTask(ShipTask args)
		{
			if (Pilot == null)
				throw new InvalidOperationException("Unable to set ship task without a pilot");

			this.task = args;
			this.target = this.task.Target;
			this.targetAction = this.task.Action;
		}

		private void GetNextTask()
		{
			if (this.task == null || this.task.NextTask == null)
			{
				this.task = null;
				this.target = null;
				this.targetAction = null;
			}
			else
			{
				this.task = this.task.NextTask;

				this.target = this.task.Target;
				this.targetAction = this.task.Action;
			}
		}

		public void ClearCurrentTask()
		{
			this.task = null;
			this.target = null;
			this.targetAction = null;
		}

		public void ClearTarget()
		{
			this.target = null;
			this.targetAction = null;

			if (this.task != null)
				GetNextTask();
		}
	}
}