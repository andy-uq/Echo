using System.Collections.Generic;

namespace data
{
	public class Repository<T> : Ia
	{
		private List<T> contents;

		public Repository()
		{
			contents = new List<T>();
		}


	}
}