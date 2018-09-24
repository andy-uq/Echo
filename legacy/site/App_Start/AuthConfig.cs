using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.WebPages.OAuth;
using Echo.Web.Models;

namespace Echo.Web
{
	public static class AuthConfig
	{
		public static void RegisterAuth()
		{
			// To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
			// you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

			//OAuthWebSecurity.RegisterMicrosoftClient(
			//    clientId: "",
			//    clientSecret: "");

			OAuthWebSecurity.RegisterTwitterClient(
				consumerKey: "3juYiueihG4UsOBv64mdQ",
				consumerSecret: "axTX5kSJUS0fwqV4NZyrTPbpMpCwzyezzdgG4bygA");

			//OAuthWebSecurity.RegisterFacebookClient(
			//    appId: "",
			//    appSecret: "");

			OAuthWebSecurity.RegisterGoogleClient();
		}
	}
}
