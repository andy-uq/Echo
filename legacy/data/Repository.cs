using System.Collections.Generic;

namespace data
{
	public class Repository<T> 
	{
		private List<T> contents;

		public Repository()
		{
			contents = new List<T>();
		}


	}
}