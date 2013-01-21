using Moq;

namespace Echo.Tests.Mocks
{
	public class LocationService : ILocationServices
	{
		private readonly Mock<ILocationServices> _mock;

		public Mock<ILocationServices> Mock
		{
			get { return _mock; }
		}

		public LocationService()
		{
			_mock = new Moq.Mock<ILocationServices>();
		}

		public Vector GetExitPosition(ILocation location)
		{
			return _mock.Object.GetExitPosition(location);
		}
	}
}