using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Echo.Tests
{
	public class TestSettings
	{
		public string BasePath { get; set; }
		private static readonly Dictionary<string, int> _uniqueSeed = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

		private static readonly TestSettings _laptopSettings = new TestSettings
		{
		                                              		BasePath = @"c:\temp"
		                                              	};

		private static readonly TestSettings _settings = _laptopSettings;

		public static readonly object SyncObject = new object();

		private TestSettings()
		{
		}

		public static string UniqueFileName(string filename)
		{
			var fullName = Path.Combine(_settings.BasePath, filename);
			var directory = Path.GetDirectoryName(fullName);
			var name = Path.GetFileNameWithoutExtension(fullName);
			var extension = Path.GetExtension(fullName);

			Debug.Assert(directory != null, "directory != null");

			int num;
			string ToFileName() => Path.Combine(directory, $"{name}_{num:d4}{extension}");

			lock ( SyncObject )
			{
				if (!_uniqueSeed.TryGetValue(filename, out num))
					num = 1;

				while (File.Exists(ToFileName()))
				{
					num++;
				}

				_uniqueSeed[filename] = (num + 1);
			}

			return ToFileName();
		}
	}
}