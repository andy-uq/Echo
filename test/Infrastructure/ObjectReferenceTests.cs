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
	}
}