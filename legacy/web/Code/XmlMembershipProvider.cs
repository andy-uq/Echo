using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;

namespace Echo.Web
{
	public class XmlMembershipProvider : MembershipProvider
	{
		private class Member
		{
			public int PasswordAttempts { get; set; }
			public int AnswerAttempts { get; set; }

			public MembershipUser ToMembershipUser()
			{
				var user = new MembershipUser("XmlMembershipProvider", Username, ID, Email, PasswordQuestion, Comment, IsApproved, IsLockedOut, CreateDate, LastLoginDate, LastActivityDate, LastPasswordChangeDate, LastLockoutDate ?? DateTime.MinValue);
				return user;
			}

			public DateTime LastPasswordChangeDate { get; set; }
			public DateTime LastLoginDate { get; set; }
			public DateTime LastActivityDate { get; set; }
			public DateTime CreateDate { get; set; }

			public bool IsApproved { get; set; }
			public DateTime? LastLockoutDate { get; set; }

			public bool IsLockedOut
			{
				get { return LastLockoutDate > DateTime.Now.Add(TimeSpan.FromMinutes(5)); }
			}

			public string PasswordQuestion { get; set; }

			public Guid ID { get; set; }
			public string Username { get; set; }
			public string Email { get; set; }

			public string Comment { get; set; }

			public Member(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved)
			{
				this.Username = username.ToLower();
				this.Email = email.ToLower();
				this.Password = password;
				this.PasswordQuestion = passwordQuestion;
				this.PasswordAnswer = passwordAnswer;
				this.IsApproved = isApproved;

				LastPasswordChangeDate = DateTime.Now;
				LastLoginDate = DateTime.Now;
				LastActivityDate = DateTime.Now;
				CreateDate = DateTime.Now;
			}

			public string PasswordAnswer { get; set; }

			public string Password { get; set; }

			public void Update()
			{
				LastActivityDate = DateTime.Now;
				PasswordAttempts = 0;
				AnswerAttempts = 0;
			}
		}

		private readonly List<Member> members;

		public XmlMembershipProvider()
		{
			this.members = new List<Member>();
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
				var member = new Member(username, EncodePassword(password), email, passwordQuestion, passwordAnswer, isApproved);
				this.members.Add(member);

				if (providerUserKey != null)
				{
					if (!(providerUserKey is Guid))
					{
						status = MembershipCreateStatus.InvalidProviderUserKey;
						return null;
					}

					member.ID = (Guid) providerUserKey;
				}

				status = MembershipCreateStatus.Success;
				return member.ToMembershipUser();
			}

			status = MembershipCreateStatus.DuplicateUserName;
			return null;
		}

		public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
		{
			if ( !ValidateUser(username, password) )
				return false;

			var user = GetMember(username);
			user.PasswordQuestion = newPasswordQuestion;
			user.PasswordAnswer = newPasswordAnswer;

			return true;
		}

