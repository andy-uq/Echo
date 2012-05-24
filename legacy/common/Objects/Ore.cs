namespace Echo.Objects
{
	public class Ore : Item
	{
		public override int ItemID
		{
			get { return OreID; }
		}

		public Ore()
		{
			Name = "Ore";
		}

		public const int OreID = 1;
	}

	public class RefinedOre : Item
	{
		public RefinedOre()
		{
			Name = "Refined Quadrium";
			SizePerUnit = 0.1d;
		}

		public override int ItemID
		{
			get { return RefinedOreID; }
		}

		public const int RefinedOreID = 2;
	}
}