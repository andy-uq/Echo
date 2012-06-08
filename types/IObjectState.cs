using System;

namespace Echo
{
	public interface IObjectState
	{
		long ObjectId { get; }
		string Name { get; }
	}
}