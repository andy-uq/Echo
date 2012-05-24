<%@ page language="C#" masterpagefile="~/Site.Master" autoeventwireup="true" codebehind="moon.aspx.cs" inherits="Echo.Web.Administration.Stars.EditMoonPage" %>
<%@ Register src="~/admin/stars/controls/solarsystem.ascx" tagname="solarsystem" tagprefix="echo" %>
<%@ Register src="~/admin/stars/controls/planet.ascx" tagname="planet" tagprefix="echo" %>
<%@ Register src="~/admin/stars/controls/moon.ascx" tagname="moon" tagprefix="echo" %>

<asp:content contentplaceholderid="main" runat="server">
	<e:validation runat="server" />
	<asp:customvalidator id="badPosition" runat="server" display="None" />
	
	<echo:solarsystem runat="server" display="Inline" />
	<echo:planet runat="server" />
	<echo:moon runat="server" />

	<div class="submit">
		<e:button runat="server" text="Update" onclick="OnUpdate" />
	</div>	
</asp:content>

