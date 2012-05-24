using System;
using System.Collections.Generic;

using Echo.Bank;
using Echo.Events;
using Echo.Objects;
using Echo.Ships;

using Ubiquity.u2ool.Collections;

namespace Echo.Entities
{
	public partial class Corporation : BaseObject
	{
		private readonly ItemCollection<IItem> assets;
		private readonly List<Auction> auctions;
		private readonly ItemCollection<IItem> blueprints;
		private List<Agent> employees;
		private readonly List<Ship> ships;
		private readonly List<Structure> structures;

		public Corporation()
		{
			Property = new List<IItem>();

			this.structures = new List<Structure>();
			this.ships = new List<Ship>();
			this.employees = new List<Agent>();
			this.auctions = new List<Auction>();
			this.assets = new ItemCollection<IItem>();
			this.blueprints = new ItemCollection<IItem>();
		}

		public ulong Bank { get; set; }

		public override ObjectType ObjectType
		{
			get { return ObjectType.Corporation; }
		}

		public List<IItem> Property { get; private set; }

		public ReadOnlyList<Ship> Ships
		{
			get { return this.ships; }
		}

		public ReadOnlyList<Structure> Structures
		{
			get { return this.structures; }
		}

		public IReadOnlyList<Agent> Employees
		{
			get { return this.employees.ReadOnly(); }
		}

		public IReadOnlyList<Auction> Auctions
		{
			get { return this.auctions.ReadOnly(); }
		}

		public IReadOnlyList<IItem> BluePrints
		{
			get { return this.blueprints; }
		}

		public IReadOnlyList<IItem> Assets
		{
			get
			{
				lock (this.assets)
				{
					return this.assets;
				}
			}
		}

		public virtual bool IsPlayer
		{
			get { return false; }
		}

		public void RemoveAsset(IItem item)
		{
			if (item.Owner != this)
				throw new InvalidOperationException("This item is owned by another corporation");

			lock (this.assets)
			{
				this.assets.Remove(item);
			}
		}

		public void AddAsset(IItem item)
		{
			if (item.Owner != this)
				throw new InvalidOperationException("This item is owned by another corporation");

			lock (this.assets)
			{
				this.assets.Add(item);
			}
		}

		public IItem Unstack(IItem item, uint quantity)
		{
			IItem unstack = item.Unstack(quantity);
			lock (this.assets)
				this.assets.Insert(0, unstack);

			return unstack;
		}

		public IItem Buy(Auction auction, uint quantity)
		{
			Agent agent = this.employees.Find(a => a.Location.StarCluster == auction.MarketPlace.StarCluster);
			return auction.MarketPlace.Buy(auction, quantity, agent);
		}

		public Auction CreateAuction(IItem item, uint pricePerUnit, uint blockSize)
		{
			if (item == null)
				throw new ArgumentNullException("item");

			if (item.Location.ObjectType != ObjectType.Structure)
				throw new InvalidOperationException("Unable to sell items on the market place when the item is not stored within a structure");

			if (this.assets.Remove(item) == false)
				throw new InvalidOperationException("This item is already being auctioned, or is not available for auctioning");

			MarketPlace marketPlace = item.Location.StarCluster.MarketPlace;
			Auction auction = marketPlace.CreateAuction(item, pricePerUnit, blockSize);

			this.auctions.Add(auction);
			return auction;
		}

		public void LoseStructure(Structure structure, Corporation newOwner)
		{
			structure.Personnel.ForEach(p => this.employees.Remove(p));
			structure.Ships.ForEach(s => this.ships.Remove(s));
			structure.Stores.ForEach(RemoveAsset);

			this.structures.Remove(structure);

			Universe.EventPump.RaiseEvent(this, EventType.StructureLost, "Lost {0} (a {1}) to {2}", Name, structure.StructureType, newOwner);
		}

		public void TakeStructure(Structure structure, Corporation oldOwner)
		{
			structure.CorporationShips(this).ForEach(s => this.ships.Add(s));
			structure.Stores.ForEach(AddAsset);

			this.structures.Add(structure);

			Universe.EventPump.RaiseEvent(this, EventType.StructureLost, "Took {0} (a {1}) from {2}", Name, structure.StructureType, oldOwner);
		}

		public Agent Recruit()
		{
			var newRecruit = ObjectFactory.Agent.Create(this);
			this.employees.Add(newRecruit);

			return newRecruit;
		}

		public void AuctionExpired(Auction auction)
		{
			AddAsset(auction.Item);
			this.auctions.Remove(auction);
		}

		public void AuctionSold(Auction auction)
		{
			if (auction.Quantity == 0)
				this.auctions.Remove(auction);
		}

		public void ShipDestroyed(Ship ship)
		{
			this.ships.Remove(ship);
		}
	}
}