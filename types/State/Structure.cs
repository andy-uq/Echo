using Echo.Structures;
using Echo;

namespace Echo.State
{
	public class StructureState
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public Vector LocalCoordinates { get; set; }
		public long OrbitsId { get; set; }

		public StructureType StructureType { get; set; }
		public ManufactoryState Manufactory { get; set; }
	}
}