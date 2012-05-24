using System;
using AutoMapper;
using Echo.Mapping;

namespace Echo
{
	public class AutoMapTypeMapper : ITypeMapper
	{
		private readonly IMappingEngine _autoMap;

		public AutoMapTypeMapper(IMappingEngine autoMap)
		{
			_autoMap = autoMap;
		}

		public TDestination Map<TSource, TDestination>(TSource value)
		{
			return _autoMap.Map<TSource, TDestination>(value);
		}

		public object Map(object value, Type destinationType)
		{
			if (value == null)
				return null;

			return _autoMap.Map(value, value.GetType(), destinationType);
		}
	}
}