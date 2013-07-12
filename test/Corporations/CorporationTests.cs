using Echo.Agents;
using Echo.Corporations;
using Echo.Items;
using Echo.Market;
using Echo.Ships;
using Echo.State;
using Echo.Structures;
using NUnit.Framework;

namespace Echo.Tests.Corporations
{
	[TestFixture]
	public class CorporationTests
	{
		[Test]
		public void CreateCorporation()
		{
			var corp = new Corporation();
			Assert.That(corp.ObjectType, Is.EqualTo(ObjectType.Corporation));
		}

		[Test]
		public void ObjectTypeIsCorporation()
		{
			var corp = new Corporation();
			Assert.That(corp.ObjectType, Is.EqualTo(ObjectType.Corporation));
		}

		[Test]
		public void CorporationItems()
		{
			var corp = new Corporation();
			corp.Property.Add(new Item(new ItemInfo(ItemCode.LightFrigate)));
		}

		[Test]
		public void CorporationStructures()
		{
			var corp = new Corporation();
			corp.Structures.Add(new Manufactory());
		}

		[Test]
		public void CorporationMembers()
		{
			var corp = new Corporation();
			corp.Employees.Add(new Agent());
		}

		[Test]
		public void CorporationVessels()
		{
			var corp = new Corporation();
			corp.Ships.Add(new Ship());
		}

		[Test]
		public void CorporationAuctions()
		{
			var corp = new Corporation();
			corp.BuyOrders.Add(new BuyOrder());
			corp.SellOrders.Add(new SellOrder());
		}

		[Test]
		public void CorporationBluePrints()
		{
			var corp = new Corporation();
			corp.BluePrints.Add(new Item(new BluePrintInfo()));
		}

		[Test]
		public void CorporationHangar()
		{
			var corp = new Corporation();
			var location = new Manufactory();

			var property = corp.GetProperty(location);
			property.Add(new Item(new ItemInfo()));
		}	
	}
}