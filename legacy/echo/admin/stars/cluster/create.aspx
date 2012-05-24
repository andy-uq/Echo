<%@ page language="C#" masterpagefile="~/Site.Master" autoeventwireup="true" codebehind="create.aspx.cs" inherits="Echo.Web.Administration.Stars.CreateStarClusterPage" %>
<%@ Register src="~/admin/stars/controls/solarsystem.ascx" tagname="solarsystem" tagprefix="echo" %>
<%@ Register src="~/admin/stars/controls/planet.ascx" tagname="planet" tagprefix="echo" %>
<%@ Register src="~/admin/stars/controls/moon.ascx" tagname="moon" tagprefix="echo" %>

<asp:content contentplaceholderid="main" runat="server">
	<e:validation runat="server" />
	
	<fieldset id="sc-details">
		<legend>Details</legend>
		<e:field runat="server" label="Name" control="txtName">
			<asp:checkbox id="chkAutoName" runat="server" text="Auto Name" cssclass="auto" checked="true" />
			<asp:textbox id="txtName" runat="server" cssclass="required" style="display: none" />
		</e:field>
		<e:field runat="server" label="Local Position" cssclass="coord">
			<asp:label runat="server" associatedcontrolid="txtX" text="X" />
			<asp:textbox id="txtX" runat="server" cssclass="required" />
			<asp:label runat="server" associatedcontrolid="txtY" text="Y" />
			<asp:textbox id="txtY" runat="server" cssclass="required" />
		</e:field>
	</fieldset>

	<div class="submit">
		<e:button runat="server" text="Create" onclick="OnCreate" />
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
		}
	</script>
	
</asp:content>

