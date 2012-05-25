using NUnit.Framework;

namespace Echo.Tests.Math
{
	[TestFixture]
	public class UnitTests
	{
		private readonly Vector _sun = new Vector(0, 0, 0);
		private readonly Vector _earth = new Vector(149597870.7, 0, 0);

		[Test] 
		public void EarthToSun()
		{
			var distance = (_earth - _sun).Magnitude;
			Assert.That(distance, Is.InRange(Units.AstronomicalUnits * 0.99, Units.AstronomicalUnits * 1.01));
		}

		[Test] 
		public void FromAU()
		{
			var distance = (_earth - _sun).Magnitude;
			Assert.That(Units.FromAU(1), Is.InRange(Units.AstronomicalUnits * 0.99, Units.AstronomicalUnits * 1.01));
		}

		[Test] 
		public void ToAU()
		{
			var distance = (_earth - _sun).Magnitude;
			Assert.That(Units.ToAU(distance), Is.InRange(0.995, 1.005));
		}

		[Test] 
		public void Equals()
		{
			Assert.That(Units.Equal(0.001d, 0.001d));
			Assert.That(Units.Equal(0.00101d, 0.00101d));
		}
	}
}