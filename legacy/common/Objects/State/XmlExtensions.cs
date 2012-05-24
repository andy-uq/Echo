using System;
using System.Collections.Generic;
using System.Xml;

using Ubiquity.u2ool;

namespace Echo.Objects
{
	public static class XmlExtensions
	{
		#region Delegates

		public delegate T LoadDelegate<T>(XmlElement xObject) where T : IObject;

		#endregion

		public static void ForEach(this XmlNodeList nodeList, Action<XmlElement> action)
		{
			foreach (XmlElement element in nodeList)
				action(element);
		}

		public static void Load<T>(this XmlNodeList nodeList, IList<T> list, LoadDelegate<T> loader) where T : IObject
		{
			foreach (XmlElement element in nodeList)
			{
				T value = loader(element);
				list.Add(value);
			}
		}

		/// <summary>Selects the root node.  Throws an exception if the root node could not be found</summary>
		public static XmlElement SelectRootElement(this XmlDocument xdoc)
		{
			XmlNode node = xdoc.FirstChild;
			while (node != null)
			{
				if (node is XmlElement)
				{
					return (XmlElement) node;
				}

				node = node.NextSibling;
			}

			throw new XmlException(string.Format("This document does not appear to have a root node"));
		}

		/// <summary>Evaluates an xpath expression to select a single node, throws an XmlException if the node could not be found or was not an element</summary>
		public static XmlElement Select(this XmlDocument xdoc, string xpath)
		{
			var xRoot = xdoc.SelectSingleNode(xpath) as XmlElement;
			if (xRoot == null)
			{
				throw new XmlException(string.Format(@"The xpath expression ""{0}"" did not return a node", xpath));
			}

			return xRoot;
		}

		/// <summary>Evaluates an xpath expression to select a single node, throws an XmlException if the node could not be found</summary>
		public static XmlElement Select(this XmlElement element, string xpath)
		{
			XmlElement child;

			if (Select(element, xpath, out child))
			{
				return child;
			}

			throw new XmlException(string.Format(@"The xpath expression ""{0}"" did not return a node from ""{1}""", xpath, element.Name));
		}

		/// <summary>Selects the inner text from evaluating an xpath expression that returns a single node.  Returns null if the xpath did not select a node.</summary>
		public static string SelectText(this XmlElement element, string xpath)
		{
			XmlElement child;

			if (Select(element, xpath, out child))
			{
				return child.InnerText;
			}

			return null;
		}

		/// <summary>Trys to evaluate an xpath expression to select a single node, returns false if the node could not be found</summary>
		public static bool Select(this XmlElement element, string xpath, out XmlElement result)
		{
			result = element.SelectSingleNode(xpath) as XmlElement;
			return (result != null);
		}

		/// <summary>Returns the raw value of an attribute, throws an XmlException if the attribute was not present</summary>
		public static string String(this XmlElement element, string name)
		{
			string value = String(element, name, null);
			if (value == null)
			{
				throw new XmlException(string.Format(@"The attribute ""{0}"" could not be found on node ""{1}""", name, element.Name));
			}

			return value;
		}

		/// <summary>Returns the enumeration value of an attribute, throws an XmlException if the attribute was not present</summary>
		/// <typeparam name="T">Enum type</typeparam>
		public static T Enum<T>(this XmlElement element, string name)
		{
			string value = String(element, name);
			return EnumHelper.Parse<T>(value);
		}

		/// <summary>Returns the enumeration value of an attribute, returns defaultValue if the attribute was not present</summary>
		/// <typeparam name="T">Enum type</typeparam>
		public static T Enum<T>(this XmlElement element, string name, T defaultValue)
		{
			string value = String(element, name, null);
			return (value == null)
			       	? defaultValue
			       	: EnumHelper.Parse<T>(value);
		}

		/// <summary>Returns the enumeration value of an attribute, returns defaultValue if the attribute was not present</summary>
		/// <typeparam name="T">Enum type</typeparam>
		public static T? Enum<T>(this XmlElement element, string name, bool required) where T : struct
		{
			string value = required
			               	? String(element, name)
			               	: String(element, name, null);
			if (value == null)
			{
				return null;
			}

			return EnumHelper.Parse<T>(value);
		}

		/// <summary>Returns the date/time value of an attribute, throws an XmlException if the attribute was not present</summary>
		public static DateTime DateTime(this XmlElement element, string name)
		{
			string value = String(element, name);
			return System.DateTime.Parse(value);
		}

		/// <summary>Returns the boolean value of an attribute, throws an XmlException if the attribute was not present</summary>
		public static bool Boolean(this XmlElement element, string name)
		{
			string value = String(element, name);
			return bool.Parse(value);
		}

