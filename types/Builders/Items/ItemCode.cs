namespace Echo.Items
{
	public enum ItemCode
	{
		Invalid,

		[Category(ItemType.Ships)]
		LightFrigate,

		[Category(ItemType.ShipWeapons)]
		[Category(ItemType.Blueprints)]
		MissileLauncher,

		[Category(ItemType.ShipWeapons)]
		[Category(ItemType.Blueprints)]
		MiningLaser,

		[Category(ItemType.ShipShields)]
		EnergyShield,

		[Category(ItemType.Ores)]
		Veldnium
	}
}