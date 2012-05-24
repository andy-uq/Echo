using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

using Echo.Entities;

namespace Echo.Web
{
	public class XmlMembershipProvider : MembershipProvider
	{
		public override bool EnablePasswordRetrieval
		{
			get { return false; }
		}

		public override bool EnablePasswordReset
		{
			get { return true; }
		}

		public override bool RequiresQuestionAndAnswer
		{
			get { return true; }
		}

		public override string ApplicationName { get; set; }

		public override int MaxInvalidPasswordAttempts
		{
			get { return 6; }
		}

		public override int PasswordAttemptWindow
		{
			get { return 3; }
		}

		public override bool RequiresUniqueEmail
		{
			get { return true; }
		}

		public override MembershipPasswordFormat PasswordFormat
		{
			get { return MembershipPasswordFormat.Hashed; }
		}

		public override int MinRequiredPasswordLength
		{
			get { return 6; }
		}

		public override int MinRequiredNonAlphanumericCharacters
		{
			get { return 0; }
		}

		public override string PasswordStrengthRegularExpression
		{
			get { return @"[a-z0-9]{6,}"; }
		}

		public static MembershipUser ToMembershipUser(PlayerCorporation player)
		{
			var user = new MembershipUser("XmlMembershipProvider", 
										player.Username, 
										player.ID, 
										player.Email, 
										null, 
										null, 
										player.IsApproved, 
										player.IsLockedOut, 
										player.CreateDate, 
										player.LastLoginDate ?? DateTime.MinValue, 
										player.LastActivityDate ?? DateTime.MinValue, 
										player.LastPasswordChangeDate ?? DateTime.MinValue, 
										player.LastLockoutDate ?? DateTime.MinValue);

			return user;
		}

		public static void Update(PlayerCorporation player)
		{
			player.LastActivityDate = DateTime.Now;
			player.PasswordAttempts = 0;
		}

		///<summary>
		///
		///                    Adds a new membership user to the data source.
		///                
		///</summary>
		///
		///<returns>
		///
		///                    A <see cref="T:System.Web.Security.MembershipUser" /> object populated with the information for the newly created user.
		///                
		///</returns>
		///
		///<param name="username">
		///                    The user name for the new user. 
		///                </param>
		///<param name="password">
		///                    The password for the new user. 
		///                </param>
		///<param name="email">
		///                    The e-mail address for the new user.
		///                </param>
		///<param name="passwordQuestion">
		///                    The password question for the new user.
		///                </param>
		///<param name="passwordAnswer">
		///                    The password answer for the new user
		///                </param>
		///<param name="isApproved">
		///                    Whether or not the new user is approved to be validated.
		///                </param>
		///<param name="providerUserKey">
		///                    The unique identifier from the membership data source for the user.
		///                </param>
		///<param name="status">
		///                    A <see cref="T:System.Web.Security.MembershipCreateStatus" /> enumeration value indicating whether the user was created successfully.
		///                </param>
		public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
		{
			var args = new ValidatePasswordEventArgs(username, password, true);
			OnValidatingPassword(args);

			if (args.Cancel)
			{
				status = MembershipCreateStatus.InvalidPassword;
				return null;
			}

			if (RequiresUniqueEmail && GetUserNameByEmail(email) != null)
			{
				status = MembershipCreateStatus.DuplicateEmail;
				return null;
			}

			MembershipUser u = GetUser(username, false);

			if (u == null)
			{
				status = MembershipCreateStatus.Success;
				return new MembershipUser("XmlMembershipProvider", username, null, email, passwordQuestion, null, false, false, DateTime.Now, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
			}

			status = MembershipCreateStatus.DuplicateUserName;
			return null;
		}

		public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
		{
			throw new NotSupportedException();
		}

		private static PlayerCorporation GetPlayer(string username)
		{
			return Global.Universe.Players.Find(p => p.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));
		}

		private static PlayerCorporation GetPlayer(Guid memberID)
		{
			return Global.Universe.Players.Find(p => p.ID == memberID);
		}

		public override string GetPassword(string username, string answer)
		{
			throw new ProviderException("Passwords are unable to be retreived.");
		}

		public override bool ChangePassword(string username, string oldPassword, string newPassword)
		{
			PlayerCorporation user = GetPlayer(username);
			
			if ( user.Password != null )
			{
				if (ValidateUser(username, oldPassword) == false)
					return false;
			}

			var args = new ValidatePasswordEventArgs(username, newPassword, false);
			OnValidatingPassword(args);

			if (args.Cancel)
			{
				if (args.FailureInformation != null)
					throw args.FailureInformation;

				throw new MembershipPasswordException("Change password canceled due to new password validation failure.");
			}

			if ( user.Password != null )
			{
				user.LastPasswordChangeDate = DateTime.Now;
				Update(user);
			}

			user.Password = EncodePassword(newPassword);
			return true;
		}

		public override string ResetPassword(string username, string answer)
		{
			if (EnablePasswordReset == false)
				throw new NotSupportedException("Password reset is not enabled.");

			PlayerCorporation player = GetPlayer(username);
			if (player == null)
				return null;

			string newPassword = Membership.GeneratePassword(MinRequiredPasswordLength, MinRequiredNonAlphanumericCharacters);

			var args = new ValidatePasswordEventArgs(username, newPassword, true);
			OnValidatingPassword(args);

			if (args.Cancel)
			{
				if (args.FailureInformation != null)
					throw args.FailureInformation;

				throw new MembershipPasswordException("Reset password canceled due to password validation failure.");
			}

			return player.Password;
		}

		public override void UpdateUser(MembershipUser user)
		{
			PlayerCorporation m = GetPlayer((Guid) user.ProviderUserKey);
			if (m == null)
				throw new ProviderException("Unable to find registered member.");

			m.Email = user.Email;
			m.IsApproved = user.IsApproved;
		}

