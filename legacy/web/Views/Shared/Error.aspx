<%@ page language="C#" masterpagefile="~/Views/Shared/Site.Master" inherits="System.Web.Mvc.ViewPage<System.Web.Mvc.HandleErrorInfo>" %>

<asp:content id="errorTitle" contentplaceholderid="TitleContent" runat="server">
	Error
</asp:content>

<asp:content id="errorContent" contentplaceholderid="MainContent" runat="server">
	<h2>Sorry, an error occurred while processing your request. </h2>
</asp:content>

