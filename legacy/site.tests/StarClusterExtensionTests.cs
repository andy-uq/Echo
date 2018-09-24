using Echo;
using Echo.State;
using NUnit.Framework;
using site.code;
using test.common;

namespace site.tests
{
	[TestFixture]
	public class StarClusterExtensionTests
	{
		[Test]
		public void EmptySolarSystemSize0()
		{
			var x = new SolarSystemState();
			Assert.That(x.Size().Magnitude, Is.EqualTo(0.0));
		}

		[Test]
		public void SolarSystemWithOneObject()
		{
			var x = ObjectFactory.Celestial.SolarSystem(s => s.Planet(localCoordinates: new Vector(1, 0)));
			Assert.That(x.Size().Magnitude, Is.EqualTo(1.0));
		}

		[Test]
		public void SolarSystemWithTwoObjects()
		{
			var x = ObjectFactory.Celestial.SolarSystem(s => s.Planet(localCoordinates: new Vector(1, 0)).Planet(localCoordinates: new Vector(2, 0)));
			Assert.That(x.Size().Magnitude, Is.EqualTo(2.0));
		}

		[Test]
		public void SolarSystemWithTwoOppositeObjects()
		{
			var x = ObjectFactory.Celestial.SolarSystem(s => s.Planet(localCoordinates: new Vector(1, 0)).Planet(localCoordinates: new Vector(-1, 0)));
			Assert.That(x.Size().Magnitude, Is.EqualTo(1.0));
		}
		
		[Test]
		public void EmptyStarClusterSize0()
		{
			var x = new StarClusterState();
			Assert.That(x.Size().Magnitude, Is.EqualTo(0.0));
		}

		[Test]
		public void StarClusterWithOneObject()
		{
			var x = ObjectFactory.Celestial.StarCluster(s => s.SolarSystem(localCoordinates: new Vector(1, 0)));
			Assert.That(x.Size().Magnitude, Is.EqualTo(1.0));
		}

		[Test]
		public void StarClusterWithTwoObjects()
		{
			var x = ObjectFactory.Celestial.StarCluster(s => s.SolarSystem(localCoordinates: new Vector(1, 0)).SolarSystem(localCoordinates: new Vector(2, 0)));
			Assert.That(x.Size().Magnitude, Is.EqualTo(2.0));
		}

		[Test]
		public void StarClusterWithTwoOppositeObjects()
		{
			var x = ObjectFactory.Celestial.StarCluster(s => s.SolarSystem(localCoordinates: new Vector(1, 0)).SolarSystem(localCoordinates: new Vector(-1, 0)));
			Assert.That(x.Size().Magnitude, Is.EqualTo(1.0));
		}
	}
}