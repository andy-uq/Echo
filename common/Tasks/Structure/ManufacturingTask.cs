using Echo.Agents;
using Echo.Items;
using Echo.State;
using Echo.Structures;
using EnsureThat;

namespace Echo.Tasks.Structure
{
	public class ManufacturingTask : ITask
	{
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

		private readonly IItemFactory _itemFactory;

		public BluePrintInfo BluePrint { get; private set; }
		public Agent Agent { get; private set; }

		private ItemCollection Property
		{
			get
			{
				if (Agent == null || Agent.Corporation == null)
					return new ItemCollection();

				return Agent.Corporation.GetProperty(Location);
			}
		}

		private Manufactory Location
		{
			get { return Agent.Location as Manufactory; }
		}

		public ManufacturingTask(IItemFactory itemFactory)
		{
			_itemFactory = itemFactory;
		}

		ITaskResult ITask.Execute()
		{
			return Manufacture();
		}

		public ManufacturingResult Manufacture()
		{
			var result = ValidateParameters();
			if (result.Success)
			{
				Property.Remove(BluePrint.Materials);
				var item = BluePrint.Build(_itemFactory);

				return Success(item);
			}

			return result;
		}

		public ITaskResult SetParameters(ITaskParameters taskParameters)
		{
			Ensure.That(() => taskParameters).IsNotNull();

			var parameters = (ManufacturingParameters)taskParameters;
			BluePrint = parameters.BluePrint;
			Agent = parameters.Agent;

			return ValidateParameters();
		}

		private ManufacturingResult ValidateParameters()
		{
			if (BluePrint == null)
				return Failed(ErrorCode.MissingBluePrint);

			if (Agent == null)
				return Failed(ErrorCode.MissingAgent);

			if (!Agent.CanUse(BluePrint))
				return Failed(ErrorCode.MissingSkillRequirement);

			if (Location == null)
				return Failed(ErrorCode.MissingAgent);

			if (!BluePrint.HasMaterials(Property))
				return Failed(ErrorCode.MissingMaterials);

			return Success();
		}

		private ManufacturingResult Success(Item item = null)
		{
			return new ManufacturingResult { Success = true, Item = item };
		}

		private ManufacturingResult Failed(ErrorCode errorCode)
		{
			return new ManufacturingResult(errorCode);
		}
	}
}