<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="moon.ascx.cs" Inherits="Echo.Web.Administration.Stars.MoonDetails" %>
	<e:popup id="_moon" runat="server">
	<fieldset>
		<legend>Details</legend>
		<e:field runat="server" label="Name" control="txtName">
			<asp:checkbox id="chkAutoName" runat="server" text="Auto Name" cssclass="auto" checked="true" />
			<asp:textbox id="txtName" runat="server" cssclass="required" style="display: none" />
		</e:field>
		<e:field runat="server" label="Size" control="txtName">
			<asp:textbox id="txtSize" runat="server" cssclass="required" style="display: none" /> x Moon
		</e:field>		
		<e:field runat="server" label="Local Position" cssclass="coord">
			<asp:label runat="server" associatedcontrolid="txtX" text="X" />
			<asp:textbox id="txtX" runat="server" cssclass="required" />
			<asp:label runat="server" associatedcontrolid="txtY" text="Y" />
			<asp:textbox id="txtY" runat="server" cssclass="required" />
		</e:field>
		<div class="submit">
			<e:button id="btnMoon" runat="server" text="Update" commandname="update" oncommand="OnUpdate"  />
		</div>
	</fieldset>
	</e:popup>
