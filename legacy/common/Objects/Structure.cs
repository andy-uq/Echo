using System;
using System.Collections.Generic;

using Echo.Entities;
using Echo.Ships;
using Echo.Vectors;

namespace Echo.Objects
{
	public abstract class Structure : OrbitingObject
	{
		private readonly ObjectCollection<Agent> personnel;
		private readonly ObjectCollection<Ship> ships;
		private readonly ItemCollection<IItem> stores;

		protected Structure(ILocation orbiting, Corporation owner) : base(orbiting)
		{
			Owner = owner;

			this.ships = new ObjectCollection<Ship>(this);
			this.personnel = new ObjectCollection<Agent>(this);
			this.stores = new ItemCollection<IItem>();
		}

		public Corporation Owner { get; private set; }

		public IReadOnlyList<IItem> Stores
		{
			get { return this.stores; }
		}

		public override ObjectType ObjectType
		{
			get { return ObjectType.Structure; }
		}

		public IReadOnlyList<Ship> Ships
		{
			get { return this.ships; }
		}

		public IReadOnlyList<Agent> Personnel
		{
			get { return this.personnel; }
		}

		public abstract StructureType StructureType
		{ get; }

		protected void AddItem(IItem item)
		{
			item.Location = this;
			this.stores.Add(item);
		}

		protected void AddShip(Ship ship)
		{
			ship.Location = this;
			this.ships.Add(ship);
		}

		protected void RemoveItem(IItem item)
		{
			this.stores.Remove(item);
		}

		public bool Dock(Ship ship)
		{
			AssertShipInRange(ship, "dock with this station");

			this.ships.Add(ship);
			this.personnel.Add(ship.Pilot);

			ship.Pilot = null;
			ship.ClearCurrentTask();

			return true;
		}

		public bool Undock(Ship ship, Agent pilot)
		{
			if (this.ships.Contains(ship) == false)
				throw new InvalidOperationException("This ship is not docked with this station");

			if (this.personnel.Contains(pilot) == false)
				throw new InvalidOperationException("This pilot is not present in this station");

			this.ships.Remove(ship);
			this.personnel.Remove(pilot);

			ship.Pilot = pilot;
			ship.Location = Location;

			if (SolarSystem.Objects.Contains(ship) == false)
				SolarSystem.EnterSystem(ship, LocalCoordinates);

			return true;
		}

		public void TakeOwnership(Corporation newOwner)
		{
			if (newOwner == null)
				throw new ArgumentNullException("newOwner");

			var oldOwner = Owner;
			if (oldOwner != null)
				oldOwner.LoseStructure(this, newOwner);

            Owner = newOwner;
	
			this.personnel.RemoveAll(a => a.Employer == oldOwner);

			CorporationItems(oldOwner).ForEach(i => i.Owner = newOwner);
			CorporationShips(oldOwner).ForEach(s => s.Owner = newOwner);
			
			Owner.TakeStructure(this, oldOwner);
		}

		public List<Agent> CorporationPersonnel(Corporation corporation)
		{
			return this.personnel.FindAll(a => a.Employer == corporation);
		}

		public List<Ship> CorporationShips(Corporation corporation)
		{
			return this.ships.FindAll(s => s.Owner == corporation);
		}

		public List<IItem> CorporationItems(Corporation corporation)
		{
			return this.stores.FindAll(i => i.Owner == corporation);
		}
	}
}