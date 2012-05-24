using System;

namespace Echo.Mapping
{
	public interface ITypeMapper
	{
		TDestination Map<TSource, TDestination>(TSource value);
		object Map(object value, Type destinationType);
	}
}