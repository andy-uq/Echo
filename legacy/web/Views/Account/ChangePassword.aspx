<%@ page language="C#" masterpagefile="~/Views/Shared/Site.Master" inherits="Echo.Web.EchoViewPage" subtitle="Change Password" %>

<asp:content contentplaceholderid="main" runat="server">
	<p>
		Use the form below to change your password.
	</p>
	<p>
		New passwords are required to be a minimum of
		<%=Html.Encode(ViewData["PasswordLength"])%>
		characters in length.
	</p>
	<%= Html.ValidationSummary("Password change was unsuccessful. Please correct the errors and try again.")%>
	<% using ( Html.BeginForm() )
	{ %>
	<div>
		<fieldset>
			<legend>Account Information</legend>
			<p>
				<label for="currentPassword">
					Current password:</label>
				<%= Html.Password("currentPassword") %>
				<%= Html.ValidationMessage("currentPassword") %>
			</p>
			<p>
				<label for="newPassword">
					New password:</label>
				<%= Html.Password("newPassword") %>
				<%= Html.ValidationMessage("newPassword") %>
			</p>
			<p>
				<label for="confirmPassword">
					Confirm new password:</label>
				<%= Html.Password("confirmPassword") %>
				<%= Html.ValidationMessage("confirmPassword") %>
			</p>
			<p>
				<input type="submit" value="Change Password" />
			</p>
		</fieldset>
	</div>
	<% } %>
</asp:content>

