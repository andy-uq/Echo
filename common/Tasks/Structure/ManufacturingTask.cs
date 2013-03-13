using Echo.Items;
using Echo.Structures;
using EnsureThat;

namespace Echo.Tasks.Structure
{
	public class ManufacturingTask : ITask
	{
		private readonly IItemFactory _itemFactory;

		public ManufacturingTask(IItemFactory itemFactory)
		{
			_itemFactory = itemFactory;
		}

		#region ErrorCode enum

		public enum ErrorCode
		{
			Success,
			MissingBluePrint,
			MissingAgent,
			MissingSkillRequirement,
			MissingMaterials
		}

		#endregion

		#region ITask Members

		public ITaskResult Execute(ITaskParameters taskParameters)
		{
			var parameters = (ManufacturingParameters) taskParameters;
			return Manufacture(parameters);
		}

		#endregion

		public ManufacturingResult Manufacture(ManufacturingParameters parameters)
		{
			Ensure.That(() => parameters).IsNotNull();

			if (parameters.BluePrint == null)
				return Failed(ErrorCode.MissingBluePrint);

			if (parameters.Agent == null)
				return Failed(ErrorCode.MissingAgent);

			if (!parameters.Agent.CanUse(parameters.BluePrint))
				return Failed(ErrorCode.MissingSkillRequirement);

			var location = parameters.Agent.Location as Manufactory;
			if ( location == null )
				return Failed(ErrorCode.MissingAgent);

			var property = parameters.Agent.Corporation.GetProperty(location);
			if ( !parameters.BluePrint.HasMaterials(property) )
				return Failed(ErrorCode.MissingMaterials);

			var item = parameters.BluePrint.Build(_itemFactory);
			return Success(item);
		}

		private ManufacturingResult Success(Item item)
		{
			return new ManufacturingResult { Item = item };
		}

		private ManufacturingResult Failed(ErrorCode errorCode)
		{
			return new ManufacturingResult(errorCode);
		}
	}
}