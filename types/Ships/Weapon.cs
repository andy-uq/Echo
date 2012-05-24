﻿namespace Echo.Ships
{
	public class Weapon : ILocation
	{
		public ObjectType ObjectType
		{
			get { return ObjectType.Item; }
		}

		public long Id { get; private set; }
		public string Name { get; private set; }
		public Position Position { get; set; }

		public void Tick(ulong tick)
		{
		}
	}
}