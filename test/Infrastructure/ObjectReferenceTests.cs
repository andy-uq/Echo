using Echo.State;
using NUnit.Framework;

namespace Echo.Tests.Infrastructure
{
	[TestFixture]
	public class ObjectReferenceTests
	{
		[Test]	 
		public void ObjectReferenceToString()
		{
			Assert.That(new ObjectReference(1, "X").ToString(), Is.EqualTo("[x0000000000000001] X"));
		}

		[Test]
		public void ParseObjectReference()
		{
			var objRef = ObjectReference.Parse("[x0000000000000001] X");
			Assert.That(objRef.Id, Is.EqualTo(1));
			Assert.That(objRef.Name, Is.EqualTo("X"));
		}

		[Test]
		public void TryParseObjectReference()
		{
			ObjectReference objRef;
			Assert.That(ObjectReference.TryParse("[x0000000000000001] X", out objRef), Is.True);
			Assert.That(objRef.Id, Is.EqualTo(1));
			Assert.That(objRef.Name, Is.EqualTo("X"));
		}
	}
}