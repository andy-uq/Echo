﻿using System.Collections.Generic;
using System.Linq;
using Echo.Agents;
using Echo.Celestial;
using Echo.State;

namespace Echo.Ships
{
	public partial class Ship : ILocation, IMoves
	{
		private readonly List<HardPoint> _hardPoints;

		public ObjectType ObjectType
		{
			get { return ObjectType.Ship; }
		}

		public long Id { get; set; }
		public string Name { get; set; }
		public Position Position { get; set; }
		public Vector Heading { get; set; }
		public ShipStatistics Statistics { get; set; }
		public ShipInfo ShipInfo { get; set; }

		public IEnumerable<HardPoint> HardPoints
		{
			get { return _hardPoints; }
		}

		public SolarSystem SolarSystem
		{
			get { return Position.GetSolarSystem(); }
		}

		public Agent Pilot { get; set; }

		public Ship()
		{
			_hardPoints = new List<HardPoint>();
		}

		/// <summary>
		/// Returns true if a hard point can aim at a particular location
		/// </summary>
		public bool CanTrack(ILocation target)
		{
			return _hardPoints.Any(x => x.CanTrack(target));
		}

		/// <summary>
		/// Returns true if a hard point can move in time to aim at a target
		/// </summary>
		public bool CanAimAt(ILocation target)
		{
			return _hardPoints.Any(x => x.InRange(target));
		}
	}
}