using System;
using Echo.Items;
using Echo.State;
using NUnit.Framework;

namespace Echo.Tests.Items
{
	[TestFixture]
	public class BluePrintTests
	{
		[Test]
		public void NewBluePrint()
		{
			var bluePrint = new BluePrintInfo();
			Assert.That(bluePrint.BuildRequirements, Is.Empty);
			Assert.That(bluePrint.Materials, Is.Empty);
		}

		[Test]
		public void CanOnlyUseBluePrintItemCodes()
		{
			new BluePrintInfo(ItemCode.MiningLaser);
			try
			{
				new BluePrintInfo(ItemCode.Veldnium);
				Assert.Fail();
			}
			catch (ArgumentException)
			{
			}
		}
	}
}