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
			double Translate(double x1, double xD) => (x1 + 5d * (xD - 0.5d));
			
			var xDelta = _random.GetNext();
			var yDelta = _random.GetNext();

			var origin = location.Position.LocalCoordinates;
			return new Vector(Translate(origin.X, xDelta), Translate(origin.Y, yDelta), origin.Z);
		}
	}

	public interface ILocationServices
	{
		Vector GetExitPosition(ILocation location);
	}
}