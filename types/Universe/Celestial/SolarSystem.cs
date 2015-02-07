using System;
using System.Collections.Generic;
using Echo.JumpGates;
using Echo.Ships;
using Echo.State;
using Echo.Structures;
using Echo;
using EnsureThat;

namespace Echo.Celestial
{
	public partial class SolarSystem : ILocation, IMoves, IEquatable<SolarSystem>
	{
		public ObjectType ObjectType { get { return ObjectType.SolarSystem;} }
		public ulong Id { get; set; }
		public string Name { get; set; }
		public Position Position { get; set; }

		public List<CelestialObject> Satellites { get; set; } 
		public List<Structure> Structures { get; set; } 
		public List<Ship> Ships { get; set; }
		public List<JumpGate> JumpGates { get; set; }

		public SolarSystem()
		{
			Satellites = new List<CelestialObject>();
			Structures = new List<Structure>();
			Ships = new List<Ship>();
			JumpGates = new List<JumpGate>();
		}

		public void LeaveSystem(Ship ship)
		{
			if ( Ships.Remove(ship) )
			{
				ship.Position = ship.Position.Leave();
			}
		}

		public void EnterSystem(Ship ship, Vector localCoordinates)
		{
			Ensure.That(() => ship.Position.Location).IsNull();
			
			Ships.Add(ship);
			ship.Position = new Position(this, localCoordinates);
		}

		public bool Equals(SolarSystem other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (ReferenceEquals(this, other)) return true;
			return other.Id == Id;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != typeof (SolarSystem)) return false;
			return Equals((SolarSystem) obj);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}
}