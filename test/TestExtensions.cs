namespace Echo.Tests
{
	public static class TestExtensions
	{
		public static T Materialise<T>(this ObjectBuilder<T> build) where T : IObject
		{
			var collection = build.FlattenObjectTree();
			var idResolver = new IdResolutionContext(collection);

			return build.Resolve(idResolver);
		}
	}
}