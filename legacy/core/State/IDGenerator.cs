using System;
using common.Interfaces;

namespace core.State
{
	public class IDGenerator : IIDGenerator
	{
		private readonly static object Sync = new object();
		private long nextID = 1;

		public long GetNextID()
		{
			lock(Sync) return nextID++;
		}
	}
}