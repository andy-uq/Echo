<%@ page SubTitle="Login" language="C#" masterpagefile="~/Site.Master" autoeventwireup="true" codebehind="login.aspx.cs" inherits="Echo.Web.LoginPage" %>

<asp:content contentplaceholderid="main" runat="server">
	
	<asp:updatepanel id="validation" runat="server" updatemode="Conditional">
		<contenttemplate>
			<e:validation runat="server" />
			<asp:customvalidator id="operation" runat="server" display="None" />
		</contenttemplate>
	</asp:updatepanel>

	<e:panel id="info" runat="server" cssclass="info">
		<strong>Not Registered?</strong> Click <asp:hyperlink runat="server" text="here" navigateurl="~/register.aspx" /> to register now.</p>
	</e:panel>
	
	<fieldset>
		<legend>Account Information</legend>
		<p>
			Enter your username and password below to access the game.
		</p>
		
		<e:field runat="server" label="Username" control="txtUsername">
			<asp:textbox id="txtUsername" runat="server" cssclass="required" />
		</e:field>
		<e:field runat="server" label="Password" control="txtPassword">
			<asp:textbox id="txtPassword" runat="server" cssclass="required" textmode="Password" />
		</e:field>
		<asp:checkbox id="chkRememberMe" runat="server" text="Remember Me" />
		<div class="submit">
			<e:button id="btnLogin" runat="server" text="Log in" onclick="OnSubmit" />
		</div>
	</fieldset>
</asp:content>

