using Echo.Tasks;

namespace Echo.Structures
{
	public partial class Manufactory : Structure
	{
		public double Efficiency { get; set; }
		public override StructureType StructureType { get { return StructureType.Manufactory; }}
	}
}