<%@ page subtitle="My Settings" language="C#" masterpagefile="~/Site.Master" autoeventwireup="true" codebehind="settings.aspx.cs" inherits="Echo.Web.MySettingsPage" %>

<asp:content contentplaceholderid="main" runat="server">

	
	<asp:updatepanel id="validation" runat="server" updatemode="Conditional">
		<contenttemplate>
			<e:validation id="Validation1" runat="server" />
			<asp:customvalidator id="operation" runat="server" display="None" />
		</contenttemplate>
	</asp:updatepanel>

	<e:clientbutton runat="server" text="Change Password" action="popup('#changepwd')" />

	<e:popup id="changepwdConfirm" runat="server">
		Your password was changed successfully.
	</e:popup>

	<e:popup id="changepwd" runat="server">
		<fieldset>
		<legend>Change Password</legend>		
			<e:field runat="server" label="Old password" control="txtOldPassword">
				<asp:textbox id="txtOldPassword" runat="server" cssclass="required" textmode="Password" />
				<asp:label runat="server" associatedcontrolid="txtOldPassword" cssclass="error" text="* Required field" style="display: none" />
			</e:field>
			<e:field runat="server" label="New Password" control="txtPassword">
				<asp:textbox id="txtPassword" runat="server" cssclass="required password" textmode="Password" />
				<asp:label runat="server" associatedcontrolid="txtPassword" cssclass="error" text="* Required field" style="display: none" />
			</e:field>
			<e:field runat="server" label="Confirm Password" control="txtConfirmPassword">
				<asp:textbox id="txtConfirmPassword" runat="server" cssclass="required confirm" textmode="Password" />
				<asp:label runat="server" associatedcontrolid="txtConfirmPassword" cssclass="error auto" text="* Required field" style="display: none" />
			</e:field>
			<div class="submit">
				<e:button runat="server" text="Change Password" onclick="OnChangePassword" />
			</div>
		</fieldset>
	</e:popup>
	
	<script type="text/javascript">
		function pageInit() {
			$('.confirm').rules('add', { required: true, equalTo: '.password', messages: { required: "* Required field", equalTo: "* Must match new password."} });
		}
	</script>

</asp:content>

