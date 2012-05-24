﻿<%@ page language="C#" masterpagefile="~/Views/Shared/Site.Master" inherits="Echo.Web.EchoViewPage" subtitle="Log On" %>

<asp:content contentplaceholderid="main" runat="server">
	<p>
		Please enter your username and password.
		<%= Html.ActionLink("Register", "Register") %>
		if you don't have an account.
	</p>
	<%= Html.ValidationSummary("Login was unsuccessful. Please correct the errors and try again.") %>
	<% using ( Html.BeginForm() )
	{ %>
	<div>
		<fieldset>
			<legend>Account Information</legend>
			<p>
				<label for="username">
					Username:</label>
				<%= Html.TextBox("username") %>
				<%= Html.ValidationMessage("username") %>
			</p>
			<p>
				<label for="password">
					Password:</label>
				<%= Html.Password("password") %>
				<%= Html.ValidationMessage("password") %>
			</p>
			<p>
				<%= Html.CheckBox("rememberMe") %>
				<label class="inline" for="rememberMe">
					Remember me?</label>
			</p>
			<p>
				<input type="submit" value="Log On" />
			</p>
		</fieldset>
	</div>
	<% } %>
</asp:content>

