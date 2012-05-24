using System;
using System.Collections.Generic;
using System.Xml;

namespace Echo.Objects
{
	public partial class BaseObject
	{
		internal abstract class BaseObjectState<T> where T : BaseObject, new()
		{
			private readonly Universe.UniverseState universeState;

			protected BaseObjectState(Universe.UniverseState universeState)
			{
				this.universeState = universeState;
			}

			protected virtual string SerialiseAs
			{
				get
				{
					return typeof(T).Name.ToLower();
				}
			}

			protected virtual T CreateInstance(ulong objectID)
			{
				return Universe.GetObject<T>(objectID);
			}

			public virtual T Load(XmlElement xObjectRoot)
			{
				XmlElement xObject = xObjectRoot.Select(this.SerialiseAs);
				return ReadXml(xObject);
			}

			protected virtual T ReadXml(XmlElement xObject)
			{
				ulong objectID = xObject.UInt64("objectID");

				T instance = CreateInstance(objectID);
				instance.objectID = objectID;
				instance.Name = xObject.String("name", null);

                XmlElement xLocation;
				if (xObject.Select("location", out xLocation))
				{
					Universe.SetLocation(instance, xLocation.UInt64("id"));
				}
				else
				{
					Universe.SetLocation(instance, 0);
				}

				return instance;
			}
            
			public virtual XmlElement Save(T obj, XmlElement xObjectRoot)
			{
				var xObject = xObjectRoot.Append(this.SerialiseAs);

				WriteXml(obj, xObject);
				return xObject;
			}

			public void Save(IEnumerable<T> list, XmlElement xObjectRoot)
			{
				foreach (var o in list)
				{
					Save(o, xObjectRoot);
				}
			}

			public void LoadAll(IList<T> list, XmlElement xObjectRoot)
			{
				xObjectRoot.SelectNodes(this.SerialiseAs).Load(list, ReadXml);
			}

			protected virtual void WriteXml(T obj, XmlElement xObject)
			{
				if ( obj.ObjectID == 0)
					throw new InvalidOperationException("{0} is part of the universe, but does not have an ID".Expand(obj));

				xObject.Attribute("objectID", obj.ObjectID);
				xObject.Attribute("name", obj.Name);
				
				if (obj.location.ObjectType == ObjectType.Universe) // don't bother recording the location if its "in the universe" (duh)
					return;

				var xLocation = xObject.Append("location");
				xLocation.Attribute("id", obj.location.ObjectID);
				xLocation.Attribute("type", obj.location.ObjectType);
			}

			protected virtual Universe.UniverseState Universe
			{
				get { return this.universeState; }
			}
		}
	}
}