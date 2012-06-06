namespace Echo
{
	public interface IIdResolver
	{
		T GetById<T>(long id) where T : class, IObject;
		bool TryGetById<T>(long id, out T value) where T : class, IObject;
	}
}