namespace Echo.Items
{
	public enum ItemCode
	{
		Invalid,

		[Category(ItemCategory.Ships)]
		LightFrigate,

		[Category(ItemCategory.ShipWeapons)]
		MissileLauncher,

		[Category(ItemCategory.ShipWeapons)]
		[Category(ItemCategory.Blueprints)]
		MiningLaser,

		[Category(ItemCategory.ShipShields)]
		EnergyShield,

		[Category(ItemCategory.Ores)]
		Veldnium
	}
}