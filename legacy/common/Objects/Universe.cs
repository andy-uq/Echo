using System;
using System.Linq;
using System.Xml;

using Echo.Entities;
using Echo.Events;
using Echo.Vectors;

namespace Echo.Objects
{
	public partial class Universe : BaseObject, ILocation
	{
		private readonly ObjectCollection<Corporation> corporations;
		private readonly ObjectCollection<StarCluster> starClusters;
		private readonly object syncRoot;
		private ulong currentTick;
		private ulong nextObjectID;
		private ObjectFactory factory;

		public Universe()
		{
			this.syncRoot = new object();

			this.factory = new ObjectFactory(this);
			this.starClusters = new ObjectCollection<StarCluster>(this);
			this.corporations = new ObjectCollection<Corporation>(this);

			Location = this;
			EventPump = new EventPump();

			this.nextObjectID = 0;
			EnsureObjectID();
		}

		public ulong CurrentTick
		{
			get { return this.currentTick; }
		}

		public IReadOnlyList<PlayerCorporation> Players
		{
			get
			{
				lock (this.corporations)
				{
					return this.corporations.OfType<PlayerCorporation>().ReadOnly();
				}
			}
		}

		public IReadOnlyList<StarCluster> StarClusters
		{
			get { return this.starClusters; }
		}

		public EventPump EventPump { get; private set; }

		public override ObjectFactory ObjectFactory { get { return this.factory; } }

		public IReadOnlyList<Corporation> Corporations
		{
			get { return this.corporations; }
		}

		#region ILocation Members

		public override ObjectType ObjectType
		{
			get { return ObjectType.Universe; }
		}

		Vector ILocation.UniversalCoordinates
		{
			get { return Vector.Zero; }
			set { throw new NotSupportedException(); }
		}

		Vector ILocation.LocalCoordinates
		{
			get { return Vector.Zero; }
			set { throw new NotSupportedException(); }
		}

		SolarSystem ILocation.SolarSystem
		{
			get { return null; }
		}

		StarCluster ILocation.StarCluster
		{
			get { return null; }
		}

		Universe IObject.Universe
		{
			get { return this; }
		}

		string ILocation.SystematicName
		{
			get { throw new NotSupportedException(); }
		}

		#endregion

		public void AddStarCluster(StarCluster starCluster)
		{
			starCluster.Location = this;
			this.starClusters.Add(starCluster);
		}

		public void Tick()
		{
			this.starClusters.ForEach(t => t.Tick(this.currentTick));
			this.currentTick++;
		}

		public override string ToString()
		{
			return "The Universe";
		}

		public ulong GenerateObjectID()
		{
			lock (this.syncRoot)
			{
				ulong result = this.nextObjectID;
				this.nextObjectID++;
				return result;
			}
		}

		public PlayerCorporation RegisterPlayerCorporation(string corporationName, string username, string email)
		{
			PlayerCorporation player = ObjectFactory.Player.Create(corporationName);
			player.Username = username;
			player.Email = email;

			lock (this.corporations)
			{
				this.corporations.Add(player);
			}

			return player;
		}
	}
}