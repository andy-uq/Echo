using System.IO;
using Newtonsoft.Json;

namespace Echo.Tests
{
	public static class SerialiseExtensions
	{
		public static string Serialize<T>(this JsonSerializer serialiser, T item)
		{
			var writer = new StringWriter();
			serialiser.Serialize(writer, item);
			return writer.ToString();
		}

		public static T Deserialize<T>(this JsonSerializer serialiser, string json)
		{
			var reader = new StringReader(json);
			JsonReader jsonReader = new JsonTextReader(reader);

			return serialiser.Deserialize<T>(jsonReader);
		}
	}
}