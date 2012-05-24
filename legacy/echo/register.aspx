<%@ page subtitle="Register" language="C#" masterpagefile="~/Site.Master" autoeventwireup="true" codebehind="register.aspx.cs" inherits="Echo.Web.RegisterPage" %>

<asp:content contentplaceholderid="main" runat="server">
	<e:validation runat="server" />
	<asp:customvalidator id="operation" runat="server" display="none" />
	
	<fieldset>
		<legend>Account Information</legend>
	
		<p>Enter your information below.</p>
	
		<e:field runat="server" label="Username" control="txtUsername">
			<asp:textbox id="txtUsername" runat="server" cssclass="required" />
		</e:field>
		<e:field runat="server" label="Password" control="txtPassword">
			<asp:textbox id="txtPassword" runat="server" cssclass="required password" textmode="Password" />
		</e:field>
		<e:field runat="server" label="Confirm Password" control="txtConfirmPassword">
			<asp:textbox id="txtConfirmPassword" runat="server" cssclass="required confirm" textmode="Password" />
		</e:field>
		<e:field runat="server" label="Email address" control="txtEmailAddress">
			<asp:textbox id="txtEmailAddress" runat="server" cssclass="required email" />
		</e:field>
		<e:field runat="server" label="Your corporation name" control="txtCorporationName">
			<asp:textbox id="txtCorporationName" runat="server" cssclass="required" />
		</e:field>
		<div class="submit">
			<e:button runat="server" text="Register" onclick="OnRegister" />
		</div>
	</fieldset>
	
	<script type="text/javascript">
		function pageInit() {
			$('.confirm').rules('add', { equalTo: '.password', messages: { equalTo: "Please enter the same password." } });
		}
	</script>
		
</asp:content>

