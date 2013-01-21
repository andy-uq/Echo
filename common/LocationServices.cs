using System;

namespace Echo
{
	public class LocationServices : ILocationServices
	{
		private readonly IRandom _random;

		public LocationServices(IRandom random)
		{
			_random = random;
		}

		public Vector GetExitPosition(ILocation location)
		{
			var xDelta = _random.GetNext();
			var yDelta = _random.GetNext();

			var origin = location.Position.LocalCoordinates;
			Func<double, double, double> translate = (x1, xD) => (x1 + 5d*(xD - 0.5d));
			return new Vector(translate(origin.X, xDelta), translate(origin.Y, yDelta), origin.Z);
		}
	}

	public interface ILocationServices
	{
		Vector GetExitPosition(ILocation location);
	}
}