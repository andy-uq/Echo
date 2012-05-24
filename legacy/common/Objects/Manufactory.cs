using System;
using System.Collections.Generic;

using Echo.Entities;
using Echo.Events;
using Echo.Ships;
using Echo.Vectors;

namespace Echo.Objects
{
	public class BuildTask
	{
		public BuildTask(Blueprint blueprint)
		{
			BluePrint = blueprint;
			Progress = 0;
			Materials = new List<IItem>();
		}

		public ulong StartTime { get; private set; }
		public Blueprint BluePrint { get; private set; }
		public double Progress { get; set; }
		public uint TimeTaken { get; set; }

		public List<IItem> Materials { get; private set; }
	}

	public class Manufactory : Structure
	{
		private BuildTask currentTask;

		public Manufactory(ILocation orbiting, Corporation owner) : base(orbiting, owner)
		{
			OreProcessedPerTick = 100;
			Efficiency = 0.1d;
		}

		public double Efficiency { get; set; }
		public uint OreProcessedPerTick { get; set; }

		public uint OreRemaining { get; private set; }
		
		public uint ProductCount
		{
			get
			{
				if ( this.currentTask == null )
					return 0;

				var buildItem = Stores.Find(i => i.ItemID == this.currentTask.BluePrint.BuildItemID);
				return (buildItem == null) ? 0 : buildItem.Quantity;
			}
		}

		protected override string SystematicNamePrefix
		{
			get { return "MF"; }
		}

		public void Build(Blueprint blueprint)
		{
			var task = new BuildTask(blueprint);

			foreach ( Material material in blueprint.Materials )
			{
				try
				{
					IItem item = Stores.Find(i => i.ItemID == material.ItemID);
					if ( item == null )
						throw new InvalidOperationException("Manufactory does not have any {0}".Expand(material.Name));

					if ( item.Quantity < material.Quantity )
						throw new InvalidOperationException("Manufactory does not have enough {0}".Expand(material.Name));

					IItem buildMaterials = item.Unstack(material.Quantity);
					task.Materials.Add(buildMaterials);
				}
				catch (Exception)
				{
					task.Materials.ForEach(AddItem);
					throw;
				}
			}

			if (currentTask != null)
			{
				currentTask.Materials.ForEach(AddItem);
			}

			currentTask = task;
		}

		/// <summary>Unload refined ore from a ship</summary>
		/// <param name="ship"></param>
		/// <returns>True if ore was unloaded</returns>
		public bool UnloadRefinedOre(Ship ship)
		{
			AssertShipInRange(ship, "unload refined ore to this manufactory");

			IItem ore = ship.RemoveCargo(RefinedOre.RefinedOreID);

			if (ore != null)
			{
				OreRemaining += ore.Quantity;

				Universe.EventPump.RaiseEvent(ship, EventType.ShipCargo, "Unloaded {0:n0} refined ore to {1}", ore.Quantity, Name);

				return true;
			}

			return false;
		}

		public override void Tick(ulong tick)
		{
			base.Tick(tick);

			if (this.currentTask == null)
				return;

			this.currentTask.Progress += Efficiency;
			if (this.currentTask.Progress > this.currentTask.BluePrint.BuildCost)
			{
				this.currentTask.Materials.ForEach(m => m.Destroy());

				IItem item = CreateItem(this.currentTask.BluePrint.CreateItem());
		
				Universe.EventPump.RaiseEvent(this, EventType.Production, "Made {0}", item);
			}
		}

		protected virtual IItem CreateItem(IItem item)
		{
			AddItem(item);
			return item;
		}

		public override StructureType StructureType
		{
			get { return StructureType.Manufactory; }
		}
	}
}