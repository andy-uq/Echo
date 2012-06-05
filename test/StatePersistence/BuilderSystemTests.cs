using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;

namespace Echo.Tests.StatePersistence
{
	[TestFixture]
	public class BuilderSystemTests
	{
		class TestObject : IObject
		{
			public ObjectType ObjectType
			{
				get { throw new System.NotImplementedException(); }
			}

			public long Id { get; set; }
			public string Name { get; set; }

			public List<TestObject> Children { get; set; }

			public void Tick(ulong tick)
			{
				throw new System.NotImplementedException();
			}

			public void Add(TestObject dependent)
			{
				Children.Add(dependent);
			}
		}

		[Test]
		public void Dependents()
		{
			var mObject = new Mock<IObject>(MockBehavior.Strict);
			mObject.SetupGet(o => o.Id).Returns(1);

			var mChildObject = new Mock<IObject>(MockBehavior.Strict);
			mChildObject.SetupGet(o => o.Id).Returns(2);

			var mChildState = new Mock<IObjectState>(MockBehavior.Strict);
			var builder = new ObjectBuilder<IObject>(mObject.Object);
			builder.Dependents(new[] { mChildState.Object }).Build(state => new ObjectBuilder<IObject>(mChildObject.Object));
			
			Assert.That(builder.Dependents, Is.Not.Empty);
			Assert.That(builder.Dependents, Has.Exactly(1).Matches<IResolutionContext>(x => x.Target.Id == 2));
		}

		[Test]
		public void Resolves()
		{
			var testObject = new TestObject() { Id = 1, Children = new List<TestObject>() };

			var mChildObject = new Mock<IObject>(MockBehavior.Strict);
			mChildObject.SetupGet(o => o.Id).Returns(2);

			var mChildState = new Mock<IObjectState>(MockBehavior.Strict);
			var builder = new ObjectBuilder<IObject>(testObject);
			builder.Dependents(new[] { mChildState.Object }).Build(state => new ObjectBuilder<IObject>(mChildObject.Object));

			var r = builder.Resolve(null);
			Assert.That(r, Is.EqualTo(testObject));
		}

		[Test]
		public void ResolvesChildren()
		{
			var testObject = new TestObject() { Id = 1, Children = new List<TestObject>() };
			var child = new TestObject();

			var mChildState = new Mock<IObjectState>(MockBehavior.Strict);
			var builder = new ObjectBuilder<TestObject>(testObject);
			var t1 = builder
				.Dependent(mChildState.Object)
				.Build(state => new ObjectBuilder<TestObject>(child));
				
			var t2 = t1.Resolve(
					(target, resolver, dependent) => target.Add(dependent)
				);

			Assert.That(t1, Is.EqualTo(t2));
			Assert.That(builder.Dependents.First(), Is.EqualTo(t2));

			var context = (IResolutionContext) t2;
			var resolved = context.Resolve(null);
			Assert.That(resolved, Is.EqualTo(child));

			var r = builder.Resolve(null);
			Assert.That(r, Is.EqualTo(testObject));
			Assert.That(r.Children, Is.Not.Null);
			Assert.That(r.Children.First(), Is.EqualTo(child));
		}
	}
}