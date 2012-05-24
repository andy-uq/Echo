using System;
using System.Diagnostics;
using System.IO;

namespace test
{
	public class TestSettings
	{
		public string BasePath { get; set; }

		private static readonly TestSettings LaptopSettings = new TestSettings()
		                                              	{
		                                              		BasePath = @"c:\temp"
		                                              	};

		private static readonly TestSettings Settings = LaptopSettings;

		private TestSettings()
		{
		}

		public static string UniqueFileName(string filename)
		{
			var fullName = Path.Combine(Settings.BasePath, filename);
			string directory = Path.GetDirectoryName(fullName);
			string name = Path.GetFileNameWithoutExtension(fullName);
			string extension = Path.GetExtension(fullName);

			Debug.Assert(directory != null, "directory != null");

			int num = 1;
			Func<string> fname = () => Path.Combine(directory, string.Format("{0}_{1:d4}{2}", name, num, extension));

			while (File.Exists(fname()))
			{
				num++;
			}

			return fname();
		}
	}
}