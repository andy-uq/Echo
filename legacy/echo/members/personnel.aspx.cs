using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Echo.Entities;
using Echo.Web.Controls.Base;

namespace Echo.Web
{
	public partial class PersonnelPage : EchoPage, IPersonnelPage
	{
		protected IReadOnlyList<Agent> Agents
		{
			get
			{
				var player = Global.Universe.Players.Find(p => p.Username == User.Identity.Name);
				return player.Employees;
			}
		}
	}
}