		public override bool ValidateUser(string username, string password)
		{
			PlayerCorporation member = GetPlayer(username);
			if (member == null)
				return false;

			if (member.IsApproved == false || member.IsLockedOut)
				return false;

			if (member.Password == EncodePassword(password))
			{
				member.LastLoginDate = DateTime.Now;
				Update(member);

				return true;
			}

			member.PasswordAttempts++;
			if (member.PasswordAttempts > MaxInvalidPasswordAttempts)
				member.LastLockoutDate = DateTime.Now;

			return false;
		}

		public override bool UnlockUser(string userName)
		{
			PlayerCorporation member = GetPlayer(userName);
			if (member == null)
				return false;

			member.LastLockoutDate = null;
			return true;
		}

		public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
		{
			var player = GetPlayer((Guid) providerUserKey);
			return (player == null) ? null : ToMembershipUser(player);
		}

		public override MembershipUser GetUser(string username, bool userIsOnline)
		{
			PlayerCorporation member = GetPlayer(username);
			if (member == null)
				return null;

			return ToMembershipUser(member);
		}

		///<summary>
		///
		///                    Gets the user name associated with the specified e-mail address.
		///                
		///</summary>
		///
		///<returns>
		///
		///                    The user name associated with the specified e-mail address. If no match is found, return null.
		///                
		///</returns>
		///
		///<param name="email">
		///                    The e-mail address to search for. 
		///                </param>
		public override string GetUserNameByEmail(string email)
		{
			PlayerCorporation player = Global.Universe.Players.Find(p => p.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
			return (player == null) ? null : player.Username;
		}

		public override bool DeleteUser(string username, bool deleteAllRelatedData)
		{
			// TODO: Support deleting corporations
			return false;
		}

		///<summary>
		///
		///                    Gets a collection of all the users in the data source in pages of data.
		///                
		///</summary>
		///
		///<returns>
		///
		///                    A <see cref="T:System.Web.Security.MembershipUserCollection" /> collection that contains a page of <paramref name="pageSize" /><see cref="T:System.Web.Security.MembershipUser" /> objects beginning at the page specified by <paramref name="pageIndex" />.
		///                
		///</returns>
		///
		///<param name="pageIndex">
		///                    The index of the page of results to return. <paramref name="pageIndex" /> is zero-based.
		///                </param>
		///<param name="pageSize">
		///                    The size of the page of results to return.
		///                </param>
		///<param name="totalRecords">
		///                    The total number of matched users.
		///                </param>
		public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
		{
			return PageMembers(Global.Universe.Players, pageIndex, pageSize, out totalRecords);
		}

		private static MembershipUserCollection PageMembers(IEnumerable<PlayerCorporation> filteredMembers, int pageIndex, int pageSize, out int totalRecords)
		{
			int startRecord = pageIndex*pageSize;
			int record = 0;

			totalRecords = 0;

			var result = new MembershipUserCollection();

			IEnumerator<PlayerCorporation> e = filteredMembers.GetEnumerator();
			while (e.MoveNext())
			{
				totalRecords++;

				if (record < startRecord)
				{
					record++;
					continue;
				}

				if ( record - startRecord < totalRecords )
					result.Add(ToMembershipUser(e.Current));
			}

			return result;
		}

		public override int GetNumberOfUsersOnline()
		{
			throw new NotImplementedException();
		}

		///<summary>
		///
		///                    Gets a collection of membership users where the user name contains the specified user name to match.
		///                
		///</summary>
		///
		///<returns>
		///
		///                    A <see cref="T:System.Web.Security.MembershipUserCollection" /> collection that contains a page of <paramref name="pageSize" /><see cref="T:System.Web.Security.MembershipUser" /> objects beginning at the page specified by <paramref name="pageIndex" />.
		///                
		///</returns>
		///
		///<param name="usernameToMatch">
		///                    The user name to search for.
		///                </param>
		///<param name="pageIndex">
		///                    The index of the page of results to return. <paramref name="pageIndex" /> is zero-based.
		///                </param>
		///<param name="pageSize">
		///                    The size of the page of results to return.
		///                </param>
		///<param name="totalRecords">
		///                    The total number of matched users.
		///                </param>
		public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			usernameToMatch = usernameToMatch.ToLower();
			List<PlayerCorporation> filteredMembers = Global.Universe.Players.FindAll(m => m.Username.Contains(usernameToMatch));

			return PageMembers(filteredMembers, pageIndex, pageSize, out totalRecords);
		}

		public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			emailToMatch = emailToMatch.ToLower();
			List<PlayerCorporation> filteredMembers = Global.Universe.Players.FindAll(m => m.Email.Contains(emailToMatch));

			return PageMembers(filteredMembers, pageIndex, pageSize, out totalRecords);
		}

		//
		// EncodePassword
		//   Encrypts, Hashes, or leaves the password clear based on the PasswordFormat.
		//
		private string EncodePassword(string password)
		{
			string encodedPassword = password;

			byte[] passwordData = Encoding.Unicode.GetBytes(password);
			switch (PasswordFormat)
			{
				case MembershipPasswordFormat.Clear:
					break;

				case MembershipPasswordFormat.Encrypted:
					encodedPassword = Convert.ToBase64String(EncryptPassword(passwordData));
					break;

				case MembershipPasswordFormat.Hashed:
					SHA1 hash = SHA1.Create();
					encodedPassword = Convert.ToBase64String(hash.ComputeHash(passwordData));
					break;

				default:
					throw new ProviderException("Unsupported password format.");
			}

			return encodedPassword;
		}
	}
}