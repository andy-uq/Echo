using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Echo.Builder;
using Echo.Celestial;
using Echo.JumpGates;
using Echo.Ships;
using Echo.State;
using NUnit.Framework;
using Shouldly;

namespace Echo.Tests.SolarSystems
{
	[TestFixture]
	public class JumpGateTests
	{
#pragma warning disable 169,649 // fields are initialised via reflection
		private JumpGate _a1, _a2;
		private JumpGate _b1, _b2;
		private JumpGate _c1, _c2;
		private JumpGate _d1, _d2, _d3, _d4, _d5;
		private JumpGate _e1, _e2;
		private JumpGate _f1;
		private JumpGate _g1, _g2;
#pragma warning restore 169,649

		private SolarSystem _a, _b, _c, _d, _e, _f, _g;
		private SolarSystem[] _solarSystems;

		private static Dictionary<string, FieldInfo> GetFields()
		{
			return typeof (JumpGateTests)
				.GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
				.ToDictionary(f => f.Name);
		}

		[SetUp]
		public void SetUp()
		{
			_a = new SolarSystem { Name = "A" };
			_b = new SolarSystem { Name = "B" };
			_c = new SolarSystem { Name = "C" };
			_d = new SolarSystem { Name = "D" };
			_e = new SolarSystem { Name = "E" };
			_f = new SolarSystem { Name = "F" };
			_g = new SolarSystem { Name = "G" };
			_solarSystems = new[] { _a, _b, _c, _d, _e, _f, _g, };

			var fields = GetFields();
			var states = 
					fields.Values
					.Where(x => x.FieldType == typeof (JumpGate))
					.Select((f, i) => new JumpGateState { Name = f.Name, ObjectId = (ulong)(i + 1), ConnectsTo = 0 })
					.ToArray();

			SetConnections(states.ToDictionary(s => s.Name));

			var register = new JumpGateRegister();
			var builders = new List<ObjectBuilder<JumpGate>>();
			
			foreach (var state in states)
			{
				var solarSystem = (SolarSystem) fields[state.Name.Substring(0, 2)].GetValue(this);
				var builder = JumpGate.Builder.Build(solarSystem, state);
				register.Register(builder.Target);

				builders.Add(builder);
			}

			foreach (var builder in builders)
			{
				var jumpGate = builder.Build(register);
				fields[builder.Target.Name].SetValue(this, jumpGate);
				jumpGate.SolarSystem.JumpGates.Add(jumpGate);
			}

			foreach ( var x in _solarSystems.Select((s, i) => new { s, i }) )
			{
				x.s.Id = (ulong)(x.i + 10);
			}
		}

		private void SetConnections(IDictionary<string, JumpGateState> states)
		{
			Action<string, string> connect = (src, target) => { states["_"+src].ConnectsTo = states["_"+target].ObjectId; };
			connect("a1", "b2");
			connect("a2", "d5");
			connect("b1", "d1");
			connect("b2", "a1");
			connect("c1", "d4");
			connect("c2", "f1");
			connect("d1", "b1");
			connect("d2", "e1");
			connect("d3", "g1");
			connect("d4", "c1");
			connect("d5", "a2");
			connect("e2", "d2");
			connect("f1", "c2");
			connect("g1", "d3");
			connect("e1", "g2");
			connect("g2", "e1");
		}

		[Test]
		public void JumpCounts()
		{
			var jumpCountTable = new JumpCountTable(_solarSystems);
			var directConnections = jumpCountTable.Table.ToDictionary(x => x.SolarSystem);

			Assert.That(directConnections[_a].SolarSystem, Is.EqualTo(_a));
			Assert.That(directConnections[_a].DirectConnections.Count, Is.EqualTo(2));
			Assert.That(directConnections[_a].DirectConnections[0].SolarSystem, Is.EqualTo(_b));
			Assert.That(directConnections[_a].DirectConnections[1].SolarSystem, Is.EqualTo(_d));
			Assert.That(directConnections[_a].GetJumpCount(_b), Is.EqualTo(1));
			Assert.That(directConnections[_a].GetJumpCount(_e), Is.EqualTo(2));
			Assert.That(directConnections[_a].GetJumpCount(_f), Is.EqualTo(3));

			Assert.That(jumpCountTable.GetJumpCount(_e, _d), Is.EqualTo(1));
			Assert.That(jumpCountTable.GetJumpCount(_e, _f), Is.EqualTo(3));
			Assert.That(jumpCountTable.GetJumpCount(_e, _c), Is.EqualTo(2));
		}

		[TestCase("a", "b", 1)]
		[TestCase("b", "d", 1)]
		[TestCase("d", "e", 1)]
		[TestCase("d", "c", 1)]
		[TestCase("a", "c", 2)]
		[TestCase("a", "e", 2)]
		[TestCase("e", "a", 2)]
		[TestCase("e", "g", 1)]
		[TestCase("g", "e", 1)]
		[TestCase("a", "f", 3)]
		[TestCase("e", "f", 3)]
		public void JumpCounts(string fromName, string toName, int expectedJumps)
		{
			var fields = GetFields();
			var jumpCountTable = new JumpCountTable(_solarSystems);
			var from = (SolarSystem)fields[string.Concat("_", fromName)].GetValue(this);
			var to = (SolarSystem)fields[string.Concat("_", toName)].GetValue(this);

			Assert.That(jumpCountTable.GetJumpCount(from, to), Is.EqualTo(expectedJumps));
		}

		[TestCase("e2", "d2")]
		[TestCase("e1", "g2")]
		[TestCase("a1", "b2")]
		[TestCase("b1", "d1")]
		[TestCase("c1", "d4")]
		public void JumpConnections(string fromName, string toName)
		{
			var fields = GetFields();
			var from = (JumpGate)fields[string.Concat("_", fromName)].GetValue(this);
			var to = (JumpGate)fields[string.Concat("_", toName)].GetValue(this);

			Assert.That(from.ConnectsTo, Is.EqualTo(to));

		}

		[Test]
		public void JumpNullShip()
		{
			Should.Throw<ArgumentNullException>(() => _a1.Jump(null));
		}

		[Test]
		public void JumpShip()
		{
			var s = new Ship {Position = new Position(_b, Vector.Zero)};
			_a.EnterSystem(s, Vector.Zero);
			_a1.Jump(s);

			Assert.That(s.SolarSystem, Is.EqualTo(_b));
		}
	}
}