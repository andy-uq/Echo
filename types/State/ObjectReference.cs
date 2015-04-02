using System;
using System.Globalization;
using System.Text.RegularExpressions;
using EnsureThat;

namespace Echo.State
{
	[Serializable]
	public struct ObjectReference : IEquatable<ObjectReference>
	{
		private static readonly Regex _regex = new Regex(@"\[x(?<Id>[a-f0-9]{4,16})\](\s(?<Name>.*))?", RegexOptions.Compiled);

		public ulong Id { get; private set; }
		public string Name { get; private set; }

		public ObjectReference(string value) : this()
		{
			if ( value != null )
			{
				var m = _regex.Match(value);
				if ( m.Success )
				{
					Id = ulong.Parse(m.Groups["Id"].Value, NumberStyles.AllowHexSpecifier);
					Name = m.Groups["Name"].Success ? m.Groups["Name"].Value : String.Empty;
					return;
				}
			}

			throw new FormatException("Invalid object reference");
		}

		public ObjectReference(ulong id) : this()
		{
			Id = id;
		}

		public ObjectReference(ulong id, string name) : this(id)
		{
			Name = name;
		}

		public static bool TryParse(string value, out ObjectReference @ref)
		{
			if ( value != null )
			{
				var m = _regex.Match(value);
				if ( m.Success )
				{
					@ref = new ObjectReference
					{
						Id = ulong.Parse(m.Groups["Id"].Value, NumberStyles.AllowHexSpecifier),
						Name = m.Groups["Name"].Success ? m.Groups["Name"].Value : String.Empty
					};

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
}