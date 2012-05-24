<%@ page subtitle="Personnel" language="C#" masterpagefile="~/Site.Master" autoeventwireup="true" codebehind="personnel.aspx.cs" inherits="Echo.Web.PersonnelPage" %>

<asp:content contentplaceholderid="main" runat="server">

	<asp:repeater runat="server" datasource="<%# Agents %>">
	<headertemplate>
		<table cellspacing="0">
		<thead>
		<tr>
			<td>Name</td>
			<td>Location</td>
			<td></td>
		</tr>
		</thead>
	</headertemplate>	
	<itemtemplate>
		<tr>
			<td><%# DataBinder.Eval(Container.DataItem, "Name") %></td>
			<td><%# DataBinder.Eval(Container.DataItem, "Location") %></td>
			<td class="details">
				<asp:hyperlink runat="server" text="details" navigateurl="~/members/personnel/view.aspx" />
			</td>
		</tr>
	</itemtemplate>
	<footertemplate>
	</table>
	</footertemplate>
	</asp:repeater>
	
</asp:content>

