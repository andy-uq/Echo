using System.Collections;
using System.Xml;

using Echo.Objects;

namespace Echo.Entities
{
	public partial class PlayerCorporation
	{
		internal class PlayerCorporationState : CorporationState<PlayerCorporation>
		{
			public PlayerCorporationState(Universe.UniverseState universeState) : base(universeState)
			{
			}

			protected override string SerialiseAs
			{
				get { return "player"; }
			}

			protected override PlayerCorporation ReadXml(XmlElement xObject)
			{
				PlayerCorporation player = base.ReadXml(xObject);

				var xDetails = xObject.Select("details");

				player.ID = xDetails.Guid("id");
				player.Username = xDetails.String("username");
				player.Password = xDetails.String("password", null);
				player.Email = xDetails.String("email");
				player.LastPasswordChangeDate = xDetails.DateTime("lastPasswordChangeDate", false);
				player.LastLoginDate = xDetails.DateTime("lastLoginDate", false);
				player.LastActivityDate = xDetails.DateTime("lastActivityDate", false);
				player.IsApproved = xDetails.Boolean("isApproved");
				player.PasswordAttempts = xDetails.Int32("passwordAttempts");
				player.LastLockoutDate = xDetails.DateTime("lastLockoutDate", false);

				return player;
			}

			protected override void WriteXml(PlayerCorporation obj, XmlElement xObject)
			{
				var xDetails = xObject.Append("details");
				xDetails.Attribute("id", obj.ID);
				xDetails.Element("username", obj.Username);
				xDetails.Element("password", obj.Password);
				xDetails.Element("email", obj.Email);
				xDetails.Element("lastPasswordChangeDate", obj.LastPasswordChangeDate);
				xDetails.Element("lastLoginDate", obj.LastLoginDate);
				xDetails.Element("lastActivityDate", obj.LastActivityDate);
				xDetails.Element("createDate", obj.CreateDate);
				xDetails.Element("isApproved", obj.IsApproved);
				xDetails.Element("passwordAttempts", obj.PasswordAttempts);
				xDetails.Element("lastLockoutDate", obj.LastLockoutDate);
				
				base.WriteXml(obj, xObject);
			}
		}
	}
}