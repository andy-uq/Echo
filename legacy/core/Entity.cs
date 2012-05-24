using System;
using common.Interfaces;
using core.State;

namespace core
{
	public class Entity : IEntity
	{
		private readonly IIDGenerator _idGenerator;
		private readonly IIDGenerator idGenerator;

		public long ID
		{
			get; private set;
		}

		public Entity() : this( ServiceLocator.Get<IIDGenerator>() )
		{
		}

		public Entity(IIDGenerator idGenerator)
		{
			_idGenerator = idGenerator;

			ID = idGenerator.GetNextID();
		}
	}
}