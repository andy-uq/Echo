using Echo.Builder;
using Echo.State;

namespace Echo.Corporations
{
	partial class Corporation
	{
		public static class Builder
		{
			public static ObjectBuilder<Corporation> Build(CorporationState state)
			{
				var corporation = new Corporation
				{
					Id = state.ObjectId,
					Name = state.Name,
				};

				return new ObjectBuilder<Corporation>(corporation);
			}
		}
	}
}