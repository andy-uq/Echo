using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Echo.State
{
	public struct ObjectReference : IEquatable<ObjectReference>
	{
		private static Regex _regex = new Regex(@"\[x(?<Id>[a-f0-9]{16})\]\s(?<Name>.*)?");

		public long Id { get; private set; }
		public string Name { get; private set; }

		public ObjectReference(long id) : this()
		{
			Id = id;
		}

		public ObjectReference(long id, string name) : this()
		{
			Id = id;
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
						Id = long.Parse(m.Groups["Id"].Value, NumberStyles.AllowHexSpecifier),
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
			if ( value != null )
			{
				var m = _regex.Match(value);
				if ( m.Success )
				{
					return new ObjectReference
					{
						Id = long.Parse(m.Groups["Id"].Value, NumberStyles.AllowHexSpecifier),
						Name = m.Groups["Name"].Success ? m.Groups["Name"].Value : String.Empty
					};
				}
			}

			throw new FormatException("Invalid object reference");
		}

		public bool Equals(ObjectReference other)
		{
			return other.Id == Id;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (obj.GetType() != typeof (ObjectReference)) return false;
			return Equals((ObjectReference) obj);
		}

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}

		public override string ToString()
		{
			return string.Format("[x{0:x16}] {1}", Id, Name).Trim();
		}
	}
}