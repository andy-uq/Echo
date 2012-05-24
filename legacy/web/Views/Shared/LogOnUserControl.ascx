<%@ control language="C#" inherits="System.Web.Mvc.ViewUserControl" %>
<ul>
<%
	if ( Request.IsAuthenticated )
	{
%>
	<li class="username"><%= Html.Encode(Page.User.Identity.Name) %></li>
	<li><%= Html.ActionLink("My Settings", "Settings", "Account") %></li>
	<li><%= Html.ActionLink("Logout", "LogOff", "Account") %></li>
<%
	}
	else
	{
%>
	<li><%= Html.ActionLink("Login", "LogOn", "Account") %></li>
<%
	}
%>
</ul>
