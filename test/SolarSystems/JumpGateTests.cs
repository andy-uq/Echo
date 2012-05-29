using System;
using System.Collections.Generic;
using Echo.Builders;
using Echo.Celestial;
using Echo.JumpGates;
using Echo.Ships;
using Echo.State;
using NUnit.Framework;

namespace Echo.Tests.SolarSystems
{
	[TestFixture]
	public class JumpGateTests
	{
		private JumpGate _j1, _j2, _j3;
		private SolarSystem _s1, _s2;


		[SetUp]
		public void SetUp()
		{
			_s1 = new SolarSystemState().Build(starCluster: null);
			_s2 = new SolarSystemState().Build(starCluster: null);

			var builder = new JumpGate.Builder();
			var j1 = builder.Build(_s1, new JumpGateState {Id = 1, ConnectsTo = 2});
			var j2 = builder.Build(_s2, new JumpGateState {Id = 2, ConnectsTo = 3});
			var j3 = builder.Build(_s2, new JumpGateState {Id = 3, ConnectsTo = -1});

			var register = new JumpGateRegister();
			register.Register(new[] { j1.Target, j2.Target, j3.Target } );

			_j1 = j1.Resolve(register);
			_j2 = j2.Resolve(register);
			_j3 = j3.Resolve(register);
		}

		[Test]
		public void JumpConnections()
		{
			Assert.That(_j1.ConnectsTo, Is.EqualTo(_j2));
			Assert.That(_j2.ConnectsTo, Is.EqualTo(_j3));
			Assert.That(_j3.ConnectsTo, Is.Null);
		}

		[Test, ExpectedException(typeof (ArgumentNullException))]
		public void JumpNullShip()
		{
			_j1.Jump(null);
		}

		[Test]
		public void JumpShip()
		{
			var s = new Ship();
			_s1.EnterSystem(s, Vector.Zero);
			_j1.Jump(s);

			Assert.That(s.SolarSystem, Is.EqualTo(_s2));
		}
	}
}