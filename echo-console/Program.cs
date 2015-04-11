using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Echo;
using Echo.Builder;
using Echo.Builders;
using Echo.Celestial;
using Echo.Engine;
using Echo.State;
using Newtonsoft.Json;

namespace echo_console
{
	internal interface ICommand
	{
	}

	class Program
	{
		private static volatile bool _alive = true;
		private static readonly Queue<ICommand> _commandQueue = new Queue<ICommand>();

		private static IEnumerable<TickRegistrationFactory> Registrations
		{
			get
			{
				yield return u => u
					.StarClusters
					.SelectMany(x => x.SolarSystems)
					.Select(solarSystem => new Orbiter(solarSystem))
					.Select(orbiter => orbiter.TickRegistration);
			}
		}

		static void Main(string[] args)
		{
			var universeFileName = args.FirstOrDefault() ?? "universe.txt";
			UniverseState universe;

			if (File.Exists(universeFileName))
			{
				using (var fs = File.OpenText(universeFileName))
				{
					var reader = new JsonTextReader(fs);
					var serialiser = new JsonSerializer();
					serialiser.Converters.Add(new VectorConverter());
					universe = serialiser.Deserialize<UniverseState>(reader);
				}
			}
			else
			{
				universe = CreateNewUniverse();
			}

			var universeBuilder = Universe.Builder.Build(universe);
			var resolver = new IdResolutionContext(universeBuilder.FlattenObjectTree());

			var game = new Game(universeBuilder.Build(resolver), Registrations);
			var gameThread = new Thread(() => GameThread(game));
			gameThread.Start();

			int? saveSlot = null;
			WriteHelp();
			while (_alive)
			{
				var key = Console.ReadKey(intercept: true);
				switch (key.KeyChar)
				{
					case 'q':
						_alive = false;
						break;
					case 's':
					case 'l':
						if (saveSlot != null)
						{
							if (key.KeyChar == 's')
							{
								RequestSave(saveSlot);
							}
							else
							{
								RequestRewind(saveSlot);
							}
						}
						else
						{
							WritePrompt("Please enter save slot [0-9]");
							break;
						}
						break;
					case '0':
					case '1':
					case '2':
					case '3':
					case '4':
					case '5':
					case '6':
					case '7':
					case '8':
					case '9':
						saveSlot = key.KeyChar - '0';
						break;
				}
			}

			gameThread.Join();
			SaveState(game, universeFileName);
		}

		private static void SaveState(Game game, string universeFileName)
		{
			var state = Universe.Builder.Save(game.Universe);

			using (var fs = File.CreateText(universeFileName))
			{
				var writer = new JsonTextWriter(fs) { Formatting = Formatting.Indented };
				var serialiser = new JsonSerializer();
				serialiser.TypeNameHandling = TypeNameHandling.None;
				serialiser.Converters.Add(new UInt64Converter());
				serialiser.Converters.Add(new VectorConverter());
				serialiser.ConstructorHandling = ConstructorHandling.Default;

				serialiser.Serialize(writer, state);
			}
		}

		private static void RequestRewind(int? saveSlot)
		{
			_commandQueue.Enqueue(new LoadState(saveSlot));
		}

		private static void RequestSave(int? saveSlot)
		{
			_commandQueue.Enqueue(new SaveState(saveSlot));
		}

		private static void GameThread(Game game)
		{
			var frameTimer = Stopwatch.StartNew();
			while (_alive)
			{
				frameTimer.Start();
				while (_commandQueue.Any())
				{
					var cmd = _commandQueue.Dequeue();
				}

				game.Update();

				Console.CursorVisible = false;
				DrawState(game);

				Console.SetCursorPosition(0, 25);
				Console.CursorVisible = true;

				var remaining = Game.TicksPerSlice - frameTimer.ElapsedTicks;
				if (remaining > 0)
				{
					Console.SetCursorPosition(0, 0);
					Console.WriteLine("IDLE: {0:n2}%, Tick: {1:x8}, Sleep: {2}ms", game.IdleTimer.Idle, game.Tick, remaining / TimeSpan.TicksPerMillisecond);
					Thread.Sleep(TimeSpan.FromTicks(remaining));
				}

				frameTimer.Reset();
			}
		}

		private static void DrawState(Game game)
		{
			Console.SetCursorPosition(0, 0);
			Console.WriteLine("IDLE: {0:n2}%, Tick: {1:x8}", game.IdleTimer.Idle, game.Tick);

			DrawSolarSystem(game.Universe.StarClusters.SelectMany(s => s.SolarSystems).SingleOrDefault());
		}

		private static Dictionary<IObject, Vector> _obj = new Dictionary<IObject, Vector>();

