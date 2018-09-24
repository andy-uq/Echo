using System;
using System.Collections.Generic;
using NUnit.Framework;
using Shouldly;

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
			Assert.That(Vector.Parse("(1,2)"), Is.EqualTo(new Vector(1, 2, 0)));
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
			Assert.That(Vector.Parse("{1,2}"), Is.EqualTo(new Vector(1, 2, 0)));
			Assert.That(Vector.Parse("{1.2345,2.3456,3.4567}"), Is.EqualTo(new Vector(1.2345, 2.3456, 3.4567)));
			Assert.That(Vector.Parse("{1.2345, 2.3456, 3.4567}"), Is.EqualTo(new Vector(1.2345, 2.3456, 3.4567)));
			Assert.That(Vector.Parse("{1, 0, 0}"), Is.EqualTo(new Vector(1, 0, 0)));
			Assert.That(Vector.Parse("{1,0,0}"), Is.EqualTo(new Vector(1, 0, 0)));
		}

		[Test]
		public void Parse3()
		{
			Assert.That(Vector.Parse("1,2"), Is.EqualTo(new Vector(1, 2, 0)));
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
			Should.Throw<ArgumentException>(() => Vector.Zero.ToUnitVector());
			Assert.That(new Vector(3, 4, 0).ToUnitVector().Magnitude, Is.EqualTo(1));
		}

		[Test]
		public void Distance()
		{
			Assert.That(Vector.Distance(new Vector(3, 4, 0), Vector.Zero), Is.EqualTo(5));
		}

		[Test]
		public void Subtract()
		{
			var a = new Vector(5, 12, 0);
			Assert.That((Vector.Zero - a).Magnitude, Is.EqualTo(13));
		}

		[TestCase("1,0,0", "0,1,0", 1.5707963267948966d)]
		[TestCase("1,0,0", "1,0,0", 0)]
		[TestCase("1,0,0", "-1,0,0", 3.141592653589793d)]
		public void Rotate(string a, string b, double angle)
		{
			Assert.That(Vector.Angle(Vector.Parse(a), Vector.Parse(b)), Is.EqualTo(angle));
		}

		[Test]
		public void Intersects()
		{
			var a = new Vector(3, 4, 0);
			var b = new Vector(5, 4, 0);

			Assert.That(Vector.Intersects(a, 1, b, 0.5d), Is.False);
			Assert.That(Vector.Intersects(a, 1, b, 1), Is.True);
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



		[Test]
		public void Equality()
		{
			var origin = new Vector(0, 0);
			var right = new Vector(1, 0);
			var left = new Vector(-1, 0);

			(origin == left).ShouldBe(false);
			(right + left == origin).ShouldBe(true);

			Equals(origin, right + left).ShouldBe(true);
		}

		[Test]
		public void AddToSet()
		{
			var origin = new Vector(0, 0);
			var right = new Vector(1, 0);
			var left = new Vector(-1, 0);

			var set = new HashSet<Vector>();
			set.Add(origin).ShouldBe(true);
			set.Add(origin).ShouldBe(false);
			set.Add(origin + right).ShouldBe(true);
			set.Add(new Vector(2, 0) + left).ShouldBe(false);
		}
	}
}