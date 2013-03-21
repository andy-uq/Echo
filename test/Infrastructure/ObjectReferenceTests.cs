using Echo.State;
using Moq;
using NUnit.Framework;

namespace Echo.Tests.Infrastructure
{
	[TestFixture]
	public class ObjectReferenceTests
	{
		[Test]	 
		public void ObjectReferenceToString()
		{
			Assert.That(new ObjectReference(1, "X").ToString(), Is.EqualTo("[x0001] X"));
		}

		[Test]
		public void ParseObjectReference()
		{
			var objRef = ObjectReference.Parse("[x0001] X");
			Assert.That(objRef.Id, Is.EqualTo(1));
			Assert.That(objRef.Name, Is.EqualTo("X"));
		}

		[Test]
		public void TryParseObjectReference()
		{
			ObjectReference objRef;
			Assert.That(ObjectReference.TryParse("[x0001] X", out objRef), Is.True);
			Assert.That(objRef.Id, Is.EqualTo(1));
			Assert.That(objRef.Name, Is.EqualTo("X"));

			Assert.That(ObjectReference.TryParse("Not an object reference", out objRef), Is.False);
		}

		[Test]
		public void ObjectEquality()
		{
			var comparer = new ObjectEqualityComparer();
			var m1 = new Mock<IObject>(MockBehavior.Strict);
			m1.SetupGet(o => o.Id).Returns(1L);

			var m2 = new Mock<IObject>(MockBehavior.Strict);
			m2.SetupGet(o => o.Id).Returns(2L);

			Assert.That(comparer.GetHashCode(null), Is.EqualTo(0));
			Assert.That(comparer.GetHashCode(m1.Object), Is.EqualTo(1L));

			Assert.That(comparer.Equals(null, null), Is.True);
			Assert.That(comparer.Equals(m1.Object, null), Is.False);
			Assert.That(comparer.Equals(m1.Object, m2.Object), Is.False);
			Assert.That(comparer.Equals(m1.Object, m1.Object), Is.True);
		}

		[Test]
		public void IObjectAsObjectReference()
		{
			IObject o = null;
			Assert.That(o.AsObjectReference(), Is.Null);

			var m = new Moq.Mock<IObject>(MockBehavior.Strict);
			m.Setup(x => x.Id).Returns(1);
			m.Setup(x => x.Name).Returns("X");

			var objRef = m.Object.AsObjectReference();
			Assert.IsNotNull(objRef);
			Assert.That(objRef.Value.Id, Is.EqualTo(1));
			Assert.That(objRef.Value.Name, Is.EqualTo("X"));
		}

		[Test]
		public void IObjectStateAsObjectReference()
		{
			IObjectState o = null;
			Assert.That(o.AsObjectReference(), Is.Null);

			var m = new Moq.Mock<IObjectState>(MockBehavior.Strict);
			m.Setup(x => x.ObjectId).Returns(1);
			m.Setup(x => x.Name).Returns("X");

			var objRef = m.Object.AsObjectReference();
			Assert.IsNotNull(objRef);
			Assert.That(objRef.Value.Id, Is.EqualTo(1));
			Assert.That(objRef.Value.Name, Is.EqualTo("X"));
		}

		[Test]
		public void ToObjectReference()
		{
			var m = new Moq.Mock<IObject>(MockBehavior.Strict);
			m.Setup(x => x.Id).Returns(1);
			m.Setup(x => x.Name).Returns("X");

			var objRef = m.Object.ToObjectReference();
			Assert.IsNotNull(objRef);
			Assert.That(objRef.Id, Is.EqualTo(1));
			Assert.That(objRef.Name, Is.EqualTo("X"));
		}

		[Test]
		public void ObjectReferenceEquality()
		{
			var o1 = new ObjectReference(10, "Bob");
			var o2 = new ObjectReference(10, "Fred");
			var o3 = new ObjectReference(10);
			var o4 = new ObjectReference(11);

			Assert.That(o1, Is.EqualTo(o2).And.EqualTo(o3));
			Assert.That(o1.GetHashCode(), Is.EqualTo(o2.GetHashCode()).And.EqualTo(o3.GetHashCode()));
			Assert.That(o1.Equals(o3), Is.True);
			Assert.That(o1.Equals(o4), Is.False);
			Assert.That(o1 == o3, Is.True);
			Assert.That(o1 != o4, Is.True);
			Assert.That(o1.Equals("not an object"), Is.False);
		}
	}
}