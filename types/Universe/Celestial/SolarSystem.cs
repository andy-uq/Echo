using System.Collections.Generic;
using Echo.Ships;
using Echo.Structures;
using Echo;
using EnsureThat;

namespace Echo.Celestial
{
	public partial class SolarSystem : ILocation, IMoves
	{
		public ObjectType ObjectType { get { return ObjectType.SolarSystem;} }
		public long Id { get; set; }
		public string Name { get; set; }
		public Position Position { get; set; }

		public List<CelestialObject> Satellites { get; set; } 
		public List<Structure> Structures { get; set; } 
		public List<Ship> Ships { get; set; } 

		public void Tick(ulong tick)
		{
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
	}
}