		/// <summary>Returns the integer value of an attribute, throws an XmlException if the attribute was not present</summary>
		public static int Int32(this XmlElement element, string name)
		{
			string value = String(element, name);
			return int.Parse(value);
		}

		/// <summary>Returns the integer value of an attribute, returns null if the attribute was not present and required was set to false</summary>
		public static int? Int32(this XmlElement element, string name, bool required)
		{
			string value;

			if (required)
			{
				value = String(element, name);
			}
			else
			{
				value = String(element, name, null);
				if (value == null)
				{
					return null;
				}
			}

			return int.Parse(value);
		}

		/// <summary>Returns the integer value of an attribute, returns defaultValue if the attribute was not present</summary>
		public static int Int32(this XmlElement element, string name, int defaultValue)
		{
			XmlAttribute xAttr = element.Attributes[name];
			return (xAttr == null)
			       	? defaultValue
			       	: int.Parse(xAttr.Value);
		}

		/// <summary>Returns the unsigned integer value of an attribute, throws an XmlException if the attribute was not present</summary>
		public static uint UInt32(this XmlElement element, string name)
		{
			string value = String(element, name);
			return uint.Parse(value);
		}

		/// <summary>Returns the integer value of an attribute, returns defaultValue if the attribute was not present</summary>
		public static uint UInt32(this XmlElement element, string name, uint defaultValue)
		{
			XmlAttribute xAttr = element.Attributes[name];
			return (xAttr == null)
			       	? defaultValue
			       	: uint.Parse(xAttr.Value);
		}

		/// <summary>Returns the unsigned integer value of an attribute, returns null if the attribute was not present and required was set to false</summary>
		public static uint? UInt32(this XmlElement element, string name, bool required)
		{
			string value;

			if (required)
			{
				value = String(element, name);
			}
			else
			{
				value = String(element, name, null);
				if (value == null)
				{
					return null;
				}
			}

			return uint.Parse(value);
		}

		/// <summary>Returns the unsigned integer value of an attribute, throws an XmlException if the attribute was not present</summary>
		public static double Double(this XmlElement element, string name)
		{
			string value = String(element, name);
			return double.Parse(value);
		}

		/// <summary>Returns the integer value of an attribute, returns defaultValue if the attribute was not present</summary>
		public static double Double(this XmlElement element, string name, double defaultValue)
		{
			XmlAttribute xAttr = element.Attributes[name];
			return (xAttr == null)
			       	? defaultValue
			       	: double.Parse(xAttr.Value);
		}

		/// <summary>Returns the unsigned integer value of an attribute, returns null if the attribute was not present and required was set to false</summary>
		public static double? Double(this XmlElement element, string name, bool required)
		{
			string value;

			if (required)
			{
				value = String(element, name);
			}
			else
			{
				value = String(element, name, null);
				if (value == null)
				{
					return null;
				}
			}

			return double.Parse(value);
		}

		/// <summary>Returns the unsigned integer value of an attribute, throws an XmlException if the attribute was not present</summary>
		public static ulong UInt64(this XmlElement element, string name)
		{
			string value = String(element, name);
			return ulong.Parse(value);
		}

		/// <summary>Returns the integer value of an attribute, returns defaultValue if the attribute was not present</summary>
		public static ulong UInt64(this XmlElement element, string name, ulong defaultValue)
		{
			XmlAttribute xAttr = element.Attributes[name];
			return (xAttr == null)
			       	? defaultValue
			       	: ulong.Parse(xAttr.Value);
		}

		/// <summary>Returns the unsigned integer value of an attribute, returns null if the attribute was not present and required was set to false</summary>
		public static ulong? UInt64(this XmlElement element, string name, bool required)
		{
			string value;

			if (required)
			{
				value = String(element, name);
			}
			else
			{
				value = String(element, name, null);
				if (value == null)
				{
					return null;
				}
			}

			return ulong.Parse(value);
		}

		/// <summary>Returns the date/time value of an attribute, returns null if the attribute was not present and required was set to false</summary>
		public static DateTime? DateTime(this XmlElement element, string name, bool required)
		{
			string value;

			if (required)
			{
				value = String(element, name);
			}
			else
			{
				value = String(element, name, null);
				if (value == null)
				{
					return null;
				}
			}

			return System.DateTime.Parse(value);
		}

		/// <summary>Returns the raw value of an attribute, returns null if the attribute was not present</summary>
		public static string String(this XmlElement element, string name, string defaultValue)
		{
			XmlAttribute xAttr = element.Attributes[name];
			if (xAttr != null)
			{
				return xAttr.Value;
			}

			XmlElement xChild;
			return Select(element, name, out xChild)
			       	? xChild.InnerText
			       	: defaultValue;
		}

