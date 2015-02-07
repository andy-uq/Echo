namespace Echo.Tests.Mocks
{
	public class IdGenerator : IIdGenerator
	{
		private ulong _id;

		public IdGenerator()
		{
			_id = 1L;
		}
		
		public ulong NextId()
		{
			return _id++;
		}
	}
}