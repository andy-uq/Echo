using System;
using System.Runtime.Serialization;

namespace Echo.Exceptions
{
	[Serializable]
	public class ItemNotFoundException : Exception
	{
		public ItemNotFoundException(string itemType, object item) : base(string.Format("Could not find {0} \"{1}\"", itemType, item))
		{
		}

		protected ItemNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}