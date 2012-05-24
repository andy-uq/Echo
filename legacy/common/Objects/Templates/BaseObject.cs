using System;
using System.Collections.Generic;
using System.Xml;

namespace Echo.Objects.Templates
{
	public abstract class BaseTemplate<T> : ITemplate where T : IObject
	{
		private readonly ObjectFactory factory;
		private ulong? templateID;
		private string name;

		protected BaseTemplate(ObjectFactory universe)
		{
			this.factory = universe;
		}

		protected virtual string SerialiseAs
		{
			get { return typeof (T).Name.ToLower(); }
		}

		protected virtual ObjectFactory Factory
		{
			get { return this.factory; }
		}

		protected Universe Universe
		{
			get { return Factory.Universe; }
		}

		#region ITemplate Members

		public ulong TemplateID
		{
			get { return EnsureTemplateID(); }
		}

		public virtual string Name
		{
			get { return this.name; }
			set { this.name = value; }
		}

		#endregion

		protected ulong EnsureTemplateID()
		{
			if (this.templateID == null)
				this.templateID = Factory.GenerateTemplateID();

			return this.templateID.Value;
		}

		public virtual void Load(XmlElement xTemplateRoot)
		{
			XmlElement xTemplate = (xTemplateRoot.LocalName == SerialiseAs) ? xTemplateRoot : xTemplateRoot.Select(SerialiseAs);
			ReadXml(xTemplate);
		}

		public virtual XmlElement Save(XmlElement xTemplateRoot)
		{
			XmlElement xObject = xTemplateRoot.Append(this.SerialiseAs);
			WriteXml(xObject);

			return xObject;
		}

		protected virtual void ReadXml(XmlElement xTemplate)
		{
			if (xTemplate.LocalName != this.SerialiseAs)
				throw new InvalidOperationException(@"Unexpected node: ""{0}"", wanted a ""{1}""".Expand(xTemplate.LocalName, this.SerialiseAs));

			this.templateID = xTemplate.UInt64("templateID");
			this.name = xTemplate.String("name", null);

			this.factory.Register(this);
		}

		protected virtual void WriteXml(XmlElement xTemplate)
		{
			xTemplate.Attribute("templateID", TemplateID);
			xTemplate.Element("name", this.name); // we don't serialise auto names
		}

		object ICloneable.Clone()
		{
			return Clone(false);
		}

		protected virtual BaseTemplate<T> Clone(bool newID)
		{
			var clone = (BaseTemplate<T>)MemberwiseClone();
			if (newID)
				clone.templateID = Factory.GenerateTemplateID();

			return clone;
		}

	}

	public interface ITemplate : ICloneable
	{
		ulong TemplateID { get; }
		string Name { get; }
	}
}