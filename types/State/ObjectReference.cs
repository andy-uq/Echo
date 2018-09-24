using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Echo.State
{
	[Serializable]
	[TypeConverter(typeof(ObjectReferenceTypeConverter))]
	public struct ObjectReference : IEquatable<ObjectReference>
	{
		private static readonly Regex _regex = new Regex(@"\[x(?<Id>[a-f0-9]{4,16})\](\s(?<Name>.*))?", RegexOptions.Compiled);

		private readonly IObjectState _state;
		private readonly ulong _id;
		private readonly string _name;

		public ulong Id
		{
			get { return _state == null ? _id : _state.ObjectId; }
		}

		public string Name
		{
			get { return _state == null ? _name : _state.Name; }
		}

		public ObjectReference(string value) : this()
		{
			if ( value != null )
			{
				var m = _regex.Match(value);
				if ( m.Success )
				{
					_id = ulong.Parse(m.Groups["Id"].Value, NumberStyles.AllowHexSpecifier);
					_name = m.Groups["Name"].Success ? m.Groups["Name"].Value : String.Empty;
					return;
				}
			}

			throw new FormatException("Invalid object reference");
		}

		public ObjectReference(IObjectState refersTo) : this()
		{
			_state = refersTo;
		}

		public ObjectReference(ulong id) : this()
		{
			_id = id;
		}

		public ObjectReference(ulong id, string name) : this(id)
		{
			_name = name;
		}

		public static bool TryParse(string value, out ObjectReference @ref)
		{
			if ( value != null )
			{
				var m = _regex.Match(value);
				if ( m.Success )
				{
					var id = ulong.Parse(m.Groups["Id"].Value, NumberStyles.AllowHexSpecifier);
					var name = m.Groups["Name"].Success ? m.Groups["Name"].Value : String.Empty;
					@ref = new ObjectReference(id, name);

					return true;
				}
			}

			@ref = new ObjectReference();
			return false;
		}

		public static ObjectReference Parse(string value)
		{
			return new ObjectReference(value);
		}

		public static bool operator ==(ObjectReference left, ObjectReference right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(ObjectReference left, ObjectReference right)
		{
			return !left.Equals(right);
		}

		public bool Equals(ObjectReference other)
		{
			return Id == other.Id;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			return obj is ObjectReference && Equals((ObjectReference) obj);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("[x{0:x8}] {1}", Id, Name).Trim();
		}
	}

	public static class ObjectReferenceExtensions
	{
		public static ObjectReference? AsObjectReference(this IObject @object)
		{
			if (@object == null)
				return null;

			return new ObjectReference(@object.Id, @object.Name);
		}

		public static ObjectReference? AsObjectReference(this IObjectState state)
		{
			if (state == null)
				return null;

			return new ObjectReference(state);
		}

		public static ObjectReference ToObjectReference(this IObject @object)
		{
			if (@object == null) throw new ArgumentNullException(nameof(@object));
			return new ObjectReference(@object.Id, @object.Name);
		}

		public static ObjectReference ToObjectReference(this IObjectState state)
		{
			if (state == null) throw new ArgumentNullException(nameof(state));
			return new ObjectReference(state);
		}
	}


	public class ObjectReferenceTypeConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof (string))
			{
				return true;
			}

			return base.CanConvertFrom(context, sourceType);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			var s = value as string;
			return s == null 
				? base.ConvertFrom(context, culture, value) 
				: ObjectReference.Parse(s);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			return destinationType == typeof(string) 
				? ((ObjectReference) value).ToString() 
				: base.ConvertTo(context, culture, value, destinationType);
		}
	}
}