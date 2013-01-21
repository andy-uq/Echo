namespace Echo.Tests.Mocks
{
	public class IdGenerator : IIdGenerator
	{
		private long _id;

		public IdGenerator()
		{
			_id = 1L;
		}
		
		public long NextId()
		{
			return _id++;
		}
	}
}