		private Member GetMember(string username)
		{
			return this.members.Find(u => u.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));
		}

		private Member GetMember(Guid memberID)
		{
			return this.members.Find(u => u.ID == memberID);
		}

		public override string GetPassword(string username, string answer)
		{
			throw new ProviderException("Passwords are unable to be retreived.");
		}

		public override bool ChangePassword(string username, string oldPassword, string newPassword)
		{
			if (ValidateUser(username, oldPassword) == false)
				return false;
            
			var args = new ValidatePasswordEventArgs(username, newPassword, false);
			OnValidatingPassword(args);

			if (args.Cancel)
			{
				if (args.FailureInformation != null)
					throw args.FailureInformation;

				throw new MembershipPasswordException("Change password canceled due to new password validation failure.");
			}

			var user = GetMember(username);
			user.Password = EncodePassword(newPassword);
			user.LastPasswordChangeDate = DateTime.Now;
			user.Update();

			return true;
		}

		public override string ResetPassword(string username, string answer)
		{
			if (EnablePasswordReset == false)
			{
				throw new NotSupportedException("Password reset is not enabled.");
			}

			if (answer == null && RequiresQuestionAndAnswer)
			{
				UpdateFailureCount(username, "passwordAnswer");

				throw new ProviderException("Password answer required for password reset.");
			}

			var user = members.Find(u => u.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase) && u.PasswordAnswer.Equals(answer, StringComparison.InvariantCultureIgnoreCase));
			if (user == null)
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

			return user.Password;
		}

		private void UpdateFailureCount(string username, string failureType)
		{
			var member = GetMember(username);
			if (member == null)
				return;

			if (failureType == "password")
			{
				member.PasswordAttempts++;			
			}
			else if (failureType == "answer")
			{
				member.AnswerAttempts++;
			}

			if (member.PasswordAttempts > MaxInvalidPasswordAttempts || member.AnswerAttempts > MaxInvalidPasswordAttempts)
			{
				member.LastLockoutDate = DateTime.Now;
			}
		}

		public override void UpdateUser(MembershipUser user)
		{
			var m = GetMember((Guid )user.ProviderUserKey);
			if (m == null)
				throw new ProviderException("Unable to find registered member.");

			m.Email = user.Email;
			m.IsApproved = user.IsApproved;
		}

		public override bool ValidateUser(string username, string password)
		{
			var member = GetMember(username);
			if (member == null)
				return false;

			if (member.IsApproved == false || member.IsLockedOut)
				return false;

			if (member.Password == EncodePassword(password))
			{
				member.LastLoginDate = DateTime.Now;
				member.Update();

				return true;
			}

			UpdateFailureCount(username, "password");
			return false;
		}

		public override bool UnlockUser(string userName)
		{
			var member = GetMember(userName);
			if (member == null)
				return false;

			member.LastLockoutDate = null;
			return true;
		}

		public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
		{
			var member = members.Find(m => m.ID == (Guid) providerUserKey);
			if (member == null)
				return null;

			return member.ToMembershipUser();
		}

		public override MembershipUser GetUser(string username, bool userIsOnline)
		{
			var member = GetMember(username);
			if (member == null)
				return null;

			return member.ToMembershipUser();
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
			var member = members.Find(m => m.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
			if (member == null)
				return null;

			return member.Username;
		}

		public override bool DeleteUser(string username, bool deleteAllRelatedData)
		{
			return this.members.RemoveAll(m => m.Username == username) == 1;
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
			return PageMembers(this.members, pageIndex, pageSize, out totalRecords);
		}

		private static MembershipUserCollection PageMembers(List<Member> filteredMembers, int pageIndex, int pageSize, out int totalRecords)
		{
			int startRecord = pageIndex * pageSize;

			totalRecords = filteredMembers.Count;

			if (startRecord > totalRecords)
				throw new ArgumentOutOfRangeException("pageIndex", "Page index is greater than the number of members");

			var count = Math.Min(totalRecords - startRecord, pageSize);
	
			var result = new MembershipUserCollection();
			filteredMembers.GetRange(startRecord, count).ForEach(m => result.Add(m.ToMembershipUser()));

			return result;
		}

		public override int GetNumberOfUsersOnline()
		{
			throw new System.NotImplementedException();
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
			var filteredMembers = this.members.FindAll(m => m.Username.Contains(usernameToMatch));
		
			return PageMembers(filteredMembers, pageIndex, pageSize, out totalRecords);
		}

		public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
		{
			emailToMatch = emailToMatch.ToLower();
			var filteredMembers = this.members.FindAll(m => m.Email.Contains(emailToMatch));

			return PageMembers(filteredMembers, pageIndex, pageSize, out totalRecords);
		}

		//
		// EncodePassword
		//   Encrypts, Hashes, or leaves the password clear based on the PasswordFormat.
		//
		private string EncodePassword(string password)
		{
			string encodedPassword = password;

			var passwordData = Encoding.Unicode.GetBytes(password);
			switch ( PasswordFormat )
			{
				case MembershipPasswordFormat.Clear:
					break;

				case MembershipPasswordFormat.Encrypted:
					encodedPassword = Convert.ToBase64String(EncryptPassword(passwordData));
					break;

				case MembershipPasswordFormat.Hashed:
					var hash = SHA1.Create();
					encodedPassword = Convert.ToBase64String(hash.ComputeHash(passwordData));
					break;

				default:
					throw new ProviderException("Unsupported password format.");
			}

			return encodedPassword;
		}

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
	}
}