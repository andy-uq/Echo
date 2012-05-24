using Echo.Celestial;
using Echo;

namespace Echo.Structures
{
	public abstract partial class Structure : OrbitingObject
	{
		public override ObjectType ObjectType
		{
			get { return ObjectType.Structure; }
		}

		public abstract StructureType StructureType { get; }
	}
}