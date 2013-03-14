using Echo.Agents;
using Echo.Items;
using Echo.State;
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

			var bluePrintInfo = parameters.BluePrint;
			var agent = parameters.Agent;

			return Manufacture(bluePrintInfo, agent);
		}

		public ManufacturingResult Manufacture(BluePrintInfo bluePrintInfo, Agent agent)
		{
			if (bluePrintInfo == null)
				return Failed(ErrorCode.MissingBluePrint);

			if (agent == null)
				return Failed(ErrorCode.MissingAgent);

			if (!agent.CanUse(bluePrintInfo))
				return Failed(ErrorCode.MissingSkillRequirement);

			var location = agent.Location as Manufactory;
			if (location == null)
				return Failed(ErrorCode.MissingAgent);

			var property = agent.Corporation.GetProperty(location);
			if (!bluePrintInfo.HasMaterials(property))
				return Failed(ErrorCode.MissingMaterials);

			var item = bluePrintInfo.Build(_itemFactory);
			property.Remove(bluePrintInfo.Materials);

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