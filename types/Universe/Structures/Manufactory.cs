﻿namespace Echo.Structures
{
	public partial class Manufactory : Structure
	{
		public double Efficiency { get; set; }
		public override StructureType StructureType => StructureType.Manufactory;
	}
}