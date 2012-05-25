using System;
using NUnit.Framework;
using Echo;

namespace Echo.Tests.Math
{
	[TestFixture]
	public class VectorTests
	{
		[Test]
		public void VectorToString()
		{
			Assert.That(new Vector(1, 0, 0).ToString(), Is.EqualTo("(1.0000, 0.0000, 0.0000)"));
			Assert.That(new Vector(1, 0, 0).ToString("x"), Is.EqualTo("1.0000"));
			Assert.That(new Vector(0, 1, 0).ToString("y"), Is.EqualTo("1.0000"));
			Assert.That(new Vector(0, 0, 1).ToString("z"), Is.EqualTo("1.0000"));
			Assert.That(new Vector(3, 4, 0).ToString("l"), Is.EqualTo("5.0000"));
		}

		[Test]
		public void Parse()
		{
			Assert.That(Vector.Parse("(1.2345,2.3456,3.4567)"), Is.EqualTo(new Vector(1.2345, 2.3456, 3.4567)));
			Assert.That(Vector.Parse("(1.2345, 2.3456, 3.4567)"), Is.EqualTo(new Vector(1.2345, 2.3456, 3.4567)));
			Assert.That(Vector.Parse("(1, 0, 0)"), Is.EqualTo(new Vector(1, 0, 0)));
			Assert.That(Vector.Parse("(1,0,0)"), Is.EqualTo(new Vector(1, 0, 0)));
			Assert.That(Vector.Parse("(-1,0,0)"), Is.EqualTo(new Vector(-1, 0, 0)));
			Assert.That(Vector.Parse("(-1,-1,0)"), Is.EqualTo(new Vector(-1, -1, 0)));
			Assert.That(Vector.Parse("(-1,-1,-1)"), Is.EqualTo(new Vector(-1, -1, -1)));
		}

		[Test]
		public void Parse2()
		{
			Assert.That(Vector.Parse("{1.2345,2.3456,3.4567}"), Is.EqualTo(new Vector(1.2345, 2.3456, 3.4567)));
			Assert.That(Vector.Parse("{1.2345, 2.3456, 3.4567}"), Is.EqualTo(new Vector(1.2345, 2.3456, 3.4567)));
			Assert.That(Vector.Parse("{1, 0, 0}"), Is.EqualTo(new Vector(1, 0, 0)));
			Assert.That(Vector.Parse("{1,0,0}"), Is.EqualTo(new Vector(1, 0, 0)));
		}

		[Test]
		public void Parse3()
		{
			Assert.That(Vector.Parse("1.0000, 0.0000, 0.0000"), Is.EqualTo(new Vector(1, 0, 0)));
			Assert.That(Vector.Parse("1.0000,0.0000,0.0000"), Is.EqualTo(new Vector(1, 0, 0)));
			Assert.That(Vector.Parse("1, 0, 0"), Is.EqualTo(new Vector(1, 0, 0)));
			Assert.That(Vector.Parse("1,0,0"), Is.EqualTo(new Vector(1, 0, 0)));
		}

		[Test]
		public void Parse4()
		{
			Assert.That(IsBad("Yo (1,0,0)"), Is.True);
			Assert.That(IsBad("1.0000, 0.0000, 0.0000)"), Is.True);
			Assert.That(IsBad("1, 0, 0}"), Is.True);
			Assert.That(IsBad("1, 0, 0)"), Is.True);
			Assert.That(IsBad("(1.2345, 1.2345, 1.2345, 1.2345)"), Is.True);
			Assert.That(IsBad("( 1.2345, 1.2345, 1.2345"), Is.True);
			Assert.That(IsBad("{ 1.2345, 1.2345, 1.2345"), Is.True);
		}

		private bool IsBad(string value)
		{
			try
			{
				var v = Vector.Parse(value);
				Console.Write(v);
				return false;
			}
			catch (FormatException)
			{
				return true;
			}
		}


		[Test]
		public void Scale()
		{
			Assert.That(new Vector(1, 0, 0).Scale(5), Is.EqualTo(new Vector(5, 0, 0)));
			Assert.That(new Vector(0, 1, 0).Scale(5), Is.EqualTo(new Vector(0, 5, 0)));
			Assert.That(new Vector(0, 0, 1).Scale(5), Is.EqualTo(new Vector(0, 0, 5)));
			Assert.That(new Vector(-1, -1, -1).Scale(5), Is.EqualTo(new Vector(-5, -5, -5)));
		}

		[Test]
		public void Magnitude()
		{
			Assert.That(new Vector(1, 0, 0).Magnitude, Is.EqualTo(1));
			Assert.That(new Vector(3, 4, 0).Magnitude, Is.EqualTo(5));
		}

		[Test]
		public void Normalise()
		{
			Assert.That(new Vector(3, 4, 0).ToUnitVector().Magnitude, Is.EqualTo(1));
		}

		[Test]
		public void CrossProduct()
		{
			var a = new Vector(1, 0, 0);
			var b = new Vector(0, 1, 0);

			Assert.That(a * b, Is.EqualTo(new Vector(0, 0, 1)));
		}

		[Test]
		public void DotProduct()
		{
			var a = new Vector(1, 0, 0);
			var b = new Vector(0, 1, 0);

			Assert.That(System.Math.Acos(a.DotProduct(b)), Is.EqualTo(System.Math.PI / 2d));

			a = new Vector(0, -1, 0);
			b = new Vector(0, 1, 0);

			Assert.That(System.Math.Acos(a.DotProduct(b)), Is.EqualTo(System.Math.PI));
		}
	}
}