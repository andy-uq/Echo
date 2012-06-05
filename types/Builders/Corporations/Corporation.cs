using Echo.State;

namespace Echo.Corporations
{
	partial class Corporation
	{
		public static class Builder
		{
			public static Corporation Build(CorporationState state)
			{
				return new Corporation
				{
					Id = state.Id,
					Name = state.Name,
				};
			}
		}
	}
}