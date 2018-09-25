using System;
using System.Collections.Generic;
using System.Linq;
using Echo.Agents.Skills;
using Echo.Builder;
using Echo.Items;
using Echo.State;
using Echo.Statistics;
using test.common;

namespace Echo.Tests
{
	public static class TestExtensions
	{
		public static IEnumerable<Item> Build(this IEnumerable<ItemState> items)
		{
			var resolver = IdResolver.Empty.RegisterTestItems();
			return items.Select(i => Item.Builder.Build(i).Build(resolver));
		}
		
		public static T Materialise<T>(this ObjectBuilder<T> build) where T : IObject
		{
			var collection = new HashSet<IObject>(build.FlattenObjectTree(), new ObjectEqualityComparer());
			var idResolver = new IdResolutionContext(collection).RegisterTestItems().RegisterTestSkills();

			return build.Build(idResolver);
		}

		public static T Materialise<T>(this ObjectBuilder<T> build, IIdResolver resolver) where T : IObject
		{
			return build.Build(resolver);
		}

		public static IEnumerable<IObject> Materialise(this IEnumerable<IBuilderContext> objects)
		{
			objects = objects.ToArray();

			var collection = objects.SelectMany(x => x.FlattenObjectTree());
			var idResolver = new IdResolutionContext(collection);

			return objects.Select(builder => builder.Build(idResolver));
		}

		public static SkillInfo ToInfo(this SkillCode skillCode) => TestSkills.For(skillCode);
	}
}