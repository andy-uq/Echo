using System.Collections.Generic;

namespace Echo
{
	public delegate TDependent ChainedResolveHandler<in T, TDependent>(IIdResolver idResolver, T target, TDependent dependentObject)
		where T : IObject
		where TDependent : IObject;

	public delegate void ResolveHandler<in T, in TDependent>(IIdResolver idResolver, T target, TDependent dependentObject)
		where T : IObject
		where TDependent : IObject;

	public delegate void ResolveHandler<in T>(IIdResolver idResolver, T target)
		where T : IObject;

	public interface IBuilderContext
	{
		IObject Target { get; }
		IEnumerable<IBuilderContext> DependentObjects { get; }
		IObject Build(IIdResolver idResolver);
	}
}