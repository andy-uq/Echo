using System;
using System.Collections.Generic;
using System.Diagnostics;

using Echo.Entities;
using Echo.Objects;

using Ubiquity.u2ool.Collections;

namespace Echo
{
	public class ItemCollection<T> : BaseCollection<T> where T : IItem
	{
		public ItemCollection()
		{
		}

		public ItemCollection(IEnumerable<T> collection)
		{
			ListUtility.ForEach(collection, Add);
		}

		#region IList<T> Members

		/// <summary>Insert the item at a specific location.  Stacks the item if possible</summary>
		public override void Add(T item)
		{
			if (Exists(i => i.ObjectID == item.ObjectID))
				throw new InvalidOperationException("The collection already contains this item");

			if ( item.Stackable )
			{
				IItem existingItem = Find(i => CanStack(i, item));
				if (existingItem != null)
				{
					existingItem.Stack(item);
					return;
				}
			}

			base.Add(item);
		}

		public override void Insert(int index, T item)
		{
			if ( Exists(i => i.ObjectID == item.ObjectID) )
				throw new InvalidOperationException("The collection already contains this item");
			
			base.Insert(index, item);
		}

		private static bool CanStack(T item1, T item2)
		{
			if (item1.ItemID != item2.ItemID)
				return false;

			if (item1.Location != item2.Location)
				return false;

			if (item1.Stackable == false || item2.Stackable == false)
				return false;

			if (item1.Owner != item2.Owner)
				return false;

			return true;
		}

		public override T this[int index]
		{
			get { return base[index]; }
			set { throw new NotSupportedException(); }
		}

		#endregion
	}
}