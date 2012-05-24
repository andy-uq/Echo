using System;
using System.Collections.Generic;

using Echo.Vectors;

namespace Echo.Objects
{
	public abstract partial class BaseObject : IObject
	{
		private ILocation location;
		private Universe universe;
		private ulong? objectID;

		public ulong ObjectID
		{
			get { return EnsureObjectID(); }
		}

		protected ulong EnsureObjectID()
		{
			if ( objectID == null )
			{
				if ( universe == null && location == null )
					return 0;

				this.objectID = Universe.GenerateObjectID();
			}

			return this.objectID.Value;
		}

		#region IObject Members

		public abstract ObjectType ObjectType { get; }

		/// <summary>The location of this object, ie: Universe > StarCluster > SolarSystem > Planet > Moon > Refinery</summary>
		public ILocation Location
		{
			get { return this.location; }
			set
			{
				if (value == null)
					throw new ArgumentNullException("value");

				this.location = value;
			}
		}

		public Universe Universe
		{
			get
			{
				if (this.universe == null)
					this.universe = Location.Universe;

				return this.universe;
			}
		}

		public virtual ObjectFactory ObjectFactory
		{
			get { return Universe.ObjectFactory; }
		}

		public string Name { get; set; }

		public virtual void Tick(ulong tick)
		{
		}

		#endregion

		protected BaseObject MemberwiseClone(bool newID)
		{
			var clone = (BaseObject)MemberwiseClone();
			if (newID)
				clone.objectID = Universe.GenerateObjectID();

			return clone;
		}

		public override string ToString()
		{
			return Name ?? base.ToString();
		}
	}

	public interface ILocation : IObject
	{
		Vector UniversalCoordinates { get; set; }
		SolarSystem SolarSystem { get; }
		StarCluster StarCluster { get; }
		string SystematicName { get; }
		Vector LocalCoordinates { get; set; }
	}

	public interface IObject
	{
		string Name { get; }
		ObjectType ObjectType { get; }
		ILocation Location { get; set; }
		Universe Universe { get; }

		ulong ObjectID { get; }

		void Tick(ulong tick);
	}
}