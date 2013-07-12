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
			var solarSystem = new SolarSystemState();
			Assert.That(solarSystem.Size().Magnitude, Is.EqualTo(0.0));
		}

		[Test]
		public void SolarSystemWithOneObject()
		{
			var solarSystem = ObjectFactory.Celestial.SolarSystem(s => s.Planet(localCoordinates: new Vector(1, 0)));
			Assert.That(solarSystem.Size().Magnitude, Is.EqualTo(1.0));
		}

		[Test]
		public void SolarSystemWithTwoObjects()
		{
			var solarSystem = ObjectFactory.Celestial.SolarSystem(s => s.Planet(localCoordinates: new Vector(1, 0)));
			Assert.That(solarSystem.Size().Magnitude, Is.EqualTo(1.0));
		}
	}
}