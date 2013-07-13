﻿using System.Collections.Generic;
using System.Collections.Immutable;
using Echo;

namespace test.common
{
	public class TestIdResolver : IdResolver
	{
		private readonly ImmutableDictionary<long, IObject> _values;

		public TestIdResolver()
		{
			_values = BuildValues().ToImmutableDictionary(k => k.Id, v => v);
		}

		public IEnumerable<IObject> BuildValues()
		{
			foreach ( var item in TestItems.Items )
				yield return item;

			foreach ( var item in TestItems.Weapons )
				yield return item;

			foreach ( var item in TestItems.BluePrints )
				yield return item;

			foreach ( var item in TestSkills.Skills )
				yield return item;
		}

		public override IEnumerable<IObject> Values
		{
			get { return _values.Values; }
		}

		protected override bool LookupValue<T>(long id, out IObject value)
		{
			return _values.TryGetValue(id, out value);
		}
	}
}