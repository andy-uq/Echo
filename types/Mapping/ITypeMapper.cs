using System;

namespace Echo.Mapping
{
	public interface ITypeMapper
	{
		TDestination Map<TDestination>(object value);
		object Map(object value, Type destinationType);
	}
}