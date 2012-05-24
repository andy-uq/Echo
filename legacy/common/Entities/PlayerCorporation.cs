using System;

using Echo.Objects;

using Ubiquity.u2ool;

namespace Echo.Entities
{
	public partial class PlayerCorporation : Corporation
	{
		public PlayerCorporation()
		{
			ID = GuidHelper.DatabaseGuid();
		}

		public Guid ID { get; set; }

		public string Username { get; set; }
		public string Password { get; set; }
		public string Email { get; set; }
	
		public DateTime? LastPasswordChangeDate { get; set; }
		public DateTime? LastLoginDate { get; set; }
		public DateTime? LastActivityDate { get; set; }
		public DateTime CreateDate { get; private set; }

		public bool IsApproved { get; set; }
		public int PasswordAttempts { get; set; }
		public DateTime? LastLockoutDate { get; set; }

		public bool IsLockedOut
		{
			get { return LastLockoutDate > DateTime.Now.Add(TimeSpan.FromMinutes(5)); }
		}

		public override bool IsPlayer
		{
			get { return true; }
		}
	}
}