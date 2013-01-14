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

		public TDestination Map<TDestination>(object value)
		{
			return _autoMap.Map<TDestination>(value);
		}

		public object Map(object value, Type destinationType)
		{
			return value == null 
				? null 
				: _autoMap.Map(value, value.GetType(), destinationType);
		}
	}
}