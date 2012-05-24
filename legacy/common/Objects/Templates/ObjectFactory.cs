using System;
using System.Collections.Generic;
using System.Xml;

using Echo.Entities;
using Echo.Objects.Templates;

namespace Echo.Objects
{
	public class ObjectFactory
	{
		private readonly Universe universe;
		private readonly object syncRoot;
		private ulong nextTemplateID;
		private Dictionary<ulong, ITemplate> lookup;

		public ObjectFactory(Universe universe)
		{
			this.syncRoot = new object();
			this.universe = universe;
			this.nextTemplateID = 0;

			this.lookup = new Dictionary<ulong, ITemplate>();

			Agent = new Agent.Template(this);
			NpcCorporation = new Corporation.Template(this);
			Player = new PlayerCorporation.PlayerTemplate(this);

			StarClusters = new List<StarCluster.Template>();
		}

		public Corporation.Template NpcCorporation { get; private set; }
		public PlayerCorporation.PlayerTemplate Player { get; private set; }
		public Agent.Template Agent { get; private set; }

		public Universe Universe
		{
			get { return this.universe; }
		}

		public List<StarCluster.Template> StarClusters { get; private set; }

		public void Load(XmlElement xUniverse)
		{
			this.nextTemplateID = xUniverse.UInt64("nextTemplateID");

			Player.Load(xUniverse);
			NpcCorporation.Load(xUniverse);
			Agent.Load(xUniverse);

			StarClusters.Clear();
			
			xUniverse.SelectNodes("starclusters/starcluster").
				ForEach(LoadStarCluster);
		}

		private void LoadStarCluster(XmlElement xStarCluster)
		{
			var starCluster = new StarCluster.Template(this);
			starCluster.Load(xStarCluster);
			StarClusters.Add(starCluster);
		}

		public XmlElement Save()
		{
			var xdoc = new XmlDocument();
			var xUniverse = xdoc.CreateElement("universe");

			xUniverse.Attribute("nextTemplateID", this.nextTemplateID);

			Player.Save(xUniverse);
			NpcCorporation.Save(xUniverse);
			Agent.Save(xUniverse);

			var xStarClusters = xUniverse.Append("starclusters");
			StarClusters.ForEach(sc => sc.Save(xStarClusters));

			return xUniverse;
		}

		public ulong GenerateTemplateID()
		{
			lock (this.syncRoot)
			{
				ulong result = this.nextTemplateID;
				this.nextTemplateID++;
				return result;
			}
		}

		public void Register(ITemplate template)
		{
			this.lookup.Add(template.TemplateID, template);
		}

		public T GetTemplate<T>(ulong templateID) where T : ITemplate
		{
			ITemplate value;
			if (this.lookup.TryGetValue(templateID, out value))
				return (T) value;

			throw new ArgumentException("Template {0} was not found".Expand(templateID));
		}
	}
}