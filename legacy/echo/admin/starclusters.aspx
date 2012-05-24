<%@ page subtitle="Star Systems" language="C#" masterpagefile="~/Site.Master" autoeventwireup="true" codebehind="starclusters.aspx.cs" inherits="Echo.Web.Administration.StarClustersPage" %>

<asp:content contentplaceholderid="main" runat="server">

	<asp:repeater runat="server" datasource="<%# StarClusters %>" onitemcommand="OnItemCommand">
	<headertemplate>
		<table cellspacing="0">
		<thead>
		<tr>
			<td>Name</td>
			<td>Coordinates</td>
			<td>Size</td>
			<td></td>
		</tr>
		</thead>
	</headertemplate>	
	<itemtemplate>
		<tr>
			<td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
			<td><%# DataBinder.Eval(Container.DataItem, "Coordinates", "{0:g}")%></td>
			<td><%# DataBinder.Eval(Container.DataItem, "Extent", "{0:n0}")%></td>
			<td class="details">
				<e:button runat="server" text="details" commandname="view" commandargument='<%# DataBinder.Eval(Container.DataItem, "TemplateID") %>' />
			</td>
		</tr>
	</itemtemplate>
	<footertemplate>
	</table>
	</footertemplate>
	</asp:repeater>
	
	<e:button runat="server" text="Create" navigateurl="~/admin/stars/create.aspx?mode=StarCluster" />

</asp:content>

