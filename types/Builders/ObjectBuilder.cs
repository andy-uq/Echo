using System.Collections.Generic;
using Echo.JumpGates;

namespace Echo
{
	namespace Builder
	{
		public abstract class ObjectBuilder
		{
			private readonly HashSet<IBuilderContext> _dependents = new HashSet<IBuilderContext>(new ResolutionContextComparer());

			private class ResolutionContextComparer : IEqualityComparer<IBuilderContext>
			{
				public bool Equals(IBuilderContext x, IBuilderContext y)
				{
					return x.Target.Id.Equals(y.Target.Id);
				}

				public int GetHashCode(IBuilderContext obj)
				{
					return obj.Target.Id.GetHashCode();
				}
			}

			public IEnumerable<IBuilderContext> DependentObjects
			{
				get { return _dependents; }
			}

			public void Add(IBuilderContext dependent)
			{
				_dependents.Remove(dependent);
				_dependents.Add(dependent);
			}
		}
	}
}