<%@ page language="C#" masterpagefile="~/Site.Master" autoeventwireup="true" codebehind="edit.aspx.cs" inherits="Echo.Web.Administration.Stars.EditStarClusterPage" %>
<%@ Register src="~/admin/stars/controls/solarsystem.ascx" tagname="solarsystem" tagprefix="echo" %>
<%@ Register src="~/admin/stars/controls/planet.ascx" tagname="planet" tagprefix="echo" %>
<%@ Register src="~/admin/stars/controls/moon.ascx" tagname="moon" tagprefix="echo" %>

<asp:content contentplaceholderid="main" runat="server">
	<e:validation runat="server" />
	<asp:customvalidator id="badPosition" runat="server" display="None" />
	
	<fieldset id="sc-details">
		<legend>Details</legend>
		<e:field runat="server" label="Name" control="txtName">
			<asp:checkbox id="chkAutoName" runat="server" text="Auto Name" cssclass="auto" checked="true" />
			<asp:textbox id="txtName" runat="server" cssclass="required" />
		</e:field>
		<e:field runat="server" label="Local Position" cssclass="coord">
			<asp:label runat="server" associatedcontrolid="txtX" text="X" />
			<asp:textbox id="txtX" runat="server" cssclass="required" />
			<asp:label runat="server" associatedcontrolid="txtY" text="Y" />
			<asp:textbox id="txtY" runat="server" cssclass="required" />
		</e:field>
	</fieldset>

	<asp:updatepanel id="_solarsystems" runat="server" childrenastriggers="true" updatemode="Conditional">
	<contenttemplate>
	<asp:repeater runat="server" datasource="<%# StarCluster.SolarSystems %>" onitemcommand="OnSolarSystemCommand">
		<headertemplate>
			<fieldset>
				<legend>Solar Systems</legend>
				<table cellspacing="0">
				<thead>
				<tr>
					<td>Name</td>
					<td>Coordinates</td>
					<td></td>
				</tr>
				</thead>
			</headertemplate>	
			<itemtemplate>
				<tr>
					<td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
					<td><%# DataBinder.Eval(Container.DataItem, "Coordinates", "{0:g}") %></td>
					<td class="details">
						<e:button runat="server" text="Details" commandname="edit" causesvalidation="false" />
						<e:button runat="server" text="Delete" commandname="delete" causesvalidation="false" />
					</td>
				</tr>
			</itemtemplate>
			<footertemplate>
			</table>
			<e:button runat="server" text="Add" onclick="OnAddSolarSystem" causesvalidation="false" />
		</fieldset>
		<echo:solarsystem id="solarsystem" runat="server" display="Popup" />
		</footertemplate>
	</asp:repeater>
	
	</contenttemplate>
	</asp:updatepanel>
	
	<asp:updatepanel id="_planets" runat="server" childrenastriggers="true" updatemode="Conditional">
	<contenttemplate>
	<asp:repeater runat="server" datasource="<%# Planets %>" onitemcommand="OnPlanetCommand">
		<headertemplate>
			<fieldset>
				<legend>Planets</legend>
				<table cellspacing="0">
				<thead>
				<tr>
					<td>Name</td>
					<td>Coordinates</td>
					<td></td>
				</tr>
				</thead>
			</headertemplate>	
			<itemtemplate>
				<tr>
					<td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
					<td><%# DataBinder.Eval(Container.DataItem, "Coordinates", "{0:g}") %></td>
					<td class="details">
						<e:button runat="server" text="Details" commandname="edit" causesvalidation="false" />
						<e:button runat="server" text="Moons" commandname="moons" causesvalidation="false" />
						<e:button runat="server" text="Delete" commandname="delete" causesvalidation="false" />
					</td>
				</tr>
			</itemtemplate>
			<footertemplate>
			</table>
			<e:button runat="server" text="Add" onclick="OnAddPlanet" causesvalidation="false" />
		</fieldset>
		<echo:planet id="planet" runat="server" />
		</footertemplate>
	</asp:repeater>
	
	</contenttemplate>
	</asp:updatepanel>

	<asp:updatepanel id="_moons" runat="server" childrenastriggers="true" updatemode="Conditional">
	<contenttemplate>
	<asp:repeater runat="server" datasource="<%# Moons %>" onitemcommand="OnMoonCommand">
		<headertemplate>
			<fieldset>
				<legend>Moons</legend>
				<table cellspacing="0">
				<thead>
				<tr>
					<td>Name</td>
					<td>Coordinates</td>
					<td></td>
				</tr>
				</thead>
			</headertemplate>	
			<itemtemplate>
				<tr>
					<td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
					<td><%# DataBinder.Eval(Container.DataItem, "Coordinates", "{0:g}") %></td>
					<td class="details">
						<e:button runat="server" text="Details" commandname="edit" causesvalidation="false" />
						<e:button runat="server" text="Delete" commandname="delete" causesvalidation="false" />
					</td>
				</tr>
			</itemtemplate>
			<footertemplate>
			</table>
			<e:button runat="server" text="Add" onclick="OnAddMoon" causesvalidation="false" />
		</fieldset>
		<echo:moon id="moon" runat="server" />
		</footertemplate>
	</asp:repeater>
	
	</contenttemplate>
	</asp:updatepanel>

	<div class="submit">
		<e:button runat="server" text="Update" onclick="OnUpdate" />
	</div>
	
	<script type="text/javascript">
		function pageInit()
		{
			$('#sc-details .auto input').click(function()
			{
				if ($(this).is(':checked'))
				{
					$('#sc-details .auto+input').hide()
				}
				else
				{
					$('#sc-details .auto+input').show();
				} 
			});

			$('#_solarsystem .auto input').click(function()
			{
				if ($(this).is(':checked'))
				{
					$('#_solarsystem .auto+input').hide()
				}
				else
				{
					$('#_solarsystem .auto+input').show();
				}
			});
		}
	</script>
	
</asp:content>

