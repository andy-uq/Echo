using System;
using System.Diagnostics;

using Echo.Ships;
using Echo.Vectors;

namespace Echo.Objects
{
	[DebuggerDisplay("{Name}, Position: {Position}")]
	public abstract class BaseLocation : BaseObject, ILocation
	{
		public double Extent { get; protected set; }

		protected abstract string SystematicNamePrefix { get; }

		protected void AssertShipInRange(Ship ship, string action)
		{
			if ( Vector.Distance(UniversalCoordinates, ship.UniversalCoordinates) > 0.1 )
				throw new InvalidOperationException("Ship is not close enough to " + action + ".");
		}

		#region ILocation Members

		/// <summary>Star cluster this object is currently within</summary>
		public StarCluster StarCluster
		{
			get { return FindParent<StarCluster>(ObjectType.StarCluster); }
		}

		/// <summary>Solar system this object is currently within</summary>
		public SolarSystem SolarSystem
		{
			get { return FindParent<SolarSystem>(ObjectType.SolarSystem); }
		}

		/// <summary>Position relative to this object's parent</summary>
		public Vector LocalCoordinates
		{
			get { return UniversalCoordinates - Location.UniversalCoordinates; }
			set { UniversalCoordinates = Location.UniversalCoordinates + value; }
		}

		public virtual Vector UniversalCoordinates { get; set; }

		public string SystematicName
		{
			get { return string.Format("{0}-{1:d5}", SystematicNamePrefix, ObjectID); }
		}

		#endregion

		private T FindParent<T>(ObjectType objectType) where T : BaseLocation
		{
			ILocation obj = this;

			while (obj.ObjectType != objectType)
			{
				obj = obj.Location;
				if (obj == null)
					return null;
			}

			return (T) obj;
		}
	}
}