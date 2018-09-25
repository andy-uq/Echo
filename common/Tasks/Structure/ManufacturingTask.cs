using System;
using System.Collections.Generic;
using Echo.Agents;
using Echo.Items;
using Echo.State;
using Echo.Structures;

namespace Echo.Tasks.Structure
{
	public class ManufacturingTask : ITask
	{
		#region StatusCode enum

		public enum StatusCode
		{
			Success,
			MissingBluePrint,
			MissingAgent,
			MissingSkillRequirement,
			MissingMaterials,
			Pending
		}

		#endregion

		private readonly IItemFactory _itemFactory;

		private ItemState[] _firstLoad;
		private ItemState[] _subsequentLoad;

		public ulong Id { get; set; }
		public string Name { get; set; }
		public ObjectType ObjectType => ObjectType.Task;

		public BluePrintInfo BluePrint { get; private set; }
		public Agent Agent { get; private set; }
		public uint TimeRemaining { get; private set; }

		public IEnumerable<ItemState> FirstLoad => _firstLoad;

		public IEnumerable<ItemState> SubsequentLoad => _subsequentLoad;

		private ItemCollection Property =>
			Agent?.Corporation == null
				? new ItemCollection()
				: Agent.Corporation.GetProperty(Location);

		private Manufactory Location => Agent.Location as Manufactory;

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
			if (!result.Success) 
				return result;

			UseMaterials();
			TimeRemaining--;

			if (TimeRemaining > 0)
			{
				Location.RegisterTask(this);
				return new ManufacturingResult(StatusCode.Pending) {Success = true, TimeRemaining = TimeRemaining};
			}

			Location.TaskComplete(this);
			var item = BluePrint.Build(_itemFactory);
			return Success(item);
		}

		private void UseMaterials()
		{
			if (TimeRemaining == BluePrint.BuildLength)
			{
				Property.Remove(_firstLoad);
				return;
			}

			Property.Remove(_subsequentLoad);
		}

		public ITaskResult SetParameters(ITaskParameters taskParameters)
		{
			return SetManufacturingParameters(taskParameters as ManufacturingParameters);
		}

		private ManufacturingResult SetManufacturingParameters(ManufacturingParameters parameters)
		{
			if (parameters == null) throw new ArgumentNullException(nameof(parameters));

			BluePrint = parameters.BluePrint;
			Agent = parameters.Agent;

			var result = ValidateParameters();
			if (result.Success)
			{
				TimeRemaining = parameters.BluePrint.BuildLength;
				BuildQuanta(parameters.BluePrint);
			}

			return result;
		}

		private void BuildQuanta(BluePrintInfo bluePrint)
		{
			var firstLoad = new List<ItemState>();
			var subsequentLoads = new List<ItemState>();

			var quantaFactor = (double)TimeRemaining;
			foreach (var item in bluePrint.Materials)
			{
				var quanta = new ItemState {Code = item.Code, Quantity = (uint) Math.Floor(item.Quantity/quantaFactor)};
				subsequentLoads.Add(quanta);

				firstLoad.Add(new ItemState { Code = item.Code, Quantity = item.Quantity - (uint )Math.Floor(quanta.Quantity * (quantaFactor - 1)) } );
			}

			_firstLoad = firstLoad.ToArray();
			_subsequentLoad = subsequentLoads.ToArray();
		}

		private ManufacturingResult ValidateParameters()
		{
			if (BluePrint == null)
				return Failed(StatusCode.MissingBluePrint);

			if (Agent == null)
				return Failed(StatusCode.MissingAgent);

			if (!Agent.CanUse(BluePrint))
				return Failed(StatusCode.MissingSkillRequirement);

			if (Location == null)
				return Failed(StatusCode.MissingAgent);

			if (!BluePrint.HasMaterials(Property))
				return Failed(StatusCode.MissingMaterials);

			return Success();
		}

		private ManufacturingResult Success(Item item = null) => new ManufacturingResult(item);
		private ManufacturingResult Failed(StatusCode statusCode) => new ManufacturingResult(statusCode);
	}
}