using System;

using Echo.Vectors;

using NUnit.Framework;

namespace Echo.Testing
{
	[TestFixture]
	public class Vectors
	{
		[Test]
		public void Magnitude()
		{
			Assert.AreEqual(1, new Vector(1, 0, 0).Magnitude);
			Assert.AreEqual(5, new Vector(3, 4, 0).Magnitude);
		}

		[Test]
		public void Normalise()
		{
			Assert.AreEqual(1.0, new Vector(3, 5, 4).ToUnitVector().Magnitude);
		}

		[Test]
		public void CrossProduct()
		{
			var a = new Vector(1, 0, 0);
			var b = new Vector(0, 1, 0);

			Assert.AreEqual(new Vector(0, 0, 1), a * b);
		}

		[Test]
		public void DotProduct()
		{
			var a = new Vector(1, 0, 0);
			var b = new Vector(0, 1, 0);

			Assert.AreEqual(Math.PI / 2d, Math.Acos(a.DotProduct(b)));

			a = new Vector(0, -1, 0);
			b = new Vector(0, 1, 0);

			Assert.AreEqual(Math.PI, Math.Acos(a.DotProduct(b)));
		}
	}
}