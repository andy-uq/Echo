<%@ control language="C#" autoeventwireup="true" codebehind="loggedin.ascx.cs" inherits="Echo.Web.Controls.LoggedInControl" %>
<div id="logindisplay">
	<ul>
		<asp:placeholder id="phLoggedIn" runat="server">
			<li class="username"><asp:literal id="litUserName" runat="server" /></li>
			<li><asp:hyperlink runat="server" text="My Settings" navigateurl="~/members/settings.aspx" /></li>
			<li><asp:hyperlink runat="server" text="Logout" navigateurl="~/logout.aspx" /></li>
		</asp:placeholder>
		<asp:placeholder id="phNotLoggedIn" runat="server">
			<li><asp:hyperlink runat="server" text="Login" navigateurl="~/login.aspx" /></li>
		</asp:placeholder>
	</ul>
</div>
