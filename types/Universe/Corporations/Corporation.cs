namespace Echo.Corporations
{
	public class Corporation : IObject
	{
		public ObjectType ObjectType
		{
			get { return ObjectType.Corporation; }
		}

		public long Id { get; set; }
		public string Name { get; set; }

		public void Tick(ulong tick)
		{
		}
	}
}