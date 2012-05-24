<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="solarsystem.ascx.cs" Inherits="Echo.Web.Administration.Stars.SolarSystemDetails" %>
	<fieldset>
		<legend>Details</legend>
		<e:field runat="server" label="Name" control="txtName">
			<asp:checkbox id="chkAutoName" runat="server" text="Auto Name" cssclass="auto" checked="true" />
			<asp:textbox id="txtName" runat="server" cssclass="required" style="display: none" />
		</e:field>
		<e:field runat="server" label="Local Position" cssclass="coord">
			<asp:label id="Label1" runat="server" associatedcontrolid="txtX" text="X" />
			<asp:textbox id="txtX" runat="server" cssclass="required" />
			<asp:label id="Label2" runat="server" associatedcontrolid="txtY" text="Y" />
			<asp:textbox id="txtY" runat="server" cssclass="required" />
		</e:field>
		<e:panel id="btnUpdate" runat="server" cssclass="submit">
			<e:button runat="server" text="Update" commandname="update" oncommand="OnUpdate"  />
		</e:panel>
	</fieldset>