		private static void DrawSolarSystem(SolarSystem solarSystem)
		{
			var origin = solarSystem.Position.UniversalCoordinates;
			var scaleX = 40/Units.FromAU(3);
			var scaleY = 10/Units.FromAU(2);
			foreach (var satellite in solarSystem.Satellites.OfType<Planet>())
			{
				var position = satellite.Position.UniversalCoordinates - origin;

				var xOffset = (int )(position.X * scaleX) + 40;
				var yOffset = (int )(position.Y * scaleY) + 15;

				if (xOffset < 0 || xOffset > 79)
					continue;

				if (yOffset < 0 || yOffset > 24)
					continue;

				if (_obj.ContainsKey(satellite))
				{
					var lastPosition = _obj[satellite];
					if (lastPosition.X == xOffset && lastPosition.Y == yOffset)
						continue;

					Console.SetCursorPosition((int )lastPosition.X, (int )lastPosition.Y);
					Console.Write(' ');
				}

				_obj[satellite] = new Vector(xOffset, yOffset);
				Console.SetCursorPosition(xOffset, yOffset);
				Console.Write(satellite.CelestialObjectType == CelestialObjectType.Planet ? '*' : '.');
			}
		}

		private static UniverseState CreateNewUniverse()
		{
			var earth = new CelestialObjectState
			{
				Name = "Earth",
				CelestialObjectType = CelestialObjectType.Planet,
				Mass = Units.SolarMass*1E-6,
				LocalCoordinates = new Vector(Units.FromAU(1), 0),
				Size = 1E-3,
			};

			return Idify(new UniverseState
			{
				StarClusters = new[]
				{
					new StarClusterState
					{
						MarketPlace = new MarketPlaceState
						{
							AuctionLength = Duration.Days(1),
							SettlementDelay = Duration.Hours(2)
						},
						Id = "SC-001",
						Name = "Central",
						SolarSystems = new[]
						{
							new SolarSystemState
							{
								Name = "Sol",
								Satellites = new[]
								{
									earth,
									new CelestialObjectState
									{
										Name = "Moon",
										CelestialObjectType = CelestialObjectType.Moon,
										Mass = Units.SolarMass*3.7E-8,
										LocalCoordinates = new Vector(Units.FromAU(1E-3), 0),
										Size = 1E-9,
										Orbits = earth.ToObjectReference(),
									}
								}
							}
						}
					}
				}
			});
		}

		private static UniverseState Idify(UniverseState universeState)
		{
			ulong id = 1 << 8;
			foreach (var state in universeState.Flatten())
			{
				var type = state.GetType();
				var property = type.GetProperty("ObjectId", typeof (ulong));
				if (property != null && property.CanWrite)
					property.SetValue(state, id++);
			}

			universeState.NextId = id;
			return universeState;
		}

		private static void WritePrompt(string prompt)
		{
			Console.SetCursorPosition(0, 25);
			Console.WriteLine(prompt);
		}

		private static void WriteHelp()
		{
			Console.SetCursorPosition(0, 1);
			Console.WriteLine("[0-9] Set state number");
			Console.WriteLine("S - Save state");
			Console.WriteLine("L - Load state");
			Console.WriteLine("Q - Quit");
		}
	}

	internal class VectorConverter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value == null)
			{
				writer.WriteNull();
			}
			else if (value is Vector)
			{
				var vector = (Vector)value;
				writer.WriteValue(vector.ToString("g"));
			}
			else
			{
				throw new JsonSerializationException("Expected Vector object value");
			}
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null)
			{
				return null;
			}
			
			if (reader.TokenType == JsonToken.String)
			{
				try
				{
					return Vector.Parse((string)reader.Value);
				}
				catch (Exception ex)
				{
					throw new Exception(string.Format("Error parsing vector: {0}", reader.Value), ex);
				}
			}

			throw new Exception(string.Format("Unexpected token or value when parsing version. Token: {0}, Value: {1}", reader.TokenType, reader.Value));
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(Vector);
		}
	}

	public class UInt64Converter : JsonConverter
	{
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			writer.WriteRawValue("0x" + ((ulong )value).ToString("x"));
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			return null;
		}

		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(ulong);
		}
	}

	internal class Duration
	{
		public const long TicksPerSecond = 60;
		public const long TicksPerMinute = 60*TicksPerSecond;
		public const long TicksPerHour = 60*TicksPerMinute;
		public const long TicksPerDay = 24*TicksPerHour;
		public const long TicksPerWeek = 7*TicksPerDay;

		public static long Days(int days)
		{
			return days * TicksPerDay;
		}

		public static long Hours(int hours)
		{
			return hours * TicksPerHour;
		}
	}

	internal class SaveState : ICommand
	{
		public SaveState(int? saveSlot)
		{
		}
	}

	internal class LoadState : ICommand
	{
		public LoadState(int? saveSlot)
		{
		}
	}
}
