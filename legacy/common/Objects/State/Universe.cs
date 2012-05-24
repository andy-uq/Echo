using System;
using System.Collections.Generic;
using System.Xml;

using Echo.Entities;

namespace Echo.Objects
{
	public partial class Universe
	{
		internal class UniverseState : BaseObjectState<Universe>
		{
			private readonly Universe universe;
			private Dictionary<ulong, IObject> objectgraph;
			private Dictionary<IObject, ulong> locationRemap;

			public UniverseState(Universe universe) : base(null)
			{
				this.universe = universe;
			}

			protected override Universe CreateInstance(ulong objectID)
			{
				return this.universe;
			}

			protected override UniverseState Universe
			{
				get { return this; }
			}

			public XmlElement Save(Universe obj)
			{
				var xdoc = new XmlDocument();
				var xUniverse = xdoc.CreateElement(SerialiseAs);

				WriteXml(obj, xUniverse);
				SaveCorporations(xUniverse);

				return xUniverse;
			}

			public override Universe Load(XmlElement xObjectRoot)
			{
				this.objectgraph = new Dictionary<ulong, IObject>();
				this.locationRemap = new Dictionary<IObject, ulong>();

				ReadXml(xObjectRoot);

				foreach (var o in locationRemap)
                {
                	ILocation location = null;

                	IObject obj;
                	if ( (o.Value != 0) && this.objectgraph.TryGetValue(o.Value, out obj) )
						location = obj as ILocation;

                	o.Key.Location = location ?? this.universe;
                }

				return this.universe;
			}

			protected override Universe ReadXml(XmlElement xUniverse)
			{
				base.ReadXml(xUniverse);

				this.universe.nextObjectID = ulong.Parse(xUniverse.String("nextObjectId"));
				this.universe.currentTick = ulong.Parse(xUniverse.String("currentTick"));
				
				LoadCorporations(xUniverse);

				return this.universe;
			}

			protected override void WriteXml(Universe obj, XmlElement xUniverse)
			{
				xUniverse.Attribute("objectID", obj.ObjectID);
				xUniverse.Attribute("name", obj.Name);

				xUniverse.Attribute("nextObjectID", obj.nextObjectID);
				xUniverse.Attribute("currentTick", obj.currentTick);
			}

			private void LoadCorporations(XmlElement xUniverse)
			{
				this.universe.corporations.Clear();

				var players = new List<PlayerCorporation>();
				var xCorporations = xUniverse.Select("corporations");

				var playerState = new PlayerCorporation.PlayerCorporationState(this);
				playerState.LoadAll(players, xCorporations);
				players.ForEach(this.universe.corporations.Add);

				var corporationState = new Corporation.CorporationState(this);
				corporationState.LoadAll(this.universe.corporations, xCorporations);
			}

			private void SaveCorporations(XmlElement xUniverse)
			{
				var xCorporations = xUniverse.Append("corporations");

				var playerState = new PlayerCorporation.PlayerCorporationState(this);
				playerState.Save(this.universe.Players, xCorporations);

				var corporationState = new Corporation.CorporationState(this);
				corporationState.Save(this.universe.corporations.FindAll(c => c.IsPlayer == false), xCorporations);
			}

			public void Register(IObject instance)
			{

				this.objectgraph.Add(instance.ObjectID, instance);
			}

			public T GetObject<T>(ulong objectID) where T : IObject, new()
			{
				IObject alreadyLoaded;
				if (this.objectgraph.TryGetValue(objectID, out alreadyLoaded))
					return (T) alreadyLoaded;

				var newObject = new T();
				this.objectgraph.Add(objectID, newObject);

				return newObject;
			}

			public T GetObject<T>(XmlElement xObject, string name) where T : IObject, new()
			{
				XmlElement xChild = xObject.Select(name);
				ulong objectID = xChild.UInt64("objID");

				var objType = xChild.Enum<ObjectType>("objType");
				var value = GetObject<T>(objectID);

                if (value.ObjectType != objType)
					throw new ApplicationException("Type mismatch between {0} and {1}".Expand(objType, value.ObjectType));

				return value;
			}

			public void SetLocation(IObject instance, ulong locationID)
			{
				this.locationRemap.Add(instance, locationID);
			}
		}

		public XmlElement SaveState()
		{
            var state = new UniverseState(this);
			return state.Save(this);
		}

		public void LoadState(XmlElement xUniverse)
		{
			var state = new UniverseState(this);
			state.Load(xUniverse);
		}
	}
}