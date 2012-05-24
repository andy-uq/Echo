using System;

using Echo.Maths;
using Echo.Vectors;

namespace Echo.Objects
{
	public partial class SolarSystem : BaseLocation
	{
		private readonly ObjectCollection<OrbitingObject> satellites;
		private readonly ObjectCollection<ILocation> objects;

		public SolarSystem() 
		{
			this.satellites = new ObjectCollection<OrbitingObject>(this);
			this.objects = new ObjectCollection<ILocation>(this);
		}

		public IReadOnlyList<ILocation> Objects
		{
			get { return this.objects; }
		}

		public IReadOnlyList<OrbitingObject> Satellites
		{
			get { return this.satellites; }
		}

		protected override string SystematicNamePrefix
		{
			get { return "SS"; }
		}

		public override ObjectType ObjectType
		{
			get { return ObjectType.SolarSystem; }
		}

		public void OrbitSun(OrbitingObject satellite, double timeToOrbit)
		{
			var r = timeToOrbit / (Math.PI * 2d);
			var radians = Rand.Next()*Math.PI*2d;
			var delta = new Vector(Math.Cos(radians) * r, Math.Sin(radians) * r, 0);

			OrbitSun(satellite, delta);
		}

		public void OrbitSun(OrbitingObject satellite, Vector initialPosition)
		{
			satellite.Location = this;
			this.satellites.Add(satellite);
			satellite.LocalCoordinates = initialPosition;

			this.satellites.Sort(SortByDistance);
			
			Extent = Math.Max(Extent, satellite.LocalCoordinates.Magnitude + satellite.Extent);
		}

		/// <summary>Makes the object enter this solar system.  The previous location is overwritten.  The solar system is now in charge of ticking the object</summary>
		/// <param name="obj"></param>
		/// <param name="position">Position of new object.  This is in local coordinates</param>
		public void EnterSystem(ILocation @obj, Vector position)
		{
			lock ( objects )
			{
				this.objects.Add(obj);
				obj.LocalCoordinates = position;
				obj.Location = this;

				this.objects.Sort(SortByDistance);
			}
		}

		/// <summary>Make the object leave the solar system</summary>
		/// <param name="obj"></param>
		public void LeaveSystem(ILocation @obj)
		{
			lock (objects)
			{
				this.objects.Remove(obj);
			}
		}

		public override void Tick(ulong tick)
		{
			this.satellites.ForEach(s => s.Tick(tick));
			this.objects.ForEach(t => t.Tick(tick));
		}

		private static int SortByDistance(ILocation x, ILocation y)
		{
			return x.UniversalCoordinates.Magnitude.CompareTo(y.UniversalCoordinates.Magnitude);
		}
	}
}