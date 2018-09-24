using Moq;
using NUnit.Framework;

namespace Echo.Tests.Infrastructure
{
	[TestFixture]
	public class LocationServicesTest
	{
		[Test]
		public void GetExitPositionMax()
		{
			var mock = new Mock<IRandom>(MockBehavior.Strict);
			mock.Setup(x => x.GetNext()).Returns(1d);
			
			var result = Calculate(mock.Object);
			Assert.That(result, Is.EqualTo(Vector.Parse("2.5,2.5")));
		}

		[Test]
		public void GetExitPositionMin()
		{
			var mock = new Mock<IRandom>(MockBehavior.Strict);
			mock.Setup(x => x.GetNext()).Returns(0d);
			
			var result = Calculate(mock.Object);
			Assert.That(result, Is.EqualTo(Vector.Parse("-2.5,-2.5")));
		}

		private static Vector Calculate(IRandom random)
		{
			ILocationServices svc = new LocationServices(random);

			var location = new Mock<ILocation>(MockBehavior.Strict);
			location.SetupGet(x => x.Position).Returns(new Position(location.Object, Vector.Zero));

			var result = svc.GetExitPosition(location.Object);
			return result;
		}
	}
}