		/// <summary>Returns the boolean value of an attribute, returns defaultValue if the attribute was not present</summary>
		public static bool Boolean(this XmlElement element, string name, bool defaultValue)
		{
			XmlAttribute xAttr = element.Attributes[name];
			return (xAttr == null)
			       	? defaultValue
			       	: bool.Parse(xAttr.Value);
		}

		/// <summary>Returns the date/time value of an attribute, returns defaultValue if the attribute was not present</summary>
		public static DateTime DateTime(this XmlElement element, string name, DateTime defaultValue)
		{
			XmlAttribute xAttr = element.Attributes[name];
			return (xAttr == null)
			       	? defaultValue
			       	: System.DateTime.Parse(xAttr.Value);
		}

		/// <summary>Creates and appends a new element to the parentNode</summary>
		public static XmlElement Append(this XmlElement parentNode, string name)
		{
			XmlDocument xdoc = parentNode.OwnerDocument;
			XmlElement element = xdoc.CreateElement(name);
			parentNode.AppendChild(element);

			return element;
		}

		/// <summary>Creates and appends a new element to the parentNode, and sets the new child element's innerText property</summary>
		public static XmlElement Append(this XmlElement parentNode, string name, string value)
		{
			XmlDocument xdoc = parentNode.OwnerDocument;
			XmlElement element = xdoc.CreateElement(name);
			parentNode.AppendChild(element);
			element.InnerText = value;

			return element;
		}

		/// <summary>Adds a named attribute to the given node if value is not null</summary>
		public static XmlElement Attribute(this XmlElement element, string name, object value)
		{
			if (value == null)
			{
				return element;
			}

			XmlDocument xdoc = element.OwnerDocument;
			XmlAttribute xAttr = xdoc.CreateAttribute(name);
			element.Attributes.Append(xAttr);

			if (value is double)
			{
				xAttr.Value = ((double) value).ToString("n4");
			}
			else
			{
				xAttr.Value = value.ToString();
			}

			return element;
		}

		/// <summary>Adds a named attribute to the given node if value is not null.  If there is no time component, it is not persisted</summary>
		public static XmlElement Attribute(this XmlElement element, string name, DateTime? value)
		{
			if (value != null)
			{
				return Attribute(element, name, value.Value);
			}

			return element;
		}

		/// <summary>Adds a named attribute to the given node</summary>
		private static XmlElement Attribute(XmlElement element, string name, DateTime value)
		{
			XmlDocument xdoc = element.OwnerDocument;
			XmlAttribute xAttr = xdoc.CreateAttribute(name);
			element.Attributes.Append(xAttr);

			xAttr.Value = (value.TimeOfDay == TimeSpan.Zero)
			              	? value.ToString("MMM d, yyyy")
			              	: value.ToString("MMM d, yyyy HH:mm:ss");
			return element;
		}

		/// <summary>Adds a named element to the given node if value is not null</summary>
		public static void Element(this XmlElement element, string name, object value)
		{
			if (value == null)
			{
				return;
			}

			string s;

			if (value is double)
			{
				s = ((double) value).ToString("n4");
			}
			else
			{
				s = value.ToString();
			}

			Append(element, name, s);
		}

		public static void Element(this XmlElement element, string name, IObject obj)
		{
			XmlElement xChild = Append(element, name);
			xChild.Attribute("objID", obj.ObjectID);
			xChild.Attribute("objType", obj.ObjectType);
		}

		public static void Element(this XmlElement element, string name, DateTime value)
		{
			if (value.TimeOfDay == TimeSpan.Zero)
			{
				Append(element, name, value.ToString("MMM d, yyyy"));
			}
			else
			{
				Append(element, name, value.ToString("MMM d, yyyy HH:mm:ss"));
			}
		}

		/// <summary>Adds a named attribute to the given node if value is not null.  If there is no time component, it is not persisted</summary>
		public static void Element(this XmlElement element, string name, DateTime? value)
		{
			if (value != null)
			{
				Element(element, name, value.Value);
			}
		}

		/// <summary>Returns the guid value of an attribute, returns defaultValue if the attribute was not present</summary>
		public static Guid Guid(this XmlElement element, string name, Guid defaultValue)
		{
			string value = String(element, name, null);
			if (value == null)
			{
				return defaultValue;
			}

			return GuidHelper.StringToGuid(value);
		}

		/// <summary>Returns the guid value of an attribute, returns null if the attribute was not present and required was set to false</summary>
		public static Guid? Guid(this XmlElement element, string name, bool required)
		{
			if (required)
			{
				return Guid(element, name);
			}

			string value = String(element, name, null);
			if (value == null)
			{
				return null;
			}

			return GuidHelper.StringToGuid(value);
		}

		/// <summary>Returns the guid value of an attribute, throws an XmlException if the attribute was not present</summary>
		public static Guid Guid(this XmlElement element, string name)
		{
			string value = String(element, name);
			return GuidHelper.StringToGuid(value);
		}
	}
}