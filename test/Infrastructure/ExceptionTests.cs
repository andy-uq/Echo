using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Echo.Exceptions;
using NUnit.Framework;
using Shouldly;

namespace Echo.Tests.Infrastructure
{
	[TestFixture]
	public class ExceptionTests
	{
		[Test]
		public void ItemNotFoundException()
		{
			try
			{
				throw new ItemNotFoundException("Foozle", "Bob");
			}
			catch (ItemNotFoundException e)
			{
				var d = AssertSerialise(e);
				d.Item.ShouldBe("Bob");
				d.ItemType.ShouldBe("Foozle");
			}
		}

		private T AssertSerialise<T>(T e)
		{
			var f = new BinaryFormatter();
			var data = Serialise(e, f);

			using (var ms = new MemoryStream(data))
				return (T) f.Deserialize(ms);
		}

		private byte[] Serialise(object o, BinaryFormatter binaryFormatter)
		{
			using ( var ms = new MemoryStream() )
			{
				binaryFormatter.Serialize(ms, o);
				return ms.ToArray();
			}
		}
	}
}