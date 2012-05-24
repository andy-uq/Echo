<%@ page subtitle="Logout" language="C#" masterpagefile="~/Site.Master" autoeventwireup="true" codebehind="logout.aspx.cs" inherits="Echo.Web.LogoutPage" %>

<asp:content contentplaceholderid="main" runat="server">
	<p>
		You have been logged out.  Click <asp:hyperlink runat="server" text="here" navigateurl="~/login.aspx" /> to log back in.
	</p>
</asp:content>

