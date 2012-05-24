namespace Echo.Mapping
{
	public interface IDependencyResolver
	{
		T Resolve<T>();
		bool TryResolve<T>(out T value);
	}
}